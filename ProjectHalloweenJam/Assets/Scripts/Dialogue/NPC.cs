using Core;
using Core.Interfaces;
using Managers;
using Player;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCLocalizated _npcData;
    [Space]
    [SerializeField] private DialogueDisplay _dialogueDisplay;
    [SerializeField] private RectTransform _hidePanel;
    [SerializeField] private RectTransform _dialoguePanel;  

    private bool _isStartedDialogue = false;

    private void Start()
    {
        InputReaderManager.Instance.GetInputReader().DialogueEvent += OnDialogue;
        DialogueManager.Instance.OnEndDialogue += HidePanels;
    }

    public void DialogueTrigger() => DialogueManager.Instance.StartDialogue(_npcData.Name, _npcData.GetDialogueSentences());

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7)
            _hidePanel.Activate();      
    }

    private void OnDialogue()
    {
        if (_hidePanel.gameObject.activeSelf || !_isStartedDialogue)
        {
            _isStartedDialogue = true;
            _hidePanel.Deactivate();
            _dialoguePanel.Activate();
            DialogueTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isStartedDialogue)      
            DialogueManager.Instance.StopDialogue();      
        HidePanels();
    }

    private void HidePanels()  
    {
        _dialoguePanel.Deactivate();
        _hidePanel.Deactivate();
    }

    private void OnDisable()
    {
        DialogueManager.Instance.OnEndDialogue -= HidePanels;

        if (InputReaderManager.Instance == null) 
            return;

        InputReaderManager.Instance.GetInputReader().DialogueEvent -= OnDialogue;
        InputReaderManager.Instance.OnInstanceDestroyed -= OnDisable;
    }
}