using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp
{
    class PointC
    {
        private int _x;
        private int _y;

        public PointC(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public PointC(Point point)
        {
            _x = point.X;
            _y = point.Y;
        }

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
    }
}
