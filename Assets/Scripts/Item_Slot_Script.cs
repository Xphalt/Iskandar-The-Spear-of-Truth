using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Slot_Script : MonoBehaviour
{
    #region

    public Image itemIcon, removeIcon;
    public Button itemButton, removeButton;
    public GameObject pickUpScript;

    private string chosenItem;

    #endregion

    void Start()
    {
        
    }


    void Update()
    {
       // AddItem();
    }

    void AddItem()
    {
        //Re-enable item icon settings.
        itemButton.interactable = true;
        itemIcon.enabled = true;

        /*__________________________________________________________________________
         * Change item icon image to new image from assets.
         * _________________________________________________________________________*/
        //Find the chosen item in the PickUp_Items_Script
        chosenItem = pickUpScript.GetComponent<PickUp_Item_Script>().chosenObject;

        if (chosenItem == "red")
        {
            itemIcon.sprite = Resources.Load<Sprite>("Assets/Images/Red_Cube_Img.png"); // <---- THIS NOT WORKING
            itemIcon.color = Color.red;
            //icon.sprite = Resources.Load("Assets/Images/Red_Cube_Img") as Sprite;
            print("RED");
        }
        if (chosenItem == "blue")
        {
            itemIcon.color = Color.blue;
        }
        if (chosenItem == "green")
        {
            itemIcon.color = Color.green;
        }

        //_________________________________________________________________________

        //Re-enable remove icon settings.
        removeButton.interactable = true;
        removeIcon.enabled = true;

    }
}
