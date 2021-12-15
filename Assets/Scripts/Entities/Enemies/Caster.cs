using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        public float ySpawnOffset;
    }
    public PrefabsToSpawn[] prefabsToSpawn;

    private bool pillarCast = false;
    private bool isSpectre = false;
    private float pillarTimer;
    public float summonChannelTime;
    public float pillarChannelTime;
    public float pillarActivateDelay;
    public float pillarActiveTime;
    public float pillarDmg;
    public GameObject pillarBase;
    public GameObject pillarHitbox;

    [NamedArrayAttribute(new string[] { "SummonSpell", "Pillar" })]
    public bool[] casterAvailableAttacks = new bool[(int)CasterAttacks.AttackTypesCount];
    [NamedArrayAttribute(new string[] { "SummonSpell", "Pillar" })]
    public float[] casterCooldowns = new float[(int)CasterAttacks.AttackTypesCount];
    [NamedArrayAttribute(new string[] { "SummonSpell", "Pillar" })]
    public float[] casterRecoveries = new float[(int)CasterAttacks.AttackTypesCount];
    private float[] casterTimers = new float[(int)CasterAttacks.AttackTypesCount];
    protected CasterAttacks casterAttack = CasterAttacks.AttackTypesCount;

    protected bool SummonAvailable => (casterTimers[(int)CasterAttacks.SummonSpell] >= casterCooldowns[(int)CasterAttacks.SummonSpell]);
    protected bool PillarAvailable => (casterTimers[(int)CasterAttacks.Pillar] >= casterCooldowns[(int)CasterAttacks.Pillar]);

    public override void Start()
    {
        base.Start();
        for (int i = 0; i < casterCooldowns.Length; i++) casterTimers[i] = casterCooldowns[i];

        if (casterAvailableAttacks[(int)CasterAttacks.Pillar])
            isSpectre = true;

        if (pillarBase.TryGetComponent(out ParticleSystem ps))
        {
            ParticleSystem.MainModule psm = ps.main;
            psm.simulationSpace = ParticleSystemSimulationSpace.Local;
        }
    }

    public override void Update()
    {
        base.Update();

        if(isSpectre)
        {
            if (pillarCast && !isDead)
                PillarTimer();
            else
                DeathCastCleanup();
        }

    }

    private void DeathCastCleanup()
    {
        pillarHitbox.SetActive(false);
        pillarBase.SetActive(false);
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

            if(!attackUsed && PillarAvailable && casterAvailableAttacks[(int)CasterAttacks.Pillar])
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
        StartCoroutine("SummonSpellChannel");
    }

    public IEnumerator SummonSpellChannel()
    {
        _myAnimator.SetBool("Channeling",true);
        yield return new WaitForSeconds(summonChannelTime);
        _myAnimator.SetBool("Channeling",false);
        _myAnimator.SetTrigger("CastSummon");
    }

    public IEnumerator SummonSpellRecovery()
    {
        _myAnimator.SetBool("Recovering",true);
        yield return new WaitForSeconds(casterRecoveries[(int)CasterAttacks.SummonSpell]);
        _myAnimator.SetBool("Recovering",false);
        AttackEnd();
    }

    private void MagicPillarCast()
    {
        casterAttack = CasterAttacks.Pillar;
        _myAnimator.SetBool("Channeling",true);
        pillarCast = true;
    }

    public void SpawnStuff()
    {
        Vector3 spawnPos = transform.position;
        for(int obj = 0; obj < prefabsToSpawn.Length; obj++)
        {
            for (int t = 0; t < prefabsToSpawn[obj].howMany; t++)
            {
                GameObject newSpawn = Instantiate(prefabsToSpawn[obj].obj);
                newSpawn.transform.position = transform.RandomRadiusPoint(minSpawnRadius, maxSpawnRadius)+Vector3.up * prefabsToSpawn[obj].ySpawnOffset;
                if (newSpawn.TryGetComponent(out NavMeshAgent newAgent))
                {
                    NavMesh.SamplePosition(newSpawn.transform.position, out NavMeshHit hit, 100.0f, NavMesh.AllAreas);
                    newAgent.Warp(hit.position);
                }
            }
        }
    }

    private void PillarTimer()
    {
        //make AoE appear on the ground and follow player here
        pillarBase.SetActive(true);

        pillarTimer += Time.deltaTime;
        if (pillarTimer >= pillarChannelTime)
         StartCoroutine("PillarActivation");
        else
         pillarBase.transform.position = detector.GetCurTarget().position;
    }

    private IEnumerator PillarActivation()
    {
        _myAnimator.SetBool("Channeling", false);
        _myAnimator.SetTrigger("CastPillar");
        yield return new WaitForSeconds(pillarActivateDelay);
        //pillar shoots out of the ground here
        curAttackDmg = pillarDmg;
        pillarHitbox.SetActive(true);
        yield return new WaitForSeconds(pillarActiveTime);
        pillarHitbox.SetActive(false);
        pillarBase.SetActive(false);
        pillarCast = false;
        pillarTimer = 0;
        yield return new WaitForSeconds(casterRecoveries[(int)CasterAttacks.Pillar]);
        AttackEnd();
    }

}
