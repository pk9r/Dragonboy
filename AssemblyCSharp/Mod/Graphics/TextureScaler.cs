//Source: https://pastebin.com/qkkhWs2J
using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using UnityEngine;
namespace Mod.Graphics
{
    /// <summary>
    /// A unility class with functions to scale Texture2D Data.
    ///
    /// Scale is performed on the GPU using RTT, so it's blazing fast.
    /// Setting up and Getting back the texture data is the bottleneck. 
    /// But Scaling itself costs only 1 draw call and 1 RTT State setup!
    /// WARNING: This script override the RTT Setup! (It sets a RTT!)	 
    ///
    /// Note: This scaler does NOT support aspect ratio based scaling. You will have to do it yourself!
    /// It supports Alpha, but you will have to divide by alpha in your shaders, 
    /// because of premultiplied alpha effect. Or you should use blend modes.
    /// </summary>
    public class TextureScaler
	{

        /// <summary>
        /// Scales the texture data of the given texture.
        /// </summary>
        /// <param name="src">Source texure to scale</param>
        /// <param name="width">Destination texture width</param>
        /// <param name="height">Destination texture height</param>
        /// <param name="mode">Filtering mode</param>
        /// <returns>Scaled texture</returns>
        public static Texture2D ScaleTexture(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
		{
			Rect texR = new Rect(0, 0, width, height);
			_gpu_scale(src, width, height, mode);

			//Get rendered data back to a new texture
			Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
			result.Resize(width, height);
			result.ReadPixels(texR, 0, 0, true);
			return result;
		}

        /// <summary>
        /// Scales the texture data of the given texture, then apply it.
        /// </summary>
        /// <param name="tex">Texure to scale</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="mode">Filtering mode</param>
        /// <returns>Scaled texture</returns>
        public static void ApplyScaleTexture(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear)
		{
			Rect texR = new Rect(0, 0, width, height);
			_gpu_scale(tex, width, height, mode);

			// Update new texture
			tex.Resize(width, height);
			tex.ReadPixels(texR, 0, 0, true);
			tex.Apply(true);    //Remove this if you hate us applying textures for you :)
		}

        /// <summary>
        /// Internal unility that renders the source texture into the RTT - the scaling method itself.
        /// </summary>
        /// <param name="src">Source texture</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="fmode">Texture filtering mode</param>
        static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
		{
			//We need the source texture in VRAM because we render with it
			src.filterMode = fmode;
			src.Apply(true);

			//Using RTT for best quality and performance. Thanks, Unity 5
			RenderTexture rtt = new RenderTexture(width, height, 32);

            //Set the RTT in order to render to it
            UnityEngine.Graphics.SetRenderTarget(rtt);

			//Setup 2D matrix in range 0..1, so nobody needs to care about sized
			GL.LoadPixelMatrix(0, 1, 1, 0);

			//Then clear & draw the texture to fill the entire RTT.
			GL.Clear(true, true, new UnityEngine.Color(0, 0, 0, 0));
            UnityEngine.Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
		}

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = System.Drawing.Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using ImageAttributes wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }
    }
}