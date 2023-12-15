using System.Collections;
using UnityEngine;
using Enemy.EnemyEntity;

namespace Enemy.Evil
{
    [RequireComponent(typeof(GlowEffect))]
    public class EvilAtack : EnemyAttack       
    {
        private GlowEffect _glow;
        private const string _attack = "Attack";

        private void Awake() => _glow = GetComponent<GlowEffect>();

        public override void Attack()
        {
            if (IsReload || IsAttacking)
                return;

            IsAttacking = true;
            Animator.SetBool(_attack, IsAttacking);
            _glow.ActivateGlow();
        }    

        public void SpawnBullet()
        {
            _glow.DeactivateGlow();
            IsAttacking = false;
            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            IsReload = true;
            Animator.SetBool(_attack, IsAttacking);
            yield return new WaitForSeconds(1f);
            IsReload = false;
        }
    }
}
