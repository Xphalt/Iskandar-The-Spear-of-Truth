using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sal : MonoBehaviour
{
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equippedStuff;


    public GameObject UIinventory;

    //max values
    const float MAX_HEALTH_VALUE = 100.0f;
    const float MAX_STAMINA_VALUE = 100.0f;

    //health & stamina
    private float HealthValue;
    private float StaminaValue;  

    //Status
    public bool isRunning = false;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < equippedStuff.maxCapacity; i++)
        {
            equippedStuff.Storage.Add(new InventorySlot(null, 0));
        }

        HealthValue = MAX_HEALTH_VALUE;
        StaminaValue = MAX_STAMINA_VALUE;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && UIinventory.activeSelf)
        {
            UIinventory.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.I) && !UIinventory.activeSelf)
        {
            UIinventory.GetComponent<DisplayInventory_Sal>().CreateDisplay();
            UIinventory.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ItemTest_Sal>();
        if (item)
        {
            if(inventory.AddItem(item.itemobj, 1))
                Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
        equippedStuff.Storage.Clear();
    }

    public void EquipItem(ItemObject_Sal item, slotType whichSlot)
    {
        switch(whichSlot)
        {
            case slotType.LeftHand:
                if (item.type != ItemType.Shield)
                    Debug.Log("You can't equip this item in this slot.");
                else
                {
                    equippedStuff.Storage[(int)ItemType.Shield].item = item;
                    equippedStuff.Storage[(int)ItemType.Shield].amount = 1;
                }
                break;
            case slotType.rightHand:
                if (item.type != ItemType.Weapon)
                    Debug.Log("You can't equip this item in this slot.");
                else
                {
                    equippedStuff.Storage[(int)ItemType.Shield].item = item;
                    equippedStuff.Storage[(int)ItemType.Shield].amount = 1;
                }
                break;
            case slotType.Armor:
                if (item.type != ItemType.Armor)
                    Debug.Log("You can't equip this item in this slot.");
                else
                {
                    equippedStuff.Storage[(int)ItemType.Shield].item = item;
                    equippedStuff.Storage[(int)ItemType.Shield].amount = 1;
                }
                break;
        }
    }
}

