using System;
using Core.Interfaces;
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

        protected void SetHealth() => CurrentHealth = _maxHealth;
        
        public abstract bool TryTakeDamage(int damage, ContactPoint2D hitPoint = default, float hitForce = 0f, bool instantKill = false, bool ignoreInvisibility = false);

        protected abstract void Kill();
    }
}