using System;
using Core;
using UnityEngine;

namespace Entities
{
    public abstract class EntityStats : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [SerializeField] private int _maxHealth = 5;
        [SerializeField] protected int CurrentHealth = 5;

        public Action OnEntityTakeDamage;
        
        protected int MaxHealth => _maxHealth;
        
        public abstract bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false);

        protected abstract void Kill();
    }
}