using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assets.GifAssets.PowerGif;
using Mod.ModHelper;
using UnityEngine;

namespace Mod.Graphics
{
    internal class GifImage : IImage
    {
        internal float[] delays => gif.Frames.Select(f => f.Delay).ToArray();
        internal int paintFrameIndex;
        long lastTimePaintAFrame;
        int frameIndex;
        internal bool isLoaded;
        internal float speed = 1f;
        Gif gif;
        int frameCount;

        public Texture2D[] Textures => gif.Frames.Select(f => f.Texture).ToArray();

        public bool IsLoaded => isLoaded;

        internal GifImage(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            frameCount = SimpleGif.Gif.GetDecodeIteratorSize(data);
            ThreadPool.QueueUserWorkItem(_ =>
            {
                List<GifFrame> frames = new List<GifFrame>();
                foreach (SimpleGif.Data.GifFrame gifFrame in SimpleGif.Gif.DecodeIterator(data))
                {
                    bool completed = false;
                    MainThreadDispatcher.dispatch(() =>
                    {
                        GifFrame convertedFrame = new GifFrame(Converter.ConvertTexture(gifFrame.Texture), gifFrame.Delay);
                        frames.Add(convertedFrame);
                        convertedFrame.Texture.filterMode = FilterMode.Point;
                        frameIndex++;
                        completed = true;
                    });
                    while (!completed)
                        Thread.Sleep(100);
                }
                gif = new Gif(frames);
                isLoaded = true;
            });
        }

        public void Paint(mGraphics g, int x, int y)
        {
            if (!isLoaded)
            {
                mFont.tahoma_7b_red.drawString(g, $"Đang tải... ({frameIndex}/{frameCount})", GameCanvas.w / 2, y, mFont.CENTER);
                return;
            }
            if (paintFrameIndex >= gif.Frames.Count)
                paintFrameIndex = 0;
            GUI.DrawTexture(new Rect(x, y, Screen.width, Screen.height), gif.Frames[paintFrameIndex].Texture, ScaleMode.ScaleToFit);
            if (mSystem.currentTimeMillis() - lastTimePaintAFrame > gif.Frames[paintFrameIndex].Delay * 1000f / speed)
            {
                lastTimePaintAFrame = mSystem.currentTimeMillis();
                paintFrameIndex++;
            }
        }
    }
}