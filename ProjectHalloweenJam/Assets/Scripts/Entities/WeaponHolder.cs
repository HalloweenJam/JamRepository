using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private float _offset;

        [SerializeField] private Sprite _sprite;

        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;
        
        private float _rotation;
        
        private Camera _camera;

        public void SetWeaponSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public void Flip(bool isFacingRight)
        {
            var s_localScale = transform.localScale;

            var value = isFacingRight ? -1f : 1f;

            s_localScale.x = value;
            s_localScale.y = value;

            transform.localScale = s_localScale;
        }

        private void FixedUpdate()
        {
            var direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            _rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, _rotation + _offset);
        }

        private void Start()
        {
            _camera = Camera.main;
            print(_camera.name);
        }
        
        private void OnValidate()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _sprite;
        }
    }
}