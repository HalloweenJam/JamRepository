using Core.Interfaces;
using Entities;
using UnityEngine;

namespace Enemy.EnemyEntity
{
    public class EnemyAttack : MonoBehaviour, IAttackable
    {
        [Header("Damage")]
        [SerializeField] private int _damage;
        protected EnemyMovement EnemyMovement;
        protected Animator Animator;

        protected bool IsAttacking = false;
        protected bool IsReload = false;

        protected WeaponSelector WeaponSelector { get; private set; }

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
            EnemyMovement = GetComponent<EnemyMovement>();
            EnemyMovement.Attack += Attack;

            WeaponSelector = GetComponent<WeaponSelector>();
            if(WeaponSelector != null)
                WeaponSelector.SetWeaponByIndex(0);
        }

        public virtual void Attack() { }
   
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision != null & collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
                damageable.TryTakeDamage(_damage);
        }
    }
}
