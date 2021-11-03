using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputActionExtensions
{
    public static bool IsPressed(this InputAction inputAction)
    {
        return inputAction.ReadValue<float>() > 0f;
    }

    public static bool WasPressedThisFrame(this InputAction inputAction)
    {
        return inputAction.triggered && inputAction.ReadValue<float>() > 0f;
    }

    public static bool WasReleasedThisFrame(this InputAction inputAction)
    {
        return inputAction.triggered && inputAction.ReadValue<float>() == 0f;
    }
}


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
    //[SerializeField] private Inventory_UI_Script _inventoryUI;
    [SerializeField] private ItemSelectionWheel _itemSelectionWheel;
    [SerializeField] private ItemSelectionBar _itemSelectionBar;
    [SerializeField] private PauseMenuManager _pauseMenuManager;

    private void Awake()
    {
        _playerActionsAsset = new PlayerActionsAsset();
        _playerRigidbody = GetComponent<Rigidbody>();


        #region New Input System Actions/Biddings setup (Will create a function to clean the code later)
        _playerActionsAsset.Player.Pause.performed += OnPause;
        _playerActionsAsset.Player.Target.performed += _ => _playerTargeting.TargetObject();
        _playerActionsAsset.Player.Inventory.performed += _ => _pauseMenuManager.TogglePauseState();

        _playerActionsAsset.Player.Attack.started += _ =>
        {
            if (_player_Interaction_Jack.IsInteractionAvailable())
                _player_Interaction_Jack.Interact();
            else
                _playerCombat_Jerzy.Attack();
        };

        _playerActionsAsset.Player.Attack.performed += _ => _playerCombat_Jerzy.ThrowAttack();

        _playerActionsAsset.Player.Dash.performed += _ => Dash();


        _playerActionsAsset.Player.ItemSelectionWheel.performed += _ => _itemSelectionWheel.ToggleItemSelectionWheel();
        _playerActionsAsset.Player.ItemSelectionBar.performed += _ => _itemSelectionBar.ShowHotbar();

        _playerActionsAsset.UI.Pause.performed += OnPause;
        #endregion
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();
        _playerMovement_Jerzy.Movement(new Vector3(inputVector.x, 0.0f, inputVector.y));
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

    public static Vector3 MousePosition()
    {
        Ray ray = GameObject.FindObjectOfType<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());

        return ray.origin;
    }

    private void OnEnable()
    {
        _playerActionsAsset.Enable();
    }

    private void OnDisable()
    {
        _playerActionsAsset.Disable();
    }
}
