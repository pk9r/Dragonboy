public class ServerScr : mScreen, IActionListener
{
	private int mainSelect;

	private MyVector vecServer = new MyVector();

	private Command cmdCheck;

	public const int icmd = 100;

	private int wc;

	private int hc;

	private int w2c;

	private int numw;

	private int numh;

	private Command cmdGlobal;

	private Command cmdVietNam;

	public ServerScr()
	{
		TileMap.bgID = (byte)(mSystem.currentTimeMillis() % 9);
		if (TileMap.bgID == 5 || TileMap.bgID == 6)
			TileMap.bgID = 4;
		GameScr.loadCamera(true, -1, -1);
		GameScr.cmx = 100;
		GameScr.cmy = 200;
	}

	public override void switchToMe()
	{
		SoundMn.gI().stopAll();
		base.switchToMe();
		cmdGlobal = new Command("VIỆT NAM", this, 98, null);
		cmdGlobal.x = 0;
		cmdGlobal.y = 0;
		cmdVietNam = new Command("GLOBAL", this, 97, null);
		cmdVietNam.x = 50;
		cmdVietNam.y = 0;
		vecServer = new MyVector();
		vecServer.addElement(cmdGlobal);
		vecServer.addElement(cmdVietNam);
		sort();
	}

	private void sort()
	{
		mainSelect = ServerListScreen.ipSelect;
		w2c = 5;
		wc = 76;
		hc = mScreen.cmdH;
		numw = 2;
		if (GameCanvas.w > 3 * (wc + w2c))
			numw = 3;
		if (vecServer.size() < 3)
			numw = 2;
		numh = vecServer.size() / numw + ((vecServer.size() % numw != 0) ? 1 : 0);
		for (int i = 0; i < vecServer.size(); i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			if (command != null)
			{
				int x = GameCanvas.hw - numw * (wc + w2c) / 2 + i % numw * (wc + w2c);
				int y = GameCanvas.hh - numh * (hc + w2c) / 2 + i / numw * (hc + w2c);
				command.x = x;
				command.y = y;
			}
		}
	}

	public override void update()
	{
		GameScr.cmx++;
		if (GameScr.cmx > GameCanvas.w * 3 + 100)
			GameScr.cmx = 100;
		for (int i = 0; i < vecServer.size(); i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			if (!GameCanvas.isTouch)
			{
				if (i == mainSelect)
				{
					if (GameCanvas.gameTick % 10 < 4)
						command.isFocus = true;
					else
						command.isFocus = false;
					cmdCheck = new Command(mResources.SELECT, this, command.idAction, null);
					center = cmdCheck;
				}
				else
					command.isFocus = false;
			}
			else if (command != null && command.isPointerPressInside())
			{
				command.performAction();
			}
		}
	}

	public override void paint(mGraphics g)
	{
		GameCanvas.paintBGGameScr(g);
		for (int i = 0; i < vecServer.size(); i++)
		{
			if (vecServer.elementAt(i) != null)
				((Command)vecServer.elementAt(i)).paint(g);
		}
		base.paint(g);
	}

	public override void updateKey()
	{
		base.updateKey();
		int num = mainSelect % numw;
		int num2 = mainSelect / numw;
		if (GameCanvas.keyPressed[4])
		{
			if (num > 0)
				mainSelect--;
			GameCanvas.keyPressed[4] = false;
		}
		else if (GameCanvas.keyPressed[6])
		{
			if (num < numw - 1)
				mainSelect++;
			GameCanvas.keyPressed[6] = false;
		}
		else if (GameCanvas.keyPressed[2])
		{
			if (num2 > 0)
				mainSelect -= numw;
			GameCanvas.keyPressed[2] = false;
		}
		else if (GameCanvas.keyPressed[8])
		{
			if (num2 < numh - 1)
				mainSelect += numw;
			GameCanvas.keyPressed[8] = false;
		}
		if (mainSelect < 0)
			mainSelect = 0;
		if (mainSelect >= vecServer.size())
			mainSelect = vecServer.size() - 1;
		if (GameCanvas.keyPressed[5])
		{
			((Command)vecServer.elementAt(num)).performAction();
			GameCanvas.keyPressed[5] = false;
		}
		GameCanvas.clearKeyPressed();
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 97:
		{
			vecServer.removeAllElements();
			for (int j = 0; j < ServerListScreen.nameServer.Length; j++)
			{
				if (ServerListScreen.language[j] != 0)
					vecServer.addElement(new Command(ServerListScreen.nameServer[j], this, 100 + j, null));
			}
			sort();
			break;
		}
		case 98:
		{
			vecServer.removeAllElements();
			for (int i = 0; i < ServerListScreen.nameServer.Length; i++)
			{
				if (ServerListScreen.language[i] == 0)
					vecServer.addElement(new Command(ServerListScreen.nameServer[i], this, 100 + i, null));
			}
			sort();
			break;
		}
		case 99:
			Session_ME.gI().clearSendingMessage();
			ServerListScreen.ipSelect = mainSelect;
			GameCanvas.serverScreen.selectServer();
			GameCanvas.serverScreen.switchToMe();
			break;
		default:
			Session_ME.gI().clearSendingMessage();
			ServerListScreen.ipSelect = idAction - 100;
			Res.outz("Default:    ServerListScreen.ipSelect " + ServerListScreen.ipSelect);
			GameCanvas.serverScreen.selectServer();
			GameCanvas.serverScreen.switchToMe();
			break;
		}
	}
}
