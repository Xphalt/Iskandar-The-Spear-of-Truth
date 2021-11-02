using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* __________________________________________________________________________________________________________
This script allows the player to click on an object and add it to the inventory.
_____________________________________________________________________________________________________________*/

public class PickUp_Item_Script : MonoBehaviour
{
    [HideInInspector]
    public string chosenObject;
    public ItemObj item;

    /* __________________________________________________________________________________________________________
    Detecting item with mouse click.
    _____________________________________________________________________________________________________________*/
    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            /*_________________________________________________________________________
            * This just prints some testing values. Update this depending on the items.
            * ________________________________________________________________________*/
            
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

    /*_________________________________________________________________________
     * This method destroys the item that's clicked in the scene.
     * ________________________________________________________________________*/
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
