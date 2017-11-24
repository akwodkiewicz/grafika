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
        public MainWindow()
        {
            InitializeComponent();
            DrawBezier();
        }

        /// <summary>
        /// Draws a Bezier curve with its bounding box and control points
        /// </summary>
        public void DrawBezier()
        {
            var bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = bmp;
            var points = new Point[] 
            {
                new Point(100, 300),
                new Point(300, 100),
                new Point(500, 500),
                new Point(700, 300)
            };
            using (var g = Graphics.FromImage(bmp))
            {
                // Draw points defining the curve
                for (int i = 0; i < points.Length; i++)
                    g.FillEllipse(Brushes.Black, points[i].X - 5, points[i].Y - 5, 10, 10);
                // Draw bounding box
                g.DrawLines(Pens.Red, points);
                // Draw Bezier curve
                BezierAlgorithm(g, points);
                pictureBox.Refresh();
            }
        }

        ///<summary>Draws a Bezier curve using de Casteljau algorithm</summary>
        ///<param name="g">Graphics object, on which the method will draw the curve</param>
        ///<param name="points">Points defining the curve [start point, 1st control point, 2nd control point, ..., end point]</param>
        private void BezierAlgorithm(Graphics g, Point[] points)
        {
            const int segmentsNum = 200;
            int tParameter = 0;
            var pointsToDraw = new PointF[segmentsNum + 1];
            var counter = 0;

            var pointsF = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
                pointsF[i] = points[i];

            // Find all the points defining the segments that approximate the curve
            for (; tParameter < segmentsNum + 1; tParameter += 1)
                FindPoint((float)tParameter / segmentsNum, pointsF);

            g.DrawLines(Pens.Black, pointsToDraw);

            // De Casteljau recursive algorithm
            void FindPoint(float t, PointF[] pts)
            {
                if (pts.Length == 1)
                    pointsToDraw[counter++] = pts[0];
                else
                {
                    var newpts = new PointF[pts.Length - 1];
                    for (int i = 0; i < newpts.Length; i++)
                    {
                        float x = (1 - t) * pts[i].X + t * pts[i + 1].X;
                        float y = (1 - t) * pts[i].Y + t * pts[i + 1].Y;
                        newpts[i] = new PointF(x, y);
                    }
                    FindPoint(t, newpts);
                }
            }
        }
    }
}
