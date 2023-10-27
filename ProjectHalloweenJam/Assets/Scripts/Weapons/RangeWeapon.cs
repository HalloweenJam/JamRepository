using Testing;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Range")]
    public class RangeWeapon : BaseWeapon
    {
        [SerializeField] private Bullet.Bullet _projectile;
        
        public override bool TryToUse(Vector2 direction)
        {
            Debug.Log("пиу пиу");
            
            return true;
        }
    }
}