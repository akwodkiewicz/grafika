using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonApp
{
    public partial class AngleConstraintForm : Form
    {
        public AngleConstraintForm()
        {
            InitializeComponent();
        }

        private int angle;
        public int Angle { get => angle; set => angle = value; }

        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
            var text = textBox1.Text;
            if (int.TryParse(text, out int value) && value < 180 && value > 0)
                Angle = value;
            else
                e.Cancel = true;
        }
    }
}
