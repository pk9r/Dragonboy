 using System.Collections.Generic;
 using System.Drawing;
 using System;
 using System.Drawing.Imaging;
 using System.Runtime.InteropServices;
 using UnityEngine;
using System.Linq;
using System.Threading;
using System.Drawing.Drawing2D;

namespace Mod.Graphics
{
    public class GifImage : IImage
    {
        System.Drawing.Image gifImage;
        FrameDimension dimension;  
        List<Texture2D> frames;
        Stack<int> applyIndex = new Stack<int>();
        public float[] delays;
        public int paintFrameIndex;
        long lastTimePaintAFrame;
        int frameIndex;
        bool isLocking;
        public bool isFullyLoaded;
        public float speed = 1f;

        public Texture2D[] Textures => frames.ToArray();

        public GifImage(string filepath)
        {
            gifImage = System.Drawing.Image.FromFile(filepath);
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            lastTimePaintAFrame = mSystem.currentTimeMillis();
            GetDelay();
            frames = GetEmptyFrames();
        }

        public GifImage(string filepath, int width, int height)
        {
            gifImage = System.Drawing.Image.FromFile(filepath);
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            lastTimePaintAFrame = mSystem.currentTimeMillis();
            GetDelay();
            frames = GetEmptyFrames(width, height);
        }

        public void LoadFrameGif()
        {
            if (isLocking)
                return;
            try
            {
                isLocking = true;
                gifImage.SelectActiveFrame(dimension, frameIndex);
                Bitmap frame = new Bitmap(gifImage.Width, gifImage.Height);
                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(frame);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(gifImage, System.Drawing.Point.Empty);
                if (gifImage.Width != frames[frameIndex].width || gifImage.Height != frames[frameIndex].height)
                    frames[frameIndex].LoadRawTextureData(Bitmap2RawBytes(TextureScaler.ResizeImage(frame, frames[frameIndex].width, frames[frameIndex].height)));
                else
                    frames[frameIndex].LoadRawTextureData(Bitmap2RawBytes(frame));
                applyIndex.Push(frameIndex);
                frameIndex++;
                isLocking = false;
            }
            catch (Exception)
            { }
        }

        public void FixedUpdate()
        {
            if (frameIndex >= frames.Count && applyIndex.Count == 0)
            {
                if (!isFullyLoaded)
                    isFullyLoaded = true;
            }
            if (applyIndex.Count > 0)
                frames[applyIndex.Pop()].Apply();
        }

        static byte[] Bitmap2RawBytes(Bitmap bmp)
        {
            byte[] bytes;
            byte[] copyToBytes;
            BitmapData bitmapData;
            bytes = new byte[bmp.Width * bmp.Height * 4];
            copyToBytes = new byte[bmp.Width * bmp.Height * 4];

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            IntPtr Iptr = bitmapData.Scan0;
            Marshal.Copy(Iptr, bytes, 0, bytes.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                copyToBytes[bytes.Length - 1 - i] = bytes[i];
            }
            bmp.UnlockBits(bitmapData);

            return copyToBytes;
        }

        void GetDelay()
        {
            PropertyItem item = gifImage.GetPropertyItem(0x5100);
            delays = new float[gifImage.GetFrameCount(dimension)];
            for (int i = 0; i < item.Value.Length - 1; i += 4)
                delays[i / 4] = (item.Value[i] + item.Value[i + 1] * 256) / 100f /* * 10 / 1000 */;
        }

        List<Texture2D> GetEmptyFrames()
        {
            List<Texture2D> gifFrames = new List<Texture2D>();
            for (int i = 0; i < gifImage.GetFrameCount(dimension); i++)
            {
                Texture2D texture2D = new Texture2D(gifImage.Width, gifImage.Height, TextureFormat.ARGB32, false);
                texture2D.filterMode = FilterMode.Trilinear;
                gifFrames.Add(texture2D);
            }
            return gifFrames;
        }

        List<Texture2D> GetEmptyFrames(int width, int height)
        {
            List<Texture2D> gifFrames = new List<Texture2D>();
            for (int i = 0; i < gifImage.GetFrameCount(dimension); i++)
                gifFrames.Add(new Texture2D(width, height, TextureFormat.ARGB32, false));
            return gifFrames;
        }

        public void Paint(mGraphics g, int x, int y)
        {
            if (!isFullyLoaded)
            {
                g.setColor(UnityEngine.Color.black);
                g.fillRect(x, y, GameCanvas.w, GameCanvas.h);
                mFont.tahoma_7b_red.drawString(g, $"Đang tải... ({frameIndex}/{frames.Count})", GameCanvas.w / 2, y, mFont.CENTER);
                return;
            }
            if (paintFrameIndex >= frames.Count)
                paintFrameIndex = 0;
            UnityEngine.Graphics.DrawTexture(new Rect(x, y, frames[paintFrameIndex].width, frames[paintFrameIndex].height), frames[paintFrameIndex]);
            if (mSystem.currentTimeMillis() - lastTimePaintAFrame > delays[paintFrameIndex] * 1000f / speed)
            {
                lastTimePaintAFrame = mSystem.currentTimeMillis();
                paintFrameIndex++;
            }
        }
    }
}