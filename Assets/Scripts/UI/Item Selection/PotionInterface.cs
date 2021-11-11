/*
 * Dominique 11-11-2021
 * Functionality for the potion interface can be added here to show the number of each potion and to use potions
 */

using UnityEngine;
using TMPro;

public class PotionInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI num_large_potion_button;
    [SerializeField] private TextMeshProUGUI num_medium_potion_button;
    [SerializeField] private TextMeshProUGUI num_small_potion_button;

    private void Start()
    {
        // Set values of each button to match the number there are in the inventory
        num_large_potion_button.text = "0";
        num_medium_potion_button.text = "0";
        num_small_potion_button.text = "0";
    }

    public void UseItem()
    {
        // Use the selected Item
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
