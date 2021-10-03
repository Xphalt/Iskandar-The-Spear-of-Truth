using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp_Item_Script : MonoBehaviour
{
    [HideInInspector]
    public string chosenObject;

    void Start()
    {
        
    }

 
    void Update()
    {
        
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
            // if (hit.transform.name.Contains("Cube"))
            if (hit.transform.CompareTag("redCube"))
            {
                chosenObject = "red";
                Destroy(gameObject);
            }
            if (hit.transform.CompareTag("greenCube"))
            {
                chosenObject = "green";
                Destroy(gameObject);
            }
            if (hit.transform.CompareTag("blueCube"))
            {
                chosenObject = "blue";
                Destroy(gameObject);
            }
            print(chosenObject);
        }
    }
}
