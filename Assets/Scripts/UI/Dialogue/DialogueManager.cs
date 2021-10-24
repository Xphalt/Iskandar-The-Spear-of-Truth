using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<NewConversation.NextCharacter> _QueueOfNextCharacters = new Queue<NewConversation.NextCharacter>();
    private Queue<string> _QueueOfNextSentences = new Queue<string>();

    public GameObject DialoguePanel;
    public Button ButtonContinue;
    public Text TextNPCName;
    public Text TextDialogueBox;
    public Text TextContinueDialogue;

    public void Start()
    {
        gameObject.SetActive(false);
        TextNPCName.text = "";
        TextDialogueBox.text = "";
        TextContinueDialogue.text = "";
    }

    public void StartDialogue(NewConversation newDialogue)
    {
        _QueueOfNextCharacters.Clear();
        TextNPCName.text = newDialogue.ListOfCharacterExchanges[0].CharacterName;
        TextContinueDialogue.text = "Next";

        foreach (NewConversation.NextCharacter nextCharacter in newDialogue.ListOfCharacterExchanges)
        {
            _QueueOfNextCharacters.Enqueue(nextCharacter);

            foreach (string sentence in nextCharacter.NumOfSentences)
            {
                _QueueOfNextSentences.Enqueue(sentence);
            }
        }

        DisplayNextExchange();
    }

    public void DisplayNextExchange()
    {
        if (_QueueOfNextSentences.Count != 0)
        {
            NewConversation.NextCharacter nextCharacter = _QueueOfNextCharacters.Dequeue();
            string nextSentence = _QueueOfNextSentences.Dequeue();

            TextNPCName.text = nextCharacter.CharacterName;
            TextDialogueBox.text = nextSentence;

            StopAllCoroutines();
            StartCoroutine(TypeSentence(nextSentence));
        }

        else if (_QueueOfNextSentences.Count == 0 && _QueueOfNextCharacters.Count != 0)
        {
            _QueueOfNextCharacters.Dequeue();
        }

        else if (_QueueOfNextCharacters.Count == 0 && _QueueOfNextSentences.Count == 0)
        {
            TextContinueDialogue.text = "End";
            EndDialogue();

            return;
        } 
    }

    IEnumerator TypeSentence(string sentence)
    {
        TextDialogueBox.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            TextDialogueBox.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        DialoguePanel.SetActive(false);
    }
}