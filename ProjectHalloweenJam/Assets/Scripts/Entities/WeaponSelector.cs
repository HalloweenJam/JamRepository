using System.Collections.Generic;
using Core;
using Player.Controls;
using UnityEngine;
using Weapons;

namespace Entities
{
    public class WeaponSelector : MonoBehaviour
    {
        [SerializeField] private RangeWeapon _weapon;
        
        private readonly List<WeaponData> _weapons = new();

        private int _selectedWeaponIndex = 0;
        private bool _isHoldingWeapon;

        private InputReader _inputReader;

        private void Start()
        {
            _inputReader = InputReaderManager.Instance.GetInputReader();

            _inputReader.ShootingEvent += () => _isHoldingWeapon = true;
            _inputReader.ShootingCancelledEvent += () => _isHoldingWeapon = false;

            _inputReader.MouseWheelScrollEvent += ChangeWeapon;
            
            Add(_weapon);
        }

        private void ChangeWeapon(float direction)
        {
            _selectedWeaponIndex += direction > 0 ? 1 : -1;
        }

        public void Add(BaseWeapon weapon)
        {
            weapon.Init(transform);
            _weapons.Add(new WeaponData(weapon));
        }

        private void Update()
        {

            
            if (_isHoldingWeapon)
                _weapons[_selectedWeaponIndex].TryToAttack(transform.position);
            
            var deltaTime = Time.deltaTime;
            
            foreach (var weapon in _weapons)
            {
                weapon.Update(deltaTime);
            }
        }
    }
}