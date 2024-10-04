using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : IEnemyState
{
    EnemyBase _enemy;
    public EnemyIdle(EnemyBase enemy)
    {
        _enemy = enemy;

    }

    // Update is called once per frame
    public void Update(EnemyBase enemy)
    {
        
    }
}
