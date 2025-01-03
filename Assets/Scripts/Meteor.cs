using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MapObject, IDamageable
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _collectablePrefab;

    private float _hp = 100.0f;
    public float HP { get => _hp; private set => _hp = value; }

    public override void Init()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        ObjectType = MapObjectType.METEOR;

        SetMeteorType();
    }

    private void SetMeteorType()
    {
        int rand = Random.Range(0, 100);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : GetComponent ���ֱ�
        if (!collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            return;
        }

        Vector2 impulse = CalculateImpulseVector2D(collision);
        float damage = impulse.magnitude;
        //Debug.Log("Meteor Damage : " + damage);
        damageable.TakeDamage(damage);
    }

    Vector2 CalculateImpulseVector2D(Collision2D collision)
    {
        // �浹 ���� �ӵ� (�浹 �߻� ������ �ӵ�)
        Vector2 preCollisionVelocity = _rigidbody.velocity;

        // �浹 �� �ӵ� (Collision2D.contacts[0].normal�� �̿��� �ݻ�� �ӵ� ���)
        Vector2 postCollisionVelocity = Vector2.Reflect(preCollisionVelocity, collision.contacts[0].normal);

        // �ӵ��� ��ȭ��(��v)
        Vector2 deltaVelocity = postCollisionVelocity - preCollisionVelocity;

        // ��ü�� ����
        float mass = _rigidbody.mass;

        // �浹��(Impulse) = ���� * �ӵ��� ��ȭ��
        Vector2 impulse = mass * deltaVelocity;
        
        return impulse;
    }

    public void TakeDamage(float damage)
    {
        HP = 0;
        if(HP <= 0)
        {
            HP = 0;
            DestoryEvent();
            Destroy(gameObject);
        }
    }

    private void DestoryEvent()
    {
        int rand = Random.Range(1, 4);
        for(int  i = 0; i < rand; i++)
        {
            CollectableItem item = Instantiate(_collectablePrefab).GetComponent<CollectableItem>();
            item.transform.position = transform.position;
            item.Init();
        }
    }
}
