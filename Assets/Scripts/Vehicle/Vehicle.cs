using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour, IDamageable
{
    // Components
    [SerializeField] private VehicleUI _vehicleUI;
    private Rigidbody2D _rigidbody2D;

    private VehicleStat _stat;

    // User Input Value
    private float _accelerationInput = 0;
    private float _steeringInput = 0;
    public float AccelerationInput { set => _accelerationInput = value; }
    public float SteeringInput { set => _steeringInput = value; }

    // Member Variable
    private float _rotationAngle = 0;
    private float _velocityVsUp = 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        var data = DataManager.Instance.GetVehicleData("Truck");
        _stat = new VehicleStat(data);
    }

    private void OnEnable()
    {
        _stat.OnFuelChange += _vehicleUI.ChangeFuelBar;
        _stat.OnHpChange += _vehicleUI.ChangeHpBar;
    }

    private void OnDisable()
    {
        _stat.OnFuelChange -= _vehicleUI.ChangeFuelBar;
        _stat.OnHpChange -= _vehicleUI.ChangeHpBar;
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();

        ApplyFuelUsage();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            return;
        }
            
        // Debug.Log("col : "+ damageable.ToString() );
        damageable.TakeDamage(5.0f);    
    }

    // ������ ���� �� ������ ����
    private void ApplyEngineForce()
    {
        if (_accelerationInput == 0 || _stat.IsFuelEmpty())
        {
            _rigidbody2D.drag = Mathf.Lerp(_rigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            _rigidbody2D.drag = 0;
        }

        _velocityVsUp = Vector2.Dot(transform.up, _rigidbody2D.velocity);

        if (_velocityVsUp > _stat.Data.MaxSpeed && _accelerationInput > 0)
        {
            return;
        }

        if (_velocityVsUp < -_stat.Data.MaxSpeed * 0.5f && _accelerationInput < 0)
        {
            return;
        }

        if (_rigidbody2D.velocity.sqrMagnitude > _stat.Data.MaxSpeed * _stat.Data.MaxSpeed && _accelerationInput > 0)
        {
            return;
        }

        if (!_stat.IsFuelEmpty())
        {
            Vector2 engineForeceVector = transform.up * _accelerationInput * _stat.Data.AcclerationForce;
            _rigidbody2D.AddForce(engineForeceVector, ForceMode2D.Force);
        }
    }

    // ������ ȸ���� ó��
    private void ApplySteering()
    {
        float minSpeedBeforeAllowTuring = (_rigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTuring = Mathf.Clamp01(minSpeedBeforeAllowTuring);

        _rotationAngle -= _stat.Data.RotationForce * _steeringInput * minSpeedBeforeAllowTuring;

        _rigidbody2D.MoveRotation(_rotationAngle);
    }
    
    // ������ ��������� ���������� �ӵ��� ����
    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = forwardVelocity + rightVelocity * _stat.Data.DriftFactor;
    }

    // ���� ����� ó��
    private void ApplyFuelUsage()
    {
        if (_accelerationInput == 0 || _stat.IsFuelEmpty())
        {
            return;
        }
        float speedRatio = _rigidbody2D.velocity.magnitude / _stat.Data.MaxSpeed;
        _stat.CurrentFuelAmount -= Mathf.Lerp(0, _stat.FUEL_USE_AMOUNT, speedRatio) * Time.fixedDeltaTime;
    }

    // IDamageable ����
    public void TakeDamage(float damage)
    {
        _stat.CurrentHp -= damage;
    }
}

