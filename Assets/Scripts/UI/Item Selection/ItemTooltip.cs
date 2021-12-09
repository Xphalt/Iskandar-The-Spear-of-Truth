/*
 * Dominique 08-12-2021
 * Only show the tooltip if the slot has a name
 */

using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] GameObject tooltip;
    public void ShowTooltip()
    {
        if (!itemName.text.Equals("")) tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
