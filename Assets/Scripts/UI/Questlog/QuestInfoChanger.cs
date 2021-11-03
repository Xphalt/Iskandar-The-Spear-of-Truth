using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestInfoChanger : MonoBehaviour
{
    public QuestLogManager QuestLog;
    private GameObject QuestInfoText;

    private void Start()
    {
        QuestInfoText = GameObject.Find("TextQuestDescription");
    }

    public void ChangeQuestInfo()
    {
        QuestInfoText.GetComponent<TextMeshProUGUI>().text = QuestLog.ListOfQuests.Find(QuestObject => QuestObject.name == gameObject.name).QuestDescription;
    }
}