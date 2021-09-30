using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory_Sal : MonoBehaviour
{
    public InventoryObject_Sal inventory;
    public Transform container;

    void Start()
    {
        gameObject.SetActive(false);
        CreateDisplay();
    }

    void Update()
    {
        
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