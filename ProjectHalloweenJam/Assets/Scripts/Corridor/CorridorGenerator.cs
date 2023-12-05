using UnityEngine;
using Generation.Rooms;
using Managers;
using System.Collections.Generic;
using Utilities;
using Utilities.Classes.Delaunay;
using System.Linq;
using DR = Door.Dir;

namespace CorridorGeneration
{
    public class CorridorGenerator : Singleton<CorridorGenerator>
    {
        [SerializeField] private TileData _tileData;
        [SerializeField] private LayerMask _layerMask;
        private GameObject _allCorridors;
        private bool _regenarate = false;
        private bool _generated = false;
        private int _limitReDraw = 9;

        public LayerMask CorridorLayer => _layerMask;
        public TileData TildeData => _tileData;

        private void Start() => CreateParent();

        private void CreateParent()
        {
            _allCorridors = new GameObject();
            _allCorridors.name = "AllCorridors";
        }

        public void ReadValue(Edge edge, List<GenerationRoom> rooms)
        {
            int tryNumber = 0;
            var (firstRoom, secondRoom, direction) = RoomInfoSelector.GetEdgeInfo(edge, rooms);
            Generate(firstRoom, secondRoom, direction, false, tryNumber);
        }
        private bool Generate(GenerationRoom startRoom, GenerationRoom secondRoom, Vector2 direction, bool checkDiagonal, int drawingOption)
        {
            drawingOption++;

            if (drawingOption > _limitReDraw)
            {
                _generated = false;
                if (!_regenarate)
                    GoToOtherLinks(startRoom, secondRoom);
                return _generated;
            }

            DR firstKey = default;
            DR secondKey = default;

            if (!checkDiagonal)
            {
                if (direction == Vector2.down || direction == Vector2.up)
                {
                    firstKey = direction == Vector2.down ? DR.Bottom : DR.Top;
                    secondKey = direction == Vector2.down ? DR.Top : DR.Bottom;
                }
                else if (direction == Vector2.right || direction == Vector2.left)
                {
                    firstKey = direction == Vector2.right ? DR.Right : DR.Left;
                    secondKey = direction == Vector2.right ? DR.Left : DR.Right;
                }
            }
            else
            {
                var keys = GetOtherDrawingOption(startRoom, secondRoom, drawingOption, direction);
                firstKey = keys.firstKey;
                secondKey = keys.secondKey;
            }

            var (corridor, firstDoor, secondDoor) = DrawCorridor(startRoom, secondRoom, (firstKey, secondKey), drawingOption);
            if (corridor == null || (corridor != null && !corridor.gameObject.activeInHierarchy))
            {
                _generated = false;
                if (firstDoor != null && secondDoor != null)
                {
                    var firstRooms = startRoom.AdjoiningRooms[firstDoor.Direction];
                    var secondRooms = secondRoom.AdjoiningRooms[secondDoor.Direction];

                    if (firstRooms.Count == 1 && firstRooms.ElementAt(0) == secondRoom)
                    {
                        startRoom.AdjoiningRooms[firstDoor.Direction].Remove(secondRoom);
                        startRoom.AdjoiningRooms.Remove(firstDoor.Direction);
                        firstDoor.SetActive(false, startRoom.WallTilemap);
                    }
                    if (secondRooms.Count == 1 && (secondRooms.ElementAt(0) == startRoom))
                    {
                        secondRoom.AdjoiningRooms[secondDoor.Direction].Remove(startRoom);
                        secondRoom.AdjoiningRooms.Remove(secondDoor.Direction);
                        secondDoor.SetActive(false, secondRoom.WallTilemap);
                    }
                }
                Generate(startRoom, secondRoom, direction, true, drawingOption);
            }
            else
                _generated = true;

            return _generated;
        }

        private (Grid, Door, Door) DrawCorridor(GenerationRoom startRoom, GenerationRoom secondRoom, (DR firstDoor, DR secondDoor) keys, int tryNumber)
        {
            Door firstDoor = startRoom.Doors.GetValueOrDefault(keys.firstDoor);
            Door secondDoor = secondRoom.Doors.GetValueOrDefault(keys.secondDoor);

            if (firstDoor == null || secondDoor == null)
                return (null, firstDoor, secondDoor);

            if (!startRoom.AdjoiningRooms.ContainsKey(firstDoor.Direction))
                startRoom.AdjoiningRooms[firstDoor.Direction] = new();
            if (!secondRoom.AdjoiningRooms.ContainsKey(secondDoor.Direction))
                secondRoom.AdjoiningRooms[secondDoor.Direction] = new();

            startRoom.AdjoiningRooms[firstDoor.Direction].Add(secondRoom);
            secondRoom.AdjoiningRooms[secondDoor.Direction].Add(startRoom);

            firstDoor.SetActive(true, startRoom.WallTilemap);
            secondDoor.SetActive(true, secondRoom.WallTilemap);

            CorridorPainter corridor = new CorridorPainter(_tileData, tryNumber);
            Grid localCorridor = corridor.GenerateCorridor(firstDoor, secondDoor);

            return (localCorridor, firstDoor, secondDoor);
        }

        private void GoToOtherLinks(GenerationRoom startRoom, GenerationRoom secondRoom)
        {
            _regenarate = true;
            GenerationRoom parentStartRoom = startRoom.AdjoiningRooms.Values.Count > secondRoom.AdjoiningRooms.Values.Count ? startRoom : secondRoom;
            secondRoom = parentStartRoom != secondRoom ? secondRoom : startRoom;

            Dictionary<GenerationRoom, float> roomsDistance = new();
            bool generated = false;

            for (int i = 0; i < parentStartRoom.AdjoiningRooms.Keys.Count; i++)
            {
                DR directionDoor = parentStartRoom.AdjoiningRooms.Keys.ElementAtOrDefault(i);
                if (parentStartRoom.AdjoiningRooms[directionDoor] != null && parentStartRoom.AdjoiningRooms[directionDoor].ElementAt(0) != null)
                {
                    GenerationRoom newStartRoom = parentStartRoom.AdjoiningRooms[directionDoor].ElementAt(0);
                    float distance = Vector2.Distance(newStartRoom.transform.position, secondRoom.transform.position);
                    roomsDistance.Add(newStartRoom, distance);
                }
            }
            var roomsAndDistances = roomsDistance.OrderBy(rd => rd.Value);

            foreach (var element in roomsAndDistances)
            {
                int tryNumber = 0;
                Vector2 direction = CalculateNewDirection(element.Key, secondRoom);
                generated = Generate(element.Key, secondRoom, direction, false, tryNumber);

                if (generated)
                {
                    _regenarate = false;
                    return;
                }
            }
            _regenarate = false;
        }
        private (DR firstKey, DR secondKey) GetOtherDrawingOption(GenerationRoom startRoom, GenerationRoom secondRoom, int drawingOption, Vector2 direction)
        {
            DR firstKey = default;
            DR secondKey = default;

            _limitReDraw = 9;
            bool directionHorizontal = direction == Vector2.right || direction == Vector2.left;
            bool directionVertical = direction == Vector2.up || direction == Vector2.down;

            bool isSecondRoomHigher = secondRoom.transform.position.y > startRoom.transform.position.y ? true : false;
            bool isSecondRoomForwards = secondRoom.transform.position.x > startRoom.transform.position.x ? true : false;

            if (directionHorizontal)
            {
                switch (drawingOption)
                {
                    case 4:
                        firstKey = direction == Vector2.right ? DR.Right : DR.Left;
                        secondKey = isSecondRoomHigher ? DR.Bottom : DR.Top;
                        break;
                    case 6:
                        firstKey = isSecondRoomHigher ? DR.Top : DR.Bottom;
                        secondKey = isSecondRoomHigher ? DR.Bottom : DR.Top;
                        break;
                }
            }
            else if (directionVertical)
            {
                switch (drawingOption)
                {
                    case 4:
                        secondKey = isSecondRoomHigher ? DR.Bottom : DR.Top;
                        firstKey = isSecondRoomForwards ? DR.Right : DR.Left;
                        break;
                    case 5:
                        secondKey = isSecondRoomHigher ? DR.Top : DR.Bottom;
                        firstKey = isSecondRoomForwards ? DR.Left : DR.Right;
                        break;
                }
            }
            switch (drawingOption)
            {
                case 2:
                    firstKey = isSecondRoomHigher ? DR.Top : DR.Bottom;
                    if (directionHorizontal)
                        secondKey = direction == Vector2.right ? DR.Left : DR.Right;
                    else
                        secondKey = isSecondRoomForwards ? DR.Left : DR.Right;
                    break;
                case 3:
                    firstKey = isSecondRoomHigher ? DR.Bottom : DR.Top;
                    if (directionHorizontal)
                        secondKey = direction == Vector2.right ? DR.Left : DR.Right;
                    else
                        secondKey = isSecondRoomForwards ? DR.Right : DR.Left;
                    break;
                case 6:
                case 7:
                    firstKey = drawingOption == 6 ? DR.Top : DR.Bottom;
                    secondKey = drawingOption == 6 ? DR.Top : DR.Bottom;
                    break;
                case 8:
                case 9:
                    firstKey = drawingOption == 8 ? DR.Right : DR.Left;
                    secondKey = drawingOption == 8 ? DR.Right : DR.Left;
                    break;
            }
            return (firstKey, secondKey);
        }

        private Vector2 CalculateNewDirection(GenerationRoom startRoom, GenerationRoom secondRoom)
        {
            Vector2 direction = secondRoom.transform.position - startRoom.transform.position;
            Vector2 normalizedDirection;

            if (direction.y > Mathf.Abs(direction.x))
                normalizedDirection = Vector2.up;

            else if (direction.y < -Mathf.Abs(direction.x))
                normalizedDirection = Vector2.down;
            else
                normalizedDirection = direction.x > 0 ? Vector2.right : Vector2.left;

            return normalizedDirection;
        }

        public void DeleteDisabledCorridors()
        {
            foreach (Transform corridor in _allCorridors.transform)
            {
                if (!corridor.gameObject.activeInHierarchy)
                    Destroy(corridor.gameObject);
            }
        }

        public void ClearAllCorridors()
        {
            Destroy(_allCorridors);
            CreateParent();
        }

        public Grid InstantiateCorridorGrid() => Instantiate(_tileData.GridPrefab, _allCorridors.transform);

        public void DestroyGrid(GameObject grid) => grid.SetActive(false);
    }
}
