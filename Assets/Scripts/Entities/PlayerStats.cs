using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class PlayerStats : StatsInterface
{
    private PlayerAnimationManager playerAnimation;
    private PlayerCombat_Jerzy playerCombat;
    private Player_Targeting_Jack playerTargeting;
    private PlayerMovement_Jerzy playerMovement;
    public InventoryObject_Sal inventory;
    public InventoryObject_Sal equipment;
    public GameObject listOfObjs;

    internal AccessoryObject Accessory;
    public ItemObject_Sal revivalGem;

    public List<List<bool>> totallynotevents = new List<List<bool>>();
    public List<List<bool>> savedEnemies = new List<List<bool>>();
    public List<List<bool>> savedPots = new List<List<bool>>();
    public List<List<bool>> savedChests = new List<List<bool>>();
    public List<List<bool>> savedDialogue= new List<List<bool>>();

    public StartPosManager startPos;

    private PlayerSFXPlayer psp;

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
    public GameObject playerBleedEffect;
    public GameObject bleedHealthColour;
    public bool poisonProtection = false;
    public bool desertProtection = false;
    public bool snowProtection = false;
    #endregion

    private void Awake()
    {
        bleedHealthColour = GameObject.Find("HealthBarFill");
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
        playerCombat = GetComponent<PlayerCombat_Jerzy>();
        playerTargeting = GetComponent<Player_Targeting_Jack>();
        playerMovement = GetComponent<PlayerMovement_Jerzy>();
        psp = GetComponent<PlayerSFXPlayer>();
        SaveDataAssistant saveAssisstant = FindObjectOfType<SaveDataAssistant>();
        if (saveAssisstant) SaveNum = saveAssisstant.currentSaveFileID;

        if (!startPos) startPos = FindObjectOfType<StartPosManager>();
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
        sceneName = m_Scene.name;
        TrueLoadStats(SaveNum);
        VillageEventsManager villageEvents = FindObjectOfType<VillageEventsManager>();
        if (villageEvents) villageEvents.SetEvents();
        Debug.Log("Currently " + playerName + " is playing");

        transform.position = startPos.transform.position;

        if (playerBleedEffect.TryGetComponent(out ParticleSystem ps))
        {
            ParticleSystem.MainModule psm = ps.main;
            psm.simulationSpace = ParticleSystemSimulationSpace.Local;
        }

        OnAfterSlotUpdate(equipment.GetSlots[(int)EquipSlot.SwordSlot]);
        OnAfterSlotUpdate(equipment.GetSlots[(int)EquipSlot.ArmorSlot]);
        OnBeforeSlotUpdate(equipment.GetSlots[(int)EquipSlot.SwordSlot]);
        OnBeforeSlotUpdate(equipment.GetSlots[(int)EquipSlot.ArmorSlot]);  
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

        if (HasBeenDefeated) playerMovement.LockPlayerMovement();

        Bleed();

        //This ensures that the gameobjects are controlled by Sword Empty GO
        if (swordEmpty.activeInHierarchy) propSwordHolder.SetActive(true);
        else propSwordHolder.SetActive(false);
    }

    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if (scriptedKill) amount = health - 1;
        health -= amount;

        PlayerSFXPlayer.AudioType prevAudioType = psp.audioType;
        psp.audioType = PlayerSFXPlayer.AudioType.playerHit;
        psp.PlayAudio();
        psp.audioType = prevAudioType;

        UIManager.instance.SetHealthBar(health);
        

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
                //gameObject.SetActive(false);
                playerMovement.LockPlayerMovement();
                playerAnimation.Dead();
                playerCombat.EndPoison();
                bleeding = false;
                playerBleedEffect.SetActive(false);
                playerTargeting.UnTargetObject();
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
        transform.position = startPos.transform.position;
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        target.TakeDamage(amount, scriptedKill);

        if (target.HasBeenDefeated && Accessory && Accessory.accessory == AccessoryType.NecklaceOfTheLifeStealers) Accessory.UseCurrent();
    }


    public void SetBleed(float _bleedDamage, float _maxBleedTicks, float _bleedDelay)
    {
        if (HasBeenDefeated) return;

        bleedDamage = _bleedDamage;
        maxBleedTicks = _maxBleedTicks;
        bleedDelay = _bleedDelay;
        timeSinceLastBleedDamage = 0;
        bleedTicks = 0;
        bleeding = true;
        bleedHealthColour.gameObject.GetComponent<Image>().color = new Color32(80, 0, 0, 255);
        playerBleedEffect.SetActive(true);
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
            playerBleedEffect.SetActive(false);
            bleedHealthColour.gameObject.GetComponent<Image>().color = new Color32(156, 8, 8, 255);
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
                        damage = ((WeaponObject_Sal)(temp)).damage;
                        spiritualDamage = ((WeaponObject_Sal)(temp)).spiritualDamage;
                        GetComponent<PlayerMovement_Jerzy>().m_Speed -= ((WeaponObject_Sal)(temp)).speedBoost;
                        break;
                    case ObjectType.Armor:
                        defence = ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                        poisonProtection = false;
                        desertProtection = false;
                        snowProtection = false;
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
                            damage = ((WeaponObject_Sal)(temp)).damage;
                            spiritualDamage = ((WeaponObject_Sal)(temp)).spiritualDamage;
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
                            defence = ((ArmorObject_Sal)(temp)).defValues.physicalDef;
                            poisonProtection = ((ArmorObject_Sal)(temp)).defValues.poisonProtection;
                            desertProtection = ((ArmorObject_Sal)(temp)).defValues.desertProtection;
                            snowProtection = ((ArmorObject_Sal)(temp)).defValues.snowProtection;
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
            if(item.itemobj.objType == ObjectType.Item) //health drop
            {
                try
                {
                    if (item.itemobj.data.name.entryReference == "Health Drop")
                    {
                        var healthDrop = ((ItemOBJECT)(item.itemobj));
                        health += healthDrop.healingValue;
                        health = Mathf.Clamp(health, 0.0f, MAX_HEALTH);
                        //Update UI
                        UIManager.instance.SetHealthBar((int)health);
                        Destroy(other.gameObject);
                        return;
                    }
                }
                catch { }
            }
            if (equipment.GetSlots[(int)EquipSlot.ItemSlot].item.id == item.itemobj.data.id)
            {
                equipment.GetSlots[(int)EquipSlot.ItemSlot].AddAmount(1);                
                Destroy(other.gameObject);
            }
            else if (inventory.AddItem(new Item(item.itemobj), 1))
            {
                UIManager.instance.ShowItemPickupPopup(item.itemobj);
                Destroy(other.gameObject);  //Only if the item is picked up
            }

            psp.SetAudioType(PlayerSFXPlayer.AudioType.playerCollection);
            psp.collectionType = PlayerSFXPlayer.CollectionType.item;
            psp.PlayAudio();
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
                    {
                        UIManager.instance.ShowItemPickupPopup(item.itemobj);
                        psp.SetAudioType(PlayerSFXPlayer.AudioType.playerCollection);
                        psp.collectionType = PlayerSFXPlayer.CollectionType.gem;
                        psp.PlayAudio();
                        Destroy(other.gameObject);
                    }
                }
            }
            else if ((((ResourceObject)(item.itemobj)).resourceType == ResourceType.Gems))
            {
                gems += ((ResourceObject)(item.itemobj)).gems;
                if (((ResourceObject)(item.itemobj)).OnUseCurrent != null)
                    ((ResourceObject)(item.itemobj)).UseCurrent();
                UIManager.instance.ShowMoneyPopup();
                Destroy(other.gameObject);
                psp.SetAudioType(PlayerSFXPlayer.AudioType.playerCollection);
                psp.collectionType = PlayerSFXPlayer.CollectionType.gem;
                psp.PlayAudio();
            }
            else
            {
                if (inventory.AddItem(new Item(item.itemobj), 1))
                {
                    UIManager.instance.ShowItemPickupPopup(item.itemobj);
                    psp.SetAudioType(PlayerSFXPlayer.AudioType.playerCollection);
                    psp.collectionType = PlayerSFXPlayer.CollectionType.item;
                    psp.PlayAudio();
                    Destroy(other.gameObject);  //Only if the item is picked up
                }
            }
        }
    }

    //Morgan's Save Edits
    Scene m_Scene;
    private int sceneEventIndex;
    string sceneName;
    internal int SaveNum;
    internal string playerName = "";

    public void SaveStats()
    {
        SaveData saveData = new SaveData(this);
        saveData.LastFileSaved = SaveNum;
        SaveManager.SavePlayerStats(saveData);
        inventory.SaveStats(SaveNum);
        equipment.SaveStats(SaveNum);
        FindObjectOfType<QuestLogManager>(true).SaveQuests(SaveNum);
        FindObjectOfType<ShopManager>().SaveShop(SaveNum, sceneEventIndex);
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
            savedPots = saveData.savedPots;
            savedChests = saveData.savedChests;
            savedEnemies = saveData.savedEnemies;
            savedDialogue = saveData.savedDialogue;
            if (saveData.levelsComplete.Length == VillageEventsStaticVariables.levelsComplete.Length) 
                VillageEventsStaticVariables.levelsComplete = saveData.levelsComplete;
            inventory.LoadStats(num);
            equipment.LoadStats(num);
            FindObjectOfType<QuestLogManager>(true).LoadQuests(num);
            FindObjectOfType<ShopManager>().LoadShop(num, sceneEventIndex);

            for (int e = 0; e < equipment.GetSlots.Length; e++)
                equipment.GetSlots[e].OnAfterUpdate.Invoke(equipment.GetSlots[e]);

            SaveNum = saveData.LastFileSaved;
            saveData.LastFileSaved = num;

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
                                    if (managers[em].actions[a].events[ev] == null) continue;
                                    if (managers[em].actions[a].events[ev].ReplayOnload)
                                        managers[em].actions[a].events[ev].TriggerEvent();
                                }
                            }
                        }
                        totalEvents += managers[em].getamountofactions();
                    }
                }
                List<EnemyStats> enemies = FindObjectsOfType<EnemyStats>(true).ToList();
                enemies = enemies.OrderBy(e => e.name).ThenBy(e => e.transform.position.x).ThenBy(e => e.transform.position.y).ThenBy(e => e.transform.position.z).ToList();
                for (int e = 0; e < enemies.Count; e++)
                {
                    enemies[e].isDead = !enemies[e].reviveOnLoad && saveData.savedEnemies[sceneEventIndex][e];
                    enemies[e].gameObject.SetActive(!enemies[e].isDead && enemies[e].gameObject.activeSelf);
                }
            
                List<LootChest_Jerzy> chests = FindObjectsOfType<LootChest_Jerzy>(true).ToList();
                chests = chests.OrderBy(c => c.name).ThenBy(c => c.transform.position.x).ThenBy(c => c.transform.position.y).ThenBy(c => c.transform.position.z).ToList();
                for (int c = 0; c < chests.Count; c++)
                    chests[c].isInteractable = saveData.savedChests[sceneEventIndex][c];

                List<ScrDestructablePot> pots = FindObjectsOfType<ScrDestructablePot>(true).ToList();
                pots = pots.OrderBy(p => p.name).ThenBy(p => p.transform.position.x).ThenBy(p => p.transform.position.y).ThenBy(p => p.transform.position.z).ToList();
                for (int p = 0; p < pots.Count; p++)
                    pots[p].destroyed = saveData.savedPots[sceneEventIndex][p];

                List<DialogueTrigger> dialogues = FindObjectsOfType<DialogueTrigger>(true).ToList();
                dialogues = dialogues.OrderBy(d => d.name).ThenBy(d => d.transform.position.x).ThenBy(d => d.transform.position.y).ThenBy(d => d.transform.position.z).ToList();
                for (int d = 0; d < dialogues.Count; d++)
                {
                    dialogues[d].hasPlayed = saveData.savedDialogue[sceneEventIndex][d];
                    if (dialogues[d].hasPlayed) dialogues[d].GetComponent<Interactable_Object_Jack>().enabled = false;
                }
            }


            startPos.SetPos(saveData.scenename);
            if (sceneName != saveData.scenename) SaveStats();
        }
        else
        {
            Debug.LogWarning("No Player Save Data exists for: " + SaveNum + ". Making a new one!");
            SaveStats();
            // Dominique, Make sure to set player name so it still works in the first scene
            playerName = SaveManager.LoadPlayerName(num);
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
