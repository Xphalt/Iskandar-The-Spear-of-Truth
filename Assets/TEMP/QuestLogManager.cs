using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogManager : MonoBehaviour
{
    public GameObject ButtonArea;
    public Button QuestButton;

    //Add quests to list
    public List<QuestObject> ListOfQuests = new List<QuestObject>();
    //[HideInInspector]
    public List<Button> ListOfButtons = new List<Button>();
    public Dictionary<QuestObject, Button> QuestDictionary = new Dictionary<QuestObject, Button>();

    private void Awake()
    {
        foreach (var item in ListOfQuests)
        {
            ListOfButtons.Add(QuestButton);
            QuestButton.GetComponentInChildren<Text>().text = item.QuestName;
            Instantiate(QuestButton, ButtonArea.transform).name = item.QuestName;
        }

        for (int i = 0; i < ListOfQuests.Count; i++)
        {
            QuestDictionary.Add(ListOfQuests[i], ListOfButtons[i]);
        }
    }
}