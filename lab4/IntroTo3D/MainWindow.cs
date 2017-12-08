using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntroTo3D
{
    public partial class MainWindow : Form
    {
        List<Edge> _cube;
        MyMatrix _modelMatrix;
        MyMatrix _viewMatrix;
        MyMatrix _projectionMatrix;
        List<Edge> _listToDraw;

        public MainWindow()
        {
            InitializeComponent();


            CreateCube();
            CreateModelMatrix();
            CreateViewMatrix();
            CreateProjectionMatrix();

            TestMatrix();


            DrawOnScene(_cube, _modelMatrix, _viewMatrix, _projectionMatrix);
        }

        private void TestMatrix()
        {
            _viewMatrix.Multiply(new MyPoint3D(0, 0, 0));
        }

        public void CreateCube()
        {
            _cube = new List<Edge>
            {
                new Edge(new MyPoint3D(0,0,0), new MyPoint3D(1,0,0)),
                new Edge(new MyPoint3D(0,0,0), new MyPoint3D(0,1,0)),
                new Edge(new MyPoint3D(0,0,0), new MyPoint3D(0,0,1)),
                new Edge(new MyPoint3D(1,0,0), new MyPoint3D(1,1,0)),
                new Edge(new MyPoint3D(1,0,0), new MyPoint3D(1,0,1)),
                new Edge(new MyPoint3D(0,1,0), new MyPoint3D(0,1,1)),
                new Edge(new MyPoint3D(0,1,0), new MyPoint3D(1,1,0)),
                new Edge(new MyPoint3D(0,0,1), new MyPoint3D(0,1,1)),
                new Edge(new MyPoint3D(0,0,1), new MyPoint3D(1,0,1)),
                new Edge(new MyPoint3D(0,1,1), new MyPoint3D(1,1,1)),
                new Edge(new MyPoint3D(1,1,0), new MyPoint3D(1,1,1)),
                new Edge(new MyPoint3D(1,0,1), new MyPoint3D(1,1,1))
            };
        }

        public void CreateModelMatrix()
        {
            var data = new double[4, 4];
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 2] = 1;
            data[3, 3] = 1;
            _modelMatrix = new MyMatrix(data);
        }

        public void CreateViewMatrix()
        {
            var res = new double[4, 4];
            res[0, 0] = 0.1;
            res[0, 1] = 0.995;
            res[0, 2] = 3e-18;
            res[0, 3] = -0.498;
            res[1, 0] = -0.066;
            res[1, 1] = 0.007;
            res[1, 2] = 0.9978;
            res[1, 3] = -0.502;
            res[2, 0] = 0.993;
            res[2, 1] = -0.099;
            res[2, 2] = 0.06619;
            res[2, 3] = -3.005;
            res[3, 3] = 1;
            _viewMatrix = new MyMatrix(res);
        }

        public void CreateProjectionMatrix()
        {
            var data = new double[4, 4];
            data[0, 0] = 2.414;
            data[1, 1] = 2.414;
            data[2, 2] = -1.02;
            data[2, 3] = -2.02;
            data[3, 2] = -1;
            _projectionMatrix = new MyMatrix(data);
        }

        public void DrawOnScene(List<Edge> model, MyMatrix modelMatrix, MyMatrix viewMatrix, MyMatrix projectionMatrix)
        {
            var listToDraw = new List<Edge>();
            foreach (var edge in model)
            {
                var p = edge.Start;
                var p1 = modelMatrix.Multiply(p);
                var p2 = viewMatrix.Multiply(p1);
                var p3 = projectionMatrix.Multiply(p2);
                var p4 = new MyPoint3D(p3.X / p3.W, p3.Y / p3.W, p3.Z / p3.W);


                var r = edge.End;
                var r1 = modelMatrix.Multiply(r);
                var r2 = viewMatrix.Multiply(r1);
                var r3 = projectionMatrix.Multiply(r2);
                var r4 = new MyPoint3D(r3.X / r3.W, r3.Y / r3.W, r3.Z / r3.W);

                if(p4.Z == 0 || r4.Z == 0)
                {
                   
                }

                var p5 = new MyPoint3D((p4.X + 1) * pictureBox.Width/2, (p4.Y + 1) * pictureBox.Height/2, 0);
                var r5 = new MyPoint3D((r4.X + 1) * pictureBox.Width/2, (r4.Y + 1) * pictureBox.Height/2, 0);
                listToDraw.Add(new Edge(p5, r5));
            }

            _listToDraw = listToDraw;
            pictureBox.Invalidate();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if(_listToDraw!=null)
            {
                foreach (var edge in _listToDraw)
                {
                    e.Graphics.DrawLine(Pens.Black, (int)edge.Start.X, (int)edge.Start.Y, (int)edge.End.X, (int)edge.End.Y);
                }
                _listToDraw = null;
            }
        }
    }
}
