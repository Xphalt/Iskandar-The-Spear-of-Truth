using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the players targeted object
public class Player_Targeting_Jack : MonoBehaviour
{
    public Transform playerModelTransform;
    public Transform targetingIcon;

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
    private PlayerAnimationManager playerAnimation;
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerMovementScript = GetComponent<PlayerMovement_Jerzy>();
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wasTargeting & !IsTargeting())
        {
            // Player was targeting an object last frame but is no longer targeting
            UnTargetObject();
		}
        // Dominique 07-10-2021, Ensure the icon follows above the targeted object
        else if (IsTargeting())
        {
            Vector3 new_pos = new Vector3(_targetedTransform.position.x, _targetedTransform.position.y + (1 * _targetedTransform.localScale.y), _targetedTransform.position.z);
            targetingIcon.position = new_pos;
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

            playerAnimation.isStrafing = true;


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

                // Dominique 07-10-2021, Outline the targeted enemy and move the interactable icon to them
                ShaderHandler.instance.SetOutlineColor(_targetedTransform.gameObject, Color.yellow);
                targetingIcon.gameObject.SetActive(true);

            }
            else
            {
                playerAnimation.isStrafing = false;
            }
        }
         
    }

    private void UnTargetObject()
    {
        // Dominique 07-10-2021, Clear enemy outline when no longer targeting and remove the interactable icon
        ShaderHandler.instance.SetOutlineColor(_targetedTransform.gameObject, Color.clear);
        targetingIcon.gameObject.SetActive(false);

        _targetedTransform = null;
        _wasTargeting = false;

        playerAnimation.isStrafing = false;
    }

    public Transform GetTargetTransform()
    {
        return _targetedTransform;
    }
}
