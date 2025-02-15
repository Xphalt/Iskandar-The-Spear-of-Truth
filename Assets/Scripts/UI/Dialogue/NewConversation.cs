using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/Dialogue/NewConversation", fileName = "NewDialogue")]
[SerializeField]
public class NewConversation : ScriptableObject
{
    public List<NextCharacter> ListOfCharacterExchanges = new List<NextCharacter>();

    [System.Serializable]
    public struct NextCharacter
    {
        public string CharacterName;

        public LocalisationTableReference[] NumOfSentences;
    }

    public string GetName(NewConversation.NextCharacter newCharacter) { return newCharacter.CharacterName; }
}