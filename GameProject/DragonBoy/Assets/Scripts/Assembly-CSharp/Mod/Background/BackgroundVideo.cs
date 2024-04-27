using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace Mod.Background
{
    internal class BackgroundVideo : IBackground
    {
        static VideoPlayer[] videoPlayers = GameObject.Find("Main Camera").GetComponents<VideoPlayer>();
        internal bool isPreparing;
        internal string url;
        long lastTimeCheckTime;
        int videoPlayerIndex;
        double lastTime;
        ScaleMode _scaleMode = ScaleMode.StretchToFill;
        public ScaleMode ScaleMode
        {
            get => _scaleMode;
            set => _scaleMode = value;
        }

        internal BackgroundVideo(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)) || !File.Exists(path))
                throw new FileNotFoundException();
            url = path;
        }

        public void Paint(mGraphics g, int x, int y)
        {
            if (mSystem.currentTimeMillis() - lastTimeCheckTime > 5000)
            {
                lastTimeCheckTime = mSystem.currentTimeMillis();
                if (videoPlayers[videoPlayerIndex].time == lastTime)
                {
                    videoPlayers[videoPlayerIndex].Stop();
                    videoPlayers[videoPlayerIndex].Play();
                    videoPlayers[videoPlayerIndex].time = lastTime;
                }
            }
            lastTime = videoPlayers[videoPlayerIndex].time;
            if (videoPlayers[videoPlayerIndex].texture != null)
                GUI.DrawTexture(new Rect(x, y, Screen.width, Screen.height), videoPlayers[videoPlayerIndex].texture, _scaleMode);
        }

        internal void Stop() =>
            videoPlayers[videoPlayerIndex].Stop();

        internal void Play() =>
            videoPlayers[videoPlayerIndex].Play();

        internal void Prepare()
        {
            isPreparing = true;
            for (int i = 0; i < videoPlayers.Length; i++)
            {
                videoPlayerIndex++;
                if (videoPlayerIndex == videoPlayers.Length)
                    videoPlayerIndex = 0;
                if (!videoPlayers[videoPlayerIndex].isPlaying)
                    break;
            }
            videoPlayers[videoPlayerIndex].url = url;
            videoPlayers[videoPlayerIndex].prepareCompleted += BackgroundVideo_prepareCompleted;
            videoPlayers[videoPlayerIndex].Prepare();
        }

        private void BackgroundVideo_prepareCompleted(VideoPlayer source)
        {
            isPreparing = false;
            videoPlayers[videoPlayerIndex].prepareCompleted -= BackgroundVideo_prepareCompleted;
        }

        internal bool isPlaying =>
            videoPlayers[videoPlayerIndex].isPlaying;

        public Texture2D[] Textures => throw new NotSupportedException("Video does not have an array of texture!");

        public bool IsLoaded => videoPlayers[videoPlayerIndex].isPrepared;
    }
}
