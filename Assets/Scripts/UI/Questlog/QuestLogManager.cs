using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuestLogManager : MonoBehaviour
{
    public GameObject ButtonArea;
    // Dominique, Make sure we get reference through prefab
    [SerializeField] private TextMeshProUGUI QuestInfoText;
    public Button QuestButton;

    public List<QuestObject> ListOfQuests = new List<QuestObject>();

    [HideInInspector] public List<Button> ListOfButtons = new List<Button>();

    public void AddQuest(QuestObject quest)
    {
        QuestButton.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestName.GetLocalisedString();
        Instantiate(QuestButton, ButtonArea.transform).name = quest.QuestName.GetLocalisedString();
    }

    /*_______________________________________Button_Functions______________________________________________*/
    // Dominique, Use the button name instead of gameobject.name since the gameobject is the QuestLogPanel
    public void ChangeQuestInfo(GameObject button)
    {
        QuestObject quest = ListOfQuests.Find(QuestObject => QuestObject.name == button.name);
        string questString = quest.QuestDescription.GetLocalisedString();
        QuestInfoText.SetText(questString);
    }
    /*_____________________________________________________________________________________________________*/
}