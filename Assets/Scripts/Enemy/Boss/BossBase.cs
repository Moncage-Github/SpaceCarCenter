using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GraphicsBuffer;

public class BossBase : EnemyBase
{
    [SerializeField] protected bool BattleMode;
    [SerializeField] protected float SkillCycle;
    protected List<IEnemyState> SkillList = new List<IEnemyState>();

    [SerializeField] protected float AttackTimer;
    [SerializeField] protected float SkillTimer;
    private bool _skillRunning;

    protected IEnemyState BossAttack;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Profiler.BeginSample("Boss Profiler");

        if (BattleMode == false)
            return;

        if(_skillRunning)
        {
            Excute();
            return;
        }

        if(_currentState != State.Skill && _currentState != State.Attack)
        {
            AttackTimer -= Time.deltaTime;
            SkillTimer -= Time.deltaTime;
        }
            
        if(AttackTimer < 0)
        {
            AttackTimer = AttackCycle;

            _currentState = State.Attack;
        }

        if(SkillTimer < 0)
        {
            SkillTimer = SkillCycle;
            OnSkill();
            _skillRunning = true;
        }

        if (_currentState == State.Dead) OnDead();
        else if (_currentState == State.Skill) OnSkill();
        else if (_currentState == State.Attack) OnAttack();
        else OnIdle();

        Excute();

        Profiler.EndSample();
    }

    public override void Init()
    {
        base.Init();

        _currentState = State.Idle;

        AttackTimer = AttackCycle;
        SkillTimer = SkillCycle;

        BossAttack = new BossBasicAttack(this, AttackEnd);

        _skillRunning = false;
    }

    protected virtual void DetectionPlayer()
    {

        BattleMode = true;
    }

    protected override void OnSkill()
    {
        //스킬 랜덤 후 해당 스킬 state에 넣기
        _enemyState = EnemySkill;

        _currentState = State.Skill;

        AttackTimer = AttackCycle;
        SkillTimer = SkillCycle;

        return;
    }

    protected override void OnAttack()
    {     
        _currentState = State.Attack;
        
        _enemyState = BossAttack;
        return;
    }

    protected virtual void SkillEnd()
    {
        _currentState = State.Idle;
        _enemyState = EnemyIdle;

        _skillRunning = false;
        AttackTimer = AttackCycle;
    }

    protected virtual void AttackEnd()
    {
        _currentState = State.Idle;
        _enemyState = EnemyIdle;

        AttackTimer = AttackCycle;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _target = collision.transform;
        }
    }
    

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DetectionPlayer();
        }

    }
}

public class BossBasicAttack : EnemyAttack
{
    Action _action;

    public BossBasicAttack(EnemyBase enemy,  Action action) : base(enemy)
    {
        _action = action;
    }

    public override void Update(EnemyBase enemy)
    {
        Debug.Log("Boss Attack 작동");

        RotateTowardsTarget();
        
        // 회전이 완료되면 총알 발사
        if (Quaternion.Angle(Enemy.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget() - 90)) < 3.0f)
        {
            Enemy.BulletShooting();

            _action();
        }
        
    }
}
