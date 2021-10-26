using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Questing/Quest", fileName = "NewQuest")]
[System.Serializable]
public class QuestObject : ScriptableObject
{
    [Space(10)]
    [Header("Quest Information")]

    [Tooltip("The name of the quest.")]
    public string QuestName;

    [Space(10)]
    [Tooltip("Shows the status of the quest")]
    public bool IsQuestActive;

    [Space(10)]
    [Tooltip("The description shown when a quest is selected in the quest log.")]
    [TextArea]
    public string QuestDescription;

    [Space(10)]
    [Tooltip("The rewards given to the player once the quest is completed.")]
    public ItemObject[] QuestReward;

}