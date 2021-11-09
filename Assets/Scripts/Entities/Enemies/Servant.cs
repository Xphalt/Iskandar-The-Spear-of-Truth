using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : EnemyBase
{

    public float slashDamage;
    public float slashCooldown;
    private float slashTimer;

    public float swingDamage;
    public float swingCooldown;
    private float swingTimer;

    public float counterDamage;
    public float tauntCooldown;
    private float tauntTimer;

    private bool slashUsed = false;
    private bool swingUsed = false;
    private bool tauntUsed = false;
    private bool canAttack = true;

    private void Awake()
    {
        slashTimer = slashCooldown;
        swingTimer = swingCooldown;
        tauntTimer = tauntCooldown;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        AttackCycle();
        ManageCooldowns();
    }

    public void AttackCycle()
    {
        canAttack = true;

        if (!tauntUsed && canAttack)
        {
            Taunt();
        }

        if (!swingUsed && canAttack)
        {
            MeleeSwing();
        }

        if (!slashUsed && canAttack)
        {
            MeleeSlash();
        }
    }

    public void MeleeSlash()
    {
        if (detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget()))
        {
            //play melee animation
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector3.zero;
            slashUsed = true;
            canAttack = false;
        }
    }

    public void MeleeSwing()
    {
        if (detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget()))
        {
            //play swing animation
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector3.zero;
            swingUsed = true;
            canAttack = false;
        }
    }

    public void Taunt()
    {
        //play taunt animation
        tauntUsed = true;
        canAttack = false;
    }

    public void ManageCooldowns()
    {
        
        slashTimer -= Time.deltaTime;
        if (slashTimer == 0)
        {
            slashUsed = false;
        }

        
        swingTimer -= Time.deltaTime;
        if (swingTimer == 0)
        {
            swingUsed = false;
        }

        
        tauntTimer -= Time.deltaTime;
        if (tauntTimer == 0)
        {
            tauntUsed = false;
        }
    }

}
