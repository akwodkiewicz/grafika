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
        private PointC _start;
        private PointC _end;
        private Color _color;
        private bool _moved;

        public Line(PointC p1, PointC p2)
        {
            Start = p1;
            End = p2;
            Color = Color.Black;
        }

        public PointC Start
        {
            get => _start;
            set { _start = value; _moved = true; }
        }
        public PointC End
        {
            get => _end;
            set { _end = value; _moved = true; }
        }
        public bool Moved { get => _moved; set => _moved = value; }
        public Color Color { get => _color; set => _color = value; }

        public void Draw(Bitmap canvas)
        {
            AllPurposeBresenham(canvas);
            Moved = false;
        }

        public double GetSquaredDistanceFromPoint(PointC p)
        {
            var start = new Point(_start.X, _start.Y);
            var end = new Point(_end.X, _end.Y);
            var point = new Point(p.X, p.Y);

            double l2 = DistanceSquared(end, start);
            if (l2 == 0.0) return DistanceSquared(point, start);

            var pminusv = new Point(point.X - start.X, point.Y - start.Y);
            var wminusv = new Point(end.X - start.X, end.Y - start.Y);

            var t = Math.Max(0.0, Math.Min(1.0, DotProduct(pminusv, wminusv) / l2));
            var x = start.X + t * wminusv.X;
            var y = start.Y + t * wminusv.Y;
            var projection = new Point((int)x, (int)y);
            return DistanceSquared(point, projection);
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
                    canvas.SetPixel(y, x, _color);
                else if (x < canvas.Width && y < canvas.Height)
                    canvas.SetPixel(x, y, _color);
            }
        }

        private double DistanceSquared(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        private double DotProduct(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
    }
}
