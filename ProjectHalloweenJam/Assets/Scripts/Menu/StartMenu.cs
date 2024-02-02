using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private static readonly string _startSceneName = "Limb";
    private void Start() => Cursor.visible = true; 
 
    public void StartGame() => SceneTransition.SwitchToScene(_startSceneName, false);
   
    public void ExitGame() => Application.Quit();
     
}
