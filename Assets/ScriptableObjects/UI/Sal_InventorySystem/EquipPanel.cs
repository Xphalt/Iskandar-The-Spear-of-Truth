using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EquipSlot
{
    SwordSlot,
    ArmorSlot,
    AccessorySlot
}

public class EquipPanel : MonoBehaviour
{
    public ItemType type;
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equipment;

    public Dictionary<GameObject, Item> slotItem = new Dictionary<GameObject, Item>();

    public GameObject slotPrefab;
    public GameObject UIDescription;
    private GameObject UIDescHolder;

    public Transform parent;
     
    public void SpawnPanel()
    { 
        for (int i = 0; i < inventory.Storage.Slots.Length; i++)
        {
            if(inventory.Storage.Slots[i].item.id > -1 && inventory.database.ItemObjects[inventory.Storage.Slots[i].item.id].type == type)
            {
                var obj = Instantiate(slotPrefab, parent);

                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.ItemObjects[inventory.Storage.Slots[i].item.id].uiDisplay;

                //Adds Events to each slot
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnPointerEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit, delegate { OnPointerExit(obj); });
                AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });

                slotItem.Add(obj, inventory.Storage.Slots[i].item);
            }
        }
    }

    //Events
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnPointerEnter(GameObject obj)
    {
        //UIDesc positioning 
        Vector3 cellPosition = obj.GetComponentInParent<GridLayoutGroup>().transform.position;
        UIDescHolder = Instantiate(UIDescription, new Vector3(obj.transform.GetChild(1).position.x + UIDescription.GetComponent<RectTransform>().sizeDelta.x / 2, cellPosition.y, .0f), Quaternion.identity, parent.parent);
         
        //Stats
        string values = string.Empty; 
        switch(inventory.database.ItemObjects[slotItem[obj].id].type)
        {
            case ItemType.Weapon: //Weapon 
                values = ((WeaponObject_Sal)(inventory.database.ItemObjects[slotItem[obj].id])).Desc; 
                break;
            case ItemType.Armor: //Armor
                values = ((ArmorObject_Sal)(inventory.database.ItemObjects[slotItem[obj].id])).Desc;
                break;
            case ItemType.Accessory: //Default
                values = ((AccessoryObject)(inventory.database.ItemObjects[slotItem[obj].id])).Desc;
                break; 
        } 

        //Assign text Desc
        UIDescHolder.GetComponentInChildren<TextMeshProUGUI>().text = string.Concat(
            "<b><color=red>Name</color></b>:\n",
            slotItem[obj].name,
            "\n<b><color=red>Description</color></b>:\n",
            inventory.database.ItemObjects[slotItem[obj].id].description, "\n\n",
            values
            );
    }
    private void OnPointerExit(GameObject obj)
    {
        Destroy(UIDescHolder);
    }
    private void OnClick(GameObject obj)
    {
        //Equip
        if (inventory.database.ItemObjects[slotItem[obj].id].type == ItemType.Weapon)
        {
            inventory.SwapItem(equipment.Storage.Slots[(int)EquipSlot.SwordSlot], inventory.FindItemOnInventory(slotItem[obj]));   //Weapon

            ClearObjects();
            SpawnPanel();
        }
        else if (inventory.database.ItemObjects[slotItem[obj].id].type == ItemType.Armor)
        {
            inventory.SwapItem(equipment.Storage.Slots[(int)EquipSlot.ArmorSlot], inventory.FindItemOnInventory(slotItem[obj]));   //Armor

            ClearObjects();
            SpawnPanel();
        }
        else 
        {
            //Run function delegate that undo accessories' effects when unequipped
            if (equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id > -1 && equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id].OnUseAfter != null)
                equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id].UseAfter();

            inventory.SwapItem(equipment.Storage.Slots[(int)EquipSlot.AccessorySlot], inventory.FindItemOnInventory(slotItem[obj]));   //Accessories
            
            ClearObjects();
            SpawnPanel();
        }
    } 

    public void ClearObjects()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        Destroy(UIDescHolder);
    }
}
