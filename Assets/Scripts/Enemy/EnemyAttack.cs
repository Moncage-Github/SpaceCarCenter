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
            // ȸ���� �Ϸ�Ǹ� �̵� ����
            if (Quaternion.Angle(_enemy.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget() - 90)) < 0.3f)
            {
                _timer = 0;
                _enemy.BulletShooting();
            }
        }
    }

    
    private void RotateTowardsTarget()
    {
        // ��ǥ ��ġ������ ���� ���
        Vector3 direction = _player.position - _enemy.transform.position;
        direction.z = 0f; // 2D ȯ���̹Ƿ� Z���� ȸ���� 0���� ����
        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z�� ȸ�� ���� ���

        // ��ǥ ȸ�� ����
        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation - 90); // Quaternion���� ��ȯ

        // ���� ȸ�� �������� ��ǥ ȸ�� ������ �ε巴�� ȸ��
        float angleDifference = Mathf.DeltaAngle(_enemy.transform.eulerAngles.z, zRotation - 90);
        float rotationAmount = Mathf.Clamp(angleDifference, -_enemy.RotationSpeed * 8 * Time.deltaTime, _enemy.RotationSpeed * 8 * Time.deltaTime);

        // ȸ�� ����
        _rigidbody2D.MoveRotation(_rigidbody2D.rotation + rotationAmount);
    }



    private float GetAngleToTarget()
    {
        // ��ǥ ��ġ������ ���� ���
        Vector3 direction = _player.position - _enemy.transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Z�� ȸ�� ���� ��ȯ
    }
}
