
/*
 * Dominique 30-09-2021
 * Handles interactions with the shop UI
 */

using UnityEngine;

// int version is passed in to OpenShop so correct goods are shown
public enum SHOP_TYPE
{
    HUNTER,
    POTION_SELLER,
    NUM_SHOP_TYPES,
}

public enum GOOD_TYPE
{
    // Hunter
    BOAR_TUSK,
    BAT_WINGS,

    // Potion Seller
    SMALL_HEALTH,
    MEDIUM_HEALTH,
}

public class ShopManager : MonoBehaviour
{
    public Shop[] shops;
    // The good prefab will set up the UI with the GoodData
    public GameObject goodPrefab;
    // Number of goods that fit on a row in the scrollable rect
    public int goodsPerRow = 2;
    // Height of prefab so scrollbar can adjust
    public float prefabHeight = 360f;

    GameObject shopPanel;
    GameObject shopContentPanel;
    Shop currentShop;

    private void Start()
    {
        shopPanel = GameObject.Find("ShopUI");
        shopContentPanel = GameObject.Find("ShopContentPanel");
        shopPanel.SetActive(false);
    }

    // Pass in an integer that corresponds to the SHOP_TYPE enum
    public void OpenShop(SHOP_TYPE shopType)
    {
        // Get the goods for the type of shop
        for (int i = 0; i < shops.Length; i++)
        {
            if (shops[i].shopType == shopType)
            {
                currentShop = shops[i];
                break;
            }
        }

        // Populate shop with goods
        for (int i = 0; i < currentShop.goodDatas.Length; i++)
        {
            GoodData goodData = currentShop.goodDatas[i];
            GameObject goodObject = Instantiate(goodPrefab, shopContentPanel.transform);

            goodObject.GetComponent<Good>().SetupGood(goodData);
        }

        // Set the size of the content panel so it fits the number of goods available
        shopPanel.SetActive(true);
        RectTransform contentRect = shopContentPanel.GetComponent<RectTransform>();
        int numGoods = currentShop.goodDatas.Length;
        if (numGoods % goodsPerRow == 1)
        {
            numGoods++;
        }
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, prefabHeight * (numGoods / goodsPerRow));
    }

    // Destroy the goods shown in the shop so they're not there when it opens again
    public void CloseShop()
    {
        Good[] goods = shopContentPanel.GetComponentsInChildren<Good>();
        for (int i = 0; i < goods.Length; i++)
        {
            DestroyImmediate(goods[i].gameObject);
        }

        shopPanel.SetActive(false);
    }
}

[System.Serializable]
public struct Shop
{
    public SHOP_TYPE shopType;
    public GoodData[] goodDatas;
}