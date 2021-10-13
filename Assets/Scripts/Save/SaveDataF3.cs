using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan script

[System.Serializable]

public class SaveDataF3
{
    Scene m_Scene;
    public string scenename;
    public float health;
    //public bool load;

    public SaveDataF3(CharacterStats playerstatsf3)
    {
        health = playerstatsf3.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
    }
}

