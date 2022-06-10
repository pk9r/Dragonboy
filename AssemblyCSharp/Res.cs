public class Res
{
	private static short[] sinz = new short[91]
	{
		0, 18, 36, 54, 71, 89, 107, 125, 143, 160,
		178, 195, 213, 230, 248, 265, 282, 299, 316, 333,
		350, 367, 384, 400, 416, 433, 449, 465, 481, 496,
		512, 527, 543, 558, 573, 587, 602, 616, 630, 644,
		658, 672, 685, 698, 711, 724, 737, 749, 761, 773,
		784, 796, 807, 818, 828, 839, 849, 859, 868, 878,
		887, 896, 904, 912, 920, 928, 935, 943, 949, 956,
		962, 968, 974, 979, 984, 989, 994, 998, 1002, 1005,
		1008, 1011, 1014, 1016, 1018, 1020, 1022, 1023, 1023, 1024,
		1024
	};

	private static short[] cosz;

	private static int[] tanz;

	public static int count;

	public static bool isIcon;

	public static bool isBig;

	public static MyVector debug = new MyVector();

	public static MyRandom r = new MyRandom();

	public static void init()
	{
		cosz = new short[91];
		tanz = new int[91];
		for (int i = 0; i <= 90; i++)
		{
			cosz[i] = sinz[90 - i];
			if (cosz[i] == 0)
			{
				tanz[i] = int.MaxValue;
			}
			else
			{
				tanz[i] = (sinz[i] << 10) / cosz[i];
			}
		}
	}

	public static int sin(int a)
	{
		a = fixangle(a);
		if (a >= 0 && a < 90)
		{
			return sinz[a];
		}
		if (a >= 90 && a < 180)
		{
			return sinz[180 - a];
		}
		if (a >= 180 && a < 270)
		{
			return -sinz[a - 180];
		}
		return -sinz[360 - a];
	}

	public static int cos(int a)
	{
		a = fixangle(a);
		if (a >= 0 && a < 90)
		{
			return cosz[a];
		}
		if (a >= 90 && a < 180)
		{
			return -cosz[180 - a];
		}
		if (a >= 180 && a < 270)
		{
			return -cosz[a - 180];
		}
		return cosz[360 - a];
	}

	public static int tan(int a)
	{
		a = fixangle(a);
		if (a >= 0 && a < 90)
		{
			return tanz[a];
		}
		if (a >= 90 && a < 180)
		{
			return -tanz[180 - a];
		}
		if (a >= 180 && a < 270)
		{
			return tanz[a - 180];
		}
		return -tanz[360 - a];
	}

	public static int atan(int a)
	{
		for (int i = 0; i <= 90; i++)
		{
			if (tanz[i] >= a)
			{
				return i;
			}
		}
		return 0;
	}

	public static int angle(int dx, int dy)
	{
		int num;
		if (dx != 0)
		{
			int a = Math.abs((dy << 10) / dx);
			num = atan(a);
			if (dy >= 0 && dx < 0)
			{
				num = 180 - num;
			}
			if (dy < 0 && dx < 0)
			{
				num = 180 + num;
			}
			if (dy < 0 && dx >= 0)
			{
				num = 360 - num;
			}
		}
		else
		{
			num = ((dy <= 0) ? 270 : 90);
		}
		return num;
	}

	public static int fixangle(int angle)
	{
		if (angle >= 360)
		{
			angle -= 360;
		}
		if (angle < 0)
		{
			angle += 360;
		}
		return angle;
	}

	public static void outz(string s)
	{
	}

	public static void outz2(string s)
	{
	}

	public static void onScreenDebug(string s)
	{
	}

	public static void paintOnScreenDebug(mGraphics g)
	{
	}

	public static void updateOnScreenDebug()
	{
	}

	public static string changeString(string str)
	{
		return str;
	}

	public static string replace(string _text, string _searchStr, string _replacementStr)
	{
		return _text.Replace(_searchStr, _replacementStr);
	}

	public static int xetVX(int goc, int d)
	{
		return cos(fixangle(goc)) * d >> 10;
	}

	public static int xetVY(int goc, int d)
	{
		return sin(fixangle(goc)) * d >> 10;
	}

	public static int random(int a, int b)
	{
		if (a == b)
		{
			return a;
		}
		return a + r.nextInt(b - a);
	}

	public static int random(int a)
	{
		return r.nextInt(a);
	}

	public static int s2tick(int currentTimeMillis)
	{
		int num = 0;
		num = currentTimeMillis * 16 / 1000;
		if (currentTimeMillis * 16 % 1000 >= 5)
		{
			num++;
		}
		return num;
	}

	public static int distance(int x1, int y1, int x2, int y2)
	{
		return sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
	}

	public static int sqrt(int a)
	{
		if (a <= 0)
		{
			return 0;
		}
		int num = (a + 1) / 2;
		int num2;
		do
		{
			num2 = num;
			num = num / 2 + a / (2 * num);
		}
		while (Math.abs(num2 - num) > 1);
		return num;
	}

	public static int rnd(int a)
	{
		return r.nextInt(a);
	}

	public static int abs(int i)
	{
		return (i <= 0) ? (-i) : i;
	}

	public static bool inRect(int x1, int y1, int width, int height, int x2, int y2)
	{
		return x2 >= x1 && x2 <= x1 + width && y2 >= y1 && y2 <= y1 + height;
	}

	public static string[] split(string original, string separator, int count)
	{
		int num = original.IndexOf(separator);
		string[] array;
		if (num >= 0)
		{
			array = split(original.Substring(num + separator.Length), separator, count + 1);
		}
		else
		{
			array = new string[count + 1];
			num = original.Length;
		}
		array[count] = original.Substring(0, num);
		return array;
	}

	public static string formatNumber(long number)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty = string.Empty;
		if (number >= 1000000000)
		{
			empty2 = mResources.billion;
			long num = number % 1000000000 / 100000000;
			number /= 1000000000;
			empty = number + string.Empty;
			if (num > 0)
			{
				string text = empty;
				return text + "," + num + empty2;
			}
			return empty + empty2;
		}
		if (number >= 1000000)
		{
			empty2 = mResources.million;
			long num2 = number % 1000000 / 100000;
			number /= 1000000;
			empty = number + string.Empty;
			if (num2 > 0)
			{
				string text = empty;
				return text + "," + num2 + empty2;
			}
			return empty + empty2;
		}
		return number + string.Empty;
	}

	public static string formatNumber2(long number)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty = string.Empty;
		if (number >= 1000000000)
		{
			empty2 = mResources.billion;
			long num = number % 1000000000 / 10000000;
			number /= 1000000000;
			empty = number + string.Empty;
			if (num >= 10)
			{
				if (num % 10 == 0)
				{
					num /= 10;
				}
				string text = empty;
				return text + "," + num + empty2;
			}
			if (num > 0)
			{
				string text = empty;
				return text + ",0" + num + empty2;
			}
			return empty + empty2;
		}
		if (number >= 1000000)
		{
			empty2 = mResources.million;
			long num2 = number % 1000000 / 10000;
			number /= 1000000;
			empty = number + string.Empty;
			if (num2 >= 10)
			{
				if (num2 % 10 == 0)
				{
					num2 /= 10;
				}
				string text = empty;
				return text + "," + num2 + empty2;
			}
			if (num2 > 0)
			{
				string text = empty;
				return text + ",0" + num2 + empty2;
			}
			return empty + empty2;
		}
		if (number >= 10000)
		{
			empty2 = "k";
			long num3 = number % 1000 / 10;
			number /= 1000;
			empty = number + string.Empty;
			if (num3 >= 10)
			{
				if (num3 % 10 == 0)
				{
					num3 /= 10;
				}
				string text = empty;
				return text + "," + num3 + empty2;
			}
			if (num3 > 0)
			{
				string text = empty;
				return text + ",0" + num3 + empty2;
			}
			return empty + empty2;
		}
		return number + string.Empty;
	}
}
