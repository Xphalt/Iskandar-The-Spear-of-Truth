using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<List<bool>> savedEnemies = new List<List<bool>>();
    public List<List<bool>> savedChests = new List<List<bool>>();
    public List<List<bool>> savedPots = new List<List<bool>>();
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
        savedPots = playerstats.savedPots;
        savedChests = playerstats.savedChests;
        savedEnemies = playerstats.savedEnemies;
        levelsComplete = VillageEventsStaticVariables.levelsComplete;

        for (int i = totallynotevents.Count; i < sceneEventIndex + 1; i++) totallynotevents.Add(new List<bool>());
        for (int i = savedPots.Count; i < sceneEventIndex + 1; i++) savedPots.Add(new List<bool>());
        for (int i = savedEnemies.Count; i < sceneEventIndex + 1; i++) savedEnemies.Add(new List<bool>());
        for (int i = savedChests.Count; i < sceneEventIndex + 1; i++) savedChests.Add(new List<bool>());

        totallynotevents[sceneEventIndex].Clear();
        savedPots[sceneEventIndex].Clear();
        savedEnemies[sceneEventIndex].Clear();
        savedChests[sceneEventIndex].Clear();
        //Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length );

        //list save enemies
        List<EnemyStats> enemies = GameObject.FindObjectsOfType<EnemyStats>(true).ToList();
        enemies = enemies.OrderBy(enemy => enemy.gameObject.GetInstanceID()).ToList();
        foreach (EnemyStats enemy in GameObject.FindObjectsOfType<EnemyStats>(true)) savedEnemies[sceneEventIndex].Add(enemy.isDead);

        //list save chests
        List<LootChest_Jerzy> chests = GameObject.FindObjectsOfType<LootChest_Jerzy>(true).ToList();
        chests = chests.OrderBy(chest => chest.gameObject.GetInstanceID()).ToList();
        foreach (LootChest_Jerzy chest in GameObject.FindObjectsOfType<LootChest_Jerzy>(true)) savedChests[sceneEventIndex].Add(chest.isInteractable);

        //list save pots
        List<ScrDestructablePot> pots = GameObject.FindObjectsOfType<ScrDestructablePot>(true).ToList();
        pots = pots.OrderBy(pot => pot.gameObject.GetInstanceID()).ToList();
        foreach (ScrDestructablePot pot in GameObject.FindObjectsOfType<ScrDestructablePot>(true)) savedPots[sceneEventIndex].Add(pot.destroyed);

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