public abstract class Dialog
{
	public Command left;

	public Command center;

	public Command right;

	private int lenCaption;

	public virtual void paint(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		GameCanvas.paintz.paintTabSoft(g);
		GameCanvas.paintz.paintCmdBar(g, left, center, right);
	}

	public virtual void keyPress(int keyCode)
	{
		switch (keyCode)
		{
		default:
			if (keyCode == -39)
				goto case -2;
			if (keyCode == -38)
				goto case -1;
			if (keyCode != -22)
			{
				if (keyCode != -21)
				{
					if (keyCode != 10)
						return;
					break;
				}
				goto case -6;
			}
			goto case -7;
		case -1:
			GameCanvas.keyHold[(!Main.isPC) ? 2 : 21] = true;
			GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] = true;
			return;
		case -2:
			GameCanvas.keyHold[(!Main.isPC) ? 8 : 22] = true;
			GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] = true;
			return;
		case -6:
			GameCanvas.keyHold[12] = true;
			GameCanvas.keyPressed[12] = true;
			return;
		case -7:
			GameCanvas.keyHold[13] = true;
			GameCanvas.keyPressed[13] = true;
			return;
		case -5:
			break;
		}
		GameCanvas.keyHold[(!Main.isPC) ? 5 : 25] = true;
		GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = true;
	}

	public virtual void update()
	{
		if (center != null && (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(center)))
		{
			GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
			GameCanvas.isPointerClick = false;
			mScreen.keyTouch = -1;
			GameCanvas.isPointerJustRelease = false;
			if (center != null)
				center.performAction();
			mScreen.keyTouch = -1;
		}
		if (left != null && (GameCanvas.keyPressed[12] || mScreen.getCmdPointerLast(left)))
		{
			GameCanvas.keyPressed[12] = false;
			GameCanvas.isPointerClick = false;
			mScreen.keyTouch = -1;
			GameCanvas.isPointerJustRelease = false;
			if (left != null)
				left.performAction();
			mScreen.keyTouch = -1;
		}
		if (right != null && (GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(right)))
		{
			GameCanvas.keyPressed[13] = false;
			GameCanvas.isPointerClick = false;
			GameCanvas.isPointerJustRelease = false;
			mScreen.keyTouch = -1;
			if (right != null)
				right.performAction();
			mScreen.keyTouch = -1;
		}
		GameCanvas.clearKeyPressed();
		GameCanvas.clearKeyHold();
	}

	public virtual void show()
	{
	}
}
