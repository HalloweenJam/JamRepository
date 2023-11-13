using Core.Interfaces;
using Entities;
using Managers;
using Player.Controls;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private float _range = 1.5f;
        [SerializeField] private LayerMask _interactionsLayerMask;

        [SerializeField, HideInInspector] private Inventory _inventory;
        [SerializeField, HideInInspector] private WeaponSelector _weaponSelector;
        
        private InputReader _inputReader;

        private void OnValidate()
        {
            _inventory = GetComponent<Inventory>();
            _weaponSelector ??= GetComponent<WeaponSelector>();
        }

        private void Start()
        {
            _inputReader = InputReaderManager.Instance.GetInputReader();

            _inputReader.InteractEvent += Interact;
        }

        private void Interact()
        {
            var overlap = Physics2D.OverlapCircle(transform.position, _range, _interactionsLayerMask);

            if (!overlap)
                return;
            
            if (overlap.TryGetComponent<IInteractable>(out var interactable))
                interactable.Interact(_inventory);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IPickUp>(out var item))
            {
                item.PickUp(_weaponSelector);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}