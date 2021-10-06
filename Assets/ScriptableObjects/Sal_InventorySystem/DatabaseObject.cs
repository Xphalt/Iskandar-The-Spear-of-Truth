using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class DatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject_Sal[] Items;
    public Dictionary<ItemObject_Sal, int> GetID = new Dictionary<ItemObject_Sal, int>();
    public Dictionary<int, ItemObject_Sal> GetItem = new Dictionary<int, ItemObject_Sal>();

    public void OnAfterDeserialize()
    {
        GetID = new Dictionary<ItemObject_Sal, int>();
        GetItem = new Dictionary<int, ItemObject_Sal>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetID.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    { }
}
