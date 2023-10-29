using Enemy;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private SpawnerEnemy _spawnerEnemy;
    [SerializeField] private Transform _walls;
    private int _countEnemy;

    private void Awake() => EnemyStats.OnDeath += (pos) => CheckEnemyCount(); 
  
    private void CheckEnemyCount()
    {
        _countEnemy--;
        _spawnerEnemy.CheckEnemyIsEmpty();

        if (_countEnemy == 0 && _spawnerEnemy.CanSpawn)
        {
            _spawnerEnemy.SpawnEnemy();
            _countEnemy = _spawnerEnemy.transform.childCount - 1;
        }
        else if(_countEnemy == 0 && !_spawnerEnemy.CanSpawn)
            ArenaCompleted();
    }

    public void ActivateArena()
    {
        _spawnerEnemy.SpawnEnemy();
        _countEnemy = _spawnerEnemy.transform.childCount;

        foreach (Transform wall in _walls)       
            wall.gameObject.SetActive(true);    
    }

    private void ArenaCompleted()
    {
        foreach (Transform wall in _walls)
            wall.gameObject.SetActive(false);
    }
}
