using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the players targeted object
public class Player_Targeting_Jack : MonoBehaviour
{
    public Transform playerModelTransform;
    public CameraMove _cameraLook;

    private Transform _targetedTransform = null;
    public LayerMask TARGETABLE_LAYERMASK = 1 << 7;
    [SerializeField]
    private float targetRadius; // Half the x, y & z dimensions for the box cast to detect targetable objects
    [SerializeField]

    private bool _wasTargeting = false;
    private PlayerMovement_Jerzy _playerMovementScript;
    private PlayerAnimationManager playerAnimation;

    private Collider[] _targetableColliders;
    private Collider _nearestTargetableCollider = null;
    private Collider _lastNearestTargetableCollider = null;
    private float _targetIconTriggerDistance = 350f; // Dominique - Idk why it's this number and doesn't align with the other stuff used for the BoxCast ;-;

    // Start is called before the first frame update
    void Awake()
    {
        _playerMovementScript = GetComponent<PlayerMovement_Jerzy>();
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();

        if (!_cameraLook) _cameraLook = FindObjectOfType<CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wasTargeting & !IsTargeting())
        {
            // Player was targeting an object last frame but is no longer targeting
            UnTargetObject();
        }
        GetTargetables();
    }

    // Dominique, Show the targeting icon over the closest targetable object (based off of code for interactables in Player_Interaction_Jack)
    private void GetTargetables()
    {
        Vector3 _boxDimensions = new Vector3(_targetIconTriggerDistance, _targetIconTriggerDistance, _targetIconTriggerDistance);
        _targetableColliders = Physics.OverlapSphere(transform.position, targetRadius, TARGETABLE_LAYERMASK);

        float _nearestTargetableDistanceSqr = _targetIconTriggerDistance;
        _nearestTargetableCollider = null;

        foreach (Collider collider in _targetableColliders)
        {
            float xDistance = collider.transform.position.x - transform.position.x;
            float zDistance = collider.transform.position.z - transform.position.z;

            float distance = xDistance * xDistance + zDistance * zDistance;

            if (distance <= _nearestTargetableDistanceSqr)
            {
                _nearestTargetableDistanceSqr = distance;
                _nearestTargetableCollider = collider;
            }
        }

        // Update targeting UI
        if (_nearestTargetableCollider != _lastNearestTargetableCollider)
        {
            _lastNearestTargetableCollider = _nearestTargetableCollider;

            bool indicateNearestTargetable = UIManager.instance.GetCurrentInput() != UIManager.INPUT_OPTIONS.MOBILE && !IsTargeting();
            if (indicateNearestTargetable &&  _nearestTargetableCollider)
            {
                // Highlight the closest targetable object with outline + icon
                UIManager.instance.EnableTargetingIcon(_nearestTargetableCollider.transform, TargetingIcon.TARGETING_TYPE.INDICATING);
            }
            else if (indicateNearestTargetable)
            {
                UIManager.instance.DisableTargetingIcon();
            }
        }
    }

    // Returns true if the player is currently targeting an object
    public bool IsTargeting()
    {
        return _targetedTransform;
	}

    // If already targeting an object calling this method will untarget the object.
    // Otherwise, it will target the nearest targetable object in front of the player if any are within range
    // Dominique, modified to use a sphere and the same targeting system as for interactables in Interactable_Object_Jack for UX purposes
    public void TargetObject()
    {
        // Reset so indicator will regrab on if disabling target
        _lastNearestTargetableCollider = null;
        if (IsTargeting())
        {
            UnTargetObject();
		}
        else
        {
            playerAnimation.isStrafing = true;

            if (_nearestTargetableCollider)
            {
                _targetedTransform = _nearestTargetableCollider.transform;
                _wasTargeting = true;

                print("target");
                _cameraLook.SetSecondTarget(_targetedTransform);

                // Dominique 07-10-2021, Outline the targeted enemy and move the interactable icon to them
                ShaderHandler.instance.SetOutlineColor(_targetedTransform.gameObject, Color.yellow);
                UIManager.instance.EnableTargetingIcon(_targetedTransform, TargetingIcon.TARGETING_TYPE.ACTIVE);
            }
            else
            {
                playerAnimation.isStrafing = false;
            }
        }
         
    }

    private void UnTargetObject()
    {
        if (_targetedTransform)
        {
            ShaderHandler.instance.SetOutlineColor(_targetedTransform.gameObject, Color.clear);
        }
        _targetedTransform = null;
        _wasTargeting = false;

        _cameraLook.ClearSecondTarget();

        playerAnimation.isStrafing = false;
        
        // Dominique 07-10-2021, Clear enemy outline when no longer targeting and remove the interactable icon
        UIManager.instance.DisableTargetingIcon();
    }

    public Transform GetTargetTransform()
    {
        return _targetedTransform;
    }
}
