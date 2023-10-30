using System.Collections;
using UnityEngine;
using Enemy.EnemyEntity;

namespace Enemy.Evil
{
    public class EvilAtack : EnemyAttack
    {
        private bool _canAttack = true;
        private bool _isAttacking = false;
        public bool IsAttacking => _isAttacking;
   
        public override void Attack()
        {
            if (_isAttacking || !_canAttack)
                return;

            StartCoroutine(AttackCor());
        }

        private IEnumerator AttackCor()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(2f);

            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            _isAttacking = false;
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            _canAttack = false;
            yield return new WaitForSeconds(2f);
            _canAttack = true;
        }
    }
}
