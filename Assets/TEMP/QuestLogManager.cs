using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogManager : MonoBehaviour
{
    public GameObject ButtonArea;
    public GameObject QuestButton;

    //Add quests to list
    public List<QuestObject> ListOfQuests = new List<QuestObject>();
    private List<GameObject> ListOfButtons = new List<GameObject>();

    //Create an buttons equal to amount of quests
    public void Awake()
    {
        foreach (var item in ListOfQuests)
        {
            ListOfButtons.Add(QuestButton);
        }

        foreach (var item in ListOfButtons)
        {
            Instantiate(QuestButton, ButtonArea.transform);
        }
    }

    public void Update()
    {
            ListOfButtons[0].transform.GetChild(0).GetComponentInChildren<Text>().text.Equals(ListOfQuests[0].name.ToString());
    }

    //Link quests to buttons
    //Make way of specifying certain quests
    //Change button text to quest name
    //Change quest description when quest is clicked
}