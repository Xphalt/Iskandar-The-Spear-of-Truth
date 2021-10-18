using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPDialogueManager : MonoBehaviour
{
    public List<NewDialogue> ListOfNewConversations = new List<NewDialogue>();
    private Queue<NewDialogue.NextExchange> QueueOfCharacterExchanges = new Queue<NewDialogue.NextExchange>();

    public void StartDialogue(NewDialogue newDialogue)
    {
        QueueOfCharacterExchanges.Clear();

        foreach (NewDialogue.NextExchange queuedCharacterExchanges in QueueOfCharacterExchanges)
        {
            QueueOfCharacterExchanges.Enqueue(queuedCharacterExchanges);
        }
    }

    public void DisplayNextExchange()
    {
        if (QueueOfCharacterExchanges.Count == 0)
        {
            EndDialogue();

            return;
        }

        NewDialogue.NextExchange queuedCharacterExchanges = QueueOfCharacterExchanges.Dequeue();
    }

    private void EndDialogue()
    {

    }
}