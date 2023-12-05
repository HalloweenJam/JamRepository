using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooting : EnemyMovement
{
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private float _shootDistance = 10f;
    private bool _isAttacking = false;

    public override void Update()
    {
        Rotation();
        if (!_isAttacking)
            Move();
    }

    private void FixedUpdate() => CheckObstale();

    private void CheckObstale()
    {
        if (!Physics2D.Linecast(transform.position, Player.position, (1 << 6)))
        {
            _isAttacking = true;
            Attack?.Invoke();
        }
        else
            _isAttacking = false;
    }
}
