using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anubis : EnemyBase
{
    public enum AnubisAttacks
    {
        Disc,
        Tornado,
        AttackTypesCount,
    };

    AnubisAttacks anubisAttack = AnubisAttacks.AttackTypesCount;

    public List<float> healthDiscTriggers;
    public int discsReleased;
    public float minDiscRad, maxDiscsRad, discSpeed;
    public float rollDamage, rollRange, rollMinDistance, runUpSpeed, rollAnimDuration = 1.45f;
    public float windRange, windAngle, channelDuration;
    public float knockbackForce, knockbackDuration, regenAmount, regenInterval;
    public GameObject discPrefab, windFX;
    private List<Disc> discs = new List<Disc>();

    private float regenTimer = 0, channelTimer = 0;
    private bool windUp = false;

    [NamedArray(new string[] { "Disc" })]
    public float[] anubisCooldowns = new float[(int)AnubisAttacks.AttackTypesCount];
    private float[] anubisTimers = new float[(int)AnubisAttacks.AttackTypesCount];

    protected bool RollAvailable => (anubisTimers[(int)AnubisAttacks.Disc] >= anubisCooldowns[(int)AnubisAttacks.Disc])
         && transform.GetDistance(detector.GetCurTarget()) < rollRange && transform.GetDistance(detector.GetCurTarget()) > rollMinDistance;
    protected bool WindAvailable => (anubisTimers[(int)AnubisAttacks.Tornado] >= anubisCooldowns[(int)AnubisAttacks.Tornado])
         && transform.GetDistance(detector.GetCurTarget()) < windRange && (anubisTimers[(int)AnubisAttacks.Disc] >= anubisCooldowns[(int)AnubisAttacks.Disc]);

    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsAggroed", true);

        healthDiscTriggers.Sort();
        healthDiscTriggers.Reverse();

        for (int t = 0; t < anubisTimers.Length; t++) anubisTimers[t] = anubisCooldowns[t];
        for (int s = 0; s < discsReleased * healthDiscTriggers.Count; s++)
        {
            discs.Add(Instantiate(discPrefab, transform).GetComponent<Disc>());
            discs[s].gameObject.SetActive(false);
        }

        chargeDistance = chargeSpeed * rollAnimDuration;
        chargeDuration = chargeDistance / chargeSpeed;

        windFX.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (windUp)
        {
            if (detector.GetCurTarget()) MyRigid.velocity = (detector.GetCurTarget().position - transform.position).normalized * runUpSpeed;
            else MyRigid.velocity = transform.forward * runUpSpeed;
            transform.rotation = Quaternion.LookRotation(MyRigid.velocity);
        }

        regenTimer += Time.deltaTime;
        if (regenTimer > regenInterval)
        {
            stats.health = Mathf.Min(stats.health + regenAmount, stats.MAX_HEALTH);
            regenTimer = 0;
        }

        if (curState == EnemyStates.Aggro && healthDiscTriggers.Count > 0)
        {
            if (stats.health / stats.MAX_HEALTH * 100 < healthDiscTriggers[0])
            {
                if (!attackEnded) AttackEnd();
                SetAttack(AnubisAttacks.Disc);
                healthDiscTriggers.RemoveAt(0);
            }
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (CanAttack)
        {
            if (!attackUsed && RollAvailable)
            {
                SetAttack(AnubisAttacks.Disc);
                curAttackDmg = rollDamage;
                windUp = true;
            }

            if (!attackUsed && WindAvailable)
            {
                SetAttack(AnubisAttacks.Tornado);
                StartCoroutine(ChannelWind());
                curAttackDmg = 0;
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < anubisCooldowns.Length; a++) anubisTimers[a] += Time.deltaTime;
    }

    public override void AttackEnd()
    {
        base.AttackEnd();

        if (windFX.activeSelf) windFX.SetActive(false);
    }

    public void SetAttack(AnubisAttacks type)
    {
        _myAnimator.SetTrigger(type.ToString());
        attackUsed = true;
        anubisAttack = type;
        curState = EnemyStates.Attacking;
        MyRigid.velocity = Vector3.zero;
        if (type != AnubisAttacks.Disc) anubisTimers[(int)anubisAttack] = 0;
        attackEnded = false;
    }

    public void Roll()
    {
        chargeStart = transform.position;
        chargeDirection = (detector.GetCurTarget().position - transform.position).normalized;
        chargeTimer = 0;
        MyRigid.velocity = chargeDirection * chargeSpeed;

        charging = true;
        windUp = false;
    }

    protected override void EndCharge()
    {
        charging = false;
        MyRigid.velocity = Vector3.zero;
    }

    public void ReleaseDiscs()
    {
        
    }

    public IEnumerator ChannelWind()
    {
        yield return new WaitForSeconds(channelDuration);
        _myAnimator.SetTrigger("WindCast");
    }

    public void WindAttack()
    {
        windFX.transform.position = shootPoint.position;
        windFX.SetActive(true);
        if (detector.GetCurTarget())
        {
            Vector3 dirToTarget = detector.GetCurTarget().position - transform.position;
            if (transform.GetDistance(detector.GetCurTarget()) < windRange && Vector3.Angle(transform.forward, dirToTarget) < windAngle / 2)
                detector.GetCurTarget().GetComponent<PlayerMovement_Jerzy>().KnockBack(transform.position, knockbackForce, knockbackDuration);
        }
    }
}
