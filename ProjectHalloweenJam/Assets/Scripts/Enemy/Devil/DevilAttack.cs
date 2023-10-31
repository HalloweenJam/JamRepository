using System.Collections;
using Enemy.EnemyEntity;
using UnityEngine;

namespace Enemy.Devil
{
    public class DevilAttack : EnemyAttack
    {
        private string _attackName = "Attack";

        public override void Attack()
        {
            if (IsAttacking || IsReload) 
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
            IsAttacking = true;
            Animator.SetBool(_attackName, true);
            
            yield return new WaitForSeconds(2f);
            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            
            Animator.SetBool(_attackName, false);

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
