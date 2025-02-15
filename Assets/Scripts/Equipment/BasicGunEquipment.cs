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

        //총알 생성
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        //총알 초기화 - (발사위치, 타겟위치, 발사한 객체, 데미지)
        bullet.GetComponent<Bullet>().Init(transform, target, Vehicle.transform, _damage);

        //반동을 위한 현재 총의 반대 방향 벡터
        
        Debug.Log(transform.up.normalized.ToString());
        //총 반대 방향 벡터와 반동량 계산 후 차량에 반동 적용
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
