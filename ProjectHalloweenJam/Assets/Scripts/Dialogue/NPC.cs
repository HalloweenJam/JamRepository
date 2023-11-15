using UnityEngine;
using System;

public class NPC : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    public void DialogueTrigger() => DialogueManager.Instance.StartDialogue(dialogue);
}