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

    public TextMeshProUGUI leftOption;
    public TextMeshProUGUI rightOption;
    public TextMeshProUGUI title;

    public GameObject equipmentMenu;
    public GameObject inventoryMenu;
    public GameObject questsMenu;
    public GameObject settingsMenu;
    private int numberOfPages = 4;

    private int currentPage = 0;

    private void Start()
    {
        Resume();
    }

    public void UpdatePauseUI(int pageTurn)
    {
        equipmentMenu.SetActive(false);
        questsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        inventoryMenu.SetActive(false);

        currentPage += pageTurn;

        if (currentPage < 0)
            currentPage = numberOfPages-1;

        if (currentPage > numberOfPages-1)
            currentPage = 0;

        switch (currentPage)
        {
            case 0:
                equipmentMenu.SetActive(true);
                title.text = "Equipment";
                leftOption.text = "Settings";
                rightOption.text = "Quests";
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        currentPage = 0;
        UpdatePauseUI(0);
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
}
