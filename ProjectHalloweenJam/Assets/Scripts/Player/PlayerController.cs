using System;
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

        [Header("Dash")] 
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
        
        private Camera _camera;

        private bool _isDashing;
        private float _dashDelayCounter;
        private int _dashesCount;
        private bool _movementCancelled = true;
        
        private Vector2 _movementDirection;
        private InputReader _inputReader;

        public bool IsDashing => _isDashing;

        public static Action<bool> OnPlayerDashing;
        
        private bool _canDash => !_isDashing && _dashesCount > 0;
        
        private void OnValidate()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
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
            _inputReader = InputReaderManager.Instance.GetInputReader();
            
            _inputReader.MoveEvent += direction =>
            {
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
            
            _rigidbody.AddForce(new Vector2(_movementDirection.x, _movementDirection.y) * (_speed * _dashForce));
            
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
            _spriteRenderer.flipX = mousePosX < _rigidbody.position.x;
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