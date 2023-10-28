using UnityEngine;
using System.Collections.Generic;

public class Arena : MonoBehaviour
{
    [SerializeField] private SpawnerEnemy _spawnerEnemy;
    [SerializeField] private Transform _walls;


    public int _countEnemy;
    private bool _canSpawning = true;
    private bool _enemyEmpty = false;

    private void Awake()
    {
        EnemyStats.OnDeath += (pos) => _countEnemy--;
    }

    public void ActivateArena()
    {
        _spawnerEnemy.SpawnEnemy();
        _countEnemy = _spawnerEnemy.transform.childCount;

        foreach (Transform wall in _walls)       
            wall.gameObject.SetActive(true);    
    }
}
