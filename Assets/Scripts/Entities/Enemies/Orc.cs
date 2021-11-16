using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : EnemyBase
{
    private float buffDuration;
    private float buffTimer = 0;

    private List<float> defaultDamages = new List<float>();
    private float defaultSpeed = 0;

    private void Awake()
    {
        foreach (float damage in attackDamages)
            defaultDamages.Add(damage);

        defaultSpeed = aggroedMoveSpeed;
    }

    public override void Update()
    {
        if(!isDead)
        {
            base.Update();

            SetMovementAnim();

            if (buffTimer < buffDuration)
            {
                buffTimer += Time.deltaTime;
                if (buffTimer > buffDuration) Debuff();
            }

        }
    }

    protected override void EndCharge()
    {
        _myAnimator.SetBool("IsCharging", false);
        base.EndCharge();
    }

    public override void ChargeAttack()
    {
        base.ChargeAttack();

        if (charging)
            _myAnimator.SetBool("IsCharging", true);
    }

    public void Buff(float percent, float duration)
    {
        buffDuration = duration;
        buffTimer = 0;

        for(int d = 0; d < attackDamages.Length; d++)
            attackDamages[d] = defaultDamages[d] * (1 + percent / 100);

        aggroedMoveSpeed = defaultSpeed * (1 + percent / 100);
    }

    public void Debuff()
    {
        for (int d = 0; d < attackDamages.Length; d++)
            attackDamages[d] = defaultDamages[d];

        aggroedMoveSpeed = defaultSpeed;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (detector.GetCurTarget() != null && charging)
        {
            if (collision.collider.transform == detector.GetCurTarget())
            {
                _myAnimator.SetTrigger("ChargeHit");
                _myAnimator.SetBool("IsCharging", false);
                stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Charge]);
                EndCharge();
            }
        }
    }
}
