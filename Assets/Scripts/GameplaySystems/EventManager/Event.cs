using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public abstract class Event
{
    public abstract void TriggerEvent();

    [SerializeField] public int testInt;
    [SerializeField] public bool testBool;
    [SerializeField] public float testFloat;
}

[System.Serializable]
public class DoorEvent : Event
{
    public override void TriggerEvent()
    {
        
	}

    [SerializeField] public GameObject door;
    [SerializeField] public bool unlockDoor;
}