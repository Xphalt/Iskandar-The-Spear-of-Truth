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
                case "Bracers of Scouting":
                    database[i].OnUseBefore += UseBracersOfScouting; 
                    break;
                case "Goggles":
                    database[i].OnUseBefore += UseGoggles;
                    database[i].OnUseAfter += UseGogglesUndo; 
                    break;
                case "Ring Of Vitality":
                    database[i].OnUseBefore += UseRingOfVitality; 
                    break;
                case "Bracers Of The Life Stealers":
                    database[i].OnUseBefore += UseBracersOfTheLifeStealers; 
                    break;
            } 
        }
    }


    public void SetVariables(float p_Interval, float p_healingVL)
    {
        regenerationInterval = p_Interval;
        HealingValue = p_healingVL;
    }

    //Timer Stuff
    private float current;
    private float regenerationInterval;

    private float HealingValue; 

    #region Use Functions 
    public void UseBracersOfScouting()
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
                playerstats.health += HealingValue;

            current = 0;
        }
    }
     
    public void UseBracersOfTheLifeStealers()
    { 
        if (playerstats.health < playerstats.MAX_HEALTH) //Magic number (variable needed);
            playerstats.health += HealingValue;
    }
    #endregion
}
