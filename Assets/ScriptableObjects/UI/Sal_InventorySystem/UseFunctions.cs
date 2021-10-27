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

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    private void Start()
    {    
        var database = FindObjectOfType<PlayerStats>().inventory.database.ItemObjects;
        var playerstats = FindObjectOfType<PlayerStats>();
        for (int i = 0; i < database.Length; i++)
        {
            switch (database[i].name)
            {
                case "Bracers of Scouting":
                    database[i].OnUseBefore += UseBracersOfScouting;
                    database[i].useParameters = new GameObject[] { playerstats.gameObject, playerstats.gameObject };
                    break;
                case "Goggles":
                    database[i].OnUseBefore += UseGoggles;
                    database[i].OnUseAfter += UseGogglesUndo;
                    database[i].useParameters = new GameObject[] { playerstats.listOfObjs };
                    break;
                case "Ring Of Vitality":
                    database[i].OnUseBefore += UseRingOfVitality;
                    database[i].useParameters = new GameObject[] { playerstats.gameObject };
                    break;
                case "Bracers Of The Life Stealers":
                    database[i].OnUseBefore += UseBracersOfTheLifeStealers;
                    database[i].useParameters = new GameObject[] { playerstats.gameObject };
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
    //0 == playerPos, 1 == teleportPos
    public void UseBracersOfScouting(params GameObject[] playerPosAndTeleport)
    { 
        Vector3 newPos = FindObjectOfType<PlayerCombat_Jerzy>().swordObject.transform.position;
        newPos.y = playerPosAndTeleport[0].transform.position.y;
        playerPosAndTeleport[0].transform.position = newPos;
    }

    //0 == List of objs
    public void UseGoggles(params GameObject[] obj)
    {
        obj = new GameObject[] { FindObjectOfType<PlayerStats>().listOfObjs };
        if(obj[0] && obj[0].activeSelf) obj[0].SetActive(false);
    }
    public void UseGogglesUndo(params GameObject[] obj)
    {
        obj = new GameObject[] { FindObjectOfType<PlayerStats>().listOfObjs };
        if (obj[0] && !obj[0].activeSelf) obj[0].SetActive(true);
    }

    //0 == Player
    public void UseRingOfVitality(params GameObject[] obj)
    {
        var stats = obj[0].GetComponent<PlayerStats>();

        current += Time.deltaTime;
        if (current > regenerationInterval)
        {
            if (stats.health < stats.MAX_HEALTH) //Magic number (variable needed);
                stats.health += HealingValue;

            current = 0;
        }
    }

    //0 == Player
    public void UseBracersOfTheLifeStealers(params GameObject[] obj)
    {
        var stats = obj[0].GetComponent<PlayerStats>();
        if (stats.health < stats.MAX_HEALTH) //Magic number (variable needed);
            stats.health += HealingValue;
    }
    #endregion
}
