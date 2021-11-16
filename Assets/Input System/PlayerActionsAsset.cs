// GENERATED AUTOMATICALLY FROM 'Assets/Input System/PlayerActionsAsset.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerActionsAsset : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActionsAsset()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActionsAsset"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""4eb4e2c1-5726-433a-ac27-7bb5151f887b"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4f02be9e-6021-4ea6-bfbe-6e43f250b61a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""b6e5aaa1-4dff-478b-a7e1-5afdb6b2db10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Target"",
                    ""type"": ""Button"",
                    ""id"": ""ba2f12cd-4faf-4be5-a023-4165718a333a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""68b87936-120f-489d-b638-d8fe014d77ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""PassThrough"",
                    ""id"": ""168d6629-6a01-4b3a-9001-152bd969d4d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PotionInterface"",
                    ""type"": ""Button"",
                    ""id"": ""e4e57a12-86b0-4380-a41f-c60f6c8e8951"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SaveInventory"",
                    ""type"": ""Button"",
                    ""id"": ""40f9b2b5-b832-4c69-9238-031a24a98585"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LoadInventory"",
                    ""type"": ""Button"",
                    ""id"": ""93a243f3-402e-4e05-b7e9-3b797b0b66bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemToggle"",
                    ""type"": ""Button"",
                    ""id"": ""0ad272d5-c099-44ca-a612-de50d1bff099"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""d518c97e-9ea9-4a8a-8288-01a4e7bc2200"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a09c1073-6ac1-4ef6-b90b-32881d30075d"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71ff6817-497a-4978-8835-094d9b3e5249"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""19ec4f23-ea2b-409d-ac4c-8a190c0d8fe9"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c5e4ecc0-1de5-4cae-ae68-82220f148bfc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b5a78475-5184-487f-beb6-cacbce53d766"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ab7ef90a-cc92-4585-a85a-833a786029f9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cda2b6b0-4579-46bd-99fb-0763a9df1c23"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7d1f179a-5d2e-4b2e-920a-dce71b684220"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c1218a15-5a80-451d-b5f8-798961a614a0"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b93edc66-d771-4d55-b58c-842a1364a7e4"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""268e8083-9b15-46ae-8cd2-7d94cbee9317"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a1ec50ff-b3d5-42e3-b270-4f5ca1ce73c2"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1b517cac-32be-47cf-9063-278f213d91e5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4391480-4c8b-4ed6-98fc-a5e3a7ccb230"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""478b4868-03f6-46a7-b681-898cc2bdd652"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9616fa0-f67c-43fc-b783-79b8fd4563ed"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a06840aa-31b0-41ea-abe1-62aab2fa952a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""28b263f5-324c-4ad1-8a7d-5c97ca0ca70d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(pressPoint=1,behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b83d5f3b-92c2-4125-89b0-5349535e30ab"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f9e9fcf-a656-438e-9f3e-c9f95169fb3c"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""PotionInterface"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd7ffb5f-945a-4e23-85e3-eb3b408d2443"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PotionInterface"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bd0f63c-f20c-4b67-8093-838c9cf88914"",
                    ""path"": ""<Keyboard>/f5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""SaveInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba4940c7-59e4-4bad-b065-9beb5c457013"",
                    ""path"": ""<Keyboard>/f6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""LoadInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d018aeb5-0453-4848-af29-cb37bacce7cd"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""ItemToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd93aa8c-29b0-46f8-8bb7-8db37e84f98a"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ItemToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4128502-8d3d-4b1f-a317-a539361a22fc"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41d26983-0120-427f-b6e7-8dec2792df8b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""2f73ad22-2049-4ed8-9e4f-6a4ed3486ecc"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""0773660c-be92-46ff-b40b-e2db90f4aae0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Large_Potion"",
                    ""type"": ""Button"",
                    ""id"": ""2ef55493-4299-42bf-a8be-b8fc9d7845eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Medium_Potion"",
                    ""type"": ""Button"",
                    ""id"": ""c11d888f-b210-4f7f-989a-b2e58fc3c17f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Small_Potion"",
                    ""type"": ""Button"",
                    ""id"": ""5f104f97-a182-454e-9f55-f3cadb1c0d36"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PotionInterface"",
                    ""type"": ""Button"",
                    ""id"": ""7b4db12f-38ec-4cbd-a8dd-9339f6678041"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a0eda673-76e1-4132-a8b3-71c7b4a2bcaa"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8e970e0-54cf-4fe4-ac88-f6248368ee91"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6297ba4-bc74-48d2-bdf8-76d2ec86e03d"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Large_Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""485476f6-bd7d-43fe-8421-9668376ea792"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Large_Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf930343-3889-42b0-b939-fb85f817f222"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Medium_Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad051192-abb2-4a79-a862-f75385ce9301"",
                    ""path"": ""<DualShockGamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Medium_Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f527e64-c834-4a3e-a5c6-6d7ddd89cc1f"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Small_Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ccfbc655-93c9-4bea-8850-9585410c0701"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Small_Potion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f349f48d-f5a5-4e58-835c-c9b0f808875f"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & mouse"",
                    ""action"": ""PotionInterface"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1ff7bc5-abdc-4401-bfd6-1fec9622b9e3"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PotionInterface"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & mouse"",
            ""bindingGroup"": ""Keyboard & mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_Target = m_Player.FindAction("Target", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_PotionInterface = m_Player.FindAction("PotionInterface", throwIfNotFound: true);
        m_Player_SaveInventory = m_Player.FindAction("SaveInventory", throwIfNotFound: true);
        m_Player_LoadInventory = m_Player.FindAction("LoadInventory", throwIfNotFound: true);
        m_Player_ItemToggle = m_Player.FindAction("ItemToggle", throwIfNotFound: true);
        m_Player_UseItem = m_Player.FindAction("UseItem", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Large_Potion = m_UI.FindAction("Large_Potion", throwIfNotFound: true);
        m_UI_Medium_Potion = m_UI.FindAction("Medium_Potion", throwIfNotFound: true);
        m_UI_Small_Potion = m_UI.FindAction("Small_Potion", throwIfNotFound: true);
        m_UI_PotionInterface = m_UI.FindAction("PotionInterface", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_Target;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_PotionInterface;
    private readonly InputAction m_Player_SaveInventory;
    private readonly InputAction m_Player_LoadInventory;
    private readonly InputAction m_Player_ItemToggle;
    private readonly InputAction m_Player_UseItem;
    public struct PlayerActions
    {
        private @PlayerActionsAsset m_Wrapper;
        public PlayerActions(@PlayerActionsAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputAction @Target => m_Wrapper.m_Player_Target;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @PotionInterface => m_Wrapper.m_Player_PotionInterface;
        public InputAction @SaveInventory => m_Wrapper.m_Player_SaveInventory;
        public InputAction @LoadInventory => m_Wrapper.m_Player_LoadInventory;
        public InputAction @ItemToggle => m_Wrapper.m_Player_ItemToggle;
        public InputAction @UseItem => m_Wrapper.m_Player_UseItem;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Target.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTarget;
                @Target.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTarget;
                @Target.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTarget;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @PotionInterface.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPotionInterface;
                @PotionInterface.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPotionInterface;
                @PotionInterface.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPotionInterface;
                @SaveInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSaveInventory;
                @SaveInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSaveInventory;
                @SaveInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSaveInventory;
                @LoadInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLoadInventory;
                @LoadInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLoadInventory;
                @LoadInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLoadInventory;
                @ItemToggle.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemToggle;
                @ItemToggle.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemToggle;
                @ItemToggle.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemToggle;
                @UseItem.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseItem;
                @UseItem.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseItem;
                @UseItem.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseItem;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Target.started += instance.OnTarget;
                @Target.performed += instance.OnTarget;
                @Target.canceled += instance.OnTarget;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @PotionInterface.started += instance.OnPotionInterface;
                @PotionInterface.performed += instance.OnPotionInterface;
                @PotionInterface.canceled += instance.OnPotionInterface;
                @SaveInventory.started += instance.OnSaveInventory;
                @SaveInventory.performed += instance.OnSaveInventory;
                @SaveInventory.canceled += instance.OnSaveInventory;
                @LoadInventory.started += instance.OnLoadInventory;
                @LoadInventory.performed += instance.OnLoadInventory;
                @LoadInventory.canceled += instance.OnLoadInventory;
                @ItemToggle.started += instance.OnItemToggle;
                @ItemToggle.performed += instance.OnItemToggle;
                @ItemToggle.canceled += instance.OnItemToggle;
                @UseItem.started += instance.OnUseItem;
                @UseItem.performed += instance.OnUseItem;
                @UseItem.canceled += instance.OnUseItem;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Large_Potion;
    private readonly InputAction m_UI_Medium_Potion;
    private readonly InputAction m_UI_Small_Potion;
    private readonly InputAction m_UI_PotionInterface;
    public struct UIActions
    {
        private @PlayerActionsAsset m_Wrapper;
        public UIActions(@PlayerActionsAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Large_Potion => m_Wrapper.m_UI_Large_Potion;
        public InputAction @Medium_Potion => m_Wrapper.m_UI_Medium_Potion;
        public InputAction @Small_Potion => m_Wrapper.m_UI_Small_Potion;
        public InputAction @PotionInterface => m_Wrapper.m_UI_PotionInterface;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Large_Potion.started -= m_Wrapper.m_UIActionsCallbackInterface.OnLarge_Potion;
                @Large_Potion.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnLarge_Potion;
                @Large_Potion.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnLarge_Potion;
                @Medium_Potion.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMedium_Potion;
                @Medium_Potion.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMedium_Potion;
                @Medium_Potion.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMedium_Potion;
                @Small_Potion.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSmall_Potion;
                @Small_Potion.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSmall_Potion;
                @Small_Potion.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSmall_Potion;
                @PotionInterface.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPotionInterface;
                @PotionInterface.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPotionInterface;
                @PotionInterface.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPotionInterface;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Large_Potion.started += instance.OnLarge_Potion;
                @Large_Potion.performed += instance.OnLarge_Potion;
                @Large_Potion.canceled += instance.OnLarge_Potion;
                @Medium_Potion.started += instance.OnMedium_Potion;
                @Medium_Potion.performed += instance.OnMedium_Potion;
                @Medium_Potion.canceled += instance.OnMedium_Potion;
                @Small_Potion.started += instance.OnSmall_Potion;
                @Small_Potion.performed += instance.OnSmall_Potion;
                @Small_Potion.canceled += instance.OnSmall_Potion;
                @PotionInterface.started += instance.OnPotionInterface;
                @PotionInterface.performed += instance.OnPotionInterface;
                @PotionInterface.canceled += instance.OnPotionInterface;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardmouseSchemeIndex = -1;
    public InputControlScheme KeyboardmouseScheme
    {
        get
        {
            if (m_KeyboardmouseSchemeIndex == -1) m_KeyboardmouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & mouse");
            return asset.controlSchemes[m_KeyboardmouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnTarget(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnPotionInterface(InputAction.CallbackContext context);
        void OnSaveInventory(InputAction.CallbackContext context);
        void OnLoadInventory(InputAction.CallbackContext context);
        void OnItemToggle(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnLarge_Potion(InputAction.CallbackContext context);
        void OnMedium_Potion(InputAction.CallbackContext context);
        void OnSmall_Potion(InputAction.CallbackContext context);
        void OnPotionInterface(InputAction.CallbackContext context);
    }
}
