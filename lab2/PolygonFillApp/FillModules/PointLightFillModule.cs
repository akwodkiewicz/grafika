using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class PointLightFillModule : IFillModule
    {
        private IFillModule _baseModule;
        private (double X, double Y, double Z) _lightPos;
        private Bitmap _normalMap;
        private Bitmap _heightMap;
        private int _xNormalMax;
        private int _yNormalMax;
        private int _xHeightMax;
        private int _yHeightMax;

        public PointLightFillModule(IFillModule baseModule, (double X, double Y, double Z) lightPos, Bitmap normalMap, Bitmap heightMap)
        {
            _baseModule = baseModule;
            _lightPos = lightPos;
            if (normalMap != null)
            {
                _normalMap = normalMap;
                _xNormalMax = normalMap.Width;
                _yNormalMax = normalMap.Height;
            }
            if (heightMap != null)
            {
                _heightMap = heightMap;
                _xHeightMax = heightMap.Width;
                _yHeightMax = heightMap.Height;
            }
        }

        public Color GetColor(int x, int y)
        {
            var oldColor = _baseModule.GetColor(x, y);

            (double X, double Y, double Z) normal;
            (double X, double Y, double Z) displacement;
            (double X, double Y, double Z) normalPrim;

            if (_normalMap == null)
                normal = (0.0, 0.0, 1.0);
            else
                normal = CreateNormalVector(GetNormalMapColor(x, y));

            if (_heightMap == null)
                displacement = (0.0, 0.0, 0.0);
            else
                displacement = CreateDiffuseVector(x, y, normal);

            normalPrim = (normal.X+displacement.X, normal.Y+displacement.Y, normal.Z+displacement.Z);

            (double X, double Y, double Z) lightVector = (_lightPos.X - x, -(_lightPos.Y - y), _lightPos.Z);
            lightVector = NormalizeVector(lightVector);
            lightVector.Z = 1.0;

            var cos = (normalPrim.X * lightVector.X + normalPrim.Y * lightVector.Y + normalPrim.Z * lightVector.Z)
                     / (VectorLength(normalPrim) * VectorLength(lightVector));
            cos = Math.Max(0.0, cos);
            var newRed = (int)(oldColor.R * cos);
            var newGreen = (int)(oldColor.G * cos);
            var newBlue = (int)(oldColor.B * cos);

            return Color.FromArgb(newRed, newGreen, newBlue);
        }

        private (double X, double Y, double Z) CreateDiffuseVector(int x, int y, (double X, double Y, double Z) normal)
        {

            var color = _heightMap.GetPixel(x % _xHeightMax, y % _yHeightMax);
            var xColor = _heightMap.GetPixel((x+1) % _xHeightMax, y % _yHeightMax);
            var yColor = _heightMap.GetPixel(x % _xHeightMax, (y+1) % _yHeightMax);
            (double X, double Y, double Z) dhx = ((xColor.R - color.R) / 255.0, (xColor.G - color.G) / 255.0, (xColor.B - color.B) / 255.0);
            (double X, double Y, double Z) dhy = ((yColor.R - color.R) / 255.0, (yColor.G - color.G) / 255.0, (yColor.B - color.B) / 255.0);

            (double X, double Y, double Z) t = (1.0, 0.0, -normal.X);
            (double X, double Y, double Z) b = (0.0, 1.0, normal.Y);

            return (t.X * dhx.X + b.X * dhy.X, t.Y * dhx.Y + b.Y * dhy.Y, t.Z * dhx.Z + b.Z * dhy.Z);
        }

        private Color GetNormalMapColor(int x, int y)
        {
            return _normalMap.GetPixel(x % _xNormalMax, y % _yNormalMax);
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

        private (double X, double Y, double Z) NormalizeVector((double X, double Y, double Z) vector)
        {
            var d = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
            d = (d == 0) ? 1 : d;
            return (vector.X / d, vector.Y / d, vector.Z / d);
        }
    }
}
