using System.Collections.Generic;
using Core;
using Core.Classes;
using Managers;
using Player.Controls;
using UnityEngine;
using Weapons;

namespace Entities
{
    public class WeaponSelector : MonoBehaviour
    {
        [SerializeField] private List<RangeWeapon> _weaponsToAdd;
        
        private readonly List<WeaponData> _weapons = new();

        private int _selectedWeaponIndex = 0;
        private bool _isHoldingWeapon;

        private Vector2 _direction;

        private InputReader _inputReader;

        private Vector3 _positionPlayer;
        
        public void Add(BaseWeapon weapon)
        {
            _weapons.Add(new WeaponData(weapon));
        }

        private void OnValidate()
        { 
            _positionPlayer = transform.position;
        }
            private void Start()
        {
            _inputReader = InputReaderManager.Instance.GetInputReader();

            _inputReader.ShootingEvent += () => _isHoldingWeapon = true;
            _inputReader.ShootingCancelledEvent += () => _isHoldingWeapon = false;

            _inputReader.MouseWheelScrollEvent += ChangeWeapon;
            _inputReader.MousePositionEvent += mousePosition =>
            {
                _direction = (mousePosition - (Vector2) _positionPlayer).normalized;
            };
            
            foreach (var weapon in _weaponsToAdd)
            {
                Add(weapon);
            }
        }

        private void ChangeWeapon(float direction)
        {
            var dir = direction > 0 ? 1 : -1;
            
            _selectedWeaponIndex = (_selectedWeaponIndex + _weapons.Count + dir) % _weapons.Count;
        }

        private void Update()
        {
            if (_isHoldingWeapon)
                _weapons[_selectedWeaponIndex].TryToAttack(transform.position, _direction);
            
            var deltaTime = Time.deltaTime;
            
            foreach (var weapon in _weapons)
            {
                weapon.Update(deltaTime);
            }
        }
    }
}