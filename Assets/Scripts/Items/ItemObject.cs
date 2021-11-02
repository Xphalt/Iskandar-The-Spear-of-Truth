using UnityEngine;

/*___________________________________________________________________
 * This script is a blueprint for a pickup-able item.
 * __________________________________________________________________*/

  [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemObj : ScriptableObject
{
 
    new public string name = "Item";
    public Sprite icon = null;
    public bool isEquipped = false;

    /*_________________________________________________________________________
    * This is a virtual method, that can is called when an item is picked up.
    * ________________________________________________________________________*/
    public virtual void Use()
    {
        //Using item code
        Debug.Log("Using item" + name);
    }
}
//___________________________________________________________________