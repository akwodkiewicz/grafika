using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Vertex vertex;
            if (verticesCount == maxVertices
                || verticesCount > 0 && vertices[0].Contains(point, vertexProximity))
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
                var dist = lines[i].GetSquaredDistanceFromPoint(point);
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

        public int CalculateAngleForVertexId(int id)
        {
            return (int)CalculateAngleABC(vertices[PrevId(id)], vertices[id], vertices[NextId(id)]);
        }

        private int PrevId(int id) { return (id == 0) ? verticesCount - 1 : id - 1; }
        private int NextId(int id) { return (id + 1) % verticesCount; }

        public void MoveLine(int id, Point location)
        {
            var dx = location.X - lines[id].LastClickPoint.X;
            var dy = location.Y - lines[id].LastClickPoint.Y;

            var nextPoint = new Point(vertices[NextId(id)].Point.X, vertices[NextId(id)].Point.Y);
            SetPointForVertexId(id, new Point(vertices[id].Point.X + dx, vertices[id].Point.Y + dy));
            if (nextPoint == new Point(vertices[NextId(id)].Point.X, vertices[NextId(id)].Point.Y))
                SetPointForVertexId(NextId(id), new Point(vertices[NextId(id)].Point.X + dx, vertices[NextId(id)].Point.Y + dy));

            //var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            //var nextId = (id + 1) % verticesCount;

            //if (lines[prevId].Constraint == Constraint.Vertical
            //    || lines[nextId].Constraint == Constraint.Vertical)
            //{
            //    dx = 0;
            //}
            //if (lines[prevId].Constraint == Constraint.Horizontal
            //    || lines[nextId].Constraint == Constraint.Horizontal)
            //{
            //    dy = 0;
            //}

            //vertices[id].MovePoint(dx, dy);
            //vertices[nextId].MovePoint(dx, dy);
            lines[id].LastClickPoint = location;
        }

        public void MovePolygon(Point point)
        {
            var dx = point.X - center.X;
            var dy = point.Y - center.Y;
            for (int i = 0; i < verticesCount; i++)
            {
                var p = new Point(vertices[i].X + dx, vertices[i].Y + dy);
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
                || lines[nextId].Constraint == Constraint.Horizontal
                )
                throw new InvalidOperationException("You can't add 2 consecutive horizontal lines!");
            if (vertices[id].Constraint == Constraint.Angle
                || vertices[nextId].Constraint == Constraint.Angle)
                throw new InvalidOperationException("You can't add this constraint here!");
            return SetConstraint(Constraint.Horizontal, id);
        }

        private bool SetConstraint(Constraint constraint, int id)
        {
            if (lines[id].SetConstraint(constraint))
            {
                if (SetPointForVertexId(id, vertices[id].Point))
                    return true;
                if (SetPointForVertexId(NextId(id), vertices[NextId(id)].Point))
                    return true;
            }
            return false;
        }

        public bool MakeLineVertical(int id)
        {
            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;

            if (lines[prevId].Constraint == Constraint.Vertical
                || lines[nextId].Constraint == Constraint.Vertical)
                throw new InvalidOperationException("You can't add 2 consecutive vertical lines!");
            if (vertices[id].Constraint == Constraint.Angle
                || vertices[nextId].Constraint == Constraint.Angle)
                throw new InvalidOperationException("You can't add this constraint here!");
            return SetConstraint(Constraint.Vertical, id);
        }

        public bool SetPointForVertexId(int id, Point point)
        {
            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;

            if (!closed)
            {
                vertices[id].SetPoint(point);
                return true;
            }

            //--------- else -----------
            // angles
            var needFixing = GetVerticesToFix(id);
            var dx = point.X - vertices[id].X;
            var dy = point.Y - vertices[id].Y;
            for (int x = 0; x < verticesCount; x++)
            {
                if (needFixing[x])
                    vertices[x].MovePoint(dx, dy);
            }

            //var needFixingNext = GetVerticesToFix(NextId(id));
            //var needFixingPrev = GetVerticesToFix(PrevId(id));
            //var needFixingCombined = new bool[VerticesCount];
            //for (int i = 0; i < verticesCount; i++)
            //    needFixingCombined[i] = needFixing[i] | needFixingNext[i] | needFixingPrev[i];
  

            //if (lines[id].Constraint == Constraint.Vertical)
            //{
            //    if (!needFixingCombined[NextId(id)])
            //        vertices[NextId(id)].X += dx;
            //}
            //else if(lines[id].Constraint == Constraint.Horizontal)
            //{
            //    if (!needFixingCombined[NextId(id)])
            //        vertices[NextId(id)].Y += dy;
            //}
            //if (lines[PrevId(id)].Constraint == Constraint.Vertical)
            //{
            //    if (!needFixingCombined[PrevId(id)])
            //    {
            //        int i = id;
            //        do
            //        {
            //            i = PrevId(i);
            //            vertices[i].X += dx;
            //        } while (needFixingCombined[i] && i != id);
            //    }
            //}
            //else if (lines[PrevId(id)].Constraint == Constraint.Horizontal)
            //{
            //    if (!needFixingCombined[PrevId(id)])
            //        vertices[PrevId(id)].Y += dy;
            //}

            vertices[id].SetPoint(point);
            //clockwise lines
            var vertex = vertices[id];
            var line = lines[id];
            int i = nextId;
            while (i != id)
            {
                if (line.Constraint == Constraint.Horizontal)
                    vertices[i].Y = vertex.Y;
                else if (line.Constraint == Constraint.Vertical)
                    vertices[i].X = vertex.X;

                vertex = vertices[i];
                line = lines[i];
                i = (i + 1) % verticesCount;

            }

            ////counter-clockwise lines
            vertex = vertices[id];
            line = lines[prevId];
            i = prevId;
            while (i != id)
            {
                if (line.Constraint == Constraint.Horizontal)
                    vertices[i].Y = vertex.Y;
                else if (line.Constraint == Constraint.Vertical)
                    vertices[i].X = vertex.X;

                vertex = vertices[i];
                i = (i == 0) ? verticesCount - 1 : i - 1;
                line = lines[i];
            }

            return true;
        }

        private bool[] GetVerticesToFix(int start)
        {
            var needFixing = new bool[verticesCount];
            //needFixing[start] = true;

            // clockwise
            bool oneMore = vertices[start].Constraint == Constraint.Angle;
            var i = NextId(start);
            while (i != start)
            {
                if (vertices[i].Constraint == Constraint.Angle)
                {
                    needFixing[i] = true;
                    oneMore = true;
                }
                else if (oneMore)
                {
                    needFixing[i] = true;
                    oneMore = false;
                }
                else break;
                i = NextId(i);
            }

            //counter-clockwise
            oneMore = vertices[start].Constraint == Constraint.Angle;
            i = PrevId(start);
            while (i != start)
            {
                if (vertices[i].Constraint == Constraint.Angle)
                {
                    needFixing[i] = true;
                    oneMore = true;
                }
                else if (oneMore)
                {
                    needFixing[i] = true;
                    oneMore = false;
                }
                else break;
                i = PrevId(i);
            }
            return needFixing;
        }

        /// <summary>
        /// Makes the lines keep the constraints
        /// </summary>
        /// <param name="needFixing">result of GetVerticesToFix method</param>
        private void LineAlgorithm(bool[] needFixing)
        {
            for (int i = 0; i < verticesCount; i++)
            {
                if (needFixing[i])
                    continue;

            }
        }

        private double CalculateAngleABC(Vertex a, Vertex b, Vertex c)
        {
            double P1X = b.X, P1Y = b.Y, P2X = a.X, P2Y = a.Y, P3X = c.X, P3Y = c.Y;

            double numerator = P2Y * (P1X - P3X) + P1Y * (P3X - P2X) + P3Y * (P2X - P1X);
            double denominator = (P2X - P1X) * (P1X - P3X) + (P2Y - P1Y) * (P1Y - P3Y);
            double ratio = numerator / denominator;

            double angleRad = Math.Atan(ratio);
            double angleDeg = (angleRad * 180) / Math.PI;

            if (angleDeg < 0)
            {
                angleDeg = 180 + angleDeg;
            }

            return angleDeg;
        }

        internal void ClearVertexConstraints(int id)
        {
            vertices[id].AngleConstraint = null;
        }

        public void SetAngleConstraint(int id, int angle)
        {
            vertices[id].AngleConstraint = angle;
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
