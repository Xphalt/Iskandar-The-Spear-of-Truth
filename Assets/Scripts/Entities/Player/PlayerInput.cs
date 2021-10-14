using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions; // Needed to have acess to the Interecations (Hold and PRess interactions) 

//Animations in this script were done by Fate. Please contact me if you have any questions.

public class PlayerInput : MonoBehaviour
{
    // Reference Variables
    private PlayerActionsAsset _playerActionsAsset;
    private Rigidbody _playerRigidbody;

    [Header("Scripts References")]
    [SerializeField] private PlayerMovement_Jerzy _playerMovement_Jerzy;
    [SerializeField] private Player_Targeting_Jack _playerTargeting;
    [SerializeField] private Player_Interaction_Jack _player_Interaction_Jack;
    [SerializeField] private PlayerCombat_Jerzy _playerCombat_Jerzy;
    [SerializeField] private Inventory_UI_Script _inventoryUI;
    [SerializeField] private ItemSelectionWheel _itemSelectionWheel;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;

    [Header("Rotation Settings")]
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _rotationSpeed;

    #region Animation Variables
    private PlayerAnimationManager animationManager;

   // public float moveAmount = 2;
    #endregion

    private void Awake()
    {
        _playerActionsAsset = new PlayerActionsAsset();
        _playerRigidbody = GetComponent<Rigidbody>();

        animationManager = FindObjectOfType<PlayerAnimationManager>();
        #region New Input System Actions/Biddings setup (Will create a function to clean the code later)
        _playerActionsAsset.Player.Pause.performed += OnPause;
        _playerActionsAsset.Player.Target.performed += _ => _playerTargeting.TargetObject();
        _playerActionsAsset.Player.Inventory.performed += _ => _inventoryUI.ToggleInventory();

        _playerActionsAsset.Player.Attack.performed += ctx =>
            {
                if (ctx.interaction is HoldInteraction)
                    _playerCombat_Jerzy.ThrowAttack();
                else if (ctx.interaction is PressInteraction)
                    if (_player_Interaction_Jack.IsInteractionAvailable())
                        _player_Interaction_Jack.Interact();
                    else
                        _playerCombat_Jerzy.Attack();
            };

        _playerActionsAsset.Player.Dash.performed += _ => Dash();

        _playerActionsAsset.Player.ItemSelectionWheel.performed += _ => _itemSelectionWheel.ToggleItemSelectionWheel();

        _playerActionsAsset.UI.Pause.performed += OnPause;
        _playerActionsAsset.UI.Inventory.performed += _ => _inventoryUI.ToggleInventory();
        #endregion
    }

    private void FixedUpdate()
    {
        //Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();
        //_playerRigidbody.velocity = new Vector3(inputVector.x, 0.0f, inputVector.y) * _movementSpeed;

        Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();
        _playerMovement_Jerzy.Movement(new Vector3(inputVector.x, 0.0f, inputVector.y));

        //Animation
        //_movementSpeed = Mathf.Clamp01(Mathf.Abs(inputVector.x) + Mathf.Abs(inputVector.y));

    }

    private void Update()
    {
        //animationManager.UpdateAnimatorValues(0, _movementSpeed);
        //animationManager.animator.SetFloat("isRunning", moveAmount);
    }


    private void OnPause(InputAction.CallbackContext ctx)
    {
        Debug.Log("Previous Action Map: " + ctx.action.actionMap.name);

        switch (ctx.action.actionMap.name)
        {
            case "Player":
                _playerActionsAsset.Player.Disable();
                _playerActionsAsset.UI.Enable();
                // Call Pause Function
                break;
            case "UI":
                _playerActionsAsset.UI.Disable();
                _playerActionsAsset.Player.Enable();
                // Call Pause/Unpause Function
                break;
            default:
                break;
        }
    }

    private void Dash()
    {
        if (_playerRigidbody.velocity != Vector3.zero)
        {
            _playerMovement_Jerzy.Dash(_playerRigidbody.velocity);
        }
    }

    private void OnEnable()
    {
        _playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        _playerActionsAsset.Player.Disable();
    }
}
