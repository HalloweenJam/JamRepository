using System;
using Core.Interfaces;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SpikeFloor : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("bebra");
            
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                print("damage");
                
                damageable.TryTakeDamage(_damage);
            }
        }
    }
}