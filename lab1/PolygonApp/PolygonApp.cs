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
        private int clickedLineId = -1;
        private bool createMode = true;

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
                draggedVertexId = polygon.AddVertex(e.Location);
                if (draggedVertexId == -1) createMode = false;
            }
            else if (!createMode && e.Button == MouseButtons.Right)
            {
                clickedVertexId = polygon.GetVertexIdFromPoint(e.Location);
                if (clickedVertexId != -1)
                    contextMenuStrip1.Show(pictureBox, e.Location);
                else
                {
                    clickedLineId = polygon.GetLineIdFromPoint(e.Location);
                    if (clickedLineId != -1)
                        contextMenuStrip2.Show(pictureBox, e.Location);
                }
                Debug.WriteLine($"Got: #{clickedLineId}");
            }
            pictureBox.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!createMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (radioVertices.Checked)
                    {
                        clickedVertexId = polygon.GetVertexIdFromPoint(e.Location);
                        if (clickedVertexId == -1)
                            clickedLineId = polygon.GetLineIdFromPoint(e.Location);
                    }
                    else if (radioPolygon.Checked)
                        polygon.Center = new PointC(e.Location);
                }
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // if in create mode:
            if (draggedVertexId != -1)
            {
                polygon.SetPointForVertexId(draggedVertexId, e.Location);
            }
            else if (!createMode && e.Button == MouseButtons.Left)
            {
                if (radioVertices.Checked && clickedVertexId != -1)
                {
                    polygon.SetPointForVertexId(clickedVertexId, e.Location);
                }
                else if (radioVertices.Checked && clickedLineId != -1)
                {
                    polygon.MoveLine(clickedLineId, e.Location);
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
            draggedVertexId = -1;
            polygon = new Polygon(trackBar1.Value);
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            createMode = true;
            pictureBox.Invalidate();
            label1.Focus();
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            polygon.VertexSize = trackBar1.Value;
            pictureBox.Invalidate();
        }

        private void AngleConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AngleConstraintForm();

            if (form.ShowDialog() == DialogResult.OK)
                polygon.SetAngleConstraint(clickedVertexId, form.Angle);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { polygon.DeleteVertex(clickedVertexId); }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You cannot delete any more vertices!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            pictureBox.Invalidate();
        }

        private void PolygonApp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && createMode)
            {
                createMode = false;
                draggedVertexId = -1;
                polygon.Close();
                pictureBox.Invalidate();
            }
        }

        private void AddVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygon.AddVertexToLine(clickedLineId);
        }

        private void MakeHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!polygon.MakeLineHorizontal(clickedLineId))
                    MessageBox.Show("This line is already horizontal", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You can't add 2 consecutive horizontal lines", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MakeVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!polygon.MakeLineVertical(clickedLineId))
                    MessageBox.Show("This line is already vertical", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You can't add 2 consecutive vertical lines", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
    }
}
