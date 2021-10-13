using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : StatsInterface
{

    private void Start()
    {
        sfx = GetComponentInParent<SoundPlayer>();
    }

    public override void TakeDamage(float amount)
    {
        health -= amount;
        sfx.PlayAudio();
        UIManager.instance.UpdateHealthBar((int)-amount);
        
        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }     
    }

    public override void DealDamage(StatsInterface target, float amount)
    {
        target.TakeDamage(amount);
    }
}
