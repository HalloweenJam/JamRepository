using Bullet;
using Core.Interfaces;
using Managers;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(SpriteRenderer))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 5f;
        
        [SerializeField, HideInInspector] private Rigidbody2D _rigidbody;
        [SerializeField, HideInInspector] private SpriteRenderer _render;
        [SerializeField, HideInInspector] private CircleCollider2D _collider;
        [SerializeField, HideInInspector] private Animator _animator;

        private int _damage;
        
        private float _speed;
        private float _lifeTimeCounter;
        
        private bool _isEnemyBullet;
        
        private Vector2 _direction;
        
        public void Init(Vector2 direction, BulletConfig bulletConfig)
        {
            gameObject.layer = bulletConfig.IsEnemyBullet
                ? LayerMask.NameToLayer("EnemyProjectile")
                : LayerMask.NameToLayer("PlayerProjectile");
            
            if (bulletConfig.AnimatorController.Enabled)
            {
                _animator.runtimeAnimatorController = bulletConfig.AnimatorController.Value;
            }
            else
            {
                _animator.runtimeAnimatorController = null;
                _render.sprite = bulletConfig.Sprite;
            }

            _collider.radius = 0.3f;
            _direction = direction;
            _speed = bulletConfig.Speed;
            _damage = bulletConfig.Damage;
            
            _lifeTimeCounter = _lifeTime;
        }

        private void OnValidate()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _render = GetComponent<SpriteRenderer>();
            _collider = GetComponent<CircleCollider2D>();
            _animator = GetComponent<Animator>();

            _collider.isTrigger = false;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _speed * _direction;

            _lifeTimeCounter -= Time.fixedDeltaTime;

            if (_lifeTimeCounter < 0)
                BulletPoolingManager.Instance.Release(this);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                print(other.collider.name);
                damageable.TryTakeDamage(_damage);
            }

            BulletPoolingManager.Instance.Release(this);
        }
    }
}
