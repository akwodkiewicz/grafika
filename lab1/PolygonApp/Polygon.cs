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
        private const int lineProximity = 500;
        private const int vertexProximity = 15;
        private int verticesCount = 0;
        private bool closed = false;
        private int vertexSize;
        private PointC center;
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

        public PointC Center { get => center; set => center = value; }


        public int AddVertex(Point point)
        {
            Vertex vertex;
            if (verticesCount == maxVertices
                || verticesCount > 0 && vertices[0].Contains(point, vertexProximity))
            {
                Close();
                return -1;
            }
            if (verticesCount == 0)
            {
                vertex = new Vertex(new PointC(point), VertexSize);
                vertices[verticesCount++] = vertex;
            }
            vertex = new Vertex(new PointC(point), VertexSize);
            vertices[verticesCount] = vertex;

            Line line = new Line(vertices[verticesCount - 1], vertex);
            lines[verticesCount - 1] = line;

            verticesCount++;
            return verticesCount - 1;
        }

        public void ClearLineConstraints(int id)
        {
            lines[id].SetConstraint(Constraint.None);
        }

        public void Close()
        {
            if (verticesCount == maxVertices)
            {
                lines[verticesCount - 1] = new Line(vertices[verticesCount - 1], vertices[0]);
            }
            else
            {
                verticesCount--;
                vertices[verticesCount] = null;
                lines[verticesCount - 1] = new Line(vertices[verticesCount - 1], vertices[0]);
            }
            closed = true;
        }

        public void DeleteVertex(int id)
        {
            if (verticesCount == 3)
                throw new InvalidOperationException();

            vertices[id] = null;
            RearrangeVertices();

            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            lines[prevId].SetConstraint(Constraint.None);
            lines[prevId].End = lines[id].End;
            lines[id] = null;
            RearrangeLines();

            verticesCount--;

        }

        public void Draw(Bitmap canvas)
        {
            if (verticesCount > 1)
                for (int i = 0; i < verticesCount - 1; i++)
                    lines[i].Draw(canvas);
            if (closed)
                lines[verticesCount - 1].Draw(canvas);
            for (int i = 0; i < verticesCount; i++)
                vertices[i].Draw(canvas);
        }

        public int GetLineIdFromPoint(Point point)
        {
            var iMax = closed ? verticesCount : verticesCount - 1;
            double minimumDistance = double.PositiveInfinity;
            int bestId = -1;
            for (int i = 0; i < iMax; i++)
            {
                var dist = lines[i].GetSquaredDistanceFromPoint(new PointC(point));
                if (dist < minimumDistance)
                {
                    minimumDistance = dist;
                    bestId = i;
                }
            }
            if (minimumDistance < lineProximity)
            {
                lines[bestId].LastClickPoint = point;
                return bestId;
            }
            else
                return -1;
        }

        public int GetVertexIdFromPoint(Point point)
        {
            for (int i = 0; i < verticesCount; i++)
                if (vertices[i].Contains(point, vertexProximity))
                    return i;

            return -1;
        }

        public void MoveLine(int id, Point location)
        {
            var dx = location.X - lines[id].LastClickPoint.X;
            var dy = location.Y - lines[id].LastClickPoint.Y;

            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;

            if (lines[prevId].Constraint == Constraint.Vertical
                || lines[nextId].Constraint == Constraint.Vertical)
            {
                dx = 0;
            }
            if (lines[prevId].Constraint == Constraint.Horizontal
                || lines[nextId].Constraint == Constraint.Horizontal)
            {
                dy = 0;
            }

            vertices[id].MovePoint(dx, dy);
            vertices[nextId].MovePoint(dx, dy);
            lines[id].LastClickPoint = location;
        }

        public void MovePolygon(Point point)
        {
            var dx = point.X - center.X;
            var dy = point.Y - center.Y;
            for (int i = 0; i < verticesCount; i++)
            {
                var p = new PointC(vertices[i].X + dx, vertices[i].Y + dy);
                vertices[i].SetPoint(p);
            }
            center.X = point.X;
            center.Y = point.Y;
        }

        public bool MakeLineHorizontal(int id)
        {
            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;

            if (lines[prevId].Constraint == Constraint.Horizontal
                || lines[nextId].Constraint == Constraint.Horizontal)
                throw new InvalidOperationException();
            return lines[id].SetConstraint(Constraint.Horizontal);
        }

        public bool MakeLineVertical(int id)
        {
            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;

            if (lines[prevId].Constraint == Constraint.Vertical
                || lines[nextId].Constraint == Constraint.Vertical)
                throw new InvalidOperationException();
            return lines[id].SetConstraint(Constraint.Vertical);
        }

        public void SetPointForVertexId(int id, Point point)
        {
            SetPointForVertexId(id, new PointC(point));
        }

        public void SetPointForVertexId(int id, PointC point)
        {
            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;

            if (!closed
                || lines[prevId].Constraint == Constraint.None
                    && lines[id].Constraint == Constraint.None)
            {
                vertices[id].SetPoint(point);
            }
            else if (lines[prevId].Constraint == Constraint.Horizontal
                    && lines[id].Constraint == Constraint.None)
            {
                vertices[prevId].Y = point.Y;
                vertices[id].SetPoint(point);
            }
            else if (lines[prevId].Constraint == Constraint.None
                    && lines[id].Constraint == Constraint.Horizontal)
            {
                vertices[nextId].Y = point.Y;
                vertices[id].SetPoint(point);
            }
            else if (lines[prevId].Constraint == Constraint.Vertical
                    && lines[id].Constraint == Constraint.None)
            {
                vertices[prevId].X = point.X;
                vertices[id].SetPoint(point);
            }
            else if (lines[prevId].Constraint == Constraint.None
                    && lines[id].Constraint == Constraint.Vertical)
            {
                vertices[nextId].X = point.X;
                vertices[id].SetPoint(point);
            }
            else if (lines[prevId].Constraint == Constraint.Horizontal
                && lines[id].Constraint == Constraint.Vertical)
            {
                vertices[prevId].Y = point.Y;
                vertices[nextId].X = point.X;
                vertices[id].SetPoint(point);
            }
            else if (lines[prevId].Constraint == Constraint.Vertical
                    && lines[id].Constraint == Constraint.Horizontal)
            {
                vertices[prevId].X = point.X;
                vertices[nextId].Y = point.Y;
                vertices[id].SetPoint(point);
            }

            //if (verticesCount > 2)
            //{
            //    int inwardLineId = (id == 0) ? verticesCount - 1 : id - 1;
            //    lines[inwardLineId].End = point;
            //    if (closed)
            //    {
            //        if (!vertices[id].HasAngleConstraint
            //            && !vertices[prevId].HasAngleConstraint
            //            && !vertices[nextId].HasAngleConstraint)
            //        {
            //            //something
            //        }
            //        else
            //        {
            //            //something complicated
            //        }
            //        int outwardLineId = id;
            //        lines[outwardLineId].Start = point;
            //    }
            //}
            //if (verticesCount == 2)
            //{
            //    if (id == 0) 
            //        lines[0].Start = point;
            //    else if (id == 1) 
            //        lines[0].End = point;
            //}
        }

        internal void ClearVertexConstraints(int id)
        {
            vertices[id].AngleConstraint = null;
        }

        public void SetAngleConstraint(int id, int angle)
        {
            vertices[id].AngleConstraint = angle;
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
            var middle = new PointC(x, y);

            for (int i = verticesCount; i > id + 1; i--)
            {
                vertices[i] = vertices[i - 1];
                lines[i] = lines[i - 1];
            }

            // vertices[id+1] and lines[id+1] are now null

            var vertex = new Vertex(middle, VertexSize);
            vertices[id + 1] = vertex;
            var line = new Line(vertex, oldLine.End);
            oldLine.End = vertex;
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
                lines[0].Start = vertices[0];
                lines[0].End = vertices[1];
            }
        }
    }
}
