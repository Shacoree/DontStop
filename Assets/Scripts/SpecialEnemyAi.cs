using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Pool;
using static UnityEngine.Rendering.DebugUI;

public class SpecialEnemyAi : EnemyAi
{
    new private IObjectPool<SpecialEnemyAi> enemyPool;

    new void Awake()
    {
        //parameters override
        enemyHealth = new UnitHealth(250, 250);

        base.Awake();
    }
    new public void ReleaseEnemy()
    {
        enemyPool.Release(this);
    }
    public void SetPool(IObjectPool<SpecialEnemyAi> pool)
    {
        enemyPool = pool;
    }
}
