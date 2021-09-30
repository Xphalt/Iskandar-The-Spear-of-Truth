using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sal : MonoBehaviour
{
    public InventoryObject_Sal inventory;
    public InventorySlot ItemEquipped;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ItemEquipped = inventory.Storage[0];
        if (Input.GetKeyDown(KeyCode.D))
            inventory.RemoveItem(inventory.Storage[0].item, 1);

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
            inventory.AddItem(item.itemobj, 1);
            Destroy(other.gameObject);

            //TEST (equip item)
            //inventory.Storage[0].item.Use();
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
    }
}
