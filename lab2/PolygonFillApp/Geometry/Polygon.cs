﻿using PolygonApp.Algorithms;
using System;
using System.Drawing;
using System.Collections.Generic;
using PolygonApp.FillModules;

namespace PolygonApp.Geometry
{
    class Polygon
    {
        private const int _maxVertices = 20;
        private const int _lineProximity = 500;
        private const int _vertexProximity = 15;
        private int _verticesCount = 0;
        private bool _closed = false;
        private bool _filled = false;
        private int _vertexSize = 15;
        private Point _center;
        private Vertex[] _vertices;
        private Edge[] _edges;

        public Polygon(int vertexSize)
        {
            _vertices = new Vertex[_maxVertices];
            _edges = new Edge[_maxVertices];
            _vertexSize = vertexSize;
        }

        public Polygon(List<Vertex> vertices)
        {          
            _vertices = new Vertex[_maxVertices];
            _edges = new Edge[_maxVertices];

            int i = 0;
            foreach (var v in vertices)
            {
                _vertices[i++] = v;
                if (i == _maxVertices)
                    break;
            }
            _verticesCount = i;
            for (i = 0; i < _verticesCount; i++)
            {
                int j = (i + 1) % _verticesCount;
                _edges[i] = new Edge(_vertices[i], _vertices[j]);
            }
            _closed = true;
        }

        #region Properties
        public Point Center { get => _center; set => _center = value; }
        public Edge[] Edges { get => _edges; }
        public Vertex[] Vertices { get => _vertices; }
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
        public bool Filled { get => _filled; set => _filled = value; }
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

            Edge line = new Edge(_vertices[_verticesCount - 1], vertex);
            _edges[_verticesCount - 1] = line;

            _verticesCount++;
            return _verticesCount - 1;
        }

        public void AddVertexToLine(int id)
        {
            if (VerticesCount == _maxVertices) return;

            var oldLine = _edges[id];
            var x = (oldLine.End.X + oldLine.Start.X) / 2;
            var y = (oldLine.End.Y + oldLine.Start.Y) / 2;
            var middle = new Point(x, y);

            for (int i = _verticesCount; i > id + 1; i--)
            {
                _vertices[i] = _vertices[i - 1];
                _edges[i] = _edges[i - 1];
            }

            // vertices[id+1] and lines[id+1] are now null

            var vertex = new Vertex(middle, VertexSize);
            _vertices[id + 1] = vertex;
            var line = new Edge(vertex, oldLine.End);
            oldLine.End = vertex;
            _edges[id + 1] = line;

            _verticesCount++;
        }

        public void Close()
        {
            if (_verticesCount == _maxVertices)
            {
                _edges[_verticesCount - 1] = new Edge(_vertices[_verticesCount - 1], _vertices[0]);
            }
            else
            {
                _verticesCount--;
                _vertices[_verticesCount] = null;
                _edges[_verticesCount - 1] = new Edge(_vertices[_verticesCount - 1], _vertices[0]);
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
            _edges[prevId].End = _edges[id].End;
            _edges[id] = null;
            RearrangeLines();

            _verticesCount--;

        }

        public void Draw(Bitmap canvas, IFillModule fillModule)
        {
            if (_filled)
                PolygonFill.Fill(this, canvas, fillModule);

            if (_verticesCount > 1)
                for (int i = 0; i < _verticesCount - 1; i++)
                    _edges[i].Draw(canvas);
            if (_closed)
                _edges[_verticesCount - 1].Draw(canvas);
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
                var dist = _edges[i].GetSquaredDistanceFromPoint(point);
                if (dist < minimumDistance)
                {
                    minimumDistance = dist;
                    bestId = i;
                }
            }
            if (minimumDistance < _lineProximity)
            {
                _edges[bestId].LastClickPoint = point;
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
            var dx = location.X - _edges[id].LastClickPoint.X;
            var dy = location.Y - _edges[id].LastClickPoint.Y;

            var nextPoint = new Point(_vertices[NextId(id)].Center.X, _vertices[NextId(id)].Center.Y);
            SetPointForVertexId(id, new Point(_vertices[id].Center.X + dx, _vertices[id].Center.Y + dy));
            if (nextPoint == new Point(_vertices[NextId(id)].Center.X, _vertices[NextId(id)].Center.Y))
                SetPointForVertexId(NextId(id), new Point(_vertices[NextId(id)].Center.X + dx, _vertices[NextId(id)].Center.Y + dy));

            _edges[id].LastClickPoint = location;
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

        public void FillPolygon()
        {
            Filled = true;
        }

        public void SetPointForVertexId(int id, Point point)
        {
            if (!_closed)
            {
                _vertices[id].SetPoint(point);
                return;
            }
            _vertices[id].SetPoint(point);
        }
        #endregion

        #region Private Methods
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
                if (_edges[i] == null && i + 1 < _verticesCount)
                {
                    _edges[i] = _edges[i + 1];
                    _edges[i + 1] = null;
                }
            }
            if (_verticesCount == 3)
            {
                int id = 0;
                if (_edges[1] != null)
                    id = 1;
                else if (_edges[2] != null)
                    id = 2;
                if (id != 0)
                {
                    _edges[0] = _edges[id];
                    _edges[id] = null;
                }
                _edges[0].Start = _vertices[0];
                _edges[0].End = _vertices[1];
            }
        }
        private int PrevId(int id) { return (id == 0) ? _verticesCount - 1 : id - 1; }
        private int NextId(int id) { return (id + 1) % _verticesCount; }
        #endregion
    }
}
