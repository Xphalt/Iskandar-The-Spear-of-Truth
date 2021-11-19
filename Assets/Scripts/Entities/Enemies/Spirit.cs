using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : EnemyBase
{
    public float knockbackForce, knockbackDuration, deathDmg;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if(!isDead)
        {
            base.Update();

            if (stats.health <= 0)
                curAttackDmg = deathDmg;
        }
    }

    public void DeathExplode()
    {
        if (detector.GetCurTarget().TryGetComponent(out PlayerMovement_Jerzy move))
            move.KnockBack(transform.position, knockbackForce, knockbackDuration);
    }
    
}
