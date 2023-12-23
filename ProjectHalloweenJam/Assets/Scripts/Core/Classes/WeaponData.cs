using UnityEngine;
using Weapons;

namespace Core.Classes
{
    public class WeaponData
    {
        private readonly BaseWeapon _baseWeapon;

        private readonly float _attackSpeed;
        private readonly float _reloadingSpeed;
        
        private float _attackSpeedCounter;
        private float _reloadingSpeedCounter;
        
        private bool _canAttack = true;
        private readonly bool _isInfinity;
        
        public int LeftBullets { get; private set; }
        public int LeftBulletsInBatch { get; private set; }
        public int TotalBullets { get; }
        public int TotalBulletsInBatch { get; }

        public Sprite InHandsSprite { get; }
        public Sprite Icon { get; private set; }

        public WeaponData(BaseWeapon baseWeapon)
        {
            _baseWeapon = baseWeapon;

            _attackSpeed = baseWeapon.WeaponConfig.AttackSpeed;
            _reloadingSpeed = baseWeapon.WeaponConfig.ReloadingSpeed;
            TotalBullets = baseWeapon.WeaponConfig.TotalBullets;
            TotalBulletsInBatch = baseWeapon.WeaponConfig.BulletsPerBatch;

            _isInfinity = baseWeapon.WeaponConfig.TotalBullets < 0;
            
            InHandsSprite = baseWeapon.Description.InHands;
            Icon = baseWeapon.Description.Icon;

            LeftBullets = TotalBullets;
            _attackSpeedCounter = _attackSpeed;
            _reloadingSpeedCounter = _reloadingSpeed;
            
            RefillBatch();
        }

        public void AddBullets(float value, bool isPercent = false)
        {
            if (TotalBullets < 0)
                return;
            
            var bulletsToAdd = isPercent ? (int) (TotalBullets * (value / 100)) : (int) value;
            LeftBullets += bulletsToAdd > TotalBullets - LeftBullets ? TotalBullets - LeftBullets : bulletsToAdd;
        }

        public bool Update(float deltaTime, bool isReloadInvoked = false) => Update(deltaTime, ref isReloadInvoked); 
        
        public bool Update(float deltaTime, ref bool isReloadInvoked)
        {
            _attackSpeedCounter -= deltaTime;

            if (!isReloadInvoked && LeftBulletsInBatch > 0)
                return false;

            if (LeftBulletsInBatch == TotalBulletsInBatch)
            {
                isReloadInvoked = false;
                return false;
            }
            
            _canAttack = false;
            _reloadingSpeedCounter -= deltaTime;

            if (_reloadingSpeedCounter > 0) 
                return false;
            
            _reloadingSpeedCounter = _reloadingSpeed;
            RefillBatch();
            return true;
        }

        public bool TryToAttack(Vector2 startPosition, Vector2 direction)
        {
            if (!_canAttack || LeftBulletsInBatch <= 0 || _attackSpeedCounter > 0) 
                return false;
            
            LeftBulletsInBatch--;
            
            _baseWeapon.TryToUse(startPosition, direction);
            _attackSpeedCounter = _attackSpeed;
            return true;
        }
        
        private void RefillBatch()
        {
            if (_isInfinity)
            {
                _canAttack = true;
                LeftBulletsInBatch = TotalBulletsInBatch;
                return;
            }

            var needed = TotalBulletsInBatch - LeftBulletsInBatch;
            LeftBulletsInBatch = LeftBullets > TotalBulletsInBatch ? TotalBulletsInBatch : LeftBullets;
            LeftBullets -= LeftBullets - needed < 0 ? LeftBulletsInBatch : needed;
     
            _canAttack = true;
        }
    }
}