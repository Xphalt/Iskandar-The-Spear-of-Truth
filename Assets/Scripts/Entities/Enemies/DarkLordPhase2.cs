using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLordPhase2 : EnemyBase
{
    public enum PhaseTwoAttacks
    {
        Skull,
        Burst,
        Damn,
        Bolts,
        AttackTypesCount
    };

    PhaseTwoAttacks phaseTwoAttack = PhaseTwoAttacks.AttackTypesCount;

    public float skullDamage, skullRange;
    public float burstDamage, burstRange;
    public float damnationDamage, damnationRange;
    public float knockbackForce, knockbackDuration;
    public float boltDamage, boltRange;

    [NamedArray(new string[] { "Skull", "Burst", "Damn", "Bolts" })]
    public float[] phaseTwoCooldowns = new float[(int)PhaseTwoAttacks.AttackTypesCount];
    [NamedArray(new string[] { "Skull", "Burst", "Damn", "Bolts" })]
    public float[] phaseTwoTimers = new float[(int)PhaseTwoAttacks.AttackTypesCount];

    [NamedArray(new string[] { "Skull", "Burst" })]
    public GameObject[] projectiles;
    public GameObject damnIndicator;
    private List<Transform> indicators = new List<Transform>();

    public Damnation damnationScript;
    private float damnationRotation = 0;

    protected bool SkullAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Skull] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Skull])
         && transform.GetDistance(detector.GetCurTarget()) < skullRange;
    protected bool BurstAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Burst] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Burst])
         && transform.GetDistance(detector.GetCurTarget()) < burstRange;
    protected bool DamnationAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Damn] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Damn])
         && transform.GetDistance(detector.GetCurTarget()) < damnationRange && damnationScript.Finished;
    protected bool BoltsAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Bolts] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Bolts])
         && transform.GetDistance(detector.GetCurTarget()) < boltRange;

    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsAggroed", true);

        for (int t = 0; t < phaseTwoTimers.Length; t++) phaseTwoTimers[t] = phaseTwoCooldowns[t];
        for (int i = 0; i < damnationScript.rows.Length; i++)
        {
            indicators.Add(Instantiate(damnIndicator, transform).transform);
            indicators[i].position = Vector3.zero;
        }
    }

    public override void Update()
    {
        base.Update();

        if (curState == EnemyStates.Attacking && phaseTwoAttack == PhaseTwoAttacks.Damn && damnationScript.Finished)
        {
            damnationScript.EndCast();
            //AttackEnd();
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (CanAttack)
        {
            if (SkullAvailable)
            {
                SetAttack(PhaseTwoAttacks.Skull);
            }

            if (!attackUsed && BurstAvailable)
            {
                SetAttack(PhaseTwoAttacks.Burst);
            }

            if (!attackUsed && DamnationAvailable)
            {
                SetAttack(PhaseTwoAttacks.Damn);
            }
            else print(DamnationAvailable);

            if (!attackUsed && BoltsAvailable)
            {
                SetAttack(PhaseTwoAttacks.Bolts);
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < phaseTwoCooldowns.Length; a++)
        {
            phaseTwoTimers[a] += Time.deltaTime;
        }
    }

    public void SetAttack(PhaseTwoAttacks type)
    {
        print(type);
        _myAnimator.SetTrigger(type.ToString());
        attackUsed = true;
        phaseTwoAttack = type;
        curState = EnemyStates.Attacking;
        MyRigid.velocity = Vector3.zero;
        phaseTwoTimers[(int)phaseTwoAttack] = 0;
        attackEnded = false;

    }

    public void SpawnProjectile()
    {
        ProjectileScript newProj = Instantiate(projectiles[(int)curAttack], null).GetComponent<ProjectileScript>();
        newProj.SetTarget(detector.GetCurTarget());
    }

    public void IndicateDamnation()
    {
        damnationRotation = Random.Range(0, damnationScript.RowAngle);
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].rotation = Quaternion.Euler(Vector3.up * (damnationRotation + damnationScript.RowAngle * i));
            Vector3 newScale = indicators[i].localScale;
            newScale.z = damnationRange;
            indicators[i].localScale = newScale;
            indicators[i].localPosition = Vector3.zero;
            indicators[i].gameObject.SetActive(true);
        }
    }

    public void CastDamnation()
    {
        //foreach (Transform i in indicators) i.gameObject.SetActive(false);
        damnationScript.Cast(damnationRotation, damnationRange);
    }
}
