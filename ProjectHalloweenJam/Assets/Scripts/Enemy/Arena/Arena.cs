using Enemy.EnemyEntity;
using UnityEngine;

namespace Enemy.Arena
{
    public class Arena : MonoBehaviour
    {
        [SerializeField] private SpawnerEnemy _spawnerEnemy;
        [SerializeField] private Transform _walls;
        private int _countEnemy;

        private void Awake() => EnemyStats.OnDeath += (_, _) => CheckEnemyCount(); 
  
        private void CheckEnemyCount()
        {
            if (_spawnerEnemy == null)
                return;

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
            foreach (Transform wall in _walls)
                wall.gameObject.SetActive(true);

            if (_spawnerEnemy == null)
                return;

            _spawnerEnemy.SpawnEnemy();
            _countEnemy = _spawnerEnemy.transform.childCount;
        }

        private void ArenaCompleted()
        {
            foreach (Transform wall in _walls)
                wall.gameObject.SetActive(false);
        }
    }
}
