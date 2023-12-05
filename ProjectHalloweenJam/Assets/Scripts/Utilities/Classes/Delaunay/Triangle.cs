using System;
using UnityEngine;

namespace Utilities.Classes.Delaunay
{
    public class Triangle : IEquatable<Triangle>
    {
        public Vertex A { get; }
        public Vertex B { get; }
        public Vertex C { get; }
        public bool IsBad { get; set; }

        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
        }

        public bool ContainsVertex(Vector3 v)
        {
            return Vector3.Distance(v, A.Position) < 0.01f
                   || Vector3.Distance(v, B.Position) < 0.01f
                   || Vector3.Distance(v, C.Position) < 0.01f;
        }

        public bool CircumCircleContains(Vector3 v)
        {
            Vector3 a = A.Position;
            Vector3 b = B.Position;
            Vector3 c = C.Position;

            var ab = a.sqrMagnitude;
            var cd = b.sqrMagnitude;
            var ef = c.sqrMagnitude;

            var circumX = (ab * (c.y - b.y) + cd * (a.y - c.y) + ef * (b.y - a.y)) / (a.x * (c.y - b.y) + b.x * (a.y - c.y) + c.x * (b.y - a.y));
            var circumY = (ab * (c.x - b.x) + cd * (a.x - c.x) + ef * (b.x - a.x)) / (a.y * (c.x - b.x) + b.y * (a.x - c.x) + c.y * (b.x - a.x));

            var circum = new Vector3(circumX / 2, circumY / 2);
            var circumRadius = Vector3.SqrMagnitude(a - circum);
            var dist = Vector3.SqrMagnitude(v - circum);

            return dist <= circumRadius;
        }

        public static bool operator ==(Triangle left, Triangle right)
        {
            return (left.A == right.A || left.A == right.B || left.A == right.C)
                   && (left.B == right.A || left.B == right.B || left.B == right.C)
                   && (left.C == right.A || left.C == right.B || left.C == right.C);
        }

        public static bool operator !=(Triangle left, Triangle right) => !(left == right);

        public override bool Equals(object obj)
        {
            if (obj is Triangle triangle)
            {
                return this == triangle;
            }

            return false;
        }

        public bool Equals(Triangle triangle) => this == triangle;

        public override int GetHashCode() => A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();
    }
}