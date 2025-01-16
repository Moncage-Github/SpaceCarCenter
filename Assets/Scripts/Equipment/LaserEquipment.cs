using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEquipment : BaseEquipment
{
    private List<Transform> _enemyList = new List<Transform>();
    private LineRenderer _lineRenderer;

    [SerializeField] private float _reloadTime;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _damageCycle;
    [SerializeField] private float _percentDamage;
    private float _timer;
    private float _damageTimer;
    private bool _reloading = false;
    private bool _attackDuring = false;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_reloading)
        {
           
            if (_timer <= 0)
            {
                _reloading = false;
                _attackDuring = false;
            }
                
        }
        else if (_enemyList.Count > 0)
        {
            Shooting(GetCloseEnemy());

            if(_timer <= 0 && _attackDuring == false)
            {
                _attackDuring = true;
                _timer = _attackDuration;
            }
            else if(_timer <= 0 && _attackDuring == true)
            {
                _reloading = true;
                _timer = _reloadTime;
                _lineRenderer.positionCount = 1;
            }

        }
        else
        {
            _lineRenderer.positionCount = 1;
            return;
        }
    }

    private void Shooting(Transform transform)
    {
        _damageTimer -= Time.deltaTime;

        IDamageable damageable = transform.GetComponent<IDamageable>();
        IGetHp hp = transform.GetComponent<IGetHp>();
        if(damageable != null )
        {
            _lineRenderer.SetPosition(0, Vehicle.transform.position);

            Debug.Log("레이저 공격");
            if (_damageTimer <= 0)
            {
                _damageTimer = _damageCycle;
                //(최대 체력 / 최종 데미지 퍼센트) / (레이저 지속 시간 / 틱 데미지 사이클) = 틱당 데미지
                damageable.TakeDamage((hp.GetMaxHp() / _percentDamage) / (_attackDuration / _damageCycle));
            }
            
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(1, transform.position);
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
