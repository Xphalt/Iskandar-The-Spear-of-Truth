using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float health;
    public int attackDamage;
    public float takeDamageDelay;

    private float timeSinceTakenDamage;

    private SoundPlayer sfx;

    [SerializeField]
    private bool _isPlayer = false;

    private void Start()
    {
        sfx = GetComponentInParent<SoundPlayer>();
    }

    private void FixedUpdate()
    {
        timeSinceTakenDamage += Time.deltaTime;
    }


    public void TakeDamage(int amt)
    {
        if(timeSinceTakenDamage >= takeDamageDelay)
        {
            timeSinceTakenDamage = 0;
            health -= amt;
            //sfx.PlayAudio();
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

}
