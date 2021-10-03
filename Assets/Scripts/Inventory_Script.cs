using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* __________________________________________________________________________________________________________
This script controls the inventory functionality such as adding and removing items.
_____________________________________________________________________________________________________________*/

public class Inventory_Script : MonoBehaviour
{
    #region Variables
    //A static inventory instance means that there'll only be 1 inventory (no duplicates)
    public static Inventory_Script instance;
    public List<ItemObject> myItems = new List<ItemObject>();

    /*__________________________________________________________________________________
    * All objects containing the delegate method will be updated automatically.
    * Methods can subscribe to this event.
    * __________________________________________________________________________________*/
    public delegate void OnItemChanged(); 
    public OnItemChanged onItemChangedCallback;
    //__________________________________________________________________________________

    public int space = 18;

    #endregion

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

    public bool Add(ItemObject item)
    {
        //If item is not immediatly equipped, add to inventory.
        if (!item.isEquipped)
        {
            if (myItems.Count >= space)
            {
                print("Inventory full!");
                return false;
            }
            myItems.Add(item);

            if (onItemChangedCallback != null)
            {
                //This triggers the callback event.
                onItemChangedCallback.Invoke();
            }
        }
        return true;
    }

    public void Remove(ItemObject item)
    {
        myItems.Remove(item);

        if (onItemChangedCallback != null)
        {
            //This triggers the callback event.
            onItemChangedCallback.Invoke();
        }
    }



}