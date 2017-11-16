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
        private Bitmap _texture;
        private Color[][] _textureColors;
        private int _xMax;
        private int _yMax;

        public TextureFillModule(Bitmap texture)
        {
            _texture = texture;
            _xMax = texture.Width;
            _yMax = texture.Height;

            _textureColors = new Color[_xMax][];
            for (int i = 0; i < _xMax; i++)
                _textureColors[i] = new Color[_yMax];

            for (int y = 0; y < _texture.Height; y++)
                for (int x = 0; x < _texture.Width; x++)
                    _textureColors[x][y] = _texture.GetPixel(x, y);
        }

        public Color GetColor(int x, int y)
        {
            return _textureColors[x % _xMax][y % _yMax];
        }
    }
}
