using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _bulletForce;
    [SerializeField] private float _bulletDamage;

    private Transform _shooter;

    

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
    /// from : �߻��� ��ü�� Transform, to : Ÿ�� ��ü�� Transform
    /// </summary>
    public void Init(Transform from, Transform to)
    {
        _shooter = from;

        Vector3 direction = to.position - from.position;
        _rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * _bulletForce;

        float rot = Mathf.Atan2(- direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    /// <summary>
    /// �Ѿ��� �߻��ϴ� ��ġ�� �� ��ü�� �ٸ� ��� �� ��ü�� �ش� �Ѿ˿� ���� �ʰ� ��������.
    /// from : �߻��� ��ġ, to : Ÿ�� ��ü�� Transform, shooter : �߻��� ��ü
    /// </summary>
    public void Init(Transform from, Transform to, Transform shooter)
    {
        _shooter = shooter;

        Vector3 direction = to.position - from.position;
        _rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * _bulletForce;

        float rot = Mathf.Atan2(- direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

}
