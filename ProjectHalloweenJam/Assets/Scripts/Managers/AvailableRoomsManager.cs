using Generation.Rooms;
using Scriptables;
using UnityEngine;

namespace Managers
{
    public class AvailableRoomsManager : Singleton<AvailableRoomsManager>
    {
        [SerializeField] private RoomsContainer _roomsContainer;

        private int _requiredRoomsCounter = 0;

        public GenerationRoom GetRoom()
        {
            if (_requiredRoomsCounter >= _roomsContainer.RequiredRooms.Count)
            {
                var room = _roomsContainer.GetRoom();
                return room;
            }
            
            var requiredRoom = _roomsContainer.RequiredRooms[_requiredRoomsCounter].Room;
            _requiredRoomsCounter++;
            return requiredRoom;
        }

        private void Start() => Restart();

        public void Restart()
        {
            _requiredRoomsCounter = 0;
        }
    }
}