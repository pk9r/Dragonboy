using System;
using UnityEngine;
using UnityEngine.Video;

namespace Mod.Graphics
{
    [Obsolete("Tốn nhiểu CPU + RAM, máy mạnh mới chạy được!")]
    public class BackgroundVideo
    {
        public static VideoPlayer videoPlayer = GameObject.Find("Main Camera").AddComponent<VideoPlayer>();

        public static AudioSource audioSource;

        public static void Start()
        {
            GameObject gameObject = GameObject.Find("Main Camera");
            audioSource = gameObject.AddComponent<AudioSource>();
            videoPlayer.playOnAwake = false;
            audioSource.playOnAwake = false;
            videoPlayer.renderMode = VideoRenderMode.APIOnly;
            videoPlayer.url = "Videos/Rick Astley - Never Gonna Give You Up Official Music Video.mp4";
            videoPlayer.isLooping = true;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.skipOnDrop = true;
            videoPlayer.SetTargetAudioSource(0, audioSource);
            audioSource.volume = 0.5f;
            videoPlayer.Play();
            audioSource.Play();
        }
    }
}
