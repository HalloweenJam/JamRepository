using Core;
using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class FatBall : MonoBehaviour
{
    [SerializeField] private float _offsetY;
    [SerializeField] private Transform _shadowTransform;
    [SerializeField] private AnimatorController _hitAnimator;

    private Animator _animator;
    private Transform _playerTransform;

    [Range(0, 5)]
    [SerializeField] private float _movementTime;
    [Range(0, 5)]
    [SerializeField] private float _shadowViewTime;

    public void Initialize(Transform playerTransform, Transform spawnTransform)
    {
        _animator = GetComponent<Animator>();

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
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
