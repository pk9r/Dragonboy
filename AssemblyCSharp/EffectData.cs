using System;

public class EffectData
{
	public Image img;

	public ImageInfo[] imgInfo;

	public Frame[] frame;

	public short[] arrFrame;

	public int ID;

	public int typeData;

	public int width;

	public int height;

	public ImageInfo getImageInfo(sbyte id)
	{
		for (int i = 0; i < imgInfo.Length; i++)
		{
			if (imgInfo[i].ID == id)
			{
				return imgInfo[i];
			}
		}
		return null;
	}

	public void readData(string patch)
	{
		DataInputStream dataInputStream = null;
		try
		{
			dataInputStream = MyStream.readFile(patch);
		}
		catch (Exception)
		{
			return;
		}
		readData(dataInputStream.r);
	}

	public void readData2(string patch)
	{
		DataInputStream dataInputStream = null;
		try
		{
			dataInputStream = MyStream.readFile(patch);
		}
		catch (Exception)
		{
			return;
		}
		readEffect(dataInputStream.r);
	}

	public void readEffect(myReader msg)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		try
		{
			sbyte b = msg.readByte();
			Res.outz("size IMG==========" + b);
			imgInfo = new ImageInfo[b];
			for (int i = 0; i < b; i++)
			{
				imgInfo[i] = new ImageInfo();
				imgInfo[i].ID = msg.readByte();
				imgInfo[i].x0 = msg.readUnsignedByte();
				imgInfo[i].y0 = msg.readUnsignedByte();
				imgInfo[i].w = msg.readUnsignedByte();
				imgInfo[i].h = msg.readUnsignedByte();
			}
			short num5 = msg.readShort();
			frame = new Frame[num5];
			for (int j = 0; j < frame.Length; j++)
			{
				frame[j] = new Frame();
				sbyte b2 = msg.readByte();
				frame[j].dx = new short[b2];
				frame[j].dy = new short[b2];
				frame[j].idImg = new sbyte[b2];
				for (int k = 0; k < b2; k++)
				{
					frame[j].dx[k] = msg.readShort();
					frame[j].dy[k] = msg.readShort();
					frame[j].idImg[k] = msg.readByte();
					if (j == 0)
					{
						if (num > frame[j].dx[k])
						{
							num = frame[j].dx[k];
						}
						if (num2 > frame[j].dy[k])
						{
							num2 = frame[j].dy[k];
						}
						if (num3 < frame[j].dx[k] + imgInfo[frame[j].idImg[k]].w)
						{
							num3 = frame[j].dx[k] + imgInfo[frame[j].idImg[k]].w;
						}
						if (num4 < frame[j].dy[k] + imgInfo[frame[j].idImg[k]].h)
						{
							num4 = frame[j].dy[k] + imgInfo[frame[j].idImg[k]].h;
						}
						width = num3 - num;
						height = num4 - num2;
					}
				}
			}
			arrFrame = new short[msg.readShort()];
			for (int l = 0; l < arrFrame.Length; l++)
			{
				arrFrame[l] = msg.readShort();
			}
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
			Res.outz("1");
		}
	}

	public void readData(myReader iss)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		try
		{
			sbyte b = iss.readByte();
			imgInfo = new ImageInfo[b];
			for (int i = 0; i < b; i++)
			{
				imgInfo[i] = new ImageInfo();
				imgInfo[i].ID = iss.readByte();
				imgInfo[i].x0 = iss.readByte();
				imgInfo[i].y0 = iss.readByte();
				imgInfo[i].w = iss.readByte();
				imgInfo[i].h = iss.readByte();
			}
			short num5 = iss.readShort();
			frame = new Frame[num5];
			for (int j = 0; j < num5; j++)
			{
				frame[j] = new Frame();
				sbyte b2 = iss.readByte();
				frame[j].dx = new short[b2];
				frame[j].dy = new short[b2];
				frame[j].idImg = new sbyte[b2];
				for (int k = 0; k < b2; k++)
				{
					frame[j].dx[k] = iss.readShort();
					frame[j].dy[k] = iss.readShort();
					frame[j].idImg[k] = iss.readByte();
					if (j == 0)
					{
						if (num > frame[j].dx[k])
						{
							num = frame[j].dx[k];
						}
						if (num2 > frame[j].dy[k])
						{
							num2 = frame[j].dy[k];
						}
						if (num3 < frame[j].dx[k] + imgInfo[frame[j].idImg[k]].w)
						{
							num3 = frame[j].dx[k] + imgInfo[frame[j].idImg[k]].w;
						}
						if (num4 < frame[j].dy[k] + imgInfo[frame[j].idImg[k]].h)
						{
							num4 = frame[j].dy[k] + imgInfo[frame[j].idImg[k]].h;
						}
						width = num3 - num;
						height = num4 - num2;
					}
				}
			}
			short num6 = iss.readShort();
			arrFrame = new short[num6];
			for (int l = 0; l < num6; l++)
			{
				arrFrame[l] = iss.readShort();
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("LOI TAI readData cua EffectDAta" + ex.ToString());
		}
	}

	public void readData(sbyte[] data)
	{
		myReader iss = new myReader(data);
		readData(iss);
	}

	public void readDataNewBoss(sbyte[] data, sbyte typeread)
	{
		myReader msg = new myReader(data);
		readMobNew(msg, typeread);
	}

	public void paintFrame(mGraphics g, int f, int x, int y, int trans, int layer)
	{
		if (this.frame == null || this.frame.Length == 0)
		{
			return;
		}
		Frame frame = this.frame[f];
		for (int i = 0; i < frame.dx.Length; i++)
		{
			ImageInfo imageInfo = getImageInfo(frame.idImg[i]);
			try
			{
				if (trans == -1)
				{
					g.drawRegion(img, imageInfo.x0, imageInfo.y0, imageInfo.w, imageInfo.h, 0, x + frame.dx[i], y + frame.dy[i], 0);
				}
				if (trans == 0)
				{
					g.drawRegion(img, imageInfo.x0, imageInfo.y0, imageInfo.w, imageInfo.h, 0, x + frame.dx[i], y + frame.dy[i] - ((layer < 4 && layer > 0) ? GameCanvas.transY : 0), 0);
				}
				if (trans == 1)
				{
					g.drawRegion(img, imageInfo.x0, imageInfo.y0, imageInfo.w, imageInfo.h, 2, x - frame.dx[i], y + frame.dy[i] - ((layer < 4 && layer > 0) ? GameCanvas.transY : 0), StaticObj.TOP_RIGHT);
				}
				if (trans == 2)
				{
					g.drawRegion(img, imageInfo.x0, imageInfo.y0, imageInfo.w, imageInfo.h, 7, x - frame.dx[i], y + frame.dy[i] - ((layer < 4 && layer > 0) ? GameCanvas.transY : 0), StaticObj.VCENTER_HCENTER);
				}
			}
			catch (Exception)
			{
			}
		}
	}

	public void readMobNew(myReader msg, sbyte typeread)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		try
		{
			sbyte b = msg.readByte();
			imgInfo = new ImageInfo[b];
			for (int i = 0; i < b; i++)
			{
				imgInfo[i] = new ImageInfo();
				imgInfo[i].ID = msg.readByte();
				if (typeread == 1)
				{
					imgInfo[i].x0 = msg.readUnsignedByte();
					imgInfo[i].y0 = msg.readUnsignedByte();
				}
				else
				{
					imgInfo[i].x0 = msg.readShort();
					imgInfo[i].y0 = msg.readShort();
				}
				imgInfo[i].w = msg.readUnsignedByte();
				imgInfo[i].h = msg.readUnsignedByte();
			}
			short num5 = msg.readShort();
			frame = new Frame[num5];
			for (int j = 0; j < frame.Length; j++)
			{
				frame[j] = new Frame();
				sbyte b2 = msg.readByte();
				frame[j].dx = new short[b2];
				frame[j].dy = new short[b2];
				frame[j].idImg = new sbyte[b2];
				for (int k = 0; k < b2; k++)
				{
					frame[j].dx[k] = msg.readShort();
					frame[j].dy[k] = msg.readShort();
					frame[j].idImg[k] = msg.readByte();
					if (j == 0)
					{
						if (num > frame[j].dx[k])
						{
							num = frame[j].dx[k];
						}
						if (num2 > frame[j].dy[k])
						{
							num2 = frame[j].dy[k];
						}
						if (num3 < frame[j].dx[k] + imgInfo[frame[j].idImg[k]].w)
						{
							num3 = frame[j].dx[k] + imgInfo[frame[j].idImg[k]].w;
						}
						if (num4 < frame[j].dy[k] + imgInfo[frame[j].idImg[k]].h)
						{
							num4 = frame[j].dy[k] + imgInfo[frame[j].idImg[k]].h;
						}
						width = num3 - num;
						height = num4 - num2;
					}
				}
			}
			arrFrame = new short[msg.readShort()];
			for (int l = 0; l < arrFrame.Length; l++)
			{
				arrFrame[l] = msg.readShort();
			}
		}
		catch (Exception)
		{
		}
	}
}
