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
    AccessorySlot,
    ItemSlot
}

public class EquipPanel : MonoBehaviour
{
    public ObjectType type;
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
            if(inventory.Storage.Slots[i].item.id > -1 && inventory.database.ItemObjects[inventory.Storage.Slots[i].item.id].objType == type)
            {
                var obj = Instantiate(slotPrefab, parent);

                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.ItemObjects[inventory.Storage.Slots[i].item.id].uiDisplay;
                obj.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = inventory.Storage.Slots[i].amount == 1 ? "" : inventory.Storage.Slots[i].amount.ToString("n0");

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
        UIDescHolder.transform.parent = obj.transform;
        UIDescHolder.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 1.0f);
        UIDescHolder.GetComponent<RectTransform>().anchorMax = new Vector2(0.0f, 1.0f);
        UIDescHolder.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 1.0f);
        UIDescHolder.GetComponent<RectTransform>().anchoredPosition = new Vector3(obj.GetComponent<RectTransform>().sizeDelta.x, 0.0f,8.0f);
        //Stats
        string values = string.Empty; 
        switch(inventory.database.ItemObjects[slotItem[obj].id].objType)
        {
            case ObjectType.Weapon: //Weapon 
                values = ((WeaponObject_Sal)(inventory.database.ItemObjects[slotItem[obj].id])).Desc; 
                break;
            case ObjectType.Armor: //Armor
                values = ((ArmorObject_Sal)(inventory.database.ItemObjects[slotItem[obj].id])).Desc;
                break;
            case ObjectType.Accessory: //Default
                values = ((AccessoryObject)(inventory.database.ItemObjects[slotItem[obj].id])).Desc;
                break; 
        }

        //Assign text Desc
        LocalisationTableReference nameTitleString;
        nameTitleString.tableReference = "InventoryStrings";
        nameTitleString.entryReference = "Name";

        LocalisationTableReference descriptionTitleString;
        descriptionTitleString.tableReference = "InventoryStrings";
        descriptionTitleString.entryReference = "Description";

        LocalisationTableReference nameString;
        nameString.tableReference = "InventoryStrings";
        nameString.entryReference = inventory.database.ItemObjects[slotItem[obj].id].data.name.entryReference;

        LocalisationTableReference descriptionString;
        descriptionString.tableReference = "InventoryStrings";
        descriptionString.entryReference = inventory.database.ItemObjects[slotItem[obj].id].description.entryReference;

        UIDescHolder.GetComponentInChildren<TextMeshProUGUI>().text = string.Concat(
            "<b><color=red>" + nameTitleString.GetLocalisedString() + "</color></b>:\n",
            nameString.GetLocalisedString(),
            "\n<b><color=red>" + descriptionTitleString.GetLocalisedString() + "</color></b>:\n", descriptionString.GetLocalisedString()
            , "\n\n",
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
        if (inventory.database.ItemObjects[slotItem[obj].id].objType == ObjectType.Weapon)
        {
            inventory.SwapItem(equipment.Storage.Slots[(int)EquipSlot.SwordSlot], inventory.FindItemOnInventory(slotItem[obj]));   //Weapon

            ClearObjects();  
            gameObject.SetActive(false);
        }
        else if (inventory.database.ItemObjects[slotItem[obj].id].objType == ObjectType.Armor)
        {
            inventory.SwapItem(equipment.Storage.Slots[(int)EquipSlot.ArmorSlot], inventory.FindItemOnInventory(slotItem[obj]));   //Armor

            ClearObjects(); 
            gameObject.SetActive(false);
        }
        else 
        {
            //Run function delegate that undo accessories' effects when unequipped
            if (equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id > -1 && equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id].OnUseAfter != null)
                equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id].UseAfter();

            inventory.SwapItem(equipment.Storage.Slots[(int)EquipSlot.AccessorySlot], inventory.FindItemOnInventory(slotItem[obj]));   //Accessories
            
            ClearObjects(); 
            gameObject.SetActive(false);
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
