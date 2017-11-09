using PolygonApp.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.Algorithms
{
    static class PolygonClipping
    {
        public static Polygon SutherlandHodgman(Polygon subject, Polygon clip)
        {
            var sVertices = subject.Vertices.Where(v => v != null).ToArray();
            var cVertices = clip.Vertices.Where(v => v != null).ToArray();
            var output = new List<Vertex>(sVertices);
            for (int i = 0; i < cVertices.Length; i++)
            {
                Edge e = clip.Edges[i];
                Vertex pNext = cVertices[(i + 2) % cVertices.Length];

                var input = new List<Vertex>(output);
                output.Clear();
                var pp = input[input.Count - 1];
                foreach(var p in input)
                {
                    if(IsSameSide(p, pNext, e))
                    {
                        if (!(IsSameSide(pp, pNext, e)))
                            output.Add(GetIntersectionPoint(new Edge(pp, p), e));
                        output.Add(p);
                    }
                    else if (IsSameSide(pp, pNext, e))
                        output.Add(GetIntersectionPoint(new Edge(pp, p), e));
                    pp = p;
                }
            }
            for (int i = 0; i < output.Count - 1; i++)
            {
                if (output[i] == output[i + 1])
                {
                    output.RemoveAt(i + 1);
                    i--;
                }
            }
            return new Polygon(output);
        }

          
        public static Vertex GetIntersectionPoint(Edge e1, Edge e2)
        {
            Point direction1 = new Point(e1.End.X - e1.Start.X, e1.End.Y - e1.Start.Y);
            Point direction2 = new Point(e2.End.X - e2.Start.X, e2.End.Y - e2.Start.Y);
            double dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);

            Point c = new Point(e2.Start.X - e1.Start.X, e2.Start.Y - e1.Start.Y);
            double t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;

            return new Vertex(new Point(e1.Start.X + (int)(t * direction1.X), e1.Start.Y + (int)(t * direction1.Y)));
        }


        public static bool IsSameSide(Vertex p1, Vertex p2, Edge e)
        {
            long d1 = ((e.End.X - e.Start.X) * (p1.Y - e.Start.Y) - (e.End.Y - e.Start.Y) * (p1.X - e.Start.X));
            long d2 = ((e.End.X - e.Start.X) * (p2.Y - e.Start.Y) - (e.End.Y - e.Start.Y) * (p2.X - e.Start.X));

            return d1 * d2 >= 0;
        }

    }
}
