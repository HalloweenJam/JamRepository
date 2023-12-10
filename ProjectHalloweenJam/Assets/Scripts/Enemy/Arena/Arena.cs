using Generation.AnchorSystem.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using Level = LevelSetting.Levels;

namespace Enemy.Arena
{
    public class Arena : MonoBehaviour
    {
        [SerializeField] private Level _level;
        [SerializeField] private SpawnPointsData _spawnPointsData;
        [SerializeField] private SpawnZone _spawnZone;
        [SerializeField] private List<Gate> _gates;
        private SpawnPointsData _ownSpawnPointsData;
             
        private void Awake()
        {
            _spawnZone.OnEntered += Activate;
            _ownSpawnPointsData = _spawnPointsData;
        }

        private void Activate()
        {
            SpawnerEnemy.Instance.ActivateArena(this, _level, _ownSpawnPointsData);
            foreach (Gate gate in _gates)
                gate.gameObject.SetActive(true);
        }

        public void Completed()
        {
            foreach (Gate gate in _gates)
            {
                if(gate.gameObject.activeInHierarchy)
                    gate.Disable();
            }
        }

        private void OnDisable() => _spawnZone.OnEntered -= Activate;
    }
}
