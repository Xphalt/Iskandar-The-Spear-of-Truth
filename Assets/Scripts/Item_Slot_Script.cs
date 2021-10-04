using UnityEngine;
using UnityEngine.UI;

/* __________________________________________________________________________________________________________
This script is given to each individual item and controls their UI.
_____________________________________________________________________________________________________________*/

public class Item_Slot_Script : MonoBehaviour
{
    #region Variables

    public Image itemIcon, removeIcon;
    public Button itemButton, removeButton;
    public GameObject pickUpScript;

    ItemObject thisItem;

    #endregion

    public void AddItem(ItemObject  newItem)
    {
        /*_________________________________________________________________________
         * This sets the inventory icon to the object being picked up.
         * ________________________________________________________________________*/
        thisItem = newItem;
        itemIcon.sprite = thisItem.icon;

        //Re-enable item icon settings.
        itemButton.interactable = true;
        itemIcon.enabled = true;
        //_________________________________________________________________________

        //Re-enable remove icon settings.
        removeButton.interactable = true;
        removeIcon.enabled = true;

    }

    public void ClearSlot()
    {
        thisItem = null;

        itemIcon.sprite = null;
        itemIcon.enabled = false;
        itemButton.interactable = false;

        removeButton.interactable = false;
        removeIcon.enabled = false;
    }

    public void RemoveItem()
    {
        Inventory_Script.instance.Remove(thisItem);
    }

    public void UseItem()
    {
        if (thisItem != null)
        {
            thisItem.Use();
        }
    }
}
