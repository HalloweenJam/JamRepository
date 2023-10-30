using System.Collections;
using Core;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Gameplay.Traps
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class SpikeTrap : MonoBehaviour
    {
        [SerializeField] private TrapActivationType _activationType;
        [SerializeField] private LayerMask _entitiesLayerMask;
        [SerializeField] private float _activationTime = 2f;
        [Space]
        [SerializeField] private int _damage = 1;

        [SerializeField, HideInInspector] private BoxCollider2D _collider;
        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;

        private bool _isEnabled = false;
        private bool _isChangingState = false;
        
        private void OnValidate()
        {
            _collider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            ChangeColors();
            _collider.isTrigger = true;
        }

        /// <summary>
        /// can be deleted in later
        /// </summary>
        private void ChangeColors()
        {
            _spriteRenderer.color = _isEnabled ? Color.red : Color.green;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_isEnabled || _isChangingState) 
                return;
            
            _isChangingState = true;
            StartCoroutine(ChangeState());
        }

        private IEnumerator ChangeState()
        {
            yield return new WaitForSeconds(_activationTime);
            _isEnabled = !_isEnabled;
            
            ChangeColors();

            if (_isEnabled)
            {
                var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0, _entitiesLayerMask);

                foreach (var target in colliders)
                {
                    if (target.TryGetComponent<IDamageable>(out var damageable))
                        damageable.TryTakeDamage(_damage);
                }
                
                yield return StartCoroutine(Disable());
            }

            _isChangingState = false;
        }

        private IEnumerator Disable()
        {
            yield return new WaitForSeconds(_activationTime);
            _isEnabled = false;
            
            ChangeColors();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            
            Gizmos.DrawCube(transform.position, new Vector3(1, 1));
        }
    }
}