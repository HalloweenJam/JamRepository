using UnityEngine;
using Weapons;

namespace Core
{
    public class WeaponData
    {
        private readonly BaseWeapon _baseWeapon;

        private readonly float _attackSpeed;
        private readonly float _reloadingSpeed;
        
        private float _attackSpeedCounter;
        private float _reloadingSpeedCounter;
        
        private readonly int _bulletsPerBatch;
        
        private int _leftBullets;
        private int _leftBulletsInBatch;

        public WeaponData(BaseWeapon baseWeapon)
        {
            _baseWeapon = baseWeapon;

            _attackSpeed = baseWeapon.WeaponConfig.AttackSpeed;
            _reloadingSpeed = baseWeapon.WeaponConfig.ReloadingSpeed;
            _leftBullets = baseWeapon.WeaponConfig.TotalBullets;
            _bulletsPerBatch = baseWeapon.WeaponConfig.BulletsPerBatch;

            _attackSpeedCounter = _attackSpeed;
            _reloadingSpeedCounter = _reloadingSpeed;
            
            RefillBatch();
        }

        private void RefillBatch()
        {
            _leftBulletsInBatch = _leftBullets > _bulletsPerBatch ? _bulletsPerBatch : _leftBullets;
            _leftBullets -= _leftBulletsInBatch;
        }

        public void Update(float deltaTime)
        {
            _attackSpeedCounter -= deltaTime;

            if (_leftBulletsInBatch > 0)
                return;
            
            _reloadingSpeedCounter -= deltaTime;

            if (_reloadingSpeedCounter > 0) 
                return;
            
            _reloadingSpeedCounter = _reloadingSpeed;
            RefillBatch();
        }

        public bool TryToAttack(Vector2 position)
        {
            if (_leftBulletsInBatch <= 0 || _attackSpeedCounter > 0) 
                return false;
            
            _leftBulletsInBatch--;
            _baseWeapon.TryToUse(position);
            _attackSpeedCounter = _attackSpeed;
            return true;
        }
    }
}