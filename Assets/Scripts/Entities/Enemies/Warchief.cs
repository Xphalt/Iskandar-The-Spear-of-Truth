using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warchief : EnemyBase
{
    public int flurryChance;

    public enum WarchiefAttacks
    {
        Warcry,
        Flurry,
        Lunge,
        Parry,
        AttackTypesCount
    };

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
}

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        if (AttackEnded && detector.GetCurTarget() != null)
        {
            attackUsed = false;

            /*if(WarcryAvailable)
            {
                WarcryAttack();
            }*/

            int rand = Random.Range(0,101);
            if (MeleeAvailable)
            {
                if (rand > flurryChance)
                    FlurryAttack();
                else
                    MeleeAttack();
            }

            /*if(LungeAvailable && Distance check from Player)
            {
               //LungeAttack();
            }*/

            if(/*ParryAvailable &&*/ stats.health < stats.health/2)
            {
                //ParryAttack()
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                //reset cooldown so Enemy can attack again
                attackTimers[(int)curAttack] = 0;
                curAttackTimer = 0;
            }
        }
    }

    private void WarcryAttack()
    {

    }

    private void FlurryAttack()
    {

    }

    private void LungeAttack()
    {

    }

    private void ParryAttack()
    {

    }
}
