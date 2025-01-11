using UnityEngine;

public class EnemyAttack : IEnemyState
{
    private EnemyBase _enemy;
    private GameObject _bullet;
    private Transform _player;
    private Rigidbody2D _rigidbody2D;

    private float _timer;
    public float Timer { set => _timer = value; }
    public EnemyAttack(EnemyBase enemy, GameObject bullet)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidbody2D = enemy.GetComponent<Rigidbody2D>();
        _enemy = enemy;
        _bullet = bullet;
        _timer = 0;
    }

    // Update is called once per frame
    public void Update(EnemyBase enemy)
    {
        RotateTowardsTarget();
        //Debug.Log(_timer);
        if (_timer < _enemy.AttackCycle)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            // 회전이 완료되면 총알 발사
            if (Quaternion.Angle(_enemy.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget() - 90)) < 0.3f)
            {
                _timer = 0;
                _enemy.BulletShooting();
            }
        }
    }

    
    private void RotateTowardsTarget()
    {
        // 목표 위치까지의 방향 계산
        Vector3 direction = _player.position - _enemy.transform.position;
        direction.z = 0f; // 2D 환경이므로 Z축의 회전은 0으로 설정
        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 계산

        // 목표 회전 각도
        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation - 90); // Quaternion으로 변환

        // 현재 회전 각도에서 목표 회전 각도로 부드럽게 회전
        float angleDifference = Mathf.DeltaAngle(_enemy.transform.eulerAngles.z, zRotation - 90);
        float rotationAmount = Mathf.Clamp(angleDifference, -_enemy.RotationSpeed * 8 * Time.deltaTime, _enemy.RotationSpeed * 8 * Time.deltaTime);

        // 회전 적용
        _rigidbody2D.MoveRotation(_rigidbody2D.rotation + rotationAmount);
    }



    private float GetAngleToTarget()
    {
        // 목표 위치까지의 방향 계산
        Vector3 direction = _player.position - _enemy.transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 반환
    }
}
