using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DisplayInventory_Sal : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject_Sal inventory;
    public Transform container;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEMS;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMNS;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    { 
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Storage.Count; i++)
        {
            if(itemsDisplayed.ContainsKey(inventory.Storage[i]))
            {
                itemsDisplayed[inventory.Storage[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Storage[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                //obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = 
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Storage[i].amount.ToString("n0");

                itemsDisplayed.Add(inventory.Storage[i], obj);
            }
        }
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Storage.Count; i++)
        {
            var obj = Instantiate(inventory.Storage[i].item.uiDisplay, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Storage[i].amount.ToString();
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0.0f);
    }
}