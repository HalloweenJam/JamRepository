using UnityEngine;
using System;

public class EnemyAttack : MonoBehaviour, IAttackable
{
    [Header("Damage")]
    [SerializeField] private int _damage;
    protected EnemyMovement EnemyMovement;
    protected Animator Animator;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        EnemyMovement = GetComponent<EnemyMovement>();
    }

    public virtual void Attack()
    {
        
    }
}
