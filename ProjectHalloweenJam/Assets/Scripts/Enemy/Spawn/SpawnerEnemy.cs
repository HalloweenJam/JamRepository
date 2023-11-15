using UnityEngine;
using System.Collections.Generic;
using Enemy.EnemyEntity;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _spawnPoinsTransfrom;
    private List<SpawnPoint> _spawnPoints;

    private bool _canSpawn = false;

    public bool CanSpawn => _canSpawn;

    private void Awake()
    {
        _spawnPoints = new List<SpawnPoint>(_spawnPoinsTransfrom.childCount);
        foreach (Transform points in _spawnPoinsTransfrom)     
            _spawnPoints.Add(points.GetComponent<SpawnPoint>());       
    }

    public void CheckEnemyIsEmpty()
    {
        _canSpawn = false;
        foreach (SpawnPoint point in _spawnPoints)      
            _canSpawn = point.CountEnemy > 0 ? _canSpawn || true : _canSpawn || false;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i].EnemyPrefabs != null && _spawnPoints[i].CountEnemy != 0)
            {
                EnemyStats enemy = _spawnPoints[i].SpawnEnemy(this.transform);
                enemy.Initialize(_playerTransform);
            }
        }
    }
}
