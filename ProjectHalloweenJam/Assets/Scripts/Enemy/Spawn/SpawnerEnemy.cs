using UnityEngine;
using Generation.AnchorSystem.Data;
using Generation.AnchorSystem;
using Enemy.EnemyEntity;
using Managers;

public class SpawnerEnemy : Singleton<SpawnerEnemy>
{   
    [SerializeField] private LevelConfiguration _levelConfiguration;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PointManager _pointManager;

    [SerializeField] private int _startPoints = 10;
    [SerializeField] private bool _spawnOnStart = true;

    private Arena _currentArena;
    private SpawnPointsData _currentSpawnData;

    #region Variables
    private int _currentCountEnemy;
    private int _enemyCount;

    private int _currentWaveNumber;
    private bool _canSpawnMiniBoss;

    private int _currentPoints;
    private int _addPoints;
    #endregion

    public static System.Action OnAllEnemiesDied;

    private void Start() 
    {
        EnemyStats.OnDeath += CheckArena;
        EnemyBoss.SecondPhase += SpawnEnemyInBossArena;
        _currentPoints = _startPoints;
    }

    private void CheckArena(Vector2 position, float loot)
    {
        _currentCountEnemy = _currentCountEnemy != 0 ? _currentCountEnemy - 1: 0;

        if (_currentWaveNumber != 0 && _currentCountEnemy == 0)
            SpawnEnemy(_currentArena, _enemyCount, _canSpawnMiniBoss);
        else if (_currentCountEnemy == 0 && !_currentArena.IsBossArena)
            _currentArena.Completed();
    }     

    public void ActivateArena(Arena arena, LevelSetting.Levels level, SpawnPointsData spawnPointsData)
    {
        _currentArena = arena;
        _currentSpawnData = spawnPointsData;

        var (arenaLevel, settings) = _levelConfiguration.GetSettings(level);  
        var (waves, enemyCount, canSpawnMiniBoss) = _levelConfiguration.SetRandomValue(arenaLevel, settings);

        _canSpawnMiniBoss = canSpawnMiniBoss;
        _currentWaveNumber = waves;
        _enemyCount = enemyCount;

        if(_spawnOnStart)
            SpawnEnemy(_currentArena, _enemyCount, _canSpawnMiniBoss);
    }

    private void SpawnEnemy(Arena arena, int enemyInScene, bool canSpawnMiniBoss)
    {
        Vector2 position = _pointManager.GetPointPosition(_currentSpawnData);
        EnemyItem enemyItem = canSpawnMiniBoss ? _enemyData.GetMiniBoss() : _enemyData.GetDefaultEnemy();
       
        if ((_currentPoints - enemyItem.Point < 0) || _currentCountEnemy > enemyInScene)
        {
            _startPoints += _addPoints;
            _currentPoints = _startPoints;
            _currentWaveNumber--; 
            return;
        }
  
        if (canSpawnMiniBoss)
            _canSpawnMiniBoss = false;  

        _currentPoints -= enemyItem.Point;
        _addPoints += enemyItem.Point;
        _currentCountEnemy++;

        EnemyStats enemy = Instantiate(enemyItem?.Prefab, arena?.transform);
        enemy.transform.localPosition = position;
        enemy.Initialize(_playerTransform);

        SpawnEnemy(_currentArena, _enemyCount, _canSpawnMiniBoss);
    }

    private void SpawnEnemyInBossArena() => SpawnEnemy(_currentArena, _enemyCount, _canSpawnMiniBoss);

    private void OnDisable()
    {
        EnemyStats.OnDeath -= CheckArena;
        EnemyBoss.SecondPhase -= SpawnEnemyInBossArena;
    }
}
