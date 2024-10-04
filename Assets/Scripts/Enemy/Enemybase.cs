using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum State
{
    None,
    Idle,
    Move,
    Attack,
    Skill,
    Dead
}

public class EnemyBase : MonoBehaviour
{
    private State _currentState = State.None;

    [SerializeField] private float _moveSpeed;          //Enemy의 이동 속도
    [SerializeField] private float _movementRadius;     //Enemy의 활동 반경
    [SerializeField] private float _movementCycle;      //Enemy의 활동 주기
    [SerializeField] private float _detectionRadius;    //Enemy의 플레이어 감지 범위
    [SerializeField] private float _rotationSpeed;      //Enemy의 회전 속도
    [SerializeField] private float _attackCycle;        //Enemy의 공격 주기

    //Setter
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public float MovementRadius { get => _movementRadius; set => _movementRadius = value; }
    public float MovementCycle { get => _movementCycle; set => _movementCycle = value; }
    public float DetectionRadius { get => _detectionRadius; set => _detectionRadius = value; }
    public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
    public float AttackCycle { get => _attackCycle; set => _attackCycle = value; }

    IEnemyState _enemyState;

    //State
    protected EnemyIdle _enemyIdle;
    protected EnemyMove _enemyMove;
    protected EnemyDead _enemyDead;
    protected EnemySkill _enemySkill;
    protected EnemyAttack _enemyAttack;

    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;


    //Bullet
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _bulletPos;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();

        _currentState = State.Move;

        _enemyIdle = new EnemyIdle(this);
        _enemyMove = new EnemyMove(this);
        _enemyDead = new EnemyDead(this);
        _enemySkill = new EnemySkill(this);
        _enemyAttack = new EnemyAttack(this, _bullet);
}

    // Update is called once per frame
    protected virtual void Update()
    {

        if (_currentState == State.Move) OnMove();
        else if (_currentState == State.Dead) OnDead();
        else if ( _currentState == State.Skill) OnSkill();
        else if ( _currentState == State.Attack) OnAttack();
        else OnIdle();

        Excute();
    }

    private void Excute()
    {
        Debug.Log("Excute");
        Debug.Log(_enemyState);
        _enemyState.Update(this);
    }

    //기본 Enemy는 스킬 없음
    protected virtual void OnSkill()
    {
        _enemyState = _enemySkill;
        return;
    }

    protected virtual void OnIdle()
    {
        //animation
        Debug.Log("Can Idle");
        _enemyState = _enemyIdle;
        return;
    }

    protected virtual void OnDead()
    {
        if (_currentState != State.Dead)
        {
            _currentState = State.Dead;
        }
        _enemyState = _enemyDead;
        return;
    }

    protected virtual void OnMove()
    {
        if (!CanMove())
        {
            _currentState = State.Move;
        }
        _enemyState = _enemyMove;
        return;
    }

    private bool CanMove()
    {
        return _currentState != State.Attack;
    }

    protected virtual void OnAttack()
    {
        if (_currentState != State.Attack)
        {
            _currentState = State.Attack;
        }
        _enemyState = _enemyAttack;
        Debug.Log(_enemyState);
        return;
    }

    public void BulletShooting()
    {
        Debug.Log("총알 발사");
        Instantiate(_bullet, _bulletPos.position, Quaternion.identity);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _currentState = State.Attack;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("적에게서 벗어남");
            OnMove();
        }

    }
}
