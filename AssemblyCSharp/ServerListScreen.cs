using System;
using System.IO;

public class ServerListScreen : mScreen, IActionListener
{
	public static string[] nameServer;

	public static string[] address;

	public static sbyte serverPriority;

	public static bool[] hasConnected;

	public static short[] port;

	public static int selected;

	public static bool isWait;

	public static Command cmdUpdateServer;

	public static sbyte[] language;

	private Command[] cmd;

	private Command cmdCallHotline;

	private int nCmdPlay;

	public static Command cmdDeleteRMS;

	private int lY;

	public static string smartPhoneVN = "Vũ trụ 1:dragon1.teamobi.com:14445:0,Vũ trụ 2:dragon2.teamobi.com:14445:0,Vũ trụ 3:dragon3.teamobi.com:14445:0,Vũ trụ 4:dragon4.teamobi.com:14445:0,Vũ trụ 5:dragon5.teamobi.com:14445:0,Vũ trụ 6:dragon6.teamobi.com:14445:0,Vũ trụ 7:dragon7.teamobi.com:14445:0,Võ đài liên vũ trụ:dragonwar.teamobi.com:20000:0,Universe 1:dragon.indonaga.com:14445:1,0,6";

	public static string javaVN = "Vũ trụ 1:112.213.94.23:14445:0,Vũ trụ 2:210.211.109.199:14445:0,Vũ trụ 3:112.213.85.88:14445:0,Vũ trụ 4:27.0.12.164:14445:0,Vũ trụ 5:27.0.12.16:14445:0,Vũ trụ 6:27.0.12.173:14445:0,Vũ trụ 7:112.213.94.223:14445:0,Võ đài liên vũ trụ:27.0.12.173:20000:0,Universe 1:54.179.255.27:14445:1,0,6";

	public static string smartPhoneIn = "Naga:dragon.indonaga.com:14446:2,2,0";

	public static string javaIn = "Naga:54.179.255.27:14446:2,2,0";

	public static string smartPhoneE = "Universe 1:54.179.255.27:14445:1,1,0";

	public static string javaE = "Universe 1:54.179.255.27:14445:1,1,0";

	public static string linkGetHost = "http://sv1.ngocrongonline.com/game/ngocrong031_t.php";

	public static string linkDefault = "Vũ trụ 1:dragon1.teamobi.com:14445:0,Vũ trụ 2:dragon2.teamobi.com:14445:0,Vũ trụ 3:dragon3.teamobi.com:14445:0,Vũ trụ 4:dragon4.teamobi.com:14445:0,Vũ trụ 5:dragon5.teamobi.com:14445:0,Vũ trụ 6:dragon6.teamobi.com:14445:0,Vũ trụ 7:dragon7.teamobi.com:14445:0,Vũ trụ 8:dragon8.teamobi.com:14445:0,Vũ trụ 9:dragon9.teamobi.com:14445:0,Vũ trụ 10:dragon10.teamobi.com:14445:0,Võ đài liên vũ trụ:dragonwar.teamobi.com:20000:0,Blue 01:103.48.194.146:14445:0,Blue 02:103.48.194.152:14445:0,Blue 03:45.119.81.28:14445:0,Blue 04:45.119.81.51:14445:0,Blue 05:103.48.194.173:14445:0,Blue 06:103.48.194.137:14445:0,Blue 07:103.48.194.159:14445:0,Blue 08:103.48.194.139:14445:0,Green 01:103.48.194.46:14445:0,Dream 1:14.225.198.30:14446:0,Dream 2:14.225.198.30:14447:0,NroZ 1:222.255.214.169:14445:0,NroZ 2:222.255.214.169:14445:0,Vũ Trụ Kakarot:103.90.224.247:14445:0,NROLOVE 1:sv1.nrolove.com:14445:0,NROLOVE 2:103.27.236.54:14446:0,Private:222.255.214.140:14445:0,SUPER 1:103.90.224.245:14446:0,SUPER 2:103.90.224.245:14447:0,Naga:dragon.indonaga.com:14446:0,Universe 1:dragon.indonaga.com:14445:0,0,9";

	public const sbyte languageVersion = 2;

	public new int keyTouch = -1;

	private int tam;

	public static bool stopDownload;

	public static string linkweb = "http://ngocrongonline.com";

	public static bool waitToLogin;

	public static int tWaitToLogin;

	public static int[] lengthServer = new int[3];

	public static int ipSelect;

	public static int flagServer;

	public static bool bigOk;

	public static int percent;

	public static string strWait;

	public static int nBig;

	public static int nBg;

	public static int demPercent;

	public static int maxBg;

	public static bool isGetData = false;

	public static Command cmdDownload;

	private Command cmdStart;

	public string dataSize;

	public static int p;

	public static int testConnect = -1;

	public static bool loadScreen;

	public ServerListScreen()
	{
		int num = 4;
		int num2 = num * 32 + 23 + 33;
		if (num2 >= GameCanvas.w)
		{
			num--;
			num2 = num * 32 + 23 + 33;
		}
		initCommand();
		if (!GameCanvas.isTouch)
		{
			selected = 0;
			processInput();
		}
		GameScr.loadCamera(fullmScreen: true, -1, -1);
		GameScr.cmx = 100;
		GameScr.cmy = 200;
		if (cmdCallHotline == null)
		{
			cmdCallHotline = new Command("Gọi hotline", this, 13, null);
			cmdCallHotline.x = GameCanvas.w - 75;
			if (mSystem.clientType == 1 && !GameCanvas.isTouch)
			{
				cmdCallHotline.y = GameCanvas.h - 20;
			}
			else
			{
				int num3 = 2;
				cmdCallHotline.y = num3 + 6;
			}
		}
		cmdUpdateServer = new Command();
		cmdUpdateServer.actionChat = delegate(string str)
		{
			string text = str;
			string text2 = str;
			if (text == null)
			{
				text = linkDefault;
			}
			else
			{
				if (text == null && text2 != null)
				{
					if (text2.Equals(string.Empty) || text2.Length < 20)
					{
						text2 = linkDefault;
					}
					getServerList(text2);
				}
				if (text != null && text2 == null)
				{
					if (text.Equals(string.Empty) || text.Length < 20)
					{
						text = linkDefault;
					}
					getServerList(text);
				}
				if (text != null && text2 != null)
				{
					if (text.Length > text2.Length)
					{
						getServerList(text);
					}
					else
					{
						getServerList(text2);
					}
				}
			}
		};
		setLinkDefault(mSystem.LANGUAGE);
	}

	public static void createDeleteRMS()
	{
		if (cmdDeleteRMS == null)
		{
			if (GameCanvas.serverScreen == null)
			{
				GameCanvas.serverScreen = new ServerListScreen();
			}
			cmdDeleteRMS = new Command(string.Empty, GameCanvas.serverScreen, 14, null);
			cmdDeleteRMS.x = GameCanvas.w - 78;
			cmdDeleteRMS.y = GameCanvas.h - 26;
		}
	}

	private void initCommand()
	{
		nCmdPlay = 0;
		string text = Rms.loadRMSString("acc");
		if (text == null)
		{
			if (Rms.loadRMS("userAo" + ipSelect) != null)
			{
				nCmdPlay = 1;
			}
		}
		else if (text.Equals(string.Empty))
		{
			if (Rms.loadRMS("userAo" + ipSelect) != null)
			{
				nCmdPlay = 1;
			}
		}
		else
		{
			nCmdPlay = 1;
		}
		cmd = new Command[4 + nCmdPlay];
		int num = GameCanvas.hh - 15 * cmd.Length + 28;
		for (int i = 0; i < cmd.Length; i++)
		{
			switch (i)
			{
			case 0:
				cmd[0] = new Command(string.Empty, this, 3, null);
				if (text == null)
				{
					cmd[0].caption = mResources.playNew;
					if (Rms.loadRMS("userAo" + ipSelect) != null)
					{
						cmd[0].caption = mResources.choitiep;
					}
					break;
				}
				if (text.Equals(string.Empty))
				{
					cmd[0].caption = mResources.playNew;
					if (Rms.loadRMS("userAo" + ipSelect) != null)
					{
						cmd[0].caption = mResources.choitiep;
					}
					break;
				}
				cmd[0].caption = mResources.playAcc + ": " + text;
				if (cmd[0].caption.Length > 23)
				{
					cmd[0].caption = cmd[0].caption.Substring(0, 23);
					cmd[0].caption += "...";
				}
				break;
			case 1:
				if (nCmdPlay == 1)
				{
					cmd[1] = new Command(mResources.playNew, this, 10100, null);
				}
				else
				{
					cmd[1] = new Command(mResources.change_account, this, 7, null);
				}
				break;
			case 2:
				if (nCmdPlay == 1)
				{
					cmd[2] = new Command(mResources.change_account, this, 7, null);
				}
				else
				{
					cmd[2] = new Command(string.Empty, this, 17, null);
				}
				break;
			case 3:
				if (nCmdPlay == 1)
				{
					cmd[3] = new Command(string.Empty, this, 17, null);
				}
				else
				{
					cmd[3] = new Command(mResources.option, this, 8, null);
				}
				break;
			case 4:
				cmd[4] = new Command(mResources.option, this, 8, null);
				break;
			}
			cmd[i].y = num;
			cmd[i].setType();
			cmd[i].x = (GameCanvas.w - cmd[i].w) / 2;
			num += 30;
		}
	}

	public static void doUpdateServer()
	{
		if (cmdUpdateServer == null && GameCanvas.serverScreen == null)
		{
			GameCanvas.serverScreen = new ServerListScreen();
		}
		Net.connectHTTP2(linkDefault, cmdUpdateServer);
	}

	public static void getServerList(string str)
	{
		lengthServer = new int[3];
		string[] array = Res.split(str.Trim(), ",", 0);
		Res.outz("tem leng= " + array.Length);
		mResources.loadLanguague(sbyte.Parse(array[array.Length - 2]));
		nameServer = new string[array.Length - 2];
		address = new string[array.Length - 2];
		port = new short[array.Length - 2];
		language = new sbyte[array.Length - 2];
		hasConnected = new bool[2];
		for (int i = 0; i < array.Length - 2; i++)
		{
			string[] array2 = Res.split(array[i].Trim(), ":", 0);
			nameServer[i] = array2[0];
			address[i] = array2[1];
			port[i] = short.Parse(array2[2]);
			language[i] = sbyte.Parse(array2[3].Trim());
			lengthServer[language[i]]++;
		}
		serverPriority = sbyte.Parse(array[array.Length - 1]);
		saveIP();
		GameCanvas.endDlg();
	}

	public override void paint(mGraphics g)
	{
		int num = 105;
		if (!loadScreen)
		{
			g.setColor(0);
			g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
			if (bigOk)
			{
			}
		}
		else
		{
			GameCanvas.paintBGGameScr(g);
		}
		int num2 = 2;
		mFont.tahoma_7_white.drawString(g, "v" + GameMidlet.VERSION + "(" + mGraphics.zoomLevel + ")", GameCanvas.w - 2, num2 + 15, 1, mFont.tahoma_7_grey);
		if (!isGetData || loadScreen)
		{
			if (mSystem.clientType == 1 && !GameCanvas.isTouch)
			{
				mFont.tahoma_7_white.drawString(g, linkweb, GameCanvas.w - 2, GameCanvas.h - 15, 1, mFont.tahoma_7_grey);
			}
			else
			{
				mFont.tahoma_7_white.drawString(g, linkweb, GameCanvas.w - 2, num2, 1, mFont.tahoma_7_grey);
			}
		}
		else
		{
			mFont.tahoma_7_white.drawString(g, linkweb, GameCanvas.w - 2, num2, 1, mFont.tahoma_7_grey);
		}
		int num3 = ((GameCanvas.w < 200) ? 160 : 180);
		if (cmdDeleteRMS != null)
		{
			mFont.tahoma_7_white.drawString(g, mResources.xoadulieu, GameCanvas.w - 2, GameCanvas.h - 15, 1, mFont.tahoma_7_grey);
		}
		if (GameCanvas.currentDialog == null)
		{
			if (!loadScreen)
			{
				if (!bigOk)
				{
					g.drawImage(LoginScr.imgTitle, GameCanvas.hw, GameCanvas.hh - 32, 3);
					if (!isGetData)
					{
						mFont.tahoma_7b_white.drawString(g, mResources.taidulieudechoi, GameCanvas.hw, GameCanvas.hh + 24, 2);
						if (cmdDownload != null)
						{
							cmdDownload.paint(g);
						}
					}
					else
					{
						if (cmdDownload != null)
						{
							cmdDownload.paint(g);
						}
						mFont.tahoma_7b_white.drawString(g, mResources.downloading_data + percent + "%", GameCanvas.w / 2, GameCanvas.hh + 24, 2);
						GameScr.paintOngMauPercent(GameScr.frBarPow20, GameScr.frBarPow21, GameScr.frBarPow22, GameCanvas.w / 2 - 50, GameCanvas.hh + 45, 100, 100f, g);
						GameScr.paintOngMauPercent(GameScr.frBarPow0, GameScr.frBarPow1, GameScr.frBarPow2, GameCanvas.w / 2 - 50, GameCanvas.hh + 45, 100, percent, g);
					}
				}
			}
			else
			{
				int num4 = GameCanvas.hh - 15 * cmd.Length - 15;
				if (num4 < 25)
				{
					num4 = 25;
				}
				if (LoginScr.imgTitle != null)
				{
					g.drawImage(LoginScr.imgTitle, GameCanvas.hw, num4, 3);
				}
				for (int i = 0; i < cmd.Length; i++)
				{
					cmd[i].paint(g);
				}
				g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
				if (testConnect == -1)
				{
					if (GameCanvas.gameTick % 20 > 10)
					{
						g.drawRegion(GameScr.imgRoomStat, 0, 14, 7, 7, 0, (GameCanvas.w - mFont.tahoma_7b_dark.getWidth(cmd[2 + nCmdPlay].caption) >> 1) - 10, cmd[2 + nCmdPlay].y + 10, 0);
					}
				}
				else
				{
					g.drawRegion(GameScr.imgRoomStat, 0, testConnect * 7, 7, 7, 0, (GameCanvas.w - mFont.tahoma_7b_dark.getWidth(cmd[2 + nCmdPlay].caption) >> 1) - 10, cmd[2 + nCmdPlay].y + 9, 0);
				}
			}
		}
		base.paint(g);
	}

	public void selectServer()
	{
		flagServer = 30;
		GameCanvas.startWaitDlg(mResources.PLEASEWAIT);
		if (Session_ME.gI().isConnected())
		{
			Session_ME.gI().close();
		}
		GameMidlet.IP = address[ipSelect];
		GameMidlet.PORT = port[ipSelect];
		if (language[ipSelect] != mResources.language)
		{
			mResources.loadLanguague(language[ipSelect]);
		}
		LoginScr.serverName = nameServer[ipSelect];
		initCommand();
		GameCanvas.connect();
	}

	public override void update()
	{
		if (waitToLogin)
		{
			tWaitToLogin++;
			if (tWaitToLogin == 50)
			{
				GameCanvas.serverScreen.selectServer();
			}
			if (tWaitToLogin == 100)
			{
				if (GameCanvas.loginScr == null)
				{
					GameCanvas.loginScr = new LoginScr();
				}
				GameCanvas.loginScr.doLogin();
				Service.gI().finishUpdate();
				waitToLogin = false;
			}
		}
		if (flagServer > 0)
		{
			flagServer--;
			if (flagServer == 0)
			{
				GameCanvas.endDlg();
			}
		}
		for (int i = 0; i < cmd.Length; i++)
		{
			if (i == selected)
			{
				cmd[i].isFocus = true;
			}
			else
			{
				cmd[i].isFocus = false;
			}
		}
		GameScr.cmx++;
		if (!loadScreen && (bigOk || percent == 100))
		{
			cmdDownload = null;
		}
		base.update();
	}

	private void processInput()
	{
		if (loadScreen)
		{
			center = new Command(string.Empty, this, cmd[selected].idAction, null);
		}
		else
		{
			center = cmdDownload;
		}
	}

	public static void updateDeleteData()
	{
		if (cmdDeleteRMS != null && cmdDeleteRMS.isPointerPressInside())
		{
			cmdDeleteRMS.performAction();
		}
	}

	public override void updateKey()
	{
		if (GameCanvas.isTouch)
		{
			updateDeleteData();
			if (cmdCallHotline != null && cmdCallHotline.isPointerPressInside())
			{
				cmdCallHotline.performAction();
			}
			if (!loadScreen)
			{
				if (cmdDownload != null && cmdDownload.isPointerPressInside())
				{
					cmdDownload.performAction();
				}
				base.updateKey();
				return;
			}
			for (int i = 0; i < cmd.Length; i++)
			{
				if (cmd[i] != null && cmd[i].isPointerPressInside())
				{
					cmd[i].performAction();
				}
			}
		}
		else if (loadScreen)
		{
			if (GameCanvas.keyPressed[8])
			{
				int num = ((mGraphics.zoomLevel <= 1) ? 4 : 2);
				GameCanvas.keyPressed[8] = false;
				selected++;
				if (selected > num)
				{
					selected = 0;
				}
				processInput();
			}
			if (GameCanvas.keyPressed[2])
			{
				int num2 = ((mGraphics.zoomLevel <= 1) ? 4 : 2);
				GameCanvas.keyPressed[2] = false;
				selected--;
				if (selected < 0)
				{
					selected = num2;
				}
				processInput();
			}
		}
		if (!isWait)
		{
			base.updateKey();
		}
	}

	public static void saveIP()
	{
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeByte(mResources.language);
			dataOutputStream.writeByte((sbyte)nameServer.Length);
			for (int i = 0; i < nameServer.Length; i++)
			{
				dataOutputStream.writeUTF(nameServer[i]);
				dataOutputStream.writeUTF(address[i]);
				dataOutputStream.writeShort(port[i]);
				dataOutputStream.writeByte(language[i]);
			}
			dataOutputStream.writeByte(serverPriority);
			Rms.saveRMS("NRlink2", dataOutputStream.toByteArray());
			dataOutputStream.close();
			SplashScr.loadIP();
		}
		catch (Exception)
		{
		}
	}

	public static bool allServerConnected()
	{
		for (int i = 0; i < 2; i++)
		{
			if (!hasConnected[i])
			{
				return false;
			}
		}
		return true;
	}

	public static void loadIP()
	{
		sbyte[] array = Rms.loadRMS("NRlink2");
		if (array == null)
		{
			getServerList(linkDefault);
			return;
		}
		DataInputStream dataInputStream = new DataInputStream(array);
		if (dataInputStream == null)
		{
			return;
		}
		try
		{
			lengthServer = new int[3];
			mResources.loadLanguague(dataInputStream.readByte());
			sbyte b = dataInputStream.readByte();
			nameServer = new string[b];
			address = new string[b];
			port = new short[b];
			language = new sbyte[b];
			for (int i = 0; i < b; i++)
			{
				nameServer[i] = dataInputStream.readUTF();
				address[i] = dataInputStream.readUTF();
				port[i] = dataInputStream.readShort();
				language[i] = dataInputStream.readByte();
				lengthServer[language[i]]++;
			}
			serverPriority = dataInputStream.readByte();
			dataInputStream.close();
			SplashScr.loadIP();
		}
		catch (Exception)
		{
		}
	}

	public override void switchToMe()
	{
		GameCanvas.connect();
		GameScr.cmy = 0;
		GameScr.cmx = 0;
		initCommand();
		isWait = false;
		GameCanvas.loginScr = null;
		string text = Rms.loadRMSString("ResVersion");
		int num = ((text == null || !(text != string.Empty)) ? (-1) : int.Parse(text));
		if (num > 0)
		{
			loadScreen = true;
			GameCanvas.loadBG(0);
		}
		bigOk = true;
		cmd[2 + nCmdPlay].caption = mResources.server + ": " + nameServer[ipSelect];
		center = new Command(string.Empty, this, cmd[selected].idAction, null);
		cmd[1 + nCmdPlay].caption = mResources.change_account;
		if (cmd.Length == 4 + nCmdPlay)
		{
			cmd[3 + nCmdPlay].caption = mResources.option;
		}
		mSystem.resetCurInapp();
		base.switchToMe();
	}

	public void switchToMe2()
	{
		GameScr.cmy = 0;
		GameScr.cmx = 0;
		initCommand();
		isWait = false;
		GameCanvas.loginScr = null;
		string text = Rms.loadRMSString("ResVersion");
		int num = ((text == null || !(text != string.Empty)) ? (-1) : int.Parse(text));
		if (num > 0)
		{
			loadScreen = true;
			GameCanvas.loadBG(0);
		}
		bigOk = true;
		cmd[2 + nCmdPlay].caption = mResources.server + ": " + nameServer[ipSelect];
		center = new Command(string.Empty, this, cmd[selected].idAction, null);
		cmd[1 + nCmdPlay].caption = mResources.change_account;
		if (cmd.Length == 4 + nCmdPlay)
		{
			cmd[3 + nCmdPlay].caption = mResources.option;
		}
		mSystem.resetCurInapp();
		base.switchToMe();
	}

	public void connectOk()
	{
	}

	public void cancel()
	{
		if (GameCanvas.serverScreen == null)
		{
			GameCanvas.serverScreen = new ServerListScreen();
		}
		demPercent = 0;
		percent = 0;
		stopDownload = true;
		GameCanvas.serverScreen.show2();
		isGetData = false;
		cmdDownload.isFocus = true;
		center = new Command(string.Empty, this, 2, null);
	}

	public void perform(int idAction, object p)
	{
		Res.outz("perform " + idAction);
		if (idAction == 1000)
		{
			GameCanvas.connect();
		}
		if (idAction == 1 || idAction == 4)
		{
			cancel();
		}
		if (idAction == 2)
		{
			stopDownload = false;
			cmdDownload = new Command(mResources.huy, this, 4, null);
			cmdDownload.x = GameCanvas.w / 2 - mScreen.cmdW / 2;
			cmdDownload.y = GameCanvas.hh + 65;
			right = null;
			if (!GameCanvas.isTouch)
			{
				cmdDownload.x = GameCanvas.w / 2 - mScreen.cmdW / 2;
				cmdDownload.y = GameCanvas.h - mScreen.cmdH - 1;
			}
			center = new Command(string.Empty, this, 4, null);
			if (!isGetData)
			{
				Service.gI().getResource(1, null);
				if (!GameCanvas.isTouch)
				{
					cmdDownload.isFocus = true;
					center = new Command(string.Empty, this, 4, null);
				}
				isGetData = true;
			}
		}
		if (idAction == 3)
		{
			Res.outz("toi day");
			if (GameCanvas.loginScr == null)
			{
				GameCanvas.loginScr = new LoginScr();
			}
			GameCanvas.loginScr.switchToMe();
			bool flag = Rms.loadRMSString("acc") != null && ((!Rms.loadRMSString("acc").Equals(string.Empty)) ? true : false);
			bool flag2 = Rms.loadRMSString("userAo" + ipSelect) != null && ((!Rms.loadRMSString("userAo" + ipSelect).Equals(string.Empty)) ? true : false);
			if (!flag && !flag2)
			{
				GameCanvas.connect();
				string text = Rms.loadRMSString("userAo" + ipSelect);
				if (text == null || text.Equals(string.Empty))
				{
					Service.gI().login2(string.Empty);
				}
				else
				{
					GameCanvas.loginScr.isLogin2 = true;
					GameCanvas.connect();
					Service.gI().setClientType();
					Service.gI().login(text, string.Empty, GameMidlet.VERSION, 1);
				}
				if (Session_ME.connected)
				{
					GameCanvas.startWaitDlg();
				}
				else
				{
					GameCanvas.startOKDlg(mResources.maychutathoacmatsong);
				}
			}
			else
			{
				GameCanvas.loginScr.doLogin();
			}
			LoginScr.serverName = nameServer[ipSelect];
		}
		if (idAction == 10100)
		{
			if (GameCanvas.loginScr == null)
			{
				GameCanvas.loginScr = new LoginScr();
			}
			GameCanvas.loginScr.switchToMe();
			GameCanvas.connect();
			Service.gI().login2(string.Empty);
			Res.outz("tao user ao");
			GameCanvas.startWaitDlg();
			LoginScr.serverName = nameServer[ipSelect];
		}
		if (idAction == 5)
		{
			doUpdateServer();
			if (nameServer.Length == 1)
			{
				return;
			}
			MyVector myVector = new MyVector(string.Empty);
			for (int i = 0; i < nameServer.Length; i++)
			{
				myVector.addElement(new Command(nameServer[i], this, 6, null));
			}
			GameCanvas.menu.startAt(myVector, 0);
			if (!GameCanvas.isTouch)
			{
				GameCanvas.menu.menuSelectedItem = ipSelect;
			}
		}
		if (idAction == 6)
		{
			ipSelect = GameCanvas.menu.menuSelectedItem;
			selectServer();
		}
		if (idAction == 7)
		{
			if (GameCanvas.loginScr == null)
			{
				GameCanvas.loginScr = new LoginScr();
			}
			GameCanvas.loginScr.switchToMe();
		}
		if (idAction == 8)
		{
			MyVector myVector2 = new MyVector("cau hinh");
			string str = Rms.loadRMSInt("lowGraphic") == 1 ? mResources.cauhinhcao : mResources.cauhinhthap;
            myVector2.addElement(new Command(str, this, 9, null));
			string str2 = Rms.loadRMSInt("levelScreenKN") == 1 ? mResources.x2Screen : mResources.x1Screen;
            myVector2.addElement(new Command(str2, this, 10, null));
            GameCanvas.menu.startAt(myVector2, 0);
            GameCanvas.menu.menuSelectedItem = -1;
        }
        if (idAction == 9)
		{
			Rms.saveRMSInt("lowGraphic", Rms.loadRMSInt("lowGraphic") == 1 ? 0 : 1);
			GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
		}
		if (idAction == 10)
		{
            Rms.clearAll();
            Rms.saveRMSInt("levelScreenKN", Rms.loadRMSInt("levelScreenKN") == 1 ? 0 : 1);
            GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
		}
		if (idAction == 11)
		{
			if (GameCanvas.loginScr == null)
			{
				GameCanvas.loginScr = new LoginScr();
			}
			GameCanvas.loginScr.switchToMe();
			string text2 = Rms.loadRMSString("userAo" + ipSelect);
			if (text2 == null || text2.Equals(string.Empty))
			{
				Service.gI().login2(string.Empty);
			}
			else
			{
				GameCanvas.loginScr.isLogin2 = true;
				GameCanvas.connect();
				Service.gI().setClientType();
				Service.gI().login(text2, string.Empty, GameMidlet.VERSION, 1);
			}
			GameCanvas.startWaitDlg(mResources.PLEASEWAIT);
			Res.outz("tao user ao");
		}
		if (idAction == 12)
		{
			GameMidlet.instance.exit();
		}
		if (idAction == 13 && (!isGetData || loadScreen))
		{
			switch (mSystem.clientType)
			{
			case 1:
				mSystem.callHotlineJava();
				break;
			case 3:
			case 5:
				mSystem.callHotlineIphone();
				break;
			case 6:
				mSystem.callHotlineWindowsPhone();
				break;
			case 4:
				mSystem.callHotlinePC();
				break;
			}
		}
		if (idAction == 14)
		{
			Command cmdYes = new Command(mResources.YES, GameCanvas.serverScreen, 15, null);
			Command cmdNo = new Command(mResources.NO, GameCanvas.serverScreen, 16, null);
			GameCanvas.startYesNoDlg(mResources.deletaDataNote, cmdYes, cmdNo);
		}
		if (idAction == 15)
		{
			Rms.clearAll2();
			GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
		}
		if (idAction == 16)
		{
			InfoDlg.hide();
			GameCanvas.currentDialog = null;
		}
		if (idAction == 17)
		{
			if (GameCanvas.serverScr == null)
			{
				GameCanvas.serverScr = new ServerScr();
			}
			GameCanvas.serverScr.switchToMe();
		}
	}

	public void init()
	{
		if (!loadScreen)
		{
			cmdDownload = new Command(mResources.taidulieu, this, 2, null);
			cmdDownload.isFocus = true;
			cmdDownload.x = GameCanvas.w / 2 - mScreen.cmdW / 2;
			cmdDownload.y = GameCanvas.hh + 45;
			if (cmdDownload.y > GameCanvas.h - 26)
			{
				cmdDownload.y = GameCanvas.h - 26;
			}
		}
		if (!GameCanvas.isTouch)
		{
			selected = 0;
			processInput();
		}
	}

	public void show2()
	{
		GameScr.cmx = 0;
		GameScr.cmy = 0;
		initCommand();
		loadScreen = false;
		percent = 0;
		bigOk = false;
		isGetData = false;
		p = 0;
		demPercent = 0;
		strWait = mResources.PLEASEWAIT;
		init();
		base.switchToMe();
	}

	public void setLinkDefault(sbyte language)
	{
		if (language == 2)
		{
			if (mSystem.clientType == 1)
			{
				linkDefault = javaIn;
			}
			else
			{
				linkDefault = smartPhoneIn;
			}
		}
		else if (language == 1)
		{
			linkDefault = javaE;
			if (mSystem.clientType == 1)
			{
				linkDefault = javaE;
			}
			else
			{
				linkDefault = smartPhoneE;
			}
		}
		else
		{
			//linkDefault = File.ReadAllLines("ModData\\ServerList.txt")[0];

			//linkDefault = javaVN;
			//if (mSystem.clientType == 1)
			//{
			//	linkDefault = javaVN;
			//}
			//else
			//{
			//	linkDefault = smartPhoneVN;
			//}
		}
	}
}
