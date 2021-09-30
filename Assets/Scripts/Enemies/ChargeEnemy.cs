using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : EnemyBase
{
    private bool rolling = false;

    public float rollSpeed = 15;
    public float rollDuration = 1;
    public float rollDamage = 25;
    public string attackSound;

    public override void Update()
    {
        base.Update();

        if (attackTimer > rollDuration)
        {
            rolling = false;
        }

        if (canAttack)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        if (canAttack)
        {
            base.Attack();
            rolling = true;
            //Shift((target.transform.position - transform.position).normalized * rollSpeed, rollDuration, 0);

            //sfxScript.PlaySFX3D(attackSound, transform.position);
        }
    }

    private void EndRoll()
    {
        rolling = false;
        //EndShift();
    }

    //public override void SaveEnemy(string saveID)
    //{
    //    base.SaveEnemy(saveID);

    //    saveID = "Enemy" + saveID;
    //    SaveManager.UpdateSavedBool(saveID + "Rolling", rolling);

    //    if (!rolling && shifting) EndShift();
    //}

    //public override void LoadEnemy(string loadID)
    //{
    //    base.LoadEnemy(loadID);

    //    loadID = "Enemy" + loadID;
    //    rolling = SaveManager.GetBool(loadID + "Rolling");
    //}
}
