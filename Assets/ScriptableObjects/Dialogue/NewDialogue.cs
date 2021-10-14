using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NewDialogue", fileName = "NewDialogue")]
[SerializeField]
public class NewDialogue : ScriptableObject
{
    public string NPCName;

    [TextArea]
    public string[] NumOfSentences;
}