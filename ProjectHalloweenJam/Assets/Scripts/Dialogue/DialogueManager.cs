using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private Queue<string> sentences;

    public Action<string> OnStartDialogue;
    public Action<string> OnNextSentence;
    public Action OnEndDialogue;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        OnStartDialogue?.Invoke(dialogue.Name);
        sentences.Clear();

        foreach (string sentence in dialogue.Sentences)
            sentences.Enqueue(sentence);
        NextSentence();
    }

    public void NextSentence()
    {
        if (sentences.Count == 0)
        {
            OnEndDialogue?.Invoke();
            return;
        }
        string sentence = sentences.Dequeue();
        OnNextSentence?.Invoke(sentence);
    }
}