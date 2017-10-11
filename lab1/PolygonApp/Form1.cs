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
        private Vertex[] vArray = new Vertex[100];
        private int howMany = 0;
        private PictureBox pictureBox;
        private Bitmap canvas;
        private bool isMouseDown = false;
        private bool wasMoving = false;
        private bool createMode = true;
        private int draggedRect = -1;

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


            pictureBox.MouseClick += PictureBox_MouseClick;
            /*
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.MouseMove += PictureBox_MouseMove;
            */
            pictureBox.Paint += PictureBox_Paint;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < howMany; i++)
            {
                vArray[i].DrawToCanvas(canvas);
            }
            e.Graphics.DrawImage(canvas, 0, 0, canvas.Width, canvas.Height);
            //DrawLines(e.Graphics);
        }

        /*
        private void DrawLines(Graphics g)
        {
            int j;
            Point p1, p2;
            for (int i = 0; i < howMany-1; i++)
            {
                p1 = new Point(vArray[i].X + Vertex.Dimension / 2, vArray[i].Y + Vertex.Dimension / 2);
                p2 = new Point(vArray[i+1].X + Vertex.Dimension / 2, vArray[i+1].Y + Vertex.Dimension / 2);
                g.DrawLine(Pens.Black, p1, p2);
            }
            if(!createMode)
            {
                p1 = new Point(vArray[howMany-1].X + Vertex.Dimension / 2, vArray[howMany - 1].Y + Vertex.Dimension / 2);
                p2 = new Point(vArray[0].X + Vertex.Dimension / 2, vArray[0].Y + Vertex.Dimension / 2);
                g.DrawLine(Pens.Black, p1, p2);
            }
        }
        
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                if(draggedRect == -1)
                {
                    int i = 0;
                    while (i < howMany)
                    {
                        if (vArray[i].Contains(e.Location))
                        {
                            vArray[i].X = e.X - Vertex.Dimension / 2;
                            vArray[i].Y = e.Y - Vertex.Dimension / 2;
                            break;
                        }
                        i++;
                    }
                    draggedRect = i;
                }
                else
                {
                    vArray[draggedRect].X = e.X - Vertex.Dimension / 2;
                    vArray[draggedRect].Y = e.Y - Vertex.Dimension / 2;
                }
                wasMoving = true;
                pictureBox.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            draggedRect = -1;
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }
        */
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!createMode) return;

            if (vArray[0] != null && vArray[0].Contains(e.Location)) { createMode = false; }
            else
            {
                if (!wasMoving)
                {
                    Vertex vertex = new Vertex(e.X, e.Y);
                    vArray[howMany++] = vertex;
                    vertex.Parent = this;
                }
                else
                {
                    wasMoving = false;
                }
            }
            pictureBox.Invalidate();
        }
    }
}
