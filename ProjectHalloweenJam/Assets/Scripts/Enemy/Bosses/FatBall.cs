using Core;
using Core.Interfaces;
using Entities;
using System;
using System.Collections;
using UnityEngine;

public class FatBall : MonoBehaviour
{
    [SerializeField] private Transform _shadowTransform;
    [SerializeField] private RuntimeAnimatorController _hitAnimator;
    [Space]
    [SerializeField] private int _damage = 15;
    [SerializeField] private float _offsetY;
    [Space]
    [SerializeField] private Material _material;

    private Animator _animator;
    private Transform _playerTransform;
    private WeaponSelector _weaponSelector;

    private static readonly int GlowColor = Shader.PropertyToID("_GlowColor");

    [Range(0, 5)]
    [SerializeField] private float _movementTime;
    [Range(0, 5)]
    [SerializeField] private float _shadowViewTime;

    public void Initialize(Transform playerTransform, Transform spawnTransform)
    {
        _animator = GetComponent<Animator>();
        _weaponSelector = GetComponent<WeaponSelector>();
        _weaponSelector.SetWeaponByIndex(0);

        _playerTransform = playerTransform;
        transform.position = spawnTransform.position;

        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        Vector3 destinationPosition = transform.position;
        Vector3 shadowPosition = Vector3.one;
        Vector3 startPosition = transform.position;
        destinationPosition.y += _offsetY;

        float elapsedTime = 0f;
        while (elapsedTime < _movementTime)
        {
            transform.position = Vector3.Lerp(startPosition, destinationPosition, elapsedTime/ _movementTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _shadowTransform.Activate();

        elapsedTime = 0f;
        while (elapsedTime < _shadowViewTime)
        {
            _shadowTransform.position = Vector3.Lerp(transform.position, _playerTransform.position, elapsedTime / _shadowViewTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        destinationPosition.x = _playerTransform.position.x;
        transform.position = destinationPosition;
        destinationPosition.y = _playerTransform.position.y;
        shadowPosition = destinationPosition;
        startPosition = transform.position;

        elapsedTime = 0f;
        while (elapsedTime < _movementTime)
        {
            _shadowTransform.localScale = Vector3.Lerp(_shadowTransform.localScale, Vector3.one, (elapsedTime * 5f) / _movementTime);
            transform.position = Vector3.Lerp(startPosition, destinationPosition, elapsedTime / _movementTime);
            _shadowTransform.position = shadowPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _shadowTransform.position = shadowPosition;
        _shadowTransform.Deactivate();
        _animator.runtimeAnimatorController = _hitAnimator;
        _weaponSelector.TryToAttack(_playerTransform.position, false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void SetToxicBall()
    {
        Color hdrColor = new Color(0f,8f,0f,1f);   
        _material.SetColor(GlowColor, hdrColor);
    }

    public void SetFireBallMaterial()
    {
        Color hdrColor = new Color(20f, 0f, 0f,1f); 
        _material.SetColor(GlowColor, hdrColor); 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent<IDamageable>(out var damageable))            
            damageable.TryTakeDamage(_damage);       
    }
}
