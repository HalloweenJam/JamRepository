using UnityEngine;
using Enemy.EnemyEntity;

public class GluttonyMovement : EnemyMovement
{
    [Range(0, 100f)]
    [SerializeField] private float _distanceForAttack;
    private bool _playerInAttackZone = false;

    public override void Initialize(Transform playerTransform, EnemyStats enemyStats)
    {
        base.Initialize(playerTransform, enemyStats);
        Agent.enabled = false;
        this.enabled = false;
    }

    public override void Move() => CheckDistance();

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, PlayerTransform.position);
        _playerInAttackZone = distance < _distanceForAttack;
        if (_playerInAttackZone)
            CanAttack();
    }
}