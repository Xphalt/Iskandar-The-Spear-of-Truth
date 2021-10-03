using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Created by Mattie Hilton - 03/10/2021 
 */
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        GameIsPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
