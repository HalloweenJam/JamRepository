using System;
using System.Collections;
using Entities;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyMovement), typeof(SpriteRenderer))]
    public class EnemyStats : EntityStats
    {
        [Header("Damage")]
        [SerializeField] private int _damage;

        public static Action<Vector2> OnDeath;

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            CurrentHealth = MaxHealth;
            _defaultColor = _spriteRenderer.color;
        }

        public override bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
        {
            if (instantKill)
            {
                Kill();
                StartCoroutine(PaintingSprite());
                return true;
            }

            OnEntityTakeDamage?.Invoke();
            CurrentHealth -= damage;
            StartCoroutine(PaintingSprite());

            if (CurrentHealth <= 0)
                Kill();

            return true;
        }

        protected override void Kill() 
        {
            CurrentHealth = 0;
            OnDeath?.Invoke(transform.position);
            Destroy(gameObject);
        }

        private IEnumerator PaintingSprite()
        {
            float ignoreTime = 1f;
            float elapsedTime = 0f;
            _spriteRenderer.color = Color.red;
            while (elapsedTime < ignoreTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }       
            _spriteRenderer.color = _defaultColor;
        }
    }
}
