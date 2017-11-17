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
        private int _vertexSize = 15;
        private Color _lightColor;
        private (double X, double Y, double Z) _lightPosition;
        private Color _solidColor;
        private Bitmap _texture;
        private Bitmap _normalMap;
        private Bitmap _heightMap;
        private Color[][] _textureColors;
        private Color[][] _normalMapColors;
        private Color[][] _heightMapColors;
        private (int X, int Y) _normalMax;
        private (int X, int Y) _heightMax;
        private ManagerState _state;
        private FillType _fillType;
        private LightType _lightType;
        private int _heightMapFactor;
        private bool _spotlightRed;
        private bool _spotlightGreen;
        private bool _spotlightBlue;
        private bool _mainLight = true;
        private (int X, int Y) _redSpotlightPos = (400, 0);
        private (int X, int Y) _greenSpotlightPos = (800, 691);
        private (int X, int Y) _blueSpotlightPos = (0, 691);
        private (int X, int Y) _center = (400, 350);
        private Size _pictureBoxSize;

        public PolygonManager()
        {
            VertexSize = 15;
            SolidColor = Color.White;
        }

        #region Properties
        public int VertexSize
        {
            get => _vertexSize;
            private set
            {
                _vertexSize = value;
                foreach (var p in _polygons)
                    p.VertexSize = value;
            }
        }
        internal ManagerState State { get => _state; private set => _state = value; }
        public Color LightColor { get => _lightColor; set => _lightColor = value; }
        public (double X, double Y, double Z) LightPosition { get => _lightPosition; set => _lightPosition = value; }
        public Bitmap Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                if (value != null)
                {
                    _fillType = FillType.Texture;
                    _textureColors = new Color[_texture.Width][];
                    for (int i = 0; i < _texture.Width; i++)
                        _textureColors[i] = new Color[_texture.Height];

                    for (int y = 0; y < _texture.Height; y++)
                        for (int x = 0; x < _texture.Width; x++)
                            _textureColors[x][y] = _texture.GetPixel(x, y);
                }
                else
                    _fillType = FillType.Solid;
            }
        }
        public Color SolidColor { get => _solidColor; set => _solidColor = value; }
        public Bitmap NormalMap
        {
            get => _normalMap;
            set
            {
                _normalMap = value;
                if(value == null)
                {
                    _normalMax = (0, 0);
                    return;
                }
                _normalMax = (_normalMap.Width, _normalMap.Height);

                _normalMapColors = new Color[_normalMap.Width][];
                for (int i = 0; i < _normalMap.Width; i++)
                    _normalMapColors[i] = new Color[_normalMap.Height];

                for (int y = 0; y < _normalMap.Height; y++)
                    for (int x = 0; x < _normalMap.Width; x++)
                        _normalMapColors[x][y] = _normalMap.GetPixel(x, y);
            }
        }
        public FillType FillType { get => _fillType; set => _fillType = value; }
        public LightType LightType { get => _lightType; set => _lightType = value; }
        public Bitmap HeightMap
        {
            get => _heightMap;
            set
            {
                _heightMap = value;
                if (value == null)
                {
                    _heightMax = (0, 0);
                    return;
                }
                _heightMax = (_heightMap.Width, _heightMap.Height);

                _heightMapColors = new Color[_heightMap.Width][];
                for (int i = 0; i < _heightMap.Width; i++)
                    _heightMapColors[i] = new Color[_heightMap.Height];

                for (int y = 0; y < _heightMap.Height; y++)
                    for (int x = 0; x < _heightMap.Width; x++)
                        _heightMapColors[x][y] = _heightMap.GetPixel(x, y);
            }
        }

        public int HeightMapFactor { get => _heightMapFactor; set => _heightMapFactor = value; }
        public bool SpotlightRed { get => _spotlightRed; set => _spotlightRed = value; }
        public Size PictureBoxSize { get => _pictureBoxSize; set => _pictureBoxSize = value; }
        public bool SpotlightGreen { get => _spotlightGreen; set => _spotlightGreen = value; }
        public bool SpotlightBlue { get => _spotlightBlue; set => _spotlightBlue = value; }
        public bool MainLight { get => _mainLight; set => _mainLight = value; }
        #endregion

        public void Draw(Bitmap canvas)
        {
            IFillModule fillModule;
            switch (_fillType)
            {
                case FillType.Texture:
                    fillModule = new TextureFillModule(_textureColors, _texture.Width, _texture.Height);
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
                    fillModule = new PointLightFillModule(fillModule, _lightPosition, _normalMapColors, _normalMax, _heightMapColors, _heightMax, _heightMapFactor);
                    break;
                case LightType.Directional:
                default:
                    fillModule = new DirectionalLightFillModule(fillModule, _normalMapColors, _normalMax, _heightMapColors, _heightMax, _heightMapFactor);
                    break;
            }

            List<IFillModule> modules = new List<IFillModule>();
            if (SpotlightRed)
                modules.Add(new SpotlightFillModule(new SolidFillModule(_solidColor), _normalMapColors, _normalMax, _heightMapColors, _heightMax, _heightMapFactor, _redSpotlightPos,_center, Color.Red));
            if (SpotlightGreen)
                modules.Add(new SpotlightFillModule(new SolidFillModule(_solidColor), _normalMapColors, _normalMax, _heightMapColors, _heightMax, _heightMapFactor, _greenSpotlightPos, _center, Color.FromArgb(0,255,0)));
            if (SpotlightBlue)
                modules.Add(new SpotlightFillModule(new SolidFillModule(_solidColor), _normalMapColors, _normalMax, _heightMapColors, _heightMax, _heightMapFactor, _blueSpotlightPos, _center, Color.Blue));

            if (MainLight)
                modules.Add(fillModule);
            fillModule = new AdditionFillModule(modules.ToArray());

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

        public void AddPolygon(Polygon poly)
        {
            _polygons.Add(poly);
            _currentPoly = poly;
            State = ManagerState.Edit;
        }

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
