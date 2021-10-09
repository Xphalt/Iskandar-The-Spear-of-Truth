using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sal : MonoBehaviour
{
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equipment;

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
        transform.position += new Vector3(-1, 0, 0) * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            if(inventory.AddItem(new Item(item.itemobj), 1))
                Destroy(other.gameObject);  //Only if the item is picked up
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
        equipment.Storage.Clear();
    } 
}

