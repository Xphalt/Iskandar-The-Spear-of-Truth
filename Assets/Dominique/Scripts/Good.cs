
/*
 * Dominique 30-09-2021
 * Hooks up GoodData to the Good UI prefab and carries out purchases/sales
 */

using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    private GoodData goodData;

    public Image image;
    public Text displayName;
    public Text buyValue;
    public Text sellValue;
    public Text amount;

    public void SetupGood(GoodData data)
    {
        goodData = data;

        image.sprite = data.displaySprite;
        displayName.text = data.displayName;
        buyValue.text = data.buyValue.ToString();
        sellValue.text = data.sellValue.ToString();
        // TODO: Get current number of items from inventory
        amount.text = "0";
    }

    public void BuyGood()
    {
        // TODO: Add good to inventory and remove money 
    }

    public void SellGood()
    {
        // TODO: Remove good from inventory and add money
    }
}

[System.Serializable]
public struct GoodData
{
    public GOOD_TYPE goodType;
    public string displayName;
    public Sprite displaySprite;
    public int sellValue;
    public int buyValue;
}
