using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemyBase : EnemyBase
{
    private BasicBoss _basicBoss;

    [HideInInspector]
    public Transform InitMovePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (_currentState == State.Move) OnMove();
        else if (_currentState == State.Dead) OnDead();
        else if (_currentState == State.Skill) OnSkill();
        else if (_currentState == State.Attack) OnAttack();
        else OnIdle();

        Excute();
    }

    public override void Init()
    {
        base.Init();

        _basicBoss.Squad.Add(gameObject);
        InitMovePoint = _basicBoss.GetTarget().transform;

        EnemyMove = new SummonEnemyMove(this);
    }


    protected override void OnDead()
    {
        _basicBoss.Squad.Remove(gameObject);

        base.OnDead();

        return;
    }

    public void SummonInit(BasicBoss basicBoss)
    {
        _basicBoss = basicBoss;

        Init();
    }
}

public class SummonEnemyMove : EnemyMove
{
    private SummonEnemyBase _enemy;

    public SummonEnemyMove(SummonEnemyBase enemy) : base(enemy)
    {
        _enemy = enemy;
    }

    public override void Update(EnemyBase enemy)
    {
        TargetPosition = _enemy.InitMovePoint.position;

        RotateTowardsTarget();

        // 회전이 완료되면 이동 시작
        if (Quaternion.Angle(_enemy.transform.rotation, Quaternion.Euler(0, 0, GetAngleToTarget() - 90)) < 0.1f)
        {
            MoveTowardsTarget();
        }
    }

    protected override void SetRandomTargetPosition()
    {
        return;
    }
}
