using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using PolygonApp.Geometry;
using PolygonApp.Algorithms;
using System.Collections.Generic;

namespace PolygonApp
{
    public partial class PolygonFillApp : Form
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
        private Polygon _pickedPolygon;

        public PolygonFillApp()
        {
            InitializeComponent();
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            //Init();
            _polygon = new Polygon(trackBar1.Value);
            _polygon2 = new Polygon(trackBar1.Value);
            _pickedPolygon = _polygon;
        }

        public void Init()
        {
            var list = new List<Vertex>()
            {
                new Vertex(new Point(50, 50)),
                new Vertex(new Point(180, 50)),
                new Vertex(new Point(180, 200)),
                new Vertex(new Point(50, 200))
            };
            _polygon = new Polygon(list);
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
                    _pickedPolygon = _polygon2;
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
        #endregion

        #region PictureBox Interaction
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (CreateMode)
            {
                _draggedVertexId = _polygon.AddVertex(e.Location);
                if (_draggedVertexId == -1) { CreateMode = false; }
            }
            else if (Create2ndMode)
            {
                _draggedVertexId = _polygon2.AddVertex(e.Location);
                if (_draggedVertexId == -1) { Create2ndMode = false; }
            }
            else if (e.Button == MouseButtons.Right)
            {
                _pickedPolygon = SelectPolygon(e.Location);
                _clickedVertexId = _pickedPolygon.GetVertexIdFromPoint(e.Location);
                if (_clickedVertexId != -1)
                    contextMenuStrip1.Show(pictureBox, e.Location);
                else
                {
                    _clickedLineId = _pickedPolygon.GetLineIdFromPoint(e.Location);
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
                    _pickedPolygon = SelectPolygon(e.Location);

                    if (!SelectAllMode)
                    {
                        _clickedVertexId = _pickedPolygon.GetVertexIdFromPoint(e.Location);
                        if (_clickedVertexId == -1)
                            _clickedLineId = _pickedPolygon.GetLineIdFromPoint(e.Location);
                    }
                    else
                        _pickedPolygon.Center = e.Location;
                }
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // if in create mode:
            if (_draggedVertexId != -1)
            {
                _pickedPolygon.SetPointForVertexId(_draggedVertexId, e.Location);
            }
            else if (EditMode && e.Button == MouseButtons.Left)
            {
                if (SelectAllMode)
                {
                    _pickedPolygon.MovePolygon(e.Location);
                }
                else if (_clickedVertexId != -1)
                {
                    _pickedPolygon.SetPointForVertexId(_clickedVertexId, e.Location);
                }
                else if (_clickedLineId != -1)
                {
                    _pickedPolygon.MoveLine(_clickedLineId, e.Location);
                }
            }
            pictureBox.Invalidate();

        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (EditMode)
            {
                if (e.Button == MouseButtons.Left)
                { _clickedVertexId = -1; _pickedPolygon = null; }
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            _polygon.Draw(_canvas);
            if (_polygon2 != null)
                _polygon2.Draw(_canvas);
            e.Graphics.DrawImage(_canvas, 0, 0, _canvas.Width, _canvas.Height);
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
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
                    case 'i':
                        var temp = PolygonClipping.SutherlandHodgman(_polygon, _polygon2);
                        if (temp == null)
                            MessageBox.Show("Neither of both polygons is convex!");
                        else
                        {
                            _polygon = temp;
                            _polygon2 = null;
                        }
                        break;
                }
        }
        #endregion

        #region ToolStrips
        private void AddVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _pickedPolygon.AddVertexToLine(_clickedLineId);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { _pickedPolygon.DeleteVertex(_clickedVertexId); }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You cannot delete any more vertices!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            pictureBox.Invalidate();
        }
        #endregion

        #region Utilities
        private void Reset()
        {
            _draggedVertexId = -1;
            _polygon = new Polygon(trackBar1.Value);
            _polygon2 = new Polygon(trackBar1.Value);
            _pickedPolygon = _polygon;
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
            if (CreateMode || Create2ndMode)
                Text = "Polygon Editor [Create Mode]";
            else
                Text = "Polygon Editor [Edit Mode]";
        }
        private Polygon SelectPolygon(Point location)
        {
            var i = _polygon.GetVertexIdFromPoint(location);
            var j = _polygon.GetLineIdFromPoint(location);
            if (i != -1 || j != -1)
                return _polygon;
            else return _polygon2;
        }
        #endregion

        private void FillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _polygon.FillPolygon();
        }
    }
}
