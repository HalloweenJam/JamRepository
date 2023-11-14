using System;
using System.Collections;
using Entities;
using Managers;
using Player.Controls;
using UnityEngine;

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
        
        [HideInInspector, SerializeField] private Rigidbody2D _rigidbody;
        [HideInInspector, SerializeField] private Animator _animator;
        [HideInInspector, SerializeField] private SpriteRenderer _spriteRenderer;
        [HideInInspector, SerializeField] private WeaponSelector _weaponSelector;
        
        private Camera _camera;

        private bool _isDashing;
        private float _dashDelayCounter;
        private int _dashesCount;
        private bool _movementCancelled = true;

        private Vector2 _cashedMovementDirection = Vector2.right;
        private Vector2 _movementDirection;

        private InputReader _inputReader;

        public bool IsDashing => _isDashing;

        public Action<bool> OnPlayerDashing;
        
        private bool _canDash => !_isDashing && _dashesCount > 0;

        public void DisableHurtCollider(float time)
        {
            _hurtCollider.enabled = false;
            StartCoroutine(EnableHurtCollider(time));
        }

        private IEnumerator EnableHurtCollider(float time)
        {
            yield return new WaitForSeconds(time);
            _hurtCollider.enabled = true;
        }

        private void OnValidate()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _weaponSelector ??= GetComponent<WeaponSelector>();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
            
            _camera = Camera.main;

            _rigidbody.mass = _mass;
            _rigidbody.drag = _linearDrag;
            _rigidbody.gravityScale = 0;
            _rigidbody.angularDrag = 0;

            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void Start()
        {
            _weaponSelector.Init(true);
            
            _inputReader = InputReaderManager.Instance.GetInputReader();
            
            _inputReader.MoveEvent += direction =>
            {
                if (direction != Vector2.zero)
                    _cashedMovementDirection = direction;
                
                _movementDirection = direction;
                _movementCancelled = false;
            };
            
            _inputReader.MoveCancelledEvent += () => _movementCancelled = true;

            _inputReader.DashEvent += () =>
            {
                if (_canDash)
                    Dash();
            };
        }

        private void Dash()
        {
            _isDashing = true;
            OnPlayerDashing?.Invoke(_isDashing);

            _dashesCount--;
            
            _rigidbody.AddForce(new Vector2(_cashedMovementDirection.x, _cashedMovementDirection.y) * (_speed * _dashForce));
            
            _isDashing = false;
            OnPlayerDashing?.Invoke(_isDashing);
        }
        
        private void Update()
        {
            if (_dashesCount >= _maxDashesCount)
                return;
            
            _dashDelayCounter -= Time.deltaTime;

            if (_dashDelayCounter > 0) 
                return;
            
            _dashDelayCounter = _dashDelay;
            _dashesCount++;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
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
            if (_movementCancelled)
            {
                _animator.Play(_idle);
                return;
            }

            _animator.Play(_run);
            
            _rigidbody.AddForce(new Vector2(_movementDirection.x, _movementDirection.y) * _speed);
        }
    }
}