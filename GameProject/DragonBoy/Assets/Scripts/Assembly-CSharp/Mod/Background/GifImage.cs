using System.Collections.Generic;
using System.IO;
using System.Threading;
using Mod.ModHelper;
using UnityEngine;

namespace Mod.Background
{
    internal class GifImage : IBackground
    {
        internal List<float> delays = new List<float>();
        internal int paintFrameIndex;
        long lastTimePaintAFrame;
        internal bool isLoaded;
        internal float speed = 1f;

        public Texture2D[] Textures => frames.ToArray();
        List<Texture2D> frames = new List<Texture2D>();

        public bool IsLoaded => isLoaded;
        ScaleMode _scaleMode = ScaleMode.StretchToFill;
        public ScaleMode ScaleMode 
        { 
            get => _scaleMode; 
            set => _scaleMode = value; 
        }

        internal GifImage(string path)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                byte[] data = File.ReadAllBytes(path);
                using var decoder = new MG.GIF.Decoder(data);
                var img = decoder.NextImage();
                do
                {
                    bool completed = false;
                    MainThreadDispatcher.dispatch(() =>
                    {
                        frames.Add(img.CreateTexture());
                        delays.Add(img.Delay);
                        completed = true;
                    });
                    while (!completed)
                        Thread.Sleep(10);
                    img = decoder.NextImage();
                } while (img != null);
                isLoaded = true;
            });
        }

        public void Paint(mGraphics g, int x, int y)
        {
            if (!isLoaded)
            {
                GUI.DrawTexture(new Rect(x, y, Screen.width, Screen.height), Texture2D.blackTexture);
                return;
            }
            if (paintFrameIndex >= frames.Count)
                paintFrameIndex = 0;
            GUI.DrawTexture(new Rect(x, y, Screen.width, Screen.height), frames[paintFrameIndex], _scaleMode);
            if (mSystem.currentTimeMillis() - lastTimePaintAFrame > delays[paintFrameIndex] / speed)
            {
                lastTimePaintAFrame = mSystem.currentTimeMillis();
                paintFrameIndex++;
            }
        }
    }
}