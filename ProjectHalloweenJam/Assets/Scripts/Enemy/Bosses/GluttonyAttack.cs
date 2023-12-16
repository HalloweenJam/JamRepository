using Enemy.EnemyEntity;
using System.Collections;
using UnityEngine;

public class GluttonyAttack : EnemyAttack
{
    [SerializeField] private FatBall _fatBallPrefab;
    [SerializeField] private Transform _jumpFirePoint;
    private Transform _defaultFirePoint;

    [Header("AnimationParameters")]
    private string _splitAttack = "Split";
    private string _burpAttack = "Burp";
    private string _jump = "Jump";

    private bool _isExoSlam = false;
    private bool _isJumping = false;

    public override void Attack()
    {
        if (IsAttacking || IsReload)
            return;

        SelectAttack();
    }

    private void SelectAttack()
    {
        System.Random random = new ();
        var value = 2;//random.Next(0, 3);
        Debug.Log(value);
        switch (value)
        {
            case 0:
                StartCoroutine(ShootFatBall());
                break;
            case 1:
                SplitBelching();
                break;
            case 2:     
                Jump();
                break;
        }
    }

    private void Jump()
    {
        _defaultFirePoint = WeaponSelector.FirePoint;
        WeaponSelector.SetFirePoint(_jumpFirePoint);

        WeaponSelector.SetWeaponByIndex(1);

        var value = Random.value;
        int repeat;

        switch (value)
        {
            case < .10f:
                repeat = 4;
                break;
            case < .66f:
                repeat = 3;
                break;
            default:
                repeat = 2;
                break;
        }

        StartCoroutine(Jump(repeat));
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

    private IEnumerator Jump(int repeat)       
    {
        _isJumping = true;
        IsAttacking = true;

        for (int i = 0; i < repeat; i++) 
        {
            Animator.SetBool(_jump, true);
            yield return new WaitUntil(() => !_isJumping);
            StartCoroutine(ExoSlam());
            yield return new WaitUntil(() => !_isExoSlam);
        }

        IsAttacking = false;
        WeaponSelector.SetFirePoint(_defaultFirePoint);
        StartCoroutine(Reload(2f));
    }

    private IEnumerator ExoSlam()
    {
        Animator.SetBool(_jump, false);
        _isExoSlam = true;
        System.Random random = new();
        var value = random.Next(1,3);
        for (int i = 0; i < value; i++)
        {
            WeaponSelector.TryToAttack(EnemyMovement.Player.position, false);
            yield return new WaitForSeconds(0.25f);
        }
        _isExoSlam = false;       
    }    

    public void EndJump() => _isJumping = false;

    private IEnumerator Reload(float time)
    {
        IsReload = true;
        yield return new WaitForSeconds(time);
        IsReload = false;
    }
}