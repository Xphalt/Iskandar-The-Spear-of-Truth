using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string NPC_Name;

    [TextArea(1, 10)]
    public string[] Sentences;
}