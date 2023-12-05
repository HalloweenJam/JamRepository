using System.Collections;
using UnityEngine;
using Enemy.EnemyEntity;
using UnityEngine.Video;

namespace Enemy.Evil
{
    public class EvilAtack : EnemyAttack
    {
        public override void Attack()
        {
            if (IsReload)
                return;
          
            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            IsReload = true;
            yield return new WaitForSeconds(1f);
            IsReload = false;
        }
    }
}
