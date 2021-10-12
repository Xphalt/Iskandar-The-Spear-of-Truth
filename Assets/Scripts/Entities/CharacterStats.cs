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
    public void SaveStats()
    {
        if (_isPlayer)
        {
            SaveManager.SavePlayerStatsf1(this);
        }
    }

    public void LoadStats()
    {
        //load = true;
        //SaveManager.SavePlayerStatsf1(this.load);
        SaveData saveData = SaveManager.LoadPlayerStatsf1();
        //SceneManager.LoadScene(saveData.scenename);
        //print(saveData.scenename);
        health = saveData.health;
    }

}
