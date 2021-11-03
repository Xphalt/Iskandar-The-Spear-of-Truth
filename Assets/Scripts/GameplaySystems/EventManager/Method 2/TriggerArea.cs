using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Morgan S Script

public class TriggerArea : MonoBehaviour
{
    public int id;
    public int sethealth;
    public PlayerInput _playerInput ;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.DoorwayTriggerEnter(id);
        GameEvents.current.PlayerHealthSet(sethealth);
        GameEvents.current.DisableUI();
        //GameEvents.current.NPCDialogue();
        GameEvents.current.PreventPlayerInteraction();
        GameEvents.current.LockPlayerInputs();
        _playerInput.enabled = false;
    }


    private void OnTriggerExit(Collider other)
    {
        GameEvents.current.DoorwayTriggerExit(id);
        GameEvents.current.EnableUI();
        GameEvents.current.ContinueAttacking();
        GameEvents.current.UnLockPlayerInputs();
    }
}