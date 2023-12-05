using Enemy.EnemyEntity;
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    protected EnemyStats EnemyStats;
    protected EnemyAttack EnemyAttack;
    protected NavMeshAgent Agent;
    protected Transform PlayerTransform;

    public Action Attack;

    public Transform Player => PlayerTransform;
    public NavMeshAgent NavAgent => Agent;

    private Vector3 _defaultScale;

    public virtual void Initialize(Transform playerTransform, EnemyStats enemyStats)
    {
        Agent = GetComponent<NavMeshAgent>();
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemyStats = enemyStats;
        PlayerTransform = playerTransform;

        Agent.enabled = true;
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        _defaultScale = transform.localScale;
    }

    public virtual void Update()
    {
        if (PlayerTransform == null)
            return;

        Move();
        Rotation();
    }

    public virtual void Move() => Agent.SetDestination(PlayerTransform.position);      

    public virtual void CanAttack() => Attack?.Invoke();

    protected void Rotation() 
    {
        var scale = _defaultScale;
        scale.x = transform.position.x < PlayerTransform.position.x ? scale.x : -1f * scale.x;
        transform.localScale = scale;
    }
}
