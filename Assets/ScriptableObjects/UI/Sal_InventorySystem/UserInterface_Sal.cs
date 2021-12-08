using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine.InputSystem;

public abstract class UserInterface_Sal : MonoBehaviour
{
    public InventoryObject_Sal inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    protected Sprite uiMask;

    public abstract void CreateSlots();

    // Dominique 13-10-2021, Changed to Awake so it is called before the UI is hiddeb by Inventory_UI_Script in Start
    void Awake()
    {

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].OnBeforeUpdate = null;
            inventory.GetSlots[i].OnAfterUpdate = null;

            if (inventory.GetSlots[i].parent == null)
                inventory.GetSlots[i].parent = this;   //Sets the right interface parent to each slot (for dragging into another database)
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;    //Setting delegate method to update the UI
        }
        CreateSlots();
    }

    private void OnSlotUpdate(InventorySlot slot)
    { 
        if (slot.item.id >= 0) //has item
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.ItemObjects[slot.item.id].uiDisplay;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>(true).text = slot.amount == 1 ? "" : slot.amount.ToString("n0");
        }
        else //No item
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = uiMask;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "";
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
}

public static class MouseData
{
    public static UserInterface_Sal interfaceMouseIsOver;

    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}
