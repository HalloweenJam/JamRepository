using Enemy.EnemyEntity;
using System.Collections;
using UnityEngine;

public class GluttonyAttack : EnemyAttack
{
    [SerializeField] private FatBall _fatBallPrefab;

    private string _splitAttack = "Split";
    private string _burpAttack = "Burp";

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
                StartCoroutine(ShootFatBall());
                break;
            case < .66f:
                SplitBelching();
                break;
        }
    }

    private void SplitBelching()
    {
        WeaponSelector.SetWeaponByIndex(0);

        var value = Random.value;
        int repeat;

        switch (value)
        {
            case < .10f:
                repeat = 5;
                break;
            case < .66f:
                repeat = 3;
                break;
            default:
                repeat = 4;
                break;
        }

        StartCoroutine(SplitAttack(repeat));
    }

    private IEnumerator ShootFatBall()
    {
        IsAttacking = true;
        Animator.SetBool(_burpAttack, true);

        yield return new WaitForSeconds(0.75f);
        FatBall fatBall = Instantiate(_fatBallPrefab);
        fatBall.Initialize(EnemyMovement.Player, WeaponSelector.FirePoint);
        Animator.SetBool(_burpAttack, false);
        IsAttacking = false;
        StartCoroutine(Reload(1f));
    }

    private IEnumerator SplitAttack(int repeat)
    {
        IsAttacking = true;

        for (int i = 0; i < repeat; i++)
        {
            Animator.SetBool(_splitAttack, true);
            yield return new WaitForSeconds(1.25f);
            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            Animator.SetBool(_splitAttack, false);
            yield return new WaitForSeconds(0.25f);
        }

        IsAttacking = false;
        StartCoroutine(Reload(2f));
    }

    private IEnumerator Reload(float time)
    {
        IsReload = true;
        yield return new WaitForSeconds(time);
        IsReload = false;
    }
}