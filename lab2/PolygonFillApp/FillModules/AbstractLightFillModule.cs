using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    abstract class AbstractLightFillModule : IFillModule
    {
        protected IFillModule _baseModule;
        protected Bitmap _normalMap;
        protected Bitmap _heightMap;
        protected Color[][] _normalMapColors;
        protected Color[][] _heightMapColors;
        protected int _xNormalMax;
        protected int _yNormalMax;
        protected int _xHeightMax;
        protected int _yHeightMax;

        public AbstractLightFillModule(IFillModule baseModule, Bitmap normalMap, Bitmap heightMap)
        {
            _baseModule = baseModule;
            if (normalMap != null)
            {
                _normalMap = normalMap;
                _xNormalMax = normalMap.Width;
                _yNormalMax = normalMap.Height;

                _normalMapColors = new Color[_xNormalMax][];
                for (int i = 0; i < _xNormalMax; i++)
                    _normalMapColors[i] = new Color[_yNormalMax];

                for (int y = 0; y < _normalMap.Height; y++)
                    for (int x = 0; x < _normalMap.Width; x++)
                        _normalMapColors[x][y] = _normalMap.GetPixel(x, y);
            }
            if (heightMap != null)
            {
                _heightMap = heightMap;
                _xHeightMax = heightMap.Width;
                _yHeightMax = heightMap.Height;

                _heightMapColors = new Color[_xHeightMax][];
                for (int i = 0; i < _xHeightMax; i++)
                    _heightMapColors[i] = new Color[_yHeightMax];

                for (int y = 0; y < _heightMap.Height; y++)
                    for (int x = 0; x < _heightMap.Width; x++)
                        _heightMapColors[x][y] = _heightMap.GetPixel(x, y);
            }
        }

        abstract public Color GetColor(int x, int y);

        protected (double X, double Y, double Z) CreateDisplacementVector(int x, int y, (double X, double Y, double Z) normal)
        {

            var color = _heightMap.GetPixel(x % _xHeightMax, y % _yHeightMax);
            var xColor = _heightMap.GetPixel((x + 1) % _xHeightMax, y % _yHeightMax);
            var yColor = _heightMap.GetPixel(x % _xHeightMax, (y + 1) % _yHeightMax);
            (double X, double Y, double Z) dhx = ((xColor.R - color.R) / 255.0, (xColor.G - color.G) / 255.0, (xColor.B - color.B) / 255.0);
            (double X, double Y, double Z) dhy = ((yColor.R - color.R) / 255.0, (yColor.G - color.G) / 255.0, (yColor.B - color.B) / 255.0);

            (double X, double Y, double Z) t = (1.0, 0.0, -normal.X);
            (double X, double Y, double Z) b = (0.0, 1.0, -normal.Y);

            return (t.X * dhx.X + b.X * dhy.X, t.Y * dhx.Y + b.Y * dhy.Y, t.Z * dhx.Z + b.Z * dhy.Z);
        }

        protected Color GetNormalMapColor(int x, int y)
        {
            return _normalMapColors[x % _xNormalMax][y % _yNormalMax];
        }

        protected (double X, double Y, double Z) CreateNormalVector(Color color)
        {
            var x = (color.R - 127) / 128.0;
            var y = (color.G - 127) / 128.0;
            var z = (color.B) / 255.0;

            var f = 1.0 / z;
            z = 1.0;
            y *= f;
            x *= f;

            return (x, y, z);
        }

        protected (double, double, double) CreateLightVector(Color color)
        {
            var x = color.R / 255.0;
            var y = color.G / 255.0;
            var z = color.B / 255.0;

            return (x, y, z);
        }

        protected double VectorLength((double X, double Y, double Z) vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        }
    }
}
