using PolygonApp.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace PolygonApp.Algorithms
{
    #region structs
    public struct EdgeBucket
    {
        public int ymax;
        public int xmin;
        public int x;
        public double exactx;
        public double slopeinv;
    }
    public struct EdgeTableRow
    {
        public List<EdgeBucket> list;
    }
    #endregion


    static class PolygonFill
    {
        static public void SimplePolygonFill(Polygon poly, Bitmap canvas)
        {
            var (edgeTable, numOfScanLines, y0) = CreateEdgeTable(poly);
            var activeEdgeList = new List<EdgeBucket>();

            for (int i = 0; i < numOfScanLines; i++)
            {
                var y = i + y0;

                activeEdgeList.RemoveAll(e => e.ymax <= y);


                if (edgeTable[i].list != null)
                    foreach (var e in edgeTable[i].list)
                    {
                        activeEdgeList.Add(e);
                        Debug.WriteLine($"[i = {i}] Added: {e.xmin}");
                    }
                activeEdgeList.Sort((a, b) => a.x.CompareTo(b.x));

                var even = activeEdgeList.Where((x, k) => k % 2 == 0);
                var odd = activeEdgeList.Where((x, k) => k % 2 == 1);
                var edgePairs = even.Zip(odd, (first, second) => (first, second));
                foreach(var pair in edgePairs)
                    for (int x = pair.first.x; x <= pair.second.x; x++)
                    {
                        Debug.WriteLineIf(i==20, $"Coloring ({x},{y})");
                        canvas.SetPixel(x, y, Color.Aquamarine);
                    }

                for (int k = 0; k < activeEdgeList.Count; k++)
                {
                    var edge = activeEdgeList.ElementAt(k);
                    activeEdgeList.RemoveAt(k);

                    edge.exactx += edge.slopeinv;
                    edge.x = (int)edge.exactx;

                    activeEdgeList.Insert(k, edge);
                }
            }

        }

        static private (EdgeTableRow[], int, int) CreateEdgeTable(Polygon poly)
        {
            var vArr = poly.Vertices;

            var highest = vArr[0];
            var lowest = vArr[0];
            for (int i = 1; i < poly.VerticesCount; i++)
            {
                if (vArr[i].Y > highest.Y)
                    highest = vArr[i];
                else if (vArr[i].Y < lowest.Y)
                    lowest = vArr[i];
            }
            var numOfScanLines = highest.Y - lowest.Y + 1;

            var eList = new List<Edge>(poly.Edges).FindAll(e => e != null);
            var edgeTable = new EdgeTableRow[numOfScanLines];

            foreach (var edge in eList)
            {
                var ymin = edge.Start.Y < edge.End.Y ? edge.Start.Y : edge.End.Y;
                var ymax = edge.Start.Y > edge.End.Y ? edge.Start.Y : edge.End.Y;
                var xmin = edge.Start.Y < edge.End.Y ? edge.Start.X : edge.End.X;

                var slopeinv = (double)(edge.End.X - edge.Start.X) / (edge.End.Y - edge.Start.Y);
                var bucket = new EdgeBucket
                {
                    ymax = ymax,
                    xmin = xmin,
                    x = xmin,
                    exactx = xmin,
                    slopeinv = slopeinv,
                };
                if (edgeTable[ymin - lowest.Y].list == null)
                    edgeTable[ymin - lowest.Y].list = new List<EdgeBucket>();
                edgeTable[ymin - lowest.Y].list.Add(bucket);
            }

            for (int i = 0; i < numOfScanLines; i++)
                if (edgeTable[i].list != null && edgeTable[i].list.Count > 1)
                    edgeTable[i].list.Sort((a, b) => a.xmin.CompareTo(b.xmin));

            return (edgeTable, numOfScanLines, lowest.Y);
        }
    }
}
