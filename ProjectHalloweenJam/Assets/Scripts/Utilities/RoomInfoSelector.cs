using System.Collections.Generic;
using Generation.Rooms;
using UnityEngine;
using Utilities.Classes.Delaunay;

namespace Utilities
{
    public static class RoomInfoSelector
    {
        public static (GenerationRoom, GenerationRoom, Vector2) GetEdgeInfo(Edge edge, List<GenerationRoom> rooms)
        {
            var firstRoom = rooms.Find(room => room.RoomCenter == edge.U.Position);
            var secondRoom = rooms.Find(room => room.RoomCenter == edge.V.Position);

            var normalizedDirection = Delaunay.GetEdgeDirection(edge);
            Vector2 roundedDirection;
            
            if (normalizedDirection.y > Mathf.Abs(normalizedDirection.x))
                roundedDirection = Vector2.up;
            
            else if (normalizedDirection.y < -Mathf.Abs(normalizedDirection.x))
                roundedDirection = Vector2.down;           
            else
                roundedDirection = normalizedDirection.x > 0 ? Vector2.right : Vector2.left; 

            return (firstRoom, secondRoom, roundedDirection);
        }
    }
}