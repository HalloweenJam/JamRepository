using System.Collections.Generic;
using Generation.AnchorSystem.Data;
using UnityEngine;
using Utilities;

namespace Generation.AnchorSystem
{
    public class PositionsGenerator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color _anchorColor = Color.yellow;
        [SerializeField] private Color _spawnPointColor = Color.magenta;
        [SerializeField] private Color _areaColor = Color.green;
        [SerializeField] private int _pointsCount = 100;
        [SerializeField] private bool _useSnap;

        [Header("Raycast")]
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _minDistanceToWall = 1.5f;
        
        [SerializeField, HideInInspector] private List<Vector2> _anchors = new();
        [SerializeField, HideInInspector] private List<Vector2> _spawnPositions = new();
        [SerializeField, HideInInspector] private List<Vector2> _borderCapsPositions = new()
        {
            new Vector2(0, 10), new Vector2(0, -10), new Vector2(10, 0), new Vector2(-10, 0)
        };
        
        [Header("Gizmos")]
        [SerializeField] private bool _showAnchorGizmos;
        [Space(12)]
        [SerializeField] private SpawnPointsData _spawnPointsData;
        
        public static PositionsGenerator Instance;
        
        public SpawnPointsData SpawnPointsData => _spawnPointsData;
        
        public bool UseSnap => _useSnap;
        public int PointsCount => _pointsCount;

        public Color AreaColor => _areaColor;
        public Color AnchorColor => _anchorColor;
        public Color SpawnPointColor => _spawnPointColor;

        public List<Vector2> BorderCapsPositions => _borderCapsPositions; 
        
        public List<Vector2> AllPositionsList => _spawnPositions.Join<Vector2, List<Vector2>>(_anchors);
        public List<Vector2> Anchors => _anchors;
        public List<Vector2> SpawnPositions => _spawnPositions;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void GenerateSpawnPositions(int count, List<Vector2> listToFill = null)
        {
            if (_anchors.Count < 1)
                return;
            
            var neededPoints = 0;
            var fillAdditionalList = listToFill != null;

            while (neededPoints < count)
            {
                var pos = new Vector2(Random.Range(_borderCapsPositions[2].x, _borderCapsPositions[3].x), Random.Range(_borderCapsPositions[1].y, _borderCapsPositions[0].y));

                if (IsHasColliderInRange(pos, _minDistanceToWall, _layerMask))
                    continue;

                foreach (var anchor in _anchors)
                {
                    var linecastHit = Physics2D.Linecast(anchor, pos, _layerMask);

                    if (linecastHit)
                        continue;

                    neededPoints++;

                    if (fillAdditionalList)
                    {
                        listToFill.Add(pos);
                    }
                    else
                    {
                        _spawnPositions.Add(pos);
                    }
                    
                    break;
                }
            }
        }

        private static bool IsHasColliderInRange(Vector2 pos, float range, LayerMask layerMask)
        {
            var hasCollider = Physics2D.OverlapCircle(pos, range, layerMask);
            return hasCollider;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_showAnchorGizmos || _anchors.Count < 2)
                return;

            Gizmos.color = _anchorColor;
            
            for (var i = 1; i < _anchors.Count; i++)
            {
                Gizmos.DrawLine(_anchors[i], _anchors[i - 1]);
            }
        }

        public void DeletePoint(int pointId)
        {
            if (pointId < _spawnPositions.Count)
            {
                _spawnPositions.RemoveAt(pointId);
                return;
            }

            pointId -= _spawnPositions.Count;
            _anchors.RemoveAt(pointId);
        }
    }
}