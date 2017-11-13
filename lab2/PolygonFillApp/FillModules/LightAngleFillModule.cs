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
        private Color _normalVector;
        public LightAngleFillModule(IFillModule baseModule, Color lightVector)
        {
            _baseModule = baseModule;
            _lightVector = lightVector;
            _normalVector = Color.FromArgb(0, 0, 255);
        }

        public Color GetColor(int x, int y)
        {
            var oldColor = _baseModule.GetColor(x, y);
            var cos = _normalVector.R / 255.0 * _lightVector.R / 255.0
                        + _normalVector.G / 255.0 * _lightVector.G / 255.0
                        + _normalVector.B / 255.0 * _lightVector.B / 255.0;

            var newRed = (int)(oldColor.R * cos);
            var newGreen = (int)(oldColor.G * cos);
            var newBlue = (int)(oldColor.B * cos);
            return Color.FromArgb(newRed, newGreen, newBlue);
        }
    }
}
