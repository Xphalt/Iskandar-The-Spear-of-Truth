using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI_Script : MonoBehaviour
{
    #region

    //public Canvas Canvas;//
    //private GameObject InventoryGO;
    public int items;
    public Transform inventoryParent;
    public GameObject InventoryGO;
    private GameObject[] inventorySlots;


    #endregion


    void Start()
    {
        // Canvas = GetComponent<Canvas>();

        InventoryGO = GetComponent<GameObject>();
        //inventorySlots = inventoryParent.GetComponentsInChildren<GameObject>();

        //Need to get item count of children through foreach loop in hierachy
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    InventoryGO = GameObject.Find("Canvas/Inventory (Parent GO)");

        //    if (InventoryGO.activeInHierarchy)
        //        InventoryGO.SetActive(false);
        //    else
        //        InventoryGO.SetActive(true);
        //}

         InventoryGO = GameObject.Find("Canvas/Inventory (Parent GO)");
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryGO.activeSelf)
                InventoryGO.SetActive(false);
            else
            {

                InventoryGO.SetActive(true);
            }
        }
    }

    void UpdateInventory()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < items)
            {
             //   inventorySlots[i].ad
            }
        }
    }
}
