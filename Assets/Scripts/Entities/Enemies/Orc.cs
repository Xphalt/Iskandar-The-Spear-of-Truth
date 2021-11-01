using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : EnemyBase
{
    private float buffDuration;
    private float buffTimer = 0;

    private List<float> defaultDamages = new List<float>();
    private float defaultSpeed = 0;

    private void Awake()
    {
        foreach (float damage in attackDamages)
            defaultDamages.Add(damage);

        defaultSpeed = chaseSpeed;
    }

    public override void Update()
    {
        if(stats.health <= 0)
        {
            _myAnimator.SetBool("IsDead", true);
            _myCapsuleCol.enabled = false;
        }

        if(!isDead)
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


            if (buffTimer < buffDuration)
            {
                buffTimer += Time.deltaTime;
                if (buffTimer > buffDuration) Debuff();
            }

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

        if (charging)
            _myAnimator.SetBool("IsCharging", true);
    }

    protected override void MeleeAttack()
    {
        if (detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget()))
        {
            _myAnimator.SetTrigger("Hit");

            attackUsed = true;
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector3.zero;
        }
    }

    public void Buff(float percent, float duration)
    {
        buffDuration = duration;
        buffTimer = 0;

        for(int d = 0; d < attackDamages.Length; d++)
            attackDamages[d] = defaultDamages[d] * (1 + percent / 100);

        chaseSpeed = defaultSpeed * (1 + percent / 100);
    }

    public void Debuff()
    {
        for (int d = 0; d < attackDamages.Length; d++)
            attackDamages[d] = defaultDamages[d];

        chaseSpeed = defaultSpeed;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (detector.GetCurTarget() != null && charging)
        {
            if (collision.collider.transform == detector.GetCurTarget())
            {
                _myAnimator.SetTrigger("ChargeHit");
                _myAnimator.SetBool("IsCharging", false);
                stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Charge]);
                EndCharge();
            }
        }
    }
}
