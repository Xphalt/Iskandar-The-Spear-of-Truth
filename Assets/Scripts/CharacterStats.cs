using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float health;
    public int attackDamage;
    [SerializeField]
    private bool _isPlayer = false;

    public void TakeDamage(int amt)
    {
        health -= amt;
        if(_isPlayer)
        {
            UIManager.instance.UpdateHealthBar(-amt);
        }
        
        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            // death
        }     
    }

    public void DealDamage(GameObject character)
    {
        character.GetComponent<CharacterStats>().TakeDamage(attackDamage);
    }
}
