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

            var width = sourceData.Stride / 4;
            var height = sourceData.Height;
            var imageBounds = new Rectangle(0, 0, width, height);
            var rad = angle * Math.PI / 180;
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
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

            var imageWidth = sourceData.Stride / 4;
            var imageHeight = sourceData.Height;

            angle = angle % 360;
            if (angle < 0) angle += 360;
            if (angle >= 45 && angle < 135)
            {
                buffer = rotate90(buffer);
                angle = angle - 90;
            }
            else if (angle >= 135 && angle < 225)
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
                    var delta = (int)Math.Floor(shear * yt);
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
            byte[] rotate90(byte[] array, bool plus = true)
            {
                byte[] result = new byte[array.Length];
                for (int y = 0; y < imageHeight; y++)
                    for (int x = 0; x < imageWidth; x++)
                        for (int i = 0; i < 4; i++)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4 + i;
                            var sourceIndex = (imageHeight - x - 1) * sourceData.Stride + y * 4 + i;
                            if (!plus)
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
                            var sourceIndex = (imageHeight - y - 1) * sourceData.Stride + (imageHeight - x - 1) * 4 + i;
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

        public static void RotateFromReferenceUsingApproximatedShearingMono(this Bitmap modifiedImage, Bitmap referenceImage, float angle)
        {
            BitmapData sourceData = referenceImage.LockBits(
                                       new Rectangle(0, 0, referenceImage.Width, referenceImage.Height),
                                       ImageLockMode.ReadOnly,
                                       PixelFormat.Format32bppArgb);
            byte[] buffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, buffer, 0, buffer.Length);
            referenceImage.UnlockBits(sourceData);

            var imageWidth = sourceData.Stride / 4;
            var imageHeight = sourceData.Height;

            angle = angle % 360;
            if (angle < 0) angle += 360;
            if (angle >= 45 && angle < 135)
            {
                buffer = rotate90(buffer);
                angle = angle - 90;
            }
            else if (angle >= 135 && angle < 225)
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


            byte[] xshear(byte[] array, double shear)
            {
                var result = new byte[array.Length];
                var xc = imageWidth / 2;
                var yc = imageHeight / 2;
                for (int y = 0; y < imageHeight; y++)
                {
                    var yt = y - yc;
                    var delta = (int)Math.Floor(shear * yt);
                    var frac = (shear * yt) - delta;
                    var oleft = 0.0;

                    for (int x = 0; x < imageWidth; x++)
                    {
                        var resultIndex = y * sourceData.Stride + x * 4;
                        var sourceIndex = y * sourceData.Stride + (x - delta) * 4;
                        if (sourceIndex >= 0 && sourceIndex < array.Length)
                        {
                            var left = (array[sourceIndex] * frac);
                            var resultColor = (byte)(array[sourceIndex] - left + oleft);
                            result[resultIndex] = resultColor;
                            result[resultIndex + 1] = resultColor;
                            result[resultIndex + 2] = resultColor;
                            result[resultIndex + 3] = array[sourceIndex + 3];
                            oleft = left;
                        }
                        else
                        {
                            result[resultIndex + 3] = 0;
                            oleft = 0.0;
                        }
                    }
                }
                return result;
            }
            byte[] yshear(byte[] array, double shear)
            {
                var result = new byte[array.Length];
                var xc = imageWidth / 2;
                var yc = imageHeight / 2;
                for (int x = 0; x < imageWidth; x++)
                {
                    var xt = x - xc;
                    var delta = (int)Math.Floor(shear * xt);
                    var frac = (shear * xt) - delta;
                    var oleft = 0.0;

                    for (int y = 0; y < imageHeight; y++)
                    {
                        var resultIndex = y * sourceData.Stride + x * 4;
                        var sourceIndex = (y - delta) * sourceData.Stride + x * 4;
                        if (sourceIndex >= 0 && sourceIndex < array.Length)
                        {
                            var left = (array[sourceIndex] * frac);
                            var resultColor = (byte)(array[sourceIndex] - left + oleft);
                            result[resultIndex] = resultColor;
                            result[resultIndex + 1] = resultColor;
                            result[resultIndex + 2] = resultColor;
                            result[resultIndex + 3] = array[sourceIndex + 3];
                            oleft = left;
                        }
                        else
                        {
                            result[resultIndex + 3] = 0;
                            oleft = 0.0;
                        }
                    }
                }
                return result;
            }
            byte[] rotate90(byte[] array, bool plus = true)
            {
                byte[] result = new byte[array.Length];
                for (int y = 0; y < imageHeight; y++)
                    for (int x = 0; x < imageWidth; x++)
                        for (int i = 0; i < 4; i++)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4 + i;
                            var sourceIndex = (imageHeight - x - 1) * sourceData.Stride + y * 4 + i;
                            if (!plus)
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
                            var sourceIndex = (imageHeight - y - 1) * sourceData.Stride + (imageHeight - x - 1) * 4 + i;
                            result[resultIndex] = array[sourceIndex];
                        }
                return result;
            }


            buffer = xshear(yshear(xshear(buffer, a), b), a);

            BitmapData resultData = modifiedImage.LockBits(
                               new Rectangle(0, 0, modifiedImage.Width, modifiedImage.Height),
                               ImageLockMode.WriteOnly,
                               PixelFormat.Format32bppArgb);
            Marshal.Copy(buffer, 0, resultData.Scan0, buffer.Length);
            modifiedImage.UnlockBits(resultData);
        }

        public static void RotateFromReferenceUsingApproximatedShearingColor(this Bitmap modifiedImage, Bitmap referenceImage, float angle)
        {
            BitmapData sourceData = referenceImage.LockBits(
                           new Rectangle(0, 0, referenceImage.Width, referenceImage.Height),
                           ImageLockMode.ReadOnly,
                           PixelFormat.Format32bppArgb);
            byte[] buffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, buffer, 0, buffer.Length);
            referenceImage.UnlockBits(sourceData);

            var imageWidth = sourceData.Stride / 4;
            var imageHeight = sourceData.Height;

            angle = angle % 360;
            if (angle < 0) angle += 360;
            if (angle >= 45 && angle < 135)
            {
                buffer = rotate90(buffer);
                angle = angle - 90;
            }
            else if (angle >= 135 && angle < 225)
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

            byte[] rotate90(byte[] array, bool plus = true)
            {
                byte[] result = new byte[array.Length];
                for (int y = 0; y < imageHeight; y++)
                    for (int x = 0; x < imageWidth; x++)
                        for (int i = 0; i < 4; i++)
                        {
                            var resultIndex = y * sourceData.Stride + x * 4 + i;
                            var sourceIndex = (imageHeight - x - 1) * sourceData.Stride + y * 4 + i;
                            if (!plus)
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
                            var sourceIndex = (imageHeight - y - 1) * sourceData.Stride + (imageHeight - x - 1) * 4 + i;
                            result[resultIndex] = array[sourceIndex];
                        }
                return result;
            }


            byte[] xshear(byte[] array, double shear)
            {
                var result = new byte[array.Length];
                var xc = imageWidth / 2;
                var yc = imageHeight / 2;
                for (int y = 0; y < imageHeight; y++)
                {
                    var yt = y - yc;
                    var delta = (int)Math.Floor(shear * yt);
                    var frac = (shear * yt) - delta;
                    var oleftB = 0.0;
                    var oleftG = 0.0;
                    var oleftR = 0.0;

                    for (int x = 0; x < imageWidth; x++)
                    {
                        var resultIndex = y * sourceData.Stride + x * 4;
                        var sourceIndex = y * sourceData.Stride + (x - delta) * 4;
                        if (sourceIndex >= 0 && sourceIndex < array.Length)
                        {
                            var leftB = (array[sourceIndex] * frac);
                            var leftG = (array[sourceIndex + 1] * frac);
                            var leftR = (array[sourceIndex + 2] * frac);
                            var resultB = (byte)(array[sourceIndex] - leftB + oleftB);
                            var resultG = (byte)(array[sourceIndex + 1] - leftG + oleftG);
                            var resultR = (byte)(array[sourceIndex + 2] - leftR + oleftR);
                            result[resultIndex] = resultB;
                            result[resultIndex + 1] = resultG;
                            result[resultIndex + 2] = resultR;
                            result[resultIndex + 3] = array[sourceIndex + 3];
                            oleftB = leftB;
                            oleftG = leftG;
                            oleftR = leftR;
                        }
                        else
                        {
                            result[resultIndex + 3] = 0;
                            oleftB = 0.0;
                            oleftG = 0.0;
                            oleftR = 0.0;
                        }
                    }
                }
                return result;
            }

            byte[] yshear(byte[] array, double shear)
            {
                var result = new byte[array.Length];
                var xc = imageWidth / 2;
                var yc = imageHeight / 2;
                for (int x = 0; x < imageWidth; x++)
                {
                    var xt = x - xc;
                    var delta = (int)Math.Floor(shear * xt);
                    var frac = (shear * xt) - delta;
                    var oleftB = 0.0;
                    var oleftG = 0.0;
                    var oleftR = 0.0;

                    for (int y = 0; y < imageHeight; y++)
                    {
                        var resultIndex = y * sourceData.Stride + x * 4;
                        var sourceIndex = (y - delta) * sourceData.Stride + x * 4;
                        if (sourceIndex >= 0 && sourceIndex < array.Length)
                        {
                            var leftB = (array[sourceIndex] * frac);
                            var leftG = (array[sourceIndex + 1] * frac);
                            var leftR = (array[sourceIndex + 2] * frac);
                            var resultB = (byte)(array[sourceIndex] - leftB + oleftB);
                            var resultG = (byte)(array[sourceIndex + 1] - leftG + oleftG);
                            var resultR = (byte)(array[sourceIndex + 2] - leftR + oleftR);
                            result[resultIndex] = resultB;
                            result[resultIndex + 1] = resultG;
                            result[resultIndex + 2] = resultR;
                            result[resultIndex + 3] = array[sourceIndex + 3];
                            oleftB = leftB;
                            oleftG = leftG;
                            oleftR = leftR;
                        }
                        else
                        {
                            result[resultIndex + 3] = 0;
                            oleftB = 0.0;
                            oleftG = 0.0;
                            oleftR = 0.0;
                        }
                    }
                }
                return result;
            }


            buffer = xshear(yshear(xshear(buffer, a), b), a);

            BitmapData resultData = modifiedImage.LockBits(
                               new Rectangle(0, 0, modifiedImage.Width, modifiedImage.Height),
                               ImageLockMode.WriteOnly,
                               PixelFormat.Format32bppArgb);
            Marshal.Copy(buffer, 0, resultData.Scan0, buffer.Length);
            modifiedImage.UnlockBits(resultData);
        }
    }

}
