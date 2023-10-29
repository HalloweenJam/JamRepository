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

        private Vector2 _cashedMousePosition;
        private Vector2 _direction;

        private InputReader _inputReader;

        private WeaponData _currentWeapon => _weapons[_selectedWeaponIndex];
        
        public Action<WeaponData, bool> OnWeaponUsed;

        public void Add(BaseWeapon weapon) => _weapons.Add(new WeaponData(weapon));

        public bool TryToAttack(Vector2 targetPosition, bool isDirection = true)
        {
            var direction = isDirection ? targetPosition : (targetPosition - (Vector2) _firePoint.position).normalized;
            var selectedWeapon = _currentWeapon;
            
            OnWeaponUsed?.Invoke(selectedWeapon, false);
            
            return selectedWeapon.TryToAttack(_firePoint.position, direction);
        }

        public void SetWeaponByIndex(int weaponIndex)
        {
            _selectedWeaponIndex = weaponIndex;
        }
        
        private void Start()
        {
            _inputReader = InputReaderManager.Instance.GetInputReader();

            _inputReader.ShootingEvent += () => _isHoldingWeapon = true;
            _inputReader.ShootingCancelledEvent += () => _isHoldingWeapon = false;

            _inputReader.MouseWheelScrollEvent += ChangeWeapon;
            _inputReader.MousePositionEvent += mousePosition => _cashedMousePosition = mousePosition;
            
            foreach (var weapon in _weaponsToAdd)
            {
                Add(weapon);
            }
            
            OnWeaponUsed?.Invoke(_currentWeapon, true);
            SetWeaponSprite();
        }

        private void ChangeWeapon(float direction)
        {
            var dir = direction > 0 ? 1 : -1;
            
            _selectedWeaponIndex = (_selectedWeaponIndex + _weapons.Count + dir) % _weapons.Count;
            
            OnWeaponUsed?.Invoke(_currentWeapon, true);
            
            SetWeaponSprite();
        }

        private void SetWeaponSprite()
        {
            if (_weaponHolder != null)
                _weaponHolder.SetWeaponSprite(_currentWeapon.InHandsSprite);
        }

        private void FixedUpdate()
        {
            if (_isHoldingWeapon)
            {
                _direction = (_cashedMousePosition - (Vector2) _firePoint.position).normalized;
                TryToAttack(_direction, true);
            }
            
            var deltaTime = Time.fixedDeltaTime;

            for (int i = 0; i < _weapons.Count; i++)
            {
                if (i == _selectedWeaponIndex)
                {
                    if (_weapons[i].Update(deltaTime))
                        OnWeaponUsed?.Invoke(_weapons[i], false);
                }
                else
                {
                    _weapons[i].Update(deltaTime);
                }
            }
        }
    }
}