using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI_Script : MonoBehaviour
{
    #region

    public GameObject InventoryGO;

    #endregion


    void Start()
    {
        InventoryGO = GetComponentInChildren<GameObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryGO.activeInHierarchy)
                InventoryGO.SetActive(false);
            else
                InventoryGO.SetActive(true);

        
        }
    }
}
