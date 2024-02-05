using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using Player.Controls;
using UnityEngine;
using Visuals;

namespace Player
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Rigidbody")] 
        [SerializeField] private float _linearDrag = 15f;
        [SerializeField] private float _mass = 1f;

        [Header("Movement")] 
        [SerializeField] private float _speed = 12;
        [SerializeField] private WeaponHolder _weaponHolder;

        [Header("Teleport")] 
        [SerializeField] private float _teleportTime = .5f;

        [Header("Dash")]
        [SerializeField] private BoxCollider2D _hurtCollider;
        [Space]
        [SerializeField] private float _dashForce = 20;
        [Space]
        [SerializeField] private float _dashDelay = 3f;
        [SerializeField] private int _maxDashesCount = 1;

        [Header("Animator")] 
        [SerializeField] private string _idle;
        [SerializeField] private string _run;

        [Header("Projectile Cleaner")] 
        [SerializeField] private LayerMask _cleanerLayerMask;
        [SerializeField] private int _clearRefills = 1000;
        [SerializeField] private float _clearDelay = 2f;
        [SerializeField] private float _maxRadius = 30f;
        [SerializeField] private float _expansionTime = 1;
        
        [HideInInspector, SerializeField] private Rigidbody2D _rigidbody;
        [HideInInspector, SerializeField] private Animator _animator;
        [HideInInspector, SerializeField] private SpriteRenderer _spriteRenderer;
        [HideInInspector, SerializeField] private WeaponSelector _weaponSelector;
        [HideInInspector, SerializeField] private DissolveEffect _dissolveEffect;
        [HideInInspector, SerializeField] private AfterImageController _afterImageController;
        [HideInInspector, SerializeField] private ShockWaveController _shockWaveController;
        
        private Camera _camera;

        private bool _isEnabled = true;
        private bool _isDashing;
        private bool _isMovementCancelled = true;

        private float _cleanerRadius = 0;
        private float _clearDelayCounter;
        private float _dashDelayCounter;
        private int _dashesCount;

        private Vector2 _cashedMovementDirection = Vector2.right;
        private Vector2 _movementDirection;

        private InputReader _inputReader;

        public static Action<float> OnDashRefill;
        public Action<bool> OnPlayerDashing;
        public static Action<Vector2> TeleportPlayer;
        private float _dashRefillRatio => _dashDelayCounter / _dashDelay;
        
        private bool _canDash => !_isDashing && _dashesCount > 0;

        public void DisableHurtCollider(float time)
        {
            _hurtCollider.enabled = false;
            StartCoroutine(EnableHurtCollider(time));
        }

        public void AddClearRefills(int value)
        {
            _clearRefills += value;
        }

        public void Init()
        {
            TeleportPlayer = Teleport;
            
            _camera = Camera.main;

            _rigidbody.mass = _mass;
            _rigidbody.drag = _linearDrag;
            _rigidbody.gravityScale = 0;
            _rigidbody.angularDrag = 0;

            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            
            _weaponSelector.Init(true);
            
            _inputReader = InputReaderManager.Instance.GetInputReader();
            
            InputReaderManager.Instance.OnInputReaderActiveStateChanged += (state) => _isEnabled = state;
            InputReaderManager.Instance.OnInstanceDestroyed += OnDisable;
            
            _inputReader.MoveEvent += direction =>
            {
                if (direction != Vector2.zero)
                    _cashedMovementDirection = direction;
                
                _movementDirection = direction;
                _isMovementCancelled = false;
            };
            _inputReader.ClearProjectilesAction += ClearProjectiles;
            _inputReader.MoveCancelledEvent += () => _isMovementCancelled = true;

            _inputReader.DashEvent += TryDash;
        }
        
        private void OnValidate()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _dissolveEffect = GetComponent<DissolveEffect>();
            _weaponSelector = GetComponent<WeaponSelector>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _afterImageController = GetComponent<AfterImageController>();
            _shockWaveController = GetComponent<ShockWaveController>();
        }

        private void Teleport(Vector2 pos)
        {
            StartCoroutine(MoveToCoroutine(pos));
        }

        private IEnumerator MoveToCoroutine(Vector2 pos)
        {
            EnableController(false);
            _dissolveEffect.Dissolve(this);
            _weaponSelector.WeaponHolder.Enable(false);
            
            yield return new WaitForSeconds(_teleportTime);

            transform.position = pos;
            
            yield return new WaitForSeconds(_teleportTime);
            
            EnableController(true);
            _dissolveEffect.Appearance(this);
            _weaponSelector.WeaponHolder.Enable(true);
        }

        private void EnableController(bool enable)
        {
            if (!enable)
            {
                _inputReader.Disable();
            }
            else
            {
                _inputReader.SetPlayerActions();
            }
        }

        private void ClearProjectiles()
        {
            if (_clearRefills < 1 || _clearDelayCounter > 0)
                return;

            _clearRefills--;
            _clearDelayCounter = _clearDelay;
            
            _shockWaveController.Play();
            StartCoroutine(DetectProjectiles());
        }

        private IEnumerator DetectProjectiles()
        {
            yield return new WaitForFixedUpdate();
            
            var elapsedTime = 0f;
            
            while (elapsedTime < _expansionTime)
            {
                _cleanerRadius = Mathf.Lerp(0, _maxRadius, elapsedTime / _expansionTime);
                var colliders = Physics2D.OverlapCircleAll(transform.position, _cleanerRadius, _cleanerLayerMask);
                
                DestroyProjectiles(colliders);
                
                elapsedTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            _cleanerRadius = _maxRadius;
        }

        private static void DestroyProjectiles(IEnumerable<Collider2D> colliders)
        {
            foreach (var item in colliders)
            {
                if (item.TryGetComponent<Projectiles.Bullet>(out var bullet) && bullet.IsEnemyBullet)
                    bullet.Destroy();
            }
        }
        
        private void Update()
        {
            _clearDelayCounter -= Time.deltaTime;
            
            if (_dashesCount >= _maxDashesCount)
                return;

            _dashDelayCounter -= Time.deltaTime;

            OnDashRefill?.Invoke(_dashRefillRatio);
             
            if (_dashDelayCounter > 0)
                return;

            _dashDelayCounter = _dashDelay;
            _dashesCount++;
        }

        private void FixedUpdate()
        {
            if (!_isEnabled)
                return;

            Move();
            Rotate();
        }

        private void TryDash()
        {
            if (!_canDash)
                return;
            
            _isDashing = true;
            OnPlayerDashing?.Invoke(_isDashing);
            _dashesCount--;
            
            _afterImageController.Play();
            _rigidbody.AddForce(new Vector2(_cashedMovementDirection.x, _cashedMovementDirection.y) * (_speed * _dashForce));
            
            _isDashing = false;
            OnPlayerDashing?.Invoke(_isDashing);
        }
        
        private void Rotate()
        {
            var mousePosX = _camera.ScreenToWorldPoint(Input.mousePosition).x;
            var localScale = transform.localScale;

            var isFacingRight = mousePosX < _rigidbody.position.x;
            
            localScale.x = isFacingRight ? -1 : 1;
            transform.localScale = localScale;
            
            _weaponHolder.Flip(isFacingRight);
        }

        private void Move()
        {
            if (_isMovementCancelled)
            {
                _animator.Play(_idle);
                return;
            }

            _animator.Play(_run);  

            _rigidbody.AddForce(new Vector2(_movementDirection.x, _movementDirection.y) * (_speed * 50 * Time.fixedDeltaTime));
        }

        private IEnumerator EnableHurtCollider(float time)
        {
            yield return new WaitForSeconds(time);
            _hurtCollider.enabled = true;
        }

        private void OnDisable()
        {
            _inputReader.DashEvent -= TryDash;
            _inputReader.ClearProjectilesAction -= ClearProjectiles;
            
            if (InputReaderManager.Instance == null)
                return;

            InputReaderManager.Instance.OnInputReaderActiveStateChanged -= (state) => _isEnabled = state;
            InputReaderManager.Instance.OnInstanceDestroyed -= OnDisable;
        }
    }
}