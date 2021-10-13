using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// morgan script

[System.Serializable]

public class SaveDataF2
{
    Scene m_Scene;
    public string scenename;
    public float health;
    //public bool load;

    public SaveDataF2(CharacterStats playerstatsf2)
    {
        health = playerstatsf2.health;
        m_Scene = SceneManager.GetActiveScene();
        scenename = m_Scene.name;
    }
}

