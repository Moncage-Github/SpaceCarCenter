using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : IEnemyState
{
    EnemyBase _enemy;
    public EnemyDead(EnemyBase enemy)
    {
        _enemy = enemy;

    }

    // Update is called once per frame
    public void Update(EnemyBase enemy)
    {
        
    }
}
