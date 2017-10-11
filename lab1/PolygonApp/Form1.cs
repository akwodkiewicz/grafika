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
        private Rectangle[] rectList = new Rectangle[100];
        private int howMany = 0;
        private static int rectHeight = 11;
        private PictureBox pictureBox;
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

            pictureBox.MouseClick += PictureBox_MouseClick;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.Paint += PictureBox_Paint;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangles(Brushes.Black, rectList);
            DrawLines(e.Graphics);
        }

        private void DrawLines(Graphics g)
        {
            int j;
            Point p1, p2;
            for (int i = 0; i < howMany-1; i++)
            {
                p1 = new Point(rectList[i].X + rectHeight / 2, rectList[i].Y + rectHeight / 2);
                p2 = new Point(rectList[i+1].X + rectHeight / 2, rectList[i+1].Y + rectHeight / 2);
                g.DrawLine(Pens.Black, p1, p2);
            }
            if(!createMode)
            {
                p1 = new Point(rectList[howMany-1].X + rectHeight / 2, rectList[howMany - 1].Y + rectHeight / 2);
                p2 = new Point(rectList[0].X + rectHeight / 2, rectList[0].Y + rectHeight / 2);
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
                        if (rectList[i].Contains(e.Location))
                        {
                            rectList[i].X = e.X - rectHeight / 2;
                            rectList[i].Y = e.Y - rectHeight / 2;
                            break;
                        }
                        i++;
                    }
                    draggedRect = i;
                }
                else
                {
                    rectList[draggedRect].X = e.X - rectHeight / 2;
                    rectList[draggedRect].Y = e.Y - rectHeight / 2;
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

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!createMode) return;

            if (rectList[0].Contains(e.Location)) { createMode = false; }
            else
            {
                if (!wasMoving)
                {
                    Rectangle rectangle = new Rectangle(e.X - rectHeight / 2, e.Y - rectHeight / 2, rectHeight, rectHeight);
                    rectList[howMany++] = rectangle;
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
