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
            if (inventory.GetSlots[i].parent == null)
                inventory.GetSlots[i].parent = this;   //Sets the right interface parent to each slot (for dragging into another database)
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;    //Setting delegate method to update the UI
        }
        CreateSlots();

        //Adds Events to check if the drag and drop is within the inventory area
        if (gameObject.GetComponent<EventTrigger>())
        {
            AddEvent(this.gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(this.gameObject); });
            AddEvent(this.gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(this.gameObject); });
        }
    }

    private void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.item.id >= 0) //has item
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemObject.uiDisplay;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount == 1 ? "" : slot.amount.ToString("n0");
        }
        else //No item
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = uiMask;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }


    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
        {
            if (slot.Value.item.id >= 0) //has item
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else //No item
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
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

    //Actions
    private void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface_Sal>();
    }

    private void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        //Instantiate an empty gameobject and adding components to it
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.id >= 0) //only if there is an item in the slot
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50); //Same size of the UIdisplay
            rt.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            tempItem.transform.SetParent(transform.parent);  //set parent to the canvas 

            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }

        MouseData.tempItemBeingDragged = tempItem;
    }
    public void OnDrag(GameObject obj)
    {
        //Update the position of the item dragged
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();

    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)//Remove item if dragged outside the interface
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        if (MouseData.slotHoveredOver)//If there is a slot hovering over, check if we can swap
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItem(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
}

public static class MouseData
{
    public static UserInterface_Sal interfaceMouseIsOver;

    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}
