using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingControl;
    public static GameMenu Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        

    }

    private void Start()
    {
        PauseManager.Instance.OffGamePause();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void ResumeGame()
    {
        PauseManager.Instance.OffGamePause();
    }

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
        SceneTransition.SwitchToScene(name);
    }
}
