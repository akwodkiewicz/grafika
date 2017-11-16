using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class DirectionalLightFillModule : AbstractLightFillModule
    {
        private (double X, double Y, double Z) _lightVector;

        public DirectionalLightFillModule(IFillModule baseModule, Bitmap normalMap, Bitmap heightMap)
            : base(baseModule, normalMap, heightMap)
        {
            _lightVector = (0.0, 0.0, 1.0);         
        }

        public override Color GetColor(int x, int y)
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
                displacement = CreateDisplacementVector(x, y, normal);

            normalPrim = (normal.X + displacement.X, normal.Y + displacement.Y, normal.Z + displacement.Z);

            var cos = (normalPrim.X * _lightVector.X + normalPrim.Y * _lightVector.Y + normalPrim.Z * _lightVector.Z)
                      / (VectorLength(normalPrim) * VectorLength(_lightVector));
            cos = Math.Max(0.0, cos);
            var newRed = (int)(oldColor.R * cos);
            var newGreen = (int)(oldColor.G * cos);
            var newBlue = (int)(oldColor.B * cos);

            return Color.FromArgb(newRed, newGreen, newBlue);
        }      
    }
}
