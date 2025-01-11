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
    /// 총알을 쏜 객체와 타깃을 계산해 방향을 구하고 발사해주는 함수.
    /// from : 발사한 객체의 Transform, to : 타겟 객체의 Transform
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
    /// 총알을 발사하는 위치와 쏜 객체가 다를 경우 쏜 객체가 해당 총알에 맞지 않게 설정해줌.
    /// from : 발사할 위치, to : 타겟 객체의 Transform, shooter : 발사한 객체
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
