using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GifAssets.PowerGif
{
	/// <summary>
	/// This script simply switches GIF-frames (textures) to get "animation" effect.
	/// </summary>
	[RequireComponent(typeof(UnityEngine.UI.Image))]
	internal class AnimatedImage : MonoBehaviour
	{
		internal Gif Gif;

		/// <summary>
		/// Will play GIF (if it's assigned) on app start if script is enabled.
		/// </summary>
		internal void Start ()
		{
			if (Gif != null)
			{
				StartCoroutine(Animate(Gif, 0));
			}
		}

		/// <summary>
		/// Play GIF.
		/// </summary>
		internal void Play(Gif gif)
		{
			Gif = gif;
			StartCoroutine(Animate(Gif, 0));
		}

		private IEnumerator Animate(Gif gif, int index)
		{
			var texture = gif.Frames[index].Texture;

			GetComponent<UnityEngine.UI.Image>().sprite = UnityEngine.Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);

			if (gif.Frames.Count == 1) yield break;

			var delay = gif.Frames[index].Delay;

			if (delay < 0.02f) // Chrome browser behaviour
			{
				delay = 0.1f;
			}

			yield return new WaitForSeconds(delay);

			if (++index == gif.Frames.Count)
			{
				index = 0;
			}

			StartCoroutine(Animate(gif, index));
		}
	}
}
