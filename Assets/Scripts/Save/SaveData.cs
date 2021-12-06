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

    public List<List<bool>> enemydeadlist = new List<List<bool>>();
    public List<List<bool>> chestopenedlist = new List<List<bool>>();
    public List<List<bool>> potbrokenlist = new List<List<bool>>();
    public List<List<bool>> totallynotevents = new List<List<bool>>();
    public bool[] levelsComplete;

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
        potbrokenlist = playerstats.savedPots;
        chestopenedlist = playerstats.savedChests;
        enemydeadlist = playerstats.savedEnemies;
        levelsComplete = VillageEventsStaticVariables.levelsComplete;

        for (int i = totallynotevents.Count; i < sceneEventIndex + 1; i++) totallynotevents.Add(new List<bool>());
        for (int i = potbrokenlist.Count; i < sceneEventIndex + 1; i++) potbrokenlist.Add(new List<int>());
        for (int i = enemydeadlist.Count; i < sceneEventIndex + 1; i++) enemydeadlist.Add(new List<int>());
        for (int i = chestopenedlist.Count; i < sceneEventIndex + 1; i++) chestopenedlist.Add(new List<int>());

        totallynotevents[sceneEventIndex].Clear();
        potbrokenlist[sceneEventIndex].Clear();
        enemydeadlist[sceneEventIndex].Clear();
        chestopenedlist[sceneEventIndex].Clear();
        //Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length );

        //list save enemies
        foreach (EnemyStats enemy in GameObject.FindObjectsOfType<EnemyStats>(true))
            if (enemy.isDead) enemydeadlist[sceneEventIndex].Add(enemy.gameObject.GetInstanceID());

        //list save chests
        foreach (LootChest_Jerzy chest in GameObject.FindObjectsOfType<LootChest_Jerzy>(true))
            if (!chest.isInteractable) chestopenedlist[sceneEventIndex].Add(chest.gameObject.GetInstanceID());

        //list save pots
        foreach (ScrDestructablePot pot in GameObject.FindObjectsOfType<ScrDestructablePot>(true))
            if (pot.destroyed) potbrokenlist[sceneEventIndex].Add(pot.gameObject.GetInstanceID());

        //try
        //{
        //    totallynotevents[sceneEventIndex].Clear();
        //}
        //catch  (System.Exception)  
        //{
        //    totallynotevents.Add(new List<bool>());
        //}
        EventManager[] managers = GameObject.FindObjectsOfType<EventManager>(true);
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
}