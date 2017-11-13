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
        private int _xMax;
        private int _yMax;
        //private Size _pictureboxSize;
        public TextureFillModule(Bitmap texture/*, Size pictureboxSize*/)
        {
            _texture = texture;
            _xMax = texture.Width;
            _yMax = texture.Height;
        }

        public Color GetColor(int x, int y)
        {

            return _texture.GetPixel(x % _xMax, y % _yMax);
        }
    }
}
