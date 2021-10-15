/*
 * Dominique 15-10-2021
 * Attack to the image on the action button for a context sensitive image to appear
 * This is hooked in to the rest of the code through the UIManager and triggered by Player_Interaction_Jack.cs
 */

using UnityEngine;
using UnityEngine.UI;

public class ActionImageChanger : MonoBehaviour
{
    private Image actionImage;

    // Pass in the sprites for each interact type
    [SerializeField] private Sprite dialogueSprite;
    [SerializeField] private Sprite sellerSprite;
    [SerializeField] private Sprite lootSprite;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private Sprite attackSprite;

    private void Awake()
    {
        actionImage = GetComponent<Image>();
    }

    // Set the type - if no type is set the default is attack
    public void SetInteractImage(Interactable_Object_Jack.InteractableType interact_type)
    {
        switch (interact_type)
        {
            case Interactable_Object_Jack.InteractableType.NPC_Dialogue:
                actionImage.sprite = dialogueSprite;
                break;
            case Interactable_Object_Jack.InteractableType.Seller:
                actionImage.sprite = sellerSprite;
                break;
            case Interactable_Object_Jack.InteractableType.LootChest:
                actionImage.sprite = lootSprite;
                break;
            case Interactable_Object_Jack.InteractableType.Item:
                actionImage.sprite = itemSprite;
                break;
            default:
                actionImage.sprite = attackSprite;
                break;
        }
    }
}
