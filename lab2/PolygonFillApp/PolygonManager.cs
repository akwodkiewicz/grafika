using PolygonApp.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PolygonApp.Algorithms;
using PolygonApp.FillModules;

namespace PolygonApp
{
    class PolygonManager
    {
        private List<Polygon> _polygons = new List<Polygon>();
        private Polygon _currentPoly;
        private int _currentVertex;
        private int _currentLine;
        private int _vertexSize;
        private Color _lightColor;
        private (double X, double Y, double Z) _lightPosition;
        private Color _solidColor;
        private Bitmap _texture;
        private Bitmap _normalMap;
        private ManagerState _state;
        private FillType _fillType;
        private LightType _lightType;

        public PolygonManager()
        {
            VertexSize = 15;
            SolidColor = Color.White;
            StartCreating();
        }

        #region Properties
        public int VertexSize
        {
            get => _vertexSize;
            set
            {
                _vertexSize = value;
                foreach (var p in _polygons)
                    p.VertexSize = value;
            }
        }
        internal ManagerState State { get => _state; private set => _state = value; }
        public Color LightColor { get => _lightColor; set => _lightColor = value; }
        public (double X, double Y, double Z) LightPosition { get => _lightPosition; set => _lightPosition = value; }
        public Bitmap Texture { get => _texture; set => _texture = value;  }
        public Color SolidColor { get => _solidColor; set => _solidColor = value;  }
        public Bitmap NormalMap { get => _normalMap; set => _normalMap = value; }
        public FillType FillType { get => _fillType; set => _fillType = value; }
        public LightType LightType { get => _lightType; set => _lightType = value; }
        #endregion

        public void Draw(Bitmap canvas)
        {
            IFillModule fillModule;
            switch (_fillType)
            {
                case FillType.Texture:
                    fillModule = new TextureFillModule(_texture);
                    break;
                case FillType.Solid:
                default:
                    fillModule = new SolidFillModule(_solidColor);
                    break;
            }
            fillModule = new LightColorFillModule(fillModule, _lightColor);
            switch (_lightType)
            {
                case LightType.Point:
                    fillModule = new PointLightFillModule(fillModule, _lightPosition, _normalMap);
                    break;
                case LightType.Directional:
                default:
                    fillModule = new DirectionalLightFillModule(fillModule, _normalMap);
                    break;
            }

            foreach (var p in _polygons)
                p.Draw(canvas, fillModule);
        }

        #region ReadyState
        public void StartCreating()
        {
            State = ManagerState.Ready;
            _currentPoly = new Polygon(VertexSize);
            _polygons.Add(_currentPoly);
        }
        #endregion

        public void AddVertex(Point location)
        {
            State = ManagerState.Creating;
            _currentVertex = _currentPoly.AddVertex(location);
            if (_currentVertex == -1)
                State = ManagerState.Edit;
        }

        #region CreatingState
        public void StopCreating()
        {
            _currentPoly.Close();
            _currentPoly = null;
            _currentLine = -1;
            _currentVertex = -1;
            State = ManagerState.Edit;
        }

        public void MoveVertex(Point location)
        {
            _currentPoly?.SetPointForVertexId(_currentVertex, location);
        }
        #endregion

        #region EditState
        #region Movement
        public void StartMove(Point location)
        {
            PrepareInternalState(location);
            if (_currentPoly == null)
                return;
            _currentPoly.Center = location;
        }

        public void MovePolygon(Point location)
        {
            _currentPoly?.MovePolygon(location);
        }

        public void MoveVertexOrLine(Point location)
        {
            if (_currentVertex != -1)
                _currentPoly.SetPointForVertexId(_currentVertex, location);
            else if (_currentLine != -1)
                _currentPoly.MoveLine(_currentLine, location);
        }

        public void StopMove()
        {
            _currentPoly = null;
            _currentLine = -1;
            _currentVertex = -1;
        }
        #endregion

        public SelectResult Select(Point location)
        {
            PrepareInternalState(location);

            if (_currentVertex != -1)
                return SelectResult.Vertex;
            if (_currentLine != -1)
                return SelectResult.Line;
            return SelectResult.None;
        }

        public void AddVertexToLine()
        {
            _currentPoly?.AddVertexToLine(_currentLine);
        }

        public void DeleteVertex()
        {
            _currentPoly?.DeleteVertex(_currentVertex);
            _currentVertex = -1;
        }

        public void Fill()
        {
            _currentPoly?.FillPolygon();
        }

        public void Clip()
        {
            if (_polygons.Count < 2)
                throw new InvalidOperationException("There is no clipping polygon!");

            var result = PolygonClipping.SutherlandHodgman(_polygons[0], _polygons[1]);
            if (result == null)
                throw new InvalidOperationException("There is no clipping polygon!");

            _polygons.RemoveAt(1);
            _polygons.RemoveAt(0);
            _polygons.Add(result);
        }
        #endregion

        private void PrepareInternalState(Point location)
        {
            foreach (var p in _polygons)
            {
                var i = p.GetVertexIdFromPoint(location);
                var j = p.GetLineIdFromPoint(location);
                if (i != -1 || j != -1)
                {
                    _currentPoly = p;
                    if (i != -1) _currentVertex = i;
                    else if (j != -1) _currentLine = j;
                    return;
                }
            }
            _currentVertex = -1;
            _currentLine = -1;
            _currentPoly = null;
        }
    }

    enum ManagerState
    {
        Ready,
        Creating,
        Edit
    }

    enum SelectResult
    {
        Vertex,
        Line,
        None
    }

    enum FillType
    {
        Solid,
        Texture
    }

    enum LightType
    {
        Directional,
        Point
    }
}
