using UnityEngine;
using TMPro;
using System.Collections;

public class QuestNotification : MonoBehaviour
{
    public GameObject QuestPopup;
    public Animator Anim;

    public TMP_Text QName;
    public TMP_Text QStatus;
    public TMP_Text QMessage;

    public void TriggerNotification(QuestObject questObject, TextMesh questStatus, bool isShown, TextMesh questObjective, float screenDuration)
    {
        SetQuestNotifName(questObject);
        SetQuestStatus(questStatus);
        SetQuestObjective(isShown, questObjective);
        StartCoroutine(LingerOnScreen(screenDuration));
    }

    public void SetQuestNotifName(QuestObject questObject) 
    {
        QName.SetText(questObject.QuestName);
    }

    public void SetQuestStatus(TextMesh questStatus)
    {
        QStatus.SetText(questStatus.text);
    }

    public void SetQuestObjective(bool isShown, TextMesh questObjective)
    {
        if (isShown) { QMessage.gameObject.SetActive(false); }
        
        else { QMessage.gameObject.SetActive(true);
             QMessage.SetText(questObjective.text); }
    }

    public IEnumerator LingerOnScreen(float screenDuration)
    {
        yield return screenDuration;
    }
}