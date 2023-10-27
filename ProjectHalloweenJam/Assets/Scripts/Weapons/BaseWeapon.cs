using Core;
using UnityEngine;

namespace Weapons
{
    public abstract class BaseWeapon : ScriptableObject
    {
        [SerializeField] private Description _description;
        [SerializeField] private WeaponConfig _weaponConfig;
        [Space]
        [SerializeField] private int _damage;
        
        public WeaponConfig WeaponConfig => _weaponConfig;

        protected int Damage => _damage;
        
        public abstract bool TryToUse(Vector2 startPosition, Vector2 direction);
    }
}