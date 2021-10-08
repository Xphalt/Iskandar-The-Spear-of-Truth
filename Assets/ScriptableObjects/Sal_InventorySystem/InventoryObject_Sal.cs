using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject_Sal : ScriptableObject
{
    public string savePath;
    public DatabaseObject database;
    public Inventory Storage;

    public void AddItem(Item p_item, int p_amount)
    {
        for (int i = 0; i < Storage.items.Length; i++)
        {
            if (Storage.items[i].ID == p_item.id)   //check if item is already in the inventory
            {
                Storage.items[i].AddAmount(p_amount);
                return;
            }
        }
        SetEmptySlot(p_item, p_amount);
    }

    public void RemoveItem(Item p_item)
    {
        for (int i = 0; i < Storage.items.Length; i++)
        {
            if(Storage.items[i].item == p_item)
            {
                Storage.items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public InventorySlot SetEmptySlot(Item p_item, int p_amount)
    {
        for (int i = 0; i < Storage.items.Length; i++)
        {
            if(Storage.items[i].ID <= -1)
            {
                Storage.items[i].UpdateSlot(p_item.id, p_item, p_amount);
                return Storage.items[i];
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
            for (int i = 0; i < Storage.items.Length; i++)
            {
                Storage.items[i].UpdateSlot(newStorage.items[i].ID, newStorage.items[i].item, newStorage.items[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Storage = new Inventory();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[30];
}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;

    //Init
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }
    
    public InventorySlot(int p_id, Item p_item, int p_amount)
    {
        ID = p_id;
        item = p_item;
        amount = p_amount;
    }

    public void UpdateSlot(int p_id, Item p_item, int p_amount)
    {
        ID = p_id;
        item = p_item;
        amount = p_amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}