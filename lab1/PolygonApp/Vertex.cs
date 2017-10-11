using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp
{
    class Vertex : Control
    {
        private const int dimension = 31;

        public Vertex(int x, int y)
        {
            Left = x - dimension / 2;
            Top = y - dimension / 2;
            ForeColor = Color.Black;
            //SetStyle(ControlStyles.UserPaint| ControlStyles.AllPaintingInWmPaint| ControlStyles.Selectable, true);

            MouseClick += Vertex_MouseClick;
        }

        private void Vertex_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        new public int Width { get => dimension; }
        new public int Height { get => dimension; }

        internal void DrawToCanvas(Bitmap canvas)
        {
            for (int x = Left; x < Left+Width; x++)
            {
                for (int y = Top; y < Top+Height; y++)
                {
                    canvas.SetPixel(x, y, ForeColor);
                }
            }
        }

        public bool Contains(Point location)
        {
            if (location.X > Left
                && location.X < Left + dimension
                && location.Y > Top
                && location.Y < Top + dimension)
                return true;
            return false;
        }
    }
}
