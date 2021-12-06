using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerStats : StatsInterface
{
    private PlayerAnimationManager playerAnimation;
    private PlayerCombat_Jerzy playerCombat;
    private Player_Targeting_Jack playerTargeting;
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equipment;
    public GameObject listOfObjs;

    internal AccessoryObject Accessory;
    public ItemObject_Sal revivalGem;

    public List<List<bool>> totallynotevents = new List<List<bool>>();

    public Transform startPos;

    #region Weapon Model Variables
    private GameObject swordEmpty;
    public GameObject propSwordHolder, ironSword, falchion, stick;
    #endregion

    /*______________________________Damage_Flash_Variables_______________________________*/
    public SkinnedMeshRenderer MeshRenderer;
    public Color Origin;
    public float FlashTime;
    /*___________________________________________________________________________________*/
    #region STATS
    private const float BASE_DAMAGE = 0;
    private const float BASE_DEFENCE = 0;

    public int gems;
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
    private float bleedDamage;
    private float maxBleedTicks;
    private float bleedTicks;
    private float bleedDelay;
    private float timeSinceLastBleedDamage;
    public bool bleeding;
    public bool poisonProtection = false;
    public bool desertProtection = false;
    public bool snowProtection = false;
    #endregion

    private void Awake()
    {
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
        playerCombat = GetComponent<PlayerCombat_Jerzy>();
        playerTargeting = GetComponent<Player_Targeting_Jack>();
        SaveDataAssistant saveAssisstant = FindObjectOfType<SaveDataAssistant>();
        if (saveAssisstant) SaveNum = saveAssisstant.currentSaveFileID;
    }

    private void Start()
    {
        //Assigning variable to the referrenced variable
        swordEmpty = playerCombat.swordEmpty;

        Origin = MeshRenderer.material.color;

        damage = BASE_DAMAGE;
        defence = BASE_DEFENCE;

        //Adding callbacks for the inventory slot update (every time something happens)
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        sfx = GetComponentInParent<SoundPlayer>();

        health = MAX_HEALTH;

        // list event in GameEvents.cs
        GameEvents.current.onPlayerHealthSet += OnPlayerHealthSet;

        m_Scene = SceneManager.GetActiveScene();
        sceneEventIndex = m_Scene.buildIndex - 1;
        TrueLoadStats(SaveNum);
        VillageEventsManager villageEvents = FindObjectOfType<VillageEventsManager>();
        if (villageEvents) villageEvents.SetEvents();
        Debug.Log("Currently " + playerName + " is playing");
    }

    private void Update()
    {
        //Gets accessory equipped
        if (equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id > -1)
        {
            try { Accessory = ((AccessoryObject)(equipment.database.ItemObjects[equipment.Storage.Slots[(int)EquipSlot.AccessorySlot].item.id])); }
            catch { Accessory = null; }
        }

        if (Accessory && (Accessory.accessory == AccessoryType.RingOfVitality || Accessory.accessory == AccessoryType.Goggles))
        {
            Accessory.UseCurrent(); //Use either ring or goggles  
        }

        Bleed();

        //This ensures that the gameobjects are controlled by Sword Empty GO
        if (swordEmpty.activeInHierarchy) propSwordHolder.SetActive(true);
        else propSwordHolder.SetActive(false);
    }

    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if (scriptedKill) amount = health - 1;
        health -= amount;
        //sfx.PlayAudio();
        UIManager.instance.UpdateHealthBar((int)-amount);
        

        StartCoroutine(EDamageFlash());


        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            //Check if player has revival gem
            var RevGem = inventory.FindItemOnInventory(revivalGem.data);
            if (RevGem != null)
            {
                inventory.database.ItemObjects[RevGem.item.id].UseCurrent();
            }
            else
            {
                playerTargeting.UnTargetObject();
                //gameObject.SetActive(false);
                playerAnimation.Dead();
                playerCombat.EndPoison();
                bleeding = false;
            }
        }
    }

    /*______________________________Damage_Flash_________________________________________*/
    private IEnumerator EDamageFlash()
    {
        MeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(FlashTime);
        MeshRenderer.material.color = Origin;
    }
    /*___________________________________________________________________________________*/

    public void Restart()
    {
        health = MAX_HEALTH;
        UIManager.instance.SetHealthBar(health);
        transform.position = startPos.position;
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        target.TakeDamage(amount, scriptedKill);

        if (target.HasBeenDefeated && Accessory && Accessory.accessory == AccessoryType.NecklaceOfTheLifeStealers) Accessory.UseCurrent();
    }


    public void SetBleed(float _bleedDamage, float _maxBleedTicks, float _bleedDelay)
    {
        bleedDamage = _bleedDamage;
        maxBleedTicks = _maxBleedTicks;
        bleedDelay = _bleedDelay;
        timeSinceLastBleedDamage = 0;
        bleedTicks = 0;
        bleeding = true;
    }

    private void Bleed()
    {
        timeSinceLastBleedDamage += Time.deltaTime;
        if (bleeding && timeSinceLastBleedDamage >= bleedDelay && bleedTicks < maxBleedTicks)
        {
            GetComponent<PlayerStats>().TakeDamage(bleedDamage);
            timeSinceLastBleedDamage = 0;
            bleedTicks++;
        }
        else if (bleeding && bleedTicks >= maxBleedTicks)
        {
            bleeding = false;
        }
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
                switch (p_slot.ItemObject.objType)
                {
                    case ObjectType.Weapon:
                        damage -= ((WeaponObject_Sal)(temp)).damage;
                        spiritualDamage -= ((WeaponObject_Sal)(temp)).spiritualDamage;
                        GetComponent<PlayerMovement_Jerzy>().m_Speed -= ((WeaponObject_Sal)(temp)).speedBoost;
                        break;
                    case ObjectType.Armor:
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
                    switch (p_slot.ItemObject.objType)
                    {
                        case ObjectType.Weapon:
                            damage += ((WeaponObject_Sal)(temp)).damage;
                            spiritualDamage += ((WeaponObject_Sal)(temp)).spiritualDamage;
                            GetComponent<PlayerMovement_Jerzy>().m_Speed += ((WeaponObject_Sal)(temp)).speedBoost;
                            #region Update Weapon on Player
                            if (equipment.GetSlots[(int)EquipSlot.SwordSlot].item.id > -1)
                            {
                                swordEmpty.SetActive(true);

                                if (temp.name == "Iron Sword")
                                { ironSword.SetActive(true); falchion.SetActive(false); stick.SetActive(false); }

                                else if (temp.name == "Sword of the Soulless ones")
                                { ironSword.SetActive(false); falchion.SetActive(true); stick.SetActive(false); }

                                else if (temp.name == "Stick")
                                { ironSword.SetActive(false); falchion.SetActive(false); stick.SetActive(true); }
                            }
                            else swordEmpty.SetActive(false);
                            #endregion
                            break;
                        case ObjectType.Armor:
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
        if (item && item.itemobj.objType != ObjectType.Resource)
        {
            if (equipment.GetSlots[(int)EquipSlot.ItemSlot].item.id == item.itemobj.data.id)
            {
                equipment.GetSlots[(int)EquipSlot.ItemSlot].AddAmount(1);
                Destroy(other.gameObject);
            }
            else if (inventory.AddItem(new Item(item.itemobj), 1))
                Destroy(other.gameObject);  //Only if the item is picked up
        }
        else if (item) //It's a resource
        {
            if (((ResourceObject)(item.itemobj)).resourceType == ResourceType.RevivalGem)
            {
                if (inventory.FindItemOnInventory(item.itemobj.data) != null)
                    Debug.Log("Can't take more Revival gems");
                else
                {
                    if (inventory.AddItem(new Item(item.itemobj), 1))
                        Destroy(other.gameObject);
                }
            }
            else if ((((ResourceObject)(item.itemobj)).resourceType == ResourceType.Gems))
            {
                gems += ((ResourceObject)(item.itemobj)).gems; // edited eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
                if (((ResourceObject)(item.itemobj)).OnUseCurrent != null)
                    ((ResourceObject)(item.itemobj)).UseCurrent();
                UIManager.instance.ShowMoneyPopup();
                Destroy(other.gameObject);
            }
            else
            {
                if (inventory.AddItem(new Item(item.itemobj), 1))
                    Destroy(other.gameObject);  //Only if the item is picked up
            }
        }
    }

    //Morgan's Save Edits
    Scene m_Scene;
    private int sceneEventIndex;
    string sceneName;
    internal int SaveNum;
    internal float X;
    internal float Y;
    internal float Z;
    internal string playerName = "";

    public void SaveStats()
    {
        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
        SaveData saveData = new SaveData(this);
        saveData.LastFileSaved = SaveNum;
        SaveManager.SavePlayerStats(saveData);
        inventory.SaveStats(SaveNum);
        equipment.SaveStats(SaveNum);
        sceneName = m_Scene.name;
        totallynotevents = saveData.totallynotevents;
    }

    public void LoadStats(int num)
    {
        SaveData saveData = SaveManager.LoadPlayerStats(num);
        SaveNum = saveData.LastFileSaved;
        saveData.LastFileSaved = num;
        SceneManager.LoadScene(saveData.scenename);
    }

    public void TrueLoadStats(int num)
    {
        SaveData saveData = SaveManager.LoadPlayerStats(num); //Loads in all player data
        if (saveData != null)
        {
            playerName = SaveManager.LoadPlayerName(num);

            UIManager.instance.SetHealthBar(saveData.health);
            Debug.Log("Player Health Saved: " + saveData.health + " | Before Loading Health: " + health);
            health = saveData.health;

            gems = saveData.gemcount;
            totallynotevents = saveData.totallynotevents;
            if (saveData.levelsComplete.Length == VillageEventsStaticVariables.levelsComplete.Length) 
                VillageEventsStaticVariables.levelsComplete = saveData.levelsComplete;
            inventory.LoadStats(num);
            equipment.LoadStats(num);

            SaveNum = saveData.LastFileSaved;
            saveData.LastFileSaved = num;

            EnemyStats[] dlist = FindObjectsOfType<EnemyStats>(true);
            foreach (EnemyStats Enemy in dlist)
            {
                foreach (var ID in saveData.enemydeadlist)
                {
                    if (Enemy.gameObject.GetInstanceID() == ID)
                        Destroy(Enemy.gameObject);
                }
            }

            //saving chests
            var colist = GameObject.FindGameObjectsWithTag("LootChest");
            foreach (var Chest in colist)
            {
                foreach (var ID in saveData.chestopenedlist)
                {
                    if (Chest.GetInstanceID() == ID)
                        Chest.GetComponent<LootChest_Jerzy>().isInteractable = false;
                    print("LootChest is " + Chest.GetComponent<LootChest_Jerzy>().isInteractable);
                }
            }

            //saving pots
            var plist = GameObject.FindGameObjectsWithTag("Pot");
            foreach (var Pot in plist)
            {
                foreach (var ID in saveData.potbrokenlist)
                {
                    if (Pot.GetInstanceID() == ID)
                        Pot.GetComponent<ScrDestructablePot>().destroyed = true;
                    print("LootChest is " + Pot.GetComponent<ScrDestructablePot>().destroyed);
                }
            }

            // shh
            if (totallynotevents.Count > sceneEventIndex)
            {
                List<bool> savedEvents = totallynotevents[sceneEventIndex];

                if (savedEvents.Count > 0)
                {
                    EventManager[] managers = GameObject.FindObjectsOfType<EventManager>(true);
                    int totalEvents = 0;
                    //loading events
                    for (int em = 0; em < managers.Length; em++)
                    {
                        for (int a = 0; a < managers[em].getamountofactions(); a++)
                        {
                            managers[em].setCompleted(a, savedEvents[totalEvents + a]);
                            if (savedEvents[totalEvents + a])
                            {
                                for (int ev = 0; ev < managers[em].actions[a].events.Count; ev++)
                                {
                                    if (managers[em].actions[a].events[ev].ReplayOnload)
                                        managers[em].actions[a].events[ev].TriggerEvent();
                                }
                            }
                        }
                        totalEvents += managers[em].getamountofactions();
                    }
                }
            }

            if (sceneName == saveData.scenename)
            {
                X = saveData.xpos;
                Y = saveData.ypos;
                Z = saveData.zpos;
                transform.position = new Vector3(X, Y, Z);
            }
            else SaveStats();

        }
        else
        {
            Debug.LogWarning("No Player Save Data exists for: " + SaveNum + ". Making a new one!");
            SaveStats();
        }
    }


    //Morgan's Event Manager: Health Set
    private void OnPlayerHealthSet(int sethealth)
    {
        if (sethealth > MAX_HEALTH) { sethealth = (int)MAX_HEALTH; }
        health = sethealth;
    }

    //Clears the invenotories when game closes 
    private void OnApplicationQuit()
    {
        inventory.Storage.Clear();
        equipment.Storage.Clear();
    }
}
