using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject DialoguePanel;
    public Dialogue Dialogue;

    public void TriggerDialogue()
    {
        DialoguePanel.SetActive(true);
        FindObjectOfType<DialogueManager>().StartDialogue(Dialogue);
        gameObject.SetActive(false);
    }
}