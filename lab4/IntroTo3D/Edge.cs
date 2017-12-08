using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroTo3D
{
    public struct Edge
    {
        public MyPoint3D Start;
        public MyPoint3D End;

        public Edge(MyPoint3D start, MyPoint3D end)
        {
            Start = start;
            End = end;
        }
    }
}
