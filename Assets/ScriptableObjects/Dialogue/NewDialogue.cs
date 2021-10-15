using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NewDialogue", fileName = "NewDialogue")]
[SerializeField]
public class NewDialogue : ScriptableObject
{
    [System.Serializable]
    public struct NextSentence
    {
        public string NPCName;

        [TextArea]
        public string[] NumOfSentences;
    }

    public List<NextSentence> ListOfNewSentences = new List<NextSentence>();
}