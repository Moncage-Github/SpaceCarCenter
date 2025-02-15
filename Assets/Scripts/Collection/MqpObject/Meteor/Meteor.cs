using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum MeteorType
{
    None = 0,
    Meteor1 = 1,
    Meteor2 = 2, 
    Meteor3 = 4, 
    Meteor4 = 8, 
    Meteor5 = 16, 
}

public class Meteor : MapObject, IDamageable
{
    private MeteorData _data;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _collectablePrefab;

    public float HP { get; set; }

    public void Init(MeteorType type)
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        ObjectType = MapObjectType.METEOR;

        _data = CollectionAssetManager.Instance.MeteorDatas[type];

        gameObject.name = type.ToString();

        HP = _data.Hp;

        _rigidbody.mass = _data.Gravity;

        GetComponent<SpriteRenderer>().sprite = CollectionAssetManager.Instance.GetSpriteWithName("Meteor1_01");
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
        //Debug.Log("Meteor Damage : " + damage);
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
        HP -= damage;
        Debug.Log(HP.ToString());
        if(HP <= 0)
        {
            HP = 0;
            DestoryEvent();
            Destroy(gameObject);
        }
    }

    private void DestoryEvent()
    {
        CollectionManager.Instance.DestroyedMeteorCount++;

        int rand = Random.Range(_data.ItemDropMin, _data.ItemDropMax);
        for(int  i = 0; i < rand; i++)
        {
            CollectableItem item = Instantiate(_collectablePrefab).GetComponent<CollectableItem>();
            item.transform.position = transform.position;
            item.transform.parent = transform.parent;

            item.Init((CollectableItemType)Mathf.Pow(2, Random.Range(0, 8)));

            item.MoveInRandomDirection();
        }
    }

    public void MoveInRandomDirection()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float moveSpeed = Random.Range(0.3f, 1.0f);

        _rigidbody.velocity = randomDirection * moveSpeed;
    }
}
