using Managers;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Range")]
    public class RangeWeapon : BaseWeapon
    {
        public override bool TryToUse(Vector2 startPosition, Vector2 direction)
        {
            AttackManager.SelectAttack(startPosition, direction, BulletConfig);

            return true;
        }
    }
}