using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp_Item_Script : MonoBehaviour
{
  
    void Start()
    {
        
    }

 
    void Update()
    {
        
    }

    void PickUp()
    {
        //destroy gameobject in scene
        //change image of inventory iten to object from empty
        //turn the delete item interactable button on
    }

    /* __________________________________________________________________________________________________________
    Detecting item with mouse click.
    _____________________________________________________________________________________________________________*/
    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name.Contains("Cube"))
            {
                print("hit");
            }
        }
    }
}
