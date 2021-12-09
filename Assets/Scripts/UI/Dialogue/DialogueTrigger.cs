using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager DialogueManager_;
    private NewConversation Conversation;
    public List<NewConversation> ListOfConversations = new List<NewConversation>();
    private Queue<NewConversation> QueueOfNewConversations = new Queue<NewConversation>();
    private Collider col;

    public bool replayable = false, hasPlayed = false;

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
        // Dominique, Dialogues don't need to be repeated and this way we can stop them from triggering during combat
        if (!replayable)
        {
            GetComponent<Interactable_Object_Jack>().enabled = false;
            gameObject.layer = 0;
            hasPlayed = true;
        }

        GameEvents.current.StopAttacking();
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