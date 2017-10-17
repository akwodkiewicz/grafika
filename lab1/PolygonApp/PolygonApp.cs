using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp
{
    public partial class PolygonApp : Form
    {
        private Bitmap canvas;
        private Polygon polygon;
        private int draggedVertexId = -1;
        private int clickedVertexId = -1;
        private bool createMode = true;
        private bool isMouseDown = false;

        public PolygonApp()
        {
            InitializeComponent();
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            polygon = new Polygon(trackBar1.Value);
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (createMode)
            {
                draggedVertexId = polygon.AddVertex(new Point(e.X, e.Y));
                if (draggedVertexId == -1) createMode = false;                
            }
            else if (!createMode && e.Button == MouseButtons.Right)
            {
                clickedVertexId = polygon.GetVertexIdFromPoint(e.Location);
                if (clickedVertexId != -1)
                    contextMenuStrip1.Show(pictureBox, e.Location);
            }
            pictureBox.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!createMode)
            {
                isMouseDown = true;
                if (e.Button == MouseButtons.Left)
                {
                    if (radioVertices.Checked)
                        clickedVertexId = polygon.GetVertexIdFromPoint(new Point(e.X, e.Y));
                    else if (radioPolygon.Checked)
                        polygon.Center = e.Location;
                }
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(draggedVertexId != -1)
            {
                polygon.SetPointForVertexId(draggedVertexId, e.Location);
            }

            else if (!createMode && isMouseDown && e.Button == MouseButtons.Left)
            {
                if (radioVertices.Checked && clickedVertexId != -1)
                {
                    polygon.SetPointForVertexId(clickedVertexId, e.Location);
                }
                else if (radioPolygon.Checked)
                {
                    polygon.MovePolygon(e.Location);
                }
            }
            pictureBox.Invalidate();

        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!createMode)
            {
                isMouseDown = false;
                if (e.Button == MouseButtons.Left)
                    clickedVertexId = -1;
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            polygon.Draw(canvas);
            e.Graphics.DrawImage(canvas, 0, 0, canvas.Width, canvas.Height);
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            polygon = new Polygon(trackBar1.Value);
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            createMode = true;
            pictureBox.Invalidate();
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            polygon.VertexSize = trackBar1.Value;
            pictureBox.Invalidate();
        }

        private void AngleConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AngleConstraintForm();

            if(form.ShowDialog() == DialogResult.OK)
                polygon.SetAngleConstraint(clickedVertexId, form.Angle);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygon.DeleteVertex(clickedVertexId);
            pictureBox.Invalidate();
        }
    }
}
