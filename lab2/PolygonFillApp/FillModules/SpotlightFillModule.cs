using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class SpotlightFillModule : AbstractLightFillModule
    {
        private const int _lightHeight = 1000;
        private const int _cosPow = 300;

        private (double X, double Y, double Z) _lightPos;
        private (double X, double Y, double Z) _targetPos;
        private Color _lightColor;

        public SpotlightFillModule(IFillModule baseModule, Color[][] normalMapColors, (int X, int Y) normalMax,
            Color[][] heightMapColors, (int X, int Y) heightMax, int heightMapFactor,
            (int X, int Y) sourcePosition, (int X, int Y) targetPosition, Color lightColor)
            :base(baseModule, normalMapColors, normalMax, heightMapColors, heightMax, heightMapFactor)
        {
            _baseModule = baseModule;
            _lightPos = (sourcePosition.X, sourcePosition.Y, _lightHeight);
            _targetPos = (targetPosition.X, targetPosition.Y, 0);
            _lightColor = lightColor;
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

            normalPrim = (normal.X + displacement.X, normal.Y + displacement.Y, normal.Z + 0.1 * displacement.Z);

            (double X, double Y, double Z) lightVector = (_lightPos.X - x, -(_lightPos.Y - y), _lightPos.Z);
            lightVector = NormalizeVector(lightVector);
            lightVector.Z = 1.0;

            var cos = (normalPrim.X * lightVector.X + normalPrim.Y * lightVector.Y + normalPrim.Z * lightVector.Z)
                     / (VectorLength(normalPrim) * VectorLength(lightVector));
            cos = Math.Max(0.0, cos);

            (double X, double Y, double Z) colorVector = base.CreateLightVector(_lightColor);
            var stVector = CreateSourceTargetVector(_targetPos, _lightPos);
            var lightColorCos = (stVector.X * lightVector.X + stVector.Y * lightVector.Y + stVector.Z * lightVector.Z)
                     / (VectorLength(stVector) * VectorLength(lightVector));
            var lightColorCosToPower = Math.Max(Math.Pow(lightColorCos, _cosPow), 0);


            var newRed = (int)Math.Min((oldColor.R * cos * lightColorCosToPower * colorVector.X), 255);
            var newGreen = (int)Math.Min((oldColor.G * cos * lightColorCosToPower * colorVector.Y), 255);
            var newBlue = (int)Math.Min((oldColor.B * cos * lightColorCosToPower * colorVector.Z), 255);

            return Color.FromArgb(newRed, newGreen, newBlue);
        }

        private (double X, double Y, double Z) NormalizeVector((double X, double Y, double Z) vector)
        {
            var d = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
            d = (d == 0) ? 1 : d;
            return (vector.X / d, vector.Y / d, vector.Z / d);
        }

        private (double X, double Y, double Z) CreateSourceTargetVector((double X, double Y, double Z) source, (double X, double Y, double Z) target)
        {
            return (( target.X - source.X),-(target.Y - source.Y), (target.Z-source.Z));
        }
    }
}
