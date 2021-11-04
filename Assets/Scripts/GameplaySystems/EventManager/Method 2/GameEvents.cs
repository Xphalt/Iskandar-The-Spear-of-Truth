using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Morgan S Script
// Contact me if ya need any help with understanding any of this part / place

public class GameEvents : MonoBehaviour
{
    public static GameEvents current; // the current game event
    //public PlayerInput _playerInput ;

    private void Awake()
    {
        current = this; // make the current game event this object upon game/room start
        //_playerInput = FindObjectOfType<PlayerInput>();
    }


    //Event 1 (Open Door)
    public event Action<int> onDoorwayTriggerEnter; // Registers this as a public action, the int applies the number based ID (identifier)
    public void DoorwayTriggerEnter(int id)
    {
        if (onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter(id);
        }
    }

    //Event 2 (Exit Door)
    public event Action<int> onDoorwayTriggerExit;
    public void DoorwayTriggerExit(int id)
    {
        if (onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit(id);
        }
    }

    //Event 3 (set player health)
    public event Action<int> onPlayerHealthSet;
    public void PlayerHealthSet(int sethealth)
    {
        if (onPlayerHealthSet != null)
        {
            onPlayerHealthSet(sethealth);
        }
    }

    //Event 4 (Lock Player Inputs)
    public event Action onLockPlayerInputs; // might want this one to be repetitively spammed as closing the pause menu may mess with this...
    public void LockPlayerInputs()
    {
        if (onLockPlayerInputs != null)
        {
            onLockPlayerInputs();
        }
    }

    //Event 5 (Unlock Player Inputs)
    public event Action onUnLockPlayerInputs;
    public void UnLockPlayerInputs()
    {
        if (onUnLockPlayerInputs != null)
        {
            onUnLockPlayerInputs();
        }
    }

    //Event 6 (Disable UI)
    public event Action onDisableUI; // might want this one to be repetitively spammed as closing the pause menu may mess with this...
    public void DisableUI()
    {
        if (onDisableUI != null)
        {
            onDisableUI();
        }
    }

    //Event 7 (Enable UI)
    public event Action onEnableUI;
    public void EnableUI()
    {
        if (onEnableUI != null)
        {
            onEnableUI();
        }
    }

    //Event 8 (enemy stops attacking)
    public event Action onStopAttacking;
    public void StopAttacking()
    {
        if (onStopAttacking != null)
        {
            onStopAttacking();
        }
    }

    //Event 9 (enemy continues attacking)
    public event Action onContinueAttacking;
    public void ContinueAttacking()
    {
        if (onContinueAttacking != null)
        {
            onContinueAttacking();
        }
    }

    //Event 10 (Prevent Player interaction)
    public event Action onPreventPlayerInteraction;
    public void PreventPlayerInteraction()
    {
        if (onPreventPlayerInteraction != null)
        {
            onPreventPlayerInteraction();
        }
    }

    //Event 11 (Allow Player interaction)
    public event Action onAllowPlayerInteraction;
    public void AllowPlayerInteraction()
    {
        if (onAllowPlayerInteraction != null)
        {
            onAllowPlayerInteraction();
        }
    }

    //Event 12 (NPC Dialogue)
    public event Action onNPCDialogue;
    public void NPCDialogue()
    {
        if (onNPCDialogue != null)
        {
            onNPCDialogue();
            print("im working");
        }
    }



    //set this to private so it can't be accidentally assigned twice
    private Func<List<GameObject>> onRequestListOfDoors;
    public void SetOnRequestListOfDoors(Func<List<GameObject>> returnEvent)
    {
        onRequestListOfDoors = returnEvent;
    }

    public List<GameObject> RequestListOfDoors()
    {
        if (onRequestListOfDoors != null)
        {
            return onRequestListOfDoors();
        }

        return null;
    }
}

