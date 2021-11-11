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

    private bool pillarCast = false;
    private float pillarTimer;
    public float pillarFollowTime;
    public float pillarActivateDelay;
    public float pillarActiveTime;
    public GameObject pillarBase;
    public GameObject pillarHitbox;

    [NamedArrayAttribute(new string[] { "SummonSpell", "Pillar" })]
    public bool[] casterAvailableAttacks = new bool[(int)CasterAttacks.AttackTypesCount];
    [NamedArrayAttribute(new string[] { "SummonSpell", "Pillar" })]
    public float[] casterCooldowns = new float[(int)CasterAttacks.AttackTypesCount];
    private float[] casterTimers = new float[(int)CasterAttacks.AttackTypesCount];
    protected CasterAttacks casterAttack = CasterAttacks.AttackTypesCount;

    protected bool SummonAvailable => (casterTimers[(int)CasterAttacks.SummonSpell] >= casterCooldowns[(int)CasterAttacks.SummonSpell]);
    protected bool PillarAvailable => (casterTimers[(int)CasterAttacks.Pillar] >= casterCooldowns[(int)CasterAttacks.Pillar]);

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        SetMovementAnim();

        if (pillarCast)
            PillarTimer();
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

            if(PillarAvailable)
            {
                MagicPillarCast();
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

        _myAnimator.SetTrigger("CastSummon");
    }
    private void MagicPillarCast()
    {
        casterAttack = CasterAttacks.Pillar;
        pillarCast = true;
        _myAnimator.SetTrigger("CastPillar");
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

    private void PillarTimer()
    {
        pillarTimer += Time.deltaTime;
        if (pillarTimer >= pillarFollowTime)
        {
            StartCoroutine("PillarActivation");
            pillarTimer = 0;
        }

        //make AoE appear on the ground and follow player here
        pillarBase.SetActive(true);
        pillarBase.transform.position = detector.GetCurTarget().position;

    }

    private IEnumerator PillarActivation()
    {
        pillarCast = false;
        yield return new WaitForSeconds(pillarActivateDelay);
        //pillar shoots out of the ground here
        pillarHitbox.SetActive(true);
        yield return new WaitForSeconds(pillarActiveTime);
        pillarHitbox.SetActive(false);
        pillarBase.SetActive(false);
        AttackEnd();
    }

}
