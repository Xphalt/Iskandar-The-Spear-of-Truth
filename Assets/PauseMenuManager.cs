using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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
    private int numberOfPages = 4;

    private int currentPage = 0;

    public float LerpPauseSpeed = 0.1f;

    [SerializeField] private MoneyPopup equipmentMoney;

    private void Start()
    {
        Resume();    
    }

    private void FixedUpdate()
    {
        if (gameIsPaused && Time.timeScale > 0)
        {
            //Time.timeScale -=LerpPauseSpeed; //To Lerp
            Time.timeScale = 0f; //To have no Lerp
        }
        else if (!gameIsPaused && Time.timeScale < 1)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, LerpPauseSpeed);

            if (Time.timeScale > 0.98f)
                Time.timeScale = 1f;
        }
    }

    public void UpdatePauseUI(int pageTurn)
    {
        StartCoroutine(ChangeMenu(pageTurn));
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
        if(Time.timeScale < 1)
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, LerpPauseSpeed);
        pauseMenuUI.SetActive(false);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        currentPage = 0;

        settingsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questsMenu.SetActive(false);

        equipmentMenu.SetActive(true);
        title.text = "Equipment";
        leftOption.text = "Settings";
        rightOption.text = "Quests";
        // Update the money value in the equipment screen
        equipmentMoney.SetNumber(GameObject.FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>().Gems);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        gameIsPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    IEnumerator ChangeMenu(int pageTurn)
    {
        currentPage += pageTurn;

        if (currentPage < 0)
            currentPage = numberOfPages - 1;

        if (currentPage > numberOfPages - 1)
            currentPage = 0;

        if (pageTurn == -1)
            anim.SetTrigger("PanLeft");
        else anim.SetTrigger("PanRight");

        yield return new WaitForSecondsRealtime(0.8f);

        settingsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questsMenu.SetActive(false);
        equipmentMenu.SetActive(false);

        ShowMenu();
    }

    public void ShowMenu()
    {
        switch (currentPage)
        {
            case 0:
                equipmentMenu.SetActive(true);
                title.text = "Equipment";
                leftOption.text = "Settings";
                rightOption.text = "Quests";
                // Update the money value in the equipment screen
                equipmentMoney.SetNumber(GameObject.FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>().Gems);
                break;
            case 1:
                questsMenu.SetActive(true);
                title.text = "Quests";
                leftOption.text = "Equipment";
                rightOption.text = "Inventory";
                break;
            case 2:
                inventoryMenu.SetActive(true);
                title.text = "Inventory";
                leftOption.text = "Quests";
                rightOption.text = "Settings";
                break;
            case 3:
                settingsMenu.SetActive(true);
                title.text = "Settings";
                leftOption.text = "Inventory";
                rightOption.text = "Equipment";
                break;
            default:
                break;
        }
    }
}