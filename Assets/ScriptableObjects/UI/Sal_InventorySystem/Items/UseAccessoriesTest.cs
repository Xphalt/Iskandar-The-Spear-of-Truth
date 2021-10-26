using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseAccessoriesTest : MonoBehaviour
{
    public InventoryObject_Sal equipment; 

    AccessoryObject Item;

    public GameObject listOfObj;
    public GameObject pos;

    // Update is called once per frame
    void Update()
    {
        if(equipment.Storage.Slots[(int)EquipSlot.MiscSlot].item.id > -1)
        {
            try
            {
                Item = ((AccessoryObject)(equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.MiscSlot].item.id]));
            }
            catch 
            {
                Item = null;
            }
        }


        if (equipment.Storage.Slots[(int)EquipSlot.MiscSlot].item.id > -1 &&
            Item.accessory == Accessories.RingOfVitality)
        {
            Item.Use(gameObject); //Recovers hp every n seconds
        }
        else if(Keyboard.current.bKey.isPressed && equipment.Storage.Slots[(int)EquipSlot.MiscSlot].item.id > -1 &&
            Item.accessory == Accessories.BracersOfScouting)
        {
            Item.Use(gameObject, pos);    //Teleport
        }
        else if (Keyboard.current.nKey.isPressed && equipment.Storage.Slots[(int)EquipSlot.MiscSlot].item.id > -1 &&
            Item.accessory == Accessories.Goggles)
        {
            Item.Use(listOfObj);   //Make things invisible
        }
        else if (Keyboard.current.mKey.isPressed && equipment.Storage.Slots[(int)EquipSlot.MiscSlot].item.id > -1 &&
            Item.accessory == Accessories.BracersOfTheLifeStealers)
        {
            Item.Use(gameObject);    //Adds vitality every time an enemy is killed
        }
    }
}
