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

        private bool _isInfinity;
        
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

        public bool Update(float deltaTime)
        {
            _attackSpeedCounter -= deltaTime;

            if (LeftBulletsInBatch > 0)
                return false;
            
            _reloadingSpeedCounter -= deltaTime;

            if (_reloadingSpeedCounter > 0) 
                return false;
            
            _reloadingSpeedCounter = _reloadingSpeed;
            RefillBatch();
            return true;
        }

        public bool TryToAttack(Vector2 startPosition, Vector2 direction)
        {
            if (LeftBulletsInBatch <= 0 || _attackSpeedCounter > 0) 
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
                LeftBulletsInBatch = TotalBulletsInBatch;
                return;
            }

            LeftBulletsInBatch = LeftBullets > TotalBulletsInBatch ? TotalBulletsInBatch : LeftBullets;
            LeftBullets -= LeftBulletsInBatch;
        }
    }
}