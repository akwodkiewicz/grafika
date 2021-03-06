﻿using System;
using System.Drawing;

namespace PolygonApp.Geometry
{
    class Edge
    {
        private Vertex _start;
        private Vertex _end;
        private Color _color;
        private bool _moved;
        private Point _lastClickPoint;

        public Edge(Vertex v1, Vertex v2)
        {
            _start = v1;
            _end = v2;
            _color = Color.Black;
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
        public double Length { get => Math.Sqrt((End.X - Start.X) * (End.X - Start.X) + (End.Y - Start.Y) * (End.Y - Start.Y)); }
        public Point Center { get => new Point((Start.X + End.X) / 2, (Start.Y + End.Y) / 2); }
        #endregion

        #region Public Methods
        public void Draw(Bitmap canvas)
        {
            Bresenham(canvas);
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
