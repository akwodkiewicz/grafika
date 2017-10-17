using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp
{
    class Polygon : IDrawable
    {
        private const int maxVertices = 20;
        private const int snapDistanceSquared = 500;
        private int verticesCount = 0;
        private bool closed = false;
        private int vertexSize;
        private Point center;
        private Vertex[] vertices;
        private Line[] lines;

        public Polygon(int vSize)
        {
            vertices = new Vertex[maxVertices];
            lines = new Line[maxVertices];
            vertexSize = vSize;
        }

        public int VerticesCount { get => verticesCount; }

        public int VertexSize
        {
            get => vertexSize;
            set
            {
                vertexSize = value;
                for (int i = 0; i < verticesCount; i++)
                    vertices[i].Dimension = vertexSize;
            }
        }

        public Point Center { get => center; set => center = value; }


        public int AddVertex(Point point)
        {
            if (VerticesCount == maxVertices) return -1;

            Vertex vertex;
            if (verticesCount > 0 && vertices[0].Contains(point))
            {
                Close();
                return -1;
            }
            if (verticesCount == 0)
            {
                vertex = new Vertex(point, VertexSize);
                vertices[verticesCount++] = vertex;
            }
            vertex = new Vertex(point, VertexSize);
            vertices[verticesCount++] = vertex;

            Line line = new Line(vertices[verticesCount - 2].Point, vertices[verticesCount - 1].Point);
            lines[verticesCount - 2] = line;

            return verticesCount - 1;
        }

        public void Close()
        {
            verticesCount--;
            vertices[verticesCount] = null;
            lines[verticesCount - 1] = new Line(vertices[verticesCount - 1].Point, vertices[0].Point);
            closed = true;
        }

        public void DeleteVertex(int id)
        {
            vertices[id] = null;
            if (verticesCount == 1)
            {

            }
            else if (verticesCount == 2)
            {
                RearrangeVertices();
                lines[0] = null;
            }
            else if (verticesCount == 3)
            {
                RearrangeVertices();
                var prevId = (id == 0) ? verticesCount - 1 : id - 1;
                lines[prevId] = null;
                lines[id] = null;
                RearrangeLines();
                closed = false;
            }
            else
            {
                RearrangeVertices();

                var prevId = (id == 0) ? verticesCount - 1 : id - 1;
                lines[prevId].End = lines[id].End;
                lines[id] = null;

                RearrangeLines();
            }
            verticesCount--;

        }

        public void Draw(Bitmap canvas)
        {
            for (int i = 0; i < verticesCount; i++)
                vertices[i].Draw(canvas);

            if (verticesCount > 1)
                for (int i = 0; i < verticesCount - 1; i++)
                    lines[i].Draw(canvas);
            if (closed)
                lines[verticesCount - 1].Draw(canvas);
        }

        public int GetLineIdFromPoint(Point point)
        {
            var iMax = closed ? verticesCount : verticesCount - 1;
            double minimumDistance = double.PositiveInfinity;
            int bestId = -1;
            for (int i = 0; i < iMax; i++)
            {
                var dist = lines[i].GetSquaredDistanceFromPoint(point);
                if (dist < minimumDistance)
                {
                    minimumDistance = dist;
                    bestId = i;
                }
            }
            if (minimumDistance < snapDistanceSquared)
                return bestId;
            else
                return -1;
        }

        public int GetVertexIdFromPoint(Point point)
        {
            for (int i = 0; i < verticesCount; i++)
                if (vertices[i].Contains(point))
                    return i;

            return -1;
        }

        public void MovePolygon(Point point)
        {
            var dx = point.X - center.X;
            var dy = point.Y - center.Y;
            for (int i = 0; i < verticesCount; i++)
            {
                var p = new Point(vertices[i].X + dx, vertices[i].Y + dy);
                SetPointForVertexId(i, p);
            }
            center = point;
        }

        public void SetPointForVertexId(int id, Point point)
        {
            vertices[id].Point = point;
            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;
            if (verticesCount > 2)
            {
                int inwardLineId = (id == 0) ? verticesCount - 1 : id - 1;
                lines[inwardLineId].End = point;
                if (closed)
                {
                    if (!vertices[id].HasAngleConstraint
                        && !vertices[prevId].HasAngleConstraint
                        && !vertices[nextId].HasAngleConstraint)
                    {
                        //something
                    }
                    else
                    {
                        //something complicated
                    }
                    int outwardLineId = id;
                    lines[outwardLineId].Start = point;
                }
            }
            else if (verticesCount == 2)
            {
                if (id == 0)
                    lines[0].Start = point;
                else if (id == 1)
                    lines[0].End = point;
            }
        }

        public void SetAngleConstraint(int id, int angle)
        {
            vertices[id].AngleConstraint = angle;
            SetPointForVertexId(id, vertices[id].Point);
        }

        public void SetLineColor(int id, Color color)
        {
            lines[id].Color = color;
        }


        public void AddVertexToLine(int id)
        {
            if (VerticesCount == maxVertices) return;


            var oldLine = lines[id];
            var x = (oldLine.End.X + oldLine.Start.X) / 2;
            var y = (oldLine.End.Y + oldLine.Start.Y) / 2;
            var middle = new Point(x, y);

            for (int i = verticesCount; i > id + 1; i--)
            {
                vertices[i] = vertices[i - 1];
                lines[i] = lines[i - 1];
            }

            // vertices[id+1] and lines[id+1] are now null

            var vertex = new Vertex(middle, VertexSize);
            vertices[id + 1] = vertex;
            var line = new Line(middle, oldLine.End);
            oldLine.End = middle;
            lines[id + 1] = line;

            verticesCount++;
        }

        private void RearrangeVertices()
        {
            for (int i = 0; i < verticesCount; i++)
            {
                if (vertices[i] == null && i + 1 < verticesCount)
                {
                    vertices[i] = vertices[i + 1];
                    vertices[i + 1] = null;
                }
            }
        }

        private void RearrangeLines()
        {
            for (int i = 0; i < verticesCount; i++)
            {
                if (lines[i] == null && i + 1 < verticesCount)
                {
                    lines[i] = lines[i + 1];
                    lines[i + 1] = null;
                }
            }
            if (verticesCount == 3)
            {
                int id = 0;
                if (lines[1] != null)
                    id = 1;
                else if (lines[2] != null)
                    id = 2;
                if (id != 0)
                {
                    lines[0] = lines[id];
                    lines[id] = null;
                }
                lines[0].Start = vertices[0].Point;
                lines[0].End = vertices[1].Point;
            }
        }
    }
}
