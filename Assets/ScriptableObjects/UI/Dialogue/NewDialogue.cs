using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NewDialogue", fileName = "NewDialogue")]
[SerializeField]
public class NewDialogue : ScriptableObject
{
    public List<NextExchange> ListOfCharacterExchanges = new List<NextExchange>();

    [System.Serializable]
    public struct NextExchange
    {
        public string NPCName;

        [TextArea]
        public string[] NumOfSentences;
    }
}