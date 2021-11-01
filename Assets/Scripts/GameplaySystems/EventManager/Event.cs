using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "EventManager/Event")]
[System.Serializable]
public abstract class Event : ScriptableObject
{
    public abstract void TriggerEvent();

    public int testInt;
    public bool testBool;
    public float testFloat;
}
