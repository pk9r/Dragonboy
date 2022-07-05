using System;

public class Effect_End
{
	public const short End_String_Lose = 0;

	public const short End_String_Win = 1;

	public const short End_String_Draw = 2;

	public const short End_FireWork = 3;

	public const sbyte Lvlpaint_All = -1;

	public const sbyte Lvlpaint_Front = 0;

	public const sbyte Lvlpaint_Mid = 1;

	public const sbyte Lvlpaint_Behind = 2;

	private MyVector VecEffEnd = new MyVector("EffectEnd VecEffEnd");

	public byte[] nFrame = new byte[10];

	public int id = -1;

	public int typeEffect;

	public int typeSub;

	public FrameImage fraImgEff;

	public FrameImage fraImgSubEff;

	public int fRemove;

	public int fMove;

	public int x;

	public int y;

	public int dir;

	public int dir_nguoc;

	public int levelPaint;

	public int f;

	public int vx;

	public int vy;

	public int x1000;

	public int y1000;

	public int vx1000;

	public int vy1000;

	public int dy_throw;

	public long time;

	public bool isRemove;

	public bool isAddSub;

	public static short[][] arrInfoEff = new short[9][]
	{
		new short[3] { 68, 264, 4 },
		new short[3] { 30, 120, 4 },
		new short[3] { 66, 280, 4 },
		new short[3] { 0, 0, 1 },
		new short[3] { 111, 68, 2 },
		new short[3] { 90, 68, 2 },
		new short[3] { 125, 68, 2 },
		new short[3] { 47, 282, 6 },
		new short[2]
	};

	public Effect_End(int type, int subtype, int x, int y, int levelPaint, int dir)
	{
		f = 0;
		typeEffect = type;
		typeSub = subtype;
		this.x = x;
		this.y = y;
		this.levelPaint = levelPaint;
		this.dir = dir;
		dir_nguoc = ((dir == 0) ? 2 : 0);
		time = mSystem.currentTimeMillis();
		isRemove = (isAddSub = false);
		create_Effect();
	}

	public static Image getImage(int id)
	{
		if (id < 0)
		{
			return null;
		}
		string path = "/e/e_" + id + ".png";
		Image result = null;
		try
		{
			result = mSystem.loadImage(path);
			return result;
		}
		catch (Exception)
		{
			return result;
		}
	}

	public static void setSoundSkill_END(int x, int y, int typeEffect)
	{
		try
		{
			int num = -1;
			int num2 = Res.random(3);
			if (num >= 0)
			{
				SoundMn.playSound(x, y, num, SoundMn.volume);
			}
		}
		catch (Exception)
		{
		}
	}

	public void create_Effect()
	{
		setSoundSkill_END(x, y, typeEffect);
		switch (typeEffect)
		{
		case 0:
		case 1:
		case 2:
			set_End_String(typeEffect);
			break;
		case 3:
			set_FireWork();
			break;
		}
	}

	public void update()
	{
		f++;
		switch (typeEffect)
		{
		case 0:
		case 1:
		case 2:
			upd_End_String();
			break;
		case 3:
			upd_FireWork();
			break;
		}
	}

	public void paint(mGraphics g)
	{
		try
		{
			if (!isRemove && f >= 0)
			{
				switch (typeEffect)
				{
				case 0:
				case 1:
				case 2:
					pnt_End_String(g);
					break;
				case 3:
					pnt_FireWork(g);
					break;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void removeEff()
	{
		isRemove = true;
	}

	private void set_End_String(int typeEffect)
	{
		switch (typeEffect)
		{
		case 0:
			fraImgEff = new FrameImage(4);
			break;
		case 1:
			fraImgEff = new FrameImage(5);
			break;
		case 2:
			fraImgEff = new FrameImage(6);
			break;
		}
		fRemove = 100;
		dy_throw = GameCanvas.h / 3 + 10;
		vy = 10;
		y1000 = 0;
		isAddSub = false;
	}

	private void upd_End_String()
	{
		x = GameCanvas.hw;
		y = y1000;
		if (f > fRemove)
		{
			removeEff();
		}
		vy++;
		if (vy > 15)
		{
			vy = 15;
		}
		if (y1000 + vy < dy_throw)
		{
			y1000 += vy;
			return;
		}
		y1000 = dy_throw;
		if (!isAddSub)
		{
			isAddSub = true;
			if (typeSub != -1)
			{
				GameScr.addEffectEnd(typeSub, 0, x, y, levelPaint, 0);
			}
		}
	}

	private void pnt_End_String(mGraphics g)
	{
		if (fraImgEff != null)
		{
			fraImgEff.drawFrame(f / 5 % fraImgEff.nFrame, x, y, 0, 33, g);
		}
	}

	private void set_FireWork()
	{
		int num = 0;
		num = Res.random(3, 5);
		fRemove = 90;
		for (int i = 0; i < num; i++)
		{
			Point point = new Point();
			point.x = x + Res.random_Am_0(4);
			point.y = y + Res.random_Am_0(5);
			if (typeSub == 0)
			{
				point.fRe = Res.random(10);
				int num2 = 1;
				if (i % 2 == 0)
				{
					num2 = -1;
				}
				point.x = x + Res.random(arrInfoEff[5][0] / 2) * num2;
				point.y = y - Res.random(arrInfoEff[5][1] / 2);
				point.fraImgEff = new FrameImage(7);
			}
			VecEffEnd.addElement(point);
		}
	}

	private void upd_FireWork()
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			point.update();
			if (point.f == point.fRe)
			{
				SoundMn.playSound(point.x, point.y, SoundMn.FIREWORK, SoundMn.volume);
			}
			if (point.f - point.fRe <= point.fraImgEff.nFrame * 3 - 1)
			{
				continue;
			}
			point.f = 0;
			if (typeSub == 0)
			{
				point.fRe = Res.random(10);
				int num = 1;
				if (i % 2 == 0)
				{
					num = -1;
				}
				point.x = x + Res.random(arrInfoEff[5][0] / 2) * num;
				point.y = y - Res.random(arrInfoEff[5][1] / 2);
			}
		}
		if (f >= fRemove)
		{
			removeEff();
		}
	}

	private void pnt_FireWork(mGraphics g)
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			if (point.f - point.fRe > -1 && point.fraImgEff != null)
			{
				point.fraImgEff.drawFrame((point.f - point.fRe) / 3 % point.fraImgEff.nFrame, point.x, point.y, 0, 3, g);
			}
		}
	}
}
