using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEquipment : BaseEquipment
{
    // 가능한 충돌 방향 (예: Vector3.up은 위쪽만 충돌)
    private Vector3 _ignoreDirection;
    [SerializeField] private float _collisionRange;
    [SerializeField] private float _damage;
    [SerializeField] private float _tickCycle;
    private float _timer;
    private bool _cooltime = false;

    private void Update()
    {
        if( _cooltime )
        {
            _timer -= Time.deltaTime;
            if(_timer < 0 )
                _cooltime = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision);

        if (!collision.collider.CompareTag("Meteor") || _cooltime == true)
        {
            return;
        }

        Debug.Log("CollisionEnter Meteor");

        foreach (ContactPoint2D contact in collision.contacts)
        {
            //충돌한 객체의 법선 벡터
            Vector2 collisionDirection = contact.normal;

            //충돌한 벡터와 위쪽 방향의 각도 구하기
            _ignoreDirection = transform.up;
            float angle = Vector2.Angle(collisionDirection, _ignoreDirection);

            Debug.Log(angle);

            //충돌 범위(_collisionRange)보다 충돌한 각도가 작다면 충돌처리
            if (angle < 180 + _collisionRange / 2 && angle > 180 - _collisionRange / 2)
            {
                Debug.Log("운석 데미지" + _damage.ToString());
                IDamageable meteor = collision.collider.GetComponent<IDamageable>();
                if(meteor != null)
                {
                    meteor.TakeDamage(_damage);
                    _cooltime = true;
                    _timer = _tickCycle;
                }
            }
        }
        
    }
}
