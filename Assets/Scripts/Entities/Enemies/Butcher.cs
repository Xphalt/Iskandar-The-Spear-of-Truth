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

    public float bleedDamage;
    public int maxBleedTicks;
    public float bleedDelay;
    public float minBandaidRadius, maxBandaidRadius;

    public float knockBackSpeed;
    public float knockBackDuration;
    public float stunDuration;

    public GameObject bandaid;

    [NamedArrayAttribute(new string[] { "BleedSlash" })]
    public float[] butcherCooldowns = new float[(int)ButcherAttacks.AttackTypesCount];
    [NamedArrayAttribute(new string[] { "BleedSlash" })]
    public float[] butcherAttackDamages = new float[(int)ButcherAttacks.AttackTypesCount];

    private float[] butcherTimers = new float[(int)ButcherAttacks.AttackTypesCount];
    protected ButcherAttacks butcherAttack = ButcherAttacks.AttackTypesCount;

    protected bool BleedSlashAvailable => (butcherTimers[(int)ButcherAttacks.BleedSlash] >= butcherCooldowns[(int)ButcherAttacks.BleedSlash])
        && detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget());
    private bool slashing=false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsChasing", true);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        if(CanAttack)
        {
            if (BleedSlashAvailable)
            {
                BleedSlashAttack();
                attackUsed = true;
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                curAttackDmg = butcherAttackDamages[(int)butcherAttack];
                //reset cooldown so Enemy can attack again
                butcherTimers[(int)butcherAttack] = 0;
                attackEnded = false;
            }
        }
        base.Attack();
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < butcherCooldowns.Length; a++)
        {
            butcherTimers[a] += Time.deltaTime;
        }
    }

    protected override void EndCharge()
    {
        _myAnimator.SetBool("IsCharging", false);
        base.EndCharge();
    }

    public override void AttackEnd()
    {
        base.AttackEnd();
        slashing = false;
    }

    protected override void ChargeAttack()
    {
        _myAnimator.SetBool("IsCharging", true);
        attackUsed = true;
        curAttack = AttackTypes.Charge;
    }

    public void ButcherCharge()
    {
        transform.rotation = Quaternion.LookRotation(detector.GetCurTarget().position - transform.position);
        base.ChargeAttack();
    }


    private void BleedSlashAttack()
    {
        slashing = true;
        butcherAttack = ButcherAttacks.BleedSlash;
        _myAnimator.SetTrigger("BleedSlash");
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if(detector.GetCurTarget() != null && charging)
        {
            if(collision.collider.transform == detector.GetCurTarget())
            {
                //call Jerzy's Knockback thing
                stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Charge]);
                collision.gameObject.GetComponent<PlayerMovement_Jerzy>().KnockBack(transform.position, knockBackSpeed, knockBackDuration, stunDuration);
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (detector.IsTarget(other.transform))
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), curAttackDmg);
            if(slashing)
            {
                other.GetComponent<PlayerStats>().SetBleed(bleedDamage,maxBleedTicks,bleedDelay);
                Vector3 spawnPos = transform.position;
                spawnPos.x = detector.GetCurTarget().position.x + Random.Range(minBandaidRadius, maxBandaidRadius);
                spawnPos.z = detector.GetCurTarget().position.z + Random.Range(minBandaidRadius, maxBandaidRadius);
                bandaid.transform.position = spawnPos;
                bandaid.SetActive(true);
            }
            hitCollider.enabled = false;
        }
    }
}
