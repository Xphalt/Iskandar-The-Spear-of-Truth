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

    public int minSkulls = 1, maxSkulls = 3;
    public float skullDamage, skullRange, skullSpeed, skullArc = 180, skullSeekDelay, healthLostForExtraSkull = 25;
    public float burstDamage, burstRange, burstSpeed, burstRadius, knockbackForce, knockbackDuration;
    public float damnationDamage, damnationRange;
    public float boltInnerDamage, boltOuterDamage, minBoltRange, boltRange, boltNum, boltInnerRadius, boltOuterRadius, boltSlowPercent, boltSlowDuration;

    [NamedArray(new string[] { "Skull", "Burst", "Damn", "Bolts" })]
    public float[] phaseTwoCooldowns = new float[(int)PhaseTwoAttacks.AttackTypesCount];
    [NamedArray(new string[] { "Skull", "Burst", "Damn", "Bolts" })]
    public float[] phaseTwoTimers = new float[(int)PhaseTwoAttacks.AttackTypesCount];

    public GameObject skullProjectile, flameProjectile, propheticBolt;
    public GameObject damnIndicator, boltIndicator;
    private List<Transform> damnIndicators = new List<Transform>();
    private List<Transform> boltIndicators = new List<Transform>();
    private List<Transform> bolts = new List<Transform>();
    private Vector3 boltIndicatorScale, boltScale;

    public Damnation damnationScript;
    private float damnationRotation = 0, boltLifetime = 0;

    protected bool SkullAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Skull] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Skull])
         && transform.GetDistance(detector.GetCurTarget()) < skullRange;
    protected bool BurstAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Burst] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Burst])
         && transform.GetDistance(detector.GetCurTarget()) < burstRange;
    protected bool DamnationAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Damn] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Damn])
         && transform.GetDistance(detector.GetCurTarget()) < damnationRange && damnationScript.Finished;
    protected bool BoltsAvailable => (phaseTwoTimers[(int)PhaseTwoAttacks.Bolts] >= phaseTwoCooldowns[(int)PhaseTwoAttacks.Bolts])
         && transform.GetDistance(detector.GetCurTarget()) < boltRange;
    protected int SkullNum => Mathf.Min(minSkulls + Mathf.FloorToInt((1 - stats.health / stats.MAX_HEALTH) * 100 / healthLostForExtraSkull), maxSkulls);

    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsAggroed", true);

        for (int t = 0; t < phaseTwoTimers.Length; t++) phaseTwoTimers[t] = phaseTwoCooldowns[t];
        for (int i = 0; i < damnationScript.rows.Length; i++)
        {
            damnIndicators.Add(Instantiate(damnIndicator, transform).transform);
            damnIndicators[i].position = Vector3.zero;
            damnIndicators[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < boltNum; i++)
        {
            boltIndicators.Add(Instantiate(boltIndicator, transform).transform);
            boltIndicators[i].gameObject.SetActive(false);
            bolts.Add(Instantiate(propheticBolt, transform).transform);
            bolts[i].gameObject.SetActive(false);
        }
        if (boltNum > 0)
        {
            boltIndicatorScale = boltIndicators[0].localScale;
            boltScale = bolts[0].localScale;
            boltLifetime = bolts[0].GetComponent<ParticleSystem>().main.duration;
        }

        isBoss = true;
    }

    public override void Update()
    {
        base.Update();

        if (curState == EnemyStates.Attacking && phaseTwoAttack == PhaseTwoAttacks.Damn && damnationScript.Finished) damnationScript.EndCast();
    }

    public override void Attack()
    {
        base.Attack();

        if (CanAttack)
        {
            if (SkullAvailable)
            {
                SetAttack(PhaseTwoAttacks.Skull);
                targeting = true;
            }

            if (!attackUsed && BurstAvailable)
            {
                SetAttack(PhaseTwoAttacks.Burst);
                targeting = true;
            }

            if (!attackUsed && DamnationAvailable)
            {
                SetAttack(PhaseTwoAttacks.Damn);
            }

            if (!attackUsed && BoltsAvailable)
            {
                SetAttack(PhaseTwoAttacks.Bolts);
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < phaseTwoCooldowns.Length; a++)  phaseTwoTimers[a] += Time.deltaTime;
    }

    public void SetAttack(PhaseTwoAttacks type)
    {
        _myAnimator.SetTrigger(type.ToString());
        attackUsed = true;
        phaseTwoAttack = type;
        curState = EnemyStates.Attacking;
        MyRigid.velocity = Vector3.zero;
        phaseTwoTimers[(int)phaseTwoAttack] = 0;
        attackEnded = false;

    }

    public void SpawnSkull()
    {
        float curArc = skullArc / maxSkulls * SkullNum;
        for (int s = 0; s < SkullNum; s++)
        {
            SkullStorm newProj = Instantiate(skullProjectile, shootPoint.position, Quaternion.identity).GetComponent<SkullStorm>();
            Vector3 skullDir = Quaternion.Euler(Vector3.up * Random.Range(-curArc / 2, curArc / 2)) * transform.forward;
            newProj.SetVariables(detector.GetCurTarget(), skullDamage, skullSpeed, skullSeekDelay, skullDir);
        }

        targeting = false;
    }

    public void SpawnFireball()
    {
        Fireball newProj = Instantiate(flameProjectile, shootPoint.position, Quaternion.identity).GetComponent<Fireball>();
        newProj.Launch(shootPoint.position, detector.GetCurTarget().position, burstSpeed, burstDamage, burstRadius, knockbackForce, knockbackDuration);
        targeting = false;
    }

    public void IndicateDamnation()
    {
        damnationRotation = Random.Range(0, damnationScript.RowAngle);
        for (int i = 0; i < damnIndicators.Count; i++)
        {
            damnIndicators[i].rotation = Quaternion.Euler(Vector3.up * (damnationRotation + damnationScript.RowAngle * i));
            Vector3 newScale = damnIndicators[i].localScale;
            newScale.z = damnationRange / 2;
            damnIndicators[i].localScale = newScale;
            damnIndicators[i].position = transform.position + damnIndicators[i].forward * damnIndicators[i].lossyScale.z / 2;
            damnIndicators[i].gameObject.SetActive(true);
        }
    }

    public void CastDamnation()
    {
        foreach (Transform i in damnIndicators) i.gameObject.SetActive(false);
        damnationScript.Cast(damnationRotation, damnationRange, damnationDamage);
    }

    public void IndicateBolts()
    {
        foreach (Transform i in boltIndicators)
        {
            i.position = transform.RandomRadiusPoint(minBoltRange, boltRange); // Use random navmesh point to avoid buildig/edges?
            i.gameObject.SetActive(true);
            i.SetParent(null);
            i.localScale = new Vector3(boltOuterRadius * 2 * boltIndicatorScale.x, i.localScale.y, boltOuterRadius * 2 * boltIndicatorScale.z);
        }
    }

    public IEnumerator CastBolts()
    {
        for (int i = 0; i < boltNum; i++)
        {
            boltIndicators[i].SetParent(transform);
            boltIndicators[i].localScale = boltIndicatorScale;
            boltIndicators[i].gameObject.SetActive(false);

            bolts[i].position = boltIndicators[i].position;
            bolts[i].SetParent(null);
            bolts[i].gameObject.SetActive(true);

            foreach (Collider c in Physics.OverlapSphere(boltIndicators[i].position, boltOuterRadius))
            {
                if (c.TryGetComponent(out PlayerStats player))
                {
                    player.TakeDamage(boltOuterDamage);
                    StartCoroutine(player.GetComponent<PlayerMovement_Jerzy>().Slow(1 - boltSlowPercent / 100.0f, boltSlowDuration));
                    if (transform.GetDistance(c.transform) < boltInnerRadius) player.TakeDamage(boltInnerDamage);
                }
            }
        }

        yield return new WaitForSeconds(boltLifetime);

        foreach (Transform b in bolts)
        {
            b.SetParent(transform);
            b.localScale = boltScale;
            b.gameObject.SetActive(false);
        }
    }
}
