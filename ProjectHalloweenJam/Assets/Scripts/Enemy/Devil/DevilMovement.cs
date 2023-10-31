using UnityEngine;
using System.Collections;

public class DevilMovement : EnemyMovement
{
    [SerializeField] private Vector3 _offset;
    private bool _isTeleportation = false;
    private bool _playerInAttackZone = false;

    public override void Move()
    {
        CheckDistance();

        if (!_playerInAttackZone && !_isTeleportation)
            StartCoroutine(Teleportation());
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, PlayerTransform.position);
        _playerInAttackZone = distance < 5f;
        if (_playerInAttackZone)
            CanAttack();      
    }

    private IEnumerator Teleportation()
    {
        _isTeleportation = true;
        EnemyStats.Dissolve();

        while (!EnemyStats.Dissolved)
            yield return null;

        transform.position = PlayerTransform.position + _offset;

        EnemyStats.Appearance();
        while (!EnemyStats.Dissolved)
            yield return null;
        _isTeleportation = false;
    }
}
