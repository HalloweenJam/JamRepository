using Enemy.EnemyEntity;
using System;
using UnityEngine;

namespace Enemy.Arena
{
    public class Arena : MonoBehaviour
    {
        [SerializeField] private Transform _walls;
        [SerializeField] private SpawnZone _spawnZone;
        [SerializeField] private SpawnerEnemy _spawner;

        private void Awake()
        {
            SpawnerEnemy.OnAllEnemiesDied += ArenaCompleted;
            _spawnZone.OnActivateArena += ActivateArena;
        }

        public void ActivateArena()
        {
            _spawner.ActivateArena();
            foreach (Transform wall in _walls)
                wall.gameObject.SetActive(true);
        }

        private void ArenaCompleted()
        {
            foreach (Transform wall in _walls)
                wall.gameObject.SetActive(false);
        }

        private void OnDisable() 
        {
            SpawnerEnemy.OnAllEnemiesDied -= ArenaCompleted;
            _spawnZone.OnActivateArena -= ActivateArena;
        }
    }
}
