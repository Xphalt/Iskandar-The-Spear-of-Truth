using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sal : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    public InventoryObject_Sal inventory;

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

        HealthValue = MAX_HEALTH_VALUE;
        StaminaValue = MAX_STAMINA_VALUE;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.itemobj), 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Storage.items = new InventorySlot[35];
    } 
}

