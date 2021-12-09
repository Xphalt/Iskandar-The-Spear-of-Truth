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
    public List<QuestSave> Quests = new List<QuestSave>();

    public List<List<bool>> savedEnemies = new List<List<bool>>();
    public List<List<bool>> savedChests = new List<List<bool>>();
    public List<List<bool>> savedPots = new List<List<bool>>();
    public List<List<bool>> savedDialogue = new List<List<bool>>();
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
        gemcount = playerstats.gems;
        LastFileSaved = playerstats.SaveNum;
        totallynotevents = playerstats.totallynotevents;
        savedPots = playerstats.savedPots;
        savedChests = playerstats.savedChests;
        savedEnemies = playerstats.savedEnemies;
        savedDialogue = playerstats.savedDialogue;
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
        enemies = enemies.OrderBy(e => e.name).ThenBy(e => e.transform.position.x).ThenBy(e => e.transform.position.y).ThenBy(e => e.transform.position.z).ToList();
        foreach (EnemyStats enemy in enemies) savedEnemies[sceneEventIndex].Add(enemy.isDead);

        //list save chests
        List<LootChest_Jerzy> chests = GameObject.FindObjectsOfType<LootChest_Jerzy>(true).ToList();
        chests = chests.OrderBy(c => c.name).ThenBy(c => c.transform.position.x).ThenBy(c => c.transform.position.y).ThenBy(c => c.transform.position.z).ToList();
        foreach (LootChest_Jerzy chest in chests) savedChests[sceneEventIndex].Add(chest.isInteractable);

        //list save pots
        List<ScrDestructablePot> pots = GameObject.FindObjectsOfType<ScrDestructablePot>(true).ToList();
        pots = pots.OrderBy(p => p.name).ThenBy(p => p.transform.position.x).ThenBy(p => p.transform.position.y).ThenBy(p => p.transform.position.z).ToList();
        foreach (ScrDestructablePot pot in pots) savedPots[sceneEventIndex].Add(pot.destroyed);

        List<DialogueTrigger> dialogues = GameObject.FindObjectsOfType<DialogueTrigger>(true).ToList();
        dialogues = dialogues.OrderBy(d => d.name).ThenBy(d => d.transform.position.x).ThenBy(d => d.transform.position.y).ThenBy(d => d.transform.position.z).ToList();
        for (int d = 0; d < dialogues.Count; d++) savedDialogue[sceneEventIndex].Add(dialogues[d].hasPlayed);

        EventManager[] managers = GameObject.FindObjectsOfType<EventManager>(true);
        for (int em = 0; em < managers.Length; em++)
            for (int a = 0; a < managers[em].getamountofactions(); a++) totallynotevents[sceneEventIndex].Add(managers[em].getCompleted(a));
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
        for (int q = 0; q < QL.StartedQuests.Count; q++)
        {
            Quests.Add(new QuestSave());
            Quests[q].IsQuestActive = QL.StartedQuests[q].IsQuestActive;
            Quests[q].QuestName = QL.StartedQuests[q].QuestName;
            Quests[q].QuestDescription= QL.StartedQuests[q].QuestDescription;
        }
    }

    public SaveData(EnemyStats enemy)
    {
        //EnemyisDead = enemy.isDead;
    }
}