using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
   // [SerializeField] private Text LoadingPercentage;
    [SerializeField] private Image LoadingProgressBar;
    [SerializeField] private float _timeLoadingScene = 0.1f;

    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false;

    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        instance.componentAnimator.SetTrigger("sceneOpening");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

        // ����� ����� �� ������ ������������� ���� ������ �������� closing:
        instance.loadingSceneOperation.allowSceneActivation = false;

        instance.LoadingProgressBar.fillAmount = 0;
    }

    private void Start()
    {
        instance = this;

        componentAnimator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
        {
            componentAnimator.SetTrigger("sceneClosing");
            instance.LoadingProgressBar.fillAmount = 1;

            // ����� ���� ��������� ������� ����� ������� SceneManager.LoadScene, �� ����������� �������� opening:
            shouldPlayOpeningAnimation = false;
        }
    }

    private void Update()
    {
        if (loadingSceneOperation != null)
        {
            //LoadingPercentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";

            // ������ ��������� ��������:
            //LoadingProgressBar.fillAmount = loadingSceneOperation.progress; 

            // ��������� �������� � ������� ���������, ����� ��������� �������:
            LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount, loadingSceneOperation.progress,
                Time.deltaTime * _timeLoadingScene);
            if(LoadingProgressBar.fillAmount > 0.8)
            {
                OnAnimationOver();
            }
        }
    }

    public void OnAnimationOver()
    {
        // ����� ��� �������� �����, ���� �� �������������, ����������� �������� opening:
        shouldPlayOpeningAnimation = true;

        loadingSceneOperation.allowSceneActivation = true;
    }
}
