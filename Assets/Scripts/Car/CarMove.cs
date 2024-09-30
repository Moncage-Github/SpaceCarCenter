using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMove : MonoBehaviour
{
    //TODO : CarStat으로 분리
    [Header("Car Stat")]
    [SerializeField] [Range(0.0f, 0.999f)] private float _driftFactor;
    [SerializeField] private float _acclerationForce;
    [SerializeField] private float _rotationForce;
    [SerializeField] private float _maxSpeed;

    private float _accelerationInput = 0;
    private float _steeringInput = 0;

    public float AccelerationInput { set => _accelerationInput = value; }
    public float SteeringInput { set => _steeringInput = value; }

    private float _rotationAngle = 0;
    private float _velocityVsUp = 0;
    

    // Components
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }

    private void ApplyEngineForce()
    {
        _velocityVsUp = Vector2.Dot(transform.up, _rigidbody2D.velocity);

        if (_velocityVsUp > _maxSpeed && _accelerationInput > 0)
        {
            return;
        }

        if (_velocityVsUp < -_maxSpeed * 0.5f && _accelerationInput < 0)
        {
            return;
        }

        if (_rigidbody2D.velocity.sqrMagnitude > _maxSpeed * _maxSpeed && _accelerationInput > 0)
        {
            return;
        }

        if (_accelerationInput == 0)
        {
            _rigidbody2D.drag = Mathf.Lerp(_rigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            _rigidbody2D.drag = 0;
        }


        Vector2 engineForeceVector = transform.up * _accelerationInput * _acclerationForce;

        _rigidbody2D.AddForce(engineForeceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        float minSpeedBeforeAllowTuring = (_rigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTuring = Mathf.Clamp01(minSpeedBeforeAllowTuring);

        _rotationAngle -= _rotationForce * _steeringInput * minSpeedBeforeAllowTuring;

        _rigidbody2D.MoveRotation(_rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = forwardVelocity + rightVelocity * _driftFactor;
    }

}
