using DG.Tweening;
using Player;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Core;
using Managers;
using Cinemachine;

public class Castscene : MonoBehaviour
{
    [Header("Animation")]
    [Range(0, 5f)][SerializeField] private float _animationTime;
    [SerializeField] private string _idle;
    [SerializeField] private string _run;
    private bool _isShowed = false;
    private Animator _playerAnimator;

    [Header("Interface")]
    [SerializeField] private List<Image> _interfaceObjects;
    [SerializeField] private List<TextMeshProUGUI> _texts;
    [SerializeField] private Image _dialogueDisplay;
    [SerializeField] private RawImage _minimapRenderer;
    [SerializeField] private BossHP _bossBar;

    [Header("Components")]
    [SerializeField] private EnemyBoss _enemyBoss;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _destinationTransform;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private Coroutine _coroutine;
    private NPC _bossDialogue;
    private EnemyMovement _bossMovement;
    private Camera _camera;

    public bool IsShowed => _isShowed;

    private void Awake()
    {
        DialogueManager.Instance.OnEndDialogue += EndCastscene;   
        _enemyBoss.OnTakeDamage += EndCastsceneNow;

        _playerAnimator = _playerController.GetComponent<Animator>();
        _bossMovement = _enemyBoss.gameObject.GetComponent<EnemyMovement>();
        _bossDialogue = _enemyBoss.gameObject.GetComponent<NPC>();
        _camera = Camera.main;
    }

    public void StartCastscene() => _coroutine = StartCoroutine(CastsceneCoroutine());

    private IEnumerator CastsceneCoroutine()
    {
        ShowInteface(false);
        InputReaderManager.Instance.SetActiveControls(false);
        _playerController.transform.DOMove(_destinationTransform.position, _animationTime);

        Vector3 destinationCameraPosition = _destinationTransform.position;
        destinationCameraPosition.z = -10f;
        _cinemachineVirtualCamera.enabled = false;
        _camera.transform.DOMove(destinationCameraPosition, _animationTime);
        _playerAnimator.Play(_run);

        yield return new WaitForSeconds(_animationTime);

        _playerAnimator.Play(_idle);
        ShowDialogueDisplay(true);
        _bossDialogue.DialogueTrigger();
    }
    private void EndCastscene()
    {
        _cinemachineVirtualCamera.enabled = true;
        InputReaderManager.Instance.SetActiveControls(true);
        _bossMovement.enabled = true;

        _isShowed = false;
        ShowDialogueDisplay(false);
        ShowInteface(true);
        _bossBar.ShowBossBaR();
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

    private void EndCastsceneNow(float _)
    {
        if (!_isShowed && _coroutine != null)
        {
            StopCoroutine(_coroutine);
            _playerController.transform.DOPause();
            EndCastscene();
            _enemyBoss.OnTakeDamage -= EndCastsceneNow;
        }
    }
}