using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveDataAssistant : MonoBehaviour
{
    internal int currentSaveFileID = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
