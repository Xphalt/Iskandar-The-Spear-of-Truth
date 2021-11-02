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

    public Inventory Storage;

    public SaveData(PlayerStats playerstats)
    {
        health = playerstats.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
    }

    public SaveData(InventoryObject_Sal playerstats)
    {
        Storage = playerstats.Storage;
    }
}

