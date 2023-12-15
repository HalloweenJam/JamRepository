using System;
using Generation.Rooms;
using UnityEngine;

namespace Utilities.Classes
{
    [Serializable]
    public class GenerationRoomData
    {
        [Header("Asset")]
        [SerializeField] private GenerationRoom _room;

        [Header("Settings")] 
        [SerializeField] private bool _isStartRoom;
        [SerializeField] private OptionalProperty<int> _minRooms;
        
        public GenerationRoom Room => _room;

        public bool IsRequiredRoom => _isStartRoom || (_minRooms.Enabled && _minRooms.Value >= 0);
        public int RequiredCount => _isStartRoom ? 1 : _minRooms.Enabled ? _minRooms.Value : 0;

        public bool IsStartRoom => _isStartRoom;

        public void Update()
        {
            _room.SetStartRoomState(_isStartRoom);
        }
    }
}