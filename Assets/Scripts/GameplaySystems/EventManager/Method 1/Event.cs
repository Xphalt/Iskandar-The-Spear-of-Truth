using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public abstract class Event
{
    public abstract void TriggerEvent();
    [HideInInspector] public bool IsComplete = false;
    public bool ReplayOnload = false;
}

[System.Serializable]
public class DoorLockEvent : Event
{
    public override void TriggerEvent()
    {
        doorScript.Locked = lockDoor;
        IsComplete = true;
	}

    [SerializeReference] public ScrDoor1 doorScript;
    [SerializeReference] public bool lockDoor;
}

public class OpenCloseDoorEvent : Event
{
	public override void TriggerEvent()
	{
        doorScript.SwapDoor();
        IsComplete = true;
	} 

    [SerializeReference] public ScrDoor1 doorScript;
}

[System.Serializable]
public class SpawnEntityEvent : Event
{
    public override void TriggerEvent()
    {
        foreach(GameObject gameObject in _objectsToActivate)
        {
            gameObject.SetActive(_activateObjects);
            IsComplete = true;
		}
    } 

    [SerializeReference] private List<GameObject> _objectsToActivate;
    [SerializeReference] private bool _activateObjects;
}

#region PanCameraEvent

[System.Serializable]
public abstract class PanCameraEvent : Event
{
    [SerializeReference] protected CameraMove _cameraPanScript;
    [SerializeReference] protected Camera _playerCamera;
    [SerializeReference] protected Camera _panCamera;
    [SerializeReference] protected float _cameraPanSpeed;

	public override void TriggerEvent()
	{
        _playerCamera.enabled = false;
        _panCamera.enabled = true;
        _cameraPanScript.panSpeed = _cameraPanSpeed;
        // start coroutine to reenable camera at end of pan
	}
}

[System.Serializable]
public class PanCameraWithTargetVectorEvent : PanCameraEvent
{
	public override void TriggerEvent()
	{
        GameEvents.current.DisableUI();
        GameEvents.current.LockPlayerInputs();

        // swap cameras
        _playerCamera.enabled = false;
        _panCamera.enabled = true;

        _cameraPanScript.StartPan(_targetVector, _linger);

        // create SwapActiveCameraAfterPanObject to start a coroutine to reenable
        // UI after camera pan is complete
        GameObject coroutineObject = new GameObject();
        coroutineObject.AddComponent<SwapActiveCameraAfterPanObject>();

        // SwapActiveCameraAfterPanObject spawnedObjectScript = GameObject.Instantiate(coroutineObject).GetComponent<SwapActiveCameraAfterPanObject>();
        SwapActiveCameraAfterPanObject spawnedObjectScript = coroutineObject.GetComponent<SwapActiveCameraAfterPanObject>();
        spawnedObjectScript.playerCamera = _playerCamera;
        spawnedObjectScript.panCamera = _panCamera;
        spawnedObjectScript.CameraPanEvent = this;
        spawnedObjectScript.timeToPanFor = _cameraPanScript.TotalPanDuration;
        
	} 

    [SerializeReference] private Vector3 _targetVector = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeReference] private float _linger = -1f; 
}

public class SwapActiveCameraAfterPanObject : MonoBehaviour
{
    public Camera playerCamera;
    public Camera panCamera;
    public float timeToPanFor;
    public Event CameraPanEvent;

    public void Start()
    {
        StartCoroutine(SwapActiveCameraAfterPan());
	}

    private IEnumerator SwapActiveCameraAfterPan()
    {
        yield return new WaitForSeconds(timeToPanFor);
      
        panCamera.enabled = false;
        playerCamera.enabled = true;
        CameraPanEvent.IsComplete = true;

        GameEvents.current.EnableUI();
        GameEvents.current.UnLockPlayerInputs();
    }
}

[System.Serializable]
public class PanCameraWithTargetTransformEvent : PanCameraEvent
{
    public override void TriggerEvent()
    {
        GameEvents.current.DisableUI();
        GameEvents.current.LockPlayerInputs();

        // swap cameras
        _playerCamera.enabled = false;
        _panCamera.enabled = true;

        _cameraPanScript.StartPan(_targetTransform, _linger);

        // create SwapActiveCameraAfterPanObject to start a coroutine to reenable
        // UI after camera pan is complete
        GameObject coroutineObject = new GameObject();
        coroutineObject.AddComponent<SwapActiveCameraAfterPanObject>();

        // SwapActiveCameraAfterPanObject spawnedObjectScript = GameObject.Instantiate(coroutineObject).GetComponent<SwapActiveCameraAfterPanObject>();
        SwapActiveCameraAfterPanObject spawnedObjectScript = coroutineObject.GetComponent<SwapActiveCameraAfterPanObject>();
        spawnedObjectScript.playerCamera = _playerCamera;
        spawnedObjectScript.panCamera = _panCamera;
        spawnedObjectScript.CameraPanEvent = this;
        spawnedObjectScript.timeToPanFor = _cameraPanScript.TotalPanDuration;

    }

    [SerializeReference] private Transform _targetTransform;
    [SerializeReference] private float _linger = -1f;
}

#endregion

public class LockPlayerInputsEvent : Event
{
    public override void TriggerEvent()
    {
        if (_lockInputs)
        {
            GameEvents.current.LockPlayerInputs();
            Debug.Log("locking");
        }
        else
        {
            GameEvents.current.UnLockPlayerInputs();
		}

        IsComplete = true;

	} 

    [SerializeReference] private bool _lockInputs;
}

public class UIEvent : Event
{
    public override void TriggerEvent()
    {
        if (_disableUI)
        {
            GameEvents.current.DisableUI();
        }
        else
        {
            GameEvents.current.EnableUI();
		}

        IsComplete = true;

    }
     
    [SerializeReference] private bool _disableUI;
}

public class PreventPlayerInteractionEvent : Event
{
	public override void TriggerEvent()
	{
		if(_disablePlayerInteraction)
        {
            GameEvents.current.PreventPlayerInteraction();
		}
        else
        {
            GameEvents.current.AllowPlayerInteraction();
		}

	} 

    [SerializeReference] private bool _disablePlayerInteraction;
}

[System.Serializable]
public class QuestLogEntryEvent : Event
{
    public override void TriggerEvent()
    {
        questLog.AddQuest(quest);
    }

    [SerializeReference] private QuestLogManager questLog;
    [SerializeReference] private QuestObject quest;
}

//---------------- Sal Changes ----------------
public class FadeInOutScreen : Event
{
    public override void TriggerEvent()
    {
        fadeScreen.SetTrigger(fadeHash);
    } 

    [SerializeReference] private Animator fadeScreen;
    private int fadeHash = Animator.StringToHash("StartTransition");
}

public class StartDialogue : Event
{
    public override void TriggerEvent()
    {
        if(dialogue != null && convCollider != null)
        {
            GameEvents.current.StopAttacking();
            Debug.Log("They should stop attacking");
            GameObject.FindObjectOfType<DialogueManager>().DialoguePanel.SetActive(true);
            GameObject.FindObjectOfType<DialogueManager>().StartDialogue(convCollider, dialogue);
        }
    }

    [SerializeReference]private Collider convCollider;
    [SerializeReference]private NewConversation dialogue;
}

public class SpawnEnemyEvent : Event
{
    public override void TriggerEvent()
    {
        GameObject.Instantiate(enemyToSpawn, spawnPos.position, spawnPos.rotation);
    } 

    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject enemyToSpawn;
}

public class ChangeSceneEvent : Event
{
    public override void TriggerEvent()
    {
        player.SaveStats();
        SceneManager.LoadScene(sceneIndex);    
    }

    [SerializeField] private PlayerStats player;
    [SerializeField] private int sceneIndex;
}

//Matt's changes
public class TeleportObjectEvent : Event
{
    public override void TriggerEvent()
    {
        objectToTeleport.transform.position = teleportPos.position;
    }

    [SerializeField] GameObject objectToTeleport;
    [SerializeField] Transform teleportPos;
}

public class AddItem : Event
{
    public override void TriggerEvent()
    {
        inventory.AddItem(itemToAdd.data, amount);
    }

    [SerializeField] InventoryObject_Sal inventory;
    [SerializeField] ItemObject_Sal itemToAdd;
    [SerializeField] int amount;
}

public class EquipWeapon : Event
{
    public override void TriggerEvent()
    {
        if(equipment.GetSlots[(int)EquipSlot.SwordSlot].item.id > -1)
            inventory.AddItem(equipment.GetSlots[(int)EquipSlot.SwordSlot].item, 1);
        equipment.GetSlots[(int)EquipSlot.SwordSlot].UpdateSlot(itemToEquip.data, 1);
    }

    [SerializeField] InventoryObject_Sal equipment;
    [SerializeField] InventoryObject_Sal inventory;
    [SerializeField] ItemObject_Sal itemToEquip;
}

public class OpenShop : Event
{
    public override void TriggerEvent()
    {
        GameObject.FindObjectOfType<ShopManager>().OpenShop();
    }
}

public class RemoveItem : Event
{
    public override void TriggerEvent()
    {
        InventorySlot slot = inventory.FindItemOnInventory(itemToRemove.data); 
        if (slot != null)  
        {
            if (slot.amount == amount) //Remove item 
                inventory.RemoveItem(itemToRemove.data);
            else //Remove amount
                slot.AddAmount(-amount);  
        } 
    }

    [SerializeField] InventoryObject_Sal inventory;
    [SerializeField] ItemObject_Sal itemToRemove;
    [SerializeField] int amount;
}

public class CompleteLevel : Event
{
    public override void TriggerEvent()
    {
        VillageEventsStaticVariables.UpdateVillage(level);
    }

    [SerializeField] VillageEventsStaticVariables.VillageEventStages level;
}

public class PlayAnimation : Event
{
    public override void TriggerEvent()
    {
        foreach(Animation animation in animations) animation.Play();
    }

    [SerializeField] List<Animation> animations = new List<Animation>();
}

