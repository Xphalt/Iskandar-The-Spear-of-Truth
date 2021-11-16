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
    public float xpos;
    public float ypos;
    public float zpos;
    public int gemcount;
    public bool EnemyisDead;

    public Inventory Storage;

    public List<int> enemylist = new List<int>();
    public List<int> enemydeadlist = new List<int>();

    //public List<EnemyData> enemyDataList = new List<EnemyData>();

    public SaveData(PlayerStats playerstats)
    {
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
        health = playerstats.health;
        xpos = playerstats.X;
        ypos = playerstats.Y;
        zpos = playerstats.Z;
        gemcount = playerstats.gems;

        //Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length );

        //list save enemies
        foreach (var enemy in GameObject.FindObjectsOfType<EnemyStats>())
        {
            enemylist.Add(enemy.gameObject.GetInstanceID());
            if (enemy.isDead)
            {
                enemydeadlist.Add(enemy.gameObject.GetInstanceID());
            }
        }
        Debug.Log(enemydeadlist);

        //outline for saving the enemies
        /*foreach (GameObject enemy in enemyarray)
        {
            Debug.Log(enemy.GetInstanceID());
            if (enemy.GetComponent<EnemyStats>().isDead)
            {
                UnityEngine.Object.Destroy(enemy);
            }
        }

        foreach (GameObject enemy in enemyarray)
        {
            if (enemy.GetComponent<EnemyStats>().isDead)
            {
                EnemyisDead = true;
            }
        }*/



        //reminder make thing above^ check on load input
        //reminder check when enemy is dead

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

    public SaveData(QuestLogManager QL)
    {
        //QL.allquests;
    }

    public SaveData(EnemyStats enemy)
    {
        //EnemyisDead = enemy.isDead;
    }

    // Morgan S
    /*public void SaveEnemy(EnemyBase enemy)
    {
        EnemyData enemydata = new EnemyData();
        enemydata.isDead = enemy.getIsDead();
        // Get position of enemy and store it in enemyData
        enemyDataList.Add(enemydata);
    }

    // Morgan S
    public void SaveEnemies()
    {
        EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();

        foreach (EnemyBase enemy in enemies)
        {
            SaveEnemy(enemy);
        }

    }

    [System.Serializable]
    public class EnemyData : SaveData
    {
        public bool isDead;
        
        // Declare new variables to hold position of enemy

        /*
        Then in the load code need to 
        1. loop through all EnemyDataList entries
        2. Get enemy at each position
        3. Set isDead for that enemy
        issue... enemies move...
        
    }*/
}


    /// extra functions needed to be added
    //if enemies are dead - WORKS BUT NEED TO ADD ID NUMBER TO EACH ENEMY
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