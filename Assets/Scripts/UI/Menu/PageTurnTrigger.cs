using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTurnTrigger : MonoBehaviour
{
    public PauseMenuManager pauseMenuM;

    public void TriggerTurn()
    {
        pauseMenuM.ShowMenu();
    }
}
