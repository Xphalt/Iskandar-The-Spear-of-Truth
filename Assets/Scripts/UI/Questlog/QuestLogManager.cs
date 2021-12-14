using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Localization;

public class QuestLogManager : MonoBehaviour
{
    public GameObject ButtonArea;
    public Button QuestButton;

    public List<QuestObject> ListOfQuests = new List<QuestObject>();
    internal List<QuestObject> StartedQuests = new List<QuestObject>();

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

    public void AddQuest(QuestObject quest, bool notify = true)
    {
        // Daniel - Won't notify on load
        // Dominique, Show the player they have a new quest
        questNotification.SetActive(notify);

        StartedQuests.Add(quest); // For saving/loading

        QuestButton.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestName.GetLocalisedString();
        // Dominique, Ensure the quest is added to the top of the log instead of the bottom
        Button newButton = Instantiate(QuestButton, ButtonArea.transform);
        // Dominique, Button name is always in English as it needs to match the scriptable object to get the description
        newButton.name = quest.name;
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


    //Saving/Loading - Daniel
    public void SaveQuests(int num)
    {
        SaveManager.SaveQuests(this, num);
    }

    public void LoadQuests(int num)
    {
        SaveData saveData = SaveManager.LoadQuests(num);

        for (int q = 0; q < saveData.Quests.Count; q++)
        {
            QuestObject nq = new QuestObject();
            nq.IsQuestActive = saveData.Quests[q].IsQuestActive;
            nq.QuestName = saveData.Quests[q].QuestName;
            nq.QuestDescription = saveData.Quests[q].QuestDescription;
            AddQuest(nq, false);
        }
    }
}