using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int _health;
    public int _currentHealth;

    [Header("Damage")]
    [SerializeField] private int _damage;

    public static Action<Vector2> OnDeath;

    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _currentHealth = _health;
        _defaultColor = _spriteRenderer.color;
    }

    public bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
    {
        if (instantKill)
        {
            Dead();
            StartCoroutine(PaintingSprite());
            return true;
        }

        _currentHealth -= damage;
        StartCoroutine(PaintingSprite());

        if (_currentHealth <= 0)
            Dead();

        return true;
    }

    private void Dead() 
    {
        _currentHealth = 0;
        OnDeath?.Invoke(transform.position);
        Destroy(gameObject);
    }

    private IEnumerator PaintingSprite()
    {
        float ignoreTime = 1f;
        float elapsedTime = 0f;
        _spriteRenderer.color = Color.red;
        while (elapsedTime < ignoreTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }       
        _spriteRenderer.color = _defaultColor;
    }
}
