﻿using System;
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
        private (double X, double Y, double Z) _lightPosition;

        public PolygonFillApp()
        {
            InitializeComponent();
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            RecalculateLight(pictureBox.Width/2,pictureBox.Height/2,false);
            _polygonManager = new PolygonManager()
            {
                VertexSize = trackBar1.Value,
                SolidColor = fillSolidPic.BackColor,
                LightColor = lightColorPic.BackColor,
                LightPosition = _lightPosition
            };
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

        public bool PickLightSource { get; private set; }
        #endregion

        #region PictureBox MouseInteraction
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {

            if (_polygonManager.State == ManagerState.Ready || _polygonManager.State == ManagerState.Creating)
                _polygonManager.AddVertex(e.Location);

            else if (PickLightSource)
                RecalculateLight(e.X, e.Y, false);
            
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
                    case 'l':
                        PickLightSource = true;
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
            _polygonManager = new PolygonManager()
            {
                VertexSize = trackBar1.Value,
                SolidColor = fillSolidPic.BackColor,
                LightColor = lightColorPic.BackColor,
                LightPosition = _lightPosition
            };
            SelectAllMode = false;
            PickLightSource = false;
            SetTitle();
            pictureBox.Invalidate();
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

        private void RadioButtonNormalImage_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                _polygonManager.NormalMap = null;
                return;
            }
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap normalMap = new Bitmap(dialog.OpenFile());
                _polygonManager.NormalMap = normalMap;
            }
        }

        #region FILL
        private void FillSolidRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;
            _polygonManager.FillType = FillType.Solid;
            pictureBox.Invalidate();
        }

        private void FillSolidButton_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _polygonManager.SolidColor = dialog.Color;
                fillSolidPic.BackColor = dialog.Color;
            }
        }

        private void FillTextureRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;
            if (fillTexturePic.Image == null)
                OpenImage();
            if (fillTexturePic.Image != null)
                _polygonManager.FillType = FillType.Texture;
            else
                fillSolidRadio.Checked = true;
            pictureBox.Invalidate();
        }

        private void FillTextureButton_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void OpenImage()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap texture = new Bitmap(dialog.OpenFile());
                fillTexturePic.Image = texture;
                _polygonManager.Texture = texture;
            }
        }
        #endregion

        #region LIGHT
        private void LightColorButton_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _polygonManager.LightColor = dialog.Color;
                lightColorPic.BackColor = dialog.Color;
            }
        }

        private void LightPosInfinity_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;
            _polygonManager.LightType = LightType.Directional;
            pictureBox.Invalidate();
        }

        private void LightPosAuto_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LightPosPointRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;
            // animate movement
            _polygonManager.LightType = LightType.Point;
            _polygonManager.LightPosition = _lightPosition;
            pictureBox.Invalidate();
        }

        private void PolygonFillApp_Resize(object sender, EventArgs e)
        {
            RecalculateLight(0, 0, true);
        }

        private void RecalculateLight(int x, int y, bool resizeOnly)
        {
            if (resizeOnly)
            {
                x = Math.Min((int)_lightPosition.X, pictureBox.Width); 
                y = Math.Min((int)_lightPosition.Y, pictureBox.Height);
            }
            var z = SphereEquation.CalculateZ(x, y, pictureBox);
            _lightPosition = (x, y, z);
            if(_polygonManager != null)
                _polygonManager.LightPosition = _lightPosition;
            lightPosLabel.Text = $"({_lightPosition.X}, {_lightPosition.Y}, {(int)_lightPosition.Z})";
            PickLightSource = false;
        }
        #endregion


    }
}
