using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<NewConversation.NextCharacter> _QueueOfCharacters = new Queue<NewConversation.NextCharacter>();
    private Queue<string[]> _QueueOfStringArrays = new Queue<string[]>();
    private Queue<string> _QueueOfStrings = new Queue<string>();

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
        newDialogue.GetName(newDialogue.ListOfCharacterExchanges[0]);
        TextContinueDialogue.text = "Next";

        _QueueOfCharacters.Clear();
        _QueueOfStringArrays.Clear();
        _QueueOfStrings.Clear();

        AddCharactersToQueue(newDialogue.ListOfCharacterExchanges);
        AddCharacterDialogueToQueue(_QueueOfCharacters);
        AddSentencesToQueue(_QueueOfStringArrays);


        DisplayNextExchange();
    }

    public void DisplayNextExchange()
    {
        //switch (_QueueOfCharacters.Count == 0, _QueueOfStringArrays.Count == 0, _QueueOfStrings.Count == 0)
        //{
        //    case (false, false, true):
        //        _QueueOfStringArrays.Dequeue();
        //        AddSentencesToQueue(_QueueOfStringArrays);
        //        break;

        //    case (false, true, true):
        //        _QueueOfCharacters.Dequeue();
        //        break;

        //    case (true, true, false):
        //        if (_QueueOfStrings.Count == 1)
        //        {
        //            TextContinueDialogue.text = "End";
        //        }
        //        break;

        //    case (true, true, true):
        //        EndDialogue();
        //        break;
        //}

        if (_QueueOfCharacters.Count == 0 && _QueueOfStringArrays.Count == 0 && _QueueOfStrings.Count == 0)
        {
            EndDialogue();
        }

        else if (_QueueOfCharacters.Count == 0 && _QueueOfStringArrays.Count == 0 && _QueueOfStrings.Count == 1)
        {
            TextContinueDialogue.text = "End";
        }

        else
        {
            if (_QueueOfStrings.Count == 0)
            {
                _QueueOfStringArrays.Dequeue();
                AddSentencesToQueue(_QueueOfStringArrays);
            }

            if (_QueueOfStringArrays.Count == 0)
            {
                _QueueOfCharacters.Dequeue();
                TextNPCName.text = _QueueOfCharacters.Peek().CharacterName;
            }

            TextNPCName.text = _QueueOfCharacters.Peek().CharacterName;

            StopAllCoroutines();
            StartCoroutine(TypeSentence(TextDialogueBox.text = _QueueOfStrings.Dequeue()));
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

    private void AddSentencesToQueue(Queue<string[]> characterDialogueQueue)
    {
        for (int i = 0; i < characterDialogueQueue.Peek().Length; i++)
        {
            _QueueOfStrings.Enqueue(characterDialogueQueue.Peek()[i]);
        }
    }
    /*_________________________________________________________________________*/
    #endregion
}