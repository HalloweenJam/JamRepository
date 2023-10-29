using Entities;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IAttackable
{
    [Header("Damage")]
    [SerializeField] private int _damage;
    protected EnemyMovement EnemyMovement;
    protected Animator Animator;

    protected WeaponSelector WeaponSelector { get; private set; }

    protected virtual void Start()
    {
        Animator = GetComponent<Animator>();
        EnemyMovement = GetComponent<EnemyMovement>();
        
        WeaponSelector = GetComponent<WeaponSelector>();
        WeaponSelector.SetWeaponByIndex(0);
    }

    public virtual void Attack() { }
}
