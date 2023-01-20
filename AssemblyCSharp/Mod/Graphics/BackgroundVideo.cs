using System;
using UnityEngine;
using UnityEngine.Video;

namespace Mod.Graphics
{
    public class BackgroundVideo : IImage
    {
        VideoPlayer videoPlayer = GameObject.Find("Main Camera").AddComponent<VideoPlayer>();
        public bool isPreparing;

        //AudioSource audioSource;

        public BackgroundVideo(string path)
        {
            //GameObject gameObject = GameObject.Find("Main Camera");
            //audioSource = gameObject.AddComponent<AudioSource>();
            videoPlayer.playOnAwake = false;
            //audioSource.playOnAwake = false;
            videoPlayer.renderMode = VideoRenderMode.APIOnly;
            videoPlayer.url = path;
            videoPlayer.isLooping = true;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
            videoPlayer.skipOnDrop = true;
            videoPlayer.prepareCompleted += (source) => isPreparing = false;
            //videoPlayer.SetTargetAudioSource(0, audioSource);
            //audioSource.volume = 0.5f;
            //videoPlayer.Play();
            //audioSource.Play();
        }

        public void Paint(mGraphics g, int x, int y)
        {
            UnityEngine.Graphics.DrawTexture(new Rect(x, y, Screen.width, Screen.height), videoPlayer.texture);
        }

        public void Stop() =>
            videoPlayer.Stop();

        public void Play() => 
            videoPlayer.Play();

        public void Prepare()
        {
            isPreparing = true;
            videoPlayer.Prepare();
        }

        public bool isPlaying =>
            videoPlayer.isPlaying;

        public bool isPrepared =>
            videoPlayer.isPrepared;

        public Texture2D[] Textures => throw new NotSupportedException("Video does not have an array of texture!");
    }
}
