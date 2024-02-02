using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    private static readonly string _firstScene = "SecondCircle";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
            SceneTransition.SwitchToScene(_firstScene, false);
    }
}
