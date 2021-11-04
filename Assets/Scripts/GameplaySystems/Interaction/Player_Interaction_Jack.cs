using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the player side of interaction logic. To be called from the player controller script.
public class Player_Interaction_Jack : MonoBehaviour
{
    [SerializeField]
    private float _interactionRadius = 5.0f;

    private const int INTERACTABLE_LAYERMASK = 1 << 8;
    private const int TARGETABLE_LAYERMASK = 1 << 7;

    private Collider[] _interactableColliders;
    private Collider _nearestInteractableCollider = null;
    private Collider _lastNearestInteractableCollider = null;

    public float interactCooldown;
    float timeSinceLastInteract;

    //morgan's event system edit
    public bool prevention = false;
    public PlayerInput _playerInput;

    private void Start()
    {
        GameEvents.current.onPreventPlayerInteraction += OnPreventPlayerInteraction;
        GameEvents.current.onAllowPlayerInteraction += OnAllowPlayerInteraction;
        GameEvents.current.onNPCDialogue += OnNPCDialogue;
        GameEvents.current.onLockPlayerInputs += OnLockPlayerInputs;
        GameEvents.current.onUnLockPlayerInputs += OnUnLockPlayerInputs;

    }

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
    }
    public void OnPreventPlayerInteraction()
    {
        prevention = true;
    }

    public void OnAllowPlayerInteraction()
    {
        prevention = false;
    }

    public void OnLockPlayerInputs()
    {
        _playerInput.enabled = false;
        print("the state of input is " + _playerInput.enabled);
    }

    public void OnUnLockPlayerInputs()
    {
        _playerInput.enabled = true;
    }

    public void OnNPCDialogue()
    {
        GameEvents.current.LockPlayerInputs();
        GameEvents.current.StopAttacking();
    }

    // end of morgan's edits


    // Update is called once per frame
    void Update()
    {
        GetInteractables();
    }

    // Puts all interactable object colliders within range into _interactableColliders
    private void GetInteractables()
    {
        _interactableColliders = Physics.OverlapSphere(transform.position, _interactionRadius, INTERACTABLE_LAYERMASK);

        float _nearestInteractableDistanceSqr = _interactionRadius * _interactionRadius;
        _nearestInteractableCollider = null;

        foreach (Collider collider in _interactableColliders)
        {
            float xDistance = collider.transform.position.x - transform.position.x;
            float zDistance = collider.transform.position.z - transform.position.z;

            float distance = xDistance * xDistance + zDistance * zDistance;

            if (distance <= _nearestInteractableDistanceSqr)
            {
                _nearestInteractableDistanceSqr = distance;
                _nearestInteractableCollider = collider;
            }
        }

        if(_nearestInteractableCollider != _lastNearestInteractableCollider)
        {
			if (_lastNearestInteractableCollider)
			{
				ShaderHandler.instance.SetOutlineColor(_lastNearestInteractableCollider.gameObject, Color.clear);
			}

			_lastNearestInteractableCollider = _nearestInteractableCollider;

            if (_nearestInteractableCollider)
            {
                // Dominique, change the action image on the action button to match the new type of interactable
                UIManager.instance.UpdateActionButtonImage(_nearestInteractableCollider.GetComponent<Interactable_Object_Jack>().GetInteractableType());
                ShaderHandler.instance.SetOutlineColor(_nearestInteractableCollider.gameObject, Color.red);
            }
            else
            {
                // Dominique, Set to attack key if there's no interactable in range
                UIManager.instance.UpdateActionButtonImage(Interactable_Object_Jack.InteractableType.NUM_INTERACTABLE_TYPES);
            }
		}
    }

    // Returns true if an interactable object is within the players interaction radius
    public bool IsInteractionAvailable()
    {
        return _interactableColliders.Length > 0;
	}

    // Calls the interact function of the nearest interactable object if any are within range
    public void Interact()
    {
        if (_nearestInteractableCollider && (prevention == false))
        {
            _nearestInteractableCollider.GetComponent<Interactable_Object_Jack>().Interact();
        }
	}


}
