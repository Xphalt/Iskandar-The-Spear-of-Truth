using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NewConversation", fileName = "NewConversation")]
[SerializeField]
public class NewConversation : ScriptableObject
{
    public List<NextCharacter> ListOfCharacterExchanges = new List<NextCharacter>();

    [System.Serializable]
    public struct NextCharacter
    {
        public string CharacterName;

        [TextArea]
        public string[] NumOfSentences;
    }
}