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
        private Point _point;
        private Point _lastPoint;
        private bool _moved;
        private int? _angleConstraint;
        private Constraint _constraint;
        private int _inverseMarkSize = 5;

        public Vertex(Point point, int dimension)
        {
            _point = point;
            _dimension = dimension;
            Left = X - _dimension / 2;
            Top = Y - _dimension / 2;
            ForeColor = Color.Black;
            BackColor = Color.White;
            _moved = true;
            _constraint = Constraint.None;
        }

        public int? AngleConstraint
        {
            get => _angleConstraint;
            set
            {
                _angleConstraint = value;
                if (_angleConstraint == null)
                    Constraint = Constraint.None;
                else
                    Constraint = Constraint.Angle;
            }
        }
        public Point Point { get => _point; }
        public int X
        {
            get => _point.X;
            set
            {
                var temp = new Point(value, _point.Y);
                SetPoint(temp);
            }
        }
        public int Y
        {
            get => _point.Y;
            set
            {
                var temp = new Point(_point.X, value);
                SetPoint(temp);
            }
        }
        new public int Width { get => Dimension; }
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
        public Constraint Constraint { get => _constraint; private set => _constraint = value; }

        public bool Contains(Point location)
        {
            if (location.X > Left
                && location.X < Left + Dimension
                && location.Y > Top
                && location.Y < Top + Dimension)
                return true;
            return false;
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

            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    canvas.SetPixel(x, y, ForeColor);
                }
            }

            if (Constraint == Constraint.Angle)
            {
                 xStart = (Left+ _inverseMarkSize > 0) ? Left+ _inverseMarkSize : 0;
                 xEnd = (Left + Width  - _inverseMarkSize > canvas.Width) ? canvas.Width : Left + Width - _inverseMarkSize;
                 yStart = (Top + _inverseMarkSize > 0) ? Top + _inverseMarkSize : 0;
                 yEnd = (Top + Height - _inverseMarkSize > canvas.Height) ? canvas.Height : Top + Height - _inverseMarkSize;

                for (int y = yStart; y < yEnd; y++)
                {
                    for (int x = xStart; x < xEnd; x++)
                    {
                        canvas.SetPixel(x, y, BackColor);
                    }
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

        public void MovePoint(int dx, int dy)
        {
            _lastPoint = _point;
            _point.X += dx;
            _point.Y += dy;
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
