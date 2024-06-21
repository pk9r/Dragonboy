using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.Background
{
    internal class StaticImage : IBackground
    {
        Texture2D texture = new Texture2D(1, 1);
        bool isLoaded;
        ScaleMode _scaleMode = ScaleMode.StretchToFill;
        public ScaleMode ScaleMode
        {
            get => _scaleMode;
            set => _scaleMode = value;
        }

        internal StaticImage(string path)
        {
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] imageData = new byte[stream.Length];
            stream.Read(imageData, 0, imageData.Length);
            stream.Close();
            texture.LoadImage(imageData);
            texture.anisoLevel = 0;
            texture.filterMode = FilterMode.Point;
            texture.mipMapBias = 0f;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            isLoaded = true;
        }

        public Texture2D[] Textures => new Texture2D[] { texture };

        public bool IsLoaded => isLoaded;

        public void Paint(mGraphics g, int x, int y)
        {
            GUI.DrawTexture(new Rect(x, y, Screen.width, Screen.height), texture, _scaleMode);
        }
    }
}
