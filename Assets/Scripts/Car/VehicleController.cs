using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    // Components
    [SerializeField] private VehicleStat _vehicleStat;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private VehicleUI _vehicleUI;


    private float _accelerationInput = 0;
    private float _steeringInput = 0;

    public float AccelerationInput { set => _accelerationInput = value; }
    public float SteeringInput { set => _steeringInput = value; }

    private float _rotationAngle = 0;
    private float _velocityVsUp = 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _vehicleStat.OnFuelChange += _vehicleUI.ChangeFuelBar;
        _vehicleStat.OnHpChange += _vehicleUI.ChangeHpBar;

        _vehicleStat.CurrentFuelAmount = _vehicleStat.MaxFuelAmount;
        _vehicleStat.CurrentHp = _vehicleStat.MaxHp;
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();

        ApplyFuelUsage();
    }

    private void ApplyEngineForce()
    {
        if (_accelerationInput == 0 || _vehicleStat.IsFuelEmpty())
        {
            _rigidbody2D.drag = Mathf.Lerp(_rigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            _rigidbody2D.drag = 0;
        }

        _velocityVsUp = Vector2.Dot(transform.up, _rigidbody2D.velocity);

        if (_velocityVsUp > _vehicleStat.MaxSpeed && _accelerationInput > 0)
        {
            return;
        }

        if (_velocityVsUp < -_vehicleStat.MaxSpeed * 0.5f && _accelerationInput < 0)
        {
            return;
        }

        if (_rigidbody2D.velocity.sqrMagnitude > _vehicleStat.MaxSpeed * _vehicleStat.MaxSpeed && _accelerationInput > 0)
        {
            return;
        }

        if (!_vehicleStat.IsFuelEmpty())
        {
            Vector2 engineForeceVector = transform.up * _accelerationInput * _vehicleStat.AcclerationForce;
            _rigidbody2D.AddForce(engineForeceVector, ForceMode2D.Force);
        }
    }

    private void ApplySteering()
    {
        float minSpeedBeforeAllowTuring = (_rigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTuring = Mathf.Clamp01(minSpeedBeforeAllowTuring);

        _rotationAngle -= _vehicleStat.RotationForce * _steeringInput * minSpeedBeforeAllowTuring;

        _rigidbody2D.MoveRotation(_rotationAngle);
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = forwardVelocity + rightVelocity * _vehicleStat.DRIFT_FACTOR;
    }

    private void ApplyFuelUsage()
    {
        if(_accelerationInput == 0 || _vehicleStat.IsFuelEmpty())
        {
            return;
        }
        float speedRatio = _rigidbody2D.velocity.magnitude / _vehicleStat.MaxSpeed;
        _vehicleStat.CurrentFuelAmount -= Mathf.Lerp(0, _vehicleStat.FUEL_USE_AMOUNT, speedRatio) * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _vehicleStat.CurrentHp -= 10.0f;
    }
}
