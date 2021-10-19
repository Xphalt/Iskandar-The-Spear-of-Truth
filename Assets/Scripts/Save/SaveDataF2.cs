using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan S script

[System.Serializable]

public class SaveDataF2
{
    Scene m_Scene;
    public string scenename;
    public float healthf2;

    public Inventory Storagef2;

    public SaveDataF2(PlayerStats playerstatsf2)
    {
        healthf2 = playerstatsf2.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
    }

    public SaveDataF2(InventoryObject_Sal playerstatsf2)
    {
        Storagef2 = playerstatsf2.Storage;
    }
}

