using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Script : MonoBehaviour
{
    //A static inventory instance means that there'll only be 1 inventory (no duplicates)
    public static Inventory_Script instance;
    public List<ItemObject> myItems = new List<ItemObject>();

    #region Singleton 
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found.");
            return;
        }
        //Setting the gameobject to use this particular instance of script.
        instance = this;
    }
    #endregion

    public void Add(ItemObject item)
    {
        //If item is not immediatly equipped, add to inventory.
        if (!item.isEquipped)
        {
            myItems.Add(item);
        }
        
    }

    public void Remove(ItemObject item)
    {
        myItems.Remove(item);
    }

}