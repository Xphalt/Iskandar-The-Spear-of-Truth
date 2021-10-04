using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the players targeted object
public class Player_Targeting_Jack : MonoBehaviour
{
    public Transform playerModelTransform;

    private Transform _targetedTransform = null;
    private bool _targetableObjectHit = false;
    private const int TARGETABLE_LAYERMASK = 1 << 7;
    [SerializeField]
    private Vector3 _boxCastHalfDimensions = new Vector3(3.0f, 3.0f, 1.0f); // Half the x, y & z dimensions for the box cast to detect targetable objects
    [SerializeField]
    private float _boxCastMaxDistance = 15.0f;
    private RaycastHit _targetRaycastHit;

    private bool _wasTargeting = false;
    private PlayerMovement_Jerzy _playerMovementScript;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerMovementScript = GetComponent<PlayerMovement_Jerzy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) //temporary until input system setup
        {
            TargetObject();
            if(IsTargeting())
            {
                _playerMovementScript.SetTargetedTransform(_targetedTransform);
			}
        }

        if (_wasTargeting & !IsTargeting())
        {
            // Player was targeting an object last frame but is no longer targeting
            UnTargetObject();
		}
    }

    // Returns true if the player is currently targeting an object
    public bool IsTargeting()
    {
        return _targetedTransform;
	}

    // If already targeting an object calling this method will untarget the object.
    // Otherwise, it will target the nearest targetable object in front of the player if any are within range
    public void TargetObject()
    {
        if(IsTargeting())
        {
            UnTargetObject();
		}
        else
        {
            _targetableObjectHit = Physics.BoxCast(transform.position,
                                            _boxCastHalfDimensions,
                                            playerModelTransform.forward,
                                            out _targetRaycastHit,
                                            transform.rotation,
                                            _boxCastMaxDistance,
                                            TARGETABLE_LAYERMASK);

            if (_targetableObjectHit)
            {
                _targetedTransform = _targetRaycastHit.transform;
                _wasTargeting = true;
            }
        }
	}

    private void UnTargetObject()
    {
        _targetedTransform = null;
        _wasTargeting = false;
    }
}
