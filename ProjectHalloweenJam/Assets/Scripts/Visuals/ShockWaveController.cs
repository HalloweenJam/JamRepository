using System.Collections;
using UnityEngine;

namespace Visuals
{
    public class ShockWaveController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _shockWaveRenderer;
        [SerializeField] private float _shockWaveTime = .5f;

        private Coroutine _coroutine;
        private Material _material;
        
        private static readonly int _distanceFromCenter = Shader.PropertyToID("_DistanceFromCenter");

        public void Play()
        {
            _shockWaveRenderer.enabled = true;
            _coroutine = StartCoroutine(ShockWaveCoroutine(-.1f, 1f));
        }
        
        private void Awake()
        {
            _shockWaveRenderer.enabled = false;
            _material = _shockWaveRenderer.material;
        }

        private IEnumerator ShockWaveCoroutine(float start, float end)
        {
            _material.SetFloat(_distanceFromCenter, start);
            var elapsedTime = 0f;

            while (elapsedTime < _shockWaveTime)
            {
                elapsedTime += Time.deltaTime;
                var lerpedAmount = Mathf.Lerp(start, end, (elapsedTime / _shockWaveTime));
                _material.SetFloat(_distanceFromCenter, lerpedAmount);
                yield return null;
            }
            
            _shockWaveRenderer.enabled = false;
        }
    }
}