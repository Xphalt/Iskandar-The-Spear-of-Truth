using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager DialogueManager_;
    private NewConversation Conversation;
    public List<NewConversation> ListOfConversations = new List<NewConversation>();
    private Queue<NewConversation> QueueOfNewConversations = new Queue<NewConversation>();
    private Collider col;

    private void Start()
    {
        foreach (NewConversation item in ListOfConversations)
        {
            QueueOfNewConversations.Enqueue(item);
        }


        col = GetComponent<Collider>();
        gameObject.SetActive(true);
        Conversation = QueueOfNewConversations.Dequeue();
    }

    public void TriggerDialogue()
    {
        col.enabled = false;
        DialogueManager_.DialoguePanel.SetActive(true);
        DialogueManager_.StartDialogue(col, Conversation);
    }

    //Call this function to move to the next line of dialogue
    public void NextConversation()
    {
        Conversation = QueueOfNewConversations.Dequeue();

        //If the conversation is the last in the queue, disable the dialogue trigger
        if (QueueOfNewConversations.Count == 0)
        {
            col.GetComponent<DialogueTrigger>().enabled = false;
        }
    }
}