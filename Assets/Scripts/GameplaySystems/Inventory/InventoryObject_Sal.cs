using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject_Sal : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private DatabaseObject database;
    public List<InventorySlot> Storage = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (DatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(DatabaseObject));
#else
        database = Resources.Load<DatabaseObject>("Database");
#endif
    }

    public void AddItem(ItemObject_Sal p_item, int p_amount)
    { 
        for (int i = 0; i < Storage.Count; i++)
        {
            if(Storage[i].item == p_item)   //check if item is already in the inventory
            {
                Storage[i].AddAmount(p_amount);
                return;
            }
        }
        Storage.Add(new InventorySlot(database.GetID[p_item], p_item, p_amount));
    }

    //As soon as something changes on our scriptable object that causes unity
    //to serialize that object we repopulate the item slot in the storage
    //to make sure it is the same item that matches with the item's id
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Storage.Count; i++)
            Storage[i].item = database.GetItem[Storage[i].ID];
    }

    public void OnBeforeSerialize()
    { }

    public void Save()
    {
        //Serialize scriptable object out to a string 
        string saveData = JsonUtility.ToJson(this, true);
       
        //Create a file and write string into the file and save it to a given location
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);

        file.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);

            //Convert file back to the scriptable object 
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);

            file.Close();
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
    public int ID;
    public ItemObject_Sal item;
    public int amount;
    
    //Init
    public InventorySlot(int p_id, ItemObject_Sal p_item, int p_amount)
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