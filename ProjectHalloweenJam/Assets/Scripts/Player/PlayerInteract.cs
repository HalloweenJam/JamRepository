using System;
using Core.Interfaces;
using Managers;
using Player.Controls;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private float _range = 1.5f;
        [SerializeField] private LayerMask _interactionsLayerMask;

        [SerializeField, HideInInspector] private Inventory _inventory;
        
        private InputReader _inputReader;

        private void OnValidate()
        {
            _inventory = GetComponent<Inventory>();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}