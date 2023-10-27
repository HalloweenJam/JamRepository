using System;
using Core.Interfaces;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStats : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private int _currentHealth = 5;

        [Header("Damage Taking")] 
        [SerializeField] private float _invisibilityLength = 1.5f;
        
        [SerializeField, HideInInspector] private PlayerController _playerController;

        public static Action OnPlayerKilled;
        
        private float _invisibilityCounter = 1;
        
        private bool _canIgnoreDamage => _playerController.IsDashing || _invisibilityCounter > 0;
        
        public bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
        {
            if (_canIgnoreDamage && !ignoreInvisibility)
                return false;

            if (instantKill)
            {
                Kill();
                return true;
            }

            _currentHealth -= damage;
            _invisibilityCounter = _invisibilityLength;

            if (_currentHealth <= 0)
                Kill();

            return true;
        }
        
        private void OnValidate()
        {
            _playerController = GetComponent<PlayerController>();
            _currentHealth = _maxHealth;
        }

        private void Kill()
        {
            OnPlayerKilled?.Invoke();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_invisibilityCounter < 0)
                return;

            _invisibilityCounter -= Time.deltaTime;
        }
    }
}