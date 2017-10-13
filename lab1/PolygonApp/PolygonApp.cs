using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp
{
    public partial class PolygonApp : Form
    {
        private PictureBox pictureBox;
        private Bitmap canvas;
        private Bitmap workingLayer;
        private Polygon polygon;
        private int draggedVertexId;
        private int vertexDimension = 11;
        private bool createMode = true;
        private bool isMouseDown = false;

        public PolygonApp()
        {
            InitializeComponent();
            pictureBox = new PictureBox
            {
                Top = 0,
                Left = 0,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
            };
            splitContainer1.Panel2.Controls.Add(pictureBox);
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            workingLayer = new Bitmap(pictureBox.Width, pictureBox.Height);
            polygon = new Polygon();


            pictureBox.MouseClick += PictureBox_MouseClick;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.Paint += PictureBox_Paint;
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
           if (createMode && !polygon.AddVertex(new Point(e.X, e.Y), vertexDimension))
                createMode = false;

            pictureBox.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!createMode)
            {
                isMouseDown = true;
                draggedVertexId = polygon.GetVertexIdFromPoint(new Point(e.X, e.Y));
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && draggedVertexId != -1)
            {
                polygon.SetPointForVertexId(draggedVertexId, e.Location);
                pictureBox.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!createMode)
            {
                isMouseDown = false;
                draggedVertexId = -1;
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            polygon.Draw(canvas);
            e.Graphics.DrawImage(canvas, 0, 0, canvas.Width, canvas.Height);
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
        }
    }
}
