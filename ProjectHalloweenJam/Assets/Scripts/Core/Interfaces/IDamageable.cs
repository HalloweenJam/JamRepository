namespace Core.Interfaces
{
    public interface IDamageable
    {
       // void OnLine(BulletInfo info);
        public bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false);
    }
}