using System.Collections;
using Enemy.EnemyEntity;
using UnityEngine;

namespace Enemy.Devil
{
    public class DevilAttack : EnemyAttack
    {
        private string _attackName = "Attack";
        private bool _canAttack = true;
        private bool _isAttacking = false;
        public bool IsAttacking => _isAttacking;

        public override void Attack()
        {
            if (_isAttacking || !_canAttack) 
                return;
            
            SelectAttack();
        }

        private void SelectAttack()
        {
            var value = Random.value;

            switch (value)
            {
                case < .33f:
                    WeaponSelector.SetWeaponByIndex(0);
                    break;
                case < .66f:
                    WeaponSelector.SetWeaponByIndex(1);
                    break;
                default:
                    WeaponSelector.SetWeaponByIndex(2);
                    break;
            }

            StartCoroutine(AttackCor());
        }

        private IEnumerator AttackCor()
        {
            _isAttacking = true;
        
            Animator.SetBool(_attackName, true);
            
            yield return new WaitForSeconds(2f);
            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            
            Animator.SetBool(_attackName, false);
            
            _isAttacking = false;
            _canAttack = false;
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(2f);
            _canAttack = true;
        }
    }
}
