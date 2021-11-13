using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSelect : UserInterface_Sal
{
    public InventoryObject_Sal playerInvenotory;
    public GameObject slot;
    public Queue<ItemObject_Sal> itemlistqueue = new Queue<ItemObject_Sal>();

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        var obj = slot;
        //Adds Events to each slot
        AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(); });

        //Storing the GameObject in the slot
        inventory.GetSlots[(int)EquipSlot.ItemSlot].slotDisplay = obj;

        //Link database to that obj
        slotsOnInterface.Add(obj, inventory.GetSlots[(int)EquipSlot.ItemSlot]);

        //Storing the GameObject in the slot
        inventory.GetSlots[(int)EquipSlot.ItemSlot].slotDisplay = obj;

        uiMask = inventory.GetSlots[(int)EquipSlot.ItemSlot].slotDisplay.transform.GetChild(0).GetComponent<Image>().sprite;
        inventory.GetSlots[(int)EquipSlot.ItemSlot].OnAfterUpdate(inventory.GetSlots[(int)EquipSlot.ItemSlot]);
    }

    public void OnClick()
    {
        //Create list of items 
        if (itemlistqueue.Count == 0)
        {
            for (int i = 0; i < playerInvenotory.GetSlots.Length; i++)
            {
                if (playerInvenotory.GetSlots[i].item.id > -1)
                {
                    ItemObject_Sal item = playerInvenotory.database.ItemObjects[playerInvenotory.GetSlots[i].item.id];
                    if (item.type == ItemType.Item)
                        itemlistqueue.Enqueue(item);
                }
            }
            if (inventory.GetSlots[(int)EquipSlot.ItemSlot].item.id > -1)
                itemlistqueue.Enqueue(inventory.database.ItemObjects[inventory.GetSlots[(int)EquipSlot.ItemSlot].item.id]);

            var a = itemlistqueue.ToList();
            a.Sort((v1, v2) => v1.data.id.CompareTo(v2.data.id));
            itemlistqueue = new Queue<ItemObject_Sal>(a);
        }

        if (itemlistqueue.Count > 0)//Equip
            inventory.SwapItem(inventory.Storage.Slots[(int)EquipSlot.ItemSlot], playerInvenotory.FindItemOnInventory(itemlistqueue.Dequeue().data));
    }
}
