using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // Reference Variables
    private PlayerActionsAsset _playerActionsAsset;
    private Rigidbody _playerRigidbody;

    [Header("Scripts References")]
    [SerializeField] private Player_Targeting_Jack _PlayerTargeting;
    [SerializeField] private Inventory_UI_Script _inventoryUI;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;

    [Header("Dash Settings")]
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private int _invulnerableFrames;

    private void Awake()
    {
        _playerActionsAsset = new PlayerActionsAsset();
        _playerRigidbody = GetComponent<Rigidbody>();

        _playerActionsAsset.Player.Pause.performed += OnPause;
        _playerActionsAsset.Player.Target.performed += ctx => _PlayerTargeting.TargetObject();
        _playerActionsAsset.Player.Inventory.performed += ctx => _inventoryUI.ToggleInventory();

        _playerActionsAsset.UI.Pause.performed += OnPause;
        _playerActionsAsset.UI.Inventory.performed += ctx => _inventoryUI.ToggleInventory();

    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();
        _playerRigidbody.velocity = new Vector3(inputVector.x, 0.0f, inputVector.y) * _movementSpeed;
    }

    // Maybe OnDash() Function should call the Dash Function from some other script to keep it clean
    #region Dash (IN PROGRESS)
    //private void OnDash(InputAction.CallbackContext ctx)
    //{
    //    Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();

    //    if (inputVector != Vector2.zero)
    //    {
    //        StartCoroutine(Dash());
    //    }
    //}

    //IEnumerator Dash()
    //{
    //    Vector2 inputVector = _playerActionsAsset.Player.Movement.ReadValue<Vector2>();

    //    _playerRigidbody.AddForce(new Vector3(inputVector.x, 0.0f, inputVector.y) * _dashSpeed, ForceMode.Force);

    //    yield return new WaitForSeconds(_dashCooldown);
    //} 
    #endregion

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

    private void OnEnable()
    {
        _playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        _playerActionsAsset.Player.Disable();
    }
}
