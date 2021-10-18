using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database")]
public class DatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject_Sal[] ItemObjects; 
     
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if (ItemObjects[i])
                ItemObjects[i].data.id = i;  
        } 
    }

    public void OnBeforeSerialize()
    {
        if (ItemObjects.Length > 0)
            for (int i = 0; i < ItemObjects.Length; i++)
            {
                if(ItemObjects[i])
                    ItemObjects[i].data.name = ItemObjects[i].name;
            }
    }
}
