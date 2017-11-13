using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class SolidFillModule : IFillModule
    {
        private Color _color;
        public SolidFillModule(Color color)
        {
            _color = color;
        }
        public Color GetColor(int x, int y)
        {
            return _color;
        }
    }
}
