using System;
using UnityEngine;

namespace Core.Classes
{
    [Serializable]
    public class WeaponConfig
    {
        [SerializeField] private int _totalBullets = 80;
        [SerializeField] private int _bulletsPerBatch = 7;
        [SerializeField] private float _attackSpeed = .1f;
        [SerializeField] private float _reloadingSpeed;

        public int TotalBullets => _totalBullets;
        public int BulletsPerBatch => _bulletsPerBatch;

        public float AttackSpeed => _attackSpeed;
        public float ReloadingSpeed => _reloadingSpeed;
    }
}