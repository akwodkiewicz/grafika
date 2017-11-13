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
        private Color _lightVector;
        private Bitmap _normalMap;
        private int _xMax;
        private int _yMax;
        public LightAngleFillModule(IFillModule baseModule, Color lightVector, Bitmap normalMap)
        {
            _baseModule = baseModule;
            _lightVector = lightVector;
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
            Color normal;
            if (_normalMap == null)
                 normal = Color.FromArgb(127, 127, 255);
            else
                normal = GetNormalMapColor(x, y);
            var cos = Cosinus(normal, _lightVector);

            var newRed = (int)Math.Min(Math.Max((oldColor.R * cos), 0.0), 255);
            var newGreen = (int)Math.Min(Math.Max((oldColor.G * cos), 0.0), 255);
            var newBlue = (int)Math.Min(Math.Max((oldColor.B * cos), 0.0), 255);
            return Color.FromArgb(newRed, newGreen, newBlue);
        }

        private double Cosinus(Color normal, Color light)
        {
            (var n_x, var n_y, var n_z) = CreateNormalVector(normal);
            (var l_x, var l_y, var l_z) = CreateLightVector(light);
            return n_x * l_x + n_y * l_y + n_z * l_z;
        }

        private Color GetNormalMapColor(int x, int y)
        {
            return _normalMap.GetPixel(x % _xMax, y % _yMax);
        }

        private (double, double, double) CreateNormalVector(Color color)
        {
            var x = (color.R - 127) / 128.0;
            var y = (color.G - 127) / 128.0;
            var z = (color.B) / 255.0;

            var d = 1.0 / z;
            x /= d;
            y /= d;
            z = 1.0;
            return (x, y, z);
        }

        private (double, double, double) CreateLightVector(Color color)
        {
            var x = color.R / 255.0;
            var y = color.G / 255.0;
            var z = color.B / 255.0;
            return (x, y, z);
        }
    }
}
