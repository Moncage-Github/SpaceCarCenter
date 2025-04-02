using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BasicBoss : BossBase
{
    private bool _isInit = false;

    [Space]

    //돌진 스킬에 사용 변수들
    [Header("Dash Skill Settings")]
    [SerializeField] private GameObject _dashZone;
    [SerializeField] private CircleCollider2D _dashCollider;
    [SerializeField] private int _dashDistance;
    [SerializeField] private float _dashChargeTime;
    [SerializeField] private int _dashSpeed;
    [SerializeField] private int _dashPower;
    [SerializeField] private int _dashDamage;

    [Space]

    //총알 폭풍 스킬에 사용 변수들
    [Header("Bullet Storm Skill Settings")]
    [SerializeField] private GameObject _stormBullet;
    [SerializeField] private float _stormBulletSpeed;
    [SerializeField] private float _stormBulletDamage;
    [SerializeField] private int _bulletsPerRotation;   //한바퀴 회전하는 동안 쏠 횟수
    [SerializeField] private int _rotationCount;        //몇 바퀴 회전할지
    [SerializeField] private float _fireDelay;          //발사 딜레이

    [Space]

    //십자 총알 스킬에 사용 변수들
    [Header("Quad Shot Skill Settings")]
    [SerializeField] private GameObject _quadBullet;
    [SerializeField] private float _quadBulletSpeed;
    [SerializeField] private float _quadBulletDamage;

    [Space]

    //쫄병 소환 스킬에 사용 변수들
    [Header("Summon Squad Skill Settings")]
    [SerializeField] private GameObject _squadPrefab;
    [SerializeField] private int _summonCount;
    [SerializeField] private float _summonRadius;



    public List<GameObject> Squad = new List<GameObject>();
    private IEnemyState _squadSkill;

    
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

    protected override void OnSkill()
    {
        if(Squad.Count == 0)
        {
            _enemyState = _squadSkill;
        }
        else
        {
            int skillCount = SkillList.Count;
            Debug.Log(skillCount);
            int num = UnityEngine.Random.Range(0, skillCount);

            //스킬 랜덤 후 해당 스킬 state에 넣기
            _enemyState = SkillList[num];
            Debug.Log(num + "번 스킬 발동");
        }

        //if (_enemyState is ISkillInit skillUser)
        //{
        //    skillUser.Init();
        //}

        _currentState = State.Skill;

        AttackTimer = AttackCycle;
        SkillTimer = SkillCycle;

        return;
    }

    public override void Init()
    {
        _dashZone.transform.localScale = new Vector3(1, _dashDistance, 1);
        _dashZone.transform.localPosition = new Vector3(0, _dashDistance / 2, 0);

        SkillList.Add(new BasicBoss.BossSkillDash(SkillEnd, this));
        SkillList.Add(new BossSkillBulletStorm(SkillEnd, this));
        SkillList.Add(new BossSkillQuadShot(SkillEnd, this));


        _squadSkill = new BossSkillSummonSquad(SkillEnd, this);

        _dashCollider.enabled = false;
        _isInit = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(collision);

            if(!_isInit)
                Init();
        }
    }

    internal Transform GetTarget()
    {
        return _target;
    }


    /// <summary>
    /// 대쉬 스킬
    /// </summary>
    //TODO:: 돌아가는거 구현해야함

    public class BossSkillDash : IEnemyState
    {
        public enum SkillState
        {
            None,
            DashCharge,
            Dash,
            BackDash
        }

        private Action _skillEnd;
        private SkillState _currentSkill;
        private BasicBoss _basicBoss;

        private SpriteRenderer _dashZoneColor;

        private float _timer;
        private EnemyBase _bossBase;
        private CircleCollider2D _dashCollider;

        private Vector3 _targetPos;

        private Vehicle _player;
        public BossSkillDash(Action action, BasicBoss basicBoss)
        {
            _skillEnd = action;
            

            _basicBoss = basicBoss;
            
            _dashZoneColor = _basicBoss._dashZone.GetComponent<SpriteRenderer>();
            _dashCollider = _basicBoss._dashCollider;

            _bossBase = _basicBoss.GetComponent<EnemyBase>();

            _player = _basicBoss._target.GetComponent<Vehicle>();
        }

        public void Update(EnemyBase enemy)
        {
            if (_currentSkill == SkillState.None) DashInit();
            else if (_currentSkill == SkillState.DashCharge) DashCharge();
            else if (_currentSkill == SkillState.Dash) Dash();
            else if (_currentSkill == SkillState.BackDash) BackDash();

            Debug.Log("보스 스킬 - 대쉬");

        }

        private void DashInit()
        {
            _timer = _basicBoss._dashChargeTime;

            RotateTowardsTarget();

            //플레이어 방향으로 바라보기
            // 회전이 완료되면 대쉬 준비
            if (Quaternion.Angle(_basicBoss.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget() - 90)) < 5.0f)
            {
                _currentSkill = SkillState.DashCharge;
                _targetPos = _basicBoss.transform.position + (_player.transform.position - _basicBoss.transform.position).normalized * _basicBoss._dashDistance;
            }
        }
        private void DashCharge()
        {
            _timer -= Time.deltaTime;
            _basicBoss._dashZone.SetActive(true);

            if (_timer > 0)
            {
                _dashZoneColor.color = Color.Lerp(Color.white, Color.red, 1 - _timer / _basicBoss._dashChargeTime);
            }
            else
            {
                _basicBoss._dashZone.SetActive(false);
                _timer = _basicBoss._dashChargeTime;
                _currentSkill = SkillState.Dash;
            }
        }
        private void Dash()
        {
            _dashCollider.enabled = true;

            ColliderPlayer();

            //false = 
            if (DashTarget())
                return;

            _currentSkill = SkillState.BackDash;
        }
        private void BackDash()
        {
            _currentSkill = SkillState.None;

            _dashCollider.enabled = false;

            _skillEnd();
        }



        /// <summary>
        /// 목표 위치로 대쉬
        /// </summary>
        /// <returns> false = 대쉬 종료, true = 대쉬 중</returns>
        private bool DashTarget()
        {
            // MoveTowards를 사용해 일정 거리만 이동
            _basicBoss.transform.position = Vector3.MoveTowards(_basicBoss.transform.position, _targetPos, Time.deltaTime * _basicBoss._dashSpeed
            );

            // 목표 위치에 도달했는지 `Vector3.Distance`를 사용하여 확인
            if (Vector3.Distance(_basicBoss.transform.position, _targetPos) < 0.1f)
            {
                return false; // 대쉬 종료
            }

            return true; // 아직 대쉬 중
        }


        protected void RotateTowardsTarget()
        {
            // 목표 위치까지의 방향 계산
            Vector3 direction = _basicBoss._target.position - _basicBoss.transform.position;
            direction.z = 0f; // 2D 환경이므로 Z축의 회전은 0으로 설정
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 계산

            // 목표 회전 각도
            Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation - 90); // Quaternion으로 변환

            // 현재 회전 각도에서 목표 회전 각도로 부드럽게 회전
            float angleDifference = Mathf.DeltaAngle(_basicBoss.transform.eulerAngles.z, zRotation - 90);
            float rotationAmount = Mathf.Clamp(angleDifference, -_bossBase.RotationSpeed * 8 * Time.deltaTime, _bossBase.RotationSpeed * 8 * Time.deltaTime);

            // 회전 적용
            _basicBoss.EnemyRigidbody2D.MoveRotation(_basicBoss.EnemyRigidbody2D.rotation + rotationAmount);
        }

        protected float GetAngleToTarget()
        {
            // 목표 위치까지의 방향 계산
            Vector3 direction = _player.transform.position - _basicBoss.transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 반환
        }

        protected void ColliderPlayer()
        {
            if (_player == null)
                Debug.Log("플레이어 찾지 못함");

            if(_basicBoss._dashCollider.IsTouching(_player.BoxCollider2D))
            {
                Vector2 direction = new Vector2(_player.transform.position.x - _basicBoss.transform.position.x, _player.transform.position.y - _basicBoss.transform.position.y);

                ForceObject(_player.Rigidbody2D, direction.normalized);

                _player.TakeDamage(_basicBoss._dashDamage);

                _currentSkill = SkillState.None;
                _skillEnd();
            }
        }

        protected void ForceObject(Rigidbody2D rigid, Vector2 direction)
        {
            rigid.AddForce(direction * _basicBoss._dashPower, ForceMode2D.Impulse);
        }

    }

    /// <summary>
    /// 총알 폭풍 스킬
    /// </summary>
    public class BossSkillBulletStorm : IEnemyState
    {
        private Action _skillEnd;
        private BasicBoss _basicBoss;

        private EnemyBase _bossBase;

        private GameObject _stormBullet;
        private int _currentRotationCount;  //현재 회전 횟수
        private int _currentAngleCount;     //현재 각도 전환 횟수
        private float _stepAngle;           //회전 중 필요한 각도 전환 횟수
        private float _targetAngle;         //지금 발사할 각도
        private Vector3 _direction;
        private Vector3 _targetPos;
        private Transform _bossTrans;

        private bool _isInit;
        public BossSkillBulletStorm(Action action, BasicBoss basicBoss)
        {
            _skillEnd = action;
            _basicBoss = basicBoss;
            _bossBase = _basicBoss.GetComponent<EnemyBase>();

            _stormBullet = _basicBoss._stormBullet;

            _currentRotationCount = 0;
            _currentAngleCount = 0;
            _targetAngle = 0;
            _stepAngle = 360 / _basicBoss._bulletsPerRotation;

            _isInit = false;
        }

        public void Update(EnemyBase enemy)
        {
            if(!_isInit)
            {
                Init();
                _isInit = true;
            }

            if(_currentRotationCount < _basicBoss._rotationCount)
            {
                _direction = Quaternion.AngleAxis(_stepAngle * _currentAngleCount, Vector3.forward) * (_targetPos - _bossTrans.position).normalized;

                RotateTowardsTarget(_bossTrans.position + _direction);

                if (Quaternion.Angle(_basicBoss.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget(_bossTrans.position + _direction) - 90)) < 5f)
                {
                    // 총알을 생성하고 초기화
                    Bullet bullet = Instantiate(_basicBoss._stormBullet, _bossTrans.position + _direction, Quaternion.identity).GetComponent<Bullet>();

                    bullet.SetSpeed(_basicBoss._stormBulletSpeed);

                    bullet.Init(_bossTrans.position, _bossTrans.position + _direction, _bossTrans, _basicBoss._stormBulletDamage);

                    _currentAngleCount++;
                }


                if (360 / _stepAngle == _currentAngleCount)
                {
                    _currentRotationCount++;
                    _currentAngleCount = 0;
                }
            }
            else
            {
                _isInit = false;

                _skillEnd();
            }
            
            Debug.Log("보스 스킬 - 총알 뿌리기");

        }

        public void Init()
        {
            _currentRotationCount = 0;
            _currentAngleCount = 0;
            _targetAngle = 0;
            _targetPos = _basicBoss._target.position;
            _bossTrans = _basicBoss.transform;
        }

        protected void RotateTowardsTarget(Vector3 to)
        {
            // 목표 위치까지의 방향 계산
            Vector3 direction = to - _bossTrans.position;
            direction.z = 0f; // 2D 환경이므로 Z축의 회전은 0으로 설정
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 계산

            // 목표 회전 각도
            Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation - 90); // Quaternion으로 변환

            // 현재 회전 각도에서 목표 회전 각도로 부드럽게 회전
            float angleDifference = Mathf.DeltaAngle(_basicBoss.transform.eulerAngles.z, zRotation - 90);
            float rotationAmount = Mathf.Clamp(angleDifference, -_bossBase.RotationSpeed * 4 * Time.deltaTime, _bossBase.RotationSpeed * 4 * Time.deltaTime);

            // 회전 적용
            _basicBoss.EnemyRigidbody2D.MoveRotation(_basicBoss.EnemyRigidbody2D.rotation + rotationAmount);
        }

        protected float GetAngleToTarget(Vector3 to)
        {
            // 목표 위치까지의 방향 계산
            Vector3 direction = to - _basicBoss.transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 반환
        }
    }

    /// <summary>
    /// 네방향 총알 발사 스킬
    /// </summary>
    private class BossSkillQuadShot : IEnemyState
    {
        private Action _skillEnd;
        private BasicBoss _basicBoss;

        private GameObject _quadBullet;
        private Vector3[] _direction = new Vector3[4];
        private Vector3[] _fireDirection = new Vector3[4];
        public BossSkillQuadShot(Action action, BasicBoss basicBoss)
        {
            _skillEnd = action;
            _basicBoss = basicBoss;

            _quadBullet = _basicBoss._quadBullet;
            _direction[0] = Vector3.up;
            _direction[1] = Vector3.down;
            _direction[2] = Vector3.left;
            _direction[3] = Vector3.right;
        }

        public void Update(EnemyBase enemy)
        {
            Vector3 direction = (_basicBoss._target.position - _basicBoss.transform.position).normalized;

            for(int i = 0; i < 4; i++)
            {
                //동서남북으로 총알 발사
                //_fireDirection[i] = new Vector3(_direction[i].x * direction.x, _direction[i].y * direction.y, 0);

                //Bullet bullet = _basicBoss.SpawnObject(_quadBullet, _basicBoss.transform.position).GetComponent<Bullet>();

                //보는 방향을 기준으로 동서남북으로 총알 발사
                _fireDirection[i] = Quaternion.FromToRotation(Vector3.up, direction) * _direction[i];

                // 총알 생성 및 초기화
                Bullet bullet = Instantiate(_quadBullet, _basicBoss.transform.position, Quaternion.identity).GetComponent<Bullet>();

                bullet.SetSpeed(_basicBoss._quadBulletSpeed);

                bullet.Init(_basicBoss.transform.position, _basicBoss.transform.position + _fireDirection[i] * 100, _basicBoss.transform, _basicBoss._quadBulletDamage, 0);

                Debug.Log("방향 벡터 i 값 : " + _fireDirection[i].x  + "," + _fireDirection[i].y + "," + _fireDirection[i].z);
            }
            

            Debug.Log("보스 스킬 - 십자 총알 발사");
            _skillEnd();
        }
    }

    /// <summary>
    /// 쫄병 소환 스킬
    /// </summary>
    public class BossSkillSummonSquad : IEnemyState
    {
        private Action _skillEnd;
        private BasicBoss _basicBoss;

        private List<GameObject> _squad;
        private int _summonCount;

        private float _stepAngle;
        private int _currentAngleCount;

        private bool _isInit;

        private Vector3 _direction;
        public BossSkillSummonSquad(Action action, BasicBoss basicBoss)
        {
            _skillEnd = action;
            _basicBoss = basicBoss;

            _squad = _basicBoss.Squad;
            _summonCount = _basicBoss._summonCount;

            _stepAngle = 360 / _basicBoss._summonCount;
            _isInit = false;
        }

        public void Update(EnemyBase enemy)
        {
            if( !_isInit )
            {
                Init();
                _isInit = true;
            }

            if(_squad.Count < _summonCount)
            {
                _direction = (_basicBoss.transform.position - _basicBoss._target.transform.position).normalized;




                EnemyBase summonSquad = Instantiate(_basicBoss._squadPrefab, _basicBoss.transform.localPosition + _direction * 30 + SetRandomTargetPosition(), Quaternion.identity).GetComponent<EnemyBase>();

                (summonSquad as SummonEnemyBase).SummonInit(_basicBoss);

                _currentAngleCount++;
            }
            else
            {

                _skillEnd();

            }

            Debug.Log("보스 스킬 - 쫄병 소환");
        }

        public void Init()
        {
            _currentAngleCount = 0;
        }

        protected Vector3 SetRandomTargetPosition()
        {
            // 반지름 내에서 랜덤한 각도
            float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * _basicBoss._summonRadius);

            // 각도에 따라 x, y 좌표 계산
            float randomX = Mathf.Cos(randomAngle) * _basicBoss._summonRadius;
            float randomY = Mathf.Sin(randomAngle) * _basicBoss._summonRadius;

            // 기준 위치에서 랜덤 좌표 설정
            return new Vector3(randomX, randomY, 0);
            //return new Vector3(randomX, 0, 0);
        }
    }


    

}
