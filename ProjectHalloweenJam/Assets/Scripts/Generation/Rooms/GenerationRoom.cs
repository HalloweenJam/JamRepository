using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generation.Rooms
{
    public class GenerationRoom : MonoBehaviour
    {
        [SerializeField] private bool _isStartRoom;
        [SerializeField] private Transform _roomCenter;
        [SerializeField] private Vector2 _overlapSize;
        [SerializeField] private int _maxPassagesCount;
        [SerializeField] private List<Door> _doors;
        [SerializeField] private Tilemap _wallTilemap;
        [Header("Editor")] 
        [SerializeField] private bool _useSnap;
        [SerializeField] private float _snapValue = .25f;

        private Vector2 _previousPosition;
        private Dictionary<Door.Dir, Door> _doorsDictionary;
        private Dictionary<Door.Dir, HashSet<GenerationRoom>> _adjoiningRooms = new();

        // idk but without attributes data will not saved
        [SerializeField, HideInInspector] private List<Vector2> _borderCapsPositions = new()
        {
            new Vector2(0, 10), new Vector2(0, -10), new Vector2(10, 0), new Vector2(-10, 0)
        };
        
        public bool UseSnap => _useSnap;
        public List<Vector2> BorderCapsPositions => _borderCapsPositions;
        public Dictionary<Door.Dir, Door>  Doors => _doorsDictionary;
        public Dictionary<Door.Dir, HashSet<GenerationRoom>> AdjoiningRooms => _adjoiningRooms;
        public Vector2 RoomCenter => _roomCenter.position;
        public Vector2 OverlapCenter { get; set; }
        public Vector2 OverlapSize => _overlapSize;
        public Tilemap WallTilemap => _wallTilemap;

        public bool IsStartRoom => _isStartRoom;
        public int MaxPassagesCount => _maxPassagesCount;
        public int MoveIterations { get; private set; } = 0;
        public float SnapValue => _snapValue;

        private void Awake()
        {
            _doorsDictionary = new Dictionary<Door.Dir, Door>(4);
            foreach (Door door in _doors)
                _doorsDictionary.Add(door.Direction, door);           
        }

        public void SetPosition(Vector2Int position)
        {
            var roomTransform = transform;
            roomTransform.position = position - (Vector2) (_roomCenter.position - roomTransform.position);
        }

        public Vector2 GetNextPosition(int speed)
        {
            var direction = (Vector2.zero - RoomCenter).normalized * speed;
            var roomTransform = RoomCenter;
            var roundedPosition = new Vector2(direction.x + roomTransform.x, direction.y + roomTransform.y);

            MoveIterations = _previousPosition == roundedPosition ? MoveIterations++ : 0;

            _previousPosition = roundedPosition;
            return roundedPosition;
        }

        public void MoveTo(Vector2 position)
        {
            var pos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            SetPosition(pos);
        }

        public void UpdateBorders()
        {
            var min = new Vector2(float.MaxValue, float.MaxValue);
            var max = new Vector2(float.MinValue, float.MinValue);

            foreach (var center in _borderCapsPositions)
            {
                min = Vector2.Min(min, center);
                max = Vector2.Max(max, center);
            }

            _overlapSize = max - min;
        }
    }
}