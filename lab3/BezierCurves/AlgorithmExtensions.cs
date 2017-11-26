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
        /// <param name="referenceImage">Image used for reference</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>
        /// Rotated bitmap
        /// </returns>
        public static void RotateFromReferenceUsingRotationMatrix(this Bitmap modified, Bitmap referenceImage, float angle)
        {
            BitmapData sourceData = referenceImage.LockBits(
                                        new Rectangle(0, 0, referenceImage.Width, referenceImage.Height),
                                        ImageLockMode.ReadOnly,
                                        PixelFormat.Format32bppArgb);

            byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            referenceImage.UnlockBits(sourceData);

            var imageBounds = new Rectangle(0, 0, referenceImage.Width, referenceImage.Height);
            var rad = angle * Math.PI / 180;
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);

            for (int row = 0; row < referenceImage.Height; row++)
            {
                for (int col = 0; col < referenceImage.Height; col++)
                {
                    var resultIndex = row * sourceData.Stride + col * 4;

                    var xc = referenceImage.Width / 2;
                    var yc = referenceImage.Height / 2;
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
            BitmapData resultData = modified.LockBits(
                                        new Rectangle(0, 0, modified.Width, modified.Height),
                                        ImageLockMode.WriteOnly,
                                        PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            modified.UnlockBits(resultData);
        }

        public static void RotateFromReferenceUsingShearing(this Bitmap modifiedImage, Bitmap referenceImage, float angle)
        {
            BitmapData sourceData = referenceImage.LockBits(
                                       new Rectangle(0, 0, referenceImage.Width, referenceImage.Height),
                                       ImageLockMode.ReadOnly,
                                       PixelFormat.Format32bppArgb);
            byte[] buffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, buffer, 0, buffer.Length);
            referenceImage.UnlockBits(sourceData);

            var imageWidth = referenceImage.Width;
            var imageHeight = referenceImage.Height;

            angle = angle % 360;
            if (angle < 0) angle += 360;
            if (angle >= 45 && angle < 135)
            {
                buffer = rotate90(buffer);
                angle = angle - 90;
            }
            else if(angle>=135 && angle < 225)
            {
                buffer = rotate180(buffer);
                angle = angle - 180;
            }
            else if (angle >= 225 && angle < 315)
            {
                buffer = rotate90(buffer, false);
                angle = angle - 270;
            }

            double rad = angle * Math.PI / 180;
            var a = -Math.Tan(rad / 2);
            var b = Math.Sin(rad);


            void xshear(byte[] array, double shear)
            {
                var xc = imageWidth / 2;
                var yc = imageHeight / 2;
                for (int y = 0; y < imageHeight; y++)
                {
                    var yt = y - yc;
                    var delta = (int)Math.Round(shear * yt);
                    if (delta < 0)
                    {
                        for (int x = 0; x < imageWidth; x++)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4;
                            var sourceIndex = y * sourceData.Stride + (x - delta) * 4;
                            if (x - delta < imageWidth)
                            {
                                array[resultIndex] = array[sourceIndex];
                                array[resultIndex + 1] = array[sourceIndex + 1];
                                array[resultIndex + 2] = array[sourceIndex + 2];
                                array[resultIndex + 3] = array[sourceIndex + 3];
                            }
                            else
                            {
                                array[resultIndex + 3] = 0;
                            }
                        }
                    }
                    else
                    {
                        for (int x = imageWidth - 1; x >= 0; x--)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4;
                            var sourceIndex = y * sourceData.Stride + (x - delta) * 4;
                            if (x - delta >= 0)
                            {
                                array[resultIndex] = array[sourceIndex];
                                array[resultIndex + 1] = array[sourceIndex + 1];
                                array[resultIndex + 2] = array[sourceIndex + 2];
                                array[resultIndex + 3] = array[sourceIndex + 3];
                            }
                            else
                            {
                                array[resultIndex + 3] = 0;
                            }
                        }
                    }
                }
            }
            void yshear(byte[] array, double shear)
            {
                var xc = imageWidth / 2;
                var yc = imageHeight / 2;
                for (int x = 0; x < imageWidth; x++)
                {
                    var xt = x - xc;
                    var delta = (int)Math.Round(shear * xt);
                    if (delta < 0)
                    {
                        for (int y = 0; y < imageHeight; y++)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4;
                            var sourceIndex = (y - delta) * sourceData.Stride + x * 4;
                            if (y - delta < imageHeight)
                            {
                                array[resultIndex] = array[sourceIndex];
                                array[resultIndex + 1] = array[sourceIndex + 1];
                                array[resultIndex + 2] = array[sourceIndex + 2];
                                array[resultIndex + 3] = array[sourceIndex + 3];
                            }
                            else
                            {
                                array[resultIndex + 3] = 0;
                            }
                        }
                    }
                    else
                    {
                        for (int y = imageHeight - 1; y >= 0; y--)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4;
                            var sourceIndex = (y - delta) * sourceData.Stride + x * 4;
                            if (y - delta >= 0)
                            {
                                array[resultIndex] = array[sourceIndex];
                                array[resultIndex + 1] = array[sourceIndex + 1];
                                array[resultIndex + 2] = array[sourceIndex + 2];
                                array[resultIndex + 3] = array[sourceIndex + 3];
                            }
                            else
                            {
                                array[resultIndex + 3] = 0;
                            }
                        }
                    }
                }
            }
            byte[] rotate90(byte[] array, bool plus=true)
            {
                byte[] result = new byte[array.Length];
                for (int y = 0; y < imageHeight; y++)
                    for (int x = 0; x < imageWidth; x++)
                        for (int i = 0; i < 4; i++)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4 + i;
                            var sourceIndex = (imageHeight-x-1)*sourceData.Stride + y*4 + i;
                            if(!plus)
                                sourceIndex = x * sourceData.Stride + (imageHeight - y - 1) * 4 + i;
                            result[resultIndex] = array[sourceIndex];
                        }
                return result;
            }
            byte[] rotate180(byte[] array)
            {
                byte[] result = new byte[array.Length];
                for (int y = 0; y < imageHeight; y++)
                    for (int x = 0; x < imageWidth; x++)
                        for (int i = 0; i < 4; i++)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4 + i;
                            var sourceIndex = (imageHeight - y - 1) * sourceData.Stride + (imageHeight - x - 1)* 4 + i;
                            result[resultIndex] = array[sourceIndex];
                        }
                return result;
            }

            xshear(buffer, a);
            yshear(buffer, b);
            xshear(buffer, a);

            BitmapData resultData = modifiedImage.LockBits(
                               new Rectangle(0, 0, modifiedImage.Width, modifiedImage.Height),
                               ImageLockMode.WriteOnly,
                               PixelFormat.Format32bppArgb);
            Marshal.Copy(buffer, 0, resultData.Scan0, buffer.Length);
            modifiedImage.UnlockBits(resultData);
        }

        public static void ShearImage(this Bitmap modified, Bitmap referenceImage, double shearX, double shearY)
        {
            BitmapData sourceData = referenceImage.LockBits(
                                        new Rectangle(0, 0, referenceImage.Width, referenceImage.Height),
                                        ImageLockMode.ReadOnly,
                                        PixelFormat.Format32bppArgb);

            byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            referenceImage.UnlockBits(sourceData);

            var imageBounds = new Rectangle(0, 0, referenceImage.Width, referenceImage.Height);

            int xOffset = (int)Math.Round(modified.Width * shearX / 2.0);
            int yOffset = (int)Math.Round(modified.Height * shearY / 2.0);

            int sourceXY = 0;
            int resultXY = 0;
            Point sourcePoint = new Point();
            Point resultPoint = new Point();

            for (int row = 0; row < modified.Height; row++)
            {
                for (int col = 0; col < modified.Width; col++)
                {
                    sourceXY = row * sourceData.Stride + col * 4;

                    sourcePoint.X = col;
                    sourcePoint.Y = row;

                    if (sourceXY >= 0 && sourceXY + 3 < sourceBuffer.Length)
                    {
                        resultPoint = sourcePoint.ShearXY(shearX, shearY, xOffset, yOffset);
                        resultXY = resultPoint.Y * sourceData.Stride + resultPoint.X * 4;

                        if (imageBounds.Contains(resultPoint))
                        {
                            // Blue
                            resultBuffer[resultXY] = sourceBuffer[sourceXY];
                            // Green
                            resultBuffer[resultXY + 1] = sourceBuffer[sourceXY + 1];
                            // Red
                            resultBuffer[resultXY + 2] = sourceBuffer[sourceXY + 2];
                            // Alpha
                            resultBuffer[resultXY + 3] = sourceBuffer[sourceXY + 3];
                        }
                    }
                }
            }


            BitmapData resultData = modified.LockBits(
                                       new Rectangle(0, 0, modified.Width, modified.Height),
                                       ImageLockMode.WriteOnly,
                                       PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            modified.UnlockBits(resultData);
        }

        private static Point ShearXY(this Point source, double shearX, double shearY, int offsetX, int offsetY)
        {
            Point result = new Point();

            result.X = (int)(Math.Round(source.X + shearX * source.Y));
            result.X -= offsetX;

            result.Y = (int)(Math.Round(source.Y + shearY * source.X));
            result.Y -= offsetY;

            return result;
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
    }

}
