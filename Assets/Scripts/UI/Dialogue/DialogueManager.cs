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

    public LocalisationTableReference nextString;
    public LocalisationTableReference endString;

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
        // Dominique 08-10-2021, Use localised sentences
        ButtonText.text = nextString.GetLocalisedString();

        SentenceQueue.Clear();

        // Dominique 08-10-2021, Use localised sentences
        foreach (LocalisationTableReference sentence in dialogue.Sentences)
        {
            SentenceQueue.Enqueue(sentence.GetLocalisedString());
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (SentenceQueue.Count <= 1)
        {
            // Dominique 08-10-2021, Use localised sentences
            ButtonText.text = endString.GetLocalisedString();
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
        //DialogueTrigger.SetActive(true);
    }
}