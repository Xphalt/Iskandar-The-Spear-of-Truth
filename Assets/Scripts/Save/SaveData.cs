using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan script

[System.Serializable]

public class SaveData
{
    Scene m_Scene;
    public string scenename;
    public float health;
    //public bool load;

    public SaveData(CharacterStats playerstats)
    {
        health = playerstats.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
        //load = playerstats.load;
    }
}

