using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : EnemyBase
{
    public enum CasterAttacks
    {
        SummonSpell,
        Pillar,
        AttackTypesCount
    };

    public float minSpawnRadius;
    public float maxSpawnRadius;
    [System.Serializable]
    public struct PrefabsToSpawn
    {
        public GameObject obj;
        public int howMany;
    }
    public PrefabsToSpawn[] prefabsToSpawn;

    [NamedArrayAttribute(new string[] { "SummonSpell", "Pillar" })]
    public bool[] casterAvailableAttacks = new bool[(int)CasterAttacks.AttackTypesCount];
    [NamedArrayAttribute(new string[] { "SummonSpell", "Pillar" })]
    public float[] casterCooldowns = new float[(int)CasterAttacks.AttackTypesCount];
    private float[] casterTimers = new float[(int)CasterAttacks.AttackTypesCount];
    protected CasterAttacks casterAttack = CasterAttacks.AttackTypesCount;

    protected bool SummonAvailable => (casterTimers[(int)CasterAttacks.SummonSpell] >= casterCooldowns[(int)CasterAttacks.SummonSpell]);

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
            if (SummonAvailable)
            {
                SummonSpellCast();
                attackUsed = true;
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                //reset cooldown so Enemy can attack again
                casterTimers[(int)casterAttack] = 0;
                attackEnded = false;
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < casterCooldowns.Length; a++)
        {
            casterTimers[a] += Time.deltaTime;
        }
    }

    private void SummonSpellCast()
    {
        casterAttack = CasterAttacks.SummonSpell;

        _myAnimator.SetTrigger("CastSpell");
    }

    public void SpawnStuff()
    {
        Vector3 spawnPos = transform.position;
        for(int obj = 0; obj < prefabsToSpawn.Length; obj++)
        {
            for (int t = 0; t < prefabsToSpawn[obj].howMany; t++)
            {
                GameObject newSpawn = Instantiate(prefabsToSpawn[obj].obj);
                newSpawn.transform.position = transform.RandomRadiusPoint(minSpawnRadius, maxSpawnRadius);
            }
        }
    }
}
