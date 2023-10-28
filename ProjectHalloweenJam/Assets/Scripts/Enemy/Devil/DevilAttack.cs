using UnityEngine;
using System.Collections;

public class DevilAttack : EnemyAttack
{
    private string _attackName = "Attack";
    private bool _isAttack = false;

    public bool IsAttack => _isAttack;

    public override void Attack()
    {
        StartCoroutine(AttackCor());
    }

    private IEnumerator AttackCor()
    {
        _isAttack = true;
        float elapsedTime = 0f;
        float attackTime = 3f;
        Animator.SetBool(_attackName, true);
        while (elapsedTime < attackTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Animator.SetBool(_attackName, false);
        _isAttack = false;
    }
}
