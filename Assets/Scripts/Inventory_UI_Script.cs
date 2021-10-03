using UnityEngine;

public class Inventory_UI_Script : MonoBehaviour
{
    #region

    //public Canvas Canvas;//
   //  public GameObject inventoryGO;

    public Transform itemsSlotParent;
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

       // inventoryGO = GetComponent<GameObject>();

        //inventorySlots = inventoryParent.GetComponentsInChildren<GameObject>();
        // Canvas = GetComponent<Canvas>();
        //Need to get item count of children through foreach loop in hierachy
    }

    //void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.I))
    //    //{
    //    //    InventoryGO = GameObject.Find("Canvas/Inventory (Parent GO)");

    //    //    if (InventoryGO.activeInHierarchy)
    //    //        InventoryGO.SetActive(false);
    //    //    else
    //    //        InventoryGO.SetActive(true);
    //    //}

    //     inventoryGO = GameObject.Find("Canvas/Inventory (Parent GO)");
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        if (inventoryGO.activeSelf)
    //            inventoryGO.SetActive(false);
    //        else
    //        {

    //            inventoryGO.SetActive(true);
    //        }
    //    }
    //}

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
