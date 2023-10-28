using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private List<EnemyMovement> _enemyPrefabs;

    public List<EnemyMovement> EnemyPrefabs => _enemyPrefabs;
    public int CountEnemy => _enemyPrefabs.Count;

    public EnemyMovement SpawnEnemy()
    {
        EnemyMovement enemy = Instantiate(_enemyPrefabs[CountEnemy - 1], this.transform);
        enemy.transform.position = transform.position;

        RemoveEnemyFromList();
        return enemy;
    }

    private void RemoveEnemyFromList() => _enemyPrefabs.RemoveAt(CountEnemy - 1);

}
