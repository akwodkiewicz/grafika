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
            AllPurposeBresenham(canvas);
            Moved = false;
        }

        private void AllPurposeBresenham(Bitmap canvas)
        {
            int x1, y1, x2, y2, temp;
            bool swapped = false;
            bool negated = false;
            var dx = End.X - Start.X;
            var dy = End.Y - Start.Y;

            // Steep slope
            if (dy * dy > dx * dx)
            { x1 = Start.Y; y1 = Start.X; x2 = End.Y; y2 = End.X; swapped = true; }
            else
            { x1 = Start.X; y1 = Start.Y; x2 = End.X; y2 = End.Y; }

            // Right to left
            if (x1 > x2)
            {
                temp = x1; x1 = x2; x2 = temp;
                temp = y1; y1 = y2; y2 = temp;
            }

            //Negavite slope
            if (y1 > y2) { y1 *= -1; y2 *= -1; negated = true; }

            Bresenham(canvas, x1, y1, x2, y2, swapped, negated);
        }

        private void Bresenham(Bitmap canvas, int x1, int y1, int x2, int y2, bool swapped, bool negated)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int y = y1;
            int eps = 0;

            for (int x = x1; x <= x2; x++)
            {
                TranslateAndDraw(canvas, x, y, swapped, negated);
                eps += dy;
                if ((eps << 1) >= dx)
                {
                    y++;
                    eps -= dx;
                }
            }
        }

        private void TranslateAndDraw(Bitmap canvas, int x, int y, bool swapped, bool negated)
        {
            if (negated)
                y *= -1;
            if (x >= 0 && y >= 0)
            {
                if (swapped && y < canvas.Width && x < canvas.Height)
                    canvas.SetPixel(y, x, color);
                else if (x < canvas.Width && y < canvas.Height)
                    canvas.SetPixel(x, y, color);
            }
        }
    }
}
