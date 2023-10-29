using System.Collections;
using Entities;
using UnityEngine;

namespace Visuals
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField, ColorUsage(false, true)] private Color _flashColor = Color.white;
        [SerializeField] private float _flashTime = .25f;
        
        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;
        [SerializeField, HideInInspector] private EntityStats _entityStats;
        
        private Material _material;
        private Coroutine _coroutine;
        
        private static readonly int MaterialColor = Shader.PropertyToID("_Color");
        private static readonly int FlashOpacity = Shader.PropertyToID("_FlashOpacity");

        private void Play()
        {
            _coroutine = StartCoroutine(Flash());
        }

        private void OnValidate()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _entityStats = GetComponent<EntityStats>();
        }

        private void Awake()
        {
            _entityStats.OnEntityTakeDamage += Play;
            Init();
        }

        private void OnDisable()
        {
            _entityStats.OnEntityTakeDamage -= Play;
        }

        private void Init()
        {
            _material = _spriteRenderer.material;
        }
        
        private IEnumerator Flash()
        {
            SetFlashColor();

            var elapsedTime = 0f;

            while (elapsedTime < _flashTime)
            {
                elapsedTime += Time.deltaTime;

                var currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
                SetFlashAmount(currentFlashAmount);
                
                yield return null;
            }
        }

        private void SetFlashColor()
        {
            _material.SetColor(MaterialColor, _flashColor);
        }

        private void SetFlashAmount(float amount)
        {
            _material.SetFloat(FlashOpacity, amount);
        }
    }
}