using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _bulletSpeed;
    private float _bulletDamage;

    private Transform _shooter;

    private float _timer;
    

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _timer = 3.0f;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if( _timer < 0 )
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_shooter != collision.transform)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_bulletDamage);
                Destroy(gameObject);

            }
        }

    }

    /// <summary>
    /// �Ѿ��� �� ��ü�� Ÿ���� ����� ������ ���ϰ� �߻����ִ� �Լ�.
    /// from : �߻��� ��ü�� Transform, to : Ÿ�� ��ü�� Transform, Damage : �Ѿ� ������
    /// </summary>
    public void Init(Transform from, Transform to, float Damage)
    {
        _shooter = from;

        _bulletDamage = Damage;

        Vector3 direction = to.position - from.position;
        _rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * _bulletSpeed;

        float rot = Mathf.Atan2(- direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    /// <summary>
    /// �Ѿ��� �߻��ϴ� ��ġ�� �� ��ü�� �ٸ� ��� �� ��ü�� �ش� �Ѿ˿� ���� �ʰ� ��������.
    /// from : �߻��� ��ġ, to : Ÿ�� ��ü�� Transform, shooter : �߻��� ��ü, Damage : �Ѿ� ������
    /// </summary>
    public void Init(Transform from, Transform to, Transform shooter, float Damage)
    {
        _shooter = shooter;

        _bulletDamage = Damage;

        Vector3 direction = to.position - from.position;
        _rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * _bulletSpeed;

        float rot = Mathf.Atan2(- direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    /// <summary>
    /// �� ��ǥ�� ������ �Ѿ��� �߻��� �� Vector3�� form�� to�� ��������.
    /// from : �߻��� ��ġ, to : Ÿ�� ��ü�� Transform, shooter : �߻��� ��ü, Damage : �Ѿ� ������
    /// </summary>
    public void Init(Vector3 from, Vector3 to, Transform shooter, float Damage)
    {
        _shooter = shooter;

        _bulletDamage = Damage;

        Vector3 direction = to - from;
        _rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * _bulletSpeed;

        float rot = Mathf.Atan2(- direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

}
