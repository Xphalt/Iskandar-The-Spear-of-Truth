
/*
 * Dominique 30-09-2021
 * Handles interactions with the shop UI
 */

using UnityEngine; 

public class ShopManager : MonoBehaviour
{
    public InventoryObject_Sal shop;
     
    // The good prefab will set up the UI with the GoodData
    public GameObject goodPrefab;
    // Number of goods that fit on a row in the scrollable rect
    public int goodsPerRow = 2;
    // Height of prefab so scrollbar can adjust
    public float prefabHeight = 360f;

    GameObject shopPanel;
    GameObject shopContentPanel; 

    private void Start()
    {
        shopPanel = GameObject.Find("ShopUI");
        shopContentPanel = GameObject.Find("ShopContentPanel");
        shopPanel.SetActive(false);
    } 

    // Pass in an integer that corresponds to the SHOP_TYPE enum
    public void OpenShop()
    {  
        // Populate shop with goods
        int numGoods = 0;
        for (int i = 0; i < shop.Storage.Slots.Length; i++)
        {
            if (shop.Storage.Slots[i].item.id > -1)
            {
                //GoodData goodData = currentShop.goodDatas[i];
                GameObject goodObject = Instantiate(goodPrefab, shopContentPanel.transform); 

                goodObject.GetComponent<Good>().SetupGood(shop.database.ItemObjects[shop.Storage.Slots[i].item.id], shop);
                ++numGoods;
            }
        }

        // Set the size of the content panel so it fits the number of goods available
        shopPanel.SetActive(true);
        RectTransform contentRect = shopContentPanel.GetComponent<RectTransform>();
        
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