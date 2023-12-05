using System.Collections.Generic;
using Enemy.EnemyEntity;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private List<EnemyStats> _enemyPrefabs;
    private bool _isEmpty;

    public List<EnemyStats> EnemyPrefabs => _enemyPrefabs;
    public bool IsEmpty => _isEmpty;

    private void Start() => CheckList();

    public EnemyStats SpawnEnemy(Transform parent)
    {
        EnemyStats enemy = Instantiate(_enemyPrefabs[_enemyPrefabs.Count - 1]);
       
        enemy.transform.position = transform.position;
        enemy.transform.SetParent(parent);

        RemoveEnemyFromList();
        return enemy;
    }

    private void RemoveEnemyFromList() 
    {
        _enemyPrefabs.RemoveAt(_enemyPrefabs.Count - 1);
        CheckList();
    } 

    private void CheckList() => _isEmpty = _enemyPrefabs.Count == 0 ? true : false;

}
