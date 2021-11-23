using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : EnemyBase
{
    public float knockbackForce, knockbackDuration, deathDmg;
    private PlayerMovement_Jerzy move;
    private PlayerStats pStats;

    public override void Start()
    {
        base.Start();
        move = GameObject.Find("Player").GetComponent<PlayerMovement_Jerzy>();
        pStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    public void DeathExplode()
    {
        if (pStats.spiritualDamage > 0.0f)
            curAttackDmg = 0.0f;
        else
            curAttackDmg = deathDmg;
            
        //if (detector.GetCurTarget().TryGetComponent(out PlayerMovement_Jerzy move))
        //    move.KnockBack(transform.position, knockbackForce, knockbackDuration);
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (detector.IsTarget(collider.transform) && isDead && (pStats.spiritualDamage <= 0.0f))
        {
            move.KnockBack(transform.position, knockbackForce, knockbackDuration);
        }
    }

}
