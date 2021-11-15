using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan S script
[System.Serializable]
public class SaveData
{
    //PlayerStats Playerstats;
    Scene m_Scene;
    public string scenename;
    public float health;
    public float X;
    public float Y;
    public float Z;

    public Inventory Storage;

    //List<EnemyData> enemyDataList = new List<EnemyData>();

    public SaveData(PlayerStats playerstats)
    {
        health = playerstats.health;
        X = playerstats.X;
        Y = playerstats.Y;
        Z = playerstats.Z;
        //m_Scene = SceneManager.GetActiveScene();
        //scenename = m_Scene.name;

        //SaveEnemies();
    }

    public SaveData()
    {
    }

    public SaveData(InventoryObject_Sal playerstats)
    {
        Storage = playerstats.Storage;
    }

    public SaveData(LootChest_Jerzy lootchest)
    {
        //lootchest.isInteractable
    }

    public SaveData(ScrDestructablePot pot)
    {
        //pot.idfk
    }

    /*public SaveData(EnemyBase enemy)
    {
        //enemy.isDead;
    }*/

    // Morgan S
    /*public void SaveEnemies()
    {
        EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();

        foreach (EnemyBase enemy in enemies)
        {
            SaveEnemy(enemy);
        }

    }

    // Morgan S
    public void SaveEnemy(EnemyBase enemy)
    {
        EnemyData enemydata = new EnemyData();
        enemydata.isDead = enemy.getIsDead();
        // Get position of enemy and store it in enemyData
        enemyDataList.Add(enemydata);
    }

    public SaveData(QuestLogManager QL)
    {
        //QL.allquests;
    }

    [System.Serializable]
    public class EnemyData : SaveData
    {
        public bool isDead;*/
        // Declare new variables to hold position of enemy

        /*
        Then in the load code need to 
        1. loop through all EnemyDataList entries
        2. Get enemy at each position
        3. Set isDead for that enemy
        */
    }


    /// extra functions needed to be added
    //player position (done)
    //if enemies are dead (attempted... wondering if save system isnt fully working judging from player position upon scene reload)
    //if chest open
    //if pot destroyed
    //quest log / progression
    //check if text is done
    //stop text from loading again if in a returning room


    //if questname.IsQuestActive = true

    /*public void SaveStats(int num)
    {
        if QuestName.IsQuestActive = true
    {
        
    }
    }


    foreach ()
    {

    }
    
}*/
