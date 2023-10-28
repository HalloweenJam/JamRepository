using Bullet;
using Core;
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

        private int _damage;
        
        private float _speed;
        private float _lifeTimeCounter;

        private bool _canTakeDamage;
        
        private Vector2 _direction;
        
        public void Init(Vector2 direction, BulletConfig bulletConfig)
        {
            _render.sprite = bulletConfig.Sprite;
            _collider.radius = bulletConfig.Radius;
            _direction = direction;
            _speed = bulletConfig.Speed;
            _damage = bulletConfig.Damage;
            
            _canTakeDamage = false;
            _lifeTimeCounter = _lifeTime;
        }

        private void OnValidate()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _render = GetComponent<SpriteRenderer>();
            _collider = GetComponent<CircleCollider2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _speed * _direction;

            _lifeTimeCounter -= Time.fixedDeltaTime;

            if (_lifeTimeCounter < 0)
                BulletPoolingManager.Instance.Release(this);
        }

        private void OnTriggerExit2D(Collider2D other) => _canTakeDamage = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canTakeDamage)
                return;
            
            if (other.TryGetComponent<IDamageable>(out var damageable))
                damageable.TryTakeDamage(_damage);

            BulletPoolingManager.Instance.Release(this);
        }
    }
}
