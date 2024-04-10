using SimpleGif.Enums;
using UnityEngine;

namespace Assets.GifAssets.PowerGif
{
	/// <summary>
	/// Texture + delay + disposal method
	/// </summary>
	internal class GifFrame
	{
		internal Texture2D Texture;
		internal float Delay;
		internal DisposalMethod DisposalMethod;

		internal GifFrame(Texture2D texture, float delay, DisposalMethod disposalMethod = DisposalMethod.RestoreToBackgroundColor)
		{
			Texture = texture;
			Delay = delay;
			DisposalMethod = disposalMethod;
		}
	}
}