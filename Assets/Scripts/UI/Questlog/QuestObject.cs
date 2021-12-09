using UnityEngine;

[CreateAssetMenu(menuName = "UI/Questing/Quest", fileName = "NewQuest")]
[System.Serializable]
public class QuestObject : ScriptableObject
{
    [Space(10)]
    [Header("Quest Information")]

    [Tooltip("The name of the quest.")]
    public LocalisationTableReference QuestName;

    [Space(10)]
    [Tooltip("Shows the status of the quest")]
    public bool IsQuestActive;

    [Space(10)]
    [Tooltip("The description shown when a quest is selected in the quest log.")]
    public LocalisationTableReference QuestDescription;

    [Space(10)]
    [Tooltip("The rewards given to the player once the quest is completed.")]
    [System.NonSerialized]
    public ItemObj[] QuestReward;
}

[System.Serializable]
public class QuestSave
{
    public bool IsQuestActive;
    public LocalisationTableReference QuestName;
    public LocalisationTableReference QuestDescription;
}
