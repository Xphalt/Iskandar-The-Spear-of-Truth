using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to any object that can be interacted with
public class Interactable_Object_Jack : MonoBehaviour
{
    private enum InteractableType
    {
        NPC,
        LootChest,
        Item,
    }

    [SerializeField]
    InteractableType type;

    // Runs the logic corresponding with the InteractableType of the object
    public void Interact()
    {
        switch(type)
        {
            case InteractableType.NPC:
                // NPC interaction logic
                print("NPC");
                break;

            case InteractableType.LootChest:
                // Loot chest interaction logic
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
