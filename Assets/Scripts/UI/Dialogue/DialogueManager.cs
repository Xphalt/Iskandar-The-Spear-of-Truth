using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public List<Dialogue> ListOfNewConversations = new List<Dialogue>();
    private Queue<Dialogue.NextExchange> QueueOfCharacterExchanges = new Queue<Dialogue.NextExchange>();
    private Queue<string> QueueOfCharacterConversations = new Queue<string>();

    public GameObject DialoguePanel;
    public Text TextNPCName;
    public Text TextDialogueBox;
    public Text TextContinueDialogue;

    public void StartDialogue(Dialogue dialogue)
    {
        QueueOfCharacterExchanges.Clear();
        QueueOfCharacterConversations.Clear();

        foreach (Dialogue.NextExchange queuedCharacterExchanges in dialogue.ListOfCharacterExchanges)
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
        if (QueueOfCharacterConversations.Count == 1)
        {
            TextContinueDialogue.text = "End";
        }
        else if (QueueOfCharacterConversations.Count == 0)
        {
            EndDialogue();

            return;
        }

        Dialogue.NextExchange _queuedCharacterExchanges = QueueOfCharacterExchanges.Dequeue();
        string _queueOfCharacterConversations = QueueOfCharacterConversations.Dequeue();

        TextDialogueBox.text = _queueOfCharacterConversations;
    }

    private void EndDialogue()
    {
        DialoguePanel.SetActive(false);
    }
}