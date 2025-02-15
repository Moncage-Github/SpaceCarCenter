using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEquipment : BaseEquipment
{
    // ������ �浹 ���� (��: Vector3.up�� ���ʸ� �浹)
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
            //�浹�� ��ü�� ���� ����
            Vector2 collisionDirection = contact.normal;

            //�浹�� ���Ϳ� ���� ������ ���� ���ϱ�
            _ignoreDirection = transform.up;
            float angle = Vector2.Angle(collisionDirection, _ignoreDirection);

            Debug.Log(angle);

            //�浹 ����(_collisionRange)���� �浹�� ������ �۴ٸ� �浹ó��
            if (angle < 180 + _collisionRange / 2 && angle > 180 - _collisionRange / 2)
            {
                Debug.Log("� ������" + _damage.ToString());
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
