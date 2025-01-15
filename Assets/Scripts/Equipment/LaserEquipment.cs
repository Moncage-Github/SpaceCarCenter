using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEquipment : BaseEquipment
{
    private List<Transform> _enemyList = new List<Transform>();
    private LineRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyList.Count > 0)
        {
            Shooting(GetCloseEnemy());

        }
        else
        {
            _lineRenderer.positionCount = 1;
        }
    }

    private void Shooting(Transform transform)
    {
        IDamageable damageable = transform.GetComponent<IDamageable>();
        if(damageable != null )
        {
            _lineRenderer.SetPosition(0, Vehicle.transform.position);
            Debug.Log("������ ����");
            //TODO:: ���� ü���� �������� IDamageable�� �����ؾߵɰ� ������
            //damageable.TakeDamage();
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
