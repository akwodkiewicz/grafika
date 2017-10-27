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
        private Polygon _polygon2;
        private int _draggedVertexId = -1;
        private int _clickedVertexId = -1;
        private int _clickedLineId = -1;
        private bool _createMode = true;
        private bool _create2ndMode = false;
        private bool _editMode = false;
        private bool _selectAllMode = false;
        private bool _experimentalMode = false;
        private bool _antialiasing = false;
        private Polygon pickedPolygon;

        public PolygonApp()
        {
            InitializeComponent();
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            _polygon = new Polygon(trackBar1.Value);
            _polygon2 = new Polygon(trackBar1.Value);
            pickedPolygon = _polygon;
        }

        #region Properties
        public bool CreateMode
        {
            get => _createMode;
            set
            {
                _createMode = value;
                if (!value)
                {
                    Create2ndMode = true;
                    pickedPolygon = _polygon2;
                }
                SetTitle();
            }
        }
        public bool Create2ndMode
        {
            get => _create2ndMode;
            set
            {
                _create2ndMode = value;
                if (!value)
                    EditMode = true;
                SetTitle();
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
        public bool EditMode
        {
            get => _editMode;
            set
            {
                _editMode = value;
                if (!value)
                    CreateMode = true;
            }
        }

        public bool Antialiasing { get => _antialiasing; set => _antialiasing = value; }
        #endregion

        #region PictureBox Interaction
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (CreateMode)
            {
                _draggedVertexId = _polygon.AddVertex(e.Location);
                if (_draggedVertexId == -1) { CreateMode = false; }
            }
            else if(Create2ndMode)
            {
                _draggedVertexId = _polygon2.AddVertex(e.Location);
                if (_draggedVertexId == -1) { Create2ndMode = false; }
            }
            else if (e.Button == MouseButtons.Right)
            {
                pickedPolygon = SelectPolygon(e.Location);
                _clickedVertexId = pickedPolygon.GetVertexIdFromPoint(e.Location);
                if (_clickedVertexId != -1)
                    contextMenuStrip1.Show(pictureBox, e.Location);
                else
                {
                    _clickedLineId = pickedPolygon.GetLineIdFromPoint(e.Location);
                    if (_clickedLineId != -1)
                        contextMenuStrip2.Show(pictureBox, e.Location);
                }
                Debug.WriteLine($"Got: #{_clickedLineId}");
            }
            pictureBox.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (EditMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    pickedPolygon = SelectPolygon(e.Location);

                    if (!SelectAllMode)
                    {
                        _clickedVertexId = pickedPolygon.GetVertexIdFromPoint(e.Location);
                        if (_clickedVertexId == -1)
                            _clickedLineId = pickedPolygon.GetLineIdFromPoint(e.Location);
                    }
                    else
                        pickedPolygon.Center = e.Location;
                }
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // if in create mode:
            if (_draggedVertexId != -1)
            {
                pickedPolygon.SetPointForVertexId(_draggedVertexId, e.Location);
            }
            else if (EditMode && e.Button == MouseButtons.Left)
            {
                if (SelectAllMode)
                {
                    pickedPolygon.MovePolygon(e.Location);
                }
                else if (_clickedVertexId != -1)
                {
                    pickedPolygon.SetPointForVertexId(_clickedVertexId, e.Location);
                }
                else if (_clickedLineId != -1)
                {
                    if (ExperimentalMode)
                        pickedPolygon.MoveLineAlongVectors(_clickedLineId, e.Location);
                    else
                        pickedPolygon.MoveLine(_clickedLineId, e.Location);
                }
            }
            pictureBox.Invalidate();

        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (EditMode)
            {
                if (e.Button == MouseButtons.Left)
                { _clickedVertexId = -1; pickedPolygon = null; }
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            _polygon.Draw(_canvas, _antialiasing);
            _polygon2.Draw(_canvas, _antialiasing);
            e.Graphics.DrawImage(_canvas, 0, 0, _canvas.Width, _canvas.Height);
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
        }

        private void AddVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pickedPolygon.AddVertexToLine(_clickedLineId);
        }
        #endregion

        #region Keys & Buttons
        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            _polygon.VertexSize = trackBar1.Value;
            _polygon2.VertexSize = trackBar1.Value;
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
            else if (e.KeyCode == Keys.Return && Create2ndMode && _polygon2.VerticesCount > 3)
            {
                Create2ndMode = false;
                _draggedVertexId = -1;
                _polygon2.Close();
                pictureBox.Invalidate();
            }
        }

        private void PolygonApp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (EditMode)
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
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Antialiasing = !radioButton1.Checked;
            pictureBox.Invalidate();
        }
        #endregion

        #region ToolStrips
        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(pickedPolygon.IsVertexConstrained(_clickedVertexId))
            {
                var angle = pickedPolygon.CalculateAngleForVertexId(_clickedVertexId);
                angleConstraintToolStripMenuItem.Text = $"Angle Locked ({angle})";
            }
            else
                angleConstraintToolStripMenuItem.Text = "Lock Angle";
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { pickedPolygon.DeleteVertex(_clickedVertexId); }
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
                if (!pickedPolygon.MakeLineHorizontal(_clickedLineId))
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
                if (!pickedPolygon.MakeLineVertical(_clickedLineId))
                    MessageBox.Show("This line is already vertical", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void AngleConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var angle = pickedPolygon.CalculateAngleForVertexId(_clickedVertexId);
            var form = new AngleConstraintForm(angle);

            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pickedPolygon.SetAngleConstraint(_clickedVertexId, form.Angle);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ClearLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pickedPolygon.ClearLineConstraints(_clickedLineId);
        }

        private void ClearConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pickedPolygon.ClearVertexConstraints(_clickedVertexId);
        }
        #endregion

        #region Other Methods
        private void Reset()
        {
            _draggedVertexId = -1;
            _polygon = new Polygon(trackBar1.Value);
            _polygon2 = new Polygon(trackBar1.Value);
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            _createMode = true;
            _create2ndMode = false;
            _editMode = false;
            SelectAllMode = false;
            pictureBox.Invalidate();
            label1.Focus();
        }
        private void SetTitle()
        {
            if(CreateMode || Create2ndMode)
                Text = "Polygon Editor [Create Mode]";
            else
                Text = "Polygon Editor [Edit Mode]";
        }
        private Polygon SelectPolygon(Point location)
        {
            var i = _polygon.GetVertexIdFromPoint(location);
            var j = _polygon.GetLineIdFromPoint(location);
            if (i != -1 || j!=-1)
                return _polygon;
            else return _polygon2;
        }
        #endregion
    }
}
