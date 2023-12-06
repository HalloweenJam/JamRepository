using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Arena
{
    public class Arena : MonoBehaviour
    {
        [SerializeField] private List<Gate> _gates;
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
            foreach (Gate gate in _gates)
                gate.gameObject.SetActive(true);
        }

        private void ArenaCompleted()
        {
            foreach (Gate gate in _gates)
                gate.Disable();
        }

        private void OnDisable() 
        {
            SpawnerEnemy.OnAllEnemiesDied -= ArenaCompleted;
            _spawnZone.OnActivateArena -= ActivateArena;
        }
    }
}
