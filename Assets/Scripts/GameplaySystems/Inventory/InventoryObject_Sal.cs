using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject_Sal : ScriptableObject
{
    public int maxCapacity;
    public List<InventorySlot> Storage = new List<InventorySlot>();
    public bool AddItem(ItemObject_Sal p_item, int p_amount)
    { 
        for (int i = 0; i < Storage.Count; i++)
        {
            if(Storage[i].item == p_item)   //check if item is already in the inventory
            {
                Storage[i].AddAmount(p_amount); 
                return true;
            }
        }

        if (Storage.Count >= maxCapacity)
        {
            Debug.Log("Full capacity reached on inventory: " + this.name);
            return false;
        }
        else
        {
            Storage.Add(new InventorySlot(p_item, p_amount));
            return true;
        }
    }
    public void RemoveItem(ItemObject_Sal p_item, int p_amount)
    {
        for (int i = 0; i < Storage.Count; i++)
        {
            if (Storage[i].item == p_item && Storage[i].amount > 1)   //check if item is already in the inventory
            {
                Storage[i].AddAmount(p_amount);
                return;
            }
            else
            {
                Storage.RemoveAt(i);
                return;
            }
        }
    }
}

public enum slotType
{
    LeftHand,
    rightHand,
    Armor,
    Default
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject_Sal item;
    public int amount;
    
    //Init
    public InventorySlot(ItemObject_Sal p_item, int p_amount)
    {
        item = p_item;
        amount = p_amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}