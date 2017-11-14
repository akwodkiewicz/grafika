using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class LightAngleFillModule : IFillModule
    {
        private IFillModule _baseModule;
        private (double X, double Y, double Z) _lightVector;
        private Bitmap _normalMap;
        private int _xMax;
        private int _yMax;
        public LightAngleFillModule(IFillModule baseModule, Color lightVector, Bitmap normalMap)
        {
            _baseModule = baseModule;
            _lightVector = CreateLightVector(lightVector);
            if (normalMap != null)
            {
                _normalMap = normalMap;
                _xMax = normalMap.Width;
                _yMax = normalMap.Height;
            }
        }

        public Color GetColor(int x, int y)
        {
            var oldColor = _baseModule.GetColor(x, y);

            (double X, double Y, double Z) normal;
            if (_normalMap == null)
                normal = (0.0, 0.0, 1.0);
            else
                normal = CreateNormalVector(GetNormalMapColor(x, y));

            var cos = normal.X * _lightVector.X + normal.Y * _lightVector.Y + normal.Z * _lightVector.Z;

            var newRed = (int)Math.Min(Math.Max((oldColor.R * cos), 0.0), 255);
            var newGreen = (int)Math.Min(Math.Max((oldColor.G * cos), 0.0), 255);
            var newBlue = (int)Math.Min(Math.Max((oldColor.B * cos), 0.0), 255);

            return Color.FromArgb(newRed, newGreen, newBlue);
        }

        private Color GetNormalMapColor(int x, int y)
        {
            return _normalMap.GetPixel(x % _xMax, y % _yMax);
        }

        private (double X, double Y, double Z) CreateNormalVector(Color color)
        {
            var x = (color.R - 127) / 128.0;
            var y = (color.G - 127) / 128.0;
            var z = (color.B) / 255.0;

            return NormalizeVector((x, y, z));
        }

        private (double, double, double) CreateLightVector(Color color)
        {
            var x = color.R / 255.0;
            var y = color.G / 255.0;
            var z = color.B / 255.0;

            return NormalizeVector((x, y, z));
        }

        private (double X, double Y, double Z) NormalizeVector((double X, double Y, double Z) vector)
        {
            var d = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z  * vector.Z);
            d = (d == 0) ? 1 : d;
            return (vector.X / d, vector.Y / d, vector.Z/ d);
        }
    }
}
