using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoostEquipment : BaseEquipment
{
    private Rigidbody2D _rigidbody;
    private Vector3 _direction;
    private Action _scheduledPhysicsAction;

    [SerializeField] private float _boostDuration;
    private float _timer = 0.0f;
    private bool _boostTimer = false;

    [SerializeField] private float _boostForce;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = transform.parent.GetComponent<Rigidbody2D>();

        _timer = _boostDuration;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Boost on");
            _scheduledPhysicsAction += Boosting;
        }

        if (_boostTimer == true)
        {
            _timer -= Time.deltaTime;
            if(_timer < 0)
            {
                Vehicle.IsBoost = false;
                _boostTimer = false;
                Debug.Log("boost off");
            }
            
        }
            
    }

    void FixedUpdate() 
    {
        _scheduledPhysicsAction?.Invoke();
        _scheduledPhysicsAction = null;
    }

    private void Boosting()
    {
        _boostTimer = true;
        _timer = _boostDuration;

        Vehicle.IsBoost = true;
        _direction = transform.up;
        _rigidbody.AddForce(_direction * _boostForce, ForceMode2D.Impulse);
    }
}
