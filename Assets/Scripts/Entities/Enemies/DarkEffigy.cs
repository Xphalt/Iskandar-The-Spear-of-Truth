using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEffigy : EnemyBase
{
    [SerializeField] private float _minThrowSwordRange;
    [SerializeField] private float _maxThrowSwordRange;
    [SerializeField] private float _spinRange;
    [SerializeField] private float _spinDamage;
    public float throwSwordDamage;

    private string _swordTag = "DarkEffigySword";

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
        if (CanAttack)
        {
            if (ThrowSwordAvailable &&
                (transform.GetDistance(detector.GetCurTarget()) < _maxThrowSwordRange) &&
                (transform.GetDistance(detector.GetCurTarget()) > _minThrowSwordRange))
            {
                print("Throw Sword");
                ThrowSwordAttack();
                attackUsed = true;
            }

            if (SpinAvailable && transform.GetDistance(detector.GetCurTarget()) < _spinRange)
            {
                print("Spin");
                SpinAttack();
                attackUsed = true;
            }
            else
            {
                base.Attack();
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

    protected override void ShootAttack()
    {
        transform.LookAt(detector.GetCurTarget().position, Vector3.up);
        GameObject projectile = Instantiate(projectileObj, transform);
        attackUsed = true;
        curAttack = AttackTypes.Shoot;
        MyRigid.velocity = Vector3.zero;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if (collider.gameObject.CompareTag(_swordTag) && GetComponentInChildren<DarkEffigyThrowSword>()._returnToSender)
        {
            _myAnimator.SetTrigger("SwordReturn");
        }
    }
}
