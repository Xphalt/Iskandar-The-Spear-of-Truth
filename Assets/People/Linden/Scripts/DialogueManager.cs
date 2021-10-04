using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialoguePanel;
    public GameObject DialogueTrigger;
    public Text NameText;
    public Text DialogueText;
    public Button ContinueText;
    public Text ButtonText;

    public Queue<string> SentenceQueue;

    private void Start()
    {
        DialoguePanel.SetActive(false);
        NameText.text = "";
        DialogueText.text = "";
        ButtonText.text = "";
        SentenceQueue = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        NameText.text = dialogue.NPC_Name;
        ButtonText.text = "Next  >>";

        SentenceQueue.Clear();

        foreach(string sentence in dialogue.Sentences)
        {
            SentenceQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (SentenceQueue.Count <= 1)
        {
            ButtonText.text = "End";
        }

        if (SentenceQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = SentenceQueue.Dequeue();
        DialogueText.text = sentence;
    }

    private void EndDialogue()
    {
        DialoguePanel.SetActive(false);
        DialogueTrigger.SetActive(true);
    }
}