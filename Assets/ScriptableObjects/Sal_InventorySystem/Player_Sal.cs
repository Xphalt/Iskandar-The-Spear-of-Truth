using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sal : MonoBehaviour
{
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equipment;

    //public GameObject UIinventory;

    #region STATS
    private const float BASE_DAMAGE = 0;
    private const float BASE_DEFENCE = 0;

    public float damage;
    public float defence;
    public float fireDefence;
    public float iceDefence;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    private void OnBeforeSlotUpdate(InventorySlot p_slot)
    {
        if (p_slot.ItemObject == null)
            return;

        switch (p_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:

                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", p_slot.ItemObject, " on ", p_slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", p_slot.allowedItems)));

                ItemObject_Sal temp = equipment.database.ItemObjects[p_slot.item.id];
                switch (p_slot.ItemObject.type)
                {
                    case ItemType.Weapon:
                        damage -= ((WeaponObject_Sal)(temp)).damage;
                        break;
                    case ItemType.Shield:
                        defence -= ((ShieldObject)(temp)).defValues.physicalDef;
                        fireDefence -= ((ShieldObject)(temp)).defValues.fireDef;
                        iceDefence -= ((ShieldObject)(temp)).defValues.iceDef;
                        break;
                    case ItemType.Helmet:
                    case ItemType.Chest:
                    case ItemType.Boots:
                        defence -= ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                        fireDefence -= ((ArmorObject_Sal)(temp)).defValues.fireDef;
                        iceDefence -= ((ArmorObject_Sal)(temp)).defValues.iceDef;
                        break;
                    default:
                        damage = BASE_DAMAGE;
                        defence = BASE_DEFENCE;
                        break;
                }
                break;
            default:
                break;
        }
    }
    private void OnAfterSlotUpdate(InventorySlot p_slot)
    {
        switch (p_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:

                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", p_slot.ItemObject, " on ", p_slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", p_slot.allowedItems)));

                if (p_slot.ItemObject != null)
                {
                    ItemObject_Sal temp = equipment.database.ItemObjects[p_slot.item.id];
                    switch (p_slot.ItemObject.type)
                    {
                        case ItemType.Weapon:
                            damage += ((WeaponObject_Sal)(temp)).damage;
                            break;
                        case ItemType.Shield:
                            defence += ((ShieldObject)(temp)).defValues.physicalDef;
                            fireDefence += ((ShieldObject)(temp)).defValues.fireDef;
                            iceDefence += ((ShieldObject)(temp)).defValues.iceDef;
                            break;
                        case ItemType.Helmet:
                        case ItemType.Chest:
                        case ItemType.Boots:
                            defence += ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                            fireDefence += ((ArmorObject_Sal)(temp)).defValues.fireDef;
                            iceDefence += ((ArmorObject_Sal)(temp)).defValues.iceDef;
                            break;
                        default:
                            damage = BASE_DAMAGE;
                            defence = BASE_DEFENCE;
                            break;
                    }
                }

                break;
            default:
                break;
        }
    }

    public void SaveInventory()
    {
        inventory.Save();
        equipment.Save();

    }
    public void LoadInventory()
    {
        inventory.Load();
        equipment.Load();

    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            if(inventory.AddItem(new Item(item.itemobj), 1))
                Destroy(other.gameObject);  //Only if the item is picked up
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
        equipment.Storage.Clear();
    } 
}
