using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameNPC;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private void Start()
    {
        DialogueManager.Instance.OnStartDialogue += (string name) => _nameNPC.text = name;
        DialogueManager.Instance.OnNextSentence += (string sentence) => StartCoroutine(TypeSentence(sentence));
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

    private void OnDisable()
    {
        DialogueManager.Instance.OnStartDialogue -= (string name) => { };
        DialogueManager.Instance.OnNextSentence -= (string sentence) => { };
    }
}