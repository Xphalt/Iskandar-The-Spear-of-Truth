using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionWheel : MonoBehaviour
{
    public Animator anim;
   
    [HideInInspector] public int itemID = -1;
    [HideInInspector] public bool itemWheelSelected = false;

    [SerializeField] private InventoryObject_Sal inventory;
    [SerializeField] private ItemObject_Sal bombBag;
    [SerializeField] private ItemObject_Sal wand;
    [SerializeField] private GameObject bombBagItemButton;
    [SerializeField] private GameObject wandItemButton;

    [SerializeField] private ItemSelect _selectedItem;
    public bool IsItemWheelSelected() { return itemWheelSelected; }
    private void Update()
    {
        if (itemWheelSelected)
        {
            anim.SetBool("OpenItemWheel", true);
            UpdateItemButtons();
        }
        else
        {
            anim.SetBool("OpenItemWheel", false);
        }
    }

    // Dominique, Make sure item buttons only show if we have those items
    private void UpdateItemButtons()
    {
        InventorySlot bombBagSlot = inventory.FindItemOnInventory(bombBag.data);
        if (bombBagSlot != null && bombBagSlot.amount > 0 && !bombBagItemButton.activeSelf)
        {
            bombBagItemButton.SetActive(true);
        }
        else if (bombBagSlot != null && bombBagSlot.amount <= 0 && bombBagItemButton.activeSelf)
        {
            bombBagItemButton.SetActive(false);
        }

        InventorySlot wandSlot = inventory.FindItemOnInventory(wand.data);
        if (wandSlot != null && wandSlot.amount > 0 && !wandItemButton.activeSelf)
        {
            wandItemButton.SetActive(true);
        }
        else if (wandSlot != null && wandSlot.amount <= 0 && wandItemButton.activeSelf)
        {
            wandItemButton.SetActive(false);
        }
    }
    public void UseItem()
    {
        _selectedItem.inventory.database.ItemObjects[_selectedItem.inventory.GetSlots[(int)EquipSlot.ItemSlot].item.id].UseCurrent();
    }

    public void SelectItem()
    {
        _selectedItem.OnClick();
    }
}
