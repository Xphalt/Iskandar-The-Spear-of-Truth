using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : EnemyBase
{

    public GameObject Explosive;
    [Range(0, 100)]
    public int explosiveDropChance;

    private bool shootUsed = false;
    private bool meleeUsed = false;
    private float shootTimer;
    private float meleeTimer;

    private void Awake()
    {
        shootTimer = attackCooldowns[(int)AttackTypes.Shoot];
        meleeTimer = attackCooldowns[(int)AttackTypes.Melee];
    }

    // Update is called once per frame
    public override void Update()
    {
        Attack();
        ResetAttacks();
    }


    public override void Attack()
    {
        if (meleeUsed == false)
        {
            meleeUsed = true;
            MeleeAttack();
        }
        
        if (shootUsed == false)
        {
            shootUsed = true;
            ShootAttack();
        }       
    }

    protected override void MeleeAttack()
    {
        base.MeleeAttack();
        
    }

    protected override void ShootAttack()
    {
        base.ShootAttack();
        
    }

    public void ResetAttacks()
    {
        if (shootUsed)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                shootUsed = false;
                shootTimer = attackCooldowns[(int)AttackTypes.Shoot];
            }
        }

        if (meleeUsed)
        {
            meleeTimer -= Time.deltaTime;
            if (meleeTimer <= 0)
            {
                meleeUsed = false;
                meleeTimer = attackCooldowns[(int)AttackTypes.Melee];
            }
        }
    }

    public void OnDeathExplosive(int chance)
    {
        int percent = Random.Range(0, 101);
        if (percent < chance)
        {
            Instantiate(Explosive, transform.position, Quaternion.identity);
        }
    }


}
