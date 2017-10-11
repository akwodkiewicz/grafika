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
        private static int dimension = 31;
        private Rectangle rectangle;

        public Vertex(int x, int y)
        {
            rectangle = new Rectangle(0, 0, dimension, dimension);
            this.Top = y;
            this.Left = x;
        }

        public void draw(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
        }
        public Rectangle Rectangle { get; private set; }
    }
}
