using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Classes.Delaunay
{
    public class Delaunay
    {
        public List<Vertex> Vertices { get; private set; }
        public List<Edge> Edges { get; }
        public List<Triangle> Triangles { get; }

        private Delaunay()
        {
            Edges = new List<Edge>();
            Triangles = new List<Triangle>();
        }

        public static Delaunay Triangulate(IEnumerable<Vertex> vertices)
        {
            var delaunay = new Delaunay
            {
                Vertices = new List<Vertex>(vertices)
            };
            delaunay.Triangulate();

            return delaunay;
        }

        public static Vector2 GetEdgeDirection(Edge edge)
        {
            var direction = edge.V.Position - edge.U.Position;
            var normalizedDirection = direction.normalized;

            return normalizedDirection;
        }

        public static List<Edge> GetMinimumSpanningTree(List<Edge> edges, Vertex start) 
        {
            var openSet = new HashSet<Vertex>();
            var closedSet = new HashSet<Vertex>();

            foreach (var edge in edges) 
            {
                openSet.Add(edge.U);
                openSet.Add(edge.V);
            }

            closedSet.Add(start);

            var results = new List<Edge>();

            while (openSet.Count > 0) 
            {
                var isChosen = false;
                Edge chosenEdge = null;
                var minWeight = float.PositiveInfinity;

                foreach (var edge in edges)
                {
                    var closedVertices = 0;
                    
                    if (!closedSet.Contains(edge.U)) 
                        closedVertices++;
                    
                    if (!closedSet.Contains(edge.V)) 
                        closedVertices++;
                    
                    if (closedVertices != 1)
                        continue;

                    if (edge.Distance > minWeight) 
                        continue;
                    
                    chosenEdge = edge;
                    isChosen = true;
                    minWeight = edge.Distance;
                }

                if (!isChosen) 
                    break;
                
                results.Add(chosenEdge);
                openSet.Remove(chosenEdge.U);
                openSet.Remove(chosenEdge.V);
                closedSet.Add(chosenEdge.U);
                closedSet.Add(chosenEdge.V);
            }

            return results;
        }

        private void Triangulate()
        {
            var minX = Vertices[0].Position.x;
            var minY = Vertices[0].Position.y;
            var maxX = minX;
            var maxY = minY;

            foreach (var vertex in Vertices)
            {
                if (vertex.Position.x < minX) 
                    minX = vertex.Position.x;
                if (vertex.Position.x > maxX) 
                    maxX = vertex.Position.x;
                if (vertex.Position.y < minY) 
                    minY = vertex.Position.y;
                if (vertex.Position.y > maxY) 
                    maxY = vertex.Position.y;
            }

            var deltaX = maxX - minX;
            var deltaY = maxY - minY;
            var deltaMax = Mathf.Max(deltaX, deltaY) * 2;

            var p1 = new Vertex(new Vector2(minX - 1, minY - 1));
            var p2 = new Vertex(new Vector2(minX - 1, maxY + deltaMax));
            var p3 = new Vertex(new Vector2(maxX + deltaMax, minY - 1));

            Triangles.Add(new Triangle(p1, p2, p3));

            foreach (var vertex in Vertices)
            {
                var polygon = new List<Edge>();

                foreach (var triangle in Triangles.Where(triangle => triangle.CircumCircleContains(vertex.Position)))
                {
                    triangle.IsBad = true;
                    polygon.Add(new Edge(triangle.A, triangle.B));
                    polygon.Add(new Edge(triangle.B, triangle.C));
                    polygon.Add(new Edge(triangle.C, triangle.A));
                }

                Triangles.RemoveAll(triangle => triangle.IsBad);

                for (int i = 0; i < polygon.Count; i++)
                {
                    for (int j = i + 1; j < polygon.Count; j++)
                    {
                        if (!Edge.AlmostEqual(polygon[i], polygon[j])) 
                            continue;
                        
                        polygon[i].IsBad = true;
                        polygon[j].IsBad = true;
                    }
                }

                polygon.RemoveAll(edge => edge.IsBad);

                foreach (var edge in polygon)
                {
                    Triangles.Add(new Triangle(edge.U, edge.V, vertex));
                }
            }

            Triangles.RemoveAll(triangle =>
                triangle.ContainsVertex(p1.Position) || 
                triangle.ContainsVertex(p2.Position) ||
                triangle.ContainsVertex(p3.Position));

            var edgeSet = new HashSet<Edge>();

            foreach (var triangle in Triangles)
            {
                var ab = new Edge(triangle.A, triangle.B);
                var bc = new Edge(triangle.B, triangle.C);
                var ca = new Edge(triangle.C, triangle.A);

                if (edgeSet.Add(ab))
                    Edges.Add(ab);

                if (edgeSet.Add(bc))
                    Edges.Add(bc);

                if (edgeSet.Add(ca))
                    Edges.Add(ca);
            }
        }
    }
}