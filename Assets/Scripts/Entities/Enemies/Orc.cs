using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : EnemyBase
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        switch (curState)
        {
            case EnemyStates.Patrolling:
                _myAnimator.SetBool("IsPatrolling", true);
                _myAnimator.SetBool("IsChasing", false);
                break;
            case EnemyStates.Chasing:
                _myAnimator.SetBool("IsPatrolling", false);
                _myAnimator.SetBool("IsChasing", true);
                break;
            case EnemyStates.Attacking:
                break;
            default:
                break;
        }
    }

    protected override void EndCharge()
    {
        _myAnimator.SetBool("IsCharging", false);
        base.EndCharge();
    }

    protected override void ChargeAttack()
    {
        base.ChargeAttack();

        if(charging)
            _myAnimator.SetBool("IsCharging", true);
    }

    protected override void MeleeAttack()
    {
        base.MeleeAttack();

        if(attackUsed)
            _myAnimator.SetTrigger("Hit");
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform == detector.GetCurTarget() && charging)
        {
            _myAnimator.SetTrigger("ChargeHit");
            _myAnimator.SetBool("IsCharging", false);
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Charge]);
        }
    }
}
