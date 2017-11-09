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
        private PolygonManager _polygonManager;
        private bool _selectAllMode = false;

        public PolygonFillApp()
        {
            InitializeComponent();
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            _polygonManager = new PolygonManager(trackBar1.Value);
        }

        #region Properties
        public bool SelectAllMode
        {
            get => _selectAllMode;
            set
            {
                _selectAllMode = value;
                label4.Visible = value;
            }
        }
        #endregion

        #region PictureBox MouseInteraction
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_polygonManager.State == ManagerState.Ready || _polygonManager.State == ManagerState.Creating)
                _polygonManager.AddVertex(e.Location);

            else if (e.Button == MouseButtons.Right)
                switch (_polygonManager.Select(e.Location))
                {
                    case SelectResult.Vertex:
                        contextMenuStrip1.Show(pictureBox, e.Location);
                        break;
                    case SelectResult.Line:
                        contextMenuStrip2.Show(pictureBox, e.Location);
                        break;
                }
            SetTitle();
            pictureBox.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_polygonManager.State == ManagerState.Edit
             && e.Button == MouseButtons.Left)
                _polygonManager.StartMove(e.Location);
        }


        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_polygonManager.State == ManagerState.Creating)
                _polygonManager.MoveVertex(e.Location);

            else if (_polygonManager.State == ManagerState.Edit && e.Button == MouseButtons.Left)
            {
                if (SelectAllMode)
                    _polygonManager.MovePolygon(e.Location);
                else
                    _polygonManager.MoveVertexOrLine(e.Location);
            }
            pictureBox.Invalidate();
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (_polygonManager.State == ManagerState.Edit
             && e.Button == MouseButtons.Left)
                _polygonManager.StopMove();
        }
        #endregion

        #region KeyboardInteraction

        private void PolygonApp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && _polygonManager.State == ManagerState.Creating)
            {
                _polygonManager.StopCreating();
                pictureBox.Invalidate();
            }
        }

        private void PolygonApp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_polygonManager.State == ManagerState.Edit)
                switch (e.KeyChar)
                {
                    case 'a':
                        SelectAllMode = !SelectAllMode;
                        break;
                    case 'c':
                        _polygonManager.StartCreating();
                        SetTitle();
                        break;
                    case 'v':
                        try { _polygonManager.Clip(); }
                        catch (InvalidOperationException exc)
                        {
                            MessageBox.Show(exc.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    case 'r':
                        Reset();
                        break;
                }
        }
        #endregion

        #region ToolStrips
        private void AddVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _polygonManager.AddVertexToLine();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { _polygonManager.DeleteVertex(); }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You cannot delete any more vertices!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            pictureBox.Invalidate();
        }

        private void FillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _polygonManager.Fill();
        }
        #endregion

        #region Utilities
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            _polygonManager.Draw(_canvas);
            e.Graphics.DrawImage(_canvas, 0, 0, _canvas.Width, _canvas.Height);
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
        }
        private void Reset()
        {
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            _polygonManager = new PolygonManager(trackBar1.Value);
            SelectAllMode = false;
            SetTitle();
            pictureBox.Invalidate();
            label1.Focus();
        }
        private void SetTitle()
        {
            if (_polygonManager.State == ManagerState.Ready || _polygonManager.State == ManagerState.Creating)
                Text = "Polygon Editor [Create Mode]";
            else
                Text = "Polygon Editor [Edit Mode]";
        }
        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            _polygonManager.VertexSize = trackBar1.Value;
            pictureBox.Invalidate();
        }
        #endregion

    }
}
