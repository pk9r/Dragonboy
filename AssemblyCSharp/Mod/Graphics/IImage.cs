using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.Graphics
{
    public interface IImage
    {
        void Paint(mGraphics g, int x, int y);

        Texture2D[] Textures { get; }
    }
}
