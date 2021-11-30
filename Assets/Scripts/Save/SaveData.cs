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
    internal int sceneEventIndex;
    
    public string scenename;
    public float health;
    public float xpos;
    public float ypos;
    public float zpos;
    public int gemcount;
    public bool EnemyisDead;
    public int LastFileSaved;
    public string playerName = "";

    public Inventory Storage;

    public List<int> enemylist = new List<int>();
    public List<int> enemydeadlist = new List<int>();
    public List<int> chestlist = new List<int>();
    public List<int> chestopenedlist = new List<int>();
    public List<int> potlist = new List<int>();
    public List<int> potbrokenlist = new List<int>();
    public List<List<bool>> totallynotevents = new List<List<bool>>();
    public List<bool> levelsComplete = new List<bool>();

    //public List<EventAction> totallynotcompletedevents = new List<EventAction>();

    //public List<EnemyData> enemyDataList = new List<EnemyData>();

    public SaveData(PlayerStats playerstats)
    {
        m_Scene = SceneManager.GetActiveScene();
        sceneEventIndex = m_Scene.buildIndex - 1; // -1 for menu scene
        scenename = m_Scene.name;
        health = playerstats.health;
        xpos = playerstats.X;
        ypos = playerstats.Y;
        zpos = playerstats.Z;
        gemcount = playerstats.gems;
        LastFileSaved = playerstats.SaveNum;
        totallynotevents = playerstats.totallynotevents;

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

        //list save chests
        foreach (var chest in GameObject.FindObjectsOfType<LootChest_Jerzy>())
        {
            chestlist.Add(chest.gameObject.GetInstanceID());
            if (!chest.isInteractable)
            {
                chestopenedlist.Add(chest.gameObject.GetInstanceID());
            }
        }

        //list save pots
        foreach (var pot in GameObject.FindObjectsOfType<ScrDestructablePot>())
        {
            potlist.Add(pot.gameObject.GetInstanceID());
            if (pot.destroyed)
            {
                potbrokenlist.Add(pot.gameObject.GetInstanceID());
            }
        }

        EventManager[] managers = GameObject.FindObjectsOfType<EventManager>(true);

        try
        {
            totallynotevents[sceneEventIndex].Clear();
        }
        catch  (System.Exception)  
        {
            totallynotevents.Add(new List<bool>());
        }
        for (int em = 0; em < managers.Length; em++)
        {
            for (int a = 0; a < managers[em].getamountofactions(); a++)
            {
                totallynotevents[sceneEventIndex].Add(managers[em].getCompleted(a));
            }
        }
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