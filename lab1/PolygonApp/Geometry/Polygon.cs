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
            _lines[id].Constraint = Constraint.None;
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
            _lines[prevId].Constraint = Constraint.None;
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

        public bool MakeLineHorizontal(int id)
        {
            if (_lines[PrevId(id)].Constraint == Constraint.Horizontal
                || _lines[NextId(id)].Constraint == Constraint.Horizontal)
                throw new InvalidOperationException("You can't add 2 consecutive horizontal lines!");
            if (_lines[id].Constraint == Constraint.Horizontal)
                return false;

            if (_vertices[id].Constraint == Constraint.None
                 && _vertices[PrevId(id)].Constraint == Constraint.None)
            {
                _vertices[id].Y = _vertices[NextId(id)].Y;
                _lines[id].Constraint = Constraint.Horizontal;
            }
            else if (_vertices[NextId(id)].Constraint == Constraint.None
                 && _vertices[NextId(NextId(id))].Constraint == Constraint.None)
            {
                _vertices[NextId(id)].Y = _vertices[id].Y;
                _lines[id].Constraint = Constraint.Horizontal;
            }
            else throw new InvalidOperationException("You can't add this constraint here!");

            return true;
        }

        public bool MakeLineVertical(int id)
        {
            if (_lines[PrevId(id)].Constraint == Constraint.Vertical
                || _lines[NextId(id)].Constraint == Constraint.Vertical)
                throw new InvalidOperationException("You can't add 2 consecutive vertical lines!");
            if (_lines[id].Constraint == Constraint.Vertical)
                return false;

            if (_vertices[id].Constraint == Constraint.None
                 && _vertices[PrevId(id)].Constraint == Constraint.None)
            {
                _vertices[id].X = _vertices[NextId(id)].X;
                _lines[id].Constraint = Constraint.Vertical;
            }
            else if (_vertices[NextId(id)].Constraint == Constraint.None
                 && _vertices[NextId(NextId(id))].Constraint == Constraint.None)
            {
                _vertices[NextId(id)].X = _vertices[id].X;
                _lines[id].Constraint = Constraint.Vertical;
            }
            else throw new InvalidOperationException("You can't add this constraint here!");

            return true;
        }

        public void MoveLine(int id, Point location)
        {
            var dx = location.X - _lines[id].LastClickPoint.X;
            var dy = location.Y - _lines[id].LastClickPoint.Y;

            var nextPoint = new Point(_vertices[NextId(id)].Center.X, _vertices[NextId(id)].Center.Y);
            SetPointForVertexId(id, new Point(_vertices[id].Center.X + dx, _vertices[id].Center.Y + dy));
            if (nextPoint == new Point(_vertices[NextId(id)].Center.X, _vertices[NextId(id)].Center.Y))
                SetPointForVertexId(NextId(id), new Point(_vertices[NextId(id)].Center.X + dx, _vertices[NextId(id)].Center.Y + dy));

            _lines[id].LastClickPoint = location;
        }

        public void MovePolygon(Point point)
        {
            var dx = point.X - _center.X;
            var dy = point.Y - _center.Y;
            for (int i = 0; i < _verticesCount; i++)
                _vertices[i].MovePoint(dx, dy);
            _center.X = point.X;
            _center.Y = point.Y;
        }

        public void SetPointForVertexId(int id, Point point)
        {
            var prevId = (id == 0) ? _verticesCount - 1 : id - 1;
            var nextId = (id + 1) % _verticesCount;

            if (!_closed)
            {
                _vertices[id].SetPoint(point);
                return;
            }

            var needFixing = GetVerticesToFix(id);
            var dx = point.X - _vertices[id].X;
            var dy = point.Y - _vertices[id].Y;
            for (int x = 0; x < _verticesCount; x++)
            {
                if (needFixing[x])
                    _vertices[x].MovePoint(dx, dy);
            }
            _vertices[id].SetPoint(point);
        }
        #endregion

        #region Public Methods TOFIX
        public void MoveLineAlongVectors(int id, Point location)
        {
            var dx = location.X - _lines[id].LastClickPoint.X;
            var dy = location.Y - _lines[id].LastClickPoint.Y;

            var A = _vertices[id];
            var B = _vertices[PrevId(id)];
            var C = _vertices[NextId(id)];
            var D = _vertices[NextId(NextId(id))];

            if(A.X == C.X)
            {
                MakeLineVertical(id);
                return;
            }
            else if (A.Y == C.Y)
            {
                MakeLineHorizontal(id);
                return;
            }

            var slopeAB = (B.Y - A.Y) / (double)(B.X - A.X);
            var slopeCD = (D.Y - C.Y) / (double)(D.X - C.X);
            var slope = (C.Y - A.Y) / (double)(C.X - A.X);
            if (double.IsInfinity(slopeAB) || double.IsInfinity(slopeCD))
                return;

            if(Math.Abs(slope) < 0.5)
            {
                A.X += (int)(dy / slopeAB);
                A.Y += dy;

                C.X += (int)(dy / slopeCD);
                C.Y += dy;
            }
            else
            {
                A.X += dx;
                A.Y += (int)(dx * slopeAB);

                var oldCX = C.X;
                C.X = (int)((C.X * slopeCD - A.X * slope + A.Y - C.Y) / (slopeCD - slope));
                C.Y += (int)((C.X - oldCX)* slopeCD);
            }

            _lines[id].LastClickPoint = location;
        }

        public void SetAngleConstraint(int id, int angle)
        {
            //var v = _vertices[id];
            //var vPrev = _vertices[PrevId(id)];
            //var vNext = _vertices[NextId(id)];
            ////var original = CalculateAngleABC(vPrev, v, vNext);
            ////double phi = angle - (int)original;
            //double phi = angle;
            //var px = vNext.X;
            //var py = vNext.Y;
            //var ox = v.X;
            //var oy = v.Y;
            //var dx = px - ox;
            //var dy = py - oy;

            //var rad = (Math.PI / 180.0) * phi;
            //vNext.X = (int)(dx * Math.Cos(rad) - dy * Math.Sin(rad) + ox);
            //vNext.Y = (int)(dx * Math.Cos(rad) + dy * Math.Sin(rad) + oy);
            _vertices[id].AngleConstraint = angle;
        }
        #endregion

        #region Private Methods
        private bool[] GetVerticesToFix(int start)
        {
            var result = new bool[VerticesCount];
            var i = start;
            do
            {
                if (_vertices[i].Constraint != Constraint.None
                    || _vertices[PrevId(i)].Constraint != Constraint.None
                    || _lines[PrevId(i)].Constraint != Constraint.None)
                {
                    i = PrevId(i);
                    result[i] = true;
                }
                else break;
            } while (i != start);

            i = start;
            do
            {
                if (_vertices[i].Constraint != Constraint.None
                    || _vertices[NextId(i)].Constraint != Constraint.None
                    || _lines[i].Constraint != Constraint.None)
                {
                    i = NextId(i);
                    result[i] = true;
                }
                else break;
            } while (i != start);

            return result;
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
