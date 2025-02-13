using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGunEquipment : BaseEquipment
{
    [SerializeField] private float _reloadTime;
    private float _timer;
    private bool _reloading = false;
    [SerializeField] private float _damage;

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
        else if(_enemyList.Count > 0)
        {
            Shooting(GetCloseEnemy());
            _reloading = true;
            _timer = _reloadTime;
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
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(transform, target, Vehicle.transform, _damage);
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
