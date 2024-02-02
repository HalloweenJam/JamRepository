using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameNPC;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private Coroutine _typeSentenceCoroutine;

    private void Start()
    {
        DialogueManager.Instance.OnStartDialogue += (string name) => _nameNPC.text = name;
        DialogueManager.Instance.OnNextSentence += (string sentence)
            => _typeSentenceCoroutine = StartCoroutine(TypeSentence(sentence));
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _dialogueText.text = "";
        foreach (char letter in sentence)
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        DialogueManager.Instance.NextSentence();
    }

    private void OnEndDialogue()
    {
        StopCoroutine(_typeSentenceCoroutine);
        _dialogueText.text = "";
    }

    private void OnDisable()
    {
        DialogueManager.Instance.OnStartDialogue -= (string name) => _nameNPC.text = name;
        DialogueManager.Instance.OnNextSentence -= (string sentence)
            => _typeSentenceCoroutine = StartCoroutine(TypeSentence(sentence));
        DialogueManager.Instance.OnEndDialogue -= OnEndDialogue;
    }
}