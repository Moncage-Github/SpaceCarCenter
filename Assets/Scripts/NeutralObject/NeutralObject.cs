using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralObject : EnemyBase
{
    public bool IsFriendly;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (_currentState == State.Move) OnMove();
        else if (_currentState == State.Dead) OnDead();
        else if (_currentState == State.Skill) OnSkill();
        else if (_currentState == State.Attack && !IsFriendly) OnAttack();
        else OnIdle();

        Excute();

        Debug.Log("중립 몬스터");
    }

    public override void Init()
    {
        base.Init();

        IsFriendly = true;
    }
}
