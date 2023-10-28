using UnityEngine;
using System.Collections.Generic;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    private bool _canSpawn = true;

    public bool CanSpawn => _canSpawn;

    public void SpawnEnemy(Arena arena)
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i].EnemyPrefabs == null || _spawnPoints[i].CountEnemy == 0)
                return;

            EnemyMovement enemy = _spawnPoints[i].SpawnEnemy(this.transform);
            enemy.Initialize(_playerTransform);
        }
    }
}
