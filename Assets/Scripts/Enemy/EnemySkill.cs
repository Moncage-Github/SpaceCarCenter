using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : IEnemyState
{
    EnemyBase _enemy;
    public EnemySkill(EnemyBase enemy)
    {
        _enemy = enemy;
        
    }

    // Update is called once per frame
    public void Update(EnemyBase enemy)
    {
        
    }
}
