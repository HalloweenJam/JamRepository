using Bullet;
using Core.Interfaces;
using Managers;
using UnityEngine;
using System.Collections;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(SpriteRenderer))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 5f;

        [Header("AnimationsOnHits")]
        [SerializeField] private RuntimeAnimatorController _hitWallAnimator;
        [SerializeField] private RuntimeAnimatorController _hitTargetAnimator;

        [SerializeField, HideInInspector] private Rigidbody2D _rigidbody;
        [SerializeField, HideInInspector] private SpriteRenderer _render;
        [SerializeField, HideInInspector] private CircleCollider2D _collider;
        [SerializeField, HideInInspector] private Animator _animator;
        
        private int _damage;
        private float _speed;
        private float _lifeTimeCounter;

        private bool _isHit;
        private bool _isEnemyBullet;
        
        private Vector2 _direction;
        
        public bool IsEnemyBullet => _isEnemyBullet;
        
        public void Init(Vector2 direction, BulletConfig bulletConfig)
        {
            gameObject.layer = bulletConfig.IsEnemyBullet
                ? LayerMask.NameToLayer("EnemyProjectile")
                : LayerMask.NameToLayer("PlayerProjectile");
            
            if (bulletConfig.AnimatorController.Enabled)            
                _animator.runtimeAnimatorController = bulletConfig.AnimatorController.Value;          
            else
            {
                _animator.runtimeAnimatorController = null;
                _render.sprite = bulletConfig.Sprite;
            }

            _render.material = bulletConfig.CustomMaterial.Enabled
                ? bulletConfig.CustomMaterial.Value
                : bulletConfig.DefaultMaterial;

            _isEnemyBullet = bulletConfig.IsEnemyBullet;
            _collider.enabled = true;
            _collider.radius = 0.3f;
            _direction = direction;
            _speed = bulletConfig.Speed;
            _damage = bulletConfig.Damage;
            transform.localScale = new Vector3(bulletConfig.Scale, bulletConfig.Scale, 1f);
            _lifeTimeCounter = _lifeTime;
        }

        public void Destroy()
        {
            StartCoroutine(Hit(new ContactPoint2D(), _hitTargetAnimator));
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
            if (_isHit)
                return;

            _rigidbody.velocity = _speed * _direction;

            _lifeTimeCounter -= Time.fixedDeltaTime;

            if (_lifeTimeCounter < 0)
                BulletPoolingManager.Instance.Release(this);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                StartCoroutine(Hit(other.contacts[0], _hitTargetAnimator));
                damageable.TryTakeDamage(_damage, other.contacts[0], 100f);
            }
            else
                StartCoroutine(Hit(other.contacts[0], _hitWallAnimator));
        }

        private IEnumerator Hit(ContactPoint2D hitPoint, RuntimeAnimatorController animatorController)
        {
            _isHit = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0;
            _collider.enabled = false;
            
            if (Mathf.Abs(hitPoint.normal.x) > 0.5)
                transform.eulerAngles = new Vector3(0, 0, 90 * hitPoint.normal.x);
            else
            {
                float angle = hitPoint.normal.y > 0 ? 180 : 0;
                transform.eulerAngles = new Vector3(0, 0, angle);
            }

            transform.localScale = _isEnemyBullet ? Vector3.one * 0.5f : transform.localScale;
            _animator.runtimeAnimatorController = animatorController;
            
            yield return new WaitForSeconds(1f);
            _isHit = false;
            BulletPoolingManager.Instance.Release(this);
        }
    }
}
