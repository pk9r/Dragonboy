using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.Graphics
{
    public interface IBackground
    {
        void Paint(mGraphics g, int x, int y);
    }
}
