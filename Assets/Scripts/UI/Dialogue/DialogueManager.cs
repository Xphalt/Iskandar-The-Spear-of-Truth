using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<NewConversation.NextCharacter> _QueueOfCharacters = new Queue<NewConversation.NextCharacter>();
    private Queue<LocalisationTableReference[]> _QueueOfStringArrays = new Queue<LocalisationTableReference[]>();
    private Queue<string> _QueueOfStrings = new Queue<string>();

    private Collider currentCollider;
    private NewConversation newConversation;
    public NewConversation NewConversation
    {
        get => newConversation;
    }
    private bool conversationIsEnded = true;
    public bool ConversationIsEnded
    {
        get => conversationIsEnded;
    }

    public GameObject DialoguePanel;
    public Button ButtonContinue;
    public Button ButtonSkip;
    public TextMeshProUGUI TextNPCName;
    public TextMeshProUGUI TextDialogueBox;
    public TextMeshProUGUI TextContinueDialogue;

    private PlayerInput playerInput;
    private PlayerStats playerStats;

    public void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        playerStats = FindObjectOfType<PlayerStats>();

        DialoguePanel.SetActive(false);
        TextNPCName.text = "";
        TextDialogueBox.text = "";
        TextContinueDialogue.text = "";
    }

    //morgan's Event Manager Edit
    public void update()
    {
        //if (DialoguePanel.SetActive(false) = true)
        //{
        //    GameEvents.current.onLockPlayerInputs += OnNPCDialogue;
        //}
    }

    public void StartDialogue(Collider newCollider, NewConversation newDialogue)
    {
        playerInput.TogglePlayerInteraction(false);

        conversationIsEnded = false;

        currentCollider = newCollider;
        newConversation = newDialogue;

        LocalisationTableReference nextString;
        nextString.entryReference = "Next";
        nextString.tableReference = "ConstantStrings";
        TextContinueDialogue.text = nextString.GetLocalisedString();

        _QueueOfCharacters.Clear();
        _QueueOfStringArrays.Clear();
        _QueueOfStrings.Clear();

        AddCharactersToQueue(newConversation.ListOfCharacterExchanges);
        AddCharacterDialogueToQueue(_QueueOfCharacters);
        AddSentencesToQueue(_QueueOfStringArrays.Dequeue());
        string characterName = _QueueOfCharacters.Dequeue().CharacterName;
        if (characterName.Equals("Son"))
        {
            TextNPCName.text = playerStats.playerName;
        }
        else
        {
            TextNPCName.text = characterName;
        }

        DisplayNextExchange();
    }

    public void DisplayNextExchange()
    {
        if (_QueueOfCharacters.Count == 0 && _QueueOfStrings.Count == 0)
        {
            EndDialogue();
            conversationIsEnded = true;
        }
        else
        {

            if (_QueueOfStrings.Count == 0)
            {
                AddSentencesToQueue(_QueueOfStringArrays.Dequeue());
                string characterName = _QueueOfCharacters.Dequeue().CharacterName;
                if (characterName.Equals("Son"))
                {
                    TextNPCName.text = playerStats.playerName;
                }
                else
                {
                    TextNPCName.text = characterName;
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
                LocalisationTableReference endString;
                endString.entryReference = "End";
                endString.tableReference = "ConstantStrings";
                TextContinueDialogue.text = endString.GetLocalisedString();
            }
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

    public void SkipDialogue()
    {
        _QueueOfCharacters.Clear();
        _QueueOfStringArrays.Clear();
        _QueueOfStrings.Clear();

        EndDialogue();
    }

    private void EndDialogue()
    {
        playerInput.enabled = true;
        playerInput.TogglePlayerInteraction(true);
        // Re-enable the collider of the GO we're speaking to when ending the dialogue
        if (currentCollider) currentCollider.enabled = true;
        DialoguePanel.SetActive(false);
        GameEvents.current.ContinueAttacking();
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

    private void AddSentencesToQueue(LocalisationTableReference[] characterDialogueQueue)
    {
        foreach (LocalisationTableReference item in characterDialogueQueue)
        {
            // Dominique, Replace any instances of {playerName} with the player's name
            string sentence = item.GetLocalisedString();
            if (sentence.Contains("{playerName}"))
            {
                sentence = sentence.Replace("{playerName}", playerStats.playerName);
            }

            _QueueOfStrings.Enqueue(sentence);
        }
    }
    /*_________________________________________________________________________*/
    #endregion
}