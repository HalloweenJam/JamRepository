using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _onlyFlipRotation = false;
    private Transform _playerTransform;
    private NavMeshAgent _agent;
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
