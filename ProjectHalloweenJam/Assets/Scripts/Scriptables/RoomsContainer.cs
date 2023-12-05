using System.Collections.Generic;
using Generation.Rooms;
using UnityEngine;
using Utilities.Classes;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "New Rooms Container", menuName = "Generation/RoomsContainer", order = 0)]
    public class RoomsContainer : ScriptableObject
    {
        [SerializeField] private List<GenerationRoomData> _requiredRooms = new();
        [SerializeField] private WeightedRandomList<GenerationRoomData> _roomsData = new();

        public IReadOnlyList<GenerationRoomData> RequiredRooms => _requiredRooms;
        public IReadOnlyList<WeightedRandomList<GenerationRoomData>.Pair> WeightedList => _roomsData.weightedList;
        
        public GenerationRoom GetRoom() => _roomsData.GetWeightedRandom().Room;
        
    }
}