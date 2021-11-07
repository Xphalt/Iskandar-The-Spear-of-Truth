using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFunctions : MonoBehaviour
{
    private static UseFunctions instance;
    public static UseFunctions Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<UseFunctions>();
            return instance;
        }
    }
    //Variables needed for the functions 
    private PlayerStats playerstats;
    private PlayerCombat_Jerzy playerCombat;
    public GameObject bomb;
    public GameObject wand;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    private void Start()
    {    
        var database = FindObjectOfType<PlayerStats>().inventory.database.ItemObjects;
        playerstats = FindObjectOfType<PlayerStats>();
        playerCombat = FindObjectOfType<PlayerCombat_Jerzy>();
        for (int i = 0; i < database.Length; i++)
        {
            switch (database[i].name)
            {
                case "Bracelet of Scouting":
                    database[i].OnUseCurrent += UseBraceletOfScouting; 
                    break;
                case "Goggles":
                    database[i].OnUseCurrent += UseGoggles;
                    database[i].OnUseAfter += UseGogglesUndo; 
                    break;
                case "Ring Of Vitality":
                    database[i].OnUseCurrent += UseRingOfVitality; 
                    break;
                case "Necklace Of The Life Stealers":
                    database[i].OnUseCurrent += UseBraceletOfTheLifeStealers; 
                    break;
                case "Gem":
                    database[i].OnUseCurrent += UseGemsPot;
                    break;
                case "Bomb Bag":
                    database[i].OnUseCurrent += UseBombBag;
                    break;
                case "Wand of Magnetism":
                    database[i].OnUseCurrent += UseWandOfMagnetism;
                    break;
                case "Revival Gem":
                    database[i].OnUseCurrent += UseRevivalGem;
                    break;
                case "Small Potion":
                    database[i].OnUseCurrent += UsePotion;
                    break;
                case "Medium Potion":
                    database[i].OnUseCurrent += UsePotion;
                    break;
                case "Max Potion":
                    database[i].OnUseCurrent += UsePotion;
                    break;
            } 
        }
    }

   
    //Useful variables
    private float current;
    private float regenerationInterval;
    public float RegenerationInterval
    {
        get { return regenerationInterval; }
        set { regenerationInterval = value; }
    }

    private float healingValue;
    public float HealingValue
    {
        get { return healingValue; }
        set { healingValue = value; }
    }
    private int gemsValue;
    public int GemsValue
    {
        get { return gemsValue; }
        set { gemsValue = value; }
    }


    #region Use Functions 
    private void UseBraceletOfScouting()
    { 
        Vector3 newPos = playerCombat.swordObject.transform.position;
        newPos.y = playerstats.transform.position.y;
        playerstats.transform.position = newPos;
    } 

    private void UseGoggles()
    { 
        if(playerstats.listOfObjs != null && playerstats.listOfObjs.activeSelf) playerstats.listOfObjs.SetActive(false);
    }

    private void UseGogglesUndo()
    { 
        if (playerstats.listOfObjs != null && !playerstats.listOfObjs.activeSelf) playerstats.listOfObjs.SetActive(true);
    }

    private void UseRingOfVitality()
    { 
        current += Time.deltaTime; 
        if (current > regenerationInterval)
        {
            if (playerstats.health < playerstats.MAX_HEALTH) //Magic number (variable needed);
            {
                playerstats.health += healingValue;
                UIManager.instance.UpdateHealthBar((int)healingValue);
            }
            current = 0;
        } 
    }

    private void UseBraceletOfTheLifeStealers()
    { 
        if (playerstats.health < playerstats.MAX_HEALTH) //Magic number (variable needed);
            playerstats.health += healingValue;
    }

    private void UseGemsPot()
    {
        playerstats.Gems += gemsValue;
    }

    private void UseBombBag()
    {
        if(!FindObjectOfType<Bomb>())
        {
            //Item removal 
            if (playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].amount == 1)
                playerstats.equipment.RemoveItem(playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].item);
            else
                playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].AddAmount(-1);

            //spawning bomb
            Instantiate(bomb, playerstats.transform.position, Quaternion.identity);
        }
    }

    private void UseWandOfMagnetism()
    {
        if (!FindObjectOfType<MagneticWand>())
        {
            Instantiate(wand, playerstats.transform.localPosition, playerstats.transform.localRotation); 
        }
    }

    private void UseRevivalGem()
    { 
        //Item removal 
        playerstats.inventory.RemoveItem(playerstats.revivalGem.data);

        //Heal player
        playerstats.health = playerstats.MAX_HEALTH * (healingValue / 100);

        UIManager.instance.UpdateHealthBar((int)playerstats.health);
    }

    private void UsePotion()
    { 
        playerstats.health += healingValue;
        playerstats.health = Mathf.Clamp(playerstats.health, 0.0f, playerstats.MAX_HEALTH);
        //Update UI
        UIManager.instance.SetHealthBar((int)playerstats.health);

        //Item removal 
        if (playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].amount == 1)
            playerstats.equipment.RemoveItem(playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].item);
        else
            playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].AddAmount(-1);
    }
    #endregion
}
