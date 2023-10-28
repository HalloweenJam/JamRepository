using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;

        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;

        public void SetWeaponSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        private void OnValidate()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _sprite;
        }
    }
}