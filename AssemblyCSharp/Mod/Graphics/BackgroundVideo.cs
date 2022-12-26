using System;
using UnityEngine;
using UnityEngine.Video;

namespace Mod.Graphics
{
    //[Obsolete("Tốn nhiểu CPU + RAM, máy mạnh mới chạy được!")]
    public class BackgroundVideo : IBackground
    {
        VideoPlayer videoPlayer = GameObject.Find("Main Camera").AddComponent<VideoPlayer>();
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
            //videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.skipOnDrop = true;
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

        public bool isPlaying =>
            videoPlayer.isPlaying;
    }
}
