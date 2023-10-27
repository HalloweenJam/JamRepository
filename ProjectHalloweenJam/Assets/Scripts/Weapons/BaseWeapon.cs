using Core;
using UnityEngine;

namespace Weapons
{
    public abstract class BaseWeapon : ScriptableObject
    {
        [SerializeField] private Description _description;
        [SerializeField] private int _damage;
        
        [SerializeField] private WeaponConfig _weaponConfig;
        
        private Transform _firePointTransform;
        
        public WeaponConfig WeaponConfig => _weaponConfig;
        
        public void Init(Transform firePoint)
        {
            _firePointTransform = firePoint;
        }

        public abstract bool TryToUse(Vector2 direction);
    }
}