using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan S script

[System.Serializable]

public class SaveData
{
    Scene m_Scene;
    public string scenename;
    public float health;
    public float X;
    public float Y;
    public float Z;

    public Inventory Storage;

    public SaveData(PlayerStats playerstats)
    {
        health = playerstats.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
        X = playerstats.X;
        Y = playerstats.Y;
        Z = playerstats.Z;
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

    public SaveData(EnemyBase enemy)
    {
        //enemy.isDead;
    }

    public SaveData(QuestLogManager QL)
    {
        //QL.allquests;
    }


    /// extra functions needed to be added
    //player position
    //if enemies are dead
    //if chest open
    //if pot destroyed
    //quest log / progression
}

