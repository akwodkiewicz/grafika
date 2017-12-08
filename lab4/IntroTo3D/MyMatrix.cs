using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroTo3D
{
    public class MyMatrix
    {
        double[,] _data;

        public MyMatrix(double[,] data)
        {
            if (data.Rank != 2)
                throw new InvalidOperationException("Matrix must me 4-dimentional!");
            _data = data;
        }

        public MyMatrix Multiply(MyMatrix m)
        {
            throw new NotImplementedException();
        }

        public MyPoint3D Multiply(MyPoint3D p)
        {
            var vector = new double[4];

            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j<4; j++)
                {
                    vector[i] += _data[i, j] * p[j];
                }
            }

            return new MyPoint3D(vector);
        }
    }
}
