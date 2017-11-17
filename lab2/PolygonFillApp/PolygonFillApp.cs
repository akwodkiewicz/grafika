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
        private Bitmap _normalMap;
        private Bitmap _heightMap;
        private Bitmap _texture;
        private PolygonManager _polygonManager;
        private bool _selectAllMode = false;
        private (double X, double Y, double Z) _lightPosition;
        private double _sphereRadius;
        private (int X, int Y) _sphereCenter;
        private double _animationParameter = 0;

        public PolygonFillApp()
        {
            InitializeComponent();
            _canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            _lightPosition = (pictureBox.Width / 2, pictureBox.Height / 2, 0);
            RecalculateLight(pictureBox.Width / 2, pictureBox.Height / 2, true);
            lightPosHeightNumeric.Value = (decimal)_lightPosition.Z;
            _polygonManager = new PolygonManager()
            {
                SolidColor = fillSolidPic.BackColor,
                LightColor = lightColorPic.BackColor,
                LightPosition = _lightPosition
            };
            Init();
        }

        public void Init()
        {
            lightSphereRadiusNumeric.Value = Math.Min((decimal)(_lightPosition.Z), lightSphereRadiusNumeric.Maximum);

            var list = new List<Vertex>
            {
                new Vertex(new Point(100, 100)),
                new Vertex(new Point(550, 100)),
                new Vertex(new Point(450, 275)),
                new Vertex(new Point(480, 460)),
                new Vertex(new Point(480, 460)),
                new Vertex(new Point(100, 370)),
            };
            var poly = new Polygon(list)
            {
                Filled = true
            };
            _polygonManager.AddPolygon(poly);
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
                        pictureBox.Invalidate();
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
                SolidColor = fillSolidPic.BackColor,
                LightColor = lightColorPic.BackColor,
                LightPosition = _lightPosition,
                NormalMap = _normalMap,
                HeightMap = _heightMap,
            };
            _polygonManager.StartCreating();
            if (_texture != null)
            {
                _polygonManager.Texture = _texture;
                _polygonManager.FillType = FillType.Texture;
            }
            if (lightPosInfinity.Checked)
                _polygonManager.LightType = LightType.Directional;
            else
                _polygonManager.LightType = LightType.Point;

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
        #endregion



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
            if (_texture == null)
                OpenImage();
            if (_texture != null)
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
                _texture = new Bitmap(dialog.OpenFile());
                fillTexturePic.Image = _texture;
                _polygonManager.Texture = _texture;
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
            if (!((RadioButton)sender).Checked)
            {
                lightAnimationTimer.Stop();
                return;
            }
            else
            {
                lightAnimationTimer.Start();
                _polygonManager.LightType = LightType.Point;
            }
        }

        private void LightPosPointRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;
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
            else
            {
                _sphereRadius = SphereEquation.CalculateR(pictureBox);
                _sphereCenter = (pictureBox.Width / 2, pictureBox.Height / 2);
                lightSphereRadiusNumeric.Maximum = (decimal)(_sphereRadius - 1);
            }
            double z;
            if (!overrideHeightCheckBox.Checked)
                z = (double)lightPosHeightNumeric.Value;
            else
                z = SphereEquation.CalculateZ(x, y, pictureBox);
            _lightPosition = (x, y, z);
            if (_polygonManager != null)
                _polygonManager.LightPosition = _lightPosition;
            lightPosLabel.Text = $"({_lightPosition.X}, {_lightPosition.Y}, {(int)_lightPosition.Z})";
            PickLightSource = false;
        }

        private void LightAnimationTimer_Tick(object sender, EventArgs e)
        {
            _animationParameter = (_animationParameter + 0.2) % (2 * Math.PI);
            var x = _sphereCenter.X + Math.Sin(_animationParameter) * ((double)lightSphereRadiusNumeric.Value);
            var y = _sphereCenter.Y + Math.Cos(_animationParameter) * ((double)lightSphereRadiusNumeric.Value);
            RecalculateLight((int)x, (int)y, false);
            pictureBox.Invalidate();
        }

        private void OverrideHeightCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RecalculateLight((int)_lightPosition.X, (int)_lightPosition.Y, false);
            pictureBox.Invalidate();
        }

        private void LightPosHeightNumeric_ValueChanged(object sender, EventArgs e)
        {
            RecalculateLight((int)_lightPosition.X, (int)_lightPosition.Y, false);
            pictureBox.Invalidate();
        }
        #endregion

        #region NORMALMAP
        private void NormalMapButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _normalMap = new Bitmap(dialog.OpenFile());
                normalMapPic.Image = _normalMap;
            }
            if (normalMapImageRadio.Checked)
                _polygonManager.NormalMap = _normalMap;
            pictureBox.Invalidate();
        }

        private void NormalMapImageRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                _polygonManager.NormalMap = null;
                pictureBox.Invalidate();
                return;
            }
            if (_normalMap == null)
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
                };
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    normalMapNoneRadio.Checked = true;
                    return;
                }
                _normalMap = new Bitmap(dialog.OpenFile());

            }
            normalMapPic.Image = _normalMap;
            _polygonManager.NormalMap = _normalMap;
            pictureBox.Invalidate();
        }
        #endregion


        #region HEIGHTMAP
        private void HeightMapButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _heightMap = new Bitmap(dialog.OpenFile());
                heightMapPic.Image = _heightMap;
            }
            if (heightMapImageRadio.Checked)
                _polygonManager.HeightMap = _heightMap;
            pictureBox.Invalidate();
        }

        private void HeightMapImageRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                _polygonManager.HeightMap = null;
                pictureBox.Invalidate();
                return;
            }
            if (_heightMap == null)
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
                };
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    heightMapNoneRadio.Checked = true;
                    return;
                }
                _heightMap = new Bitmap(dialog.OpenFile());
            }
            heightMapPic.Image = _heightMap;
            _polygonManager.HeightMap = _heightMap;
            pictureBox.Invalidate();
        }
        #endregion

        #region PRESETS
        private void PresetButton1_Click(object sender, EventArgs e)
        {
            LoadPreset("texture1", "normal1", "bump1");  
        }
        private void PresetButton2_Click(object sender, EventArgs e)
        {
            LoadPreset("texture2", "normal2", "bump2");
        }
        private void PresetButton3_Click(object sender, EventArgs e)
        {
            LoadPreset("texture3", "normal3", "bump3");
        }
        private void LoadPreset(string textureName, string normalMapName, string heightMapName)
        {
            object t = Properties.Resources.ResourceManager.GetObject(textureName);
            if (t is Bitmap)
            {
                _texture = t as Bitmap;
                fillTexturePic.Image = _texture;
                fillTextureRadio.Checked = true;
                _polygonManager.Texture = _texture;
            }
            else
            {
                fillSolidRadio.Checked = true;
                _polygonManager.Texture = null;
            }

            object n = Properties.Resources.ResourceManager.GetObject(normalMapName);
            if (n is Bitmap)
            {
                _normalMap = n as Bitmap;
                normalMapPic.Image = _normalMap;
                normalMapImageRadio.Checked = true;
                _polygonManager.NormalMap = _normalMap;
            }
            else
            {
                normalMapNoneRadio.Checked = true;
                _polygonManager.NormalMap = null;
            }

            object h = Properties.Resources.ResourceManager.GetObject(heightMapName);
            if (h is Bitmap)
            {
                _heightMap = h as Bitmap;
                heightMapPic.Image = _heightMap;
                heightMapImageRadio.Checked = true;
                _polygonManager.HeightMap = _heightMap;
            }
            else
            {
                heightMapNoneRadio.Checked = true;
                _polygonManager.HeightMap = null;
            }
            lightPosPointRadio.Checked = true;
            pictureBox.Invalidate();
        }
        #endregion

        private void HelpButton_Click(object sender, EventArgs e)
        {
            var caption = "Help";
            var text = "Press Right Mouse Button near the polygon edge to open the context menu\n\n" +
                "C - Create new polygon\nA - Toggle Select All Mode\nL, LMB - Pick light source location\nV - Clip polygon";
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
