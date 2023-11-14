using Core.Interfaces;
using Items;
using Managers;
using Player;
using UnityEngine;

namespace Gameplay.Interactions
{
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class ShopCell : MonoBehaviour, IInteractable
    {
        [SerializeField] private Collectable _collectable;

        private void Start()
        {
            _collectable = ShopManager.Instance.GetCollectable();
        }

        public bool Interact(Inventory inventory)
        {
            var playerStats = inventory.PlayerStats;

            if (!playerStats.Wallet.TryToSpendPoints(_collectable.Cost)) 
                return false;
            
            _collectable.Apply(playerStats);
            return true;
        }
    }
}