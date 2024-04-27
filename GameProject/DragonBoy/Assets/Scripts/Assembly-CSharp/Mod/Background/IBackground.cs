using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.Background
{
    public interface IBackground
    {
        void Paint(mGraphics g, int x, int y);

        Texture2D[] Textures { get; }

        bool IsLoaded { get; }

        ScaleMode ScaleMode { get; set; }
    }
}
