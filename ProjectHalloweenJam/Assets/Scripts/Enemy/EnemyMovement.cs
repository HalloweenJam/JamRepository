using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private Vector3 _defaultScale;

    public void Initialize(Transform playerTransform)
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerTransform = playerTransform;

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _defaultScale = transform.localScale;
    }

    private void Update()
    {
        if (_playerTransform == null)
            return;

        Move();
        Rotation();
    }

    private void Move() => _agent.SetDestination(_playerTransform.position);

    private void Rotation() 
    {
        var scale = _defaultScale;
        scale.x = transform.position.x < _playerTransform.position.x ? scale.x : -1f * scale.x;
        transform.localScale = scale;
    }
}
