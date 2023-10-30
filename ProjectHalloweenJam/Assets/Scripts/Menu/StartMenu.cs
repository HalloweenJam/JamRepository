using Managers;
using Player.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    
    public void StartGame(string name)
    {
        SceneTransition.SwitchToScene(name);
    }
    

    public void SettingGame()
    {
     
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    
}
