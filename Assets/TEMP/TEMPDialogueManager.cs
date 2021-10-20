using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEMPDialogueManager : MonoBehaviour
{
    //public List<NewDialogue> ListOfNewConversations = new List<NewDialogue>();
    private Queue<NewDialogue.NextExchange> QueueOfCharacterExchanges = new Queue<NewDialogue.NextExchange>();
    private Queue<string> QueueOfCharacterConversations = new Queue<string>();

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

    public void StartDialogue(NewDialogue newDialogue)
    {
        QueueOfCharacterExchanges.Clear();
        QueueOfCharacterConversations.Clear();
        TextContinueDialogue.text = "Next";

        foreach (NewDialogue.NextExchange queuedCharacterExchanges in newDialogue.ListOfCharacterExchanges)
        {
            TextNPCName.text = queuedCharacterExchanges.NPCName;
            QueueOfCharacterExchanges.Enqueue(queuedCharacterExchanges);

            foreach (string queuedCharacterConversations in queuedCharacterExchanges.NumOfSentences)
            {
                QueueOfCharacterConversations.Enqueue(queuedCharacterConversations);
            }
        }

        DisplayNextExchange();
    }

    public void DisplayNextExchange()
    {
        if (QueueOfCharacterConversations.Count == 0)
        {
            TextContinueDialogue.text = "End";
            EndDialogue();

            return;
        }

        NewDialogue.NextExchange _queuedCharacterExchanges = QueueOfCharacterExchanges.Dequeue();
        string _queueOfCharacterConversations = QueueOfCharacterConversations.Dequeue();

        TextDialogueBox.text = _queueOfCharacterConversations;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(_queueOfCharacterConversations));
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