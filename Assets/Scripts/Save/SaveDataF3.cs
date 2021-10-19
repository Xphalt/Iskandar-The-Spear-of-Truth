using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan S script

[System.Serializable]

public class SaveDataF3
{
    Scene m_Scene;
    public string scenename;
    public float healthf3;

    public Inventory Storagef3;

    public SaveDataF3(PlayerStats playerstatsf3)
    {
        healthf3 = playerstatsf3.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
    }

    public SaveDataF3(InventoryObject_Sal playerstatsf3)
    {
        Storagef3 = playerstatsf3.Storage;
    }
}

