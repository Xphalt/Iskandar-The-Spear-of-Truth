using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the player side of interaction logic. To be called from the player controller script.
public class Player_Interaction_Jack : MonoBehaviour
{
    [SerializeField]
    private float _interactionRadius = 5.0f;

    private const int INTERACTABLE_LAYERMASK = 1 << 6;

    private Collider[] _interactableColliders;

    // Update is called once per frame
    void Update()
    {
        GetInteractables();
    }

    // Puts all interactable object colliders within range into _interactableColliders
    private void GetInteractables()
    {
        _interactableColliders = Physics.OverlapSphere(transform.position, _interactionRadius, INTERACTABLE_LAYERMASK);
	}

    // Returns true if an interactable object is within the players interaction radius
    public bool IsInteractionAvailable()
    {
        return _interactableColliders.Length > 0;
	}

    // Calls the interact function of the nearest interactable object if any are within range
    public void Interact()
    {
        float _nearestInteractableDistanceSqr = _interactionRadius * _interactionRadius;
        Collider _nearestInteractableCollider = null;

        foreach (Collider collider in _interactableColliders)
        {
            float xDistance = collider.transform.position.x - transform.position.x;
            float zDistance = collider.transform.position.z - transform.position.z;

            float distance = xDistance * xDistance + zDistance * zDistance;

            if(distance <= _nearestInteractableDistanceSqr)
            {
                _nearestInteractableDistanceSqr = distance;
                _nearestInteractableCollider = collider;
			}
		}

        if (_nearestInteractableCollider)
        {
            _nearestInteractableCollider.GetComponent<Interactable_Object_Jack>().Interact();
        }
	}
}
