using UnityEngine;

namespace Core.Interfaces
{
    public interface IDamageable
    {
        public bool TryTakeDamage(int damage, ContactPoint2D hitPoint = default, float hitForce = 0, bool instantKill = false, bool ignoreInvisibility = false);
    }
}