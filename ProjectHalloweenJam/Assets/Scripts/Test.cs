using System.Collections;
using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(Tp(new Vector2(-30, -10)));
        }

        private IEnumerator Tp(Vector2 vector2)
        {
            yield return new WaitForSeconds(5);
            PlayerController.TeleportPlayer.Invoke(vector2);
        }
    }
}