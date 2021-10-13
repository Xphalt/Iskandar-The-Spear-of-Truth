using UnityEngine;

/* __________________________________________________________________________________________________________
This script controls the UI for the inventory menu.
_____________________________________________________________________________________________________________*/

public class Inventory_UI_Script : MonoBehaviour
{
    #region Variables

    public GameObject inventoryUI;
    public GameObject equipmentUI;

    #endregion

    private void Start()
    {
        inventoryUI.SetActive(false);
        equipmentUI.SetActive(false);
    }

    public void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        equipmentUI.SetActive(!equipmentUI.activeSelf);
    }
}
