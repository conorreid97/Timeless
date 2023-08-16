using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Queue<string> sentences;

    public Text dialogueText;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}
	
	public void StartDialogue (Dialogue dialogue)
    {
        sentences.Clear();

        //loops through the sentences in the dialogue and puts them in a queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        Debug.Log("End Conversation");
    }
}
