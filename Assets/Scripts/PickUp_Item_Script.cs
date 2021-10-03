using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp_Item_Script : MonoBehaviour
{
    [HideInInspector]
    public string chosenObject;
    public ItemObject item;

    /* __________________________________________________________________________________________________________
    Detecting item with mouse click.
    _____________________________________________________________________________________________________________*/
    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // if (hit.transform.name.Contains("Cube"))
            if (hit.transform.CompareTag("redCube"))
            {
                chosenObject = "red";
                PickUp();
            }
            if (hit.transform.CompareTag("greenCube"))
            {
                chosenObject = "green";
                PickUp();
            }
            if (hit.transform.CompareTag("blueCube"))
            {
                chosenObject = "blue";
                PickUp();
            }
            print(chosenObject);
        }
    }

    void PickUp()
    {
        /* ______________________________________________________________________________________________
        * Method is a bool to ensure that object is not Destroyed if inventory's full.
        * ______________________________________________________________________________________________*/
        bool wasPickedUp = Inventory_Script.instance.Add(item);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
        //_______________________________________________________________________________________________  
    }
}
