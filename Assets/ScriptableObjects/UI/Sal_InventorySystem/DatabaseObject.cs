using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class DatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject_Sal[] ItemObjects; 

    public void OnEnable()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.name = ItemObjects[i].name;
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.id = i;  
        } 
    }

    public void OnBeforeSerialize()
    { 
    }
}
