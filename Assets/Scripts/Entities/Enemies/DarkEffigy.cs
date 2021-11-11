using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEffigy : EnemyBase
{
    [SerializeField] float minThrowSwordRange;
    [SerializeField] float maxThrowSwordRange;
    [SerializeField] float spinRange;

    public enum DarkEffigyAttacks
    {
        ThrowSword,
        Spin,
        AttackTypesCount
    };

    [NamedArrayAttribute(new string[] { "ThrowSword", "Spin" })]
    public float[] darkEffigyCooldowns = new float[(int)DarkEffigyAttacks.AttackTypesCount];
    private float[] darkEffigyTimer = new float[(int)DarkEffigyAttacks.AttackTypesCount];
    protected DarkEffigyAttacks darkEffigyAttack = DarkEffigyAttacks.AttackTypesCount;
    protected bool ThrowSwordAvailable => (darkEffigyTimer[(int)DarkEffigyAttacks.ThrowSword] >= darkEffigyCooldowns[(int)DarkEffigyAttacks.ThrowSword]);

    protected bool SpinAvailable => (darkEffigyTimer[(int)DarkEffigyAttacks.Spin] >= darkEffigyCooldowns[(int)DarkEffigyAttacks.Spin]);

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        SetMovementAnim();
    }

    public override void Attack()
    {
        base.Attack();

        if (CanAttack)
        {
            if (ThrowSwordAvailable &&
                (transform.GetDistance(detector.GetCurTarget()) < maxThrowSwordRange) &&
                (transform.GetDistance(detector.GetCurTarget()) > minThrowSwordRange))
            {
                print("Throw Sword");
                ThrowSwordAttack();
                attackUsed = true;
            }

            if (SpinAvailable && transform.GetDistance(detector.GetCurTarget()) < spinRange)
            {
                print("Spin");
                SpinAttack();
                attackUsed = true;
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                //reset cooldown so Enemy can attack again
                darkEffigyTimer[(int)darkEffigyAttack] = 0;
                attackEnded = false;
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();

        for (int a = 0; a < darkEffigyCooldowns.Length; a++)
        {
            darkEffigyTimer[a] += Time.deltaTime;
        }
    }

    private void ThrowSwordAttack()
    {
        darkEffigyAttack = DarkEffigyAttacks.ThrowSword;

        _myAnimator.SetTrigger("SwordThrow");
    }

    private void SpinAttack()
    {
        darkEffigyAttack = DarkEffigyAttacks.Spin;

        _myAnimator.SetTrigger("Spin");
    }
}
