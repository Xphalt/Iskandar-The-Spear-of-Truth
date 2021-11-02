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
                case "Gem Pot":
                    database[i].OnUseCurrent += UseGemsPot;
                    break;
            } 
        }
    }
     

    //Usefull variables
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
    public void UseBraceletOfScouting()
    { 
        Vector3 newPos = playerCombat.swordObject.transform.position;
        newPos.y = playerstats.transform.position.y;
        playerstats.transform.position = newPos;
    }
     
    public void UseGoggles()
    { 
        if(playerstats.listOfObjs != null && playerstats.listOfObjs.activeSelf) playerstats.listOfObjs.SetActive(false);
    }
    public void UseGogglesUndo()
    { 
        if (playerstats.listOfObjs != null && !playerstats.listOfObjs.activeSelf) playerstats.listOfObjs.SetActive(true);
    }
     
    public void UseRingOfVitality()
    { 
        current += Time.deltaTime; 
        if (current > regenerationInterval)
        {
            if (playerstats.health < playerstats.MAX_HEALTH) //Magic number (variable needed);
                playerstats.health += healingValue;

            current = 0;
        }
    }
     
    public void UseBraceletOfTheLifeStealers()
    { 
        if (playerstats.health < playerstats.MAX_HEALTH) //Magic number (variable needed);
            playerstats.health += healingValue;
    }

    public void UseGemsPot()
    {
        playerstats.Gems += gemsValue;
    }

    public void UseBomb()
    {

    }
    #endregion
}
