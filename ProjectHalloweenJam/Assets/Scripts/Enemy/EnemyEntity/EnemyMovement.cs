using Enemy.EnemyEntity;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Entities;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    protected NavMeshAgent Agent;
    protected EnemyStats EnemyStats;
    protected EnemyAttack EnemyAttack;
    protected Transform PlayerTransform;
    protected WeaponSelector WeaponSelector;
    
    public Transform Player => PlayerTransform;
    public NavMeshAgent NavAgent => Agent;

    public Animator Animator { get; private set; }

    public Action Attack;

    private Rigidbody2D _rigidbody;
    private Vector3 _defaultScale;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        EnemyAttack = GetComponent<EnemyAttack>();
        WeaponSelector = GetComponentInChildren<WeaponSelector>();
        WeaponSelector.OnKnockBack += OnKnockBack;

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
    }

    private void OnKnockBack(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode2D.Force);
    }

    private void OnDestroy()
    {
        WeaponSelector.OnKnockBack -= OnKnockBack;
    }

    public virtual void Initialize(Transform playerTransform, EnemyStats enemyStats)
    {
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

    public void Repel(ContactPoint2D hitPoint, float hitForce) => StartCoroutine(RepelCoroutine(hitPoint, hitForce));

    private IEnumerator RepelCoroutine(ContactPoint2D hitPoint, float hitForce)
    {
        hitForce = 100f;
        Vector2 direction = (Vector2)transform.position - hitPoint.point;
        Vector2 force = hitForce * direction;
        _rigidbody.AddForce(force);
        yield return new WaitForSeconds(0.25f);
        _rigidbody.velocity = Vector2.zero;
    }

    protected void Rotation() 
    {
        var scale = _defaultScale;
        scale.x = transform.position.x < PlayerTransform.position.x ? scale.x : -1f * scale.x;
        transform.localScale = scale;
    }
}
