using System.Collections;
using UnityEngine;
using Enemy.EnemyEntity;

namespace Enemy.Evil
{
    public class EvilAtack : EnemyAttack
    { 
        public override void Attack()
        {
            if (IsAttacking || IsReload)
                return;

            StartCoroutine(AttackCor());
        }

        private IEnumerator AttackCor()
        {       
            yield return new WaitForSeconds(2f);

            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            IsAttacking = false;
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            IsReload = true;
            yield return new WaitForSeconds(2f);
            IsReload = false;
        }
    }
}
