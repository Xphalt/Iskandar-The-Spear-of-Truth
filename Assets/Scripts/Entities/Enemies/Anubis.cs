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

    public List<float> healthTornadoTriggers;

    public float discDamage, discSpeed;
    public float tornadoDamage, minTornadoRad, maxTornadoRad;
    public GameObject discPrefab, tornadoPrefab;
    private List<Tornado> tornados = new List<Tornado>();

    [NamedArray(new string[] { "Disc" })]
    public float[] anubisCooldowns = new float[(int)AnubisAttacks.AttackTypesCount];
    private float[] anubisTimers = new float[(int)AnubisAttacks.AttackTypesCount];

    protected bool DiscAvailable => anubisTimers[(int)AnubisAttacks.Disc] >= anubisCooldowns[(int)AnubisAttacks.Disc];

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
    }

    public override void Update()
    {
        base.Update();

        if (curState == EnemyStates.Aggro && healthTornadoTriggers.Count > 0)
        {
            if (stats.health / stats.MAX_HEALTH * 100 <= healthTornadoTriggers[0])
            {
                if (!attackEnded) AttackEnd();
                SetAttack(AnubisAttacks.Disc);
                healthTornadoTriggers.RemoveAt(0);
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
                SetAttack(AnubisAttacks.Tornado);
                curAttackDmg = 0;
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
        
    }

    public void SpawnTornado()
    {
        tornados[0].transform.position = transform.RandomRadiusPoint(minTornadoRad, maxTornadoRad);
        tornados[0].transform.SetParent(null);
        tornados[0].gameObject.SetActive(true);
        tornados.RemoveAt(0);
    }
}
