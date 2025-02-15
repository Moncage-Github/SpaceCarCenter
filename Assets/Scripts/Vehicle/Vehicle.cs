using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour, IDamageable
{
    // Components
    [SerializeField] private VehicleUI _vehicleUI;
    private VehicleInventory _inventory;

    private Rigidbody2D _rigidbody2D;

    [SerializeField] private VehicleStat _stat;
    [SerializeField] private VehicleData _data;

    // User Input Value
    private float _accelerationInput = 0;
    private float _steeringInput = 0;
    private float _currentSteering;
    public float AccelerationInput { set => _accelerationInput = value; }
    public float SteeringInput { set => _steeringInput = value; }

    // Member Variable
    private float _rotationAngle = 0;
    private float _velocityVsUp = 0;

    private Action _hpChangeAction;

    //boost 실행 여부
    private bool _isBoost = false;
    public bool IsBoost { set => _isBoost = value; get { return _isBoost; } }

    //Barrier 여부
    [SerializeField] private int _barrier;
    public int Barrier { set => _barrier = value; get { return _barrier; } }
    public Action IsTakeDamage;

    //Animation
    [SerializeField] private GameObject _image;
    private Animator _animator;

    //아이템 획득 시 작동
    public delegate void GetHealthRegen();
    public GetHealthRegen GetRegen;
    public float RegenValue = 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inventory = GetComponent<VehicleInventory>();
        _animator = _image.GetComponent<Animator>();

        //VehicleData data = DataManager.Instance.GetVehicleData("Test");
        _stat = new VehicleStat(_data);
        _hpChangeAction = () => { _vehicleUI.ChangeHpBar(_stat.CurrentHp / _stat.Data.MaxHp); };

    }

    private void OnEnable()
    {
        _stat.OnFuelChange += _vehicleUI.ChangeFuelBar;
        _stat.OnHpChange += _hpChangeAction;
    }

    private void OnDisable()
    {
        _stat.OnFuelChange -= _vehicleUI.ChangeFuelBar;
        _stat.OnHpChange -= _hpChangeAction;
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

    // 차량의 가속 및 감속을 제어
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
            if (_isBoost == false)
            {
                Vector2 clampedVelocity = _rigidbody2D.velocity.normalized * _stat.Data.MaxSpeed;
                _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, clampedVelocity, Time.fixedDeltaTime * 5);
            }

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

    // 차량의 회전을 처리
    private void ApplySteering()
    {
        float minSpeedBeforeAllowTuring = (_rigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTuring = Mathf.Clamp01(minSpeedBeforeAllowTuring);

        _rotationAngle -= _stat.Data.RotationForce * _steeringInput * minSpeedBeforeAllowTuring;


        _rigidbody2D.MoveRotation(_rotationAngle);

        if (_currentSteering == _steeringInput) return;

        _currentSteering = _steeringInput;

        //애니메이션
        //좌회전
        if (_steeringInput == -1)
        {
            Debug.Log("좌");
            _animator.SetTrigger("LeftSteering");
        }
        //우회전
        else if (_steeringInput == 1)
        {
            Debug.Log("우");
            _animator.SetTrigger("RightSteering");
        }
        //전진
        else
        {
            Debug.Log("전진");
            _animator.SetTrigger("FowardSteering");
        }
    }

    // 차량의 진행방향의 수직방향의 속도를 감소
    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = forwardVelocity + rightVelocity * _stat.Data.DriftFactor;
    }

    // 연료 사용을 처리
    private void ApplyFuelUsage()
    {
        if(_stat.IsFuelEmpty())
        {
            CollectionManager.Instance.GameOver(GameOverType.OutOfFuel);
        }
        if (_accelerationInput == 0)
        {
            return;
        }
        float speedRatio = _rigidbody2D.velocity.magnitude / _stat.Data.MaxSpeed;
        _stat.CurrentFuelAmount -= Mathf.Lerp(0, _stat.FUEL_USE_AMOUNT, speedRatio) * Time.fixedDeltaTime;

        
    }

    // IDamageable 구현
    public void TakeDamage(float damage)
    {
        IsTakeDamage?.Invoke();

        FindObjectOfType<CollectionManager>().ShakeCamera(0.2f, 0.15f);

        if (_barrier > 0)
        {
            _barrier--;
            Debug.Log("Barrier");
            return;
        }

        _stat.CurrentHp -= damage;
        CollectionManager.Instance.ReceivedDamage += damage;

        Debug.Log("Player Damaged, Current HP : " + _stat.CurrentHp);

        if(_stat.CurrentHp <= 0)
        {
            CollectionManager.Instance.GameOver(GameOverType.Dead);
        }
    }

    public void HealthRegen()
    {
        _stat.CurrentHp += RegenValue;
        Debug.Log("체력회복" + _stat.CurrentHp.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.TryGetComponent(out CollectableItem item))
        //{
        //    _inventory.AddItemToInventory(item.ItemCode);
        //    Destroy(item.gameObject);
        //}
    }
}

