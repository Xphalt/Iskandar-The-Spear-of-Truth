using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : StatsInterface
{
    private PlayerAnimationManager playerAnimation;
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equipment;

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
    public float defence;
    public float fireDefence;
    public float iceDefence;
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
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            playerAnimation.Dead();
        }
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        target.TakeDamage(amount, scriptedKill);
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
                        try
                        {
                            damage -= ((WeaponObject_Sal)(temp)).damage;
                        }
                        catch
                        {
                            defence -= ((ShieldObject)(temp)).defValues.physicalDef;
                        }
                        break;
                    case ItemType.Armor:
                        defence -= ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                        fireDefence -= ((ArmorObject_Sal)(temp)).defValues.fireDef;
                        iceDefence -= ((ArmorObject_Sal)(temp)).defValues.iceDef;
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
                            try
                            {
                                damage += ((WeaponObject_Sal)(temp)).damage;
                            }
                            catch
                            {
                                defence += ((ShieldObject)(temp)).defValues.physicalDef;
                            }
                            break;
                        case ItemType.Armor:
                            defence += ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                            fireDefence += ((ArmorObject_Sal)(temp)).defValues.fireDef;
                            iceDefence += ((ArmorObject_Sal)(temp)).defValues.iceDef;
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
        else if(item) //It's a gem pot
        { 
            gems += ((ResourceObject)(item.itemobj)).gems;
            Destroy(other.gameObject);
        }
    }

    //Morgan's Save Edits
    public void SaveStatsf1()
    {
        SaveManager.SavePlayerStatsf1(this);
        inventory.SaveStatsf1();
    }

    public void SaveStatsf2()
    {
        SaveManager.SavePlayerStatsf2(this);
        inventory.SaveStatsf2();
    }

    public void SaveStatsf3()
    {
        SaveManager.SavePlayerStatsf3(this);
        inventory.SaveStatsf3();
    }

    public void LoadStatsf1()
    {
        SaveDataF1 saveDataf1 = SaveManager.LoadPlayerStatsf1();
        health = saveDataf1.healthf1;
        inventory.LoadStatsf1();
    }

    public void LoadStatsf2()
    {
        SaveDataF2 saveDataf2 = SaveManager.LoadPlayerStatsf2();
        health = saveDataf2.healthf2;
        inventory.LoadStatsf2();
    }

    public void LoadStatsf3()
    {
        SaveDataF3 saveDataf3 = SaveManager.LoadPlayerStatsf3();
        health = saveDataf3.healthf3;
        inventory.LoadStatsf3();
    }


    //Clears the invenotories when game closes 
    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
        equipment.Storage.Clear();
    }
}
