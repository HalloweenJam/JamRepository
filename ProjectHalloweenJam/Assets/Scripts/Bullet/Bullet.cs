using System;
using Core.Interfaces;
using UnityEngine;

namespace Bullet
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private int _damage;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TryTakeDamage(_damage);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            gameObject.SetActive(false);
        }
    }
}
