using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GifAssets.PowerGif
{
    /// <summary>
    /// Implements converting data from SimpleGif library (Texture2D and Color32).
    /// </summary>
    internal static class Converter
	{
		/// <summary>
		/// Convert GIF frames from SimpleGif to PowerGif.
		/// </summary>
		internal static List<GifFrame> ConvertFrames(List<SimpleGif.Data.GifFrame> frames)
		{
            return frames.Select(i => new GifFrame(ConvertTexture(i.Texture), i.Delay)).ToList();
        }

        /// <summary>
        /// Convert GIF frames from PowerGif to SimpleGif.
        /// </summary>
        internal static List<SimpleGif.Data.GifFrame> ConvertFrames(List<GifFrame> frames)
		{
			return frames.Select(i => new SimpleGif.Data.GifFrame { Texture = ConvertTexture(i.Texture), Delay = i.Delay }).ToList();
		}

		internal static Texture2D ConvertTexture(SimpleGif.Data.Texture2D source)
		{
			var pixels = new Color32[source.width * source.height];
			var pixels32 = source.GetPixels32();
			var transparency = false;

			for (var i = 0; i < pixels.Length; i++)
			{
				pixels[i] = new Color32(pixels32[i].r, pixels32[i].g, pixels32[i].b, pixels32[i].a);

				if (!transparency && pixels[i].a == 0)
				{
					transparency = true;
				}
			}

			var texture = new Texture2D(source.width, source.height, transparency ? TextureFormat.RGBA32 : TextureFormat.RGB24, true);

			texture.SetPixels32(pixels);
			texture.Apply();

			return texture;
		}

		internal static SimpleGif.Data.Texture2D ConvertTexture(Texture2D source)
		{
			var texture = new SimpleGif.Data.Texture2D(source.width, source.height);
			var pixels = source.GetPixels32().Select(i => new SimpleGif.Data.Color32(i.r, i.g, i.b, i.a)).ToArray();

			texture.SetPixels32(pixels);
			texture.Apply();

			return texture;
		}
	}
}