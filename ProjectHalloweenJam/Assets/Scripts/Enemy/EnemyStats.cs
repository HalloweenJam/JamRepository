using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int _health;
    private int _currentHealth;

    [Header("Damage")]
    [SerializeField] private int _damage;

    public static Action<Vector2> OnDeath;

    private EnemyMovement _movement;
    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;

    public void Spawn(Transform playerPosition)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<EnemyMovement>();

        _currentHealth = _health;
        _defaultColor = _spriteRenderer.color;

        _movement.Initialize(playerPosition);
        _movement.enabled = false;

        StartCoroutine(SpawnCor());
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

    private IEnumerator SpawnCor()
    {
        float fadeMaterial = 0f;
        float spawnTime = 1.75f;
        float elapsedTime = 0f;
        float percent = (1 / spawnTime);
        while (elapsedTime < spawnTime)
        {
            fadeMaterial += percent * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            _spriteRenderer.material.SetFloat("_Fade", fadeMaterial);
            yield return null;
        }
        _movement.enabled = true;
        _spriteRenderer.material.SetFloat("_Fade", 1f);
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
