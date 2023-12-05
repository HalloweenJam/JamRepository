using UnityEngine;

namespace Utilities.Classes.Delaunay
{
    public class Vertex
    {
        public Vector2 Position { get; }
        
        public Vertex(Vector2 pos)
        {
            Position = pos;
        }
    }
}