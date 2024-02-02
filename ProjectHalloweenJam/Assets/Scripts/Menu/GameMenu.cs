using Managers;
using UnityEngine;

public class GameMenu : Singleton<GameMenu>
{
    [SerializeField] private GameObject _settingControl;

    private void Start() => PauseManager.Instance.OffGamePause();

    public void SetActive(bool active) => gameObject.SetActive(active);

    public void ResumeGame() => PauseManager.Instance.OffGamePause();
 
    public void SettingGame()
    {
        if (_settingControl != null)
        {
            if (_settingControl.activeSelf)
            {
                _settingControl.SetActive(false);
            }
            else
            {
                _settingControl.SetActive(true);
            }
        }
    }

    public void ExitMenuGame(string name)
    {
        PauseManager.Instance.ExitGameMenu();
        SceneTransition.SwitchToScene(name, false);
    }
}
