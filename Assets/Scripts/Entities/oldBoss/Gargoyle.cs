using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gargoyle : BossStats
{
    public float lightAtkKnockback;
    public float chargeDistance;
    public float chargeSpeed;
    private bool isCharging;
    [Header("Final Attack")]
    public int maxNumOfCharges;
    private int usedCharges;


    public override void LightAttack()
    {
        lightAtkHit = false;
        //this attack needs to be tied to an active raidius around the Gargoyle 
        //and if the player gets inside it, the Gargoyle will jab at the
        //player, inflicting damage and a knockback effect
        if(lightAtkTimer >= lightAtkCooldown && transform.GetDistance(detector.GetCurTarget()) <= lightAtkRange)
        {
            detector.GetCurTarget().GetComponent<PlayerStats>().TakeDamage(lightAtkDamage, godMode);
            detector.GetCurTarget().GetComponent<Rigidbody>().AddForce((detector.GetCurTarget().position - transform.position).normalized * lightAtkKnockback,ForceMode.Impulse);
            lightAtkTimer = 0;
            lightAtkHit = true;
        }
    }

    public override void HeavyAttack()
    {
        //this charge is different to the Boar's, as it goes past the Player's position
        //if attack hits return true, if not return false
        //dont forget to only return when charge ends (be it missing or hitting the player)
        //For the Gargoyle this is the only attack that we care to return true or false back to the FSM,
        //since there'll be different outcomes
        if(!isCharging)
        {
            myRigid.velocity = (detector.GetCurTarget().position - transform.position).normalized * chargeSpeed;
            isCharging = true;
        }
    }

    public override bool HeavyAttackFinished()
    {
        if (((returnSpot.position - transform.position).magnitude >= chargeDistance))
        {
            isCharging = false;

            return true;
        }
        return false;
    }

    public override void FinalAttack()
    {
        if (usedCharges == 0 || (returnSpot.position - transform.position).magnitude >= chargeDistance)
            HeavyAttack();


    }

    public override bool HasFinalAttackFinished()
    {
        return usedCharges >= maxNumOfCharges;
    }

    public override void FinishFinalAttack()
    {
        usedCharges = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isCharging)
        {
            heavyAtkHit = true;
            detector.GetCurTarget().GetComponent<PlayerStats>().TakeDamage(heavyAtkDamage, godMode);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Node"))
            hasReturned = true;
    }
}
