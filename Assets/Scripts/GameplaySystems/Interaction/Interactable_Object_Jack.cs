using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to any object that can be interacted with

public class Interactable_Object_Jack : MonoBehaviour
{
    public enum InteractableType
    {
        NPC_Dialogue,
        Seller,
        LootChest,
        Item,
        // Dominique, We'll use this to determine that the attack image should be showing on the mobile UI action button
        NUM_INTERACTABLE_TYPES,
    }

    private DialogueTrigger _npcDialogueTrigger;
    private ShopManager _shopManager;
    private LootChest_Jerzy _lootChest;
    private ToolTip toolTip;

    [SerializeField]
    InteractableType type;
    public InteractableType GetInteractableType() { return type; }

    private void Awake()
	{
        _shopManager = FindObjectOfType<ShopManager>();
        toolTip = GetComponentInChildren<ToolTip>();
    }

    private void Update()
    {
        DisplayGamepadUi();
        ShowQuestionmark();
    }

    // Runs the logic corresponding with the InteractableType of the object
    public void Interact()
    {
        switch(type)
        {
            case InteractableType.NPC_Dialogue:
                // NPC interaction logic
                _npcDialogueTrigger = GetComponent<DialogueTrigger>();
                _npcDialogueTrigger.TriggerDialogue();
                break;

            case InteractableType.Seller:
                _shopManager.OpenShop();
                break;

            case InteractableType.LootChest:
                // Loot chest interaction logic
                _lootChest = GetComponent<LootChest_Jerzy>();
                _lootChest.Interact();
                break;

            case InteractableType.Item:
                // Item interaction logic
                break;

            default:
                print("Error: Unknown Interaction Type");
                break;
		}
	}

    private void ShowQuestionmark()
    {
        if (enabled && toolTip && toolTip.inRange)
        {
            switch (type)
            {
                case InteractableType.NPC_Dialogue:
                    toolTip.Show();
                    break;
                case InteractableType.Seller:
                    toolTip.Show();
                    break;
                default:
                    print("No tool tip image found");
                    break;
            }
        }
        else toolTip.Hide();
    }

    private void DisplayGamepadUi()
    {
        if (toolTip)
        {
            if (GetInteractableType() == InteractableType.NPC_Dialogue ||
                (GetInteractableType() == InteractableType.Seller))
            {
                toolTip.isTalkType = true;

            }
            else if (GetInteractableType() == InteractableType.LootChest)
            {
                toolTip.isTalkType = false;
            }
        }
    }

    private void OnMouseExit() { if (toolTip) toolTip.Hide(); }
}
