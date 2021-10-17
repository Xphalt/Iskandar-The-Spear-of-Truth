
/*
 * Dominique 30-09-2021
 * Hooks up GoodData to the Good UI prefab and carries out purchases/sales
 */

using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{  
    private ItemObject_Sal objHolder; 

    public Image image;
    public Text displayName;
    public Text buyValue;
    public Text sellValue;
    public Text amount; public int howMany;

    public void SetupGood(ItemObject_Sal item, InventoryObject_Sal inv)
    { 
        objHolder = item;

        LocalisationTableReference localisationTableReference = new LocalisationTableReference();
        localisationTableReference.tableReference = "ShopStrings";
        localisationTableReference.entryReference = item.name;
        displayName.text = localisationTableReference.GetLocalisedString();
        
        image.sprite = item.uiDisplay;
        buyValue.text = item.BuyValue.ToString();
        sellValue.text = item.SellValue.ToString();

        amount.text = inv.FindItemOnInventory(item.data).amount.ToString();
        howMany = int.Parse(amount.text.ToString());
    }

    public void BuyGood(InventoryObject_Sal destinationInvenotory)
    { 
        if (howMany > 0)
        {
            if (destinationInvenotory.AddItem(destinationInvenotory.database.ItemObjects[objHolder.data.id].data, 1))
            {
                //Reduce amount in the shop
                GameObject.FindObjectOfType<ShopManager>().shop.FindItemOnInventory(objHolder.data).AddAmount(-1);
                amount.text = (--howMany).ToString(); 

                //TODO:pay
            }
        }
    }

    public void SellGood(InventoryObject_Sal destinationInvenotory)
    {
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

            

            //TODO: pay
        }
        
    }
} 