using System;

public class FrameImage
{
	public int frameWidth;

	public int frameHeight;

	public int nFrame;

	public Image imgFrame;

	public int Id = -1;

	public int numWidth;

	public int numHeight;

	public FrameImage(Image img, int width, int height)
	{
		if (img != null)
		{
			imgFrame = img;
			frameWidth = width;
			frameHeight = height;
			nFrame = img.getHeight() / height;
			if (nFrame < 1)
			{
				nFrame = 1;
			}
		}
	}

	public FrameImage(Image img, int numW, int numH, int numNull)
	{
		if (img != null)
		{
			imgFrame = img;
			numWidth = numW;
			numHeight = numH;
			frameWidth = imgFrame.getWidth() / numW;
			frameHeight = imgFrame.getHeight() / numH;
			nFrame = numW * numH - numNull;
		}
	}

	public void drawFrame(int idx, int x, int y, int trans, int anchor, mGraphics g)
	{
		try
		{
			if (imgFrame != null)
			{
				if (idx > nFrame)
				{
					idx = nFrame;
				}
				int num = idx * frameHeight;
				if (num > frameHeight * (nFrame - 1) || num < 0)
				{
					num = frameHeight * (nFrame - 1);
				}
				g.drawRegion(imgFrame, 0, idx * frameHeight, frameWidth, frameHeight, trans, x, y, anchor);
			}
		}
		catch (Exception)
		{
		}
	}
}
