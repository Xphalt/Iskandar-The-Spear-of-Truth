using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string NPC_Name;

    // Dominique 08-10-2021, Use localised sentences
    public LocalisationTableReference[] Sentences;
}