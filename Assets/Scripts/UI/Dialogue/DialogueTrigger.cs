using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager DialogueManager_;
    public NewConversation Conversation;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    public void TriggerDialogue()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        DialogueManager_.DialoguePanel.SetActive(true);
        DialogueManager_.StartDialogue(collider, Conversation);
    }
}