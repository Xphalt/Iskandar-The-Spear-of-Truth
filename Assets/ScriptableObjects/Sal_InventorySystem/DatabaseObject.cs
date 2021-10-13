using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class DatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject_Sal[] ItemObjects;
    //public Dictionary<int, ItemObject_Sal> GetItem = new Dictionary<int, ItemObject_Sal>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.id = i;
            //GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        //GetItem = new Dictionary<int, ItemObject_Sal>();
    }
}
