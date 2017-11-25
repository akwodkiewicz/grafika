using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurves
{
    public static class AlgorithmExtensions
    {
        ///<summary>Calculates a Bezier curve from given points, using de Casteljau algorithm</summary>
        ///<param name="points">Points defining the curve [start point, 1st control point, 2nd control point, ..., end point]</param>
        ///<param name="numberOfSegments">Number of segments approximating the curve</param>
        ///<returns>
        ///Tuple containing: 
        ///- points approximating the Bezier curve 
        ///- vectors tangent to those points 
        ///</returns>
        public static (PointF[] pointsToDraw, PointF[] tangentVectors) BezierAlgorithm(this PointF[] points, int numberOfSegments)
        {
            var curIndex = 0;
            var pointsToDraw = new PointF[numberOfSegments + 1];
            var tangentVectors = new PointF[numberOfSegments + 1];

            // Find all the points defining the segments that approximate the curve
            for (int tParameter = 0; tParameter < numberOfSegments + 1; tParameter += 1)
                Casteljau((float)tParameter / numberOfSegments, points);

            // Result
            return (pointsToDraw, tangentVectors);

            /// De Casteljau recursive algorithm
            void Casteljau(float t, PointF[] pts)
            {
                if (pts.Length == 1)
                    pointsToDraw[curIndex++] = pts[0];
                else
                {
                    if (pts.Length == 2)
                    {
                        var dx = pts[1].X - pts[0].X;
                        var dy = pts[1].Y - pts[0].Y;
                        tangentVectors[curIndex] = new PointF(dx, dy);
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

        /// <summary>
        /// Rotate user image clockwise by a given angle, using rotation matrix calculations
        /// </summary>
        /// <param name="original">Original image</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>
        /// Rotated bitmap
        /// </returns>
        public static Bitmap RotateImageUsingRotationMatrix(this Bitmap original, float angle)
        {
            var resultBitmap = new Bitmap(original.Width, original.Height);
            var rad = angle * Math.PI / 180;
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);
            for (int y = 0; y < resultBitmap.Height; y++)
            {
                for (int x = 0; x < resultBitmap.Height; x++)
                {
                    var xc = original.Width / 2;
                    var yc = original.Height / 2;
                    var xt = x - xc;
                    var yt = y - yc;
                    var xr = xt * cos + yt * sin;
                    var yr = -xt * sin + yt * cos;
                    var x2 = xr + xc;
                    var y2 = yr + yc;
                    x2 = Math.Round(x2);
                    y2 = Math.Round(y2);
                    if (x2 < 0 || x2 >= original.Width || y2 < 0 || y2 >= original.Height)
                        resultBitmap.SetPixel(x, y, Color.Transparent);
                    else
                        resultBitmap.SetPixel(x, y, original.GetPixel((int)x2, (int)y2));
                }
            }
            return resultBitmap;
        }
    }

}
