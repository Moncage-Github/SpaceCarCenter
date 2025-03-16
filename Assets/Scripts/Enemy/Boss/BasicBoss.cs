using System;
using UnityEngine;


public class BasicBoss : BossBase
{
    private float _skillCasting;

    //스킬에 사용 변수들
    [SerializeField] private GameObject _dashZone;
    [SerializeField] private CircleCollider2D _dashCollider;
    [SerializeField] private int _dashChargeTime;
    [SerializeField] private int _dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

    protected override void OnSkill()
    {
        int skillCount = SkillList.Count;
        Debug.Log(skillCount);
        int num = UnityEngine.Random.Range(0, skillCount);

        //스킬 랜덤 후 해당 스킬 state에 넣기
        _enemyState = SkillList[num];
        Debug.Log(num + "번 스킬 발동");

        _currentState = State.Skill;

        AttackTimer = AttackCycle;
        SkillTimer = SkillCycle;

        return;
    }

    public override void Init()
    {
        SkillList.Add(new BasicBoss.BossSkillDash(SkillEnd, this));
        SkillList.Add(new BossSkillBulletStorm(SkillEnd));
        SkillList.Add(new BossSkillSummonSquad(SkillEnd));

        _dashCollider.enabled = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(collision);

            Init();
        }
    }


    /// <summary>
    /// 대쉬 스킬
    /// </summary>
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
        public BossSkillDash(Action action, BasicBoss basicBoss)
        {
            _skillEnd = action;
            _currentSkill = SkillState.None;

            _basicBoss = basicBoss;
            
            _dashZoneColor = _basicBoss._dashZone.GetComponent<SpriteRenderer>();

            _bossBase = _basicBoss.GetComponent<EnemyBase>();

            _timer = _basicBoss._dashChargeTime;
        }

        public void Update(EnemyBase enemy)
        {
            if (_currentSkill == SkillState.None) DashInit();
            else if (_currentSkill == SkillState.DashCharge) DashCharge();
            else if (_currentSkill == SkillState.Dash) Dash();
            else if (_currentSkill == SkillState.BackDash) ;

            Debug.Log("보스 스킬 - 대쉬");

        }

        private void DashInit()
        {
            _currentSkill = SkillState.DashCharge;
        }
        private void DashCharge()
        {
            _timer -= Time.deltaTime;
            _basicBoss._dashZone.SetActive(true);

            if (_timer > 0)
            {
                _dashZoneColor.color = Color.Lerp(Color.white, Color.red, 1 - _timer / _basicBoss._dashChargeTime);
                RotateTowardsTarget();
            }
            else
            {
                _targetPos = _basicBoss._target.position;

                _basicBoss._dashZone.SetActive(false);
                _timer = _basicBoss._dashChargeTime;
                _currentSkill = SkillState.Dash;
            }
        }
        private void Dash()
        {
            _dashCollider.enabled = true;

            //false = 
            if (DashTarget())
                return;

            _currentSkill = SkillState.None;
            _skillEnd();
        }

        /// <summary>
        /// 목표 위치로 대쉬
        /// </summary>
        /// <returns> false = 대쉬 종료, true = 대쉬 중</returns>
        private bool DashTarget()
        {
            _basicBoss.transform.position = Vector3.Lerp(_basicBoss.transform.position, _targetPos, Time.deltaTime * _basicBoss._dashSpeed);
            return true;
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
    }
}

/// <summary>
/// 총알 폭풍 스킬
/// </summary>
public class BossSkillBulletStorm: IEnemyState
{
    private Action _skillEnd;
    public BossSkillBulletStorm(Action action)
    {
        _skillEnd = action;
    }

    public void Update(EnemyBase enemy)
    {
        Debug.Log("보스 스킬 - 총알 뿌리기");
        _skillEnd();
    }
}

/// <summary>
/// 쫄병 소환 스킬
/// </summary>
public class BossSkillSummonSquad: IEnemyState
{
    private Action _skillEnd;
    public BossSkillSummonSquad(Action action)
    {
        _skillEnd = action;
    }
    public void Update(EnemyBase enemy)
    {
        Debug.Log("보스 스킬 - 쫄병 소환");
        _skillEnd();
    }
}
