using System.Collections;
using UnityEngine;

namespace Visuals
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AfterImageController : MonoBehaviour
    {
        [SerializeField] private float _spawnDelay = .1f;
        [SerializeField] private float _startOpacity = .7f;
        [SerializeField] private float _length = 1f;
        [Space(12)]
        [SerializeField] private SpriteRenderer _playerRenderer;
        [SerializeField] private SpriteRenderer _childRenderer;
        
        private Coroutine _spawnCoroutine;

        public void Play(bool autoDisable = true)
        {
            Stop();
            _spawnCoroutine = StartCoroutine(Spawn());
            
            if (autoDisable)
                StartCoroutine(StopStopCoroutine());
        }

        private IEnumerator StopStopCoroutine()
        {
            yield return new WaitForSeconds(_length);
            Stop();
        }

        public void Stop()
        {
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                VisualsManager.Instance.CreateAfterImage(_playerRenderer, _childRenderer, _startOpacity);
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}