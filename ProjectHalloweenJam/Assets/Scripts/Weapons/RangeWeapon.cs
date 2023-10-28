using Managers;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Range")]
    public class RangeWeapon : BaseWeapon
    {
        [SerializeField] private float _bulletSpeed;
        
        public override bool TryToUse(Vector2 startPosition, Vector2 direction)
        {
            //BulletPoolingManager.Instance.GetBullet(startPosition, direction, BulletConfig);
            AttackManager.Instance.GetSelectAttack(startPosition, direction, BulletConfig);

            return true;
        }
    }
}