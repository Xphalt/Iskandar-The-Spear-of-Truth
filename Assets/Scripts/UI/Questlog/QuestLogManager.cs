using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuestLogManager : MonoBehaviour
{
    public GameObject ButtonArea;
    public Button QuestButton;

    public List<QuestObject> ListOfQuests = new List<QuestObject>();

    [HideInInspector] public List<Button> ListOfButtons = new List<Button>();
    private GameObject QuestInfoText;

    private void Awake()
    {
        QuestInfoText = GameObject.Find("TextQuestDescription");
    }

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
}