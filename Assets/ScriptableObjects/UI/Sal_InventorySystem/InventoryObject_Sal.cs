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
public class InventoryObject_Sal : ScriptableObject
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
                Storage.Slots[i].UpdateSlot(null, 0);
            }
        }
    }

    public void SwapItem(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
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

    [ContextMenu("Save")]
    public void Save()
    {
        ////Serialize scriptable object out to a string 
        //string saveData = JsonUtility.ToJson(this, true);
        //
        ////Create a file and write string into the file and save it to a given location
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //
        //file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Storage);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //
            ////Convert file back to the scriptable object 
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newStorage = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < Storage.Slots.Length; i++)
            {
                Storage.Slots[i].UpdateSlot(newStorage.Slots[i].item, newStorage.Slots[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Storage.Clear();
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