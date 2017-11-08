using PolygonApp.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp
{
    class PolygonManager
    {
        private List<Polygon> _polygons = new List<Polygon>();
        private Polygon _currentPoly;
        private int _currentVertex;
        private int _vertexSize;
        private ManagerState _state;

        public PolygonManager(int vertexSize)
        {
            State = ManagerState.Create;
            VertexSize = vertexSize;
        }

        public List<Polygon> Polygons { get => _polygons; set => _polygons = value; }
        public int VertexSize { get => _vertexSize; private set => _vertexSize = value; }
        internal ManagerState State { get => _state; private set => _state = value; }

        public void AddVertex(Point location)
        {
            if (_currentPoly == null)
            {
                _currentPoly = new Polygon(VertexSize);
                _polygons.Add(_currentPoly);
            }
             _currentVertex = _currentPoly.AddVertex(location);
            if (_currentVertex == -1)
                State = ManagerState.Edit;
        }

        public void Draw(Bitmap canvas)
        {
            foreach (var p in _polygons)
                p.Draw(canvas);
        }

        public void SetVertexSize(int value)
        {
            VertexSize = value;
            foreach (var p in _polygons)
                p.VertexSize = value;
        }


        #region CurrentPolygon Methods

        internal void AddVertexToLine(int clickedLineId)
        {
            _currentPoly?.AddVertexToLine(clickedLineId);
        }

        internal void DeleteVertex(int clickedVertexId)
        {
            _currentPoly?.DeleteVertex(clickedVertexId);
        }

        internal void Fill()
        {
            _currentPoly?.FillPolygon();
        }
        #endregion


        private void Select(Point location)
        {
            foreach (var p in _polygons)
            {
                var i = p.GetVertexIdFromPoint(location);
                var j = p.GetLineIdFromPoint(location);
                if (i != -1 || j != -1)
                {
                    _currentPoly = p;
                    return;
                }
            }
            _currentPoly = null;
        }
    }

    enum ManagerState
    {
        Create,
        Edit
    }
}
