using Core.Interfaces;
using Entities;
using Items;
using UnityEngine;

namespace Gameplay.Interactions
{
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class ItemHolder : MonoBehaviour, IPickUp
    {
        [SerializeField] private Item _item;
        
        [SerializeField, HideInInspector] private CircleCollider2D _collider;
        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;

        public void SetItem(Item item)
        {
            _spriteRenderer.sprite = item.Sprite;
            _item = item;
        }

        public void PickUp(WeaponSelector weaponSelector)
        {
            weaponSelector.AddItem(_item);
            Destroy(gameObject);
        }
        
        private void Start()
        {
            if (_item != null)
                SetItem(_item);
        }

        private void OnValidate()
        {
            _collider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _collider.isTrigger = true;
        }
    }
}