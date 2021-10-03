using UnityEngine;

/* __________________________________________________________________________________________________________
This script controls the UI for the inventory menu.
_____________________________________________________________________________________________________________*/

public class Inventory_UI_Script : MonoBehaviour
{
    #region

    public Transform itemsSlotParent;
    public GameObject inventoryUI;
    Item_Slot_Script[] inventorySlots;

    Inventory_Script inventory;

    #endregion


    void Start()
    {
        //Caching inventory instance 
        inventory = Inventory_Script.instance;
        //This event is triggered whenever we update the inventory.
        inventory.onItemChangedCallback += UpdateInventory;

        inventorySlots = itemsSlotParent.GetComponentsInChildren<Item_Slot_Script>();
    }

    private void Update()
    {
        //------------------Open/close inventory with key "I"----------------------//
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateInventory()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.myItems.Count)
            {
                inventorySlots[i].AddItem(inventory.myItems[i]);
            }
            else
                inventorySlots[i].RemoveItem();
        }
    }
}
