using UnityEngine;

public class EnemyShooting : EnemyMovement
{
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private float _shootDistance = 10f;

    private const string _move = "Move";

    public override void Update() 
    {
        Rotation();
        if (EnemyAttack.Attacking)
            Move();
    }

    private void FixedUpdate() => CheckObstale();

    private void CheckObstale()
    {
        if (!Physics2D.Linecast(transform.position, Player.position, (1 << 6)))
        {
            Attack?.Invoke();
            Agent.isStopped = true;
            Animator.SetBool(_move, false);
        }
        else if(!EnemyAttack.Attacking)
        {
            Agent.isStopped = false;
            Animator.SetBool(_move, true);
        }
    }
}
