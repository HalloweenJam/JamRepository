using Core.Interfaces;
using Items;
using Player;
using UnityEngine;

namespace Gameplay.Interactions
{
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class ItemHolder : MonoBehaviour, IPickUp
    {
        [SerializeField] private Collectable _collectable;
        
        [SerializeField, HideInInspector] private CircleCollider2D _collider;
        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;

        public void SetItem(Collectable item)
        {
            _spriteRenderer.sprite = item.Sprite;
            _collectable = item;
        }

        public void PickUp(PlayerStats playerStats)
        {
            _collectable.Apply(playerStats);
            Destroy(gameObject);
        }
        
        private void Start()
        {
            if (_collectable != null)
                SetItem(_collectable);
        }

        private void OnValidate()
        {
            _collider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sortingOrder = 2;

            _collider.isTrigger = true;
        }
    }
}