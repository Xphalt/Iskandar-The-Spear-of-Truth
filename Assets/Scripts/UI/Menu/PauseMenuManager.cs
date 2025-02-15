using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    public Animator anim;

    public TextMeshProUGUI leftOption;
    public TextMeshProUGUI rightOption;
    public TextMeshProUGUI title;

    public GameObject equipmentMenu;
    public GameObject inventoryMenu;
    public GameObject questsMenu;
    public GameObject settingsMenu;

    public GameObject audioSettings;
    public GameObject graphicsSettings;
    public GameObject keyboardControls;
    public GameObject quttingConfirmation;
    public GameObject weaponPannel, armourPannel;

    private GameObject playerLight;

    private int numberOfPages = 4;

    private int currentPage = 0;

    public float LerpPauseSpeed = 0.1f;

    [SerializeField] private MoneyPopup equipmentMoney;

    private PlayerStats player;

    private void Start()
    {
        playerLight = GameObject.Find("Player Illumination");
        playerLight.SetActive(false);
        player = FindObjectOfType<PlayerStats>();
        Resume();    
    }

    private void FixedUpdate()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f; //To have no Lerp
        }
        else if (!gameIsPaused && Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
    }

    public void UpdatePauseUI(int pageTurn)
    {
        ChangeMenu(pageTurn);
    }

    #region PauseMenu

    public void TogglePauseState()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
            Pause();
        else Resume();
    }


    void Resume()
    {
        playerLight.SetActive(false);
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }

    void Pause()
    {
        playerLight.SetActive(true);
        pauseMenuUI.SetActive(true);
        currentPage = 0;

        settingsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questsMenu.SetActive(false);

        keyboardControls.SetActive(false);
        quttingConfirmation.SetActive(false);
        graphicsSettings.SetActive(false);
        audioSettings.SetActive(false);

        weaponPannel.SetActive(false); weaponPannel.GetComponent<EquipPanel>().ClearObjects();
        armourPannel.SetActive(false); armourPannel.GetComponent<EquipPanel>().ClearObjects();

        equipmentMenu.SetActive(true);

        // Localised strings
        LocalisationTableReference equipment_string = new LocalisationTableReference();
        equipment_string.tableReference = "ConstantStrings";
        equipment_string.entryReference = "Equipment";

        LocalisationTableReference settings_string = new LocalisationTableReference();
        settings_string.tableReference = "ConstantStrings";
        settings_string.entryReference = "Settings";

        LocalisationTableReference quests_string = new LocalisationTableReference();
        quests_string.tableReference = "ConstantStrings";
        quests_string.entryReference = "Quests";

        title.text = equipment_string.GetLocalisedString();
        leftOption.text = settings_string.GetLocalisedString();
        rightOption.text = quests_string.GetLocalisedString();

        // Update the money value in the equipment screen
        equipmentMoney.SetNumber(GameObject.FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>().Gems);
    }

    public void LoadMenu()
    {
        //Get items left over
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        GroundItem[] groundObjs = FindObjectsOfType<GroundItem>();
        foreach (GroundItem item in groundObjs)
        {
            if (item.itemobj.objType != ObjectType.Resource)
            {
                if (item.itemobj.data.name.entryReference == "Health Drop")
                {
                    Destroy(item.gameObject);
                    continue;
                }
                if (player.equipment.GetSlots[(int)EquipSlot.ItemSlot].item.id == item.itemobj.data.id)
                {
                    stats.equipment.GetSlots[(int)EquipSlot.ItemSlot].AddAmount(1);
                    Destroy(item.gameObject);
                }
                else if (stats.inventory.AddItem(new Item(item.itemobj), 1))
                    Destroy(item.gameObject);  //Only if the item is picked up 
            }
            else //It's a resource
            {
                if (((ResourceObject)(item.itemobj)).resourceType == ResourceType.RevivalGem)
                {
                    if (stats.inventory.FindItemOnInventory(item.itemobj.data) != null)
                    { }
                    else if (stats.inventory.AddItem(new Item(item.itemobj), 1))
                        Destroy(item.gameObject);
                }
                else if ((((ResourceObject)(item.itemobj)).resourceType == ResourceType.Gems)) 
                    Destroy(item.gameObject); 
                else if (stats.inventory.AddItem(new Item(item.itemobj), 1))
                    Destroy(item.gameObject);  //Only if the item is picked up  
            }
        }

        if (player) player.SaveStats();
        Time.timeScale = 1f;
        gameIsPaused = false;
        Debug.Log("Fading Out");
        UIManager.instance.GetBlackoutScreen().FadeOutOfScene(0);
        UIManager.instance.GetBlackoutScreen().GetLoadingScreen();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    void ChangeMenu(int pageTurn)
    {
        currentPage += pageTurn;

        if (currentPage < 0)
            currentPage = numberOfPages - 1;

        if (currentPage > numberOfPages - 1)
            currentPage = 0;

        if (pageTurn == 1)
            anim.SetTrigger("PanLeft");
        else anim.SetTrigger("PanRight");        
    }

    public void HideMenu()
    {
        settingsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questsMenu.SetActive(false);
        equipmentMenu.SetActive(false);

        keyboardControls.SetActive(false);
        quttingConfirmation.SetActive(false);
        graphicsSettings.SetActive(false);
        audioSettings.SetActive(false);
    }

    public void ShowMenu()
    {
        // Dominique 11-11-2021, Ensure navigation strings are localised
        LocalisationTableReference equipment_string = new LocalisationTableReference();
        equipment_string.tableReference = "ConstantStrings";
        equipment_string.entryReference = "Equipment";

        LocalisationTableReference settings_string = new LocalisationTableReference();
        settings_string.tableReference = "ConstantStrings";
        settings_string.entryReference = "Settings";

        LocalisationTableReference quests_string = new LocalisationTableReference();
        quests_string.tableReference = "ConstantStrings";
        quests_string.entryReference = "Quests";

        LocalisationTableReference inventory_string = new LocalisationTableReference();
        inventory_string.tableReference = "ConstantStrings";
        inventory_string.entryReference = "Inventory";

        switch (currentPage)
        {
            case 0:
                equipmentMenu.SetActive(true);
                title.text = equipment_string.GetLocalisedString();
                leftOption.text = settings_string.GetLocalisedString();
                rightOption.text = quests_string.GetLocalisedString();
                // Update the money value in the equipment screen
                equipmentMoney.SetNumber(GameObject.FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>().Gems);
                break;
            case 1:
                questsMenu.SetActive(true);
                title.text = quests_string.GetLocalisedString();
                leftOption.text = equipment_string.GetLocalisedString();
                rightOption.text = inventory_string.GetLocalisedString();
                break;
            case 2:
                inventoryMenu.SetActive(true);
                title.text = inventory_string.GetLocalisedString();
                leftOption.text = quests_string.GetLocalisedString();
                rightOption.text = settings_string.GetLocalisedString();
                break;
            case 3:
                settingsMenu.SetActive(true);
                title.text = settings_string.GetLocalisedString();
                leftOption.text = inventory_string.GetLocalisedString();
                rightOption.text = equipment_string.GetLocalisedString();
                break;
            default:
                break;
        }
    }
}