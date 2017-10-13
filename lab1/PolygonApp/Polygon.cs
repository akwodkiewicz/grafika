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
        private Vertex[] vertices;
        private Line[] lines;

        public Polygon()
        {
            vertices = new Vertex[maxVertices];
            lines = new Line[maxVertices];
        }

        public int VerticesCount { get => verticesCount; }

        public bool AddVertex(Point point)
        {
            if (verticesCount > 0 && vertices[0].Contains(point))
                return false;

            Vertex vertex = new Vertex(point);
            vertices[verticesCount++] = vertex;

            if (verticesCount == 1) return true;
            
            Line line = new Line(vertices[verticesCount - 2].Point, vertices[verticesCount - 1].Point);
            lines[verticesCount-2] = line;

            return true;
        }

        public void Draw(Bitmap canvas)
        {
            for (int i = 0; i < verticesCount; i++)
                //if(vertices[i].Invalid)
                    vertices[i].Draw(canvas);

            if (verticesCount > 1)
                for (int i = 0; i < verticesCount-1; i++)
                    //if(lines[i].Invalid)
                        lines[i].Draw(canvas);
        }

        public Vertex GetVertexFromPoint(Point point)
        {
            for (int i = 0; i < verticesCount; i++)
                if (vertices[i].Contains(point))
                    return vertices[i];

            return null;            
        }
    }
}
