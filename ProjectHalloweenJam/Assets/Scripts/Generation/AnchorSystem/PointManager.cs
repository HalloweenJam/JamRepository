using System.Collections.Generic;
using Generation.AnchorSystem.Data;
using Managers;
using UnityEngine;
using Utilities;

namespace Generation.AnchorSystem
{
    public class PointManager : Singleton<PointManager>
    {
        private SpawnPointsData _spawnPointsData;
        private readonly List<int> _usedIndexes = new();
        private readonly List<Vector2> _additionalSpawnPositions = new();        
        public int PointsCount => _spawnPositions.Count;
        private IReadOnlyList<Vector2> _spawnPositions => _spawnPointsData.Positions;
        private List<Vector2> _totalSpawnPositions => _additionalSpawnPositions.Join<Vector2, IReadOnlyList<Vector2>>(_spawnPointsData.Positions);
       

        public Vector2 GetPointPosition(SpawnPointsData spawnPoint)
        {
            _spawnPointsData = spawnPoint;
            var id = _totalSpawnPositions.GetRandomId();
            _usedIndexes.Add(id);
            
            return _totalSpawnPositions[id];
        }

        public void AddNewPositions(int count)
        {
            count = Mathf.Abs(count);
            
            PositionsGenerator.Instance.GenerateSpawnPositions(count, _additionalSpawnPositions);
        }
    }
}