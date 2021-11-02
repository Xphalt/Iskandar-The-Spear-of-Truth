using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Morgan S Script

public class TriggerArea : MonoBehaviour
{
    public int id;
    public int sethealth;
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.DoorwayTriggerEnter(id);
        GameEvents.current.PlayerHealthSet(sethealth);
    }

    private void OnTriggerExit(Collider other)
    {
        GameEvents.current.DoorwayTriggerExit(id);
    }
}