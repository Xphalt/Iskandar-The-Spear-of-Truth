using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest_Sal : MonoBehaviour
{
    public InventoryObject_Sal inventory;
    public GameObject UIinventory;


    void Start()
    {
       
    } 

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
        if(item)
        {
            inventory.AddItem(item.itemobj, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
    }
}
