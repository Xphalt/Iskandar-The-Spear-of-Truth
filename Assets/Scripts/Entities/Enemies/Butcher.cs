using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butcher : EnemyBase
{
    public enum ButcherAttacks
    {
        BleedSlash,
        AttackTypesCount
    };

    [NamedArrayAttribute(new string[] { "BleedSlash" })]
    public float[] butcherCooldowns = new float[(int)ButcherAttacks.AttackTypesCount];
    private float[] butcherTimers = new float[(int)ButcherAttacks.AttackTypesCount];
    protected ButcherAttacks butcherAttack = ButcherAttacks.AttackTypesCount;

    protected bool BleedSlashAvailable => (butcherTimers[(int)ButcherAttacks.BleedSlash] >= butcherCooldowns[(int)ButcherAttacks.BleedSlash]);

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        base.Attack();

        if (!attackUsed && BleedSlashAvailable)
        {
            BleedSlashAttack();
            attackUsed = true;
        }

        if (attackUsed)
        {
            //change state to Attacking
            curState = EnemyStates.Attacking;
            //reset cooldown so Enemy can attack again
            butcherTimers[(int)butcherAttack] = 0;
            attackEnded = false;
        }
    }

    private void BleedSlashAttack()
    {

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if(detector.GetCurTarget() != null && charging)
        {
            if(collision.collider.transform == detector.GetCurTarget())
            {
                //call Jerzy's Knockback thing
                stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Charge]);
            }
        }
    }
}
