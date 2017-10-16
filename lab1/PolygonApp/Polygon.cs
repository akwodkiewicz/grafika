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
            vertexSize = vSize * 10 + 11;
        }

        public int VerticesCount { get => verticesCount; }
        public int VertexSize
        {
            get => vertexSize;
            set
            {
                var difference = vertexSize - (value * 10 + 11);
                vertexSize = value * 10 + 11;
                for (int i = 0; i < verticesCount; i++)
                {
                    vertices[i].Top += difference / 2;
                    vertices[i].Left += difference / 2;
                    vertices[i].Dimension = vertexSize;
                }
            }
        }

        public Point Center { get => center; set => center = value; }

        public bool AddVertex(Point point)
        {
            if (verticesCount > 0 && vertices[0].Contains(point))
            {
                lines[verticesCount - 1] = new Line(vertices[verticesCount - 1].Point, vertices[0].Point);
                closed = true;
                return false;
            }

            Vertex vertex = new Vertex(point, VertexSize);
            vertices[verticesCount++] = vertex;

            if (verticesCount == 1) return true;

            Line line = new Line(vertices[verticesCount - 2].Point, vertices[verticesCount - 1].Point);
            lines[verticesCount - 2] = line;

            return true;
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
            if (verticesCount > 2)
            {
                int outwardLineId = id;
                int inwardLineId = (id == 0) ? verticesCount - 1 : id - 1;
                lines[outwardLineId].Start = point;
                lines[inwardLineId].End = point;
            }
            else if (verticesCount == 2)
            {
                if (id == 0)
                    lines[0].Start = point;
                else if (id == 1)
                    lines[0].End = point;
            }
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
            }
        }
    }
}
