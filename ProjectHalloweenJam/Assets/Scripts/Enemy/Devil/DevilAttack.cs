using UnityEngine;
using System.Collections;

public class DevilAttack : EnemyAttack
{
    private string _attackName = "Attack";
    private bool _canAttack = true;
    private bool _isAttack = false;

    public bool IsAttack => _isAttack;

    public override void Attack()
    {
        if(!_isAttack && _canAttack)
            StartCoroutine(AttackCor());
    }

    private IEnumerator AttackCor()
    {
        _isAttack = true;
        float elapsedTime = 0f;
        float attackTime = 2f;
        Animator.SetBool(_attackName, true);
        while (elapsedTime < attackTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Animator.SetBool(_attackName, false);
        _isAttack = false;
        _canAttack = false;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        float elapsedTime = 0f;
        float reloadTime = 2f;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _canAttack = true;
    }
}
