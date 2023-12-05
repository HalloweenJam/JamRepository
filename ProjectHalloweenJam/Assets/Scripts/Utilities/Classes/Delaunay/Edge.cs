using UnityEngine;

namespace Utilities.Classes.Delaunay
{
    public class Edge
    {
        public Vertex U { get; }
        public Vertex V { get; }
        public bool IsBad { get; set; }

        public float Distance { get; private set; }
        
        public Edge(Vertex u, Vertex v)
        {
            U = u;
            V = v;
            
            Distance = Vector3.Distance(u.Position, v.Position);
        }

        public static bool operator ==(Edge left, Edge right)
        {
            return (left.U == right.U || left.U == right.V) && (left.V == right.U || left.V == right.V);
        }

        public static bool operator !=(Edge left, Edge right) => !(left == right);
        
        public override bool Equals(object obj)
        {
            if (obj is Edge edge)
                return this == edge;

            return false;
        }

        public override int GetHashCode() => U.GetHashCode() ^ V.GetHashCode();

        public static bool AlmostEqual(Edge left, Edge right)
        {
            return AlmostEqual(left.U, right.U) && AlmostEqual(left.V, right.V)
                   || AlmostEqual(left.U, right.V) && AlmostEqual(left.V, right.U);
        }
        
        private static bool AlmostEqual(Vertex left, Vertex right)
        {
            return AlmostEqual(left.Position.x, right.Position.x) && AlmostEqual(left.Position.y, right.Position.y);
        }
        
        private static bool AlmostEqual(float x, float y)
        {
            return Mathf.Abs(x - y) <= float.Epsilon * Mathf.Abs(x + y) * 2 || Mathf.Abs(x - y) < float.MinValue;
        }
    }
}