using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGunEquipment : BaseEquipment
{
    [SerializeField] private float _reloadTime;
    private float _timer;
    private bool _reloading = false;
    [SerializeField] private float _damage;
    [SerializeField] private float _recoilForce;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private GameObject _bulletPrefab;

    private List<Transform> _enemyList = new List<Transform>();

    private void Update()
    {
        if(_reloading)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0 ) 
                _reloading = false;
        }
        if(_enemyList.Count > 0)
        {
            Shooting(GetCloseEnemy());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemyList.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemyList.Remove(collision.transform);
        }
    }

    private void Shooting(Transform target)
    {
        if(RotateTowardsTarget(target.position, 0.01f, _rotationSpeed) == false)
        {
            return;
        }


        if (_reloading)
            return;

        //�Ѿ� ����
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        //�Ѿ� �ʱ�ȭ - (�߻���ġ, Ÿ����ġ, �߻��� ��ü, ������)
        bullet.GetComponent<Bullet>().Init(transform, target, Vehicle.transform, _damage);

        //�ݵ��� ���� ���� ���� �ݴ� ���� ����
        
        Debug.Log(transform.up.normalized.ToString());
        //�� �ݴ� ���� ���Ϳ� �ݵ��� ��� �� ������ �ݵ� ����
        Vehicle.GetComponent<Rigidbody2D>().velocity += -(Vector2)transform.up.normalized * _recoilForce;

        _reloading = true;
        _timer = _reloadTime;
    }

    
    

    private Transform GetCloseEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in _enemyList)
        {
            float distance = Vector3.Distance(Vehicle.transform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

}
