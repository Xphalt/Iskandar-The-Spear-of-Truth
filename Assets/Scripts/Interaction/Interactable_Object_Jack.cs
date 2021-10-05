using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to any object that can be interacted with
public class Interactable_Object_Jack : MonoBehaviour
{
    private enum InteractableType
    {
        NPC_Dialogue,
        Seller,
        LootChest,
        Item,
    }

    private DialogueTrigger _npcDialogueTrigger;
    private ShopManager _shopManager;
    private LootChest_Jerzy _lootChest;

    [SerializeField]
    InteractableType type;

	private void Start()
	{
        _shopManager = FindObjectOfType<ShopManager>();
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
                _shopManager.OpenShop(SHOP_TYPE.POTION_SELLER);
                break;

            case InteractableType.LootChest:
                // Loot chest interaction logic
                _lootChest.Interact();
                print("loot chest");
                break;

            case InteractableType.Item:
                // Item interaction logic
                print("item");
                break;

            default:
                print("Error: Unknown Interaction Type");
                break;
		}
	}
}
