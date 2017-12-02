﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    class Delaunay
    {
        public List<Triangle> Triangulate(List<Point> triangulationPoints)
        {
            if (triangulationPoints.Count < 3) throw new ArgumentException("Can not triangulate less than three vertices!");

            List<Triangle> triangles = new List<Triangle>();

            Triangle superTriangle = this.SuperTriangle(triangulationPoints);
            triangles.Add(superTriangle);

            for (int i = 0; i < triangulationPoints.Count; i++)
            {
                List<Edge> EdgeBuffer = new List<Edge>();

                for (int j = triangles.Count - 1; j >= 0; j--)
                {
                    Triangle t = triangles[j];
                    if (t.ContainsInCircumcircle(triangulationPoints[i]) > 0)
                    {
                        EdgeBuffer.Add(new Edge(t.vertex1, t.vertex2));
                        EdgeBuffer.Add(new Edge(t.vertex2, t.vertex3));
                        EdgeBuffer.Add(new Edge(t.vertex3, t.vertex1));
                        triangles.RemoveAt(j);
                    }
                }

                for (int j = EdgeBuffer.Count - 2; j >= 0; j--)
                {
                    for (int k = EdgeBuffer.Count - 1; k >= j + 1; k--)
                    {
                        if (EdgeBuffer[j] == EdgeBuffer[k])
                        {
                            EdgeBuffer.RemoveAt(k);
                            EdgeBuffer.RemoveAt(j);
                            k--;
                            continue;
                        }
                    }
                }

                for (int j = EdgeBuffer.Count - 2; j >= 0; j--)
                {
                    for (int k = EdgeBuffer.Count - 1; k >= j + 1; k--)
                    {
                        if (EdgeBuffer[j] == EdgeBuffer[k])
                        {
                            EdgeBuffer.RemoveAt(k);
                            EdgeBuffer.RemoveAt(j);
                            k--;
                            continue;
                        }
                    }
                }

                for (int j = 0; j < EdgeBuffer.Count; j++)
                {
                    triangles.Add(new Triangle(EdgeBuffer[j].start, EdgeBuffer[j].end, triangulationPoints[i]));
                }
            }

            for (int i = triangles.Count - 1; i >= 0; i--)
            {
                if (triangles[i].SharesVertexWith(superTriangle)) triangles.RemoveAt(i);
            }

            return triangles;
        }

        private Triangle SuperTriangle(List<Point> triangulationPoints)
        {
            double M = triangulationPoints[0].x;
            
            for (int i = 1; i < triangulationPoints.Count; i++)
            {
                double xAbs = Math.Abs(triangulationPoints[i].x);
                double yAbs = Math.Abs(triangulationPoints[i].y);
                if (xAbs > M) M = xAbs;
                if (yAbs > M) M = yAbs;
            }
            
            Point sp1 = new Point(10 * M, 0, 0);
            Point sp2 = new Point(0, 10 * M, 0);
            Point sp3 = new Point(-10 * M, -10 * M, 0);

            return new Triangle(sp1, sp2, sp3);
        }
    }
}
