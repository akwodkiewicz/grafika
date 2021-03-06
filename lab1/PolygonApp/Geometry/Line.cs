﻿using System;
using System.Drawing;

using PolygonApp.Commons;

namespace PolygonApp.Geometry
{
    class Line
    {
        private Vertex _start;
        private Vertex _end;
        private Color _color;
        private bool _moved;
        private Point _lastClickPoint;
        private Constraint _constraint;

        public Line(Vertex v1, Vertex v2)
        {
            _start = v1;
            _end = v2;
            _color = Color.Black;
            _constraint = Constraint.None;
        }

        #region Properties
        public Vertex Start
        {
            get => _start;
            set { _start = value; _moved = true; }
        }
        public Vertex End
        {
            get => _end;
            set { _end = value; _moved = true; }
        }
        public bool Moved { get => _moved; set => _moved = value; }
        public Color Color { get => _color; set => _color = value; }
        public Point LastClickPoint { get => _lastClickPoint; set => _lastClickPoint = value; }
        public Constraint Constraint { get => _constraint; set => _constraint = value; }
        public double Length { get => Math.Sqrt((End.X - Start.X) * (End.X - Start.X) + (End.Y - Start.Y) * (End.Y - Start.Y)); }
        public Point Center { get => new Point((Start.X + End.X) / 2, (Start.Y + End.Y) / 2); }
        #endregion

        #region Public Methods
        public void Draw(Bitmap canvas, bool antialiasing)
        {
            if (antialiasing)
                XiaolinWu(canvas);
            else
                Bresenham(canvas);
            // Drawing a `horizontal` mark
            if (Constraint == Constraint.Horizontal)
            {
                var center = Center;
                var endX = (center.X + 10) < canvas.Width ? (center.X + 10) : canvas.Width - 1;
                var endY = (center.Y - 10) < canvas.Height ? (center.Y - 10) : canvas.Height - 1;

                for (int y = center.Y - 15; y < endY && y > 0; y++)
                    for (int x = center.X - 10; x < endX && x > 0; x++)
                        canvas.SetPixel(x, y, Color.BlueViolet);
            }
            // Drawing a `vertical` mark
            else if (Constraint == Constraint.Vertical)
            {
                var center = Center;
                var endX = (center.X - 10) < canvas.Width ? (center.X - 10) : canvas.Width - 1;
                var endY = (center.Y + 10) < canvas.Height ? (center.Y + 10) : canvas.Height - 1;
                for (int y = center.Y - 10; y < endY && y > 0; y++)
                    for (int x = center.X - 15; x < endX && x > 0; x++)
                        canvas.SetPixel(x, y, Color.BlueViolet);
            }
            Moved = false;
        }

        public double GetSquaredDistanceFromPoint(Point p)
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
        #endregion

        #region Private Methods
        private void Bresenham(Bitmap canvas)
        {
            var x0 = Start.X;
            var x1 = End.X;
            var y0 = Start.Y;
            var y1 = End.Y;
            var dx = x1 - x0;
            var dy = y1 - y0;

            // right->left or left->right
            var incX = (dx < 0) ? -1 : 1;
            var incY = (dy < 0) ? -1 : 1;
            if (dx < 0) dx *= -1;
            if (dy < 0) dy *= -1;

            // Close to X axis
            if (dx >= dy)
            {
                var d = 2 * dy - dx;
                var deltaA = 2 * dy;
                var deltaB = 2 * dy - 2 * dx;

                int x = 0, y = 0;
                for (int i = 0; i < dx; i++)
                {
                    if (!(x0 + x < 0 || x0 + x >= canvas.Width || y0 + y < 0 || y0 + y >= canvas.Height))
                        canvas.SetPixel(x0 + x, y0 + y, Color);
                    if (d > 0)
                    {
                        d += deltaB;
                        x += incX;
                        y += incY;
                    }
                    else
                    {
                        d += deltaA;
                        x += incX;
                    }
                }
            }
            // Close to Y axis (steep slope)
            else
            {
                var d = 2 * dx - dy;
                var deltaA = 2 * dx;
                var deltaB = 2 * dx - 2 * dy;

                int x = 0, y = 0;
                for (int i = 0; i < dy; i++)
                {
                    if (!(x0 + x < 0 || x0 + x >= canvas.Width || y0 + y < 0 || y0 + y >= canvas.Height))
                        canvas.SetPixel(x0 + x, y0 + y, Color);
                    if (d > 0)
                    {
                        d += deltaB;
                        x += incX;
                        y += incY;
                    }
                    else
                    {
                        d += deltaA;
                        y += incY;
                    }
                }
            }
        }

        private void XiaolinWu(Bitmap canvas)
        {
            int x0 = Start.X, x1 = End.X, y0 = Start.Y, y1 = End.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep) { x0 = Start.Y;x1 = End.Y; y0 = Start.X; y1 = End.X; }
            if (x0 > x1) { x0 = End.X; x1 = Start.X; y0 = End.Y; y1 = Start.Y; }
            var dx = x1 - x0;
            var dy = y1 - y0;
            var grad = (double)dy / dx;
            if (dx == 0)
                grad = 1;

            var xend = x0;
            var yend = y0;
            var xgap = ReverseFractionalPart(x0 + 0.5);
            var xpxl1 = xend;
            var ypxl1 = y0;

            var intery = yend + grad;

            xend = x1;
            yend = y1;
            xgap = FractionalPart(x1 + 0.5);
            var xpxl2 = xend;
            var ypxl2 = yend;

            if (steep)
            {
                for (int x = xpxl1; x <= xpxl2; x++)
                {
                    canvas.SetPixel((int)Math.Floor(intery), x, GetColor(ReverseFractionalPart(intery)));
                    canvas.SetPixel((int)Math.Floor(intery)+1, x, GetColor(FractionalPart(intery)));
                    intery += grad;
                }
            }
            else
            {
                for (int x = xpxl1; x <= xpxl2; x++)
                {
                    canvas.SetPixel(x, (int)Math.Floor(intery), GetColor(ReverseFractionalPart(intery)));
                    canvas.SetPixel(x, (int)Math.Floor(intery) + 1, GetColor(FractionalPart(intery)));
                    intery += grad;
                }
            }

        }
        private Color GetColor(double intensity)
        {
            var value = (int)(255 * intensity);
            return Color.FromArgb(value, value, value);
        }
        private double FractionalPart(double x) { return x - Math.Floor(x); }
        private double ReverseFractionalPart(double x) { return 1 - FractionalPart(x); }
        private double DistanceSquared(Point a, Point b) => (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        private double DotProduct(Point a, Point b) => a.X * b.X + a.Y * b.Y;
        #endregion
    }
}
