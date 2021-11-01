using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<NewConversation.NextCharacter> _QueueOfCharacters = new Queue<NewConversation.NextCharacter>();
    private Queue<string[]> _QueueOfStringArrays = new Queue<string[]>();
    private Queue<string> _QueueOfStrings = new Queue<string>();

    private NewConversation newConversation;

    public GameObject DialoguePanel;
    public Button ButtonContinue;
    public Text TextNPCName;
    public Text TextDialogueBox;
    public Text TextContinueDialogue;

    public void Start()
    {
        DialoguePanel.SetActive(false);
        TextNPCName.text = "";
        TextDialogueBox.text = "";
        TextContinueDialogue.text = "";
    }

    public void StartDialogue(NewConversation newDialogue)
    {
        newConversation = newDialogue;

        TextContinueDialogue.text = "Next";

        _QueueOfCharacters.Clear();
        _QueueOfStringArrays.Clear();
        _QueueOfStrings.Clear();

        AddCharactersToQueue(newConversation.ListOfCharacterExchanges);
        AddCharacterDialogueToQueue(_QueueOfCharacters);
        AddSentencesToQueue(_QueueOfStringArrays.Dequeue());
        TextNPCName.text = _QueueOfCharacters.Peek().CharacterName;

        DisplayNextExchange();
    }

    public void DisplayNextExchange()
    {
        if (_QueueOfCharacters.Count == 0 && _QueueOfStrings.Count == 0)
        {
            EndDialogue();
        }

        else
        {

            if (_QueueOfStrings.Count == 0)
            {
                AddSentencesToQueue(_QueueOfStringArrays.Dequeue());

                if (_QueueOfStringArrays.Count == 0)
                {
                    TextNPCName.text = _QueueOfCharacters.Dequeue().CharacterName;
                }
            }

            if (_QueueOfStrings.Count != 0)
            {
                StopAllCoroutines();
                StartCoroutine(TypeSentence(TextDialogueBox.text = _QueueOfStrings.Dequeue()));
            }

            if (_QueueOfCharacters.Count != 0 && _QueueOfStringArrays.Count == 0)
            {
                TextNPCName.text = _QueueOfCharacters.Dequeue().CharacterName;
            }

            if (_QueueOfCharacters.Count == 0 && _QueueOfStringArrays.Count == 0 && _QueueOfStrings.Count == 0)
            {
                TextContinueDialogue.text = "End";
            }

            Debug.Log(_QueueOfCharacters.Count);
            Debug.Log(_QueueOfStringArrays.Count);
            Debug.Log(_QueueOfStrings.Count);
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

    #region Queues
    /*_________________________________________________________________________*/
    private void AddCharactersToQueue(List<NewConversation.NextCharacter> characterList)
    {
        foreach (NewConversation.NextCharacter character in characterList)
        {
            _QueueOfCharacters.Enqueue(character);
        }
    }

    private void AddCharacterDialogueToQueue(Queue<NewConversation.NextCharacter> characterQueue)
    {
        foreach (NewConversation.NextCharacter characterDialogue in characterQueue)
        {
            _QueueOfStringArrays.Enqueue(characterDialogue.NumOfSentences);
        }
    }

    private void AddSentencesToQueue(string[] characterDialogueQueue)
    {
        foreach (string item in characterDialogueQueue)
        {
            _QueueOfStrings.Enqueue(item);
        }
    }
    /*_________________________________________________________________________*/
    #endregion
}