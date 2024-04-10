using System.Collections.Generic;
using UnityEngine;

namespace Assets.GifAssets.PowerGif
{
	/// <summary>
	/// Main class for working with GIF format. It is a wrapper over SimpleGif.Gif.
	/// </summary>
	internal class Gif
	{
		/// <summary>
		/// List of GIF frames.
		/// </summary>
		internal List<GifFrame> Frames;

		/// <summary>
		/// Create a new instance from GIF frames.
		/// </summary>
		internal Gif(List<GifFrame> frames)
		{
			Frames = frames;
		}

		/// <summary>
		/// Decode byte array and return a new instance.
		/// </summary>
		internal static Gif Decode(byte[] bytes, FilterMode filterMode = FilterMode.Point)
		{
			var frames = Converter.ConvertFrames(SimpleGif.Gif.Decode(bytes).Frames);

			frames.ForEach(i => i.Texture.filterMode = filterMode);

			return new Gif(frames);
		}

		/// <summary>
		/// Encode all frames to byte array
		/// </summary>
		internal byte[] Encode()
		{
			var frames = Converter.ConvertFrames(Frames);

			return new SimpleGif.Gif(frames).Encode();
		}
	}
}