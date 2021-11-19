using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : EnemyBase
{
    public enum ServantAttacks
    {
        Swing,
        Taunt,
        AttackTypesCount
    };

    public float blockDuration;
    private bool blocking = false;
    private float blockTimer;

    [NamedArray(new string[] { "Swing", "Taunt" })]
    public float[] servantCooldowns = new float[(int)ServantAttacks.AttackTypesCount];

    private float[] servantTimers = new float[(int)ServantAttacks.AttackTypesCount];

    protected ServantAttacks servantAttack = ServantAttacks.AttackTypesCount;

    protected bool SwingAvailable => servantTimers[(int)ServantAttacks.Swing] >= servantCooldowns[(int)ServantAttacks.Swing]
        && detector.MeleeRangeCheck(swingRange);
    protected bool TauntAvailable => servantTimers[(int)ServantAttacks.Taunt] >= servantCooldowns[(int)ServantAttacks.Taunt]
        && detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee]);

    public float swingDamage, swingRange;

    public override void Start()
    {
        base.Start();
        for (int t = 0; t < servantTimers.Length; t++) servantTimers[t] = servantTimers[t];
    }

    public override void Attack()
    {
        if (blocking)
        {
            blockTimer += Time.deltaTime;
            if (detector.GetCurTarget()) transform.rotation = Quaternion.LookRotation(detector.GetCurTarget().position - transform.position);
            if (blockTimer > blockDuration) EndBlock();
        }
        else if (CanAttack)
        {
            if (SwingAvailable && !attackUsed)
            {
                SetAttack(ServantAttacks.Swing);
                curAttackDmg = swingDamage;
            }
            else if (TauntAvailable && !attackUsed)
            {
                SetAttack(ServantAttacks.Taunt);
                curAttackDmg = attackDamages[(int)AttackTypes.Melee];
                Block();
            }
            base.Attack();
        }
    }

    public void SetAttack(ServantAttacks type)
    {
        _myAnimator.SetTrigger(type.ToString());
        attackUsed = true;
        servantAttack = type;
        curState = EnemyStates.Attacking;
        MyRigid.velocity = Vector3.zero;
        servantTimers[(int)servantAttack] = 0;
        attackEnded = false;
    }

    public void Block()
    {
        blocking = true;
        blockTimer = 0;
        stats.vulnerable = false;
        _myAnimator.SetBool("IsBlocking", true);
    }

    public void EndBlock()
    {
        blocking = false;
        stats.vulnerable = true;
        _myAnimator.SetBool("IsBlocking", false);
        AttackEnd();
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < servantCooldowns.Length; a++)
        {
            servantTimers[a] += Time.deltaTime;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("playerSword") && blocking)
        {
            EndBlock();
            MeleeAttack();
        }
    }
}
