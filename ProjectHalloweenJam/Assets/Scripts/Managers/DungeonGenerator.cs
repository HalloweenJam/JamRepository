using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Generation.Rooms;
using UnityEngine;
using Utilities.Classes.Delaunay;
using CorridorGeneration;
using static Utilities.Classes.Delaunay.Delaunay;

namespace Managers
{
    public class DungeonGenerator : Singleton<DungeonGenerator>
    {
        [Header("Generation")]
        [SerializeField] private LayerMask _generationLayerMask;
        [SerializeField] private Vector2Int _range = new(40, 40);
        [SerializeField] private int _roomsCount;
        [SerializeField, Range(1, 10)] private int _clusteringOffset = 3;
        [SerializeField, Range(5, 35)] private int _moveIterations = 3;

        [SerializeField] private bool _generate = true;
        
        private List<GenerationRoom> _generatedRooms = new();
        private List<Vector2> _positions = new ();
        private List<Edge> _edges = new();

        private Delaunay _delaunay;
        private bool _isCanShowGizmos = false;
        
        private bool _isClusteringFinishing;
        private bool _isGenerationFinished;
        
        private void Update()
        {
            if (!_generate)
                return;
            
            _generate = false;
            
            Clear();
            GenerateDungeon();
        }

        private async void GenerateDungeon()
        {
            if (!_isGenerationFinished)
                StartCoroutine(GenerateRooms());

            if (!_isGenerationFinished)
                return;

            if (!_isClusteringFinishing)
                StartCoroutine(ClusteringRooms());
            
            if (!_isClusteringFinishing)
                return;
            
            _delaunay = TriangulateDelaunay();
            _edges = _delaunay.Edges;

            _edges = GetMinimumSpanningTree(_edges, _edges[0].U);

            foreach (var edge in _edges)
            {
                CorridorGenerator.Instance.ReadValue(edge, _generatedRooms);
                await Awaitable.WaitForSecondsAsync(0.2f);
            }
            CorridorGenerator.Instance.DeleteDisabledCorridors();
            Bootstrap.Instance.EnableComponents(_range.x);
        }

        private IEnumerator GenerateRooms()
        {
            var counter = 1000;
            var isStartRoom = true;
            var hasContact = false;
            
            for (int rooms = 0; rooms < _roomsCount;)
            {
                counter--;
                rooms++;
                
                if (counter < 0)
                    break;

                var position = isStartRoom ? Vector2Int.zero : new Vector2Int(Random.Range(-_range.x, _range.x), Random.Range(-_range.y, _range.y));

                var room = AvailableRoomsManager.Instance.GetRoom(hasContact); 
                hasContact = HasContact(position, room);
                
                if (hasContact)
                {
                    rooms--;
                    continue;
                }
                
                var instantiate = Instantiate(room);
                instantiate.SetPosition(position);
                _generatedRooms.Add(instantiate);

                isStartRoom = false;
                
                yield return new WaitForFixedUpdate();
            }
            
            _isGenerationFinished = true;
            _isCanShowGizmos = true;
            GenerateDungeon();
        }

        private bool HasContact(Vector2 position, GenerationRoom room)
        {
            var hasContact = (bool) Physics2D.OverlapBox(position, room.OverlapSize, _generationLayerMask);
            return hasContact;
        }

        private IEnumerator ClusteringRooms()
        {
            print("clustering'");
            
            var rooms = new List<GenerationRoom>(_generatedRooms.Where(room => !room.IsStartRoom));
            
            for (int i = 0; i < 50; i++)
            {
                foreach (var room in rooms)
                {
                    var position = room.GetNextPosition(_clusteringOffset);
                    var colliders = Physics2D.OverlapBoxAll(position, room.OverlapSize, 0, _generationLayerMask);
                    
                    if (colliders.Length != 1 || room.MoveIterations >= _moveIterations) 
                        continue;
                    
                    room.MoveTo(position);
                    yield return new WaitForFixedUpdate();
                }
            }

            _positions.Clear();
            _positions = _generatedRooms.Select(room => room.RoomCenter).ToList();
            _isClusteringFinishing = true;
            
            GenerateDungeon();
        }

        private Delaunay TriangulateDelaunay()
        {
            var vertices = _positions.Select(position => new Vertex(position)).ToList();
            var delaunay = Triangulate(vertices);
            return delaunay;
        }

        private void Clear()
        {
            foreach (var room in _generatedRooms)
            {
                Destroy(room.gameObject);
            }

            CorridorGenerator.Instance.ClearAllCorridors();
            AvailableRoomsManager.Instance.Restart();
            _isGenerationFinished = false;
            _isClusteringFinishing = false;
            _positions.Clear();
            _generatedRooms.Clear();
        }
        
        private void OnDrawGizmos()
        {
            if(!_isCanShowGizmos)
                return;
            
            Gizmos.color = Color.magenta;
            
            foreach (var room in _generatedRooms)
            {
                Gizmos.DrawWireCube( room.RoomCenter, room.OverlapSize);
            }
            
            Gizmos.color = Color.green;
            foreach (var triangle in _edges)
            {
                Gizmos.DrawLine(triangle.U.Position, triangle.V.Position);
            }
        }
    }
}
