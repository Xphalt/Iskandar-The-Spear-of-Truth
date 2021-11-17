using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : EnemyBase
{
    public enum DwarfAttacks
    {
        Melee,
        Shoot,
        AttackTypesCount
    };


    public GameObject Explosive;
    [Range(0, 100)]
    public int explosiveDropChance;
    private bool droppedExplosive = false;

    public bool IsDead = false;

    [NamedArrayAttribute(new string[] { "Melee", "Shoot"})]
    public float[] dwarfCooldowns = new float[(int)DwarfAttacks.AttackTypesCount];
    private float[] dwarfTimers = new float[(int)DwarfAttacks.AttackTypesCount];

    protected DwarfAttacks dwarfAttack = DwarfAttacks.AttackTypesCount;

    protected bool dwarfMeleeAvalable => (dwarfTimers[(int)DwarfAttacks.Melee] >= dwarfCooldowns[(int)DwarfAttacks.Melee]);
    protected bool dwarfShootAvalable => (dwarfTimers[(int)DwarfAttacks.Shoot] >= dwarfCooldowns[(int)DwarfAttacks.Shoot]);


    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
      
        base.Update();
        

        //if (stats.health <= 0)
        //{
        //    IsDead = true;
        //    OnDeathExplosive(explosiveDropChance);
        //}
    }

    public void OnDeathExplosive(int chance)
    {
        int percent = Random.Range(0, 101);
        if (percent < chance && !droppedExplosive)
        {
            droppedExplosive = true;
            Instantiate(Explosive, transform.position, Quaternion.identity);
        }
    }

    public override void Attack()
    {

        if (CanAttack)
        {

            if (MeleeAvailable && detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget()))
            {
                MeleeAttack();
            }

            if (ShootAvailable && (detector.GetCurTarget().position - transform.position).magnitude <= attackRanges[(int)AttackTypes.Shoot])
            {
                ShootAttack();
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                //reset cooldown so Enemy can attack again
                dwarfTimers[(int)dwarfAttack] = 0;
            }
        }
    }

    protected override void MeleeAttack()
    {
        base.MeleeAttack();
        dwarfAttack = DwarfAttacks.Melee;
    }

    protected override void ShootAttack()
    {
        base.ShootAttack();
        dwarfAttack = DwarfAttacks.Shoot;
    }

    protected override void AttackCooldown()
    {
        for (int a = 0; a < dwarfCooldowns.Length; a++)
        {
            dwarfTimers[a] += Time.deltaTime;
        }
    }
}
