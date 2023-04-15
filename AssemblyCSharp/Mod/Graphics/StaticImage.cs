using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.Graphics
{
    public class StaticImage : IImage
    {
        public Image image;
        public StaticImage(string path, int width, int height)
        {
            Texture2D texture = new Texture2D(1, 1);
            image = new Image();
            image.w = width;
            image.h = height;
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] imageData = new byte[stream.Length];
            stream.Read(imageData, 0, imageData.Length);
            stream.Close();
            texture.LoadImage(imageData);
            if (width == -1 && height != -1) 
                width = texture.width * height / texture.height;
            else if (height == -1 && width != -1) 
                height = texture.height * width / texture.width;
            if ((texture.width != width || texture.height != height) && width != -1 && height != -1)
                texture = TextureScaler.ScaleTexture(texture, width, height);
            texture.anisoLevel = 0;
            texture.filterMode = FilterMode.Point;
            texture.mipMapBias = 0f;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            image.texture = texture;
        }

        public StaticImage(string path)
        {
            Texture2D texture = new Texture2D(1, 1);
            image = new Image();
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
            image.texture = texture;
            image.w = texture.width;
            image.h = texture.height;
        }

        public Texture2D[] Textures => new Texture2D[] { image.texture };

        public void Paint(mGraphics g, int x, int y)
        {
            g.drawImage(image, x, y);
        }
    }
}
