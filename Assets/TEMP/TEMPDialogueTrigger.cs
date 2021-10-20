using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPDialogueTrigger : MonoBehaviour
{
    public TEMPDialogueManager DialogueManager;
    public NewDialogue Dialogue;

    public void OnClick()
    {
        gameObject.SetActive(false);
        DialogueManager.gameObject.SetActive(true);
        DialogueManager.StartDialogue(Dialogue);
    }
}