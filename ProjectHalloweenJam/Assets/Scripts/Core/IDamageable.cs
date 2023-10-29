namespace Core
{
    public interface IDamageable
    {
        public bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false);
    }
}
