using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float tima);
    public event StartTouch OnStartTouch;

    public delegate void EndTouch(Vector2 position, float tima);
    public event StartTouch OnEndTouch;
    #endregion

    // Reference Variables
    private PlayerActionsAsset _playerActionsAsset;
    private Rigidbody _playerRigidbody;
    private PotionInterface potionInterface;
    private Camera _mainCamera;

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
        potionInterface = FindObjectOfType<PotionInterface>();
        
        _mainCamera = Camera.main;

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

        _playerActionsAsset.Player.UseWand.performed += _ => {
            var objs = FindObjectsOfType<MagneticObj>();
            foreach (var item in objs)
            {
                item.StopInteraction();
            }
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

        _playerActionsAsset.UI.Large_Potion.performed += ctx => UsePotion(ctx, potionInterface.largePotion);
        _playerActionsAsset.UI.Medium_Potion.performed += ctx => UsePotion(ctx, potionInterface.mediumPotion);
        _playerActionsAsset.UI.Small_Potion.performed += ctx => UsePotion(ctx, potionInterface.smallPotion);
  
        _playerActionsAsset.Player.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        _playerActionsAsset.Player.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

        //_playerActionsAsset.Player.DeviceUsed.performed += ctx => ChangeDevice(ctx);

        //InputSystem.onDeviceChange += (device, change) => OnDeviceChange(device, change);

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
            _playerMovement_Jerzy.Dash(_playerRigidbody.velocity.normalized);
        }
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnStartTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerActionsAsset.Player.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnEndTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerActionsAsset.Player.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
    }

    private void UsePotion(InputAction.CallbackContext ctx, ItemObject_Sal potion)
    {
        if (_UIManager.IsPotionInterfaceOpen())
        {
            var interfaces = FindObjectsOfType<PotionInterface>();

            // Use a small health potion
            foreach (var item in interfaces)
            {
                if (item.gameObject.activeSelf)
                    item.UseItem(potion);
            }
        }
    }

    public Vector3 GetMovementVector()
    {
        Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();
        return (new Vector3(inputVector.x, 0.0f, inputVector.y));
    }
    //    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    //    {
    //#if UNITY_ANDROID
    //        UIManager.INPUT_OPTIONS currentInput = UIManager.INPUT_OPTIONS.MOBILE;
    //#else
    //        UIManager.INPUT_OPTIONS currentInput = UIManager.INPUT_OPTIONS.KEYBOAD_AND_MOUSE;
    //#endif

    //        switch (change)
    //        {
    //            case InputDeviceChange.Added:
    //                // New Device.
    //                UIManager.instance.SetUIForInput(UIManager.INPUT_OPTIONS.GAMEPAD);
    //                break;
    //            case InputDeviceChange.Disconnected:
    //                // If this is happening, activate some boolean that will tell the game that a controller is now "missing".
    //                UIManager.instance.SetUIForInput(currentInput);
    //                break;
    //            case InputDeviceChange.Reconnected:
    //                // Plugged back in.
    //                UIManager.instance.SetUIForInput(UIManager.INPUT_OPTIONS.GAMEPAD);
    //                break;
    //            case InputDeviceChange.Removed:
    //                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
    //                UIManager.instance.SetUIForInput(currentInput);
    //                break;
    //            default:
    //                // Always includes a default case for when a unused case is being called. Leave it empty.
    //                break;
    //        }
    //    }

//    private void ChangeDevice(InputAction.CallbackContext ctx)
//    {
//        if (ctx.control.device.displayName != "Keyboard")
//#if UNITY_ANDROID
//            UIManager.instance.SetUIForInput(UIManager.INPUT_OPTIONS.MOBILE);
//#else
//            UIManager.instance.SetUIForInput(UIManager.INPUT_OPTIONS.GAMEPAD);
//#endif
//        else
//            UIManager.instance.SetUIForInput(UIManager.INPUT_OPTIONS.KEYBOAD_AND_MOUSE);
//    }

    private void OnEnable()
    {
        _playerActionsAsset.Enable();
    }

    private void OnDisable()
    {
        _playerActionsAsset.Disable();
    }
}
