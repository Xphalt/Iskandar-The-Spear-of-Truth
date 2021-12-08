/*
 * Dominique 11-11-2021
 * Functionality for the potion interface can be added here to show the number of each potion and to use potions
 */

using UnityEngine;
using TMPro;

public class PotionInterface : MonoBehaviour
{
    private PlayerStats stats;
    public InventoryObject_Sal inventory;
    public ItemObject_Sal smallPotion, mediumPotion, largePotion;

    [SerializeField] private TextMeshProUGUI num_large_potion_button;
    [SerializeField] private TextMeshProUGUI num_medium_potion_button;
    [SerializeField] private TextMeshProUGUI num_small_potion_button;

    private void Start()
    {
        stats = FindObjectOfType<PlayerStats>();

        SetAmounts();
    }

    public void UseItem(ItemObject_Sal potion)
    {
        // Use the selected Item
        InventorySlot invSlot = inventory.FindItemOnInventory(potion.data);
        if (invSlot != null)
        {
            stats.health += ((ItemOBJECT)(potion)).healingValue;
            stats.health = Mathf.Clamp(stats.health, 0.0f, stats.MAX_HEALTH);
            
            //Update UI
            UIManager.instance.SetHealthBar((int)stats.health);

            //Item removal  
            if (invSlot.amount == 1)
                inventory.RemoveItem(potion.data);
            else
                invSlot.AddAmount(-1);

            SetAmounts();
        } 
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void SetAmounts()
    {
        if (num_large_potion_button && num_medium_potion_button && num_small_potion_button)
        {
            // Set values of each button to match the number there are in the inventory
            InventorySlot large = inventory.FindItemOnInventory(largePotion.data);
            num_large_potion_button.text = large == null ? "0" : large.amount.ToString();

            InventorySlot medium = inventory.FindItemOnInventory(mediumPotion.data);
            num_medium_potion_button.text = medium == null ? "0" : medium.amount.ToString();

            InventorySlot small = inventory.FindItemOnInventory(smallPotion.data);
            num_small_potion_button.text = small == null ? "0" : small.amount.ToString();
        }
    }
}
