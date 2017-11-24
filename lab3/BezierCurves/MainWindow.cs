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
        private float[] _bezierTangents;
        private int _bezierPointsAmount;
        private Point _userImageUpperLeft;
        //private float _userImageAngle;
        private State _state;
        private bool _isMouseDown;
        private int _draggedPoint;
        private Bitmap _pointsBmp;
        private Bitmap _bezierBmp;
        private Bitmap _userImage;
        private Graphics _pointsGraphics;
        private Graphics _bezierGraphics;
        private Graphics _userImageGraphics;
        private int _animationParameter;
        private int _animationDelta;
        private System.Timers.Timer _timer;

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
            _animationParameter = 0;
            _animationDelta = 1;

            _timer = new System.Timers.Timer();
            _timer.Interval = 5;
            _timer.Elapsed += Timer_Elapsed;
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

        #region Drawing
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_userImage != null)
                e.Graphics.DrawImage(_userImage, _userImageUpperLeft);
            e.Graphics.DrawImage(_pointsBmp, 0, 0);
            e.Graphics.DrawImage(_bezierBmp, 0, 0);
        }

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
            var tangents = new float[segmentsNum + 1];
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
                        tangents[counter] = dy / dx;
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
        #endregion

        #region Mouse Events
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
                    if(!_timer.Enabled)
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
        #endregion

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
                _userImageUpperLeft = CalculateUserUpperLeft(_start);
            }
            _pictureBox.Refresh();
        }

        private Point CalculateUserUpperLeft(Point source)
        {
            return new Point(source.X - _userImage.Width / 2, source.Y - _userImage.Height / 2);
        }

        private void StartMovementBtn_Click(object sender, EventArgs e)
        {
            if (!_timer.Enabled)
                _timer.Start();
            else
                _timer.Stop();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _animationParameter = (_animationParameter + _animationDelta) % _bezierPointsAmount;
            if (_animationParameter == 0 || _animationParameter == _bezierPointsAmount - 1)
                _animationDelta *= -1;
            var originalF = _bezierPoints[_animationParameter];
            var original = new Point((int)originalF.X, (int)originalF.Y);
            _userImageUpperLeft = CalculateUserUpperLeft(original);
            //_userImageGraphics.TranslateTransform((float)_userImage.Width / 2, (float)_userImage.Height / 2);
            //_userImageGraphics.RotateTransform(_bezierTangents[_animationParameter]);
            //_userImageGraphics.TranslateTransform(-(float)_userImage.Width / 2, -(float)_userImage.Height / 2);
            //_userImageGraphics.DrawImage(_userImage, 0, 0);
            //_userImageGraphics.ResetTransform();
            _pictureBox.Refresh();
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
