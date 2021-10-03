using UnityEngine;

  [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemObject : ScriptableObject
{
 
    new public string name = "Item";
    public Sprite icon = null;
    public bool isEquipped = false;

    void PickUp()
    {

    }
}
