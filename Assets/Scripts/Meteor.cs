using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MapObject, IDamageable
{
    [SerializeField] private Rigidbody2D _rigidbody;

    public override void Init()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        ObjectType = MapObjectType.METEOR;




    }

    private void SetMeteorType()
    {
        int rand = Random.Range(0, 100);

        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : GetComponent 없애기
        if (!collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            return;
        }

        Vector2 impulse = CalculateImpulseVector2D(collision);
        float damage = impulse.magnitude;
        Debug.Log("Meteor Damage : " + damage);
        damageable.TakeDamage(damage);
    }

    Vector2 CalculateImpulseVector2D(Collision2D collision)
    {
        // 충돌 직전 속도 (충돌 발생 순간의 속도)
        Vector2 preCollisionVelocity = _rigidbody.velocity;

        // 충돌 후 속도 (Collision2D.contacts[0].normal을 이용해 반사된 속도 계산)
        Vector2 postCollisionVelocity = Vector2.Reflect(preCollisionVelocity, collision.contacts[0].normal);

        // 속도의 변화량(Δv)
        Vector2 deltaVelocity = postCollisionVelocity - preCollisionVelocity;

        // 물체의 질량
        float mass = _rigidbody.mass;

        // 충돌량(Impulse) = 질량 * 속도의 변화량
        Vector2 impulse = mass * deltaVelocity;
        
        return impulse;
    }

    public void TakeDamage(float damage)
    {
        //Destroy(gameObject);
    }
}
