using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager DialogueManager_;
    public NewConversation Conversation;

    public void OnClick()
    {
        gameObject.SetActive(false);
        DialogueManager_.gameObject.SetActive(true);
        DialogueManager_.StartDialogue(Conversation);
    }
}