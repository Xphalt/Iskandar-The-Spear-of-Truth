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
    public List<ItemObj> myItems = new List<ItemObj>();

    /*__________________________________________________________________________________
    * All objects containing the delegate method will be updated automatically.
    * Methods can subscribe to this event.
    * __________________________________________________________________________________*/
    public delegate void OnItemChanged(); 
    public OnItemChanged onItemChangedCallback;
    //__________________________________________________________________________________

    //Change this value in inspector
    public int inventorySpace = 18;

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

    /*_________________________________________________________________________
    * This method adds the chosen item into the inventory.
    * ________________________________________________________________________*/
    public bool Add(ItemObj item)
    {
        //If item is not immediatly equipped, add to inventory.
        if (!item.isEquipped)
        {
            if (myItems.Count >= inventorySpace)
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

    public void RemoveItem(ItemObj item)
    {
        myItems.Remove(item);

        if (onItemChangedCallback != null)
        {
            //This triggers the callback event.
            onItemChangedCallback.Invoke();
        }
    }

}