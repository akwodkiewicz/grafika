using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp
{
    class Vertex : Control, IDrawable
    {
        private int _dimension;
        private PointC _point;
        private PointC _lastPoint;
        private bool _moved;
        private int _angleConstraint;
        private bool _hasAngleConstraint;

        public Vertex(PointC point, int dimension)
        {
            _point = point;
            _dimension = dimension;
            Left = X - _dimension / 2;
            Top = Y - _dimension / 2;
            ForeColor = Color.Black;
            BackColor = Color.White;
            _moved = true;
        }

        public int AngleConstraint
        {
            get => _angleConstraint;
            set
            {
                _angleConstraint = value;
                HasAngleConstraint = true;
            }
        }
        public PointC Point { get => _point; }
        public int X { get => _point.X; private set => _point.X = value; }
        public int Y { get => _point.Y; private set => _point.Y = value; }
        new public int Width { get => Dimension; }
        public bool HasAngleConstraint { get => _hasAngleConstraint; set => _hasAngleConstraint = value; }
        new public int Height { get => Dimension; }
        public bool Moved { get => _moved; set => _moved = value; }
        public int Dimension
        {
            get => _dimension;
            set
            {
                _dimension = value;
                Left = _point.X - value / 2;
                Top = _point.Y - value / 2;
            }
        }


        public bool Contains(Point location)
        {
            if (location.X > Left
                && location.X < Left + Dimension
                && location.Y > Top
                && location.Y < Top + Dimension)
                return true;
            return false;
        }
        public bool Contains(PointC location)
        {
            return Contains(new Point(location.X, location.Y));
        }

        public bool Contains(Point location, int proximity)
        {
            if (location.X > Left - proximity
                && location.X < Left + Dimension + proximity
                && location.Y > Top - proximity
                && location.Y < Top + Dimension + proximity)
                return true;
            return false;
        }
        public bool Contains(PointC location, int proximity)
        {
            return Contains(new Point(location.X, location.Y), proximity);
        }

        public void Draw(Bitmap canvas)
        {
            /*
            if (Moved)
                Erase(canvas);
            */
            int xStart = (Left > 0) ? Left : 0;
            int xEnd = (Left + Width > canvas.Width) ? canvas.Width : Left + Width;
            int yStart = (Top > 0) ? Top : 0;
            int yEnd = (Top + Height > canvas.Height) ? canvas.Height : Top + Height;

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    canvas.SetPixel(x, y, ForeColor);
                }
            }

            Moved = false;
        }

        public void SetPoint(Point point)
        {
            _lastPoint = _point;
            _point.X = point.X;
            _point.Y = point.Y;
            Left = _point.X - Dimension / 2;
            Top = _point.Y - Dimension / 2;
            _moved = true;
        }
        public void SetPoint(PointC point)
        {
            _lastPoint = _point;
            _point.X = point.X;
            _point.Y = point.Y;
            Left = _point.X - Dimension / 2;
            Top = _point.Y - Dimension / 2;
            _moved = true;
        }


        private void Erase(Bitmap canvas)
        {
            int xStart = _lastPoint.X - Width / 2;
            int xEnd = xStart + Width;
            int yStart = _lastPoint.Y - Height / 2;
            int yEnd = yStart + Height;

            if (xStart < 0) xStart = 0;
            if (xEnd > canvas.Width) xEnd = canvas.Width;
            if (yStart < 0) yStart = 0;
            if (yEnd > canvas.Height) yEnd = canvas.Height;

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    canvas.SetPixel(x, y, BackColor);
                }
            }
        }
    }
}
