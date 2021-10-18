using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool isMum = false;
    public bool isMilk = false;

    public GameObject DialoguePanel;
    public Dialogue Dialogue;

    public void TriggerDialogue()
    {
        DialoguePanel.SetActive(true);
        FindObjectOfType<DialogueManager>().StartDialogue(Dialogue);
        gameObject.SetActive(false);

        // THIS IS TEMPORARY SO WE CAN SHOW DESIGNERS STUFF
        if (isMum)
        {
            HACKY_SCRIPT_VERTICAL_SLICE.instance.hasSpokenToMum = true;
        }
        else if (isMilk)
        {
            HACKY_SCRIPT_VERTICAL_SLICE.instance.hasSpokenToMilkperson = true;
        }
    }
}