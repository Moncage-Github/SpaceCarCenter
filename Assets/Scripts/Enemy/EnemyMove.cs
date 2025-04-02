using UnityEngine;

public class EnemyMove : IEnemyState
{
    EnemyBase _enemy;
    private Rigidbody2D _rigidbody2D;
    private Quaternion _targetRotation; //목표 각도
    protected Vector3 TargetPosition; // 목표 위치
    private Vector3 _startPosition; // 목표 위치
    private float _startRotation; // 목표 각도
    private bool _isMoving = false;  // 이동 중인지 여부
    private bool _isTurn = false;  // 이동 중인지 여부

    public EnemyMove(EnemyBase enemy)
    {
        _enemy = enemy;
        _startPosition = _enemy.transform.position;     // 에너미의 초기 위치를 기준점으로 설정
        _startRotation = _enemy.transform.rotation.eulerAngles.z;
        _rigidbody2D = _enemy.GetComponent<Rigidbody2D>();
        SetRandomTargetPosition();                      // 처음 목표 위치 설정
    }

    // Update is called once per frame
    public virtual void Update(EnemyBase enemy)
    {
        if (_isTurn)
        {
            RotateTowardsTarget();

            // 회전이 완료되면 이동 시작
            if (Quaternion.Angle(_enemy.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget() - 90)) < 0.1f)
            {
                _isTurn = false; // 회전 종료
                _isMoving = true; // 이동 상태로 전환
            }
        }

        if (_isMoving)
        {
            MoveTowardsTarget();
        }
    }


    protected virtual void SetRandomTargetPosition()
    {
        // 반지름 내에서 랜덤한 각도
        float randomAngle = Random.Range(0f, Mathf.PI * _enemy.MovementRadius);

        // 각도에 따라 x, y 좌표 계산
        float randomX = Mathf.Cos(randomAngle) * _enemy.MovementRadius;
        float randomY = Mathf.Sin(randomAngle) * _enemy.MovementRadius;

        // 기준 위치에서 랜덤 좌표 설정
        TargetPosition = _enemy.transform.position + new Vector3(randomX, randomY, 0);

        _isTurn = true; // 회전 시작
    }


    protected void RotateTowardsTarget()
    {
        // 목표 위치까지의 방향 계산
        Vector3 direction = TargetPosition - _enemy.transform.position;
        direction.z = 0f; // 2D 환경이므로 Z축의 회전은 0으로 설정
        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 계산

        // 목표 회전 각도
        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation - 90); // Quaternion으로 변환

        // 현재 회전 각도에서 목표 회전 각도로 부드럽게 회전
        float angleDifference = Mathf.DeltaAngle(_enemy.transform.eulerAngles.z, zRotation - 90);
        float rotationAmount = Mathf.Clamp(angleDifference, -_enemy.RotationSpeed * 3 * Time.deltaTime, _enemy.RotationSpeed * 3 * Time.deltaTime);

        // 회전 적용
        _rigidbody2D.MoveRotation(_rigidbody2D.rotation + rotationAmount);
    }

    protected virtual void MoveTowardsTarget()
    {
        Vector2 targetPosition2D = new Vector2(TargetPosition.x, TargetPosition.y);

        float distance = Vector2.Distance(_rigidbody2D.position, targetPosition2D);

        if (distance > 0.5f)
        {
            Vector2 direction = (targetPosition2D - _rigidbody2D.position).normalized;
            _rigidbody2D.velocity = direction * _enemy.MoveSpeed;
        }
        else
        {
            //_rigidbody2D.velocity = Vector2.zero;
            _isMoving = false; // 이동 종료
            SetRandomTargetPosition(); // 새로운 랜덤 위치 설정
        }

        return;
    }

    protected float GetAngleToTarget()
    {
        // 목표 위치까지의 방향 계산
        Vector3 direction = TargetPosition - _enemy.transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z축 회전 각도 반환
    }
}
