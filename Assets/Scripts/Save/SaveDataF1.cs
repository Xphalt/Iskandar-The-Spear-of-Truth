using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan S script

[System.Serializable]

public class SaveDataF1
{
    Scene m_Scene;
    public string scenename;
    public float healthf1;

    public Inventory Storagef1;

    public SaveDataF1(PlayerStats playerstatsf1)
    {
        healthf1 = playerstatsf1.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
    }

    public SaveDataF1(InventoryObject_Sal playerstatsf1)
    {
        Storagef1 = playerstatsf1.Storage;
    }
}

