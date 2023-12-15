using System.Collections.Generic;
using System.Linq;
using Generation.Rooms;
using Scriptables;
using UnityEngine;

namespace Managers
{
    public class AvailableRoomsManager : Singleton<AvailableRoomsManager>
    {
        [SerializeField] private RoomsContainer _roomsContainer;

        private List<GenerationRoom> _requiredRooms = new();
        private int _requiredRoomsCounter;

        private bool _isRequiredListInitialized = false;
        private bool _isRequiredRoomsListEnded = false;
        
        public GenerationRoom GetRoom(bool hasContact)
        {
            if (!_isRequiredListInitialized)
            {
                var requiredRoomsCount =
                    _roomsContainer.RoomsData.weightedList.Count(item => item.weightedItem.IsRequiredRoom);

                if (requiredRoomsCount > 0)
                {
                    _requiredRooms = GetAllRequiredRooms();
                    _isRequiredListInitialized = true;
                    _requiredRoomsCounter = -1;
                }
            }

            if (_requiredRooms.Count - 1 > _requiredRoomsCounter && !_isRequiredRoomsListEnded)
            {
                if (!hasContact)
                    _requiredRoomsCounter++;

                print(_requiredRoomsCounter);

                var requiredRoom = _requiredRooms[_requiredRoomsCounter];

                return requiredRoom;
            }

            if (!hasContact)
                _isRequiredRoomsListEnded = true;

            print("==== not required rooms ====");

            var room = _roomsContainer.GetRoom();
            return room;
        }


        private List<GenerationRoom> GetAllRequiredRooms()
        {
            var rooms = new List<GenerationRoom>();
            var pairsList = _roomsContainer.RoomsData.weightedList.Where(room => room.weightedItem.IsRequiredRoom).ToList();
            
            foreach (var pair in pairsList)
            {
                for (int i = 0; i < pair.weightedItem.RequiredCount; i++)
                {
                    rooms.Add(pair.weightedItem.Room);
                }
            }
            
            print($"required rooms count:<color=#00FFFF> {rooms.Count} </color>");

            return rooms;
        }

        private void Start() => Restart();

        public void Restart()
        {
            _requiredRooms.Clear();
            _requiredRoomsCounter = 0;
            _isRequiredListInitialized = false;
        }
    }
}