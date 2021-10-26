using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEMPDialogueManager : MonoBehaviour
{
    public List<NewDialogue> ListOfNewConversations = new List<NewDialogue>();
    private Queue<NewDialogue.NextExchange> QueueOfCharacterExchanges = new Queue<NewDialogue.NextExchange>();
    private Queue<string> QueueOfCharacterConversations = new Queue<string>();

    public GameObject DialoguePanel;
    public Text TextNPCName;
    public Text TextDialogueBox;
    public Text TextContinueDialogue;

    public void StartDialogue(NewDialogue newDialogue)
    {
        QueueOfCharacterExchanges.Clear();
        QueueOfCharacterConversations.Clear();

        foreach (NewDialogue.NextExchange queuedCharacterExchanges in newDialogue.ListOfCharacterExchanges)
        {
            TextNPCName.text = queuedCharacterExchanges.NPCName;
            QueueOfCharacterExchanges.Enqueue(queuedCharacterExchanges);

            foreach (string queuedCharacterConversations in queuedCharacterExchanges.NumOfSentences)
            {
                QueueOfCharacterConversations.Enqueue(queuedCharacterConversations);
            }
        }

        DisplayNextExchange(newDialogue);
    }

    public void DisplayNextExchange(NewDialogue newDialogue)
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
    }

    private void EndDialogue()
    {
        DialoguePanel.SetActive(false);
    }
}