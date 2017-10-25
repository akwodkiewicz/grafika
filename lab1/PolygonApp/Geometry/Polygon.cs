using System;
using System.Drawing;

using PolygonApp.Commons;

namespace PolygonApp.Geometry
{
    class Polygon : IDrawable
    {
        private const int _maxVertices = 20;
        private const int _lineProximity = 500;
        private const int _vertexProximity = 15;
        private int _verticesCount = 0;
        private bool _closed = false;
        private int _vertexSize;
        private Point _center;
        private Vertex[] _vertices;
        private Line[] _lines;

        public Polygon(int vertexSize)
        {
            _vertices = new Vertex[_maxVertices];
            _lines = new Line[_maxVertices];
            _vertexSize = vertexSize;
        }

        #region Properties
        public Point Center { get => _center; set => _center = value; }
        public int VertexSize
        {
            get => _vertexSize;
            set
            {
                _vertexSize = value;
                for (int i = 0; i < _verticesCount; i++)
                    _vertices[i].Size = _vertexSize;
            }
        }
        public int VerticesCount { get => _verticesCount; }
        #endregion

        #region Public Methods
        public int AddVertex(Point point)
        {
            Vertex vertex;
            if (_verticesCount == _maxVertices
                || _verticesCount > 0 && _vertices[0].Contains(point, _vertexProximity))
            {
                Close();
                return -1;
            }
            if (_verticesCount == 0)
            {
                vertex = new Vertex(point, VertexSize);
                _vertices[_verticesCount++] = vertex;
            }
            vertex = new Vertex(point, VertexSize);
            _vertices[_verticesCount] = vertex;

            Line line = new Line(_vertices[_verticesCount - 1], vertex);
            _lines[_verticesCount - 1] = line;

            _verticesCount++;
            return _verticesCount - 1;
        }

        public void AddVertexToLine(int id)
        {
            if (VerticesCount == _maxVertices) return;

            var oldLine = _lines[id];
            var x = (oldLine.End.X + oldLine.Start.X) / 2;
            var y = (oldLine.End.Y + oldLine.Start.Y) / 2;
            var middle = new Point(x, y);

            for (int i = _verticesCount; i > id + 1; i--)
            {
                _vertices[i] = _vertices[i - 1];
                _lines[i] = _lines[i - 1];
            }

            // vertices[id+1] and lines[id+1] are now null

            var vertex = new Vertex(middle, VertexSize);
            _vertices[id + 1] = vertex;
            var line = new Line(vertex, oldLine.End);
            oldLine.End = vertex;
            _lines[id + 1] = line;

            _verticesCount++;
        }

        public int CalculateAngleForVertexId(int id)
        {
            return (int)CalculateAngleABC(_vertices[PrevId(id)], _vertices[id], _vertices[NextId(id)]);
        }

        public void ClearLineConstraints(int id)
        {
            _lines[id].SetConstraint(Constraint.None);
        }

        public void ClearVertexConstraints(int id)
        {
            _vertices[id].AngleConstraint = null;
        }

        public void Close()
        {
            if (_verticesCount == _maxVertices)
            {
                _lines[_verticesCount - 1] = new Line(_vertices[_verticesCount - 1], _vertices[0]);
            }
            else
            {
                _verticesCount--;
                _vertices[_verticesCount] = null;
                _lines[_verticesCount - 1] = new Line(_vertices[_verticesCount - 1], _vertices[0]);
            }
            _closed = true;
        }

        public void DeleteVertex(int id)
        {
            if (_verticesCount == 3)
                throw new InvalidOperationException();

            _vertices[id] = null;
            RearrangeVertices();

            var prevId = (id == 0) ? _verticesCount - 1 : id - 1;
            _lines[prevId].SetConstraint(Constraint.None);
            _lines[prevId].End = _lines[id].End;
            _lines[id] = null;
            RearrangeLines();

            _verticesCount--;

        }

        public void Draw(Bitmap canvas)
        {
            if (_verticesCount > 1)
                for (int i = 0; i < _verticesCount - 1; i++)
                    _lines[i].Draw(canvas);
            if (_closed)
                _lines[_verticesCount - 1].Draw(canvas);
            for (int i = 0; i < _verticesCount; i++)
                _vertices[i].Draw(canvas);
        }

        public int GetLineIdFromPoint(Point point)
        {
            var iMax = _closed ? _verticesCount : _verticesCount - 1;
            double minimumDistance = double.PositiveInfinity;
            int bestId = -1;
            for (int i = 0; i < iMax; i++)
            {
                var dist = _lines[i].GetSquaredDistanceFromPoint(point);
                if (dist < minimumDistance)
                {
                    minimumDistance = dist;
                    bestId = i;
                }
            }
            if (minimumDistance < _lineProximity)
            {
                _lines[bestId].LastClickPoint = point;
                return bestId;
            }
            else
                return -1;
        }

        public int GetVertexIdFromPoint(Point point)
        {
            for (int i = 0; i < _verticesCount; i++)
                if (_vertices[i].Contains(point, _vertexProximity))
                    return i;

            return -1;
        }

        public void MoveLine(int id, Point location)
        {
            var dx = location.X - _lines[id].LastClickPoint.X;
            var dy = location.Y - _lines[id].LastClickPoint.Y;

            var nextPoint = new Point(_vertices[NextId(id)].Center.X, _vertices[NextId(id)].Center.Y);
            SetPointForVertexId(id, new Point(_vertices[id].Center.X + dx, _vertices[id].Center.Y + dy));
            if (nextPoint == new Point(_vertices[NextId(id)].Center.X, _vertices[NextId(id)].Center.Y))
                SetPointForVertexId(NextId(id), new Point(_vertices[NextId(id)].Center.X + dx, _vertices[NextId(id)].Center.Y + dy));

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
            _lines[id].LastClickPoint = location;
        }

        public void MovePolygon(Point point)
        {
            var dx = point.X - _center.X;
            var dy = point.Y - _center.Y;
            for (int i = 0; i < _verticesCount; i++)
            {
                var p = new Point(_vertices[i].X + dx, _vertices[i].Y + dy);
                _vertices[i].SetPoint(p);
            }
            _center.X = point.X;
            _center.Y = point.Y;
        }

        public bool MakeLineHorizontal(int id)
        {
            var prevId = (id == 0) ? _verticesCount - 1 : id - 1;
            var nextId = (id + 1) % _verticesCount;

            if (_lines[prevId].Constraint == Constraint.Horizontal
                || _lines[nextId].Constraint == Constraint.Horizontal
                )
                throw new InvalidOperationException("You can't add 2 consecutive horizontal lines!");
            if (_vertices[id].Constraint == Constraint.Angle
                || _vertices[nextId].Constraint == Constraint.Angle)
                throw new InvalidOperationException("You can't add this constraint here!");
            return SetConstraint(Constraint.Horizontal, id);
        }

        public bool MakeLineVertical(int id)
        {
            var prevId = (id == 0) ? _verticesCount - 1 : id - 1;
            var nextId = (id + 1) % _verticesCount;

            if (_lines[prevId].Constraint == Constraint.Vertical
                || _lines[nextId].Constraint == Constraint.Vertical)
                throw new InvalidOperationException("You can't add 2 consecutive vertical lines!");
            if (_vertices[id].Constraint == Constraint.Angle
                || _vertices[nextId].Constraint == Constraint.Angle)
                throw new InvalidOperationException("You can't add this constraint here!");
            return SetConstraint(Constraint.Vertical, id);
        }

        public void SetAngleConstraint(int id, int angle)
        {
            _vertices[id].AngleConstraint = angle;
        }

        public bool SetPointForVertexId(int id, Point point)
        {
            var prevId = (id == 0) ? _verticesCount - 1 : id - 1;
            var nextId = (id + 1) % _verticesCount;

            if (!_closed)
            {
                _vertices[id].SetPoint(point);
                return true;
            }

            //--------- else -----------
            // angles
            var needFixing = GetVerticesToFix(id);
            var dx = point.X - _vertices[id].X;
            var dy = point.Y - _vertices[id].Y;
            for (int x = 0; x < _verticesCount; x++)
            {
                if (needFixing[x])
                    _vertices[x].MovePoint(dx, dy);
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

            _vertices[id].SetPoint(point);
            //clockwise lines
            var vertex = _vertices[id];
            var line = _lines[id];
            int i = nextId;
            while (i != id)
            {
                if (line.Constraint == Constraint.Horizontal)
                    _vertices[i].Y = vertex.Y;
                else if (line.Constraint == Constraint.Vertical)
                    _vertices[i].X = vertex.X;

                vertex = _vertices[i];
                line = _lines[i];
                i = (i + 1) % _verticesCount;

            }

            ////counter-clockwise lines
            vertex = _vertices[id];
            line = _lines[prevId];
            i = prevId;
            while (i != id)
            {
                if (line.Constraint == Constraint.Horizontal)
                    _vertices[i].Y = vertex.Y;
                else if (line.Constraint == Constraint.Vertical)
                    _vertices[i].X = vertex.X;

                vertex = _vertices[i];
                i = (i == 0) ? _verticesCount - 1 : i - 1;
                line = _lines[i];
            }

            return true;
        }
        #endregion

        #region Private Methods
        private bool[] GetVerticesToFix(int start)
        {
            var needFixing = new bool[_verticesCount];
            //needFixing[start] = true;

            // clockwise
            bool oneMore = _vertices[start].Constraint == Constraint.Angle;
            var i = NextId(start);
            while (i != start)
            {
                if (_vertices[i].Constraint == Constraint.Angle)
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
            oneMore = _vertices[start].Constraint == Constraint.Angle;
            i = PrevId(start);
            while (i != start)
            {
                if (_vertices[i].Constraint == Constraint.Angle)
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

        private bool SetConstraint(Constraint constraint, int id)
        {
            if (_lines[id].SetConstraint(constraint))
            {
                if (SetPointForVertexId(id, _vertices[id].Center))
                    return true;
                if (SetPointForVertexId(NextId(id), _vertices[NextId(id)].Center))
                    return true;
            }
            return false;
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

        private void RearrangeVertices()
        {
            for (int i = 0; i < _verticesCount; i++)
            {
                if (_vertices[i] == null && i + 1 < _verticesCount)
                {
                    _vertices[i] = _vertices[i + 1];
                    _vertices[i + 1] = null;
                }
            }
        }

        private void RearrangeLines()
        {
            for (int i = 0; i < _verticesCount; i++)
            {
                if (_lines[i] == null && i + 1 < _verticesCount)
                {
                    _lines[i] = _lines[i + 1];
                    _lines[i + 1] = null;
                }
            }
            if (_verticesCount == 3)
            {
                int id = 0;
                if (_lines[1] != null)
                    id = 1;
                else if (_lines[2] != null)
                    id = 2;
                if (id != 0)
                {
                    _lines[0] = _lines[id];
                    _lines[id] = null;
                }
                _lines[0].Start = _vertices[0];
                _lines[0].End = _vertices[1];
            }
        }

        private int PrevId(int id) { return (id == 0) ? _verticesCount - 1 : id - 1; }
        private int NextId(int id) { return (id + 1) % _verticesCount; }
        #endregion
    }
}
