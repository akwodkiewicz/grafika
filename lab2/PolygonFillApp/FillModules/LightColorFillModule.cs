using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class LightColorFillModule : IFillModule
    {
        private IFillModule _baseModule;
        private Color _light;
        public LightColorFillModule(IFillModule baseModule, Color light)
        {
            _baseModule = baseModule;
            _light = light;
        }
        public Color GetColor(int x, int y)
        {
            var baseColor = _baseModule.GetColor(x, y);
            var newRed = (int)(baseColor.R * (double)_light.R / 255);
            var newGreen = (int)(baseColor.G * (double)_light.G / 255);
            var newBlue = (int)(baseColor.B * (double)_light.B / 255);
            var newColor = Color.FromArgb(newRed, newGreen, newBlue);
            return newColor;
        }
    }
}
