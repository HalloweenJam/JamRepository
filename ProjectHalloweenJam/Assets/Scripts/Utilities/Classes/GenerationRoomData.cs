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
        [SerializeField] private OptionalProperty<int> _minRooms;
        
        public GenerationRoom Room => _room;
    }
}