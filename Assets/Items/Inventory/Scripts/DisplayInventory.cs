using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public Transform container;

    Dictionary<InventorySlot, GameObject> itemDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        CreateDisplay();
    }

    public void CreateDisplay()
    {
        //Clear inventory
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        //Builds inventory
        for (int i = 0; i < inventory.Storage.Count; i++)
        {
            var obj = Instantiate(inventory.Storage[i].item.prefabDisplay, container);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Storage[i].amount.ToString();
        }
    }
}