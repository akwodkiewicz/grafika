using System.Drawing;

namespace PolygonApp.Geometry
{
    class Vertex
    {
        private int _left;
        private int _top;
        private int _size;
        private Point _center;
        private Point _lastCenter;
        private Color _foreColor;
        private Color _backColor;
        private bool _moved;

        public Vertex(Point point, int size)
        {
            _center = point;
            _size = size;
            Left = X - _size / 2;
            Top = Y - _size / 2;
            ForeColor = Color.Black;
            BackColor = Color.White;
            _moved = true;
        }

        #region Properties
        public Point Center { get => _center; }
        public int X
        {
            get => _center.X;
            set
            {
                var temp = new Point(value, _center.Y);
                SetPoint(temp);
            }
        }
        public int Y
        {
            get => _center.Y;
            set
            {
                var temp = new Point(_center.X, value);
                SetPoint(temp);
            }
        }
        public int Width { get => Size; }
        public int Height { get => Size; }
        public bool Moved { get => _moved; set => _moved = value; }
        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                Left = _center.X - value / 2;
                Top = _center.Y - value / 2;
            }
        }
        public int Left { get => _left; set => _left = value; }
        public int Top { get => _top; set => _top = value; }
        public Color ForeColor { get => _foreColor; set => _foreColor = value; }
        public Color BackColor { get => _backColor; set => _backColor = value; }
        #endregion

        #region Public Methods
        public bool Contains(Point location)
        {
            if (location.X > Left
                && location.X < Left + Size
                && location.Y > Top
                && location.Y < Top + Size)
                return true;
            return false;
        }

        public bool Contains(Point location, int proximity)
        {
            if (location.X > Left - proximity
                && location.X < Left + Size + proximity
                && location.Y > Top - proximity
                && location.Y < Top + Size + proximity)
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
            Moved = false;
        }

        public void MovePoint(int dx, int dy)
        {
            _lastCenter = _center;
            _center.X += dx;
            _center.Y += dy;
            _left = _center.X - _size / 2;
            _top = _center.Y - _size / 2;
            _moved = true;
        }

        public void SetPoint(Point point)
        {
            _lastCenter = _center;
            _center.X = point.X;
            _center.Y = point.Y;
            _left = _center.X - _size / 2;
            _top = _center.Y - _size / 2;
            _moved = true;
        }
        #endregion

        #region Private Methods
        private void Erase(Bitmap canvas)
        {
            int xStart = _lastCenter.X - Width / 2;
            int xEnd = xStart + Width;
            int yStart = _lastCenter.Y - Height / 2;
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
        #endregion
    }
}
