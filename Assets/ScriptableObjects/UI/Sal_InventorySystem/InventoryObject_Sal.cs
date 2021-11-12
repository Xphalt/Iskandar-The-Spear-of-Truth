using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Shop
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject_Sal : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    public DatabaseObject database;
    public InterfaceType type;
    public Inventory Storage;

    public InventorySlot[] GetSlots
    {
        get { return Storage.Slots; }
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Storage.Slots.Length; i++)
            {
                if (Storage.Slots[i].item.id <= -1)
                    counter++;
            }
            return counter;
        }
    }

    public bool AddItem(Item p_item, int p_amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        
        InventorySlot slot = FindItemOnInventory(p_item);
        if(!database.ItemObjects[p_item.id].stackable || slot == null)
        {
            SetEmptySlot(p_item, p_amount);
            return true;
        }
        slot.AddAmount(p_amount);

        return true; 
    }

    public InventorySlot FindItemOnInventory(Item p_item)
    {
        for (int i = 0; i < Storage.Slots.Length; i++)
        {
            if (Storage.Slots[i].item.id == p_item.id)
            {
                return Storage.Slots[i];
            }
        }
        return null;
    } 

    public void RemoveItem(Item p_item)
    {
        for (int i = 0; i < Storage.Slots.Length; i++)
        {
            if(Storage.Slots[i].item.id == p_item.id)
            {
                Storage.Slots[i].UpdateSlot(new Item(), 0);
            }
        }
    }

    public void SwapItem(InventorySlot item1, InventorySlot item2)
    {
        if (item1 != null && item2 != null)
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public InventorySlot SetEmptySlot(Item p_item, int p_amount)
    {
        for (int i = 0; i < Storage.Slots.Length; i++)
        {
            if(Storage.Slots[i].item.id <= -1)
            {
                Storage.Slots[i].UpdateSlot(p_item, p_amount);
                return Storage.Slots[i];
            }
        }
        //Inventory is full (Add functionality)
        return null;
    }

    /////////////////////////////////////////////////////// MORGAN'S SAVE CHANGES ///////////////////////////////////////////////////
    [ContextMenu("Save")]
    public void SaveStats(int num)
    {
        SaveManager.SavePlayerInventory(this, num);
    }

    [ContextMenu("Load")]
    public void LoadStats(int num)
    {
        SaveData saveData = SaveManager.LoadPlayerInventory(num);

        //Inventory newStorage = (Inventory)formatter.Deserialize(stream);

        Inventory newStorage = (Inventory)saveData.Storage;
        for (int i = 0; i < Storage.Slots.Length; i++)
        {
            Storage.Slots[i].UpdateSlot(newStorage.Slots[i].item, newStorage.Slots[i].amount);
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Storage.Clear();
    }



    public void OnBeforeSerialize()
    {
        for (int i = 0; i < Storage.Slots.Length; i++)
        {
            if (Storage.Slots[i].item.id > -1 && Storage.Slots[i].item.id < database.ItemObjects.Length)
                Storage.Slots[i].item.name = database.ItemObjects[Storage.Slots[i].item.id].name;
            else
                Storage.Slots[i].item.name = string.Empty;
        }
    }

    public void OnAfterDeserialize()
    {
        
    }

    //morgan's autosave game edit
    private void OnDestroy()
    {
        SaveStats(0);
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[30];
    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
}


public delegate void SlotUpdated(InventorySlot p_slot);
[System.Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];
   
    [System.NonSerialized] //Prevents the save system from trying to save this variable (cause scriptable objects can't be saved)
    public UserInterface_Sal parent;
    [System.NonSerialized]
    public GameObject slotDisplay; //Ref to the slot this object is on 
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;

    public Item item;
    public int amount;

    public ItemObject_Sal ItemObject
    {
        get
        {
            if(item.id >= 0)
            { 
                return parent.inventory.database.ItemObjects[item.id];
            }
            return null;
        }
    }

    //Init
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    
    public InventorySlot(Item p_item, int p_amount)
    {
        UpdateSlot(p_item, p_amount);
    }

    public void UpdateSlot(Item p_item, int p_amount)
    {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);

        item = p_item;
        amount = p_amount;

        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public bool CanPlaceInSlot(ItemObject_Sal p_itemObj)
    {
        if (allowedItems.Length <= 0 || p_itemObj == null || p_itemObj.data.id < 0)
            return true;
        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (p_itemObj.type == allowedItems[i]) //Can be equipped
                return true;
        }
        return false;
    }
}