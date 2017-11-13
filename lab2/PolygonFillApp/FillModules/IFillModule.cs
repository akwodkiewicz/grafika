using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonApp.FillModules
{
    public interface IFillModule
    {
        Color GetColor(int x, int y);
    }
}
