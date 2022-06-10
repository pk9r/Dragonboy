public class ItemTime
{
	public short idIcon;

	public int second;

	public int minute;

	private long curr;

	private long last;

	private bool isText;

	private bool dontClear;

	private string text;

	public ItemTime()
	{
	}

	public ItemTime(short idIcon, int s)
	{
		this.idIcon = idIcon;
		minute = s / 60;
		second = s % 60;
		curr = (last = mSystem.currentTimeMillis());
	}

	public void initTimeText(sbyte id, string text, int time)
	{
		if (time == -1)
		{
			dontClear = true;
		}
		else
		{
			dontClear = false;
		}
		isText = true;
		minute = time / 60;
		second = time % 60;
		idIcon = id;
		curr = (last = mSystem.currentTimeMillis());
		this.text = text;
	}

	public void initTime(int time, bool isText)
	{
		minute = time / 60;
		second = time % 60;
		curr = (last = mSystem.currentTimeMillis());
		this.isText = isText;
	}

	public static bool isExistItem(int id)
	{
		for (int i = 0; i < Char.vItemTime.size(); i++)
		{
			ItemTime itemTime = (ItemTime)Char.vItemTime.elementAt(i);
			if (itemTime.idIcon == id)
			{
				return true;
			}
		}
		return false;
	}

	public static ItemTime getMessageById(int id)
	{
		for (int i = 0; i < GameScr.textTime.size(); i++)
		{
			ItemTime itemTime = (ItemTime)GameScr.textTime.elementAt(i);
			if (itemTime.idIcon == id)
			{
				return itemTime;
			}
		}
		return null;
	}

	public static bool isExistMessage(int id)
	{
		for (int i = 0; i < GameScr.textTime.size(); i++)
		{
			ItemTime itemTime = (ItemTime)GameScr.textTime.elementAt(i);
			if (itemTime.idIcon == id)
			{
				return true;
			}
		}
		return false;
	}

	public static ItemTime getItemById(int id)
	{
		for (int i = 0; i < Char.vItemTime.size(); i++)
		{
			ItemTime itemTime = (ItemTime)Char.vItemTime.elementAt(i);
			if (itemTime.idIcon == id)
			{
				return itemTime;
			}
		}
		return null;
	}

	public void initTime(int time)
	{
		minute = time / 60;
		second = time % 60;
		curr = (last = mSystem.currentTimeMillis());
	}

	public void paint(mGraphics g, int x, int y)
	{
		SmallImage.drawSmallImage(g, idIcon, x, y, 0, 3);
		string empty = string.Empty;
		empty = minute + "'";
		if (minute == 0)
		{
			empty = second + "s";
		}
		mFont.tahoma_7b_white.drawString(g, empty, x, y + 15, 2, mFont.tahoma_7b_dark);
	}

	public void paintText(mGraphics g, int x, int y)
	{
		string empty = string.Empty;
		empty = minute + "'";
		if (minute < 1)
		{
			empty = second + "s";
		}
		if (minute < 0)
		{
			empty = string.Empty;
		}
		if (dontClear)
		{
			empty = string.Empty;
		}
		mFont.tahoma_7b_white.drawString(g, text + " " + empty, x, y, mFont.LEFT, mFont.tahoma_7b_dark);
	}

	public void update()
	{
		curr = mSystem.currentTimeMillis();
		if (curr - last >= 1000)
		{
			last = mSystem.currentTimeMillis();
			second--;
			if (second <= 0)
			{
				second = 60;
				minute--;
			}
		}
		if (minute < 0 && !isText)
		{
			Char.vItemTime.removeElement(this);
		}
		if (minute < 0 && isText && !dontClear)
		{
			GameScr.textTime.removeElement(this);
		}
	}
}
