using DG.Tweening;
using Player;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Core;

public class Castscene : MonoBehaviour
{
    [Header("Animation")]
    [Range(0, 5f)][SerializeField] private float _animationTime;
    [SerializeField] private string _idle;
    [SerializeField] private string _run;
    private Animator _playerAnimator;

    [Header("Interface")]
    [SerializeField] private List<Image> _interfaceObjects;
    [SerializeField] private List<TextMeshProUGUI> _texts;
    [SerializeField] private Image _dialogueDisplay;
    [SerializeField] private RawImage _minimapRenderer;

    [SerializeField] private EnemyMovement _bossMovement;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _destnationTransform;
    [SerializeField] private NPC _bossDialogue;

    private void Awake()
    {
        DialogueManager.Instance.OnEndDialogue += EndCastscene;
        _playerAnimator = _playerController.GetComponent<Animator>();
    }

    public void StartCastscene() => StartCoroutine(CastsceneCoroutine());

    private IEnumerator CastsceneCoroutine()
    {
        ShowInteface(false);
        _playerController.enabled = false;
        _playerController.transform.DOMove(_destnationTransform.position, _animationTime);
        _playerAnimator.Play(_run);
        yield return new WaitForSeconds(_animationTime);
        _playerAnimator.Play(_idle);
        ShowDialogueDisplay(true);
        _bossDialogue.DialogueTrigger();
    }
    private void EndCastscene()
    {
        ShowDialogueDisplay(false);
        ShowInteface(true);
        _playerController.enabled = true;
        _bossMovement.enabled = true;
    }

    private void ShowInteface(bool active)
    {
        foreach (var image in _interfaceObjects)
        {
            _minimapRenderer.enabled = active;
            image.enabled = active;
        }
        foreach (var text in _texts)
            text.enabled = active;
    }

    private void ShowDialogueDisplay(bool active)
    {
        if (active)
        {
            float fadeValue = active ? 1 : 0f;
            float fadeTime = 1.5f;
            _dialogueDisplay.DOFade(fadeValue, fadeTime);
        }
        else
            _dialogueDisplay.Deactivate();
    }
}