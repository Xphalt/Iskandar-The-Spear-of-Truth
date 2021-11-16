using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StaticInterface_Sal : UserInterface_Sal
{
    public GameObject[] slots;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            var obj = slots[i];

            //Adds Events to each slot
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            //Storing the GameObject in the slot
            inventory.GetSlots[i].slotDisplay = obj;

            //Link database to that obj
            slotsOnInterface.Add(obj, inventory.Storage.Slots[i]);
        }

        uiMask = inventory.GetSlots[0].slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
    }
}
