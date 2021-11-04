using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Morgan S Script
// Contact me if ya need any help with understanding any of this part / place

public class TriggerArea : MonoBehaviour
{
    public int id;
    public int sethealth;
    //public PlayerInput _playerInput ;

    //private void Awake()
    //{
    //    _playerInput = FindObjectOfType<PlayerInput>();
    //}

    private void OnTriggerEnter(Collider other)
    {
        //GameEvents.current.DoorwayTriggerEnter(id);
        GameEvents.current.PlayerHealthSet(sethealth); // this works
        GameEvents.current.DisableUI(); // this works
        GameEvents.current.PreventPlayerInteraction(); // this works by the player_interaction script's definition of an interaction
        //GameEvents.current.StopAttacking();  // this works
        //GameEvents.current.LockPlayerInputs();
        GameEvents.current.NPCDialogue();
        print("trigger working");
    }


    private void OnTriggerExit(Collider other)
    {
        //GameEvents.current.DoorwayTriggerExit(id);
        GameEvents.current.EnableUI(); // this works
        //GameEvents.current.ContinueAttacking(); // this works
        GameEvents.current.AllowPlayerInteraction(); // this works by the player_interaction script's definition of an interaction
        //GameEvents.current.UnLockPlayerInputs();
    }
}