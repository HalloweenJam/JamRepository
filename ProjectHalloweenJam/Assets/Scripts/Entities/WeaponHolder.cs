using Managers;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(SpriteRenderer), typeof(DissolveEffect))]
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private float _offset;

        [SerializeField] private Sprite _sprite;

        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;
        [SerializeField, HideInInspector] private DissolveEffect _dissolveEffect;
        
        private float _rotation;
        private bool _isEnabled = true; 
        
        private Camera _camera;

        public void Enable(bool enable)
        {
            if (!enable)
            {
                _dissolveEffect.Dissolve(this);
            }
            else
            {
                _dissolveEffect.Appearance(this);
            }
        }

        public void SetWeaponSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        private void Start()
        {
            InputReaderManager.Instance.OnInputReaderActiveStateChanged += (state) => _isEnabled = state;
            InputReaderManager.Instance.OnInstanceDestroyed += OnDisable;
            _camera = Camera.main;
        }
        
        private void OnDisable()
        {
            if (InputReaderManager.Instance == null)
                return;
            
            InputReaderManager.Instance.OnInputReaderActiveStateChanged -= (state) => _isEnabled = state;
            InputReaderManager.Instance.OnInstanceDestroyed -= OnDisable;
        }
        
        public void Flip(bool isFacingRight)
        {
            var localScale = transform.localScale;

            var value = isFacingRight ? -1f : 1f;

            localScale.x = value;
            localScale.y = value;

            transform.localScale = localScale;
        }

        private void FixedUpdate()
        {
            if (!_isEnabled) 
                return;
            
            var direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            _rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, _rotation + _offset);
        }
        
        private void OnValidate()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _dissolveEffect = GetComponent<DissolveEffect>();
            _spriteRenderer.sprite = _sprite;
        }
    }
}