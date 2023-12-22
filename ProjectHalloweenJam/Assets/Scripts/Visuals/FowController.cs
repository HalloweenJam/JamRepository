using System.Collections;
using Player;
using SimpleFOW;
using UnityEngine;

namespace Visuals
{
    public class FowController : MonoBehaviour
    {
        [SerializeField] private FogOfWarShaderControl _control;
        public PlayerController _playerController;
        
        void Start()
        {
            _control.Init();
            /*_control.AddPoint(new Vector2(8,5));
            _control.AddPoint(new Vector2(30,8));
            _control.AddPoint(new Vector2(10,5));
            _control.AddPoint(new Vector2(40,5));
            _control.AddPoint(new Vector2(50,5));
            _control.AddPoint(new Vector2(44,5));
            _control.SendPoints();*/

            StartCoroutine(Boobs());
        }

        private IEnumerator Boobs()
        {
            yield return new WaitForFixedUpdate();
            _control.AddPoint(_playerController.transform.position);
            _control.SendPoints();
            
            StartCoroutine(Boobs());
        }
    }
}
