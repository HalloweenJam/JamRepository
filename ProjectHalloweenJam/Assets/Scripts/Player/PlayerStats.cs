using System;
using Entities;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStats : EntityStats
    {
        [Header("Damage Taking")] 
        [SerializeField] private float _invisibilityLength = 1.5f;
        [SerializeField] private float _dashInvisibilityLength = .7f;
        
        [SerializeField, HideInInspector] private PlayerController _playerController;

        public static Action OnPlayerKilled;
        public Action<float> OnPlayerTakeDamage;
        
        private float _invisibilityCounter = 1;
        
        private bool _canIgnoreDamage => _invisibilityCounter > 0;

        private void Start()
        {
            _playerController.OnPlayerDashing += _ => _invisibilityCounter = _dashInvisibilityLength; 
        }

        public override bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
        {
            if (_canIgnoreDamage && !ignoreInvisibility)
                return false;
            
            if (instantKill)
            {
                Kill();
                return true;
            }
            
            CurrentHealth -= damage;
            _invisibilityCounter = _invisibilityLength;
            
            OnEntityTakeDamage?.Invoke();
            OnPlayerTakeDamage?.Invoke((float) CurrentHealth / MaxHealth);

            if (CurrentHealth <= 0)
                Kill();

            return true;
        }
        
        private void OnValidate()
        {
            _playerController = GetComponent<PlayerController>();
            CurrentHealth = MaxHealth;
        }

        protected override void Kill()
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