﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BezierCurves
{
    public partial class MainWindow : Form
    {
        private const int MAX_POINTS = 10;
        private const int POINT_SIZE = 10;
        private const int MAX_WIDTH = 300;
        private const int MAX_HEIGHT = 300;
        private Point _start;
        private Point _end;
        private List<PointF> _controlPoints;
        private PointF[] _bezierPoints;
        private PointF[] _bezierTangentVectors;
        private int _bezierPointsAmount;
        private Point _userImageUpperLeft;
        private State _state;
        private bool _isMouseDown;
        private int _draggedPoint;
        private Bitmap _pointsBmp;
        private Bitmap _bezierBmp;
        private Bitmap _userImageBox;
        private Bitmap _userImageOriginal;
        private Bitmap _userImageOriginalInBox;
        private Bitmap _userImageBoxRotated;
        private Graphics _pointsGraphics;
        private Graphics _bezierGraphics;
        private Graphics _userImageBoxGraphics;
        private int _movementAnimationParameter;
        private int _movementAnimationDelta;
        private System.Timers.Timer _bezierTimer;
        private int _rotationAnimationParameter;
        private int _rotationAnimationDelta;
        private System.Timers.Timer _rotationTimer;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _controlPoints = new List<PointF>(MAX_POINTS - 2);
            _state = State.AddingStart;
            _pointsBmp = new Bitmap(_pictureBox.Width, _pictureBox.Height);
            _bezierBmp = new Bitmap(_pictureBox.Width, _pictureBox.Height);
            _pointsGraphics = Graphics.FromImage(_pointsBmp);
            _bezierGraphics = Graphics.FromImage(_bezierBmp);
            _isMouseDown = false;
            _draggedPoint = -1;
            _movementAnimationParameter = 0;
            _movementAnimationDelta = 1;
            _rotationAnimationParameter = 0;
            _rotationAnimationDelta = 5;

            _bezierTimer = new System.Timers.Timer();
            _bezierTimer.Interval = 75;
            _bezierTimer.Elapsed += BezierTimer_Elapsed;
            _rotationTimer = new System.Timers.Timer();
            _rotationTimer.Interval = 100;
            _rotationTimer.Elapsed += RotationTimer_Elapsed;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_userImageBoxRotated != null)
            {
                e.Graphics.DrawImage(_userImageBox, _userImageUpperLeft);
            }
            e.Graphics.DrawImage(_pointsBmp, 0, 0);
            e.Graphics.DrawImage(_bezierBmp, 0, 0);
        }

        #region Adding Points
        private void AddStartPoint(Point location)
        {
            _start = location;

            _pointsGraphics.FillEllipse(Brushes.Black, _start.X - POINT_SIZE / 2, _start.Y - POINT_SIZE / 2, POINT_SIZE, POINT_SIZE);
            _pictureBox.Refresh();

            _state = State.AddingEnd;
        }

        private void AddEndPoint(Point location)
        {
            _end = location;

            _pointsGraphics.FillEllipse(Brushes.Black, _end.X - POINT_SIZE / 2, _end.Y - POINT_SIZE / 2, POINT_SIZE, POINT_SIZE);
            _pictureBox.Refresh();

            _state = State.AddingControl;
        }

        private void AddControlPoint(Point location)
        {
            if (_controlPoints.Count == MAX_POINTS)
            {
                _state = State.Moving;
                return;
            }
            _controlPoints.Add(location);
            DrawPoints();
            if (_controlPoints.Count >= 2)
                DrawBezier();
        }
        #endregion

        #region Bezier
        private void DrawPoints()
        {
            _pointsGraphics.Clear(Color.FromArgb(0));
            _pointsGraphics.FillEllipse(Brushes.Black, _start.X - POINT_SIZE / 2, _start.Y - POINT_SIZE / 2, POINT_SIZE, POINT_SIZE);
            _pointsGraphics.FillEllipse(Brushes.Black, _end.X - POINT_SIZE / 2, _end.Y - POINT_SIZE / 2, POINT_SIZE, POINT_SIZE);
            foreach (var p in _controlPoints)
                _pointsGraphics.FillEllipse(Brushes.Red, p.X - POINT_SIZE / 2, p.Y - POINT_SIZE / 2, POINT_SIZE, POINT_SIZE);
            _pictureBox.Refresh();
        }

        /// <summary>
        /// Draws a Bezier curve with its bounding box
        /// </summary>
        public void DrawBezier()
        {
            _bezierGraphics.Clear(Color.FromArgb(0));
            _bezierPointsAmount = 301;

            // Draw bounding box
            _bezierGraphics.DrawLine(Pens.Red, _start, _controlPoints[0]);
            _bezierGraphics.DrawLines(Pens.Red, _controlPoints.ToArray());
            _bezierGraphics.DrawLine(Pens.Red, _controlPoints[_controlPoints.Count - 1], _end);

            // Calculate Bezier curve
            var points = new PointF[_controlPoints.Count + 2];
            points[0] = _start;
            points[_controlPoints.Count+1] = _end;
            var curIndex = 1;
            foreach (var p in _controlPoints)
                points[curIndex++] = p;
            
            (_bezierPoints, _bezierTangentVectors) = points.BezierAlgorithm(_bezierPointsAmount-1);

            // Draw Bezier curve
            _bezierGraphics.DrawLines(Pens.Black, _bezierPoints);
            _pictureBox.Refresh();
        }

        private void StartMovementBtn_Click(object sender, EventArgs e)
        {
            if (!_bezierTimer.Enabled)
            {
                rotateBtn.Enabled = false;
                _bezierTimer.Start();
            }
            else
            {
                _bezierTimer.Stop();
                rotateBtn.Enabled = true;
            }
        }

        private void BezierTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _movementAnimationParameter = (_movementAnimationParameter + _movementAnimationDelta) % _bezierPointsAmount;
            if (_movementAnimationParameter == 0 || _movementAnimationParameter == _bezierPointsAmount - 1)
                _movementAnimationDelta *= -1;
            var originalF = _bezierPoints[_movementAnimationParameter];
            var original = new Point((int)originalF.X, (int)originalF.Y);
            _userImageUpperLeft = CalculateUserUpperLeft(original);

            if (rotateCheckbox.Checked)
            {
                var tangentVector = _bezierTangentVectors[_movementAnimationParameter];
                var angleRad = Math.Atan2(tangentVector.X, tangentVector.Y);
                var angleDeg = (angleRad * 180) / Math.PI;
                RotateImage(-(float)angleDeg);
            }
            DrawUserImage();
            _pictureBox.Refresh();
        }
        #endregion

        #region Mouse Interaction
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                switch (_state)
                {
                    case State.AddingStart:
                        AddStartPoint(e.Location);
                        break;
                    case State.AddingEnd:
                        AddEndPoint(e.Location);
                        break;
                    case State.AddingControl:
                        AddControlPoint(e.Location);
                        break;
                }
            else
            {
                AddControlPoint(e.Location);
                _state = State.Moving;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseDown || _state != State.Moving)
                return;
            switch (_draggedPoint)
            {
                case -1:
                    return;
                case 0:
                    _start = new Point(e.X, e.Y);
                    if (_userImageOriginal != null && !_bezierTimer.Enabled)
                        _userImageUpperLeft = CalculateUserUpperLeft(_start);
                    break;
                case 1:
                    _end = new Point(e.X, e.Y);
                    break;
                default:
                    _controlPoints[_draggedPoint - 2] = new Point(e.X, e.Y);
                    break;
            }
            DrawPoints();
            DrawBezier();
            _pictureBox.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDown = true;
            _draggedPoint = SelectPoint(e.Location);
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            _draggedPoint = -1;
        }

        private int SelectPoint(Point location)
        {
            if (IsContained(_start, location))
                return 0;
            if (IsContained(_end, location))
                return 1;
            for (int i = 0; i < _controlPoints.Count; i++)
            {
                if (IsContained(_controlPoints[i], location))
                    return i + 2;
            }
            return -1;

            bool IsContained(PointF point, Point loc)
            {
                return loc.X <= point.X + POINT_SIZE / 2
                    && loc.X >= point.X - POINT_SIZE / 2
                    && loc.Y <= point.Y + POINT_SIZE / 2
                    && loc.Y >= point.Y - POINT_SIZE / 2;
            }
        }
        #endregion

        #region User Image
        private void LoadImageBtn_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var image = new Bitmap(dialog.OpenFile());
                if (image.Width > MAX_WIDTH || image.Height > MAX_HEIGHT)
                {
                    float scale = Math.Min((float)MAX_WIDTH / image.Width, (float) MAX_HEIGHT / image.Height);
                    var scaledWidth = (int)(image.Width * scale);
                    var scaledHeight = (int)(image.Height * scale);
                    _userImageOriginal = new Bitmap(image, new Size(scaledWidth, scaledHeight));
                }
                else
                    _userImageOriginal = image;

                var width = _userImageOriginal.Width;
                var height = _userImageOriginal.Height;
                var d = Math.Sqrt(width * width + height * height);
                var boxWidth = (int)Math.Round(d);
                var boxHeight = (int)Math.Round(d);

                _userImageOriginalInBox = new Bitmap(boxWidth, boxHeight);
                _userImageBoxRotated = new Bitmap(boxWidth, boxHeight);

                if (!_start.IsEmpty)
                    _userImageUpperLeft = CalculateUserUpperLeft(_start);
                else
                    _userImageUpperLeft = CalculateUserUpperLeft(new Point(_pictureBox.Width / 2, _pictureBox.Height / 2));

                _userImageBox = new Bitmap(boxWidth, boxHeight);
                _userImageBoxGraphics = Graphics.FromImage(_userImageBox);
                using (var g = Graphics.FromImage(_userImageOriginalInBox))
                {
                    var x = (_userImageOriginalInBox.Width - _userImageOriginal.Width) / 2;
                    var y = (_userImageOriginalInBox.Height - _userImageOriginal.Height) / 2;
                    g.DrawImage(_userImageOriginal, x, y);
                }
                using (var g = Graphics.FromImage(_userImageBoxRotated))
                {
                    g.DrawImage(_userImageOriginalInBox, 0, 0);
                }

                DrawUserImage();
            }
            _pictureBox.Refresh();
        }

        private Point CalculateUserUpperLeft(Point source)
        {
            return new Point(source.X - _userImageOriginalInBox.Width / 2,
                source.Y - _userImageOriginalInBox.Height / 2);
        }

        /// <summary>
        /// Draws user image on a prepared, larger bitmap 
        /// </summary>
        private void DrawUserImage()
        {
            _userImageBoxGraphics.Clear(Color.FromArgb(0));
            _userImageBoxGraphics.DrawImage(_userImageBoxRotated, 0, 0);
            //_userImageBoxGraphics.DrawLines(Pens.Red, new Point[5]
            //{
            //    new Point(0,0),
            //    new Point(_userImageBox.Width-1, 0),
            //    new Point(_userImageBox.Width-1, _userImageBox.Height-1),
            //    new Point(0,_userImageBox.Height-1),
            //    new Point(0,0)
            //});
        }

        private void RotateBtn_Click(object sender, EventArgs e)
        {
            if (!_rotationTimer.Enabled)
            {
                _userImageUpperLeft = CalculateUserUpperLeft(new Point(_pictureBox.Width / 2, _pictureBox.Height / 2));
                _startMovementBtn.Enabled = false;
                _rotationTimer.Start();
            }
            else
            {
                _rotationTimer.Stop();
                _startMovementBtn.Enabled = true;
            }            
            _pictureBox.Refresh();
        }

        private void RotationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _rotationAnimationParameter = (_rotationAnimationParameter + _rotationAnimationDelta) % 360;
            RotateImage((float)_rotationAnimationParameter);
            DrawUserImage();
            _pictureBox.Refresh();
        }

        private void RotateImage(float angle)
        {
            if (matrixRadio.Checked)
                _userImageBoxRotated.RotateFromReferenceUsingRotationMatrix(_userImageOriginalInBox, angle);
            else if (shearRadio.Checked)
                _userImageBoxRotated = _userImageOriginalInBox.RotateImageUsingShearing(angle);
        }
        #endregion
    }

    enum State
    {
        AddingStart,
        AddingEnd,
        AddingControl,
        Moving
    }
}
