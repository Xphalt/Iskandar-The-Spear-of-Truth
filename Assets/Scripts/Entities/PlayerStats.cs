 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : StatsInterface
{
    private PlayerAnimationManager playerAnimation;
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equipment;
    public GameObject listOfObjs;

    internal AccessoryObject Item;
    #region STATS
    private const float BASE_DAMAGE = 0;
    private const float BASE_DEFENCE = 0;

    private int gems;
    public int Gems
    {
        get
        {
            return gems;
        }
        set
        {
            gems = value;
        }
    }
    
    public float damage;
    public float spiritualDamage;
    public float defence;
    public bool poisonProtection = false;
    public bool desertProtection = false;
    public bool snowProtection = false;
    #endregion

    private void Awake()
    {
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
    }

    private void Start()
    {
        damage = BASE_DAMAGE;
        defence = BASE_DEFENCE;

        //Adding callbacks for the inventory slot update (every time something happens)
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        sfx = GetComponentInParent<SoundPlayer>();

        // list event in GameEvents.cs
        GameEvents.current.onPlayerHealthSet += OnPlayerHealthSet;
    }

    private void Update()
    {
        if (equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id > -1)
        {
            try
            {
                Item = ((AccessoryObject)(equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id]));
            }
            catch
            {
                Item = null;
            }
        }
        if (Item && Item.accessory == Accessories.RingOfVitality)
        {
            Item.UseCurrent(); //Recovers hp every n seconds
        }
        if (Item && Item.accessory == Accessories.Goggles)
        {
            Item.UseCurrent(); //deactivates objects
        }
    }

    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if (scriptedKill) amount = health - 1;
        health -= amount;
        sfx.PlayAudio();
        UIManager.instance.UpdateHealthBar((int)-amount);

        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            //gameObject.SetActive(false);
            playerAnimation.Dead();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        target.TakeDamage(amount, scriptedKill);
        
        if (target.HasBeenDefeated && Item && Item.accessory == Accessories.NecklaceOfTheLifeStealers) Item.UseCurrent();
    }


    //Delegate callbacks
    private void OnBeforeSlotUpdate(InventorySlot p_slot)
    {
        if (p_slot.ItemObject == null)
            return;

        switch (p_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:

                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", p_slot.ItemObject, " on ", p_slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", p_slot.allowedItems)));

                ItemObject_Sal temp = equipment.database.ItemObjects[p_slot.item.id];
                switch (p_slot.ItemObject.type)
                {
                    case ItemType.Weapon: 
                        damage -= ((WeaponObject_Sal)(temp)).damage;
                        spiritualDamage -= ((WeaponObject_Sal)(temp)).spiritualDamage;
                        GetComponent<PlayerMovement_Jerzy>().m_Speed -= ((WeaponObject_Sal)(temp)).speedBoost; 
                        break;
                    case ItemType.Armor:
                        defence -= ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                        poisonProtection = false;
                        desertProtection = false;
                        snowProtection = false;
                        break;
                    default:
                        damage = BASE_DAMAGE;
                        defence = BASE_DEFENCE;
                        break;
                }
                break;
            default:
                break;
        }
    }
    private void OnAfterSlotUpdate(InventorySlot p_slot)
    {
        switch (p_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:

                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", p_slot.ItemObject, " on ", p_slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", p_slot.allowedItems)));

                if (p_slot.ItemObject != null)
                {
                    ItemObject_Sal temp = equipment.database.ItemObjects[p_slot.item.id];
                    switch (p_slot.ItemObject.type)
                    {
                        case ItemType.Weapon: 
                            damage += ((WeaponObject_Sal)(temp)).damage;
                            spiritualDamage += ((WeaponObject_Sal)(temp)).spiritualDamage;
                            GetComponent<PlayerMovement_Jerzy>().m_Speed += ((WeaponObject_Sal)(temp)).speedBoost; 
                            break;
                        case ItemType.Armor:
                            defence += ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                            poisonProtection = ((ArmorObject_Sal)(temp)).defValues.poisonProtection;
                            desertProtection = ((ArmorObject_Sal)(temp)).defValues.desertProtection;
                            snowProtection = ((ArmorObject_Sal)(temp)).defValues.snowProtection;
                            break;
                        default:
                            damage = BASE_DAMAGE;
                            defence = BASE_DEFENCE;
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }

    //Pick up
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item && item.itemobj.type != ItemType.Resource)
        {
            if (inventory.AddItem(new Item(item.itemobj), 1))
                Destroy(other.gameObject);  //Only if the item is picked up
        }
        else if(item) //It's a resource
        { 
            gems += ((ResourceObject)(item.itemobj)).gems;
            UIManager.instance.ShowMoneyPopup();
            if (((ResourceObject)(item.itemobj)).OnUseCurrent != null)
                ((ResourceObject)(item.itemobj)).UseCurrent();
            Destroy(other.gameObject);
        }
    }

    //Morgan's Save Edits
    public void SaveStats(int num)
    {
        SaveManager.SavePlayerStats(this, num);
        inventory.SaveStats(num);
    }

    public void LoadStats(int num)
    {
        SaveData saveData = SaveManager.LoadPlayerStats(num);
        health = saveData.health;
        inventory.LoadStats(num);
    }

    //Morgan's Event Manager: Health Set
    private void OnPlayerHealthSet(int sethealth)
    {
        if (sethealth > 10) { sethealth = 10; }
        health = sethealth;
    }

    //Clears the invenotories when game closes 
    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
        equipment.Storage.Clear();
    }
}
