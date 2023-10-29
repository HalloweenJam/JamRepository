using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    protected EnemyStats EnemyStats;
    protected EnemyAttack EnemyAttack;
    protected NavMeshAgent Agent;
    protected Transform PlayerTransform;

    private Vector3 _defaultScale;

    public void Initialize(Transform playerTransform, EnemyStats enemyStats)
    {
        Agent = GetComponent<NavMeshAgent>();
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemyStats = enemyStats;
        PlayerTransform = playerTransform;

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        _defaultScale = transform.localScale;
    }

    private void Update()
    {
        if (PlayerTransform == null)
            return;

        Move();
        Rotation();
    }

    public virtual void Move()
    {
        Agent.SetDestination(PlayerTransform.position);
    }

    private void Rotation() 
    {
        var scale = _defaultScale;
        scale.x = transform.position.x < PlayerTransform.position.x ? scale.x : -1f * scale.x;
        transform.localScale = scale;
    }
}
