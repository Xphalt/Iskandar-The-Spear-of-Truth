using UnityEngine;

public class PageTurnTrigger : MonoBehaviour
{
    public PauseMenuManager pauseMenuM;

    public void ShowMenu()
    {
        pauseMenuM.ShowMenu();
    }

    public void HideMenu()
    {
        pauseMenuM.HideMenu();
    }
}
