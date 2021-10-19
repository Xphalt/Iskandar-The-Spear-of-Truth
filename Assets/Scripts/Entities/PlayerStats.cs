using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : StatsInterface
{

    private void Start()
    {
        sfx = GetComponentInParent<SoundPlayer>();
    }

    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if (scriptedKill) amount = health - 1;
        health -= amount;
        sfx.PlayAudio();
        UIManager.instance.UpdateHealthBar((int)-amount);
        
        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            gameObject.SetActive(false);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        target.TakeDamage(amount, scriptedKill);
    }
}
