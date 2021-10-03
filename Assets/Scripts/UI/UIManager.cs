using UnityEngine;
using UnityEngine.UI;

/*
 * Created by Mattie Hilton - 03/10/2021 
 */

public class UIManager : MonoBehaviour
{
    #region Health Bar

    // :'( Bork 

    //////////////////////////////////////////////
    // To use the Health Bar UI Updater simply use 'UIManager.healthChange = -1;' with '-1' as an example of taking 1 hp of damage
    //////////////////////////////////////////////

    [Tooltip("Health Bar Slider Parent Prefab")]
    public GameObject HealthBarUI;

    [Header("This is set to be this value times 3, to keep the UI neat. E.G. 1x3 health")]
    public int maxHealth = 1;

    [ContextMenu("Use a postive or negative Int, to affect the health on the next fixed frame")]
    public void UpdateHealthBar(int healthChange)
    {
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().value += healthChange;
    }
    #endregion


    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HealthBarUI.transform.GetChild(0).GetComponent<Slider>().maxValue = maxHealth*3;
    }

}
