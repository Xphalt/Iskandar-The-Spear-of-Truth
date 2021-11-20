using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLordPhase1 : EnemyBase
{
    public enum PhaseOneAttacks
    {
        Combo,
        Spin,
        Kick,
        Lunge,
        AttackTypesCount
    };

    PhaseOneAttacks phaseOneAttack = PhaseOneAttacks.AttackTypesCount;

    public GameObject SecondPhaseObj;
    public float[] meleeAttackDamages = new float[4];
    public float shieldStunDuration;
    private int curCombo = 0;

    public float comboRange;
    public float spinDamage, spinRange;
    public float kickDamage, kickRange;
    public float knockbackForce, knockbackDuration;
    public float lungeDamage, minLungeRange;

    private bool stunning = false;

    [NamedArrayAttribute(new string[] { "Combo", "Spin", "Kick", "Lunge" })]
    public float[] phaseOneCooldowns = new float[(int)PhaseOneAttacks.AttackTypesCount];
    [NamedArrayAttribute(new string[] { "Combo", "Spin", "Kick", "Lunge" })]
    private float[] phaseOneTimers = new float[(int)PhaseOneAttacks.AttackTypesCount];

    protected bool ComboAvailable => (phaseOneTimers[(int)PhaseOneAttacks.Combo] >= phaseOneCooldowns[(int)PhaseOneAttacks.Combo])
         && detector.MeleeRangeCheck(comboRange);
    protected bool SpinAvailable => (phaseOneTimers[(int)PhaseOneAttacks.Spin] >= phaseOneCooldowns[(int)PhaseOneAttacks.Spin])
         && detector.MeleeRangeCheck(spinRange);
    protected bool KickAvailable => (phaseOneTimers[(int)PhaseOneAttacks.Kick] >= phaseOneCooldowns[(int)PhaseOneAttacks.Kick])
         && detector.MeleeRangeCheck(kickRange);
    protected bool LungeAvailable => (phaseOneTimers[(int)PhaseOneAttacks.Lunge] >= phaseOneCooldowns[(int)PhaseOneAttacks.Lunge])
         && transform.GetDistance(detector.GetCurTarget()) > minLungeRange;

    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsAggroed", true);

        for (int t = 0; t < phaseOneTimers.Length; t++) phaseOneTimers[t] = phaseOneCooldowns[t];
        isBoss = true;
    }

    public override void Attack()
    {
        base.Attack();

        if (CanAttack)
        {
            if (ComboAvailable)
            {
                SetAttack(PhaseOneAttacks.Combo);
                curCombo = 0;
                curAttackDmg = meleeAttackDamages[curCombo];
            }

            if (!attackUsed && SpinAvailable)
            {
                SetAttack(PhaseOneAttacks.Spin);
                curAttackDmg = spinDamage;
            }

            if (!attackUsed && KickAvailable)
            {
                SetAttack(PhaseOneAttacks.Kick);
                curAttackDmg = kickDamage;
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < phaseOneCooldowns.Length; a++)
        {
            phaseOneTimers[a] += Time.deltaTime;
        }
    }

    public void SetAttack(PhaseOneAttacks type)
    {
        _myAnimator.SetTrigger(type.ToString());
        attackUsed = true;
        phaseOneAttack = type;
        curState = EnemyStates.Attacking;
        MyRigid.velocity = Vector3.zero;
        phaseOneTimers[(int)phaseOneAttack] = 0;
        attackEnded = false;
    }

    public void IncreaseCombo()
    {
        curCombo++;
        curAttackDmg = meleeAttackDamages[curCombo];
    }

    public void SetStun(int active)
    {
        stunning = active > 0;
    }

    public override void ChargeAttack()
    {
        if (LungeAvailable)
        {
            SetAttack(PhaseOneAttacks.Lunge);
            curAttackDmg = 0;
            base.ChargeAttack();
        }
        else AttackEnd();
    }

    public void PhaseTransition()
    {
        SecondPhaseObj.transform.position = transform.position;
        SecondPhaseObj.SetActive(true);
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        if (collider.TryGetComponent(out PlayerMovement_Jerzy move))
        {
            if (phaseOneAttack == PhaseOneAttacks.Kick) move.KnockBack(transform.position, knockbackForce, knockbackDuration);
            if (stunning) move.Stun(shieldStunDuration);
        }
    }
}
