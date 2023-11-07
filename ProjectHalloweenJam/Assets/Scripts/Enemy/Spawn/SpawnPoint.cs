using System.Collections.Generic;
using Enemy.EnemyEntity;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private List<EnemyStats> _enemyPrefabs;

    public List<EnemyStats> EnemyPrefabs => _enemyPrefabs;
    public int CountEnemy => _enemyPrefabs.Count;

    public EnemyStats SpawnEnemy(Transform parent)
    {
        EnemyStats enemy = Instantiate(_enemyPrefabs[CountEnemy - 1]);
       
        enemy.transform.position = transform.position;
        enemy.transform.SetParent(parent);

        RemoveEnemyFromList();
        return enemy;
    }

    private void RemoveEnemyFromList() => _enemyPrefabs.RemoveAt(CountEnemy - 1);

}
