using UnityEngine;
using UnityEngine.UI;
// Dominique 07-10-2021, Allow us to give the enemy health bar a name
using TMPro;

#if DEBUG // Dominique 07-10-2021, Needed for debugging
using System.Collections;
#endif // DEBUG

/*
 * Created by Mattie Hilton - 03/10/2021 
 * Edited by Mattie Hilton - 02/11/2021
 */

public class UIManager : MonoBehaviour
{
    #region Health Bar

    //////////////////////////////////////////////
    // To use the Health Bar UI Updater simply use 'UIManager.UpdateHealthBar(-1);' with '-1' as an example of taking 1 hp of damage
    //////////////////////////////////////////////

    [Tooltip("Health Bar Slider Parent Prefab")]
    public GameObject HealthBarUI;

    [Header("How many segments each heart has - 1 damage removes 1 segment")]
    public int heartSegments = 1;

    [ContextMenu("Use a postive or negative Int, to affect the health on the next fixed frame")]
    public void UpdateHealthBar(float healthChange)
    {
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().value += Mathf.CeilToInt(healthChange);
    }

    public void SetHealthBar(float newHealth)
    {
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().value = Mathf.CeilToInt(newHealth);
    }
    #endregion

    // Dominique 07-10-2021, We can now setup the health bar for a boss with their name
    // Show an addition/loss of health with UpdateEnemyHealthBar(value)
    #region Enemy Health Bar
    public GameObject enemyHealthBarUI;
    private TextMeshProUGUI enemyNameText;
    private Slider enemyHealthSlider;

    public void SetupEnemyHealthBar(string enemyName, int enemyMaxHealth)
    {
        enemyNameText.text = enemyName;
        enemyHealthSlider.maxValue = enemyMaxHealth;
        enemyHealthSlider.value = enemyMaxHealth;

        ToggleEnemyHealthBar(true);
    }
    public void UpdateEnemyHealthBar(int healthChange)
    {
        // Make sure we don't make the slider value -ve
        if (enemyHealthSlider.value + healthChange < enemyHealthSlider.minValue)
        {
            enemyHealthSlider.value = enemyHealthSlider.minValue;
        }
        // Make sure slider doesn't go above max value
        else if (enemyHealthSlider.value + healthChange > enemyHealthSlider.maxValue)
        {
            enemyHealthSlider.value = enemyHealthSlider.maxValue;
        }
        else
        {
            enemyHealthSlider.value += healthChange;
        }
    }
    // So we can hide the health bar when the enemy dies
    public void ToggleEnemyHealthBar(bool visible)
    {
        enemyHealthBarUI.SetActive(visible);
    }

    #endregion

    // Dominique 14-10-2021, Enables/disables UI so we have input specific objects in use
    #region Input Specific UI
    [SerializeField] private GameObject[] sharedUIElements;
    [SerializeField] private GameObject[] gamepadUIElements;
    [SerializeField] private GameObject[] keyboardMouseUIElements;
    [SerializeField] private GameObject[] mobileUIElements;

    public enum INPUT_OPTIONS
    {
        GAMEPAD,
        KEYBOAD_AND_MOUSE,
        MOBILE,
        NUM_OPTIONS,
    };

    private INPUT_OPTIONS current_input = INPUT_OPTIONS.NUM_OPTIONS;
    public INPUT_OPTIONS GetCurrentInput() { return current_input; }

    public void SetUIForInput(INPUT_OPTIONS input)
    {
        // Make sure we don't do all this unless we need to!
        if (current_input != input)
        {
            current_input = input;

            // Disable everything first so that shared elements will still be enabled
            for (int i = 0; i < sharedUIElements.Length; i++)
            {
                sharedUIElements[i].SetActive(false);
            }
            for (int i = 0; i < gamepadUIElements.Length; i++)
            {
                gamepadUIElements[i].SetActive(false);
            }
            for (int i = 0; i < keyboardMouseUIElements.Length; i++)
            {
                keyboardMouseUIElements[i].SetActive(false);
            }
            for (int i = 0; i < mobileUIElements.Length; i++)
            {
                mobileUIElements[i].SetActive(false);
            }

            // Now enable the correct objects for this UI
            GameObject[] enableObjects = new GameObject[0];
            switch (input)
            {
                case INPUT_OPTIONS.GAMEPAD:
                    enableObjects = gamepadUIElements;
                    break;
                case INPUT_OPTIONS.KEYBOAD_AND_MOUSE:
                    enableObjects = keyboardMouseUIElements;
                    break;
                case INPUT_OPTIONS.MOBILE:
                    enableObjects = mobileUIElements;
                    break;
            }
            for (int i = 0; i < enableObjects.Length; i++)
            {
                enableObjects[i].SetActive(true);
            }
        }

        // Ensure the icon above targetable enemies updates according to the input type
        targetingIcon.SetSprite(input);
    }
    #endregion

    #region Action Button Image Changing
    [SerializeField] private ActionImageChanger actionImageChanger;

    public void UpdateActionButtonImage(Interactable_Object_Jack.InteractableType interactableType)
    {
        actionImageChanger.SetInteractImage(interactableType);
    }
    #endregion

    #region Targeting Icon
    [SerializeField] private TargetingIcon targetingIcon;

    public void EnableTargetingIcon(Transform transform, TargetingIcon.TARGETING_TYPE targetingType)
    {
        targetingIcon.gameObject.SetActive(true);
        targetingIcon.SetTarget(transform, targetingType);
    }
    public void DisableTargetingIcon()
    {
        targetingIcon.SetTarget(null, TargetingIcon.TARGETING_TYPE.NUM_TARGETING_TYPES);
        targetingIcon.gameObject.SetActive(false);
    }
    #endregion // Targeting Icon

    #region Money Popup
    [SerializeField] private MoneyPopup moneyPopup;
    private PlayerStats playerStats;
    public void ShowMoneyPopup()
    {
        moneyPopup.SetNumber(playerStats.Gems);
        moneyPopup.gameObject.SetActive(true);
    }
    #endregion // Money Popup

    #region QuestNotification
    /*____________________________________
     * To trigger the quest notification
     * call the instance of the ui manager
     * and call the TriggerNotification
     * function. Pass in the correct 
     * variables to trigger and display
     * the correct messages
    _____________________________________*/

    public QuestNotification QuestNotif;

    public GameObject QuestPopup;
    public Animator Anim;

    public TMP_Text QName;
    public TMP_Text QStatus;
    public TMP_Text QMessage;

    public void TriggerNotification(QuestObject questObject, string questStatus, bool isShown, string questObjective, float screenDuration)
    {
        QuestPopup.SetActive(true);
        SetQuestNotifName(questObject);
        SetQuestStatus(questStatus);
        SetQuestObjective(isShown, questObjective);
        StartCoroutine(LingerOnScreen(screenDuration));
        QuestPopup.SetActive(false);
    }

    public void SetQuestNotifName(QuestObject questObject)
    {
        QName.SetText(questObject.QuestName);
    }

    public void SetQuestStatus(string questStatus)
    {
        QStatus.SetText(questStatus);
    }

    public void SetQuestObjective(bool isShown, string questObjective)
    {
        if (isShown) { QMessage.gameObject.SetActive(false); }

        else
        {
            QMessage.gameObject.SetActive(true);
            QMessage.SetText(questObjective);
        }
    }

    public IEnumerator LingerOnScreen(float screenDuration)
    {
        yield return screenDuration;
    }
    #endregion

    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        QuestPopup.SetActive(false);
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().maxValue = heartSegments;

        enemyHealthSlider = enemyHealthBarUI.GetComponentInChildren<Slider>();
        enemyNameText = enemyHealthBarUI.GetComponentInChildren<TextMeshProUGUI>();

        playerStats = GameObject.FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>();
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().maxValue = playerStats.MAX_HEALTH;

        // At the moment we're using keyboard and mouse to play the game
#if UNITY_ANDROID
        SetUIForInput(INPUT_OPTIONS.MOBILE);
#else
        SetUIForInput(INPUT_OPTIONS.KEYBOAD_AND_MOUSE);
#endif
    }

#if DEBUG // Dominique 07-10-2021, Use to test enemy health bar (make sure to SetupEnemyHealthBar first)
    IEnumerator TestEnemyBar()
    {
        for (int i = 0; i < 10; i++)
        {
            UpdateEnemyHealthBar(-100);
            yield return new WaitForSeconds(1);
        }
    }
#endif // DEBUG
}
