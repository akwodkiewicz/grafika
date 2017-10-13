using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp
{
    class Line : IDrawable
    {
        private Point start;
        private Point end;
        private Color color;
        private bool moved;

        public Line(Point _p1, Point _p2)
        {
            Start = _p1;
            End = _p2;
            Color = Color.Black;
        }

        public Point Start
        {
            get => start;
            set { start = value; moved = true; }
        }
        public Point End
        {
            get => end;
            set { end = value; moved = true; }
        }
        public bool Moved { get => moved; set => moved = value; }
        public Color Color { get => color; set => color = value; }

        public void Draw(Bitmap canvas)
        {
            int dx = End.X - Start.X;
            int dy = End.Y - Start.Y;
            int y = Start.Y;
            int eps = 0;

            for (int x = Start.X; x <= End.X; x++)
            {
                canvas.SetPixel(x, y, color);
                eps += dy;
                if ((eps << 1) >= dx)
                {
                    y++;
                    eps -= dx;
                }
            }

            Moved = false;
        }
    }
}
