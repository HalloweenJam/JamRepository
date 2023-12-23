using System;
using System.Collections.Generic;
using Core.Classes;
using Managers;
using Player.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private bool _isReloadInvoked;
        private bool _isAttacking;
        private bool _isPlayer;
        private bool _isStarted;

        private Vector2 _cashedMousePosition;
        private Camera _camera;
        private InputReader _inputReader;

        private WeaponData _currentWeapon => _weapons[_selectedWeaponIndex];

        public WeaponHolder WeaponHolder => _weaponHolder;
        
        public Action<WeaponData, bool> OnWeaponUsed;

        public Transform FirePoint => _firePoint;

        public void Add(BaseWeapon weapon) => _weapons.Add(new WeaponData(weapon));

        public bool TryToAttack(Vector2 targetPosition, bool isDirection = true)
        {
            var direction = isDirection ? targetPosition : (targetPosition - (Vector2) _firePoint.position).normalized;
            var selectedWeapon = _currentWeapon;
            
            OnWeaponUsed?.Invoke(selectedWeapon, false);
            
            return selectedWeapon.TryToAttack(_firePoint.position, direction);
        }

        public void SetFirePoint(Transform firePoint) => _firePoint = firePoint;

        public void SetWeaponByIndex(int weaponIndex) => _selectedWeaponIndex = weaponIndex;
           
        public void Init(bool isPlayer = false)
        { 
            _isPlayer = isPlayer;
            Start();
            _isStarted = true;
        }

        private void OnDisable() => SceneManager.sceneLoaded -= (_, _) => _camera = Camera.main;

        private void Start()
        {
            SceneManager.sceneLoaded += (_, _) => _camera = Camera.main;
            
            if (_isStarted)
                return;
            
            if (_isPlayer)
            {
                _inputReader = InputReaderManager.Instance.GetInputReader();

                _inputReader.ShootingEvent += () => _isAttacking = true;
                _inputReader.ShootingCancelledEvent += () => _isAttacking = false;

                _inputReader.ReloadEvent += () => _isReloadInvoked = true;

                _inputReader.MouseWheelScrollEvent += ChangeWeapon;

                _camera = Camera.main;
            }
            
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
            if (_isAttacking)
            {
                var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                TryToAttack(mousePosition, false);
            }
            
            var deltaTime = Time.fixedDeltaTime;

            for (int i = 0; i < _weapons.Count; i++)
            {
                if (i == _selectedWeaponIndex)
                {
                    if (!_weapons[i].Update(deltaTime, ref _isReloadInvoked))
                        continue;
                    
                    _isReloadInvoked = false;
                    OnWeaponUsed?.Invoke(_weapons[i], false);
                }
                else
                {
                    _weapons[i].Update(deltaTime);
                }
            }
        }

        public void AddRefill(float refillAmount, bool isPercent)
        {
            foreach (var weapon in _weapons)
            {
                weapon.AddBullets(refillAmount, isPercent);
            }
            
            OnWeaponUsed?.Invoke(_currentWeapon, false);
        }
    }
}