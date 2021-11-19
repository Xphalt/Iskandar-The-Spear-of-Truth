using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : EnemyBase
{
    public float knockbackForce, knockbackDuration, deathDmg;
    private PlayerMovement_Jerzy move;

    public override void Start()
    {
        base.Start();
        move = GameObject.Find("Player").GetComponent<PlayerMovement_Jerzy>();
    }

    public override void Update()
    {
        if(!isDead)
        {
            base.Update();
                
            SetMovementAnim();
        }
    }

    public void DeathExplode()
    {
        curAttackDmg = deathDmg;

        //if (detector.GetCurTarget().TryGetComponent(out PlayerMovement_Jerzy move))
            move.KnockBack(transform.position, knockbackForce, knockbackDuration);
    }
    
}
