
/*
 * Dominique 30-09-2021
 * Handles interactions with the shop UI
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public List<ItemObject_Sal> items = new List<ItemObject_Sal>();
    public List<int> amounts = new List<int>();

    public InventoryObject_Sal shop;
     
    // The good prefab will set up the UI with the GoodData
    public GameObject goodPrefab;
    // Number of goods that fit on a row in the scrollable rect
    public int goodsPerRow = 2;
    // Height of prefab so scrollbar can adjust
    public float prefabHeight = 360f;

    GameObject shopPanel;
    TextMeshProUGUI moneyValue;
    GameObject shopContentPanel;

    private void Awake()
    {
        SetUpShop();
    }

    private void Start()
    {
        shopPanel = GameObject.Find("ShopUI");
        moneyValue = GameObject.Find("ShopMoney").GetComponentInChildren<TextMeshProUGUI>();
        shopContentPanel = GameObject.Find("ShopContentPanel");
        shopPanel.SetActive(false);
    } 

    public void UpdateMoney()
    {
        // Set the money value to the number of gems the player has
        moneyValue.text = FindObjectOfType<PlayerStats>().Gems.ToString();
    }

    // Pass in an integer that corresponds to the SHOP_TYPE enum
    public void OpenShop()
    {
        // Dominique, Make sure we don't add items more than once
        if (!shopPanel.activeSelf)
        {
            UpdateMoney();

            // Dominique, Pause time when opening the shop
            Time.timeScale = 0;
            // Populate shop with goods
            int numGoods = 0;
            for (int i = 0; i < shop.Storage.Slots.Length; i++)
            {
                if (shop.Storage.Slots[i].item.id > -1)
                {
                    //GoodData goodData = currentShop.goodDatas[i];
                    GameObject goodObject = Instantiate(goodPrefab, shopContentPanel.transform);

                    goodObject.GetComponent<Good>().SetupGood(shop.database.ItemObjects[shop.Storage.Slots[i].item.id], shop, moneyValue);
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
        // Dominique, Start time up again when closing the shop
        Time.timeScale = 1;
    }

    private void SetUpShop()
    {
        shop.Storage.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            shop.AddItem(items[i].data, amounts[i]);
        }
    }

    public void SaveShop(int num, int sceneIndex)
    {
        shop.SaveStats(num, sceneIndex);
    }
    public void LoadShop(int num, int sceneIndex)
    {
        shop.LoadStats(num, sceneIndex);
    }

    private void OnApplicationQuit()
    { 
        shop.Clear();
    }
} 