using System;
using Entities;
using UnityEngine;

namespace Enemy.EnemyEntity
{
    [RequireComponent(typeof(EnemyMovement), typeof(SpriteRenderer), typeof(DissolveEffect))]
    public class EnemyStats : EntityStats
    {
        [Header("Loot")] 
        [SerializeField, Range(0f, 1f)] private float _dropLootChance = .2f;

        private Collider2D _collider;
        private EnemyMovement _movement;
        private DissolveEffect _dissolveEffect;

        private bool _isDead = false;

        public static Action<Vector2, float> OnDeath;
        
        private static readonly int Fade = Shader.PropertyToID("_Fade");

        public bool Dissolved => _dissolveEffect.Dissolved;

        public void Initialize(Transform playerPosition)
        {
            SetHealth();
            _movement = GetComponent<EnemyMovement>();
            _dissolveEffect = GetComponent<DissolveEffect>();
            _collider = GetComponent<Collider2D>();

            _movement.enabled = false;
            _collider.enabled = false;

            _movement.Initialize(playerPosition, this);
            Appearance();
        }

        public override bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
        {
            if (instantKill)
            {
                Kill();
                return true;
            }

            OnEntityTakeDamage?.Invoke();
            
            CurrentHealth -= damage;
            if (CurrentHealth <= 0 && !_isDead)           
                Kill();

            return true;
        }

        protected override void Kill()
        {
            _isDead = true;
            CurrentHealth = 0;
            OnDeath?.Invoke(transform.position, _dropLootChance);
           
            Destroy(gameObject);
        }

        public void Appearance()  
        {
            Behaviour component = _dissolveEffect.CanActivateComponent ? _movement : null;
            _dissolveEffect.Appearance(component, _collider);
        }

        public void Dissolve() => _dissolveEffect.Dissolve(_movement);
    }
}
