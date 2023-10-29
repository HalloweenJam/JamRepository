using System;
using System.Collections.Generic;
using Core.Classes;
using Managers;
using Player.Controls;
using UnityEngine;
using Weapons;

namespace Entities
{
    public class WeaponSelector : MonoBehaviour
    {
        [SerializeField] private WeaponHolder _weaponHolder;
        [SerializeField] private Transform _firePoint;
        
        [SerializeField] private List<RangeWeapon> _weaponsToAdd;
        
        private readonly List<WeaponData> _weapons = new();

        private int _selectedWeaponIndex = 0;
        private bool _isHoldingWeapon;

        private Vector2 _direction;

        private InputReader _inputReader;

        public void Add(BaseWeapon weapon) => _weapons.Add(new WeaponData(weapon));

        public bool TryToAttack(Vector2 targetPosition, bool isDirection = true)
        {
            var direction = isDirection ? targetPosition : (targetPosition - (Vector2) _firePoint.position).normalized;
            
            return _weapons[_selectedWeaponIndex].TryToAttack(_firePoint.position, direction);
        }

        public void SetWeaponByIndex(int weaponIndex)
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex > _weapons.Count - 1)
                throw new OutOfMemoryException("Selected Weapon Index is out of available weapons count");
            
            _selectedWeaponIndex = weaponIndex;
        }

        private void Start()
        {
            _inputReader = InputReaderManager.Instance.GetInputReader();

            _inputReader.ShootingEvent += () => _isHoldingWeapon = true;
            _inputReader.ShootingCancelledEvent += () => _isHoldingWeapon = false;

            _inputReader.MouseWheelScrollEvent += ChangeWeapon;
            _inputReader.MousePositionEvent += mousePosition =>
            {
                _direction = (mousePosition - (Vector2) _firePoint.position).normalized;
            };
            
            foreach (var weapon in _weaponsToAdd)
            {
                Add(weapon);
            }
            
            SetWeaponSprite();
        }

        private void ChangeWeapon(float direction)
        {
            var dir = direction > 0 ? 1 : -1;
            
            _selectedWeaponIndex = (_selectedWeaponIndex + _weapons.Count + dir) % _weapons.Count;
            
            SetWeaponSprite();
        }

        private void SetWeaponSprite()
        {
            _weaponHolder.SetWeaponSprite(_weapons[_selectedWeaponIndex].WeaponSprite);
        }

        private void Update()
        {
            if (_isHoldingWeapon)
                TryToAttack(_direction);
            
            var deltaTime = Time.deltaTime;
            
            foreach (var weapon in _weapons)
            {
                weapon.Update(deltaTime);
            }
        }
    }
}