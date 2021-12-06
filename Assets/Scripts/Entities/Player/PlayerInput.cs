using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float tima);
    public event StartTouch OnStartTouch;

    public delegate void EndTouch(Vector2 position, float tima);
    public event StartTouch OnEndTouch;
    #endregion

    #region Reference Variables
    private PlayerActionsAsset _playerActionsAsset;
    private Rigidbody _playerRigidbody;
    private PotionInterface potionInterface;
    private Camera _mainCamera;
    // Dominique, Check if we're panning before pausing
    private CameraMove _cameraMove;
    #endregion

    [Header("Scripts References")]
    [SerializeField] private PlayerMovement_Jerzy _playerMovement_Jerzy;
    [SerializeField] private Player_Targeting_Jack _playerTargeting;
    [SerializeField] private Player_Interaction_Jack _player_Interaction_Jack;
    [SerializeField] private PlayerCombat_Jerzy _playerCombat_Jerzy;
    [SerializeField] private PauseMenuManager _pauseMenuManager;
    [SerializeField] private ItemSelect _changeItem;
    [SerializeField] private UIManager _UIManager;

    #region Jerzy's Change
    // this is so we have a visual cue of when the player can throw their sword
    private const float holdThreshold = 1.0f;
    private const float chargeDelay = 0.5f;
    private IEnumerator coroutine;
    // Player.Attack.started changed & Player.Attack.canceled added
    #endregion

    private void Awake()
    {
        _playerActionsAsset = new PlayerActionsAsset();
        _playerRigidbody = GetComponent<Rigidbody>();
        potionInterface = FindObjectOfType<PotionInterface>();
       
        _mainCamera = Camera.main;
        _cameraMove = FindObjectOfType<CameraMove>().GetComponent<CameraMove>();

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
            {
                _playerCombat_Jerzy.Attack();
                coroutine = StartThrow();
                StartCoroutine(coroutine);
            }

        };

        _playerActionsAsset.Player.Attack.canceled += _ => { StopCoroutine(coroutine); _playerCombat_Jerzy.cancelChargedAttack(); };

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
            // Dominique, If the camera is panning or potion interface is open don't open the pause menu
            if (!_cameraMove.IsPanning() && !_UIManager.IsPotionInterfaceOpen())
            {
                _pauseMenuManager.TogglePauseState();
                TogglePlayerInteraction(true);
            }
        };
        _playerActionsAsset.UI.PotionInterface.performed += _ =>
        {
            // Dominique, If the camera is panning or pause menu is open don't open the potion interface
            if (!_cameraMove.IsPanning() && !PauseMenuManager.gameIsPaused)
            {
                _UIManager.TogglePotionInterface();
                TogglePlayerInteraction(true);
            }
        };

        _playerActionsAsset.UI.Large_Potion.performed += _ => UsePotion(potionInterface.largePotion);
        _playerActionsAsset.UI.Medium_Potion.performed += _ => UsePotion(potionInterface.mediumPotion);
        _playerActionsAsset.UI.Small_Potion.performed += _ => UsePotion( potionInterface.smallPotion);

        _playerActionsAsset.Player.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        _playerActionsAsset.Player.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

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
        if (!Utils.DiscardSwipe(_playerActionsAsset.Player.PrimaryPosition.ReadValue<Vector2>()))
            OnStartTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerActionsAsset.Player.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnEndTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerActionsAsset.Player.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
    }

    private void UsePotion(ItemObject_Sal potion)
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

    private void OnEnable()
    {
        _playerActionsAsset.Enable();
    }

    private void OnDisable()
    {
        _playerActionsAsset.Disable();
    }

    IEnumerator StartThrow()
    {
        yield return new WaitForSeconds(chargeDelay);
        _playerCombat_Jerzy.startChargingEffect();
        yield return new WaitForSeconds(holdThreshold - chargeDelay);
        _playerCombat_Jerzy.swordChargedEffect();
    }
}
