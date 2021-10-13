using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{

    public float health;
    public int attackDamage;

    public string playerCurrentScene;

    private SoundPlayer sfx;

    [SerializeField]
    private bool _isPlayer = false;

    private void Start()
    {
        sfx = GetComponentInParent<SoundPlayer>();
        LoadStatsf1();
    }

    public void TakeDamage(int amt)
    {
        health -= amt;
        sfx.PlayAudio();
        if (_isPlayer)
        {
            UIManager.instance.UpdateHealthBar(-amt);
        }
        
        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            if (!_isPlayer) gameObject.SetActive(false);
        }     
    }

    public void DealDamage(GameObject character, int amt = 0)
    {
        if (amt == 0) amt = attackDamage;
        character.GetComponent<CharacterStats>().TakeDamage(amt);
    }

    public void DealDamage(GameObject character, float amt = 0)
    {
        if (amt == 0) amt = (float)attackDamage;
        character.GetComponent<CharacterStats>().TakeDamage((int)amt);
    }

    //morgan's save features edit
    public void SaveStatsf1()
    {
        if (_isPlayer)
        {
            SaveManager.SavePlayerStatsf1(this);
        }
    }

    public void LoadStatsf1()
    {
        SaveDataF1 saveDataf1 = SaveManager.LoadPlayerStatsf1();
        health = saveDataf1.healthf1;
    }

    public void SaveStatsf2()
    {
        if (_isPlayer)
        {
            SaveManager.SavePlayerStatsf2(this);
        }
    }

    public void LoadStatsf2()
    {
        SaveDataF2 saveDataf2 = SaveManager.LoadPlayerStatsf2();
        health = saveDataf2.healthf2;
    }

    public void SaveStatsf3()
    {
        if (_isPlayer)
        {
            SaveManager.SavePlayerStatsf3(this);
        }
    }

    public void LoadStatsf3()
    {
        SaveDataF3 saveDataf3 = SaveManager.LoadPlayerStatsf3();
        health = saveDataf3.healthf3;
    }

    private void OnDestroy()
    {
        SaveStatsf1();
    }

}
