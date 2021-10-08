using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public abstract class UserInterface_Sal : MonoBehaviour
{
    public Player_Sal player;

    public InventoryObject_Sal inventory;  
    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();


    void Start()
    {
        for (int i = 0; i < inventory.Storage.items.Length; i++)
        {
            inventory.Storage.items[i].parent = this;   //Sets the right interface parent to each slot (for dragging into another database)
        }
        CreateSlots();

        //Adds Events to check if the drag and drop is within the inventory area
        AddEvent(this.gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(this.gameObject); });
        AddEvent(this.gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(this.gameObject); });
    }

    void Update()
    {
        UpdateSlots();
    }


    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.ID >= 0) //has item
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.Value.item.id].uiDisplay;
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

    public abstract void CreateSlots();
     
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
        player.mouseItem.ui = obj.GetComponent<UserInterface_Sal>();
    }
    
    private void OnExitInterface(GameObject obj)
    {
        player.mouseItem.ui = null;
    }

    public void OnEnter(GameObject obj)
    {
        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
            player.mouseItem.hoverItem = itemsDisplayed[obj];
    }

    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        //Instantiate an empty gameobject and adding components to it
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50); //Same size of the UIdisplay
        rt.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        mouseObject.transform.SetParent(transform.parent);  //set parent to the canvas

        if (itemsDisplayed[obj].ID >= 0) //Check if there's an item to that slot
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }
        player.mouseItem.obj = mouseObject;
        player.mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDrag(GameObject obj)
    {
        //Update the position of the item dragged
        if (player.mouseItem.obj != null)
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var getItemObject = inventory.database.GetItem;

        if(itemOnMouse.ui != null)  //If the item is dropped in the UI pannel
        {
            //Swap items 
            if (mouseHoverObj)
                //Checks if the item can be placed in the slot && the slot is empty || there is already an item in the slot && that item can be placed in the slot
                if(mouseHoverItem.CanPlaceInSlot(getItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.id <= -1 || (mouseHoverItem.item.id >= 0 && itemsDisplayed[obj].CanPlaceInSlot(getItemObject[mouseHoverItem.item.id]))))
                    inventory.MoveItem(itemsDisplayed[obj], itemOnMouse.hoverItem.parent.itemsDisplayed[itemOnMouse.hoverObj]/*itemsDisplayed[player.mouseItem.hoverObj]*/);
        }
        else
            inventory.RemoveItem(itemsDisplayed[obj].item);

        Destroy(player.mouseItem.obj);
        player.mouseItem.item = null;
    }
}

public class MouseItem
{
    public UserInterface_Sal ui;

    public GameObject obj;
    public GameObject hoverObj;
    public InventorySlot item;
    public InventorySlot hoverItem;
}