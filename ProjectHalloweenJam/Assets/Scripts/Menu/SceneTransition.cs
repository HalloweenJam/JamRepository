using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Image _loadingProgressBar;
    [SerializeField] private float _timeLoadingScene = 0.1f;

    public static SceneTransition Instance { get; private set; }
    private static bool _shouldPlayOpeningAnimation = false;

    private Animator _componentAnimator;
    private AsyncOperation _loadingSceneOperation;

    private bool _instantlyOpenScene = false;

    public static void SwitchToScene(string sceneName, bool instantlyOpenScene)
    {
        Instance._componentAnimator.SetTrigger("sceneOpening");
        Instance._loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        Instance._loadingSceneOperation.allowSceneActivation = false;
        Instance._loadingProgressBar.fillAmount = 0;
        Instance._instantlyOpenScene = instantlyOpenScene;
        Cursor.visible = false;
    }

    private void Start()  
    {
        Instance = this;
        _componentAnimator = GetComponent<Animator>(); 
        if(_instantlyOpenScene)
            OpenScene();
    }

    public void OpenScene()
    {
        if (_shouldPlayOpeningAnimation)
        {
            _componentAnimator.SetTrigger("sceneClosing");
            Instance._loadingProgressBar.fillAmount = 1;
            _shouldPlayOpeningAnimation = false;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        if (_loadingSceneOperation != null)
        {
            _loadingProgressBar.fillAmount = Mathf.Lerp(_loadingProgressBar.fillAmount, _loadingSceneOperation.progress,
                Time.deltaTime * _timeLoadingScene);
            if(_loadingProgressBar.fillAmount > 0.8)           
                OnAnimationOver();           
        }
    }

    public void OnAnimationOver()
    {
        _shouldPlayOpeningAnimation = true;
        _loadingSceneOperation.allowSceneActivation = true;
    }
}
