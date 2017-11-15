using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class DirectionalLightFillModule : IFillModule
    {
        private IFillModule _baseModule;
        private (double X, double Y, double Z) _lightVector;
        private Bitmap _normalMap;
        private int _xMax;
        private int _yMax;
        public DirectionalLightFillModule(IFillModule baseModule, Bitmap normalMap)
        {
            _baseModule = baseModule;
            _lightVector = (0.0, 0.0, 1.0);
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

            var cos = (normal.X * _lightVector.X + normal.Y * _lightVector.Y + normal.Z * _lightVector.Z)
                      / (VectorLength(normal) * VectorLength(_lightVector));
            cos = Math.Max(0.0, cos);
            var newRed = (int)(oldColor.R * cos);
            var newGreen = (int)(oldColor.G * cos);
            var newBlue = (int)(oldColor.B * cos);

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

            var f = 1.0 / z;
            z = 1.0;
            y *= f;
            x *= f;

            //return NormalizeVector((x, y, z));
            return (x, y, z);
        }

        private (double, double, double) CreateLightVector(Color color)
        {
            var x = color.R / 255.0;
            var y = color.G / 255.0;
            var z = color.B / 255.0;

            // return NormalizeVector((x, y, z));
            return (x, y, z);
        }

        private double VectorLength((double X, double Y, double Z) vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        }

        //private (double X, double Y, double Z) NormalizeVector((double X, double Y, double Z) vector)
        //{
        //    var d = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z  * vector.Z);
        //    d = (d == 0) ? 1 : d;
        //    return (vector.X / d, vector.Y / d, vector.Z/ d);
        //}
    }
}
