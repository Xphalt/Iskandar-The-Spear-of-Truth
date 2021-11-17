using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : EnemyBase
{
    public enum ServantAttacks
    {
        Slash,
        Swing,
        Taunt,
        AttackTypesCount
    };

    public float blockDuration;
    private bool blocking = false;
    private float blockTimer;

    [NamedArrayAttribute(new string[] { "Warcry", "Flurry", "Lunge", "Parry" })]
    public float[] servantCooldowns = new float[(int)ServantAttacks.AttackTypesCount];

    private float[] servantTimers = new float[(int)ServantAttacks.AttackTypesCount];

    protected ServantAttacks servantAttack = ServantAttacks.AttackTypesCount;

    protected bool SlashAvailable => (servantTimers[(int)ServantAttacks.Slash] >= servantCooldowns[(int)ServantAttacks.Slash]);
    protected bool SwingAvailable => (servantTimers[(int)ServantAttacks.Swing] >= servantCooldowns[(int)ServantAttacks.Swing]);
    protected bool TauntAvailable => (servantTimers[(int)ServantAttacks.Taunt] >= servantCooldowns[(int)ServantAttacks.Taunt]);

    public float slashDamage;
    public float swingDamage;
    public float counterDamage;


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Attack();
    }

    public override void Attack()
    {
        if (blocking)
        {
            blockTimer += Time.deltaTime;
            if (detector.GetCurTarget()) transform.rotation = Quaternion.LookRotation(detector.GetCurTarget().position - transform.position);
            if (blockTimer > blockDuration) EndBlock();
        }
        else if (detector.GetCurTarget() != null)
        {
            base.Attack();
            if (TauntAvailable && !attackUsed)
            {
                Taunt();
            }
            else if (SwingAvailable && !attackUsed)
            {
                MeleeSwing();
            }
            else if (SlashAvailable && !attackUsed)
            {
                MeleeSlash();
            }
        }
    }

    public void MeleeSlash()
    {
        attackUsed = true;
        Debug.Log("slash");
    }

    public void MeleeSwing()
    {
        attackUsed = true;
        Debug.Log("swing");
    }

    public void Taunt()
    {
        attackUsed = true;
        Debug.Log("taunt");
        blocking = true;
    }

    public void EndBlock()
    {
        blocking = false;
    }

    protected override void AttackCooldown()
    {
        for (int a = 0; a < servantCooldowns.Length; a++)
        {
            servantTimers[a] += Time.deltaTime;
        }
    }

}
