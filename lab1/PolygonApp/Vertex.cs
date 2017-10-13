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
        private const int dimension = 31;
        private Point point;
        private Point lastPoint;
        private bool moved;

        public Vertex(Point _point)
        {
            Point = _point;
            Left = X - dimension / 2;
            Top = Y - dimension / 2;
            ForeColor = Color.Black;
            BackColor = Color.White;
            Moved = true;

            MouseClick += Vertex_MouseClick;
        }

        private void Vertex_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Point Point
        {
            get => point;
            set
            {
                lastPoint = point;
                point = value;
                Left = point.X - dimension / 2;
                Top = point.Y - dimension / 2;
                moved = true;
            }
        }
        public int X { get => point.X; }
        public int Y { get => point.Y; }
        new public int Width { get => dimension; }
        new public int Height { get => dimension; }
        public bool Moved { get => moved; set => moved = value; }

        public bool Contains(Point location)
        {
            if (location.X > Left
                && location.X < Left + dimension
                && location.Y > Top
                && location.Y < Top + dimension)
                return true;
            return false;
        }

        public void Draw(Bitmap canvas)
        {
            if (Moved)
                Erase(canvas);

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

        private void Erase(Bitmap canvas)
        {
            int xStart = lastPoint.X - Width / 2;
            int xEnd = xStart + Width;
            int yStart = lastPoint.Y - Height / 2;
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
