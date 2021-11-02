using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSelect : UserInterface_Sal
{
    public GameObject slot;
    public InventoryObject_Sal equipment;
    public Queue<ItemObject_Sal> itemlistqueue = new Queue<ItemObject_Sal>();

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>(); 
       
        var obj = slot; 
        //Adds Events to each slot
        AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(); });

        //Link database to that obj
        slotsOnInterface.Add(obj, inventory.Storage.Slots[0]); 
    }

    private void OnClick()
    {
        //Create list of items 
        if(itemlistqueue.Count == 0)
        {
            for (int i = 0; i < inventory.GetSlots.Length; i++)
            {
                ItemObject_Sal item = inventory.database.ItemObjects[inventory.GetSlots[i].item.id];
                if (item.type == ItemType.Item)
                    itemlistqueue.Enqueue(item); 
            }
        }
        else //Equip
        {
            inventory.SwapItem(equipment.Storage.Slots[(int)EquipSlot.ItemSlot], inventory.FindItemOnInventory(itemlistqueue.Dequeue().data));
        } 
    }
}
