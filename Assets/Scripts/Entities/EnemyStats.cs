using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : StatsInterface
{
    // THIS IS TEMPORARY FOR VERTICAL SLICE
    public bool HasBeenDefeated() { return health <= 0; }

    public override void TakeDamage(float amount)
    {
        health -= amount;

        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public override void DealDamage(StatsInterface target, float amount)
    {
        target.TakeDamage(amount);
    }
}
