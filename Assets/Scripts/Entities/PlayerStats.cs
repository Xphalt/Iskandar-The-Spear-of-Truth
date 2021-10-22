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

    //Morgan's Save Edits
    public void SaveStatsf1()
    {
        SaveManager.SavePlayerStatsf1(this);
    }

    public void SaveStatsf2()
    {
        SaveManager.SavePlayerStatsf2(this);
    }

    public void SaveStatsf3()
    {
        SaveManager.SavePlayerStatsf3(this);
    }

    public void LoadStatsf1()
    {
        SaveDataF1 saveDataf1 = SaveManager.LoadPlayerStatsf1();
        health = saveDataf1.healthf1;
    }

    public void LoadStatsf2()
    {
        SaveDataF2 saveDataf2 = SaveManager.LoadPlayerStatsf2();
        health = saveDataf2.healthf2;
    }

    public void LoadStatsf3()
    {
        SaveDataF3 saveDataf3 = SaveManager.LoadPlayerStatsf3();
        health = saveDataf3.healthf3;
    }

}
