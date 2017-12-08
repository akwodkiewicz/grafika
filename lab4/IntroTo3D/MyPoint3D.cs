using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroTo3D
{
    public struct MyPoint3D
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public MyPoint3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            W = 1;
        }

        public MyPoint3D(double[] vector)
        {
            X = vector[0];
            Y = vector[1];
            Z = vector[2];
            W = vector[3];
        }

        public double this[int key]
        {
            get
            {   switch(key)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return W;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public PointF ToPointF()
        {
            return new PointF((float)X, (float)Y);
        }
    }
}
