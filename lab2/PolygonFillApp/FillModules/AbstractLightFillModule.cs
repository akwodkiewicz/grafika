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
        protected Color[][] _normalMapColors;
        protected Color[][] _heightMapColors;
        protected int _xNormalMax;
        protected int _yNormalMax;
        protected int _xHeightMax;
        protected int _yHeightMax;
        protected int _heightMapFactor;

        public AbstractLightFillModule(IFillModule baseModule, Color[][] normalMapColors, (int X, int Y) normalMax, 
            Color[][] heightMapColors, (int X, int Y) heightMax, int heightMapFactor)
        {
            _baseModule = baseModule;

            _normalMapColors = normalMapColors;
            _xNormalMax = normalMax.X;
            _yNormalMax = normalMax.Y;

            _heightMapColors = heightMapColors;
            _xHeightMax = heightMax.X;
            _yHeightMax = heightMax.Y;

            _heightMapFactor = heightMapFactor;
        }

        abstract public Color GetColor(int x, int y);

        protected (double X, double Y, double Z) CreateDisplacementVector(int x, int y, (double X, double Y, double Z) normal)
        {
            var color = _heightMapColors[x % _xHeightMax][y % _yHeightMax];
            var xColor = _heightMapColors[(x + 1) % _xHeightMax][y % _yHeightMax];
            var yColor = _heightMapColors[x % _xHeightMax][(y + 1) % _yHeightMax];
            (double X, double Y, double Z) dhx = ((xColor.R - color.R) / 255.0, (xColor.G - color.G) / 255.0, (xColor.B - color.B) / 255.0);
            (double X, double Y, double Z) dhy = ((yColor.R - color.R) / 255.0, (yColor.G - color.G) / 255.0, (yColor.B - color.B) / 255.0);

            (double X, double Y, double Z) t = (1.0, 0.0, -normal.X);
            (double X, double Y, double Z) b = (0.0, 1.0, -normal.Y);

            return (_heightMapFactor*(t.X * dhx.X + b.X * dhy.X),_heightMapFactor*( t.Y * dhx.Y + b.Y * dhy.Y),_heightMapFactor*( t.Z * dhx.Z + b.Z * dhy.Z));
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

            if (z == 0)
            {
                x = 1.0;
                y = 1.0;
            }
            else
            {
                x /= z;
                y /= z;
            }
            z = 1.0;

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
