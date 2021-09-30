using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public float meleeDamage;
    public float meleeRange;
    public string attackSound;

    public override void Update()
    {
        base.Update();

        if (TargetInFront()) Attack();
    }

    public bool TargetInFront()
    {
        foreach (RaycastHit targetScan in Physics.RaycastAll(transform.position, transform.forward, meleeRange))
        {
            if (targetScan.transform == curTarget) return true;
        }

        return false;
    }

    public override void Attack()
    {
        if (canAttack)
        {
            base.Attack();
            print(curTarget.name);

            //sfxScript.PlaySFX3D(attackSound, transform.position);
        }
    }
}
