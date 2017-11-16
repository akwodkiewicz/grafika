using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class TextureFillModule : IFillModule
    {
        private Color[][] _textureColors;
        private int _xMax;
        private int _yMax;

        public TextureFillModule(Color[][] textureColors, int xMax, int yMax)
        {
            _textureColors = textureColors;
            _xMax = xMax;
            _yMax = yMax;
        }

        public Color GetColor(int x, int y)
        {
            return _textureColors[x % _xMax][y % _yMax];
        }
    }
}
