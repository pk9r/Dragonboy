using System;
using UnityEngine;

public class ServerScr : mScreen, IActionListener
{
	internal int mainSelect;

	internal MyVector vecServer = new MyVector();

	internal Command cmdCheck;

	public const int icmd = 100;

	internal int wc;

	internal int hc;

	internal int w2c;

	internal int numw;

	internal int numh;

	internal Command cmdGlobal;

	internal Command cmdVietNam;

	internal const string RMS_SELECT_AREA = "area_select";

	public bool isChooseArea;

	public bool isPaintNewUi;

	internal ListNew list;

	internal sbyte select_Area;

	internal sbyte select_Lang;

	internal sbyte select_typeSv;

	internal Command cmdChooseArea;

	internal bool isPaint_select_area;

	internal bool isPaint_select_lang;

	internal int x;

	internal int y;

	internal int w;

	internal int h;

	internal int xName;

	internal int yName;

	internal int xsub;

	internal int ysub;

	internal int wsub;

	internal int hsub;

	internal int xsubpaint;

	internal int ysubpaint;

	internal int xPop;

	internal int yPop;

	internal int wPop;

	internal int hPop;

	internal int xinfo;

	internal int yinfo;

	internal int winfo;

	internal int hinfo;

	internal int yBox;

	internal int wBox;

	internal int hBox;

	internal int ntypeSv;

	internal int xPopUp_Area;

	internal int yPopUp_Area;

	internal int xPopUp_Lang;

	internal int yPopUp_Lang;

	internal int htext = 15;

	internal string[] strLang = new string[3] { "Tiếng Việt", "English", "Indo" };

	internal string[] strArea = new string[2] { "VIỆT NAM", "GLOBAL" };

	internal string[] strTypeSV = new string[2] { "Máy chủ tiêu chuẩn", "Máy chủ theo mùa" };

	internal string[] strTypeSV_info = new string[2] { "Máy chủ tiêu chuẩn:\n-Không reset.\nTiến trình game bình thường.", "Máy chủ theo mùa:\n -Reset toàn bộ server và phát thưởng vào cuối mùa.\n x3 Sức mạnh\n x3 Tiềm năng\n x3 Vàng\n x3 Vật phẩm khác" };

	public int cmy;

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
		Debug.LogError(">>>>>>switchToMe: ");
		SoundMn.gI().stopAll();
		base.switchToMe();
		Load_NewUI();
		if (!isPaintNewUi && !isChooseArea)
		{
			cmdGlobal = new Command(strArea[0], this, 98, null);
			cmdGlobal.x = 0;
			cmdGlobal.y = 0;
			cmdVietNam = new Command(strArea[1], this, 97, null);
			cmdVietNam.x = 50;
			cmdVietNam.y = 0;
			vecServer = new MyVector();
			vecServer.addElement(cmdGlobal);
			vecServer.addElement(cmdVietNam);
			sort();
		}
	}

	internal void sort()
	{
		mainSelect = ServerListScreen.ipSelect;
		w2c = 5;
		wc = 76;
		hc = mScreen.cmdH;
		numw = 2;
		if (vecServer.size() > 2)
			numw = GameCanvas.w / (wc + w2c);
		numh = vecServer.size() / numw + ((vecServer.size() % numw != 0) ? 1 : 0);
		for (int i = 0; i < vecServer.size(); i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			if (command != null)
			{
				int num = GameCanvas.hw - numw * (wc + w2c) / 2 + i % numw * (wc + w2c);
				int num2 = GameCanvas.hh - numh * (hc + w2c) / 2 + i / numw * (hc + w2c);
				command.x = num;
				command.y = num2;
				command.w = wc;
			}
		}
	}

	internal void sort_newUI()
	{
		mainSelect = ServerListScreen.ipSelect;
		w2c = 5;
		wc = 76;
		hc = mScreen.cmdH;
		numw = 1;
		int num = xsub + wsub / 2 + 3;
		ysubpaint = ysub + 5;
		numw = wsub / (wc + w2c);
		numh = vecServer.size() / numw + ((vecServer.size() % numw != 0) ? 1 : 0);
		xsubpaint = num - numw * (wc + w2c) / 2;
		for (int i = 0; i < vecServer.size(); i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			if (command != null)
			{
				int num2 = xsubpaint + i % numw * (wc + w2c);
				int num3 = ysubpaint + i / numw * (hc + w2c);
				command.x = num2;
				command.y = num3;
				command.w = wc;
			}
		}
		list = new ListNew(xsub, ysub, wsub, hsub, 0, 0, 0, true);
		list.setMaxCamera(numh * (hc + w2c) - hsub);
		list.resetList();
	}

	public override void update()
	{
		GameScr.cmx++;
		if (GameScr.cmx > GameCanvas.w * 3 + 100)
			GameScr.cmx = 100;
		if (!isPaintNewUi)
		{
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
		UpdTouch_NewUI();
		UpdTouch_NewUI_Popup();
	}

	public override void paint(mGraphics g)
	{
		GameCanvas.paintBGGameScr(g);
		if (isChooseArea)
			paintChooseArea(g);
		else if (isPaintNewUi)
		{
			paintNewSelectMenu(g);
		}
		else
		{
			for (int i = 0; i < vecServer.size(); i++)
			{
				if (vecServer.elementAt(i) != null)
					((Command)vecServer.elementAt(i)).paint(g);
			}
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
		default:
			if (idAction == 999)
			{
				Save_RMS_Area();
				SetNewSelectMenu(select_Area, 0);
			}
			else
			{
				ServerListScreen.ipSelect = idAction - 100;
				GameCanvas.serverScreen.selectServer();
				GameCanvas.serverScreen.switchToMe();
			}
			break;
		case 97:
		{
			if (isPaintNewUi)
				break;
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
			if (isPaintNewUi)
				break;
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
		}
	}

	internal void SetNewSelectMenu(int area, int typeSv)
	{
		isChooseArea = false;
		isPaintNewUi = true;
		w = GameCanvas.w / 3 * 2;
		h = GameCanvas.h / 3 * 2;
		x = (GameCanvas.w - w) / 2;
		y = (GameCanvas.h - h) / 2 + 20;
		xName = GameCanvas.w / 2;
		yName = y - 30;
		wsub = w / 3 * 2;
		wPop = w - wsub - 15;
		if (wPop < 80)
		{
			wPop = 80;
			wsub = w - wPop - 15;
		}
		hsub = h - 10;
		xsub = x + w - wsub - 5;
		ysub = y + 5;
		xPop = x + 5;
		yPop = y + 5;
		hPop = 20;
		xinfo = x + 5;
		yinfo = y + strTypeSV.Length * (hPop + 5) + 5;
		winfo = wPop;
		hinfo = h - (5 + strTypeSV.Length * (hPop + 5) + 5);
		yBox = 10;
		wBox = 70;
		hBox = 20;
		GetVecTypeSv((sbyte)area, (sbyte)typeSv);
	}

	internal void GetVecTypeSv(sbyte area, sbyte typeSv)
	{
		vecServer.removeAllElements();
		ntypeSv = 1;
		select_Area = area;
		mResources.loadLanguague(area);
		for (int i = 0; i < ServerListScreen.nameServer.Length; i++)
		{
			if (area == 1)
			{
				if (ServerListScreen.language[i] != 0 && ServerListScreen.typeSv[i] == 1)
					ntypeSv = 2;
			}
			else if (ServerListScreen.typeSv[i] == 1)
			{
				ntypeSv = 2;
			}
		}
		if (typeSv > (sbyte)(ntypeSv - 1))
			typeSv = (sbyte)(ntypeSv - 1);
		select_typeSv = typeSv;
		for (int j = 0; j < ServerListScreen.nameServer.Length; j++)
		{
			if (area == 1)
			{
				if (ServerListScreen.language[j] != 0)
				{
					if (ServerListScreen.typeSv[j] == 1)
						ntypeSv = 2;
					if (ServerListScreen.typeSv[j] == typeSv)
					{
						Command command = new Command(ServerListScreen.nameServer[j], this, 100 + j, null);
						command.isPaintNew = ServerListScreen.isNew[j] == 1;
						vecServer.addElement(command);
					}
				}
			}
			else
			{
				if (ServerListScreen.typeSv[j] == 1)
					ntypeSv = 2;
				if (ServerListScreen.language[j] == 0 && ServerListScreen.typeSv[j] == typeSv)
				{
					Command command = new Command(ServerListScreen.nameServer[j], this, 100 + j, null);
					command.isPaintNew = ServerListScreen.isNew[j] == 1;
					vecServer.addElement(command);
				}
			}
		}
		Sort_NewSv();
		sort_newUI();
	}

	internal void paintChooseArea(mGraphics g)
	{
		if (isChooseArea)
		{
			paint_Area(g, GameCanvas.hw - wBox / 2, yBox);
			paint_Lang(g, GameCanvas.hw + 20, yBox);
			cmdChooseArea.paint(g);
		}
	}

	internal void paintNewSelectMenu(mGraphics g)
	{
		if (!isPaintNewUi)
			return;
		g.setColor(14601141);
		g.fillRect(x, y, w, h);
		PopUp.paintPopUp(g, xName - 50, yName, 100, 20, 0, true);
		mFont.tahoma_7b_dark.drawString(g, mResources.selectServer2, xName, yName + 5, 2);
		for (int i = 0; i < ntypeSv; i++)
		{
			int num = yPop + i * (hPop + 5);
			PopUp.paintPopUp(g, xPop, num, wPop, hPop, (select_typeSv == i) ? 1 : 0, true);
			mFont.tahoma_7b_dark.drawString(g, strTypeSV[i], xPop + wPop / 2, num + 5, 2);
		}
		g.setColor(10254674);
		g.fillRect(xinfo, yinfo, winfo, hinfo);
		string[] array = mFont.tahoma_7.splitFontArray(strTypeSV_info[select_typeSv], winfo - 10);
		for (int j = 0; j < array.Length; j++)
		{
			mFont.tahoma_7_white.drawString(g, array[j], xinfo + 5, yinfo + 5 + j * 11, 0);
		}
		paint_Area(g, 10, yBox);
		paint_Lang(g, GameCanvas.w - wBox - 10, yBox);
		g.setColor(10254674);
		g.fillRect(xsub, ysub, wsub, hsub);
		g.setClip(xsub, ysub, wsub, hsub);
		g.translate(0, -list.cmx);
		for (int k = 0; k < vecServer.size(); k++)
		{
			Command command = (Command)vecServer.elementAt(k);
			if (command != null)
			{
				command.paint(g);
				if (command.isPaintNew && GameCanvas.gameTick % 10 > 1)
					g.drawImage(Panel.imgNew, command.x + 60, command.y, 0);
			}
		}
		GameCanvas.resetTrans(g);
	}

	internal void paint_Area(mGraphics g, int x, int y)
	{
		xPopUp_Area = x;
		PopUp.paintPopUp(g, x, y, wBox, hBox, 0, true);
		mFont.tahoma_7b_dark.drawString(g, strArea[select_Area], x + (wBox - 10) / 2, y + 5, 2);
		g.drawRegion(Mob.imgHP, 0, 30, 9, 6, 0, x + wBox - 10, y + 14, mGraphics.BOTTOM | mGraphics.HCENTER);
		if (!isPaint_select_area)
			return;
		yPopUp_Area = y + hBox + 5;
		g.setColor(10254674);
		g.fillRect(x, yPopUp_Area, wBox, strArea.Length * htext + 1);
		for (int i = 0; i < strArea.Length; i++)
		{
			mFont.tahoma_7_white.drawString(g, strArea[i], x + wBox / 2, yPopUp_Area + i * htext + 2, 2);
			if (select_Area == i)
			{
				g.setColor(15591444);
				g.drawRect(x + 2, yPopUp_Area + i * htext + 1, wBox - 4, htext - 2);
			}
		}
	}

	internal void paint_Lang(mGraphics g, int x, int y)
	{
	}

	internal void UpdTouch_NewUI()
	{
		if (!isPaintNewUi)
			return;
		int num = 0;
		if (list != null)
		{
			list.moveCamera();
			if (GameCanvas.isPointer(xsub, 0, wsub, GameCanvas.h))
				list.update_Pos_UP_DOWN();
			num = list.cmx;
		}
		if (GameCanvas.isPointSelect(xsub, ysub, wsub, hsub))
		{
			int num2 = (GameCanvas.px - xsubpaint) / (wc + w2c) + (GameCanvas.py - ysubpaint + num) / (hc + w2c) * numw;
			int num3 = vecServer.size();
			if (num2 >= 0 && num2 < num3)
			{
				mainSelect = num2;
				Command command = (Command)vecServer.elementAt(mainSelect);
				if (command != null)
				{
					command.isFocus = true;
					command.performAction();
				}
			}
		}
		if (ntypeSv == 1)
			return;
		for (sbyte b = 0; b < ntypeSv; b++)
		{
			int num4 = yPop + b * (hPop + 5);
			if (GameCanvas.isPointerHoldIn(xPop, num4, wPop, hPop) && GameCanvas.isPointerDown)
			{
				GetVecTypeSv(select_Area, b);
				break;
			}
		}
	}

	internal void UpdTouch_NewUI_Popup()
	{
		if (GameCanvas.isPointer(xPopUp_Area, yBox, wBox, hBox) && GameCanvas.isPointerJustRelease)
		{
			isPaint_select_area = !isPaint_select_area;
			isPaint_select_lang = false;
			GameCanvas.isPointerJustRelease = false;
		}
		if (!isPaint_select_area)
			return;
		for (sbyte b = 0; b < strArea.Length; b++)
		{
			int num = yPopUp_Area + b * htext;
			if (GameCanvas.isPointerHoldIn(xPopUp_Area, num, wBox, htext) && GameCanvas.isPointerDown)
			{
				if (isChooseArea)
					select_Area = b;
				else
					SetNewSelectMenu(b, select_typeSv);
				isPaint_select_lang = (isPaint_select_area = false);
				break;
			}
		}
	}

	internal void Load_NewUI()
	{
		if (GameCanvas.isTouch)
		{
			if (Rms.loadRMS("area_select") == null)
			{
				isChooseArea = true;
				cmdChooseArea = new Command(mResources.OK, this, 999, null);
				cmdChooseArea.x = GameCanvas.hw - 38;
				cmdChooseArea.y = GameCanvas.hh + 50;
				vecServer = new MyVector();
				vecServer.addElement(cmdChooseArea);
				yBox = GameCanvas.hh - 30;
				wBox = 70;
				hBox = 20;
			}
			else
			{
				isChooseArea = false;
				Load_RMS_Area();
				SetNewSelectMenu(select_Area, select_typeSv);
			}
		}
	}

	internal void Save_RMS_Area()
	{
		Rms.saveRMS("area_select", new sbyte[2] { select_Area, select_Lang });
	}

	internal void Load_RMS_Area()
	{
		sbyte[] array = Rms.loadRMS("area_select");
		try
		{
			select_Area = array[0];
			select_Lang = array[1];
		}
		catch (Exception)
		{
			select_Area = (select_Lang = 0);
		}
	}

	public void Sort_NewSv()
	{
		for (int i = 0; i < vecServer.size() - 1; i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			for (int j = i + 1; j < vecServer.size(); j++)
			{
				Command command2 = (Command)vecServer.elementAt(j);
				if (command2.isPaintNew && !command.isPaintNew)
				{
					Command command3 = command2;
					command2 = command;
					command = command3;
					vecServer.setElementAt(command, i);
					vecServer.setElementAt(command2, j);
				}
			}
		}
	}
}
