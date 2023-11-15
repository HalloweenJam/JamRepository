using Enemy.EnemyEntity;
using System.Collections;
using UnityEngine;

public class GluttonyAttack : EnemyAttack
{
    private string _burpAttack = "Burp";

    public override void Attack()
    {
        if (IsAttacking || IsReload)
            return;

        Belching();
    }

    private void Belching()
    {
        WeaponSelector.SetWeaponByIndex(0);
        StartCoroutine(BurpCoroutine());
    }

    private IEnumerator BurpCoroutine()
    {
        IsAttacking = true;
        Animator.SetBool(_burpAttack, true);

        WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
        yield return new WaitForSeconds(2f);
        WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
        Animator.SetBool(_burpAttack, false);

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