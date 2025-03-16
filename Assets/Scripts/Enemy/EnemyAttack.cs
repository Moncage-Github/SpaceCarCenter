using UnityEngine;

public class EnemyAttack : IEnemyState
{
    protected EnemyBase Enemy;
    private Transform _player;
    private Rigidbody2D _rigidbody2D;

    private float _timer;
    public float Timer { set => _timer = value; }
    public EnemyAttack(EnemyBase enemy)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidbody2D = enemy.GetComponent<Rigidbody2D>();
        Enemy = enemy;
        _timer = 0;
    }

    // Update is called once per frame
    public virtual void Update(EnemyBase enemy)
    {
        RotateTowardsTarget();
        //Debug.Log(_timer);
        if (_timer < Enemy.AttackCycle)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            // 회전이 완료되면 총알 발사
            if (Quaternion.Angle(Enemy.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget() - 90)) < 1.0f)
            {
                _timer = 0;
                Enemy.BulletShooting();
            }
        }
    }


    protected void RotateTowardsTarget()
    {
        // 목표 위치까지의 방향 계산
        Vector3 direction = _player.position - Enemy.transform.position;
        direction.z = 0f; // 2D 환경이므로 Z축의 회전은 0으로 설정
        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 계산

        // 목표 회전 각도
        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation - 90); // Quaternion으로 변환

        // 현재 회전 각도에서 목표 회전 각도로 부드럽게 회전
        float angleDifference = Mathf.DeltaAngle(Enemy.transform.eulerAngles.z, zRotation - 90);
        float rotationAmount = Mathf.Clamp(angleDifference, -Enemy.RotationSpeed * 8 * Time.deltaTime, Enemy.RotationSpeed * 8 * Time.deltaTime);

        // 회전 적용
        _rigidbody2D.MoveRotation(_rigidbody2D.rotation + rotationAmount);
    }



    protected float GetAngleToTarget()
    {
        // 목표 위치까지의 방향 계산
        Vector3 direction = _player.position - Enemy.transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 반환
    }
}
