using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        // DISABLE UI HERE
        // LOCK PLAYER INPUTS HERE

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

        // ENABLE UI HERE
        // UNLOCK PLAYER INPUTS HERE
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
        
	}
}

public class UIEvent : Event
{
    public override void TriggerEvent()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class QuestLogEntryEvent : Event
{
    public override void TriggerEvent()
    {

    }
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
