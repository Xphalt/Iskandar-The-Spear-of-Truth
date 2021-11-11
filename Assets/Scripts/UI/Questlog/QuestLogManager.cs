using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestLogManager : MonoBehaviour
{
    [HideInInspector] public GameObject ButtonArea;
    [HideInInspector] public Button QuestButton;

    public List<QuestObject> ListOfQuests = new List<QuestObject>();
    [HideInInspector] public List<Button> ListOfButtons = new List<Button>();

    private void Awake()
    {
        foreach (var item in ListOfQuests)
        {
            ListOfButtons.Add(QuestButton);
            QuestButton.GetComponentInChildren<TextMeshProUGUI>().text = item.QuestName.GetLocalisedString();
            Instantiate(QuestButton, ButtonArea.transform).name = item.QuestName.GetLocalisedString();
        }
    }
}