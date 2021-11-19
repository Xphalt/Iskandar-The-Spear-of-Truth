using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public abstract class Event
{
    public abstract void TriggerEvent(); 
}

[System.Serializable]
public class DoorLockEvent : Event
{
    public override void TriggerEvent()
    {
        doorScript.Locked = lockDoor;
	}

    [SerializeReference] public ScrDoor1 doorScript;
    [SerializeReference] public bool lockDoor;
}

public class OpenCloseDoorEvent : Event
{
	public override void TriggerEvent()
	{
        doorScript.SwapDoor();
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

	public override void TriggerEvent()
	{
        _playerCamera.enabled = false;
        _panCamera.enabled = true;

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

        SwapActiveCameraAfterPanObject spawnedObjectScript = GameObject.Instantiate(coroutineObject).GetComponent<SwapActiveCameraAfterPanObject>();
        spawnedObjectScript.playerCamera = _playerCamera;
        spawnedObjectScript.panCamera = _panCamera;
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

    public void Initiate()
    {
        StartCoroutine(SwapActiveCameraAfterPan());
	}

    private IEnumerator SwapActiveCameraAfterPan()
    {
        yield return new WaitForSeconds(timeToPanFor);
      
        panCamera.enabled = false;
        playerCamera.enabled = true;

        GameEvents.current.EnableUI();
        GameEvents.current.UnLockPlayerInputs();
    }
}

[System.Serializable]
public class PanCameraWithTargetTransformEvent : PanCameraEvent
{
	public override void TriggerEvent()
	{
        _cameraPanScript.StartPan(_targetTransform);
	}
     
    [SerializeReference] private Transform _targetTransform;
}

#endregion

public class LockPlayerInputsEvent : Event
{
    public override void TriggerEvent()
    {
        if (_lockInputs)
        {
            GameEvents.current.LockPlayerInputs();
        }
        else
        {
            GameEvents.current.UnLockPlayerInputs();
		}
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
        SceneManager.LoadScene(sceneIndex);    
    } 

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

//[System.Serializable]
//public class SetPlayerInvulnerableEvent : Event
//{
//    public override void TriggerEvent()   
//    {

//    }
//}

//[System.Serializable]
//public class EndGameEvent : Event
//{
//    public override void TriggerEvent()
//    {

//    }
//}
