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
            if(nextPoint == new Point(vertices[NextId(id)].Point.X, vertices[NextId(id)].Point.Y))
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
            var result = lines[id].SetConstraint(constraint);
            //if (!result) return result;
            //Vertex firstone, secondone, thirdone;
            //Tuple<double, double> vector;
            //double dx, dy;
            //if (vertices[NextId(NextId(id))].Constraint == Constraint.Angle 
            //    || vertices[PrevId(id)].Constraint == Constraint.Angle)
            //    throw new InvalidOperationException("You can't add this constraint here!");

            ////if (vertices[NextId(NextId(id))].Constraint == Constraint.Angle)
            ////{
            ////    firstone = vertices[NextId(id)];
            ////    secondone = vertices[NextId(NextId(id))];
            ////    thirdone = vertices[id];
            ////}
            ////else if (vertices[PrevId(id)] == Constraint.Angle)
            ////{
            ////    firstone = vertices[id];
            ////    secondone = vertices[PrevId(id)];
            ////    thirdone = vertices[NextId(id)];
            ////}
            ////vector = new Tuple<double, double>(secondone.X - firstone.X, secondone.Y - firstone.X);
            ////if (constraint == Constraint.Vertical)
            ////{
            ////    dx = thirdone.X - firstone.X;
            ////    var d = dx / vector.Item1;
            ////    dy = vector.Item2 * d;
            ////}
            ////else
            ////{
            ////    dy = vertices[id].Y - vertices[NextId(id)].Y;
            ////    var d = dy / vector.Item2;
            ////    dx = vector.Item1 * d;
            ////}
            ////vertices[NextId(id)].MovePoint((int)dx, (int)dy);

            //if (constraint == Constraint.Horizontal)
            //    SetPointForVertexId(id vertices[NextId(id)].Y;
            //else
            //    vertices[id].X = vertices[NextId(id)].X;
            return result;
           
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

        public void SetPointForVertexId(int id, Point point)
        {
            SetPointForVertexId(id, new PointC(point));
        }

        public void SetPointForVertexId(int id, PointC point)
        {
            var prevId = (id == 0) ? verticesCount - 1 : id - 1;
            var nextId = (id + 1) % verticesCount;

            if (!closed)
            {
                vertices[id].SetPoint(point);
                return;
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

            //var A = vertices[nextId];
            //var B = vertices[id];
            //var C = vertices[prevId];


            //// 
            //if (B.Constraint == Constraint.Angle)
            //{
            //    var vBA = new Tuple<double, double>(A.X - B.X, A.Y - B.Y);
            //    var vBC = new Tuple<double, double>(C.X - B.X, C.Y - B.Y);
            //    var vDelta = new Tuple<double, double>(dx, dy);

            //    var BAover = vBA.Item1 / vBA.Item2;
            //    var BCover = vBC.Item1 / vBC.Item2;
            //    var Deltaover = vDelta.Item1 / vDelta.Item2;

            //    //move along BA vector
            //    if (Math.Abs(BAover - Deltaover) < 0.005 || double.IsInfinity(BAover) && double.IsInfinity(Deltaover))
            //    {
            //        C.MovePoint(dx, dy);
            //    }
            //    else if (Math.Abs(BCover - Deltaover) < 0.005 || double.IsInfinity(BCover) && double.IsInfinity(Deltaover))
            //    {
            //        A.MovePoint(dx, dy);
            //    }
            //    else
            //    {
            //        A.MovePoint(dx, dy);
            //        C.MovePoint(dx, dy);
            //    }
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

            //counter-clockwise lines
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
        }

        private bool[] GetVerticesToFix(int start)
        {
            var needFixing = new bool[verticesCount];
            needFixing[start] = true;

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
