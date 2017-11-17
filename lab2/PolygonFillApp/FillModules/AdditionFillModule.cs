using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    class AdditionFillModule : IFillModule
    {
        private List<IFillModule> _modules;
        public AdditionFillModule(params IFillModule[] modules)
        {
            _modules = new List<IFillModule>(modules);
        }

        public Color GetColor(int x, int y)
        {
            var color = Color.FromArgb(0, 0, 0);
            foreach (var m in _modules)
            {
                color = Color.FromArgb
                    (
                        Math.Min(m.GetColor(x, y).R + color.R, 255),
                        Math.Min(m.GetColor(x, y).G + color.G, 255),
                        Math.Min(m.GetColor(x, y).B + color.B, 255)
                    );
            }
            return color;
        }
    }
}
