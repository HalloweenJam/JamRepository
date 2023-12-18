using System;
using Core.Classes;
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
        [SerializeField, HideInInspector] private WeaponSelector _weaponSelector;
        
        public static Action OnPlayerKilled;
        public Action<float> OnPlayerTakeDamage;
        
        private float _invisibilityCounter = 1;

        public WeaponSelector WeaponSelector => _weaponSelector;
        public static Wallet Wallet { get; } = new();

        private bool _canIgnoreDamage => _invisibilityCounter > 0;

        public static void DepositMoney(int value) => Wallet.DepositPoints(value);

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
            _playerController.DisableHurtCollider(_invisibilityLength);
            
            OnEntityTakeDamage?.Invoke();
            OnPlayerTakeDamage?.Invoke((float) CurrentHealth / MaxHealth);

            if (CurrentHealth <= 0)
                Kill();

            return true;
        }
        
        private void Start()
        {
            SetHealth();
            _playerController.OnPlayerDashing += _ =>
            {
                _invisibilityCounter = _dashInvisibilityLength;
                _playerController.DisableHurtCollider(_dashInvisibilityLength);
            }; 
        }
        
        private void OnValidate()
        {
            _playerController = GetComponent<PlayerController>();
            _weaponSelector = GetComponent<WeaponSelector>();
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

        private void OnDisable()
        {
            _playerController.OnPlayerDashing -= _ =>
            {
                _invisibilityCounter = _dashInvisibilityLength;
                _playerController.DisableHurtCollider(_dashInvisibilityLength);
            };
        }
    }
}