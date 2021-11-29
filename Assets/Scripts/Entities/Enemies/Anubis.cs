using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anubis : EnemyBase
{
    public enum AnubisAttacks
    {
        Disc,
        AttackTypesCount,
        Tornado,
    };

    AnubisAttacks anubisAttack = AnubisAttacks.AttackTypesCount;

    public List<float> healthTornadoTriggers;

    public float discDamage, discSpeed, discAngle;
    public float tornadoDamage, minTornadoRad, maxTornadoRad, tornadoSpeed;
    public GameObject discPrefab, tornadoPrefab;
    private List<Tornado> tornados = new List<Tornado>();
    private int tornadoIndex = 0;

    [NamedArray(new string[] { "Disc" })]
    public float[] anubisCooldowns = new float[(int)AnubisAttacks.AttackTypesCount];
    private float[] anubisTimers = new float[(int)AnubisAttacks.AttackTypesCount];

    protected bool DiscAvailable => anubisTimers[(int)AnubisAttacks.Disc] >= anubisCooldowns[(int)AnubisAttacks.Disc];

    private bool _atHalfHealth;

    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsAggroed", true);

        healthTornadoTriggers.Sort();
        healthTornadoTriggers.Reverse();

        for (int t = 0; t < anubisTimers.Length; t++) anubisTimers[t] = anubisCooldowns[t];
        for (int s = 0; s < healthTornadoTriggers.Count; s++)
        {
            tornados.Add(Instantiate(tornadoPrefab, transform).GetComponent<Tornado>());
            tornados[s].gameObject.SetActive(false);
        }

        _atHalfHealth = false;
        isBoss = true;
    }

    public override void Update()
    {
        base.Update();

        if (stats.health <= 0)
        {
            foreach (Tornado t in tornados) t.gameObject.SetActive(false);
        }

        else
        {
            if (stats.health < (stats.MAX_HEALTH / 2))
                _atHalfHealth = true;

            if (curState != EnemyStates.Patrolling && healthTornadoTriggers.Count > 0)
            {
                if (stats.health / stats.MAX_HEALTH * 100 <= healthTornadoTriggers[0])
                {
                    if (!attackEnded) AttackEnd();
                    SetAttack(AnubisAttacks.Tornado);
                    healthTornadoTriggers.RemoveAt(0);
                }
            }
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (CanAttack)
        {
            if (!attackUsed && DiscAvailable)
            {
                SetAttack(AnubisAttacks.Disc);
                curAttackDmg = discDamage;
            }
        }
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < anubisCooldowns.Length; a++) anubisTimers[a] += Time.deltaTime;
    }

    public void SetAttack(AnubisAttacks type)
    {
        _myAnimator.SetTrigger(type.ToString());
        attackUsed = true;
        anubisAttack = type;
        curState = EnemyStates.Attacking;
        MyRigid.velocity = Vector3.zero;
        if (type != AnubisAttacks.Tornado) anubisTimers[(int)anubisAttack] = 0;
        attackEnded = false;
    }

    public void ReleaseDiscs()
    {
        Disc newDisc = Instantiate(projectileObj, shootPoint.position, transform.rotation).GetComponent<Disc>();
        newDisc.SetVariables(detector.GetCurTarget(), projectileSpeed, discDamage, 0.0f);

        if (_atHalfHealth)
        {
            Disc rightDisc = Instantiate(projectileObj, shootPoint.position, transform.rotation).GetComponent<Disc>();
            rightDisc.SetVariables(detector.GetCurTarget(), projectileSpeed, discDamage, discAngle);

            Disc leftDisc = Instantiate(projectileObj, shootPoint.position, transform.rotation).GetComponent<Disc>();
            leftDisc.SetVariables(detector.GetCurTarget(), projectileSpeed, discDamage, -discAngle);
        }

    }

    public void SpawnTornado()
    {
        tornados[tornadoIndex].Summon(transform.RandomRadiusPoint(minTornadoRad, maxTornadoRad), tornadoDamage, tornadoSpeed);
        tornadoIndex++;
    }
}
