using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestilentOne : EnemyBase
{
    public enum PestilentAttacks
    {
        Roll,
        Wind,
        AttackTypesCount,
        Spores
    };

    PestilentAttacks pestilentAttack = PestilentAttacks.AttackTypesCount;

    public List<float> healthSporeTriggers;
    public int sporesReleased;
    public float minSporeRad, maxSporesRad, sporeSpeed;
    public float rollDamage, rollRange, rollMinDistance, runUpSpeed, rollAnimDuration = 1.45f;
    public float windRange, windAngle, channelDuration;
    public float knockbackForce, knockbackDuration, regenAmount, regenInterval;
    public GameObject sporePrefab, windFX;
    private List<Spore> spores = new List<Spore>();

    private float regenTimer = 0, channelTimer = 0;
    private bool windUp = false;

    [NamedArray(new string[] { "Roll", "Wind" })]
    public float[] pestilentCooldowns = new float[(int)PestilentAttacks.AttackTypesCount];
    private float[] pestilentTimers = new float[(int)PestilentAttacks.AttackTypesCount];

    protected bool RollAvailable => (pestilentTimers[(int)PestilentAttacks.Roll] >= pestilentCooldowns[(int)PestilentAttacks.Roll])
         && transform.GetDistance(detector.GetCurTarget()) < rollRange && transform.GetDistance(detector.GetCurTarget()) > rollMinDistance;
    protected bool WindAvailable => (pestilentTimers[(int)PestilentAttacks.Wind] >= pestilentCooldowns[(int)PestilentAttacks.Wind])
         && transform.GetDistance(detector.GetCurTarget()) < windRange && (pestilentTimers[(int)PestilentAttacks.Roll] >= pestilentCooldowns[(int)PestilentAttacks.Roll]);

    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsAggroed", true);

        healthSporeTriggers.Sort();
        healthSporeTriggers.Reverse();

        for (int t = 0; t < pestilentTimers.Length; t++) pestilentTimers[t] = pestilentCooldowns[t];
        for (int s = 0; s < sporesReleased * healthSporeTriggers.Count; s++)
        {
            spores.Add(Instantiate(sporePrefab, transform).GetComponent<Spore>());
            spores[s].gameObject.SetActive(false);
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

        if (curState == EnemyStates.Aggro && healthSporeTriggers.Count > 0)
        {
            if (stats.health / stats.MAX_HEALTH * 100 < healthSporeTriggers[0])
            {
                SetAttack(PestilentAttacks.Spores);
                healthSporeTriggers.RemoveAt(0);
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
                SetAttack(PestilentAttacks.Roll);
                curAttackDmg = rollDamage;
                windUp = true;
            }

            if (!attackUsed && WindAvailable)
            {
                SetAttack(PestilentAttacks.Wind);
                StartCoroutine(ChannelWind());
                curAttackDmg = 0;
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < pestilentCooldowns.Length; a++) pestilentTimers[a] += Time.deltaTime;
    }

    public override void AttackEnd()
    {
        base.AttackEnd();

        if (windFX.activeSelf) windFX.SetActive(false);
    }

    public void SetAttack(PestilentAttacks type)
    {
        _myAnimator.SetTrigger(type.ToString());
        attackUsed = true;
        pestilentAttack = type;
        curState = EnemyStates.Attacking;
        MyRigid.velocity = Vector3.zero;
        if (type != PestilentAttacks.Spores) pestilentTimers[(int)pestilentAttack] = 0;
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

    public void ReleaseSpores()
    {
        for (int s = 0; s < sporesReleased; s++)
            spores[s].StartArc(shootPoint.position, transform.RandomRadiusPoint(minSporeRad, maxSporesRad), sporeSpeed);
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
