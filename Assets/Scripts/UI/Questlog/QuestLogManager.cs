using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuestLogManager : MonoBehaviour
{
    //QuestLogVars
    [HideInInspector] public GameObject ButtonArea;
    [HideInInspector] public Button QuestButton;
    public List<QuestObject> ListOfQuests = new List<QuestObject>();
    [HideInInspector] public List<Button> ListOfButtons = new List<Button>();
    private GameObject QuestInfoText;

    //QuestNotificationVars
    public GameObject QuestPopup;
    public Animator Anim;
    public TMP_Text QName;
    public TMP_Text QStatus;
    public TMP_Text QMessage;

    private void Start()
    {
        QuestPopup.SetActive(false);
        QuestInfoText = GameObject.Find("TextQuestDescription");
    }

    #region QuestLog
    public void AddQuest(QuestObject quest)
    {
        ListOfQuests.Remove(quest);
        QuestButton.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestName.GetLocalisedString();
        Instantiate(QuestButton, ButtonArea.transform).name = quest.QuestName.GetLocalisedString();
    }

    /*_______________________________________Button_Functions______________________________________________*/
    public void ChangeQuestInfo()
    {
        QuestInfoText.GetComponent<TextMeshProUGUI>().text = ListOfQuests.Find(QuestObject => QuestObject.name == gameObject.name).QuestDescription.GetLocalisedString();
    }
    /*_____________________________________________________________________________________________________*/
    #endregion

    #region QuestNotification
    /*____________________________________
     * To trigger the quest notification
     * pass in the correct 
     * variables to trigger and display
     * the correct messages
    _____________________________________*/

    //Trigger for when a quest objective is required
    public void TriggerNotification(QuestObject questObject, string questStatus, bool isShown, string questObjective, float screenDuration)
    {
        QuestPopup.SetActive(true);
        SetQuestNotifName(questObject);
        SetQuestStatus(questStatus);
        SetQuestObjective(questObjective);
        StartCoroutine(LingerOnScreen(screenDuration));
        QuestPopup.SetActive(false);
    }

    //Overloaded trigger for when quest objective is not required
    public void TriggerNotification(QuestObject questObject, string questStatus, float screenDuration)
    {
        QuestPopup.SetActive(true);
        SetQuestNotifName(questObject);
        SetQuestStatus(questStatus);
        QMessage.gameObject.SetActive(false);
        StartCoroutine(LingerOnScreen(screenDuration));
        QuestPopup.SetActive(false);
    }

    public void SetQuestNotifName(QuestObject questObject)
    {
        QName.SetText(questObject.QuestName.GetLocalisedString());
    }

    public void SetQuestStatus(string questStatus)
    {
        QStatus.SetText(questStatus);
    }

    public void SetQuestObjective(string questObjective)
    {
        QMessage.gameObject.SetActive(true);
        QMessage.SetText(questObjective);
    }

    public IEnumerator LingerOnScreen(float screenDuration)
    {
        yield return screenDuration;
    }
    #endregion
}