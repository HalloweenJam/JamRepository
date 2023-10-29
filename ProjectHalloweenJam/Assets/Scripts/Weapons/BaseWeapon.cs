using Bullet;
using Core.Classes;
using UnityEngine;

namespace Weapons
{
    public abstract class BaseWeapon : ScriptableObject
    {
        [SerializeField] private Description _description;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private BulletConfig _bulletConfig;
        
        public Description Description => _description;
        public WeaponConfig WeaponConfig => _weaponConfig;
        public BulletConfig BulletConfig => _bulletConfig;
        
        public abstract bool TryToUse(Vector2 startPosition, Vector2 direction);
    }
}