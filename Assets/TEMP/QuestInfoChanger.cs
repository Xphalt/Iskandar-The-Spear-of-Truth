using UnityEngine;
using UnityEngine.UI;

public class QuestInfoChanger : MonoBehaviour
{
    public QuestLogManager QuestLog;
    public GameObject QuestInfoText;

    public void ChangeQuestInfo()
    {
        QuestInfoText.GetComponent<Text>().text = QuestLog.ListOfQuests.Find(QuestObject => QuestObject.name == gameObject.name).QuestDescription;
    }
}