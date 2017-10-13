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
        private Vertex[] vertices;
        private Line[] lines;

        public Polygon()
        {
            vertices = new Vertex[maxVertices];
            lines = new Line[maxVertices];
        }

        public int VerticesCount { get => verticesCount; }

        public bool AddVertex(Point point, int dimension)
        {
            if (verticesCount > 0 && vertices[0].Contains(point))
            {
                lines[verticesCount - 1] = new Line(vertices[verticesCount - 1].Point, vertices[0].Point);
                closed = true;
                return false;
            }

            Vertex vertex = new Vertex(point, dimension);
            vertices[verticesCount++] = vertex;

            if (verticesCount == 1) return true;

            Line line = new Line(vertices[verticesCount - 2].Point, vertices[verticesCount - 1].Point);
            lines[verticesCount - 2] = line;

            return true;
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

        public void SetPointForVertexId(int id, Point point)
        {
            int outwardLineId = id;
            int inwardLineId = (id == 0) ? verticesCount - 1 : id - 1;
            vertices[id].Point = point;
            lines[outwardLineId].Start = point;
            lines[inwardLineId].End = point;
        }
    }
}
