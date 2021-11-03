using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Morgan S Script

public class GameEvents : MonoBehaviour
{
    public static GameEvents current; // the current game event

    public PlayerInput _playerInput ;

    private void Awake()
    {
        current = this; // make the current game event this object upon game/room start
        _playerInput = FindObjectOfType<PlayerInput>();
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
            _playerInput.enabled = false;
            if (_playerInput.enabled == false)
            {
                print("inputs are off");
            }
            print("inputs are sorta off");
        }
    }

    //Event 5 (Unlock Player Inputs)
    public event Action onUnLockPlayerInputs;
    public void UnLockPlayerInputs()
    {
        if (onUnLockPlayerInputs != null)
        {
            onUnLockPlayerInputs();
            _playerInput.enabled = true;
            if (_playerInput.enabled == true)
            {
                print("inputs are true");
            }
        }
    }

    //Event 6 (Disable UI)
    public event Action onDisableUI; // might want this one to be repetitively spammed as closing the pause menu may mess with this...
    public void DisableUI()
    {
        if (onDisableUI != null)
        {
            onDisableUI();
            print("im working");
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
            print("im ALSO working");
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

    //Event 10 (Prevent Player interaction???? issue is im geussing this excludes swinging the sword, its gonnna be way harder, if it was just disabling the interact button. period, that's ez)
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
            _playerInput.enabled = true;
            if (_playerInput.enabled == true)
            {
                print("i am false");
            }
        }
    }

    //Event 12 (NPC Dialogue)
    public event Action onNPCDialogue;
    public void NPCDialogue()
    {
        if (onNPCDialogue != null)
        {
            onNPCDialogue();
            onStopAttacking();
            onLockPlayerInputs();
            print("im working");
        }
    }

    //interaction or NPC script





    //set this to private so it cant be accidentally assigned twice
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

