using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp.Algorithms
{
    static class SphereEquation
    {
        public static double CalculateZ(double x, double y, PictureBox pictureBox, double z_0=0)
        {
            var x_0 = pictureBox.Width / 2;
            var y_0 = pictureBox.Height / 2;
            var r = Math.Sqrt(x_0 * x_0 + y_0 * y_0);
            return Math.Sqrt(r * r - (x - x_0) * (x - x_0) - (y - y_0) * (y - y_0)) + z_0;
        }

        public static double CalculateR(PictureBox pictureBox)
        {
            var x_0 = pictureBox.Width / 2;
            var y_0 = pictureBox.Height / 2;
            return Math.Sqrt(x_0 * x_0 + y_0 * y_0);
        }
    }
}
