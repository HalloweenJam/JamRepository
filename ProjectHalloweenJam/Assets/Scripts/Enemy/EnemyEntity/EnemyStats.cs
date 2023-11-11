using System;
using System.Collections;
using Entities;
using UnityEngine;

namespace Enemy.EnemyEntity
{
    [RequireComponent(typeof(EnemyMovement), typeof(SpriteRenderer))]
    public class EnemyStats : EntityStats
    {
        [Header("Loot")] 
        [SerializeField, Range(0f, 1f)] private float _dropLootChance = .2f;
        [Header("Dissolve")]
        [SerializeField] private float _dissolveTime = 1f;
        [SerializeField] private Material _dissolveMaterial;
        
        private bool _dissolved = false;
        private float _elapsedTime = 0f;
        
        private EnemyMovement _movement;
        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;

        public static Action<Vector2, float> OnDeath;
        
        private static readonly int Fade = Shader.PropertyToID("_Fade");

        public bool Dissolved => _dissolved;

        public void Spawn(Transform playerPosition)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _movement = GetComponent<EnemyMovement>();
            _defaultColor = _spriteRenderer.color;

            _spriteRenderer.material = _dissolveMaterial;
            
            _movement.enabled = false;
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
            if (CurrentHealth <= 0)
                Kill();

            return true;
        }

        protected override void Kill()
        {
            CurrentHealth = 0;
            OnDeath?.Invoke(transform.position, _dropLootChance);

            Destroy(gameObject);
        }

        public void Appearance() => StartCoroutine(AppearanceCor());

        public void Dissolve() => StartCoroutine(DissolveCor());

        private IEnumerator AppearanceCor()
        {
            _dissolved = false;
            float fadeMaterial = 0;
            float percent = (1 / _dissolveTime);

            while (_elapsedTime < _dissolveTime)
            {
                fadeMaterial += percent * Time.deltaTime;
                _elapsedTime += Time.deltaTime;
                _spriteRenderer.material.SetFloat(Fade, fadeMaterial);
                yield return null;
            }
            _elapsedTime = 0f;
            _movement.enabled = true;
            _spriteRenderer.material.SetFloat(Fade, 1f);
            _dissolved = true;
        }

        private IEnumerator DissolveCor()
        {
            _dissolved = false;
            float fadeMaterial = 1;
            float percent = -(1 / _dissolveTime);

            while (_elapsedTime < _dissolveTime)
            {
                fadeMaterial += percent * Time.deltaTime;
                _elapsedTime += Time.deltaTime;
                _spriteRenderer.material.SetFloat(Fade, fadeMaterial);
                yield return null;
            }
            _elapsedTime = 0f;
            _movement.enabled = true;
            _spriteRenderer.material.SetFloat(Fade, 0f);
            _dissolved = true;
        }
    }
}
