using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveDataAssistant : MonoBehaviour
{
    internal int currentSaveFileID = 0;
    public float SFXVol = -1;
    public float MusicVol = 0;
    public float AmbienceVol = 0;

    private void Awake()
    {
        SaveDataAssistant[] sdas = GameObject.FindObjectsOfType<SaveDataAssistant>();

        if (sdas.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
