using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue", fileName = "Dialogue")]
[System.Serializable]
public class Dialogue
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