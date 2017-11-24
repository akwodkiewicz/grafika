using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private Point _start;
        private Point _end;
        private List<Point> _controlPoints;
        private PointF[] _bezierPoints;
        private (float X, float Y)[] _bezierTangents;
        private int _bezierPointsAmount;
        private Point _userImageUpperLeft;
        //private float _userImageAngle;
        private State _state;
        private bool _isMouseDown;
        private int _draggedPoint;
        private Bitmap _pointsBmp;
        private Bitmap _bezierBmp;
        private Bitmap _userImage;
        private Bitmap _userImageBox;
        private Graphics _pointsGraphics;
        private Graphics _bezierGraphics;
        private Graphics _userImageGraphics;
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
            _controlPoints = new List<Point>(MAX_POINTS - 2);
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
            _bezierTimer.Interval = 5;
            _bezierTimer.Elapsed += BezierTimer_Elapsed;
            _rotationTimer = new System.Timers.Timer();
            _rotationTimer.Interval = 25;
            _rotationTimer.Elapsed += RotationTimer_Elapsed;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_userImageBox != null)
                e.Graphics.DrawImage(_userImageBox, _userImageUpperLeft);
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
            var controlPointsArray = _controlPoints.ToArray();

            // Draw bounding box
            _bezierGraphics.DrawLine(Pens.Red, _start, _controlPoints[0]);
            _bezierGraphics.DrawLines(Pens.Red, controlPointsArray);
            _bezierGraphics.DrawLine(Pens.Red, _controlPoints[_controlPoints.Count - 1], _end);
            // Draw Bezier curve
            BezierAlgorithm(_start, _end, controlPointsArray);
            _bezierGraphics.DrawLines(Pens.Black, _bezierPoints);

            _pictureBox.Refresh();
        }

        ///<summary>Calculates a Bezier curve using de Casteljau algorithm</summary>
        ///<param name="controlPoints">Points defining the curve [start point, 1st control point, 2nd control point, ..., end point]</param>
        private void BezierAlgorithm(Point start, Point end, Point[] controlPoints)
        {
            const int segmentsNum = 300;
            int tParameter = 0;
            var pointsToDraw = new PointF[segmentsNum + 1];
            var tangents = new(float X, float Y)[segmentsNum + 1];
            var counter = 0;

            var pointsF = new PointF[controlPoints.Length + 2];
            pointsF[0] = start;
            pointsF[pointsF.Length - 1] = end;
            for (int i = 0; i < controlPoints.Length; i++)
                pointsF[i + 1] = controlPoints[i];

            // Find all the points defining the segments that approximate the curve
            for (; tParameter < segmentsNum + 1; tParameter += 1)
                Casteljau((float)tParameter / segmentsNum, pointsF);


            _bezierPoints = pointsToDraw;
            _bezierPointsAmount = segmentsNum + 1;
            _bezierTangents = tangents;
            // De Casteljau recursive algorithm
            void Casteljau(float t, PointF[] pts)
            {
                if (pts.Length == 1)
                    pointsToDraw[counter++] = pts[0];
                else
                {
                    if (pts.Length == 2)
                    {
                        var dx = pts[1].X - pts[0].X;
                        var dy = pts[1].Y - pts[0].Y;
                        tangents[counter] = (dx, dy);
                    }
                    var newpts = new PointF[pts.Length - 1];
                    for (int i = 0; i < newpts.Length; i++)
                    {
                        float x = (1 - t) * pts[i].X + t * pts[i + 1].X;
                        float y = (1 - t) * pts[i].Y + t * pts[i + 1].Y;
                        newpts[i] = new PointF(x, y);
                    }
                    Casteljau(t, newpts);
                }
            }
        }

        private void StartMovementBtn_Click(object sender, EventArgs e)
        {
            if (!_bezierTimer.Enabled)
                _bezierTimer.Start();
            else
                _bezierTimer.Stop();
        }

        private void BezierTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _movementAnimationParameter = (_movementAnimationParameter + _movementAnimationDelta) % _bezierPointsAmount;
            if (_movementAnimationParameter == 0 || _movementAnimationParameter == _bezierPointsAmount - 1)
                _movementAnimationDelta *= -1;
            var originalF = _bezierPoints[_movementAnimationParameter];
            var original = new Point((int)originalF.X, (int)originalF.Y);
            _userImageUpperLeft = CalculateUserUpperLeft(original);

            var angleRad = Math.Atan2(_bezierTangents[_movementAnimationParameter].X, _bezierTangents[_movementAnimationParameter].Y);
            var angleDeg = (angleRad * 180) / Math.PI;
            RotateImage(-(float)angleDeg);
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
                    if (!_bezierTimer.Enabled)
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

            bool IsContained(Point point, Point loc)
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
                _userImage = new Bitmap(dialog.OpenFile());
                _userImageGraphics = Graphics.FromImage(_userImage);

                var cos45 = Math.Cos(Math.PI / 4);
                var sin45 = Math.Sin(Math.PI / 4);
                var boxWidth = (int)Math.Round(_userImage.Width * cos45 + _userImage.Height * sin45);
                var boxHeight = (int)Math.Round(_userImage.Width * sin45 + _userImage.Height * cos45);
                _userImageBox = new Bitmap(boxWidth, boxHeight);
                _userImageBoxGraphics = Graphics.FromImage(_userImageBox);

                _userImageUpperLeft = CalculateUserUpperLeft(_start);
                DrawUserImage();
            }
            _pictureBox.Refresh();
        }

        private Point CalculateUserUpperLeft(Point source)
        {
            return new Point(source.X - _userImageBox.Width / 2,
                source.Y - _userImageBox.Height/ 2);
        }

        /// <summary>
        /// Draws user image on a prepared, larger bitmap 
        /// </summary>
        private void DrawUserImage()
        {
            _userImageBoxGraphics.Clear(Color.FromArgb(0));
            var x = (_userImageBox.Width - _userImage.Width) / 2;
            var y = (_userImageBox.Height - _userImage.Height) / 2;
            _userImageBoxGraphics.DrawImage(_userImage, x, y);
        }

        private void RotateBtn_Click(object sender, EventArgs e)
        {
            _userImageUpperLeft = CalculateUserUpperLeft(new Point(_pictureBox.Width / 2, _pictureBox.Height / 2));

            if (!_rotationTimer.Enabled)
                _rotationTimer.Start();
            else
                _rotationTimer.Stop();
            _pictureBox.Refresh();
        }

        private void RotationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _rotationAnimationParameter = (_rotationAnimationParameter + _rotationAnimationDelta) % 360;
            RotateImage((float)_rotationAnimationParameter);
            _pictureBox.Refresh();
        }

        /// <summary>
        /// Rotate user image using .NET's internal transformations and rotations
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        private void RotateImage(float angle)
        {
            _userImageBoxGraphics.TranslateTransform((float)_userImageBox.Width / 2, (float)_userImageBox.Height / 2);
            _userImageBoxGraphics.RotateTransform(angle);
            _userImageBoxGraphics.TranslateTransform(-(float)_userImageBox.Width / 2, -(float)_userImageBox.Height / 2);
            DrawUserImage();
            _userImageBoxGraphics.ResetTransform();
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
