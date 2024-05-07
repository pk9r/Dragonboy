public class FireWorkMn
{
	internal int x;

	internal int y;

	internal int goc = 1;

	internal int n = 360;

	internal MyRandom rd = new MyRandom();

	internal MyVector fw = new MyVector();

	internal int[] color = new int[8] { 16711680, 16776960, 65280, 16777215, 255, 65535, 15790320, 12632256 };

	public FireWorkMn(int x, int y, int goc, int n)
	{
		this.x = x;
		this.y = y;
		this.goc = goc;
		this.n = n;
		for (int i = 0; i < n; i++)
		{
			fw.addElement(new Firework(x, y, Math2.abs(rd.nextInt() % 8) + 3, i * goc, color[Math2.abs(rd.nextInt() % color.Length)]));
		}
	}

	public void paint(mGraphics g)
	{
		for (int i = 0; i < fw.size(); i++)
		{
			Firework firework = (Firework)fw.elementAt(i);
			if (firework.y < -200)
				fw.removeElementAt(i);
			firework.paint(g);
		}
	}
}
