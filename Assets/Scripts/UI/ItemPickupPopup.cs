/*
 * Dominique Russell 08-12-2021
 * As for MoneyPopup have an image that appears for a certain amount of time to indicate the picking up of an item
 */

using UnityEngine;
using UnityEngine.UI;

public class ItemPickupPopup : MonoBehaviour
{
    [SerializeField] private bool enableTimer = true;
    [SerializeField] private float disappearTime = 5;
    private float timer;

    [SerializeField] private Image itemImage;

    private void Awake()
    {
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (enableTimer && timer > disappearTime)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }

    public void SetItem(ItemObject_Sal item)
    {
        itemImage.sprite = item.uiDisplay;
    }
}
