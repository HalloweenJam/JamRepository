using Generation.AnchorSystem.Data;
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
        [SerializeField] private bool _isBossArena = false;

        private SpawnPointsData _ownSpawnPointsData;
        public bool IsBossArena => _isBossArena;

        private void Awake() => _ownSpawnPointsData = _spawnPointsData;

        public void Activate()
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
    }
}
