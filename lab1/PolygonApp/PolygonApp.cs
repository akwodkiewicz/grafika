using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using PolygonApp.Geometry;

namespace PolygonApp
{
    public partial class PolygonApp : Form
    {
        private Bitmap _canvas;
        private Polygon _polygon;
        private int _draggedVertexId = -1;
        private int _clickedVertexId = -1;
        private int _clickedLineId = -1;
        private bool _createMode = true;
        private bool _selectAllMode = false;
        private bool _experimentalMode = false;

        public PolygonApp()
        {
            InitializeComponent();
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            _polygon = new Polygon(trackBar1.Value);
        }

        #region Properties
        public bool CreateMode
        {
            get => _createMode;
            set
            {
                _createMode = value;
                if (_createMode)
                    Text = "Polygon Editor [Create Mode]";
                else
                    Text = "Polygon Editor [Edit Mode]";
            }
        }
        public bool SelectAllMode
        {
            get => _selectAllMode;
            set
            {
                _selectAllMode = value;
                label4.Visible = value;
            }
        }
        public bool ExperimentalMode
        {
            get => _experimentalMode;
            set
            {
                _experimentalMode = value;
                label5.Visible = value;
            }
        }
        #endregion

        #region PictureBox Interaction
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (CreateMode)
            {
                _draggedVertexId = _polygon.AddVertex(e.Location);
                if (_draggedVertexId == -1) { CreateMode = false; Text = "Polygon Editor [Edit Mode]"; }
            }
            else if (!CreateMode && e.Button == MouseButtons.Right)
            {
                _clickedVertexId = _polygon.GetVertexIdFromPoint(e.Location);
                if (_clickedVertexId != -1)
                    contextMenuStrip1.Show(pictureBox, e.Location);
                else
                {
                    _clickedLineId = _polygon.GetLineIdFromPoint(e.Location);
                    if (_clickedLineId != -1)
                        contextMenuStrip2.Show(pictureBox, e.Location);
                }
                Debug.WriteLine($"Got: #{_clickedLineId}");
            }
            pictureBox.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CreateMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!SelectAllMode)
                    {
                        _clickedVertexId = _polygon.GetVertexIdFromPoint(e.Location);
                        if (_clickedVertexId == -1)
                            _clickedLineId = _polygon.GetLineIdFromPoint(e.Location);
                    }
                    else
                        _polygon.Center = e.Location;
                }
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // if in create mode:
            if (_draggedVertexId != -1)
            {
                _polygon.SetPointForVertexId(_draggedVertexId, e.Location);
            }
            else if (!CreateMode && e.Button == MouseButtons.Left)
            {
                if (SelectAllMode)
                {
                    _polygon.MovePolygon(e.Location);
                }
                else if (_clickedVertexId != -1)
                {
                    _polygon.SetPointForVertexId(_clickedVertexId, e.Location);
                }
                else if (_clickedLineId != -1)
                {
                    if (ExperimentalMode)
                        _polygon.MoveLineAlongVectors(_clickedLineId, e.Location);
                    else
                        _polygon.MoveLine(_clickedLineId, e.Location);
                }
            }
            pictureBox.Invalidate();

        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!CreateMode)
            {
                if (e.Button == MouseButtons.Left)
                    _clickedVertexId = -1;
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            _polygon.Draw(_canvas);
            e.Graphics.DrawImage(_canvas, 0, 0, _canvas.Width, _canvas.Height);
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
        }

        private void AddVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _polygon.AddVertexToLine(_clickedLineId);
        }
        #endregion

        #region Keys & Buttons
        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            _polygon.VertexSize = trackBar1.Value;
            pictureBox.Invalidate();
        }

        private void PolygonApp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && CreateMode && _polygon.VerticesCount > 3)
            {
                CreateMode = false;
                _draggedVertexId = -1;
                _polygon.Close();
                pictureBox.Invalidate();
            }
        }

        private void PolygonApp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!CreateMode)
                switch (e.KeyChar)
                {
                    case 'a':
                        SelectAllMode = !SelectAllMode;
                        break;
                    case 'r':
                        Reset();
                        break;
                    case 'x':
                        ExperimentalMode = !ExperimentalMode;
                        break;
                }
        }
        #endregion

        #region ToolStrips
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { _polygon.DeleteVertex(_clickedVertexId); }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You cannot delete any more vertices!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            pictureBox.Invalidate();
        }

        private void LockHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_polygon.MakeLineHorizontal(_clickedLineId))
                    MessageBox.Show("This line is already horizontal", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LockVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_polygon.MakeLineVertical(_clickedLineId))
                    MessageBox.Show("This line is already vertical", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void AngleConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var angle = _polygon.CalculateAngleForVertexId(_clickedVertexId);
            var form = new AngleConstraintForm(angle);

            if (form.ShowDialog() == DialogResult.OK)
                _polygon.SetAngleConstraint(_clickedVertexId, form.Angle);
        }

        private void ClearLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _polygon.ClearLineConstraints(_clickedLineId);
        }

        private void ClearConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _polygon.ClearVertexConstraints(_clickedVertexId);
        }

        private void LockAngleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var angle = _polygon.CalculateAngleForVertexId(_clickedVertexId);
            _polygon.SetAngleConstraint(_clickedVertexId, angle);
        }
        #endregion

        #region Other Methods
        private void Reset()
        {
            _draggedVertexId = -1;
            _polygon = new Polygon(trackBar1.Value);
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            CreateMode = true;
            SelectAllMode = false;
            pictureBox.Invalidate();
            label1.Focus();
        }
        #endregion
    }
}
