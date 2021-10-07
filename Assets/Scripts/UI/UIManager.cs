using UnityEngine;
using UnityEngine.UI;
// Dominique 07-10-2021, Allow us to give the enemy health bar a name
using TMPro;

#if DEBUG // Dominique 07-10-2021, Needed for debugging
using System.Collections;
#endif // DEBUG

/*
 * Created by Mattie Hilton - 03/10/2021 
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
    public void UpdateHealthBar(int healthChange)
    {
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().value += healthChange;
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

    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().maxValue = heartSegments * 3;

        enemyHealthSlider = enemyHealthBarUI.GetComponentInChildren<Slider>();
        enemyNameText = enemyHealthBarUI.GetComponentInChildren<TextMeshProUGUI>();
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
