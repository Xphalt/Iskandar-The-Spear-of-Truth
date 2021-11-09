using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifTest : MonoBehaviour
{
    public QuestObject Quest;
    public string status;
    public string objective;
    public bool test = false;

    private void Update()
    {
        if (test)
        {
            UIManager.instance.TriggerNotification(Quest, status, true, objective, 5);
        }
    }
}