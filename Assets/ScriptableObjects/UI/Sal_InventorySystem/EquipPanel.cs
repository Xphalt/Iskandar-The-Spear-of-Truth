using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        Vector3 cellPosition = obj.GetComponentInParent<GridLayoutGroup>().transform.position;
        UIDescHolder = Instantiate(UIDescription, new Vector3(obj.transform.GetChild(1).position.x + UIDescription.GetComponent<RectTransform>().sizeDelta.x / 2, cellPosition.y, .0f), Quaternion.identity, parent.parent);
         
        //Stats
        float damage = 0;
        float physicalDEF = 0;
        try //Weapon
        {
            damage = ((WeaponObject_Sal)(inventory.database.ItemObjects[slotItem[obj].id])).damage;
        }
        catch
        {
            try //Shield
            {
                physicalDEF = ((ShieldObject)(inventory.database.ItemObjects[slotItem[obj].id])).defValues.physicalDef;
            }
            catch
            {
                try //Armor
                {
                    physicalDEF = ((ArmorObject_Sal)(inventory.database.ItemObjects[slotItem[obj].id])).defValues.physicalDef;
                }
                catch { }
            }
        }

        UIDescHolder.GetComponentInChildren<TextMeshProUGUI>().text = string.Concat(
            "Name:\n",
            slotItem[obj].name,
            "\nDescription:\n",
            inventory.database.ItemObjects[slotItem[obj].id].description, "\n\n",
            "Damage value: ", damage,
            "\nDefence value: ", physicalDEF
            );
    }
    private void OnPointerExit(GameObject obj)
    {
        Destroy(UIDescHolder);
    }
    private void OnClick(GameObject obj)
    {
        //Equip
        if (inventory.database.ItemObjects[slotItem[obj].id].type == ItemType.Weapon || inventory.database.ItemObjects[slotItem[obj].id].type == ItemType.Weapon)
        {
            inventory.SwapItem(equipment.Storage.Slots[0], inventory.FindItemOnInventory(slotItem[obj]));   //Weapon

            ClearObjects();
            SpawnPanel();
        }
        else
        {
            inventory.SwapItem(equipment.Storage.Slots[1], inventory.FindItemOnInventory(slotItem[obj]));   //Armor

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
