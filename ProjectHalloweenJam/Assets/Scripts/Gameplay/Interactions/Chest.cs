using System;
using Core.Interfaces;
using Player;
using UnityEngine;

namespace Gameplay.Interactions
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(BoxCollider2D))]
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField, Range(1, 5)] private int _rarity = 1;
        
        [SerializeField, HideInInspector] private BoxCollider2D _collider;

        private bool _enabled;
        
        public static Action<int> OnChestOpened;
        
        private void OnValidate()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = true;
        }

        public bool Interact(Inventory inventory)
        {
            if (_enabled)
                return false;
            
            if (inventory.Keys[_rarity] <= 0)
                return false;
            
            _enabled = true;
            inventory.Keys[_rarity]--;
            OnChestOpened?.Invoke(_rarity);
            
            return true;
        }
    }
}