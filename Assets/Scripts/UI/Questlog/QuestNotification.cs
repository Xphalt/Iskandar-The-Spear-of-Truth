using System.Collections;
using TMPro;
using UnityEngine;

public class QuestNotification : MonoBehaviour
{
    public GameObject QuestPopup;
    public Animator Anim;
    public TMP_Text HeaderName;
    public TMP_Text QuestName;
    public TMP_Text QuestObjective;
    public LocalisationTableReference NewQuestText;
    public LocalisationTableReference UpdatedQuestText;
    public LocalisationTableReference CompletedQuestText;

    public enum TypeOfMessage
    {
        NewQuest,
        UpdatedQuest,
        CompletedQuest
    }

    [SerializeField] TypeOfMessage Message;

    private void Awake()
    {
        QuestPopup.SetActive(false);
    }

    public void TriggerNotification(QuestObject name, LocalisationTableReference questObjective, float screenDuration)
    {
        QuestPopup.SetActive(true);
        QuestName.SetText(name.QuestName.GetLocalisedString());

        switch (Message)
        {
            case TypeOfMessage.NewQuest:
                HeaderName.SetText(NewQuestText.GetLocalisedString());
                QuestObjective.SetText(questObjective.GetLocalisedString());
                break;

            case TypeOfMessage.UpdatedQuest:
                HeaderName.SetText(UpdatedQuestText.GetLocalisedString());
                QuestObjective.SetText(questObjective.GetLocalisedString());
                break;

            case TypeOfMessage.CompletedQuest:
                HeaderName.SetText(CompletedQuestText.GetLocalisedString());
                break;
        }

        LingerOnScreen(screenDuration);
        QuestPopup.SetActive(false);
    }

    public IEnumerator LingerOnScreen(float screenDuration)
    {
        yield return screenDuration;
    }
}