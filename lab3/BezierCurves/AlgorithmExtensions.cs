using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
        /// <param name="sourceBitmap">Original image</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>
        /// Rotated bitmap
        /// </returns>
        public static Bitmap RotateImageUsingRotationMatrix(this Bitmap sourceBitmap, float angle)
        {
            BitmapData sourceData = sourceBitmap.LockBits(
                                        new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly,
                                        PixelFormat.Format32bppArgb);

            byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            var imageBounds = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            var rad = angle * Math.PI / 180;
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);

            for (int row = 0; row < sourceBitmap.Height; row++)
            {
                for (int col = 0; col < sourceBitmap.Height; col++)
                {
                    var resultIndex = row * sourceData.Stride + col * 4;

                    var xc = sourceBitmap.Width / 2;
                    var yc = sourceBitmap.Height / 2;
                    var xt = col - xc;
                    var yt = row - yc;
                    var xr = xt * cos + yt * sin;
                    var yr = -xt * sin + yt * cos;
                    var x2 = (int)Math.Round(xr + xc);
                    var y2 = (int)Math.Round(yr + yc);

                    var sourceIndex = y2 * sourceData.Stride + x2 * 4;

                    if (imageBounds.Contains(x2, y2))
                    {
                        // Blue
                        resultBuffer[resultIndex] = sourceBuffer[sourceIndex];
                        // Green
                        resultBuffer[resultIndex + 1] = sourceBuffer[sourceIndex + 1];
                        // Red
                        resultBuffer[resultIndex + 2] = sourceBuffer[sourceIndex + 2];
                        // Alpha
                        resultBuffer[resultIndex + 3] = sourceBuffer[sourceIndex + 3];
                    }
                }
            }

            var resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(
                                        new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly,
                                        PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public static Bitmap RotateImageUsingShearing(this Bitmap original, float angle)
        {
            var rad = (angle * 180) / Math.PI;
            var a = -Math.Tan(rad / 2);
            var b = Math.Sin(rad);
            return original.ShearImage(a, 0).ShearImage(0, b).ShearImage(a, 0);
        }

        public static Bitmap ShearImage(this Bitmap sourceBitmap,
                               double shearX,
                               double shearY)
        {
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];


            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            int xOffset = (int)Math.Round(sourceBitmap.Width *
                                                shearX / 2.0);


            int yOffset = (int)Math.Round(sourceBitmap.Height *
                                                  shearY / 2.0);


            int sourceXY = 0;
            int resultXY = 0;


            Point sourcePoint = new Point();
            Point resultPoint = new Point();


            Rectangle imageBounds = new Rectangle(0, 0,
                                    sourceBitmap.Width,
                                   sourceBitmap.Height);


            for (int row = 0; row < sourceBitmap.Height; row++)
            {
                for (int col = 0; col < sourceBitmap.Width; col++)
                {
                    sourceXY = row * sourceData.Stride + col * 4;


                    sourcePoint.X = col;
                    sourcePoint.Y = row;


                    if (sourceXY >= 0 &&
                        sourceXY + 3 < pixelBuffer.Length)
                    {
                        resultPoint = sourcePoint.ShearXY(shearX,
                                        shearY, xOffset, yOffset);


                        resultXY = resultPoint.Y * sourceData.Stride +
                                   resultPoint.X * 4;


                        if (imageBounds.Contains(resultPoint) &&
                                              resultXY >= 0)
                        {
                            if (resultXY + 6 <= resultBuffer.Length)
                            {
                                resultBuffer[resultXY + 4] =
                                     pixelBuffer[sourceXY];


                                resultBuffer[resultXY + 5] =
                                     pixelBuffer[sourceXY + 1];


                                resultBuffer[resultXY + 6] =
                                     pixelBuffer[sourceXY + 2];


                                resultBuffer[resultXY + 7] = 255;
                            }


                            if (resultXY - 3 >= 0)
                            {
                                resultBuffer[resultXY - 4] =
                                     pixelBuffer[sourceXY];


                                resultBuffer[resultXY - 3] =
                                     pixelBuffer[sourceXY + 1];


                                resultBuffer[resultXY - 2] =
                                     pixelBuffer[sourceXY + 2];


                                resultBuffer[resultXY - 1] = 255;
                            }


                            if (resultXY + 3 < resultBuffer.Length)
                            {
                                resultBuffer[resultXY] =
                                 pixelBuffer[sourceXY];


                                resultBuffer[resultXY + 1] =
                                 pixelBuffer[sourceXY + 1];


                                resultBuffer[resultXY + 2] =
                                 pixelBuffer[sourceXY + 2];


                                resultBuffer[resultXY + 3] = 255;
                            }
                        }
                    }
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        private static Point ShearXY(this Point source, double shearX,
                                               double shearY,
                                               int offsetX,
                                               int offsetY)
        {
            Point result = new Point();

            result.X = (int)(Math.Round(source.X + shearX * source.Y));
            result.X -= offsetX;

            result.Y = (int)(Math.Round(source.Y + shearY * source.X));
            result.Y -= offsetY;

            return result;
        }
    }

}
