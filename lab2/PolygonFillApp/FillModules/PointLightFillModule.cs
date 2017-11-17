using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class PointLightFillModule : AbstractLightFillModule
    {
        private (double X, double Y, double Z) _lightPos;
        
        public PointLightFillModule(IFillModule baseModule, (double X, double Y, double Z) lightPos, Color[][] normalMapColors, 
            (int X, int Y) normalMax, Color[][] heightMapColors, (int X, int Y) heightMax, int heightMapFactor)
           : base(baseModule, normalMapColors, normalMax, heightMapColors, heightMax, heightMapFactor)
        {
            _lightPos = lightPos;
        }

        public override Color GetColor(int x, int y)
        {
            var oldColor = _baseModule.GetColor(x, y);

            (double X, double Y, double Z) normal;
            (double X, double Y, double Z) displacement;
            (double X, double Y, double Z) normalPrim;

            if (_xNormalMax == 0)
                normal = (0.0, 0.0, 1.0);
            else
                normal = CreateNormalVector(GetNormalMapColor(x, y));

            if (_xHeightMax == 0)
                displacement = (0.0, 0.0, 0.0);
            else
                displacement = CreateDisplacementVector(x, y, normal);

            normalPrim = (normal.X+displacement.X, normal.Y+displacement.Y, normal.Z+0.1*displacement.Z);

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

        private (double X, double Y, double Z) NormalizeVector((double X, double Y, double Z) vector)
        {
            var d = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
            d = (d == 0) ? 1 : d;
            return (vector.X / d, vector.Y / d, vector.Z / d);
        }
    }
}
