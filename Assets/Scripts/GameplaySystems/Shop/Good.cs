
/*
 * Dominique 30-09-2021
 * Hooks up GoodData to the Good UI prefab and carries out purchases/sales
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Good : MonoBehaviour
{  
    private ItemObject_Sal objHolder; 

    public Image image;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI buyValue; private int buyVl;
    public TextMeshProUGUI sellValue; private int sellVl;
    public TextMeshProUGUI amount; private int howMany;

    public void SetupGood(ItemObject_Sal item, InventoryObject_Sal inv)
    { 
        objHolder = item;

        LocalisationTableReference localisationTableReference = new LocalisationTableReference();
        localisationTableReference.tableReference = "ShopStrings";
        localisationTableReference.entryReference = item.name;
        displayName.text = localisationTableReference.GetLocalisedString();
        
        image.sprite = item.uiDisplay;
        buyValue.text = item.BuyValue.ToString();   buyVl = item.BuyValue;
        sellValue.text = item.SellValue.ToString(); sellVl = item.SellValue;

        amount.text = inv.FindItemOnInventory(item.data).amount.ToString();
        howMany = int.Parse(amount.text.ToString());
    }

    public void BuyGood(InventoryObject_Sal destinationInvenotory)
    { 
        var playerstats = FindObjectOfType<PlayerStats>();
        if (howMany > 0 && playerstats.Gems >= buyVl)
        {
            if (destinationInvenotory.AddItem(destinationInvenotory.database.ItemObjects[objHolder.data.id].data, 1))
            {
                //Reduce amount in the shop
                GameObject.FindObjectOfType<ShopManager>().shop.FindItemOnInventory(objHolder.data).AddAmount(-1);
                amount.text = (--howMany).ToString();

                //pay
                playerstats.Gems -= buyVl; 
                Debug.Log(playerstats.Gems);
            }
        }
    }

    public void SellGood(InventoryObject_Sal destinationInvenotory)
    {
        var playerstats = FindObjectOfType<PlayerStats>();

        InventorySlot obj = destinationInvenotory.FindItemOnInventory(objHolder.data);
        
        if (obj == null) //No item in the inventory
            return;
        if (obj.amount > 0) //Player has item
        {
            if (obj.amount == 1) //Remove item if all amount is sold
                destinationInvenotory.RemoveItem(objHolder.data);
            else //Remove 1 from player inventory
                obj.AddAmount(-1);

            //Add amount to the shop
            GameObject.FindObjectOfType<ShopManager>().shop.FindItemOnInventory(objHolder.data).AddAmount(1);
            amount.text = (++howMany).ToString();
             
            //pay
            playerstats.Gems += sellVl;
            Debug.Log(playerstats.Gems);
        } 
    }
} 