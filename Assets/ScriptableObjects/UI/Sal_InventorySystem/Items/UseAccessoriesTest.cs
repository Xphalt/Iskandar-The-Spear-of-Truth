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
        if(equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id > -1)
        {
            try
            {
                Item = ((AccessoryObject)(equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id]));
            }
            catch 
            {
                Item = null;
            }
        }


        if (Item && Item.accessory == Accessories.RingOfVitality)
        {
            Item.UseBefore(); //Recovers hp every n seconds
        }
        else if(Item && Keyboard.current.bKey.isPressed && Item.accessory == Accessories.BraceletOfScouting)
        {
            Item.UseBefore();    //Teleport
        }
        else if (Item && Keyboard.current.nKey.isPressed && Item.accessory == Accessories.Goggles)
        {
            Item.UseBefore();   //Make things invisible
        }
        else if (Item && Keyboard.current.mKey.isPressed && Item.accessory == Accessories.NecklaceOfTheLifeStealers)
        {
            Item.UseBefore();    //Adds vitality every time an enemy is killed
        }

    }
}
