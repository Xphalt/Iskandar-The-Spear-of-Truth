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
    [SerializeField] private PauseMenuManager _pauseMenuManager;
    [SerializeField] private ItemSelect _changeItem;
    [SerializeField] private UIManager _UIManager;

    private void Awake()
    {
        _playerActionsAsset = new PlayerActionsAsset();
        _playerRigidbody = GetComponent<Rigidbody>();

        #region New Input System Actions/Biddings setup (Will create a function to clean the code later)
        // Disable player interaction if pause is set
        _playerActionsAsset.Player.Pause.performed += _ =>
        {
            TogglePlayerInteraction(false);
            _pauseMenuManager.TogglePauseState();
        };
        _playerActionsAsset.Player.PotionInterface.performed += _ =>
        {
            TogglePlayerInteraction(false);
            _UIManager.TogglePotionInterface();
        };
        _playerActionsAsset.Player.Target.performed += _ => _playerTargeting.TargetObject();

        _playerActionsAsset.Player.Attack.started += _ =>
        {
            if (_player_Interaction_Jack.IsInteractionAvailable())
                _player_Interaction_Jack.Interact();
            else
                _playerCombat_Jerzy.Attack();
        };

        _playerActionsAsset.Player.Attack.performed += _ => _playerCombat_Jerzy.ThrowAttack();

        _playerActionsAsset.Player.Dash.performed += _ => Dash();

        //Items
        _playerActionsAsset.Player.ItemToggle.performed += _ => _changeItem.OnClick();
        _playerActionsAsset.Player.UseItem.performed += use =>
        {
            if (_changeItem.inventory.GetSlots[(int)EquipSlot.ItemSlot].item.id > -1)
                _changeItem.inventory.database.ItemObjects[_changeItem.inventory.GetSlots[(int)EquipSlot.ItemSlot].item.id].UseCurrent();
        };

        // Re-enable player actions if pause is triggered in pause menu
        _playerActionsAsset.UI.Pause.performed += _ =>
        {
            _pauseMenuManager.TogglePauseState();
            TogglePlayerInteraction(true);
        };
        _playerActionsAsset.UI.PotionInterface.performed += _ =>
        {
            _UIManager.TogglePotionInterface();
            TogglePlayerInteraction(true);
        };
        _playerActionsAsset.UI.Large_Potion.performed += _ =>
        {
            if (_UIManager.IsPotionInterfaceOpen())
            {
                // Use a large health potion
            }
        };
        _playerActionsAsset.UI.Medium_Potion.performed += _ =>
        {
            if (_UIManager.IsPotionInterfaceOpen())
            {
                // Use a medium health potion
            }
        };
        _playerActionsAsset.UI.Small_Potion.performed += _ =>
        {
            if (_UIManager.IsPotionInterfaceOpen())
            {
                // Use a small health potion
            }
        };


        #endregion
    }

    // UI Interaction is disabled upon starting
    private void Start()
    {
        _playerActionsAsset.UI.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();
        _playerMovement_Jerzy.Movement(new Vector3(inputVector.x, 0.0f, inputVector.y));
    }

    // Allow us to enable/disable player interaction so it isn't triggered when UI is being interacted with
    public void TogglePlayerInteraction(bool enabled)
    {
        if (enabled)
        {
            _playerActionsAsset.Player.Enable();
            _playerActionsAsset.UI.Disable();
        }
        else
        {
            _playerActionsAsset.Player.Disable();
            _playerActionsAsset.UI.Enable();
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
