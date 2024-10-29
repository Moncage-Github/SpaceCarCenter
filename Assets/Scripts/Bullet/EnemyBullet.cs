using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _bulletForce;
    


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //TODO: �Լ�ȭ�ؼ� ���ʹ̿��� �÷��̾��� ������ ����� �� ���Ⱚ�� �Ѱ��ֱ�
        _player = GameObject.FindGameObjectWithTag("Player");
        

        Vector3 direction = _player.transform.position - transform.position;
        _rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * _bulletForce;

        float rot = Mathf.Atan2(- direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(10);
            Destroy(gameObject);

        }
    }

}
