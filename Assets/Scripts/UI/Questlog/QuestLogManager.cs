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

    [SerializeField] private TextMeshProUGUI NoQuestsString;
    [SerializeField] private GameObject questNotification;

    // Dominique, If there are no quests show a string for that
    private void OnEnable()
    {
        if (ListOfButtons.Count > 0)
        {
            NoQuestsString.gameObject.SetActive(false);
        }
        else
        {
            NoQuestsString.gameObject.SetActive(true);
        }
    }

    public void AddQuest(QuestObject quest)
    {
        // Dominique, Show the player they have a new quest
        questNotification.SetActive(true);

        QuestButton.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestName.GetLocalisedString();
        // Dominique, Ensure the quest is added to the top of the log instead of the bottom
        Button newButton = Instantiate(QuestButton, ButtonArea.transform);
        newButton.name = quest.QuestName.GetLocalisedString();
        newButton.transform.SetAsFirstSibling();

        // Go through all previous quests and set them to complete
        for (int i = 0; i < ListOfButtons.Count; i++)
        {
            QuestStatus currentQuest = ListOfButtons[i].GetComponent<QuestStatus>();
            currentQuest.SetComplete();
        }

        ListOfButtons.Add(newButton);
    }

    /*_______________________________________Button_Functions______________________________________________*/
    // Dominique, Use the button name instead of gameobject.name since the gameobject is the QuestLogPanel
    public void ChangeQuestInfo(GameObject button)
    {
        QuestObject quest = ListOfQuests.Find(QuestObject => QuestObject.name == button.name);
        string questString = quest.QuestDescription.GetLocalisedString();
        // Dominique, For some reason trying to store this text to update it won't work so we'll have to get it here :((
        GameObject.Find("TextQuestDescription").GetComponent<TextMeshProUGUI>().text = questString;
    }
    /*_____________________________________________________________________________________________________*/
}