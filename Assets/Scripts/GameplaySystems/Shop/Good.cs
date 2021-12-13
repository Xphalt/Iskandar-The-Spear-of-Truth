
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
    public TextMeshProUGUI displayMoney;
    public TextMeshProUGUI buyValue; private int buyVl;
    public TextMeshProUGUI sellValue; private int sellVl;
    public TextMeshProUGUI amount; private int howMany;

    public void SetupGood(ItemObject_Sal item, InventoryObject_Sal inv, TextMeshProUGUI money)
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

        displayMoney = money;
    }

    public void BuyGood()
    {
        var playerstats = FindObjectOfType<PlayerStats>();
        if (howMany > 0 && playerstats.Gems >= buyVl) 
        {  
            //Add item in either equipment or inventory
            ItemOBJECT item = ((ItemOBJECT)(objHolder));
            if(playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].item.id == -1 && item != null && item.itemType == ItemType.Item)
                playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].UpdateSlot(objHolder.data, 1);
            else if (playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].item.id == objHolder.data.id)
                playerstats.equipment.GetSlots[(int)EquipSlot.ItemSlot].AddAmount(1);
            else 
                playerstats.inventory.AddItem(playerstats.equipment.database.ItemObjects[objHolder.data.id].data, 1);

            //Reduce amount in the shop
            ShopManager shopManager = FindObjectOfType<ShopManager>();
            shopManager.shop.FindItemOnInventory(objHolder.data).AddAmount(-1);
            amount.text = (--howMany).ToString();

            //pay
            playerstats.Gems -= buyVl;
            displayMoney.text = playerstats.Gems.ToString();

            shopManager.UpdateMoney();  
        }
    }

    public void SellGood()
    {
        var playerstats = FindObjectOfType<PlayerStats>();

        InventorySlot objInv = playerstats.inventory.FindItemOnInventory(objHolder.data);
        InventorySlot objEq = playerstats.equipment.FindItemOnInventory(objHolder.data);

        if (objInv == null && objEq == null) //No item in the inventory
            return; 

        if (objInv != null && objInv.amount > 0) //Player has item in inventory
            sell(playerstats.inventory, objInv, playerstats);
        else if (objEq != null && objEq.amount > 0) //Player has item in the equipment 
            sell(playerstats.equipment, objEq, playerstats); 
    }

    private void sell(InventoryObject_Sal destination, InventorySlot obj, PlayerStats stats)
    {
        if (obj.amount == 1) //Remove item if all amount is sold
            destination.RemoveItem(objHolder.data);
        else //Remove 1 from storage
            obj.AddAmount(-1);

        //Add amount to the shop
        ShopManager shopManager = FindObjectOfType<ShopManager>();
        shopManager.shop.FindItemOnInventory(objHolder.data).AddAmount(1);
        amount.text = (++howMany).ToString();

        //pay
        stats.Gems += sellVl;
        displayMoney.text = stats.Gems.ToString();

        shopManager.UpdateMoney(); 
    }
} 