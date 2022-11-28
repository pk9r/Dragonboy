 using System.Collections.Generic;
 using System.Drawing;
 using System;
 using System.Drawing.Imaging;
 using System.Runtime.InteropServices;
 using UnityEngine;

namespace Mod.Graphics
{
    public class Gif
    {
        System.Drawing.Image gifImage;
        FrameDimension dimension;  
        List<Texture2D> frames;
        public List<Image> images;
        public float delay = 0.1f;
        int frameCount;
        long lastTimePaintAFrame;

        public Gif(string filepath)
        {
            gifImage = System.Drawing.Image.FromFile(filepath);
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            lastTimePaintAFrame = mSystem.currentTimeMillis();
            frames = GetFrames();
            images = new List<Image>();
            foreach (Texture2D texture2D in frames)
            {
                Image image = new Image();
                image.texture = texture2D;
                image.w = texture2D.width;
                image.h = texture2D.height;
                images.Add(image);
            }
        }

        static byte[] Bitmap2RawBytes(Bitmap bmp)
        {
            byte[] bytes;
            byte[] copyToBytes;
            BitmapData bitmapData;
            IntPtr Iptr = IntPtr.Zero;

            bytes = new byte[bmp.Width * bmp.Height * 4];
            copyToBytes = new byte[bmp.Width * bmp.Height * 4];

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            Iptr = bitmapData.Scan0;
            Marshal.Copy(Iptr, bytes, 0, bytes.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                copyToBytes[bytes.Length - 1 - i] = bytes[i];
            }
            bmp.UnlockBits(bitmapData);

            return copyToBytes;
        }

        List<Texture2D> GetFrames()
        {
            List<Texture2D> gifFrames = new List<Texture2D>();
            for (int i = 0; i < gifImage.GetFrameCount(dimension); i++)
            {
                gifImage.SelectActiveFrame(dimension, i);
                PropertyItem item = gifImage.GetPropertyItem(0x5100);
                int frameDelay = (item.Value[0] + item.Value[1] * 256) * 10;
                delay = frameDelay / 1000f;
                var frame = new Bitmap(gifImage.Width, gifImage.Height);
                System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, System.Drawing.Point.Empty);
                Texture2D texture = new Texture2D(frame.Width, frame.Height, TextureFormat.ARGB32, false);
                texture.LoadRawTextureData(Bitmap2RawBytes(frame));
                texture.Apply();
                gifFrames.Add(texture);
            }
            return gifFrames;
        }

        public void Paint(mGraphics g, int x, int y)
        {
            g.drawImage(images[frameCount], x, y);
            if (mSystem.currentTimeMillis() - lastTimePaintAFrame > delay * 1000)
            {
                lastTimePaintAFrame = mSystem.currentTimeMillis();
                frameCount++;
                if (frameCount >= images.Count)
                    frameCount = 0;
            }
        }
    }
}