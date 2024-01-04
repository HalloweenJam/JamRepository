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

        [Header("FogOfWar")]
        [SerializeField] private Transform _centerRoom;
        [SerializeField] private float _radius;

        private bool _isCompleted = false;

        private SpawnPointsData _ownSpawnPointsData;
        public bool IsBossArena => _isBossArena;
        public bool IsCompleted => _isCompleted;

        private void Awake() => _ownSpawnPointsData = _spawnPointsData;

        public void Activate()
        {
            SpawnerEnemy.Instance.ActivateArena(this, _level, _ownSpawnPointsData);
            FogOfWar.Instance.MakeHole(_centerRoom.position, _radius); 
            foreach (Gate gate in _gates)
                gate.gameObject.SetActive(true);
        }

        public void Completed()
        {
            _isCompleted = true;
            foreach (Gate gate in _gates)
            {
                if(gate.gameObject.activeInHierarchy)
                    gate.Disable();
            }
        }
    }
}
