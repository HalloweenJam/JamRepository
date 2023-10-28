using Bullet;
using Core;
using Core.Classes;
using UnityEngine;

namespace Weapons
{
    public abstract class BaseWeapon : ScriptableObject
    {
        [SerializeField] private Description _description;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private BulletConfig _bulletConfig;
        [Space]
        [SerializeField] private int _damage;
        
        public WeaponConfig WeaponConfig => _weaponConfig;
        public BulletConfig BulletConfig => _bulletConfig;

        protected int Damage => _damage;
        
        public abstract bool TryToUse(Vector2 startPosition, Vector2 direction);
    }
}