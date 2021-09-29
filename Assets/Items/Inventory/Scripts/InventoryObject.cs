using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Storage = new List<InventorySlot>();
    public void AddItem(ItemObject p_item, int p_amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Storage.Count; i++)
        {
            if(Storage[i].item == p_item)   //check if item is already in the inventory
            {
                Storage[i].AddAmount(p_amount);
                hasItem = true;
                break;
            }
        }
        if(!hasItem)
        {
            Storage.Add(new InventorySlot(p_item, p_amount));
        }
    }
}


[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    
    //Init
    public InventorySlot(ItemObject p_item, int p_amount)
    {
        item = p_item;
        amount = p_amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}