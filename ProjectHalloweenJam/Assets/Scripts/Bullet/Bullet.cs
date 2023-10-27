using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TryTakeDamage(_damage);
        }
        if (other.tag == "Wall") 
        {
            transform.position = new Vector3(0, 0, -100);
            this.gameObject.SetActive(false);
        }
        
    }
}
