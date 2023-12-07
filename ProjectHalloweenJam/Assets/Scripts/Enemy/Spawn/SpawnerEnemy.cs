using UnityEngine;
using System.Collections.Generic;
using Enemy.EnemyEntity;
using System;
using Managers;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoinsTransfrom;
    [SerializeField] private bool _spawnOnStart = false;

    private List<SpawnPoint> _spawnPoints;

    private int _countEnemy;
    private bool _canSpawn = false;

    public static Action OnAllEnemiesDied;

    private void Awake()
    {
        EnemyStats.OnDeath += (_, _) => CheckEnemyCount();
        EnemyBoss.SecondPhase += SpawnEnemy;

        _spawnPoints = new List<SpawnPoint>(_spawnPoinsTransfrom.childCount);
        foreach (Transform points in _spawnPoinsTransfrom)     
            _spawnPoints.Add(points.GetComponent<SpawnPoint>());
    }

    public void ActivateArena()
    {
        if (_spawnOnStart)
        {
            SpawnEnemy();
            _countEnemy = transform.childCount;
        }
    }
    private void SpawnEnemy()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i].EnemyPrefabs != null && !_spawnPoints[i].IsEmpty)
            {
                EnemyStats enemy = _spawnPoints[i].SpawnEnemy(this.transform);
                enemy.Initialize(DungeonGenerator.Instance.PlayerTransform);
            }
        }
    }

    private void CheckEnemyIsEmpty()
    {
        _canSpawn = false;  
        foreach (SpawnPoint point in _spawnPoints)    
            _canSpawn = _canSpawn || !point.IsEmpty;    
    }

    private void CheckEnemyCount()
    {
        _countEnemy--;
        CheckEnemyIsEmpty();

        if (_countEnemy == 0 && _canSpawn)
        {
            SpawnEnemy();
            _countEnemy = transform.childCount - 1;
        }
        else if (_countEnemy == 0 && !_canSpawn)
            OnAllEnemiesDied?.Invoke();
    }
}
