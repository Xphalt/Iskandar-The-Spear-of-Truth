using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : StatsInterface
{
    public bool vulnerable = true;

    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if (!vulnerable) return;
        health -= amount;

        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        target.TakeDamage(amount, scriptedKill);
    }
}
