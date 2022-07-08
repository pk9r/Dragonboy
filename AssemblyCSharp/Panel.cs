using System;
using Assets.src.g;
using UnityEngine;

public class Panel : IActionListener, IChatable
{
	public bool isShow;

	public int X;

	public int Y;

	public int W;

	public int H;

	public int ITEM_HEIGHT;

	public int TAB_W;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	public int xc;

	public int[] cmyLast;

	public int cmtoX;

	public int cmx;

	public int cmxLim;

	public int cmxMap;

	public int cmyMap;

	public int cmxMapLim;

	public int cmyMapLim;

	public int cmyQuest;

	public static Image imgBantay;

	public static Image imgX;

	public static Image imgMap;

	public TabClanIcon tabIcon;

	public MyVector vItemCombine = new MyVector();

	public int moneyGD;

	public int friendMoneyGD;

	public bool isLock;

	public bool isFriendLock;

	public bool isAccept;

	public bool isFriendAccep;

	public string topName;

	public ChatTextField chatTField;

	public static string specialInfo;

	public static short spearcialImage;

	public static Image imgStar;

	public static Image imgMaxStar;

	public static Image imgStar8;

	public static Image imgNew;

	public static Image imgXu;

	public static Image imgTicket;

	public static Image imgLuong;

	public static Image imgLuongKhoa;

	private static Image imgUp;

	private static Image imgDown;

	private int pa1;

	private int pa2;

	private bool trans;

	private int pX;

	private int pY;

	private Command left = new Command(mResources.SELECT, 0);

	public int type;

	public int currentTabIndex;

	public int startTabPos;

	public int[] lastTabIndex;

	public string[][] currentTabName;

	private int[] currClanOption;

	public int mainTabPos = 4;

	public int shopTabPos = 50;

	public int boxTabPos = 50;

	public string[][] mainTabName;

	public string[] mapNames;

	public string[] planetNames;

	public static string[] strTool = new string[7]
	{
		mResources.gameInfo,
		mResources.change_flag,
		mResources.change_zone,
		mResources.chat_world,
		mResources.account,
		mResources.option,
		mResources.change_account
	};

	public static string[] strCauhinh = new string[4]
	{
		(!Char.isPaintAura) ? mResources.aura_on : mResources.aura_off,
		(!GameCanvas.isPlaySound) ? mResources.turnOnSound : mResources.turnOffSound,
		GameCanvas.lowGraphic ? mResources.cauhinhcao : mResources.cauhinhthap,
		(mGraphics.zoomLevel <= 1) ? mResources.x2Screen : mResources.x1Screen,
    };

	public static string[] strAccount = new string[5]
	{
		mResources.inventory_Pass,
		mResources.friend,
		mResources.enemy,
		mResources.msg,
		mResources.charger
	};

	public static string[] strAuto = new string[1] { mResources.useGem };

	public static int graphics = 0;

	public string[][] shopTabName;

	public int[] maxPageShop;

	public int[] currPageShop;

	private static string[][] boxTabName = new string[2][]
	{
		mResources.chestt,
		mResources.inventory
	};

	private static string[][] boxCombine = new string[2][]
	{
		mResources.combine,
		mResources.inventory
	};

	private static string[][] boxZone = new string[1][] { mResources.zonee };

	private static string[][] boxMap = new string[1][] { mResources.mapp };

	private static string[][] boxGD = new string[3][]
	{
		mResources.inventory,
		mResources.item_give,
		mResources.item_receive
	};

	private static string[][] boxPet = mResources.petMainTab;

	public string[][][] tabName = new string[27][][]
	{
		null,
		null,
		boxTabName,
		boxZone,
		boxMap,
		null,
		null,
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		boxCombine,
		boxGD,
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		boxPet,
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } },
		new string[1][] { new string[1] { string.Empty } }
	};

	private static sbyte BOX_BAG = 0;

	private static sbyte BAG_BOX = 1;

	private static sbyte BOX_BODY = 2;

	private static sbyte BODY_BOX = 3;

	private static sbyte BAG_BODY = 4;

	private static sbyte BODY_BAG = 5;

	private static sbyte BAG_PET = 6;

	private static sbyte PET_BAG = 7;

	public int hasUse;

	public int hasUseBag;

	public int currentListLength;

	private int[] lastSelect;

	public static int[] mapIdTraidat = new int[16]
	{
		21, 0, 1, 2, 24, 3, 4, 5, 6, 27,
		28, 29, 30, 42, 47, 46
	};

	public static int[] mapXTraidat = new int[16]
	{
		39, 42, 105, 93, 61, 93, 142, 165, 210, 100,
		165, 220, 233, 10, 125, 125
	};

	public static int[] mapYTraidat = new int[16]
	{
		28, 60, 48, 96, 88, 131, 136, 95, 32, 200,
		189, 167, 120, 110, 20, 20
	};

	public static int[] mapIdNamek = new int[14]
	{
		22, 7, 8, 9, 25, 11, 12, 13, 10, 31,
		32, 33, 34, 43
	};

	public static int[] mapXNamek = new int[14]
	{
		55, 30, 93, 80, 24, 149, 219, 220, 233, 170,
		148, 195, 148, 10
	};

	public static int[] mapYNamek = new int[14]
	{
		136, 84, 69, 34, 25, 42, 32, 110, 192, 70,
		106, 156, 210, 57
	};

	public static int[] mapIdSaya = new int[14]
	{
		23, 14, 15, 16, 26, 17, 18, 20, 19, 35,
		36, 37, 38, 44
	};

	public static int[] mapXSaya = new int[14]
	{
		90, 95, 144, 234, 231, 122, 176, 158, 205, 54,
		105, 159, 231, 27
	};

	public static int[] mapYSaya = new int[14]
	{
		10, 43, 20, 36, 69, 87, 112, 167, 160, 151,
		173, 207, 194, 29
	};

	public static int[][] mapId = new int[3][] { mapIdTraidat, mapIdNamek, mapIdSaya };

	public static int[][] mapX = new int[3][] { mapXTraidat, mapXNamek, mapXSaya };

	public static int[][] mapY = new int[3][] { mapYTraidat, mapYNamek, mapYSaya };

	public Item currItem;

	public Clan currClan;

	public ClanMessage currMess;

	public Member currMem;

	public Clan[] clans;

	public MyVector member;

	public MyVector myMember;

	public MyVector logChat = new MyVector();

	public MyVector vPlayerMenu = new MyVector();

	public MyVector vFriend = new MyVector();

	public MyVector vMyGD = new MyVector();

	public MyVector vFriendGD = new MyVector();

	public MyVector vTop = new MyVector();

	public MyVector vEnemy = new MyVector();

	public MyVector vFlag = new MyVector();

	public Command cmdClose;

	public static bool CanNapTien = false;

	public static int WIDTH_PANEL = 240;

	private int position;

	public Char charMenu;

	private bool isThachDau;

	public int typeShop = -1;

	public int xScroll;

	public int yScroll;

	public int wScroll;

	public int hScroll;

	public ChatPopup cp;

	public int idIcon;

	public int[] partID;

	private int timeShow;

	public bool isBoxClan;

	public int w;

	private int pa;

	public int selected;

	private int cSelected;

	private bool isClanOption;

	public bool isSearchClan;

	public bool isMessage;

	public bool isViewMember;

	public const int TYPE_MAIN = 0;

	public const int TYPE_SHOP = 1;

	public const int TYPE_BOX = 2;

	public const int TYPE_ZONE = 3;

	public const int TYPE_MAP = 4;

	public const int TYPE_CLANS = 5;

	public const int TYPE_INFOMATION = 6;

	public const int TYPE_BODY = 7;

	public const int TYPE_MESS = 8;

	public const int TYPE_ARCHIVEMENT = 9;

	public const int PLAYER_MENU = 10;

	public const int TYPE_FRIEND = 11;

	public const int TYPE_COMBINE = 12;

	public const int TYPE_GIAODICH = 13;

	public const int TYPE_MAPTRANS = 14;

	public const int TYPE_TOP = 15;

	public const int TYPE_ENEMY = 16;

	public const int TYPE_KIGUI = 17;

	public const int TYPE_FLAG = 18;

	public const int TYPE_OPTION = 19;

	public const int TYPE_ACCOUNT = 20;

	public const int TYPE_PET_MAIN = 21;

	public const int TYPE_AUTO = 22;

	public const int TYPE_GAMEINFO = 23;

	public const int TYPE_GAMEINFOSUB = 24;

	public const int TYPE_SPEACIALSKILL = 25;

	private int pointerDownTime;

	private int pointerDownFirstX;

	private int[] pointerDownLastX = new int[3];

	private bool pointerIsDowning;

	private bool isDownWhenRunning;

	private bool wantUpdateList;

	private int waitToPerform;

	private int cmRun;

	private int keyTouchLock = -1;

	private int keyToundGD = -1;

	private int keyTouchCombine = -1;

	private int keyTouchMapButton = -1;

	public int indexMouse = -1;

	private bool justRelease;

	private int keyTouchTab = -1;

	public string[][] clansOption;

	public string clanInfo = string.Empty;

	public string clanReport = string.Empty;

	private bool isHaveClan;

	private Scroll scroll;

	private int cmvx;

	private int cmdx;

	private bool isSelectPlayerMenu;

	private string[] strStatus = new string[6]
	{
		mResources.follow,
		mResources.defend,
		mResources.attack,
		mResources.gohome,
		mResources.fusion,
		mResources.fusionForever
	};

	private int currentButtonPress;

	private int[] zoneColor = new int[3] { 43520, 14743570, 14155776 };

	public string[] combineInfo;

	public string[] combineTopInfo;

	public static int[] color1 = new int[3] { 2327248, 8982199, 16713222 };

	public static int[] color2 = new int[3] { 4583423, 16719103, 16714764 };

	private bool isUp;

	private int compare;

	public static string strWantToBuy = string.Empty;

	public int xstart;

	public int ystart;

	public int popupW = 140;

	public int popupH = 160;

	public int cmySK;

	public int cmtoYSK;

	public int cmdySK;

	public int cmvySK;

	public int cmyLimSK;

	public int popupY;

	public int popupX;

	public int isborderIndex;

	public int isselectedRow;

	public int indexSize = 28;

	public int indexTitle;

	public int indexSelect;

	public int indexRow = -1;

	public int indexRowMax;

	public int indexMenu;

	public int columns = 6;

	public int rows;

	public int inforX;

	public int inforY;

	public int inforW;

	public int inforH;

	private int yPaint;

	private int xMap;

	private int yMap;

	private int xMapTask;

	private int yMapTask;

	private int xMove;

	private int yMove;

	public static bool isPaintMap = true;

	public bool isClose;

	private int infoSelect;

	public static MyVector vGameInfo = new MyVector(string.Empty);

	public static string[] contenInfo;

	public bool isViewChatServer;

	private int currInfoItem;

	public Char charInfo;

	private bool isChangeZone;

	private bool isKiguiXu;

	private bool isKiguiLuong;

	private int delayKigui;

	public sbyte combineSuccess = -1;

	public int idNPC;

	public int xS;

	public int yS;

	private int rS;

	private int angleS;

	private int angleO;

	private int iAngleS;

	private int iDotS;

	private int speed;

	private int[] xArgS;

	private int[] yArgS;

	private int[] xDotS;

	private int[] yDotS;

	private int time;

	private int typeCombine;

	private int countUpdate;

	private int countR;

	private int countWait;

	private bool isSpeedCombine;

	private bool isCompleteEffCombine = true;

	private bool isPaintCombine;

	public bool isDoneCombine = true;

	public short iconID1;

	public short iconID2;

	public short iconID3;

	public string[][] speacialTabName;

	public static int[] sizeUpgradeEff = new int[3] { 2, 1, 1 };

	public static int nsize = 1;

	public const sbyte COLOR_WHITE = 0;

	public const sbyte COLOR_GREEN = 1;

	public const sbyte COLOR_PURPLE = 2;

	public const sbyte COLOR_ORANGE = 3;

	public const sbyte COLOR_BLUE = 4;

	public const sbyte COLOR_YELLOW = 5;

	public const sbyte COLOR_RED = 6;

	public const sbyte COLOR_BLACK = 7;

	public static int[][] colorUpgradeEffect = new int[7][]
	{
		new int[6] { 16777215, 15000805, 13487823, 11711155, 9671828, 7895160 },
		new int[6] { 61952, 58624, 52224, 45824, 39168, 32768 },
		new int[6] { 13500671, 12058853, 10682572, 9371827, 7995545, 6684800 },
		new int[6] { 16744192, 15037184, 13395456, 11753728, 10046464, 8404992 },
		new int[6] { 37119, 33509, 28108, 24499, 21145, 17536 },
		new int[6] { 16776192, 15063040, 12635136, 11776256, 10063872, 8290304 },
		new int[6] { 16711680, 15007744, 13369344, 11730944, 10027008, 8388608 }
	};

	public const int color_item_white = 15987701;

	public const int color_item_green = 2786816;

	public const int color_item_purple = 7078041;

	public const int color_item_orange = 12537346;

	public const int color_item_blue = 1269146;

	public const int color_item_yellow = 13279744;

	public const int color_item_red = 11599872;

	public const int color_item_black = 2039326;

	private Image imgo_0;

	private Image imgo_1;

	private Image imgo_2;

	private Image imgo_3;

	public Panel()
	{
		init();
		cmdClose = new Command(string.Empty, this, 1003, null);
		cmdClose.img = GameCanvas.loadImage("/mainImage/myTexture2dbtX.png");
		cmdClose.cmdClosePanel = true;
	}

	public static void loadBg()
	{
		imgMap = GameCanvas.loadImage("/img/map" + TileMap.planetID + ".png");
		imgBantay = GameCanvas.loadImage("/mainImage/myTexture2dbantay.png");
		imgX = GameCanvas.loadImage("/mainImage/myTexture2dbtX.png");
		imgXu = GameCanvas.loadImage("/mainImage/myTexture2dimgMoney.png");
		imgLuong = GameCanvas.loadImage("/mainImage/myTexture2dimgDiamond.png");
		imgLuongKhoa = GameCanvas.loadImage("/mainImage/luongkhoa.png");
		imgUp = GameCanvas.loadImage("/mainImage/myTexture2dup.png");
		imgDown = GameCanvas.loadImage("/mainImage/myTexture2ddown.png");
		imgStar = GameCanvas.loadImage("/mainImage/star.png");
		imgMaxStar = GameCanvas.loadImage("/mainImage/starE.png");
		imgStar8 = GameCanvas.loadImage("/mainImage/star8.png");
		imgNew = GameCanvas.loadImage("/mainImage/new.png");
		imgTicket = GameCanvas.loadImage("/mainImage/ticket12.png");
	}

	public void init()
	{
		pX = GameCanvas.pxLast + cmxMap;
		pY = GameCanvas.pyLast + cmyMap;
		lastTabIndex = new int[tabName.Length];
		for (int i = 0; i < lastTabIndex.Length; i++)
		{
			lastTabIndex[i] = -1;
		}
	}

	public int getXMap()
	{
		for (int i = 0; i < mapId[TileMap.planetID].Length; i++)
		{
			if (TileMap.mapID == mapId[TileMap.planetID][i])
			{
				return mapX[TileMap.planetID][i];
			}
		}
		return -1;
	}

	public int getYMap()
	{
		for (int i = 0; i < mapId[TileMap.planetID].Length; i++)
		{
			if (TileMap.mapID == mapId[TileMap.planetID][i])
			{
				return mapY[TileMap.planetID][i];
			}
		}
		return -1;
	}

	public int getXMapTask()
	{
		if (Char.myCharz().taskMaint == null)
		{
			return -1;
		}
		for (int i = 0; i < mapId[TileMap.planetID].Length; i++)
		{
			if (GameScr.mapTasks[Char.myCharz().taskMaint.index] == mapId[TileMap.planetID][i])
			{
				return mapX[TileMap.planetID][i];
			}
		}
		return -1;
	}

	public int getYMapTask()
	{
		if (Char.myCharz().taskMaint == null)
		{
			return -1;
		}
		for (int i = 0; i < mapId[TileMap.planetID].Length; i++)
		{
			if (GameScr.mapTasks[Char.myCharz().taskMaint.index] == mapId[TileMap.planetID][i])
			{
				return mapY[TileMap.planetID][i];
			}
		}
		return -1;
	}

	private void setType(int position)
	{
		typeShop = -1;
		W = WIDTH_PANEL;
		H = GameCanvas.h;
		X = 0;
		Y = 0;
		ITEM_HEIGHT = 24;
		this.position = position;
		switch (position)
		{
		case 0:
			xScroll = 2;
			yScroll = 80;
			wScroll = W - 4;
			hScroll = H - 96;
			cmx = wScroll;
			cmtoX = 0;
			X = 0;
			break;
		case 1:
			wScroll = W - 4;
			xScroll = GameCanvas.w - wScroll;
			yScroll = 80;
			hScroll = H - 96;
			X = xScroll - 2;
			cmx = -(GameCanvas.w + W);
			cmtoX = GameCanvas.w - W;
			break;
		}
		TAB_W = W / 5 - 1;
		currentTabIndex = 0;
		currentTabName = tabName[type];
		if (currentTabName.Length < 5)
		{
			TAB_W += 5;
		}
		startTabPos = xScroll + wScroll / 2 - currentTabName.Length * TAB_W / 2;
		lastSelect = new int[currentTabName.Length];
		cmyLast = new int[currentTabName.Length];
		for (int i = 0; i < currentTabName.Length; i++)
		{
			lastSelect[i] = (GameCanvas.isTouch ? (-1) : 0);
		}
		if (lastTabIndex[type] != -1)
		{
			currentTabIndex = lastTabIndex[type];
		}
		if (currentTabIndex < 0)
		{
			currentTabIndex = 0;
		}
		if (currentTabIndex > currentTabName.Length - 1)
		{
			currentTabIndex = currentTabName.Length - 1;
		}
		scroll = null;
	}

	public void setTypeMapTrans()
	{
		type = 14;
		setType(0);
		setTabMapTrans();
		cmx = (cmtoX = 0);
	}

	public void setTypeInfomatioin()
	{
		type = 6;
		cmx = wScroll;
		cmtoX = 0;
	}

	public void setTypeMap()
	{
		if (!GameScr.gI().isMapFize() && isPaintMap)
		{
			if (Hint.isOnTask(2, 0))
			{
				Hint.isViewMap = true;
				GameScr.info1.addInfo(mResources.go_to_quest, 0);
			}
			type = 4;
			currentTabName = tabName[type];
			startTabPos = xScroll + wScroll / 2 - currentTabName.Length * TAB_W / 2;
			cmx = (cmtoX = 0);
			setTabMap();
		}
	}

	public void setTypeArchivement()
	{
		currentListLength = Char.myCharz().arrArchive.Length;
		setType(0);
		type = 9;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = 0);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	public void setTypeKiGuiOnly()
	{
		type = 17;
		setType(1);
		setTabKiGui();
		typeShop = 2;
		currentTabIndex = 0;
	}

	public void setTabKiGui()
	{
		ITEM_HEIGHT = 24;
		currentListLength = Char.myCharz().arrItemShop[4].Length;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	public void setTypeBodyOnly()
	{
		type = 7;
		setType(1);
		setTabInventory();
		currentTabIndex = 0;
	}

	public void addChatMessage(InfoItem info)
	{
		logChat.insertElementAt(info, 0);
		if (logChat.size() > 20)
		{
			logChat.removeElementAt(logChat.size() - 1);
		}
	}

	public void addPlayerMenu(Command pm)
	{
		vPlayerMenu.addElement(pm);
	}

	public void setTabPlayerMenu()
	{
		ITEM_HEIGHT = 24;
		currentListLength = vPlayerMenu.size();
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	public void setTypeFlag()
	{
		type = 18;
		setType(0);
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		setTabFlag();
	}

	public void setTabFlag()
	{
		currentListLength = vFlag.size();
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		if (selected > currentListLength - 1)
		{
			selected = currentListLength - 1;
		}
		cmx = (cmtoX = 0);
	}

	public void setTypePlayerMenu(Char c)
	{
		type = 10;
		setType(0);
		setTabPlayerMenu();
		charMenu = c;
	}

	public void setTypeFriend()
	{
		type = 11;
		setType(0);
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		setTabFriend();
	}

	public void setTypeEnemy()
	{
		type = 16;
		setType(0);
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		setTabEnemy();
	}

	public void setTypeTop(sbyte t)
	{
		type = 15;
		setType(0);
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		setTabTop();
		isThachDau = ((t != 0) ? true : false);
	}

	public void setTabTop()
	{
		currentListLength = vTop.size();
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		if (selected > currentListLength - 1)
		{
			selected = currentListLength - 1;
		}
		cmx = (cmtoX = 0);
	}

	public void setTabFriend()
	{
		currentListLength = vFriend.size();
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		if (selected > currentListLength - 1)
		{
			selected = currentListLength - 1;
		}
		cmx = (cmtoX = 0);
	}

	public void setTabEnemy()
	{
		currentListLength = vEnemy.size();
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		if (selected > currentListLength - 1)
		{
			selected = currentListLength - 1;
		}
		cmx = (cmtoX = 0);
	}

	public void setTypeMessage()
	{
		type = 8;
		setType(0);
		setTabMessage();
		currentTabIndex = 0;
	}

	public void setTypeLockInventory()
	{
		type = 8;
		setType(0);
		setTabMessage();
		currentTabIndex = 0;
	}

	public void setTypeShop(int typeShop)
	{
		type = 1;
		setType(0);
		setTabShop();
		currentTabIndex = 0;
		this.typeShop = typeShop;
	}

	public void setTypeBox()
	{
		type = 2;
		if (GameCanvas.w > 2 * WIDTH_PANEL)
		{
			boxTabName = new string[1][] { mResources.chestt };
		}
		else
		{
			boxTabName = new string[2][]
			{
				mResources.chestt,
				mResources.inventory
			};
		}
		tabName[2] = boxTabName;
		setType(0);
		if (currentTabIndex == 0)
		{
			setTabBox();
		}
		if (currentTabIndex == 1)
		{
			setTabInventory();
		}
		if (GameCanvas.w > 2 * WIDTH_PANEL)
		{
			GameCanvas.panel2 = new Panel();
			GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
			GameCanvas.panel2.setTypeBodyOnly();
			GameCanvas.panel2.show();
		}
	}

	public void setTypeCombine()
	{
		type = 12;
		if (GameCanvas.w > 2 * WIDTH_PANEL)
		{
			boxCombine = new string[1][] { mResources.combine };
		}
		else
		{
			boxCombine = new string[2][]
			{
				mResources.combine,
				mResources.inventory
			};
		}
		tabName[type] = boxCombine;
		setType(0);
		if (currentTabIndex == 0)
		{
			setTabCombine();
		}
		if (currentTabIndex == 1)
		{
			setTabInventory();
		}
		if (GameCanvas.w > 2 * WIDTH_PANEL)
		{
			GameCanvas.panel2 = new Panel();
			GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
			GameCanvas.panel2.setTypeBodyOnly();
			GameCanvas.panel2.show();
		}
		combineSuccess = -1;
		isDoneCombine = true;
	}

	public void setTabCombine()
	{
		currentListLength = vItemCombine.size() + 1;
		ITEM_HEIGHT = 24;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 9;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	public void setTypeAuto()
	{
		type = 22;
		setType(0);
		setTabAuto();
		cmx = (cmtoX = 0);
	}

	private void setTabAuto()
	{
		currentListLength = strAuto.Length;
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
	}

	public void setTypePetMain()
	{
		type = 21;
		if (GameCanvas.panel2 != null)
		{
			boxPet = mResources.petMainTab2;
		}
		else
		{
			boxPet = mResources.petMainTab;
		}
		tabName[21] = boxPet;
		if (Char.myCharz().cgender == 1)
		{
			strStatus = new string[6]
			{
				mResources.follow,
				mResources.defend,
				mResources.attack,
				mResources.gohome,
				mResources.fusion,
				mResources.fusionForever
			};
		}
		else
		{
			strStatus = new string[5]
			{
				mResources.follow,
				mResources.defend,
				mResources.attack,
				mResources.gohome,
				mResources.fusion
			};
		}
		setType(2);
		if (currentTabIndex == 0)
		{
			setTabPetInventory();
		}
		if (currentTabIndex == 1)
		{
			setTabPetStatus();
		}
		if (currentTabIndex == 2)
		{
			setTabInventory();
		}
	}

	public void setTypeMain()
	{
		type = 0;
		setType(0);
		if (currentTabIndex == 1)
		{
			setTabInventory();
		}
		if (currentTabIndex == 2)
		{
			setTabSkill();
		}
		if (currentTabIndex == 3)
		{
			if (mainTabName.Length == 4)
			{
				setTabTool();
			}
			else
			{
				setTabClans();
			}
		}
		if (currentTabIndex == 4)
		{
			setTabTool();
		}
	}

	public void setTypeZone()
	{
		type = 3;
		setType(0);
		setTabZone();
		cmx = (cmtoX = 0);
	}

	public void addItemDetail(Item item)
	{
		cp = new ChatPopup();
		string empty = string.Empty;
		string text = string.Empty;
		if (item.template.gender != Char.myCharz().cgender)
		{
			if (item.template.gender == 0)
			{
				text = text + "\n|7|1|" + mResources.from_earth;
			}
			else if (item.template.gender == 1)
			{
				text = text + "\n|7|1|" + mResources.from_namec;
			}
			else if (item.template.gender == 2)
			{
				text = text + "\n|7|1|" + mResources.from_sayda;
			}
		}
		string text2 = string.Empty;
		if (item.itemOption != null)
		{
			for (int i = 0; i < item.itemOption.Length; i++)
			{
				if (item.itemOption[i].optionTemplate.id == 72)
				{
					text2 = " [+" + item.itemOption[i].param + "]";
				}
			}
		}
		text = text + "|0|1|" + item.template.name + text2;
		if (item.itemOption != null)
		{
			for (int j = 0; j < item.itemOption.Length; j++)
			{
				if (item.itemOption[j].optionTemplate.name.StartsWith("$") ? true : false)
				{
					empty = item.itemOption[j].getOptiongColor();
					if (item.itemOption[j].param == 1)
					{
						text = text + "\n|1|1|" + empty;
					}
					if (item.itemOption[j].param == 0)
					{
						text = text + "\n|0|1|" + empty;
					}
					continue;
				}
				empty = item.itemOption[j].getOptionString();
				if (!empty.Equals(string.Empty) && item.itemOption[j].optionTemplate.id != 72)
				{
					if (item.itemOption[j].optionTemplate.id == 102)
					{
						cp.starSlot = (sbyte)item.itemOption[j].param;
						Res.outz("STAR SLOT= " + cp.starSlot);
					}
					else if (item.itemOption[j].optionTemplate.id == 107)
					{
						cp.maxStarSlot = (sbyte)item.itemOption[j].param;
						Res.outz("STAR SLOT= " + cp.maxStarSlot);
					}
					else
					{
						text = text + "\n|1|1|" + empty;
					}
				}
			}
		}
		if (currItem.template.strRequire > 1)
		{
			string text3 = mResources.pow_request + ": " + currItem.template.strRequire;
			if (currItem.template.strRequire > Char.myCharz().cPower)
			{
				text = text + "\n|3|1|" + text3;
				string text4 = text;
				text = text4 + "\n|3|1|" + mResources.your_pow + ": " + Char.myCharz().cPower;
			}
			else
			{
				text = text + "\n|6|1|" + text3;
			}
		}
		else
		{
			text += "\n|6|1|";
		}
		currItem.compare = getCompare(currItem);
		text += "\n--";
		text = text + "\n|6|" + item.template.description;
		if (!item.reason.Equals(string.Empty))
		{
			if (!item.template.description.Equals(string.Empty))
			{
				text += "\n--";
			}
			text = text + "\n|2|" + item.reason;
		}
		if (cp.maxStarSlot > 0)
		{
			text += "\n\n";
		}
		popUpDetailInit(cp, text);
		idIcon = item.template.iconID;
		partID = null;
		charInfo = null;
	}

	public void popUpDetailInit(ChatPopup cp, string chat)
	{
		cp.isClip = false;
		cp.sayWidth = 180;
		cp.cx = 3 + X - ((X != 0) ? (Res.abs(cp.sayWidth - W) + 8) : 0);
		cp.says = mFont.tahoma_7_red.splitFontArray(chat, cp.sayWidth - 10);
		cp.delay = 10000000;
		cp.c = null;
		cp.sayRun = 7;
		cp.ch = 15 - cp.sayRun + cp.says.Length * 12 + 10;
		if (cp.ch > GameCanvas.h - 80)
		{
			cp.ch = GameCanvas.h - 80;
			cp.lim = cp.says.Length * 12 - cp.ch + 17;
			if (cp.lim < 0)
			{
				cp.lim = 0;
			}
			ChatPopup.cmyText = 0;
			cp.isClip = true;
		}
		cp.cy = GameCanvas.menu.menuY - cp.ch;
		while (cp.cy < 10)
		{
			cp.cy++;
			GameCanvas.menu.menuY++;
		}
		cp.mH = 0;
		cp.strY = 10;
	}

	public void popUpDetailInitArray(ChatPopup cp, string[] chat)
	{
		cp.sayWidth = 160;
		cp.cx = 3 + X;
		cp.says = chat;
		cp.delay = 10000000;
		cp.c = null;
		cp.sayRun = 7;
		cp.ch = 15 - cp.sayRun + cp.says.Length * 12 + 10;
		cp.cy = GameCanvas.menu.menuY - cp.ch;
		cp.mH = 0;
		cp.strY = 10;
	}

	public void addMessageDetail(ClanMessage cm)
	{
		cp = new ChatPopup();
		string text = "|0|" + cm.playerName;
		text = text + "\n|1|" + Member.getRole(cm.role);
		for (int i = 0; i < myMember.size(); i++)
		{
			Member member = (Member)myMember.elementAt(i);
			if (cm.playerId == member.ID)
			{
				string text2 = text;
				text = text2 + "\n|5|" + mResources.clan_capsuledonate + ": " + member.clanPoint;
				text2 = text;
				text = text2 + "\n|5|" + mResources.clan_capsuleself + ": " + member.curClanPoint;
				text2 = text;
				text = text2 + "\n|4|" + mResources.give_pea + ": " + member.donate + mResources.time;
				text2 = text;
				text = text2 + "\n|4|" + mResources.receive_pea + ": " + member.receive_donate + mResources.time;
				partID = new int[3] { member.head, member.leg, member.body };
				break;
			}
		}
		text += "\n--";
		for (int j = 0; j < cm.chat.Length; j++)
		{
			text = text + "\n" + cm.chat[j];
		}
		if (cm.type == 1)
		{
			string text2 = text;
			text = text2 + "\n|6|" + mResources.received + " " + cm.recieve + "/" + cm.maxCap;
		}
		popUpDetailInit(cp, text);
		charInfo = null;
	}

	public void addThachDauDetail(TopInfo t)
	{
		string text = "|0|1|" + t.name;
		text = text + "\n|1|Top " + t.rank;
		text = text + "\n|1|" + t.info;
		text = text + "\n|2|" + t.info2;
		cp = new ChatPopup();
		popUpDetailInit(cp, text);
		partID = new int[3] { t.headID, t.leg, t.body };
		currItem = null;
		charInfo = null;
	}

	public void addClanMemberDetail(Member m)
	{
		string text = "|0|1|" + m.name;
		string text2 = "\n|2|1|";
		if (m.role == 0)
		{
			text2 = "\n|7|1|";
		}
		if (m.role == 1)
		{
			text2 = "\n|1|1|";
		}
		if (m.role == 2)
		{
			text2 = "\n|0|1|";
		}
		text = text + text2 + Member.getRole(m.role);
		string text3 = text;
		text = text3 + "\n|2|1|" + mResources.power + ": " + m.powerPoint;
		text += "\n--";
		text3 = text;
		text = text3 + "\n|5|" + mResources.clan_capsuledonate + ": " + m.clanPoint;
		text3 = text;
		text = text3 + "\n|5|" + mResources.clan_capsuleself + ": " + m.curClanPoint;
		text3 = text;
		text = text3 + "\n|4|" + mResources.give_pea + ": " + m.donate + mResources.time;
		text3 = text;
		text = text3 + "\n|4|" + mResources.receive_pea + ": " + m.receive_donate + mResources.time;
		text3 = text;
		text = text3 + "\n|6|" + mResources.join_date + ": " + m.joinTime;
		cp = new ChatPopup();
		popUpDetailInit(cp, text);
		partID = new int[3] { m.head, m.leg, m.body };
		currItem = null;
		charInfo = null;
	}

	public void addClanDetail(Clan cl)
	{
		string text = "|0|" + cl.name;
		string[] array = mFont.tahoma_7_green.splitFontArray(cl.slogan, wScroll - 60);
		for (int i = 0; i < array.Length; i++)
		{
			text = text + "\n|2|" + array[i];
		}
		text += "\n--";
		string text2 = text;
		text = text2 + "\n|7|" + mResources.clan_leader + ": " + cl.leaderName;
		text2 = text;
		text = text2 + "\n|1|" + mResources.power_point + ": " + cl.powerPoint;
		text2 = text;
		text = text2 + "\n|4|" + mResources.member + ": " + cl.currMember + "/" + cl.maxMember;
		text2 = text;
		text = text2 + "\n|4|" + mResources.level + ": " + cl.level;
		text2 = text;
		text = text2 + "\n|4|" + mResources.clan_birthday + ": " + NinjaUtil.getDate(cl.date);
		cp = new ChatPopup();
		popUpDetailInit(cp, text);
		idIcon = ClanImage.getClanImage((sbyte)cl.imgID).idImage[0];
		currItem = null;
	}

	public void addSkillDetail(SkillTemplate tp, Skill skill, Skill nextSkill)
	{
		string text = "|0|" + tp.name;
		for (int i = 0; i < tp.description.Length; i++)
		{
			text = text + "\n|4|" + tp.description[i];
		}
		text += "\n--";
		if (skill != null)
		{
			string text2 = text;
			text = text2 + "\n|2|" + mResources.cap_do + ": " + skill.point;
			text = text + "\n|5|" + NinjaUtil.replace(tp.damInfo, "#", skill.damage + string.Empty);
			text2 = text;
			text = text2 + "\n|5|" + mResources.KI_consume + skill.manaUse + ((tp.manaUseType != 1) ? string.Empty : "%");
			text2 = text;
			text = text2 + "\n|5|" + mResources.speed + ": " + skill.coolDown + mResources.milisecond;
			text += "\n--";
			if (skill.point == tp.maxPoint)
			{
				text = text + "\n|0|" + mResources.max_level_reach;
			}
			else
			{
				text2 = text;
				text = text2 + "\n|1|" + mResources.next_level_require + Res.formatNumber(nextSkill.powRequire) + " " + mResources.potential;
				text = text + "\n|4|" + NinjaUtil.replace(tp.damInfo, "#", nextSkill.damage + string.Empty);
			}
		}
		else
		{
			text = text + "\n|2|" + mResources.not_learn;
			string text2 = text;
			text = text2 + "\n|1|" + mResources.learn_require + Res.formatNumber(nextSkill.powRequire) + " " + mResources.potential;
			text = text + "\n|4|" + NinjaUtil.replace(tp.damInfo, "#", nextSkill.damage + string.Empty);
			text2 = text;
			text = text2 + "\n|4|" + mResources.KI_consume + nextSkill.manaUse + ((tp.manaUseType != 1) ? string.Empty : "%");
			text2 = text;
			text = text2 + "\n|4|" + mResources.speed + ": " + nextSkill.coolDown + mResources.milisecond;
		}
		currItem = null;
		partID = null;
		charInfo = null;
		cp = new ChatPopup();
		popUpDetailInit(cp, text);
		idIcon = 0;
	}

	public void show()
	{
		if (GameCanvas.isTouch)
		{
			cmdClose.x = 156;
			cmdClose.y = 3;
		}
		else
		{
			cmdClose.x = GameCanvas.w - 19;
			cmdClose.y = GameCanvas.h - 19;
		}
		cmdClose.isPlaySoundButton = false;
		ChatPopup.currChatPopup = null;
		InfoDlg.hide();
		timeShow = 20;
		isShow = true;
		isClose = false;
		SoundMn.gI().panelOpen();
		if (isTypeShop())
		{
			Char.myCharz().setPartOld();
		}
	}

	public void chatTFUpdateKey()
	{
		if (chatTField != null && chatTField.isShow)
		{
			if (chatTField.left != null && (GameCanvas.keyPressed[12] || mScreen.getCmdPointerLast(chatTField.left)) && chatTField.left != null)
			{
				chatTField.left.performAction();
			}
			if (chatTField.right != null && (GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(chatTField.right)) && chatTField.right != null)
			{
				chatTField.right.performAction();
			}
			if (chatTField.center != null && (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(chatTField.center)) && chatTField.center != null)
			{
				chatTField.center.performAction();
			}
			if (chatTField.isShow && GameCanvas.keyAsciiPress != 0)
			{
				chatTField.keyPressed(GameCanvas.keyAsciiPress);
				GameCanvas.keyAsciiPress = 0;
			}
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
		}
	}

	public void updateKey()
	{
		if ((chatTField != null && chatTField.isShow) || !GameCanvas.panel.isDoneCombine || InfoDlg.isShow)
		{
			return;
		}
		if (tabIcon != null && tabIcon.isShow)
		{
			tabIcon.updateKey();
		}
		else
		{
			if (isClose || !isShow)
			{
				return;
			}
			if (cmdClose.isPointerPressInside())
			{
				cmdClose.performAction();
				return;
			}
			if (GameCanvas.keyPressed[13])
			{
				if (type != 4)
				{
					hide();
					return;
				}
				setTypeMain();
				cmx = (cmtoX = 0);
			}
			if (GameCanvas.keyPressed[12] || GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
			{
				if (left.idAction > 0)
				{
					perform(left.idAction, left.p);
				}
				else
				{
					waitToPerform = 2;
				}
			}
			if (Equals(GameCanvas.panel) && GameCanvas.panel2 == null && GameCanvas.isPointerJustRelease && !GameCanvas.isPointer(X, Y, W, H) && !pointerIsDowning)
			{
				hide();
				return;
			}
			if (!isClanOption)
			{
				updateKeyInTabBar();
			}
			switch (type)
			{
			case 23:
			case 24:
				updateKeyScrollView();
				break;
			case 21:
				if (currentTabIndex == 0)
				{
					updateKeyScrollView();
				}
				if (currentTabIndex == 1)
				{
					updateKeyPetStatus();
				}
				if (currentTabIndex == 2)
				{
					updateKeyScrollView();
				}
				break;
			case 0:
				if (currentTabIndex == 0)
				{
					updateKeyQuest();
					GameCanvas.clearKeyPressed();
					return;
				}
				if (currentTabIndex == 1)
				{
					updateKeyScrollView();
				}
				if (currentTabIndex == 2)
				{
					updateKeySkill();
				}
				if (currentTabIndex == 3)
				{
					if (mainTabName.Length == 4)
					{
						updateKeyTool();
					}
					else
					{
						updateKeyClans();
					}
				}
				if (currentTabIndex == 4)
				{
					updateKeyTool();
				}
				break;
			case 2:
				updateKeyScrollView();
				break;
			case 3:
				updateKeyScrollView();
				break;
			case 14:
				updateKeyScrollView();
				break;
			case 1:
			case 17:
			case 25:
				updateKeyScrollView();
				break;
			case 4:
				updateKeyMap();
				GameCanvas.clearKeyPressed();
				return;
			case 7:
				updateKeyScrollView();
				break;
			case 8:
				updateKeyScrollView();
				break;
			case 9:
				updateKeyScrollView();
				break;
			case 10:
				updateKeyScrollView();
				break;
			case 11:
			case 16:
				updateKeyScrollView();
				break;
			case 15:
				updateKeyScrollView();
				break;
			case 12:
				updateKeyCombine();
				break;
			case 13:
				updateKeyGiaoDich();
				break;
			case 18:
				updateKeyScrollView();
				break;
			case 19:
				updateKeyOption();
				break;
			case 20:
				updateKeyOption();
				break;
			case 22:
				updateKeyAuto();
				break;
			}
			GameCanvas.clearKeyHold();
			for (int i = 0; i < GameCanvas.keyPressed.Length; i++)
			{
				GameCanvas.keyPressed[i] = false;
			}
		}
	}

	private void updateKeyAuto()
	{
	}

	private void updateKeyPetStatus()
	{
		updateKeyScrollView();
	}

	private void updateKeyPetSkill()
	{
	}

	private void keyGiaodich()
	{
		updateKeyScrollView();
	}

	private void updateKeyGiaoDich()
	{
		if (currentTabIndex == 0)
		{
			if (Equals(GameCanvas.panel))
			{
				updateKeyScrollView();
			}
			if (Equals(GameCanvas.panel2))
			{
				keyGiaodich();
			}
		}
		if (currentTabIndex == 1 || currentTabIndex == 2)
		{
			keyGiaodich();
		}
	}

	private void updateKeyTool()
	{
		updateKeyScrollView();
	}

	private void updateKeySkill()
	{
		updateKeyScrollView();
	}

	private void updateKeyClanIcon()
	{
		updateKeyScrollView();
	}

	public void setTabGiaoDich(bool isMe)
	{
		currentListLength = ((!isMe) ? (vFriendGD.size() + 3) : (vMyGD.size() + 3));
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
	}

	public void setTypeGiaoDich(Char cGD)
	{
		type = 13;
		tabName[type] = boxGD;
		isAccept = false;
		isLock = false;
		isFriendLock = false;
		vMyGD.removeAllElements();
		vFriendGD.removeAllElements();
		moneyGD = 0;
		friendMoneyGD = 0;
		if (GameCanvas.w > 2 * WIDTH_PANEL)
		{
			GameCanvas.panel2 = new Panel();
			GameCanvas.panel2.type = 13;
			GameCanvas.panel2.tabName[type] = new string[1][] { mResources.item_receive };
			GameCanvas.panel2.setType(1);
			GameCanvas.panel2.setTabGiaoDich(isMe: false);
			GameCanvas.panel.tabName[type] = new string[2][]
			{
				mResources.inventory,
				mResources.item_give
			};
			GameCanvas.panel2.show();
			GameCanvas.panel2.charMenu = cGD;
		}
		if (Equals(GameCanvas.panel))
		{
			setType(0);
		}
		if (currentTabIndex == 0)
		{
			setTabInventory();
		}
		if (currentTabIndex == 1)
		{
			setTabGiaoDich(isMe: true);
		}
		if (currentTabIndex == 2)
		{
			setTabGiaoDich(isMe: false);
		}
		charMenu = cGD;
	}

	private void paintGiaoDich(mGraphics g, bool isMe)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		MyVector myVector = ((!isMe) ? vFriendGD : vMyGD);
		for (int i = 0; i < currentListLength; i++)
		{
			int num = xScroll + 36;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = wScroll - 36;
			int num4 = ITEM_HEIGHT - 1;
			int num5 = xScroll;
			int num6 = yScroll + i * ITEM_HEIGHT;
			int num7 = 34;
			int num8 = ITEM_HEIGHT - 1;
			if (num2 - cmy > yScroll + hScroll || num2 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			if (i == currentListLength - 1)
			{
				if (!isMe)
				{
					continue;
				}
				g.setColor(15196114);
				g.fillRect(num5, num2, wScroll, num4);
				if (!isLock)
				{
					if (!isFriendLock)
					{
						mFont.tahoma_7_grey.drawString(g, mResources.opponent + mResources.not_lock_trade, xScroll + wScroll / 2, num2 + num4 / 2 - 4, mFont.CENTER);
					}
					else
					{
						mFont.tahoma_7_grey.drawString(g, mResources.opponent + mResources.locked_trade, xScroll + wScroll / 2, num2 + num4 / 2 - 4, mFont.CENTER);
					}
				}
				else if (isFriendLock)
				{
					g.setColor(15196114);
					g.fillRect(num5, num2, wScroll, num4);
					g.drawImage((i != selected) ? GameScr.imgLbtn2 : GameScr.imgLbtnFocus2, xScroll + wScroll - 5, num2 + 2, StaticObj.TOP_RIGHT);
					((i != selected) ? mFont.tahoma_7b_dark : mFont.tahoma_7b_green2).drawString(g, mResources.done, xScroll + wScroll - 22, num2 + 7, 2);
					mFont.tahoma_7_grey.drawString(g, mResources.opponent + mResources.locked_trade, xScroll + 5, num2 + num4 / 2 - 4, mFont.LEFT);
				}
				else
				{
					mFont.tahoma_7_grey.drawString(g, mResources.opponent + mResources.not_lock_trade, xScroll + wScroll / 2, num2 + num4 / 2 - 4, mFont.CENTER);
				}
				continue;
			}
			if (i == currentListLength - 2)
			{
				if (isMe)
				{
					g.setColor(15196114);
					g.fillRect(num5, num2, wScroll, num4);
					if (!isAccept)
					{
						if (!isLock)
						{
							g.drawImage((i != selected) ? GameScr.imgLbtn2 : GameScr.imgLbtnFocus2, xScroll + wScroll - 5, num2 + 2, StaticObj.TOP_RIGHT);
							((i != selected) ? mFont.tahoma_7b_dark : mFont.tahoma_7b_green2).drawString(g, mResources.mlock, xScroll + wScroll - 22, num2 + 7, 2);
							mFont.tahoma_7_grey.drawString(g, mResources.you + mResources.not_lock_trade, xScroll + 5, num2 + num4 / 2 - 4, mFont.LEFT);
						}
						else
						{
							g.drawImage((i != selected) ? GameScr.imgLbtn2 : GameScr.imgLbtnFocus2, xScroll + wScroll - 5, num2 + 2, StaticObj.TOP_RIGHT);
							((i != selected) ? mFont.tahoma_7b_dark : mFont.tahoma_7b_green2).drawString(g, mResources.CANCEL, xScroll + wScroll - 22, num2 + 7, 2);
							mFont.tahoma_7_grey.drawString(g, mResources.you + mResources.locked_trade, xScroll + 5, num2 + num4 / 2 - 4, mFont.LEFT);
						}
					}
				}
				else if (!isFriendLock)
				{
					mFont.tahoma_7b_dark.drawString(g, mResources.not_lock_trade_upper, xScroll + wScroll / 2, num2 + num4 / 2 - 4, mFont.CENTER);
				}
				else
				{
					mFont.tahoma_7b_dark.drawString(g, mResources.locked_trade_upper, xScroll + wScroll / 2, num2 + num4 / 2 - 4, mFont.CENTER);
				}
				continue;
			}
			if (i == currentListLength - 3)
			{
				if (isLock)
				{
					g.setColor(13748667);
				}
				else
				{
					g.setColor((i != selected) ? 15196114 : 16383818);
				}
				g.fillRect(num, num2, num3, num4);
				if (isLock)
				{
					g.setColor(13748667);
				}
				else
				{
					g.setColor((i != selected) ? 9993045 : 7300181);
				}
				g.fillRect(num5, num6, num7, num8);
				g.drawImage(imgXu, num5 + num7 / 2, num6 + num8 / 2, 3);
				mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys((!isMe) ? friendMoneyGD : moneyGD) + " " + mResources.XU, num + 5, num2 + 11, 0);
				mFont.tahoma_7_green.drawString(g, mResources.money_trade, num + 5, num2, 0);
				continue;
			}
			if (myVector.size() == 0)
			{
				return;
			}
			if (isLock)
			{
				g.setColor(13748667);
			}
			else
			{
				g.setColor((i != selected) ? 15196114 : 16383818);
			}
			g.fillRect(num, num2, num3, num4);
			if (isLock)
			{
				g.setColor(13748667);
			}
			else
			{
				g.setColor((i != selected) ? 9993045 : 9541120);
			}
			Item item = (Item)myVector.elementAt(i);
			if (item != null)
			{
				for (int j = 0; j < item.itemOption.Length; j++)
				{
					if (item.itemOption[j].optionTemplate.id != 72 || item.itemOption[j].param <= 0)
					{
						continue;
					}
					sbyte color_Item_Upgrade = GetColor_Item_Upgrade(item.itemOption[j].param);
					int color_ItemBg = GetColor_ItemBg(color_Item_Upgrade);
					if (color_ItemBg != -1)
					{
						if (isLock)
						{
							g.setColor(13748667);
						}
						else
						{
							g.setColor((i != selected) ? GetColor_ItemBg(color_Item_Upgrade) : GetColor_ItemBg(color_Item_Upgrade));
						}
					}
				}
			}
			g.fillRect(num5, num6, num7, num8);
			if (item == null)
			{
				continue;
			}
			string text = string.Empty;
			if (item.itemOption != null)
			{
				for (int k = 0; k < item.itemOption.Length; k++)
				{
					if (item.itemOption[k].optionTemplate.id == 72)
					{
						text = " [+" + item.itemOption[k].param + "]";
					}
				}
			}
			mFont.tahoma_7_green2.drawString(g, item.template.name + text, num + 5, num2 + 1, 0);
			string text2 = string.Empty;
			if (item.itemOption != null)
			{
				if (item.itemOption.Length > 0 && item.itemOption[0] != null)
				{
					text2 += item.itemOption[0].getOptionString();
				}
				mFont mFont2 = mFont.tahoma_7_blue;
				if (item.compare < 0 && item.template.type != 5)
				{
					mFont2 = mFont.tahoma_7_red;
				}
				if (item.itemOption.Length > 1)
				{
					for (int l = 1; l < item.itemOption.Length; l++)
					{
						if (item.itemOption[l] != null && item.itemOption[l].optionTemplate.id != 102 && item.itemOption[l].optionTemplate.id != 107)
						{
							text2 = text2 + "," + item.itemOption[l].getOptionString();
						}
					}
				}
				mFont2.drawString(g, text2, num + 5, num2 + 11, mFont.LEFT);
			}
			SmallImage.drawSmallImage(g, item.template.iconID, num5 + num7 / 2 - ((item.quantity > 1) ? 8 : 0), num6 + num8 / 2, 0, 3);
			if (item.itemOption != null)
			{
				for (int m = 0; m < item.itemOption.Length; m++)
				{
					paintOptItem(g, item.itemOption[m].optionTemplate.id, item.itemOption[m].param, num5, num6, num7, num8);
				}
				for (int n = 0; n < item.itemOption.Length; n++)
				{
					paintOptSlotItem(g, item.itemOption[n].optionTemplate.id, item.itemOption[n].param, num5, num6, num7, num8);
				}
			}
			if (item.quantity > 1)
			{
				mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num5 + num7 - 15, num6 + 6, 0);
			}
		}
		paintScrollArrow(g);
	}

	private void updateKeyMap()
	{
		if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21])
		{
			yMove -= 5;
			cmyMap = yMove - (yScroll + hScroll / 2);
			if (yMove < yScroll)
			{
				yMove = yScroll;
			}
		}
		if (GameCanvas.keyHold[(!Main.isPC) ? 8 : 22])
		{
			yMove += 5;
			cmyMap = yMove - (yScroll + hScroll / 2);
			if (yMove > yScroll + 200)
			{
				yMove = yScroll + 200;
			}
		}
		if (GameCanvas.keyHold[(!Main.isPC) ? 4 : 23])
		{
			xMove -= 5;
			cmxMap = xMove - wScroll / 2;
			if (xMove < 16)
			{
				xMove = 16;
			}
		}
		if (GameCanvas.keyHold[(!Main.isPC) ? 6 : 24])
		{
			xMove += 5;
			cmxMap = xMove - wScroll / 2;
			if (xMove > 250)
			{
				xMove = 250;
			}
		}
		if (GameCanvas.isPointerDown)
		{
			pointerIsDowning = true;
			if (!trans)
			{
				pa1 = cmxMap;
				pa2 = cmyMap;
				trans = true;
			}
			cmxMap = pa1 + (GameCanvas.pxLast - GameCanvas.px);
			cmyMap = pa2 + (GameCanvas.pyLast - GameCanvas.py);
		}
		if (GameCanvas.isPointerJustRelease)
		{
			trans = false;
			GameCanvas.pxLast = GameCanvas.px;
			GameCanvas.pyLast = GameCanvas.py;
			pX = GameCanvas.pxLast + cmxMap;
			pY = GameCanvas.pyLast + cmyMap;
		}
		if (GameCanvas.isPointerClick)
		{
			pointerIsDowning = false;
		}
		if (cmxMap < 0)
		{
			cmxMap = 0;
		}
		if (cmxMap > cmxMapLim)
		{
			cmxMap = cmxMapLim;
		}
		if (cmyMap < 0)
		{
			cmyMap = 0;
		}
		if (cmyMap > cmyMapLim)
		{
			cmyMap = cmyMapLim;
		}
	}

	private void updateKeyCombine()
	{
		if (currentTabIndex == 0)
		{
			updateKeyScrollView();
			keyTouchCombine = -1;
			if (selected == vItemCombine.size() && GameCanvas.isPointerClick)
			{
				GameCanvas.isPointerClick = false;
				keyTouchCombine = 1;
			}
		}
		if (currentTabIndex == 1)
		{
			updateKeyScrollView();
		}
	}

	private void updateKeyQuest()
	{
		if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21])
		{
			cmyQuest -= 5;
		}
		if (GameCanvas.keyHold[(!Main.isPC) ? 8 : 22])
		{
			cmyQuest += 5;
		}
		if (cmyQuest < 0)
		{
			cmyQuest = 0;
		}
		int num = indexRowMax * 12 - (hScroll - 60);
		if (num < 0)
		{
			num = 0;
		}
		if (cmyQuest > num)
		{
			cmyQuest = num;
		}
		if (scroll != null)
		{
			if (!GameCanvas.isTouch)
			{
				scroll.cmy = cmyQuest;
			}
			scroll.updateKey();
		}
		int num2 = xScroll + wScroll / 2 - 35;
		int num3 = ((GameCanvas.h <= 300) ? 15 : 20);
		int num4 = yScroll + hScroll - num3 - 15;
		int px = GameCanvas.px;
		int py = GameCanvas.py;
		keyTouchMapButton = -1;
		if (isPaintMap && !GameScr.gI().isMapDocNhan() && px >= num2 && px <= num2 + 70 && py >= num4 && py <= num4 + 30 && (scroll == null || !scroll.pointerIsDowning))
		{
			keyTouchMapButton = 1;
			if (GameCanvas.isPointerJustRelease)
			{
				SoundMn.gI().buttonClick();
				waitToPerform = 2;
				GameCanvas.clearAllPointerEvent();
			}
		}
	}

	private void getCurrClanOtion()
	{
		isClanOption = false;
		if (type != 0 || mainTabName.Length != 5 || currentTabIndex != 3)
		{
			return;
		}
		isClanOption = false;
		if (selected == 0)
		{
			currClanOption = new int[clansOption.Length];
			for (int i = 0; i < currClanOption.Length; i++)
			{
				currClanOption[i] = i;
			}
			if (!isViewMember)
			{
				isClanOption = true;
			}
		}
		else if (selected != 1 && !isSearchClan && selected > 0)
		{
			currClanOption = new int[1];
			for (int j = 0; j < currClanOption.Length; j++)
			{
				currClanOption[j] = j;
			}
			isClanOption = true;
		}
	}

	private void updateKeyClansOption()
	{
		if (currClanOption == null)
		{
			return;
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
		{
			currMess = getCurrMessage();
			cSelected--;
			if (selected == 0 && cSelected < 0)
			{
				cSelected = currClanOption.Length - 1;
			}
			if (selected > 1 && isMessage && currMess.option != null && cSelected < 0)
			{
				cSelected = currMess.option.Length - 1;
			}
		}
		else if (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
		{
			currMess = getCurrMessage();
			cSelected++;
			if (selected == 0 && cSelected > currClanOption.Length - 1)
			{
				cSelected = 0;
			}
			if (selected > 1 && isMessage && currMess.option != null && cSelected > currMess.option.Length - 1)
			{
				cSelected = 0;
			}
		}
	}

	private void updateKeyClans()
	{
		updateKeyScrollView();
		updateKeyClansOption();
	}

	private void checkOptionSelect()
	{
		if (type != 0 || currentTabIndex != 3 || mainTabName.Length != 5 || selected == -1)
		{
			return;
		}
		int num = 0;
		if (selected == 0)
		{
			num = xScroll + wScroll / 2 - clansOption.Length * TAB_W / 2;
			cSelected = (GameCanvas.px - num) / TAB_W;
		}
		else
		{
			currMess = getCurrMessage();
			if (currMess != null && currMess.option != null)
			{
				num = xScroll + wScroll - 2 - currMess.option.Length * 40;
				cSelected = (GameCanvas.px - num) / 40;
			}
		}
		if (GameCanvas.px < num)
		{
			cSelected = -1;
		}
	}

	public void updateScroolMouse(int a)
	{
		bool flag = false;
		if (GameCanvas.pxMouse > wScroll)
		{
			return;
		}
		if (indexMouse == -1)
		{
			indexMouse = selected;
		}
		if (a > 0)
		{
			indexMouse -= a;
			flag = true;
		}
		else if (a < 0)
		{
			indexMouse += -a;
			flag = true;
		}
		if (indexMouse < 0)
		{
			indexMouse = 0;
		}
		if (flag)
		{
			cmtoY = indexMouse * 12;
			if (cmtoY > cmyLim)
			{
				cmtoY = cmyLim;
			}
			if (cmtoY < 0)
			{
				cmtoY = 0;
			}
		}
	}

	private void updateKeyScrollView()
	{
		if (currentListLength <= 0)
		{
			return;
		}
		bool flag = false;
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
		{
			flag = true;
			selected--;
			if (type == 24)
			{
				selected -= 2;
				if (selected < 0)
				{
					selected = 0;
				}
			}
			else if (selected < 0)
			{
				if (Equals(GameCanvas.panel) && typeShop == 2 && currentTabIndex <= 3 && maxPageShop[currentTabIndex] > 1)
				{
					InfoDlg.showWait();
					if (currPageShop[currentTabIndex] <= 0)
					{
						Service.gI().kigui(4, -1, (sbyte)currentTabIndex, maxPageShop[currentTabIndex] - 1, -1);
					}
					else
					{
						Service.gI().kigui(4, -1, (sbyte)currentTabIndex, currPageShop[currentTabIndex] - 1, -1);
					}
					return;
				}
				selected = currentListLength - 1;
				if (isClanOption)
				{
					selected = -1;
				}
			}
			lastSelect[currentTabIndex] = selected;
			cSelected = 0;
			getCurrClanOtion();
		}
		else if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
		{
			flag = true;
			selected++;
			if (type == 24)
			{
				selected += 2;
				if (selected > currentListLength - 1)
				{
					selected = currentListLength - 1;
				}
			}
			else if (selected > currentListLength - 1)
			{
				if (Equals(GameCanvas.panel) && typeShop == 2 && currentTabIndex <= 3 && maxPageShop[currentTabIndex] > 1)
				{
					InfoDlg.showWait();
					if (currPageShop[currentTabIndex] >= maxPageShop[currentTabIndex] - 1)
					{
						Service.gI().kigui(4, -1, (sbyte)currentTabIndex, 0, -1);
					}
					else
					{
						Service.gI().kigui(4, -1, (sbyte)currentTabIndex, currPageShop[currentTabIndex] + 1, -1);
					}
					return;
				}
				selected = 0;
			}
			lastSelect[currentTabIndex] = selected;
			cSelected = 0;
			getCurrClanOtion();
		}
		if (flag)
		{
			cmtoY = selected * ITEM_HEIGHT - hScroll / 2;
			if (cmtoY > cmyLim)
			{
				cmtoY = cmyLim;
			}
			if (cmtoY < 0)
			{
				cmtoY = 0;
			}
			if (selected == currentListLength || selected == 0)
			{
				cmy = cmtoY;
			}
		}
		if (GameCanvas.isPointerDown)
		{
			justRelease = false;
			if (!pointerIsDowning && GameCanvas.isPointer(xScroll, yScroll, wScroll, hScroll))
			{
				for (int i = 0; i < pointerDownLastX.Length; i++)
				{
					pointerDownLastX[0] = GameCanvas.py;
				}
				pointerDownFirstX = GameCanvas.py;
				pointerIsDowning = true;
				isDownWhenRunning = cmRun != 0;
				cmRun = 0;
			}
			else if (pointerIsDowning)
			{
				pointerDownTime++;
				if (pointerDownTime > 5 && pointerDownFirstX == GameCanvas.py && !isDownWhenRunning)
				{
					pointerDownFirstX = -1000;
					selected = (cmtoY + GameCanvas.py - yScroll) / ITEM_HEIGHT;
					if (selected >= currentListLength)
					{
						selected = -1;
					}
					checkOptionSelect();
				}
				else
				{
					indexMouse = -1;
				}
				int num = GameCanvas.py - pointerDownLastX[0];
				if (num != 0 && selected != -1)
				{
					selected = -1;
					cSelected = -1;
				}
				for (int num2 = pointerDownLastX.Length - 1; num2 > 0; num2--)
				{
					pointerDownLastX[num2] = pointerDownLastX[num2 - 1];
				}
				pointerDownLastX[0] = GameCanvas.py;
				cmtoY -= num;
				if (cmtoY < 0)
				{
					cmtoY = 0;
				}
				if (cmtoY > cmyLim)
				{
					cmtoY = cmyLim;
				}
				if (cmy < 0 || cmy > cmyLim)
				{
					num /= 2;
				}
				cmy -= num;
				if (cmy < -(GameCanvas.h / 3))
				{
					wantUpdateList = true;
				}
				else
				{
					wantUpdateList = false;
				}
			}
		}
		if (!GameCanvas.isPointerJustRelease || !pointerIsDowning)
		{
			return;
		}
		justRelease = true;
		int i2 = GameCanvas.py - pointerDownLastX[0];
		GameCanvas.isPointerJustRelease = false;
		if (Res.abs(i2) < 20 && Res.abs(GameCanvas.py - pointerDownFirstX) < 20 && !isDownWhenRunning)
		{
			cmRun = 0;
			cmtoY = cmy;
			pointerDownFirstX = -1000;
			selected = (cmtoY + GameCanvas.py - yScroll) / ITEM_HEIGHT;
			if (selected >= currentListLength)
			{
				selected = -1;
			}
			checkOptionSelect();
			pointerDownTime = 0;
			waitToPerform = 10;
			SoundMn.gI().panelClick();
		}
		else if (selected != -1 && pointerDownTime > 5)
		{
			pointerDownTime = 0;
			waitToPerform = 1;
		}
		else if (selected == -1 && !isDownWhenRunning)
		{
			if (cmy < 0)
			{
				cmtoY = 0;
			}
			else if (cmy > cmyLim)
			{
				cmtoY = cmyLim;
			}
			else
			{
				int num3 = GameCanvas.py - pointerDownLastX[0] + (pointerDownLastX[0] - pointerDownLastX[1]) + (pointerDownLastX[1] - pointerDownLastX[2]);
				num3 = ((num3 > 10) ? 10 : ((num3 < -10) ? (-10) : 0));
				cmRun = -num3 * 100;
			}
		}
		pointerIsDowning = false;
		pointerDownTime = 0;
		GameCanvas.isPointerJustRelease = false;
	}

	public string subArray(string[] str)
	{
		return null;
	}

	private void updateKeyInTabBar()
	{
		if ((scroll != null && scroll.pointerIsDowning) || pointerIsDowning)
		{
			return;
		}
		int num = currentTabIndex;
		if (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
		{
			currentTabIndex++;
			if (currentTabIndex >= currentTabName.Length)
			{
				if (GameCanvas.panel2 != null)
				{
					currentTabIndex = currentTabName.Length - 1;
					GameCanvas.isFocusPanel2 = true;
				}
				else
				{
					currentTabIndex = 0;
				}
			}
			selected = lastSelect[currentTabIndex];
			lastTabIndex[type] = currentTabIndex;
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
		{
			currentTabIndex--;
			if (currentTabIndex < 0)
			{
				currentTabIndex = currentTabName.Length - 1;
			}
			if (GameCanvas.isFocusPanel2)
			{
				GameCanvas.isFocusPanel2 = false;
			}
			selected = lastSelect[currentTabIndex];
			lastTabIndex[type] = currentTabIndex;
		}
		keyTouchTab = -1;
		for (int i = 0; i < currentTabName.Length; i++)
		{
			if (!GameCanvas.isPointer(startTabPos + i * TAB_W, 52, TAB_W - 1, 25))
			{
				continue;
			}
			keyTouchTab = i;
			if (GameCanvas.isPointerJustRelease)
			{
				currentTabIndex = i;
				lastTabIndex[type] = i;
				GameCanvas.isPointerJustRelease = false;
				selected = lastSelect[currentTabIndex];
				if (num == currentTabIndex && cmRun == 0)
				{
					cmtoY = 0;
					selected = (GameCanvas.isTouch ? (-1) : 0);
				}
				break;
			}
		}
		if (num == currentTabIndex)
		{
			return;
		}
		SoundMn.gI().panelClick();
		switch (type)
		{
		case 21:
			if (currentTabIndex == 0)
			{
				setTabPetInventory();
			}
			if (currentTabIndex == 1)
			{
				setTabPetStatus();
			}
			if (currentTabIndex == 2)
			{
				setTabInventory();
			}
			break;
		case 0:
			if (currentTabIndex == 0)
			{
				setTabTask();
			}
			if (currentTabIndex == 1)
			{
				setTabInventory();
			}
			if (currentTabIndex == 2)
			{
				setTabSkill();
			}
			if (currentTabIndex == 3)
			{
				if (mainTabName.Length > 4)
				{
					setTabClans();
				}
				else
				{
					setTabTool();
				}
			}
			if (currentTabIndex == 4)
			{
				setTabTool();
			}
			break;
		case 2:
			if (currentTabIndex == 0)
			{
				setTabBox();
			}
			if (currentTabIndex == 1)
			{
				setTabInventory();
			}
			break;
		case 3:
			setTabZone();
			break;
		case 1:
			setTabShop();
			break;
		case 25:
			setTabSpeacialSkill();
			break;
		case 12:
			if (currentTabIndex == 0)
			{
				setTabCombine();
			}
			if (currentTabIndex == 1)
			{
				setTabInventory();
			}
			break;
		case 13:
			if (currentTabIndex == 0)
			{
				if (Equals(GameCanvas.panel))
				{
					setTabInventory();
				}
				else if (Equals(GameCanvas.panel2))
				{
					setTabGiaoDich(isMe: false);
				}
			}
			if (currentTabIndex == 1)
			{
				setTabGiaoDich(isMe: true);
			}
			if (currentTabIndex == 2)
			{
				setTabGiaoDich(isMe: false);
			}
			break;
		}
		selected = lastSelect[currentTabIndex];
	}

	private void setTabPetStatus()
	{
		currentListLength = strStatus.Length;
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
	}

	private void setTabPetSkill()
	{
	}

	private void setTabTool()
	{
		SoundMn.gI().getSoundOption();
		currentListLength = strTool.Length;
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
	}

	public void initTabClans()
	{
		if (isSearchClan)
		{
			currentListLength = ((clans != null) ? (clans.Length + 2) : 2);
			clanInfo = mResources.clan_list;
		}
		else if (isViewMember)
		{
			clanReport = string.Empty;
			currentListLength = ((member != null) ? member.size() : myMember.size()) + 2;
			clanInfo = mResources.member + " " + ((currClan == null) ? Char.myCharz().clan.name : currClan.name);
		}
		else if (isMessage)
		{
			currentListLength = ClanMessage.vMessage.size() + 2;
			clanInfo = mResources.msg;
			clanReport = string.Empty;
		}
		if (Char.myCharz().clan == null)
		{
			clansOption = new string[2][]
			{
				mResources.findClan,
				mResources.createClan
			};
		}
		else if (!isViewMember)
		{
			if (myMember.size() > 1)
			{
				clansOption = new string[3][]
				{
					mResources.chatClan,
					mResources.request_pea2,
					mResources.memberr
				};
			}
			else
			{
				clansOption = new string[1][] { mResources.memberr };
			}
		}
		else if (Char.myCharz().role > 0)
		{
			clansOption = new string[2][]
			{
				mResources.msgg,
				mResources.leaveClan
			};
		}
		else if (myMember.size() > 1)
		{
			clansOption = new string[4][]
			{
				mResources.msgg,
				mResources.leaveClan,
				mResources.khau_hieuu,
				mResources.bieu_tuongg
			};
		}
		else
		{
			clansOption = new string[3][]
			{
				mResources.msgg,
				mResources.khau_hieuu,
				mResources.bieu_tuongg
			};
		}
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
	}

	public void setTabClans()
	{
		GameScr.isNewClanMessage = false;
		ITEM_HEIGHT = 24;
		if (lastSelect != null && lastSelect[3] == 0)
		{
			lastSelect[3] = -1;
		}
		currentListLength = 2;
		if (Char.myCharz().clan != null)
		{
			isMessage = true;
			isViewMember = false;
			isSearchClan = false;
		}
		else
		{
			isMessage = false;
			isViewMember = false;
			isSearchClan = true;
		}
		if (Char.myCharz().clan != null)
		{
			currentListLength = ClanMessage.vMessage.size() + 2;
		}
		initTabClans();
		cSelected = -1;
		if (chatTField == null)
		{
			chatTField = new ChatTextField();
			chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
			chatTField.initChatTextField();
			chatTField.parentScreen = GameCanvas.panel;
		}
		if (Char.myCharz().clan == null)
		{
			clanReport = mResources.findingClan;
			Service.gI().searchClan(string.Empty);
		}
		selected = lastSelect[currentTabIndex];
		if (GameCanvas.isTouch)
		{
			selected = -1;
		}
	}

	public void initLogMessage()
	{
		currentListLength = logChat.size() + 1;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		cmx = (cmtoX = 0);
	}

	private void setTabMessage()
	{
		ITEM_HEIGHT = 24;
		initLogMessage();
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	public void setTabShop()
	{
		ITEM_HEIGHT = 24;
		if (currentTabIndex == currentTabName.Length - 1 && GameCanvas.panel2 == null && typeShop != 2)
		{
			currentListLength = Char.myCharz().arrItemBag.Length + Char.myCharz().arrItemBody.Length;
		}
		else
		{
			currentListLength = Char.myCharz().arrItemShop[currentTabIndex].Length;
		}
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	private void setTabSkill()
	{
		ITEM_HEIGHT = 30;
		currentListLength = Char.myCharz().nClass.skillTemplates.Length + 6;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = cmyLim;
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	private void setTabMapTrans()
	{
		ITEM_HEIGHT = 24;
		currentListLength = mapNames.Length;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		cmy = (cmtoY = 0);
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	private void setTabZone()
	{
		ITEM_HEIGHT = 24;
		currentListLength = GameScr.gI().zones.Length;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		cmy = (cmtoY = 0);
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	private void setTabBox()
	{
		Item[] arrItemBox = Char.myCharz().arrItemBox;
		currentListLength = arrItemBox.Length;
		ITEM_HEIGHT = 24;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 9;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	private void setTabPetInventory()
	{
		ITEM_HEIGHT = 30;
		Item[] arrItemBody = Char.myPetz().arrItemBody;
		Skill[] arrPetSkill = Char.myPetz().arrPetSkill;
		currentListLength = arrItemBody.Length + arrPetSkill.Length;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = 0);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	private void setTabInventory()
	{
		ITEM_HEIGHT = 24;
		Item[] arrItemBody = Char.myCharz().arrItemBody;
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		currentListLength = arrItemBody.Length + arrItemBag.Length;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = 0);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	private void setTabMap()
	{
		if (!isPaintMap)
		{
			return;
		}
		if (TileMap.lastPlanetId != TileMap.planetID)
		{
			Res.outz("LOAD TAM HINH");
			imgMap = GameCanvas.loadImageRMS("/img/map" + TileMap.planetID + ".png");
			TileMap.lastPlanetId = TileMap.planetID;
		}
		cmxMap = getXMap() - wScroll / 2;
		cmyMap = getYMap() + yScroll - (yScroll + hScroll / 2);
		pa1 = cmxMap;
		pa2 = cmyMap;
		cmxMapLim = 250 - wScroll;
		cmyMapLim = 220 - hScroll;
		if (cmxMapLim < 0)
		{
			cmxMapLim = 0;
		}
		if (cmyMapLim < 0)
		{
			cmyMapLim = 0;
		}
		for (int i = 0; i < mapId[TileMap.planetID].Length; i++)
		{
			if (TileMap.mapID == mapId[TileMap.planetID][i])
			{
				xMove = mapX[TileMap.planetID][i] + xScroll;
				yMove = mapY[TileMap.planetID][i] + yScroll + 5;
				break;
			}
		}
		xMap = getXMap() + xScroll;
		yMap = getYMap() + yScroll;
		xMapTask = getXMapTask() + xScroll;
		yMapTask = getYMapTask() + yScroll;
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	private void setTabTask()
	{
		cmyQuest = 0;
	}

	public void moveCamera()
	{
		if (timeShow > 0)
		{
			timeShow--;
		}
		if (justRelease && Equals(GameCanvas.panel) && typeShop == 2 && maxPageShop[currentTabIndex] > 1)
		{
			if (cmy < -50)
			{
				InfoDlg.showWait();
				justRelease = false;
				if (currPageShop[currentTabIndex] <= 0)
				{
					Service.gI().kigui(4, -1, (sbyte)currentTabIndex, maxPageShop[currentTabIndex] - 1, -1);
				}
				else
				{
					Service.gI().kigui(4, -1, (sbyte)currentTabIndex, currPageShop[currentTabIndex] - 1, -1);
				}
			}
			else if (cmy > cmyLim + 50)
			{
				justRelease = false;
				InfoDlg.showWait();
				if (currPageShop[currentTabIndex] >= maxPageShop[currentTabIndex] - 1)
				{
					Service.gI().kigui(4, -1, (sbyte)currentTabIndex, 0, -1);
				}
				else
				{
					Service.gI().kigui(4, -1, (sbyte)currentTabIndex, currPageShop[currentTabIndex] + 1, -1);
				}
			}
		}
		if (cmx != cmtoX && !pointerIsDowning)
		{
			cmvx = cmtoX - cmx << 2;
			cmdx += cmvx;
			cmx += cmdx >> 3;
			cmdx &= 15;
		}
		if (Math.abs(cmtoX - cmx) < 10)
		{
			cmx = cmtoX;
		}
		if (isClose)
		{
			isClose = false;
			cmtoX = wScroll;
		}
		if (cmtoX >= wScroll - 10 && cmx >= wScroll - 10 && position == 0)
		{
			isShow = false;
			cleanCombine();
			if (isChangeZone)
			{
				isChangeZone = false;
				if (Char.myCharz().cHP > 0 && Char.myCharz().statusMe != 14)
				{
					InfoDlg.showWait();
					if (type == 3)
					{
						Service.gI().requestChangeZone(selected, -1);
					}
					else if (type == 14)
					{
						Service.gI().requestMapSelect(selected);
					}
				}
			}
			if (isSelectPlayerMenu)
			{
				isSelectPlayerMenu = false;
				Command command = (Command)vPlayerMenu.elementAt(selected);
				command.performAction();
			}
			vPlayerMenu.removeAllElements();
			charMenu = null;
		}
		if (cmRun != 0 && !pointerIsDowning)
		{
			cmtoY += cmRun / 100;
			if (cmtoY < 0)
			{
				cmtoY = 0;
			}
			else if (cmtoY > cmyLim)
			{
				cmtoY = cmyLim;
			}
			else
			{
				cmy = cmtoY;
			}
			cmRun = cmRun * 9 / 10;
			if (cmRun < 100 && cmRun > -100)
			{
				cmRun = 0;
			}
		}
		if (cmy != cmtoY && !pointerIsDowning)
		{
			cmvy = cmtoY - cmy << 2;
			cmdy += cmvy;
			cmy += cmdy >> 4;
			cmdy &= 15;
		}
		cmyLast[currentTabIndex] = cmy;
	}

	public void paintDetail(mGraphics g)
	{
		if (cp == null || cp.says == null)
		{
			return;
		}
		cp.paint(g);
		int num = cp.cx + 13;
		int num2 = cp.cy + 11;
		if (type == 15)
		{
			num += 5;
			num2 += 26;
		}
		if (type == 0 && currentTabIndex == 3)
		{
			if (isSearchClan)
			{
				num -= 5;
			}
			else if (partID != null || charInfo != null)
			{
				num = cp.cx + 21;
				num2 = cp.cy + 40;
			}
		}
		if (partID != null)
		{
			Part part = GameScr.parts[partID[0]];
			Part part2 = GameScr.parts[partID[1]];
			Part part3 = GameScr.parts[partID[2]];
			SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, num + Char.CharInfo[0][0][1] + part.pi[Char.CharInfo[0][0][0]].dx, num2 - Char.CharInfo[0][0][2] + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
			SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[0][1][0]].id, num + Char.CharInfo[0][1][1] + part2.pi[Char.CharInfo[0][1][0]].dx, num2 - Char.CharInfo[0][1][2] + part2.pi[Char.CharInfo[0][1][0]].dy, 0, 0);
			SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[0][2][0]].id, num + Char.CharInfo[0][2][1] + part3.pi[Char.CharInfo[0][2][0]].dx, num2 - Char.CharInfo[0][2][2] + part3.pi[Char.CharInfo[0][2][0]].dy, 0, 0);
		}
		else if (charInfo != null)
		{
			charInfo.paintCharBody(g, num + 5, num2 + 25, 1, 0, isPaintBag: true);
		}
		else if (idIcon != -1)
		{
			SmallImage.drawSmallImage(g, idIcon, num, num2, 0, 3);
		}
		if (currItem != null && currItem.template.type != 5)
		{
			if (currItem.compare > 0)
			{
				g.drawImage(imgUp, num - 7, num2 + 13, 3);
				mFont.tahoma_7b_green.drawString(g, Res.abs(currItem.compare) + string.Empty, num + 1, num2 + 8, 0);
			}
			else if (currItem.compare < 0 && currItem.compare != -1)
			{
				g.drawImage(imgDown, num - 7, num2 + 13, 3);
				mFont.tahoma_7b_red.drawString(g, Res.abs(currItem.compare) + string.Empty, num + 1, num2 + 8, 0);
			}
		}
	}

	public void paintTop(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		g.setColor(0);
		if (currentListLength == 0)
		{
			return;
		}
		int num = (cmy + hScroll) / 24 + 1;
		if (num < hScroll / 24 + 1)
		{
			num = hScroll / 24 + 1;
		}
		if (num > currentListLength)
		{
			num = currentListLength;
		}
		int num2 = cmy / 24;
		if (num2 >= num)
		{
			num2 = num - 1;
		}
		if (num2 < 0)
		{
			num2 = 0;
		}
		for (int i = num2; i < num; i++)
		{
			int num3 = xScroll;
			int num4 = yScroll + i * ITEM_HEIGHT;
			int num5 = 24;
			int h = ITEM_HEIGHT - 1;
			int num6 = xScroll + num5;
			int num7 = yScroll + i * ITEM_HEIGHT;
			int num8 = wScroll - num5;
			int num9 = ITEM_HEIGHT - 1;
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num6, num7, num8, num9);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num3, num4, num5, h);
			TopInfo topInfo = (TopInfo)vTop.elementAt(i);
			if (topInfo.headICON != -1)
			{
				SmallImage.drawSmallImage(g, topInfo.headICON, num3, num4, 0, 0);
			}
			else
			{
				Part part = GameScr.parts[topInfo.headID];
				SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, num3 + part.pi[Char.CharInfo[0][0][0]].dx, num4 + num9 - 1, 0, mGraphics.BOTTOM | mGraphics.LEFT);
			}
			g.setClip(xScroll, yScroll + cmy, wScroll, hScroll);
			if (topInfo.pId != Char.myCharz().charID)
			{
				mFont.tahoma_7b_green.drawString(g, topInfo.name, num6 + 5, num7, 0);
			}
			else
			{
				mFont.tahoma_7b_red.drawString(g, topInfo.name, num6 + 5, num7, 0);
			}
			mFont.tahoma_7_blue.drawString(g, topInfo.info, num6 + num8 - 5, num7 + 11, 1);
			mFont.tahoma_7_green2.drawString(g, mResources.rank + ": " + topInfo.rank + string.Empty, num6 + 5, num7 + 11, 0);
		}
		paintScrollArrow(g);
	}

	public void paint(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY() + mGraphics.addYWhenOpenKeyBoard);
		g.translate(-cmx, 0);
		g.translate(X, Y);
		if (GameCanvas.panel.combineSuccess != -1)
		{
			if (Equals(GameCanvas.panel))
			{
				paintCombineEff(g);
			}
			return;
		}
		GameCanvas.paintz.paintFrameSimple(X, Y, W, H, g);
		paintTopInfo(g);
		paintBottomMoneyInfo(g);
		paintTab(g);
		switch (type)
		{
		case 9:
			paintArchivement(g);
			break;
		case 21:
			if (currentTabIndex == 0)
			{
				paintPetInventory(g);
			}
			if (currentTabIndex == 1)
			{
				paintPetStatus(g);
			}
			if (currentTabIndex == 2)
			{
				paintInventory(g);
			}
			break;
		case 24:
			paintGameSubInfo(g);
			break;
		case 23:
			paintGameInfo(g);
			break;
		case 0:
			if (currentTabIndex == 0)
			{
				paintTask(g);
			}
			if (currentTabIndex == 1)
			{
				paintInventory(g);
			}
			if (currentTabIndex == 2)
			{
				paintSkill(g);
			}
			if (currentTabIndex == 3)
			{
				if (mainTabName.Length == 4)
				{
					paintTools(g);
				}
				else
				{
					paintClans(g);
				}
			}
			if (currentTabIndex == 4)
			{
				paintTools(g);
			}
			break;
		case 2:
			if (currentTabIndex == 0)
			{
				paintBox(g);
			}
			if (currentTabIndex == 1)
			{
				paintInventory(g);
			}
			break;
		case 3:
			paintZone(g);
			break;
		case 1:
			paintShop(g);
			break;
		case 25:
			paintSpeacialSkill(g);
			break;
		case 4:
			paintMap(g);
			break;
		case 7:
			paintInventory(g);
			break;
		case 17:
			paintShop(g);
			break;
		case 8:
			paintLogChat(g);
			break;
		case 10:
			paintPlayerMenu(g);
			break;
		case 11:
			paintFriend(g);
			break;
		case 16:
			paintEnemy(g);
			break;
		case 15:
			paintTop(g);
			break;
		case 12:
			if (currentTabIndex == 0)
			{
				paintCombine(g);
			}
			if (currentTabIndex == 1)
			{
				paintInventory(g);
			}
			break;
		case 13:
			if (currentTabIndex == 0)
			{
				if (Equals(GameCanvas.panel))
				{
					paintInventory(g);
				}
				else
				{
					paintGiaoDich(g, isMe: false);
				}
			}
			if (currentTabIndex == 1)
			{
				paintGiaoDich(g, isMe: true);
			}
			if (currentTabIndex == 2)
			{
				paintGiaoDich(g, isMe: false);
			}
			break;
		case 14:
			paintMapTrans(g);
			break;
		case 18:
			paintFlagChange(g);
			break;
		case 19:
			paintOption(g);
			break;
		case 20:
			paintAccount(g);
			break;
		case 22:
			paintAuto(g);
			break;
		}
		GameScr.resetTranslate(g);
		paintDetail(g);
		if (cmx == cmtoX)
		{
			cmdClose.paint(g);
		}
		if (tabIcon != null && tabIcon.isShow)
		{
			tabIcon.paint(g);
		}
	}

	private void paintShop(mGraphics g)
	{
		if (type == 1 && currentTabIndex == currentTabName.Length - 1 && GameCanvas.panel2 == null && typeShop != 2)
		{
			paintInventory(g);
			return;
		}
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		if (typeShop == 2 && Equals(GameCanvas.panel))
		{
			if (currentTabIndex <= 3 && GameCanvas.isTouch)
			{
				if (cmy < -50)
				{
					GameCanvas.paintShukiren(xScroll + wScroll / 2, yScroll + 30, g);
				}
				else if (cmy < 0)
				{
					mFont.tahoma_7_grey.drawString(g, mResources.getDown, xScroll + wScroll / 2, yScroll + 15, 2);
				}
				else if (cmyLim >= 0)
				{
					if (cmy > cmyLim + 50)
					{
						GameCanvas.paintShukiren(xScroll + wScroll / 2, yScroll + hScroll - 30, g);
					}
					else if (cmy > cmyLim)
					{
						mFont.tahoma_7_grey.drawString(g, mResources.getUp, xScroll + wScroll / 2, yScroll + hScroll - 25, 2);
					}
				}
			}
			if (Char.myCharz().arrItemShop[currentTabIndex].Length == 0 && type != 17)
			{
				mFont.tahoma_7_grey.drawString(g, mResources.notYetSell, xScroll + wScroll / 2, yScroll + hScroll / 2 - 10, 2);
				return;
			}
		}
		g.translate(0, -cmy);
		Item[] array = Char.myCharz().arrItemShop[currentTabIndex];
		if (typeShop == 2 && (currentTabIndex == 4 || type == 17))
		{
			array = Char.myCharz().arrItemShop[4];
			if (array.Length == 0)
			{
				mFont.tahoma_7_grey.drawString(g, mResources.notYetSell, xScroll + wScroll / 2, yScroll + hScroll / 2 - 10, 2);
				return;
			}
		}
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			int num2 = xScroll + 26;
			int num3 = yScroll + i * ITEM_HEIGHT;
			int num4 = wScroll - 26;
			int h = ITEM_HEIGHT - 1;
			int num5 = xScroll;
			int num6 = yScroll + i * ITEM_HEIGHT;
			int num7 = 24;
			int num8 = ITEM_HEIGHT - 1;
			if (num3 - cmy > yScroll + hScroll || num3 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num2, num3, num4, h);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num5, num6, num7, num8);
			Item item = array[i];
			if (item != null)
			{
				string text = string.Empty;
				if (item.itemOption != null)
				{
					for (int j = 0; j < item.itemOption.Length; j++)
					{
						if (item.itemOption[j].optionTemplate.id == 72)
						{
							text = " [+" + item.itemOption[j].param + "]";
						}
					}
				}
				if (item.isMe != 0 && typeShop == 2 && currentTabIndex <= 3 && !Equals(GameCanvas.panel2))
				{
					mFont.tahoma_7b_green.drawString(g, item.template.name + text, num2 + 5, num3 + 1, 0);
				}
				else
				{
					mFont.tahoma_7_green2.drawString(g, item.template.name + text, num2 + 5, num3 + 1, 0);
				}
				string text2 = string.Empty;
				if (item.itemOption != null && item.itemOption.Length >= 1)
				{
					if (item.itemOption[0] != null && item.itemOption[0].optionTemplate.id != 102 && item.itemOption[0].optionTemplate.id != 107)
					{
						text2 += item.itemOption[0].getOptionString();
					}
					mFont mFont2 = mFont.tahoma_7_blue;
					if (item.compare < 0 && item.template.type != 5)
					{
						mFont2 = mFont.tahoma_7_red;
					}
					if (typeShop == 2 && item.itemOption.Length > 1 && item.buyType != -1)
					{
						text2 += string.Empty;
					}
					if (typeShop != 2 || (typeShop == 2 && item.buyType <= 1))
					{
						mFont2.drawString(g, text2, num2 + 5, num3 + 11, 0);
					}
				}
				if (item.buySpec > 0)
				{
					SmallImage.drawSmallImage(g, item.iconSpec, num2 + num4 - 7, num3 + 9, 0, 3);
					mFont.tahoma_7b_blue.drawString(g, Res.formatNumber(item.buySpec), num2 + num4 - 15, num3 + 1, mFont.RIGHT);
				}
				if (item.buyCoin != 0 || item.buyGold != 0)
				{
					if (typeShop != 2 && item.powerRequire == 0)
					{
						if (item.buyCoin > 0 && item.buyGold > 0)
						{
							if (item.buyCoin > 0)
							{
								g.drawImage(imgXu, num2 + num4 - 7, num3 + 7, 3);
								mFont.tahoma_7b_yellow.drawString(g, Res.formatNumber(item.buyCoin), num2 + num4 - 15, num3 + 1, mFont.RIGHT);
							}
							if (item.buyGold > 0)
							{
								g.drawImage(imgLuong, num2 + num4 - 7, num3 + 7 + 11, 3);
								mFont.tahoma_7b_green.drawString(g, Res.formatNumber(item.buyGold), num2 + num4 - 15, num3 + 12, mFont.RIGHT);
							}
						}
						else
						{
							if (item.buyCoin > 0)
							{
								g.drawImage(imgXu, num2 + num4 - 7, num3 + 7, 3);
								mFont.tahoma_7b_yellow.drawString(g, Res.formatNumber(item.buyCoin), num2 + num4 - 15, num3 + 1, mFont.RIGHT);
							}
							if (item.buyGold > 0)
							{
								g.drawImage(imgLuong, num2 + num4 - 7, num3 + 7, 3);
								mFont.tahoma_7b_green.drawString(g, Res.formatNumber(item.buyGold), num2 + num4 - 15, num3 + 1, mFont.RIGHT);
							}
						}
					}
					if (typeShop == 2 && currentTabIndex <= 3 && !Equals(GameCanvas.panel2))
					{
						if (item.buyCoin > 0 && item.buyGold > 0)
						{
							if (item.buyCoin > 0)
							{
								g.drawImage(imgXu, num2 + num4 - 7, num3 + 7, 3);
								mFont.tahoma_7b_yellow.drawString(g, Res.formatNumber2(item.buyCoin), num2 + num4 - 15, num3 + 1, mFont.RIGHT);
							}
							if (item.buyGold > 0)
							{
								g.drawImage(imgLuong, num2 + num4 - 7, num3 + 7 + 11, 3);
								mFont.tahoma_7b_green.drawString(g, Res.formatNumber2(item.buyGold), num2 + num4 - 15, num3 + 12, mFont.RIGHT);
							}
						}
						else
						{
							if (item.buyCoin > 0)
							{
								g.drawImage(imgXu, num2 + num4 - 7, num3 + 7, 3);
								mFont.tahoma_7b_yellow.drawString(g, Res.formatNumber2(item.buyCoin), num2 + num4 - 15, num3 + 1, mFont.RIGHT);
							}
							if (item.buyGold > 0)
							{
								g.drawImage(imgLuong, num2 + num4 - 7, num3 + 7, 3);
								mFont.tahoma_7b_green.drawString(g, Res.formatNumber2(item.buyGold), num2 + num4 - 15, num3 + 1, mFont.RIGHT);
							}
						}
					}
				}
				SmallImage.drawSmallImage(g, item.template.iconID, num5 + num7 / 2, num6 + num8 / 2, 0, 3);
				if (item.quantity > 1)
				{
					mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num5 + num7, num6 + num8 - mFont.tahoma_7_yellow.getHeight(), 1);
				}
				if (item.newItem && GameCanvas.gameTick % 10 > 5)
				{
					g.drawImage(imgNew, num5 + num7 / 2, num3 + 19, 3);
				}
			}
			if (typeShop != 2 || (!Equals(GameCanvas.panel2) && currentTabIndex != 4) || item.buyType == 0)
			{
				continue;
			}
			if (item.buyType == 1)
			{
				mFont.tahoma_7_green.drawString(g, mResources.dangban, num2 + num4 - 5, num3 + 1, mFont.RIGHT);
				if (item.buyCoin != -1)
				{
					g.drawImage(imgXu, num2 + num4 - 7, num3 + 19, 3);
					mFont.tahoma_7b_yellow.drawString(g, Res.formatNumber2(item.buyCoin), num2 + num4 - 15, num3 + 13, mFont.RIGHT);
				}
				else if (item.buyGold != -1)
				{
					g.drawImage(imgLuongKhoa, num2 + num4 - 7, num3 + 17, 3);
					mFont.tahoma_7b_red.drawString(g, Res.formatNumber2(item.buyGold), num2 + num4 - 15, num3 + 11, mFont.RIGHT);
				}
			}
			else if (item.buyType == 2)
			{
				mFont.tahoma_7b_blue.drawString(g, mResources.daban, num2 + num4 - 5, num3 + 1, mFont.RIGHT);
				if (item.buyCoin != -1)
				{
					g.drawImage(imgXu, num2 + num4 - 7, num3 + 17, 3);
					mFont.tahoma_7b_yellow.drawString(g, Res.formatNumber2(item.buyCoin), num2 + num4 - 15, num3 + 11, mFont.RIGHT);
				}
				else if (item.buyGold != -1)
				{
					g.drawImage(imgLuongKhoa, num2 + num4 - 7, num3 + 17, 3);
					mFont.tahoma_7b_red.drawString(g, Res.formatNumber2(item.buyGold), num2 + num4 - 15, num3 + 11, mFont.RIGHT);
				}
			}
		}
		paintScrollArrow(g);
	}

	private void paintAuto(mGraphics g)
	{
	}

	private void paintPetStatus(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < strStatus.Length; i++)
		{
			int x = xScroll;
			int num = yScroll + i * ITEM_HEIGHT;
			int num2 = wScroll - 1;
			int h = ITEM_HEIGHT - 1;
			if (num - cmy <= yScroll + hScroll && num - cmy >= yScroll - ITEM_HEIGHT)
			{
				g.setColor((i != selected) ? 15196114 : 16383818);
				g.fillRect(x, num, num2, h);
				mFont.tahoma_7b_dark.drawString(g, strStatus[i], xScroll + wScroll / 2, num + 6, mFont.CENTER);
			}
		}
		paintScrollArrow(g);
	}

	private void paintPetSkill()
	{
	}

	private void paintPetInventory(mGraphics g)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		Item[] arrItemBody = Char.myPetz().arrItemBody;
		Skill[] arrPetSkill = Char.myPetz().arrPetSkill;
		for (int i = 0; i < arrItemBody.Length + arrPetSkill.Length; i++)
		{
			bool flag = i < arrItemBody.Length;
			int num = i;
			int num2 = i - arrItemBody.Length;
			int num3 = xScroll + 36;
			int num4 = yScroll + i * ITEM_HEIGHT;
			int num5 = wScroll - 36;
			int h = ITEM_HEIGHT - 1;
			int num6 = xScroll;
			int num7 = yScroll + i * ITEM_HEIGHT;
			int num8 = 34;
			int num9 = ITEM_HEIGHT - 1;
			if (num4 - cmy > yScroll + hScroll || num4 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			Item item = ((!flag) ? null : arrItemBody[num]);
			g.setColor((i == selected) ? 16383818 : ((!flag) ? 15723751 : 15196114));
			g.fillRect(num3, num4, num5, h);
			g.setColor((i == selected) ? 9541120 : ((!flag) ? 11837316 : 9993045));
			if (item != null)
			{
				for (int j = 0; j < item.itemOption.Length; j++)
				{
					if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
					{
						sbyte color_Item_Upgrade = GetColor_Item_Upgrade(item.itemOption[j].param);
						int color_ItemBg = GetColor_ItemBg(color_Item_Upgrade);
						if (color_ItemBg != -1)
						{
							g.setColor((i != selected) ? GetColor_ItemBg(color_Item_Upgrade) : GetColor_ItemBg(color_Item_Upgrade));
						}
					}
				}
			}
			g.fillRect(num6, num7, num8, num9);
			if (item != null && item.isSelect && GameCanvas.panel.type == 12)
			{
				g.setColor((i != selected) ? 6047789 : 7040779);
				g.fillRect(num6, num7, num8, num9);
			}
			if (item != null)
			{
				string text = string.Empty;
				if (item.itemOption != null)
				{
					for (int k = 0; k < item.itemOption.Length; k++)
					{
						if (item.itemOption[k].optionTemplate.id == 72)
						{
							text = " [+" + item.itemOption[k].param + "]";
						}
					}
				}
				mFont.tahoma_7_green2.drawString(g, item.template.name + text, num3 + 5, num4 + 1, 0);
				string text2 = string.Empty;
				if (item.itemOption != null)
				{
					if (item.itemOption.Length > 0 && item.itemOption[0] != null && item.itemOption[0].optionTemplate.id != 102 && item.itemOption[0].optionTemplate.id != 107)
					{
						text2 += item.itemOption[0].getOptionString();
					}
					mFont mFont2 = mFont.tahoma_7_blue;
					if (item.compare < 0 && item.template.type != 5)
					{
						mFont2 = mFont.tahoma_7_red;
					}
					if (item.itemOption.Length > 1)
					{
						for (int l = 1; l < 2; l++)
						{
							if (item.itemOption[l] != null && item.itemOption[l].optionTemplate.id != 102 && item.itemOption[l].optionTemplate.id != 107)
							{
								text2 = text2 + "," + item.itemOption[l].getOptionString();
							}
						}
					}
					mFont2.drawString(g, text2, num3 + 5, num4 + 11, mFont.LEFT);
				}
				SmallImage.drawSmallImage(g, item.template.iconID, num6 + num8 / 2 - ((item.quantity > 1) ? 8 : 0), num7 + num9 / 2, 0, 3);
				if (item.itemOption != null)
				{
					for (int m = 0; m < item.itemOption.Length; m++)
					{
						paintOptItem(g, item.itemOption[m].optionTemplate.id, item.itemOption[m].param, num6, num7, num8, num9);
					}
					for (int n = 0; n < item.itemOption.Length; n++)
					{
						paintOptSlotItem(g, item.itemOption[n].optionTemplate.id, item.itemOption[n].param, num6, num7, num8, num9);
					}
				}
				if (item.quantity > 1)
				{
					mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num6 + num8 - 15, num7 + 6, 0);
				}
			}
			else if (!flag)
			{
				Skill skill = arrPetSkill[num2];
				g.drawImage(GameScr.imgSkill, num6 + num8 / 2, num7 + num9 / 2, 3);
				if (skill.template != null)
				{
					mFont.tahoma_7_blue.drawString(g, skill.template.name, num3 + 5, num4 + 1, 0);
					mFont.tahoma_7_green2.drawString(g, mResources.level + ": " + skill.point + string.Empty, num3 + 5, num4 + 11, 0);
					SmallImage.drawSmallImage(g, skill.template.iconId, num6 + num8 / 2, num7 + num9 / 2, 0, 3);
				}
				else
				{
					mFont.tahoma_7_green2.drawString(g, skill.moreInfo, num3 + 5, num4 + 5, 0);
					SmallImage.drawSmallImage(g, GameScr.efs[98].arrEfInfo[0].idImg, num6 + num8 / 2, num7 + num9 / 2, 0, 3);
				}
			}
		}
		paintScrollArrow(g);
	}

	private void paintScrollArrow(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		if ((cmy > 24 && currentListLength > 0) || (Equals(GameCanvas.panel) && typeShop == 2 && maxPageShop[currentTabIndex] > 1))
		{
			g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 1, xScroll + wScroll - 12, yScroll + 3, 0);
		}
		if ((cmy < cmyLim && currentListLength > 0) || (Equals(GameCanvas.panel) && typeShop == 2 && maxPageShop[currentTabIndex] > 1))
		{
			g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, xScroll + wScroll - 12, yScroll + hScroll - 8, 0);
		}
	}

	private void paintTools(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < strTool.Length; i++)
		{
			int num = xScroll;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = wScroll - 1;
			int h = ITEM_HEIGHT - 1;
			if (num2 - cmy > yScroll + hScroll || num2 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num, num2, num3, h);
			mFont.tahoma_7b_dark.drawString(g, strTool[i], xScroll + wScroll / 2, num2 + 6, mFont.CENTER);
			if (!strTool[i].Equals(mResources.gameInfo))
			{
				continue;
			}
			for (int j = 0; j < vGameInfo.size(); j++)
			{
				GameInfo gameInfo = (GameInfo)vGameInfo.elementAt(j);
				if (!gameInfo.hasRead)
				{
					if (GameCanvas.gameTick % 20 > 10)
					{
						g.drawImage(imgNew, num + 10, num2 + 10, 3);
					}
					break;
				}
			}
		}
		paintScrollArrow(g);
	}

	private void paintGameSubInfo(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < contenInfo.Length; i++)
		{
			int num = xScroll;
			int num2 = yScroll + i * 15;
			int num3 = wScroll - 1;
			int num4 = ITEM_HEIGHT - 1;
			if (num2 - cmy <= yScroll + hScroll && num2 - cmy >= yScroll - ITEM_HEIGHT)
			{
				mFont.tahoma_7b_dark.drawString(g, contenInfo[i], xScroll + 5, num2 + 6, mFont.LEFT);
			}
		}
		paintScrollArrow(g);
	}

	private void paintGameInfo(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < vGameInfo.size(); i++)
		{
			GameInfo gameInfo = (GameInfo)vGameInfo.elementAt(i);
			int num = xScroll;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = wScroll - 1;
			int h = ITEM_HEIGHT - 1;
			if (num2 - cmy <= yScroll + hScroll && num2 - cmy >= yScroll - ITEM_HEIGHT)
			{
				g.setColor((i != selected) ? 15196114 : 16383818);
				g.fillRect(num, num2, num3, h);
				mFont.tahoma_7b_dark.drawString(g, gameInfo.main, xScroll + wScroll / 2, num2 + 6, mFont.CENTER);
				if (!gameInfo.hasRead && GameCanvas.gameTick % 20 > 10)
				{
					g.drawImage(imgNew, num + 10, num2 + 10, 3);
				}
			}
		}
		paintScrollArrow(g);
	}

	private void paintSkill(mGraphics g)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		int num = Char.myCharz().nClass.skillTemplates.Length;
		for (int i = 0; i < num + 6; i++)
		{
			int num2 = xScroll + 30;
			int num3 = yScroll + i * ITEM_HEIGHT;
			int num4 = wScroll - 30;
			int h = ITEM_HEIGHT - 1;
			int num5 = xScroll;
			int num6 = yScroll + i * ITEM_HEIGHT;
			int num7 = 34;
			int num8 = ITEM_HEIGHT - 1;
			if (num3 - cmy > yScroll + hScroll || num3 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			if (i == 5)
			{
				g.setColor((i != selected) ? 16765060 : 16776068);
			}
			g.fillRect(num2, num3, num4, h);
			g.drawImage(GameScr.imgSkill, num5, num6, 0);
			if (i == 0)
			{
				SmallImage.drawSmallImage(g, 567, num5 + 4, num6 + 4, 0, 0);
				string st = mResources.HP + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cHPGoc);
				mFont.tahoma_7b_blue.drawString(g, st, num2 + 5, num3 + 3, 0);
				mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cHPGoc + 1000) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().hpFrom1000TiemNang, num2 + 5, num3 + 15, 0);
			}
			if (i == 1)
			{
				SmallImage.drawSmallImage(g, 569, num5 + 4, num6 + 4, 0, 0);
				string st2 = mResources.KI + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cMPGoc);
				mFont.tahoma_7b_blue.drawString(g, st2, num2 + 5, num3 + 3, 0);
				mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cMPGoc + 1000) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().mpFrom1000TiemNang, num2 + 5, num3 + 15, 0);
			}
			if (i == 2)
			{
				SmallImage.drawSmallImage(g, 568, num5 + 4, num6 + 4, 0, 0);
				string st3 = mResources.hit_point + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cDamGoc);
				mFont.tahoma_7b_blue.drawString(g, st3, num2 + 5, num3 + 3, 0);
				mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cDamGoc * 100) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().damFrom1000TiemNang, num2 + 5, num3 + 15, 0);
			}
			if (i == 3)
			{
				SmallImage.drawSmallImage(g, 721, num5 + 4, num6 + 4, 0, 0);
				string st4 = mResources.armor + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cDefGoc);
				mFont.tahoma_7b_blue.drawString(g, st4, num2 + 5, num3 + 3, 0);
				mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(500000 + Char.myCharz().cDefGoc * 100000) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().defFrom1000TiemNang, num2 + 5, num3 + 15, 0);
			}
			if (i == 4)
			{
				SmallImage.drawSmallImage(g, 719, num5 + 4, num6 + 4, 0, 0);
				string st5 = mResources.critical + " " + mResources.root + ": " + Char.myCharz().cCriticalGoc + "%";
				long num9 = 50000000L;
				for (int j = 0; j < Char.myCharz().cCriticalGoc; j++)
				{
					num9 *= 5;
				}
				mFont.tahoma_7b_blue.drawString(g, st5, num2 + 5, num3 + 3, 0);
				long m = num9;
				mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(m) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().criticalFrom1000Tiemnang, num2 + 5, num3 + 15, 0);
			}
			if (i == 5)
			{
				if (specialInfo != null)
				{
					SmallImage.drawSmallImage(g, spearcialImage, num5 + 4, num6 + 4, 0, 0);
					string[] array = mFont.tahoma_7.splitFontArray(specialInfo, 120);
					for (int k = 0; k < array.Length; k++)
					{
						mFont.tahoma_7_green2.drawString(g, array[k], num2 + 5, num3 + 3 + k * 12, 0);
					}
				}
				else
				{
					mFont.tahoma_7_green2.drawString(g, string.Empty, num2 + 5, num3 + 9, 0);
				}
			}
			if (i < 6)
			{
				continue;
			}
			int num10 = i - 6;
			SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[num10];
			SmallImage.drawSmallImage(g, skillTemplate.iconId, num5 + 4, num6 + 4, 0, 0);
			Skill skill = Char.myCharz().getSkill(skillTemplate);
			if (skill != null)
			{
				mFont.tahoma_7b_blue.drawString(g, skillTemplate.name, num2 + 5, num3 + 3, 0);
				mFont.tahoma_7_blue.drawString(g, mResources.level + ": " + skill.point, num2 + num4 - 5, num3 + 3, mFont.RIGHT);
				if (skill.point == skillTemplate.maxPoint)
				{
					mFont.tahoma_7_green2.drawString(g, mResources.max_level_reach, num2 + 5, num3 + 15, 0);
					continue;
				}
				Skill skill2 = skillTemplate.skills[skill.point];
				mFont.tahoma_7_green2.drawString(g, mResources.level + " " + (skill.point + 1) + " " + mResources.need + " " + Res.formatNumber2(skill2.powRequire) + " " + mResources.potential, num2 + 5, num3 + 15, 0);
			}
			else
			{
				Skill skill3 = skillTemplate.skills[0];
				mFont.tahoma_7b_green.drawString(g, skillTemplate.name, num2 + 5, num3 + 3, 0);
				mFont.tahoma_7_green2.drawString(g, mResources.need_upper + " " + Res.formatNumber2(skill3.powRequire) + " " + mResources.potential_to_learn, num2 + 5, num3 + 15, 0);
			}
		}
		paintScrollArrow(g);
	}

	private void paintMapTrans(mGraphics g)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < mapNames.Length; i++)
		{
			int num = xScroll + 36;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = wScroll - 36;
			int h = ITEM_HEIGHT - 1;
			int num4 = xScroll;
			int num5 = yScroll + i * ITEM_HEIGHT;
			int num6 = 34;
			int num7 = ITEM_HEIGHT - 1;
			if (num2 - cmy <= yScroll + hScroll && num2 - cmy >= yScroll - ITEM_HEIGHT)
			{
				g.setColor((i != selected) ? 15196114 : 16383818);
				g.fillRect(xScroll, num2, wScroll, h);
				mFont.tahoma_7b_blue.drawString(g, mapNames[i], 5, num2 + 1, 0);
				mFont.tahoma_7_grey.drawString(g, planetNames[i], 5, num2 + 11, 0);
			}
		}
		paintScrollArrow(g);
	}

	private void paintZone(mGraphics g)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		int[] zones = GameScr.gI().zones;
		int[] pts = GameScr.gI().pts;
		for (int i = 0; i < pts.Length; i++)
		{
			int num = xScroll + 36;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = wScroll - 36;
			int h = ITEM_HEIGHT - 1;
			int num4 = xScroll;
			int y = yScroll + i * ITEM_HEIGHT;
			int num5 = 34;
			int h2 = ITEM_HEIGHT - 1;
			if (num2 - cmy > yScroll + hScroll || num2 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num, num2, num3, h);
			g.setColor(zoneColor[pts[i]]);
			g.fillRect(num4, y, num5, h2);
			if (zones[i] != -1)
			{
				if (pts[i] != 1)
				{
					mFont.tahoma_7_yellow.drawString(g, zones[i] + string.Empty, num4 + num5 / 2, num2 + 6, mFont.CENTER);
				}
				else
				{
					mFont.tahoma_7_grey.drawString(g, zones[i] + string.Empty, num4 + num5 / 2, num2 + 6, mFont.CENTER);
				}
				mFont.tahoma_7_green2.drawString(g, GameScr.gI().numPlayer[i] + "/" + GameScr.gI().maxPlayer[i], num + 5, num2 + 6, 0);
			}
			if (GameScr.gI().rankName1[i] != null)
			{
				mFont.tahoma_7_grey.drawString(g, GameScr.gI().rankName1[i] + "(Top " + GameScr.gI().rank1[i] + ")", num + num3 - 2, num2 + 1, mFont.RIGHT);
				mFont.tahoma_7_grey.drawString(g, GameScr.gI().rankName2[i] + "(Top " + GameScr.gI().rank2[i] + ")", num + num3 - 2, num2 + 11, mFont.RIGHT);
			}
		}
		paintScrollArrow(g);
	}

	private void paintSpeacialSkill(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		g.setColor(0);
		if (currentListLength == 0)
		{
			return;
		}
		int num = (cmy + hScroll) / 24 + 1;
		if (num < hScroll / 24 + 1)
		{
			num = hScroll / 24 + 1;
		}
		if (num > currentListLength)
		{
			num = currentListLength;
		}
		int num2 = cmy / 24;
		if (num2 >= num)
		{
			num2 = num - 1;
		}
		if (num2 < 0)
		{
			num2 = 0;
		}
		for (int i = num2; i < num; i++)
		{
			int num3 = xScroll;
			int num4 = yScroll + i * ITEM_HEIGHT;
			int num5 = 24;
			int num6 = ITEM_HEIGHT - 1;
			int num7 = xScroll + num5;
			int num8 = yScroll + i * ITEM_HEIGHT;
			int num9 = wScroll - num5;
			int h = ITEM_HEIGHT - 1;
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num7, num8, num9, h);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num3, num4, num5, num6);
			SmallImage.drawSmallImage(g, Char.myCharz().imgSpeacialSkill[currentTabIndex][i], num3 + num5 / 2, num4 + num6 / 2, 0, 3);
			string[] array = mFont.tahoma_7_grey.splitFontArray(Char.myCharz().infoSpeacialSkill[currentTabIndex][i], 140);
			for (int j = 0; j < array.Length; j++)
			{
				mFont.tahoma_7_grey.drawString(g, array[j], num7 + 5, num8 + 1 + j * 11, 0);
			}
		}
		paintScrollArrow(g);
	}

	private void paintBox(mGraphics g)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		Item[] arrItemBox = Char.myCharz().arrItemBox;
		int num = arrItemBox.Length;
		for (int i = 0; i < num; i++)
		{
			int num2 = xScroll + 36;
			int num3 = yScroll + i * ITEM_HEIGHT;
			int num4 = wScroll - 36;
			int h = ITEM_HEIGHT - 1;
			int num5 = xScroll;
			int num6 = yScroll + i * ITEM_HEIGHT;
			int num7 = 34;
			int num8 = ITEM_HEIGHT - 1;
			if (num3 - cmy > yScroll + hScroll || num3 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num2, num3, num4, h);
			Item item = arrItemBox[i];
			g.setColor((i != selected) ? 9993045 : 9541120);
			if (item != null)
			{
				for (int j = 0; j < item.itemOption.Length; j++)
				{
					if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
					{
						sbyte color_Item_Upgrade = GetColor_Item_Upgrade(item.itemOption[j].param);
						int color_ItemBg = GetColor_ItemBg(color_Item_Upgrade);
						if (color_ItemBg != -1)
						{
							g.setColor((i != selected) ? GetColor_ItemBg(color_Item_Upgrade) : GetColor_ItemBg(color_Item_Upgrade));
						}
					}
				}
			}
			g.fillRect(num5, num6, num7, num8);
			if (item == null)
			{
				continue;
			}
			string text = string.Empty;
			if (item.itemOption != null)
			{
				for (int k = 0; k < item.itemOption.Length; k++)
				{
					if (item.itemOption[k].optionTemplate.id == 72)
					{
						text = " [+" + item.itemOption[k].getOptionString() + "]";
					}
				}
			}
			mFont.tahoma_7_green2.drawString(g, item.template.name + text, num2 + 5, num3 + 1, 0);
			string text2 = string.Empty;
			if (item.itemOption != null)
			{
				if (item.itemOption.Length > 0 && item.itemOption[0] != null)
				{
					text2 += item.itemOption[0].getOptionString();
				}
				mFont mFont2 = mFont.tahoma_7_blue;
				if (item.compare < 0 && item.template.type != 5)
				{
					mFont2 = mFont.tahoma_7_red;
				}
				if (item.itemOption.Length > 1)
				{
					for (int l = 1; l < item.itemOption.Length; l++)
					{
						if (item.itemOption[l] != null && item.itemOption[l].optionTemplate.id != 102 && item.itemOption[l].optionTemplate.id != 107)
						{
							text2 = text2 + "," + item.itemOption[l].getOptionString();
						}
					}
				}
				mFont2.drawString(g, text2, num2 + 5, num3 + 11, mFont.LEFT);
			}
			SmallImage.drawSmallImage(g, item.template.iconID, num5 + num7 / 2 - ((item.quantity > 1) ? 8 : 0), num6 + num8 / 2, 0, 3);
			if (item.itemOption != null)
			{
				for (int m = 0; m < item.itemOption.Length; m++)
				{
					paintOptItem(g, item.itemOption[m].optionTemplate.id, item.itemOption[m].param, num5, num6, num7, num8);
				}
				for (int n = 0; n < item.itemOption.Length; n++)
				{
					paintOptSlotItem(g, item.itemOption[n].optionTemplate.id, item.itemOption[n].param, num5, num6, num7, num8);
				}
			}
			if (item.quantity > 1)
			{
				mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num5 + num7 - 15, num6 + 6, 0);
			}
			mFont.tahoma_7_yellow.drawStringBorder(g, i.ToString(), num2 - 35, num3, 0, mFont.tahoma_7b_dark);
		}
		paintScrollArrow(g);
	}

	public Member getCurrMember()
	{
		if (selected < 2)
		{
			return null;
		}
		if (selected > ((member == null) ? myMember.size() : member.size()) + 1)
		{
			return null;
		}
		return (member == null) ? ((Member)myMember.elementAt(selected - 2)) : ((Member)member.elementAt(selected - 2));
	}

	public ClanMessage getCurrMessage()
	{
		if (selected < 2)
		{
			return null;
		}
		if (selected > ClanMessage.vMessage.size() + 1)
		{
			return null;
		}
		return (ClanMessage)ClanMessage.vMessage.elementAt(selected - 2);
	}

	public Clan getCurrClan()
	{
		if (selected < 2)
		{
			return null;
		}
		if (selected > clans.Length + 1)
		{
			return null;
		}
		return clans[selected - 2];
	}

	private void paintLogChat(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		g.setColor(0);
		if (logChat.size() == 0)
		{
			mFont.tahoma_7_green2.drawString(g, mResources.no_msg, xScroll + wScroll / 2, yScroll + hScroll / 2 - mFont.tahoma_7.getHeight() / 2 + 24, 2);
		}
		for (int i = 0; i < currentListLength; i++)
		{
			int num = xScroll;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = 24;
			int h = ITEM_HEIGHT - 1;
			int num4 = xScroll + num3;
			int num5 = yScroll + i * ITEM_HEIGHT;
			int num6 = wScroll - num3;
			int num7 = ITEM_HEIGHT - 1;
			if (i == 0)
			{
				g.setColor(15196114);
				g.fillRect(num, num5, wScroll, num7);
				g.drawImage((i != selected) ? GameScr.imgLbtn2 : GameScr.imgLbtnFocus2, xScroll + wScroll - 5, num5 + 2, StaticObj.TOP_RIGHT);
				((i != selected) ? mFont.tahoma_7b_dark : mFont.tahoma_7b_green2).drawString(g, (!isViewChatServer) ? mResources.on : mResources.off, xScroll + wScroll - 22, num5 + 7, 2);
				mFont.tahoma_7_grey.drawString(g, (!isViewChatServer) ? mResources.onPlease : mResources.offPlease, xScroll + 5, num5 + num7 / 2 - 4, mFont.LEFT);
				continue;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num4, num5, num6, num7);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num, num2, num3, h);
			InfoItem infoItem = (InfoItem)logChat.elementAt(i - 1);
			Part part = GameScr.parts[infoItem.charInfo.head];
			SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, num + part.pi[Char.CharInfo[0][0][0]].dx, num2 + 3 + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
			g.setClip(xScroll, yScroll + cmy, wScroll, hScroll);
			mFont tahoma_7b_dark = mFont.tahoma_7b_dark;
			tahoma_7b_dark = mFont.tahoma_7b_green2;
			tahoma_7b_dark.drawString(g, infoItem.charInfo.cName, num4 + 5, num5, 0);
			if (!infoItem.isChatServer)
			{
				mFont.tahoma_7_blue.drawString(g, Res.split(infoItem.s, "|", 0)[2], num4 + 5, num5 + 11, 0);
			}
			else
			{
				mFont.tahoma_7_red.drawString(g, Res.split(infoItem.s, "|", 0)[2], num4 + 5, num5 + 11, 0);
			}
		}
		paintScrollArrow(g);
	}

	private void paintFlagChange(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		g.setColor(0);
		for (int i = 0; i < currentListLength; i++)
		{
			int num = xScroll + 26;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = wScroll - 26;
			int h = ITEM_HEIGHT - 1;
			int num4 = xScroll;
			int num5 = yScroll + i * ITEM_HEIGHT;
			int num6 = 24;
			int num7 = ITEM_HEIGHT - 1;
			if (num2 - cmy > yScroll + hScroll || num2 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num, num2, num3, h);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num4, num5, num6, num7);
			Item item = (Item)vFlag.elementAt(i);
			if (item == null)
			{
				continue;
			}
			mFont.tahoma_7_green2.drawString(g, item.template.name, num + 5, num2 + 1, 0);
			string text = string.Empty;
			if (item.itemOption != null && item.itemOption.Length >= 1)
			{
				if (item.itemOption[0] != null && item.itemOption[0].optionTemplate.id != 102 && item.itemOption[0].optionTemplate.id != 107)
				{
					text += item.itemOption[0].getOptionString();
				}
				mFont tahoma_7_blue = mFont.tahoma_7_blue;
				tahoma_7_blue.drawString(g, text, num + 5, num2 + 11, 0);
				SmallImage.drawSmallImage(g, item.template.iconID, num4 + num6 / 2 - ((item.quantity > 1) ? 8 : 0), num5 + num7 / 2, 0, 3);
			}
		}
		paintScrollArrow(g);
	}

	private void paintEnemy(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		g.setColor(0);
		if (currentListLength == 0)
		{
			mFont.tahoma_7_green2.drawString(g, mResources.no_enemy, xScroll + wScroll / 2, yScroll + hScroll / 2 - mFont.tahoma_7.getHeight() / 2, 2);
			return;
		}
		for (int i = 0; i < currentListLength; i++)
		{
			int num = xScroll;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = 24;
			int h = ITEM_HEIGHT - 1;
			int num4 = xScroll + num3;
			int num5 = yScroll + i * ITEM_HEIGHT;
			int num6 = wScroll - num3;
			int h2 = ITEM_HEIGHT - 1;
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num4, num5, num6, h2);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num, num2, num3, h);
			InfoItem infoItem = (InfoItem)vEnemy.elementAt(i);
			if (infoItem.charInfo.headICON != -1)
			{
				SmallImage.drawSmallImage(g, infoItem.charInfo.headICON, num, num2, 0, 0);
			}
			else
			{
				Part part = GameScr.parts[infoItem.charInfo.head];
				SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, num + part.pi[Char.CharInfo[0][0][0]].dx, num2 + 3 + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
			}
			g.setClip(xScroll, yScroll + cmy, wScroll, hScroll);
			if (infoItem.isOnline)
			{
				mFont.tahoma_7b_green.drawString(g, infoItem.charInfo.cName, num4 + 5, num5, 0);
				mFont.tahoma_7_blue.drawString(g, infoItem.s, num4 + 5, num5 + 11, 0);
			}
			else
			{
				mFont.tahoma_7_grey.drawString(g, infoItem.charInfo.cName, num4 + 5, num5, 0);
				mFont.tahoma_7_grey.drawString(g, infoItem.s, num4 + 5, num5 + 11, 0);
			}
		}
		paintScrollArrow(g);
	}

	private void paintFriend(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		g.setColor(0);
		if (currentListLength == 0)
		{
			mFont.tahoma_7_green2.drawString(g, mResources.no_friend, xScroll + wScroll / 2, yScroll + hScroll / 2 - mFont.tahoma_7.getHeight() / 2, 2);
			return;
		}
		for (int i = 0; i < currentListLength; i++)
		{
			int num = xScroll;
			int num2 = yScroll + i * ITEM_HEIGHT;
			int num3 = 24;
			int h = ITEM_HEIGHT - 1;
			int num4 = xScroll + num3;
			int num5 = yScroll + i * ITEM_HEIGHT;
			int num6 = wScroll - num3;
			int h2 = ITEM_HEIGHT - 1;
			g.setColor((i != selected) ? 15196114 : 16383818);
			g.fillRect(num4, num5, num6, h2);
			g.setColor((i != selected) ? 9993045 : 9541120);
			g.fillRect(num, num2, num3, h);
			InfoItem infoItem = (InfoItem)vFriend.elementAt(i);
			if (infoItem.charInfo.headICON != -1)
			{
				SmallImage.drawSmallImage(g, infoItem.charInfo.headICON, num, num2, 0, 0);
			}
			else
			{
				Part part = GameScr.parts[infoItem.charInfo.head];
				SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, num + part.pi[Char.CharInfo[0][0][0]].dx, num2 + 3 + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
			}
			g.setClip(xScroll, yScroll + cmy, wScroll, hScroll);
			if (infoItem.isOnline)
			{
				mFont.tahoma_7b_green.drawString(g, infoItem.charInfo.cName, num4 + 5, num5, 0);
				mFont.tahoma_7_blue.drawString(g, infoItem.s, num4 + 5, num5 + 11, 0);
			}
			else
			{
				mFont.tahoma_7_grey.drawString(g, infoItem.charInfo.cName, num4 + 5, num5, 0);
				mFont.tahoma_7_grey.drawString(g, infoItem.s, num4 + 5, num5 + 11, 0);
			}
		}
		paintScrollArrow(g);
	}

	public void paintPlayerMenu(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < vPlayerMenu.size(); i++)
		{
			int x = xScroll;
			int num = yScroll + i * ITEM_HEIGHT;
			int num2 = wScroll - 1;
			int h = ITEM_HEIGHT - 1;
			if (num - cmy <= yScroll + hScroll && num - cmy >= yScroll - ITEM_HEIGHT)
			{
				Command command = (Command)vPlayerMenu.elementAt(i);
				g.setColor((i != selected) ? 15196114 : 16383818);
				g.fillRect(x, num, num2, h);
				if (command.caption2.Equals(string.Empty))
				{
					mFont.tahoma_7b_dark.drawString(g, command.caption, xScroll + wScroll / 2, num + 6, mFont.CENTER);
					continue;
				}
				mFont.tahoma_7b_dark.drawString(g, command.caption, xScroll + wScroll / 2, num + 1, mFont.CENTER);
				mFont.tahoma_7b_dark.drawString(g, command.caption2, xScroll + wScroll / 2, num + 11, mFont.CENTER);
			}
		}
		paintScrollArrow(g);
	}

	private void paintClans(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(-cmx, -cmy);
		g.setColor(0);
		int num = xScroll + wScroll / 2 - clansOption.Length * TAB_W / 2;
		if (currentListLength == 2)
		{
			mFont.tahoma_7_green2.drawString(g, clanReport, xScroll + wScroll / 2, yScroll + 24 + hScroll / 2 - mFont.tahoma_7.getHeight() / 2, 2);
			if (isMessage && myMember.size() == 1)
			{
				for (int i = 0; i < mResources.clanEmpty.Length; i++)
				{
					mFont.tahoma_7b_dark.drawString(g, mResources.clanEmpty[i], xScroll + wScroll / 2, yScroll + 24 + hScroll / 2 - mResources.clanEmpty.Length * 12 / 2 + i * 12, mFont.CENTER);
				}
			}
		}
		for (int j = 0; j < currentListLength; j++)
		{
			int num2 = xScroll;
			int num3 = yScroll + j * ITEM_HEIGHT;
			int num4 = 24;
			int num5 = ITEM_HEIGHT - 1;
			int num6 = xScroll + num4;
			int num7 = yScroll + j * ITEM_HEIGHT;
			int num8 = wScroll - num4;
			int num9 = ITEM_HEIGHT - 1;
			if (num7 - cmy > yScroll + hScroll || num7 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			switch (j)
			{
			case 0:
			{
				for (int k = 0; k < clansOption.Length; k++)
				{
					g.setColor((k != cSelected || j != selected) ? 15723751 : 16383818);
					g.fillRect(num + k * TAB_W, num7, TAB_W - 1, 23);
					for (int l = 0; l < clansOption[k].Length; l++)
					{
						mFont.tahoma_7_grey.drawString(g, clansOption[k][l], num + k * TAB_W + TAB_W / 2, yScroll + l * 11, mFont.CENTER);
					}
				}
				continue;
			}
			case 1:
				g.setColor((j != selected) ? 15196114 : 16383818);
				g.fillRect(xScroll, num7, wScroll, num9);
				if (clanInfo != null)
				{
					mFont.tahoma_7b_dark.drawString(g, clanInfo, xScroll + wScroll / 2, num7 + 6, mFont.CENTER);
				}
				continue;
			}
			if (isSearchClan)
			{
				if (clans == null || clans.Length == 0)
				{
					continue;
				}
				g.setColor((j != selected) ? 15196114 : 16383818);
				g.fillRect(num6, num7, num8, num9);
				g.setColor((j != selected) ? 9993045 : 9541120);
				g.fillRect(num2, num3, num4, num5);
				if (ClanImage.isExistClanImage(clans[j - 2].imgID))
				{
					if (ClanImage.getClanImage((sbyte)clans[j - 2].imgID).idImage != null)
					{
						SmallImage.drawSmallImage(g, ClanImage.getClanImage((sbyte)clans[j - 2].imgID).idImage[0], num2 + num4 / 2, num3 + num5 / 2, 0, StaticObj.VCENTER_HCENTER);
					}
				}
				else
				{
					ClanImage clanImage = new ClanImage();
					clanImage.ID = clans[j - 2].imgID;
					if (!ClanImage.isExistClanImage(clanImage.ID))
					{
						ClanImage.addClanImage(clanImage);
					}
				}
				mFont.tahoma_7b_green2.drawString(g, clans[j - 2].name, num6 + 5, num7, 0);
				g.setClip(num6, num7, num8 - 10, num9);
				mFont.tahoma_7_blue.drawString(g, clans[j - 2].slogan, num6 + 5, num7 + 11, 0);
				g.setClip(xScroll, yScroll + cmy, wScroll, hScroll);
				mFont.tahoma_7_green2.drawString(g, clans[j - 2].currMember + "/" + clans[j - 2].maxMember, num6 + num8 - 5, num7, mFont.RIGHT);
				continue;
			}
			if (isViewMember)
			{
				g.setColor((j != selected) ? 15196114 : 16383818);
				g.fillRect(num6, num7, num8, num9);
				g.setColor((j != selected) ? 9993045 : 9541120);
				g.fillRect(num2, num3, num4, num5);
				Member member = ((this.member == null) ? ((Member)myMember.elementAt(j - 2)) : ((Member)this.member.elementAt(j - 2)));
				if (member.headICON != -1)
				{
					SmallImage.drawSmallImage(g, member.headICON, num2, num3, 0, 0);
				}
				else
				{
					Part part = GameScr.parts[member.head];
					SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, num2 + part.pi[Char.CharInfo[0][0][0]].dx, num3 + 3 + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
				}
				g.setClip(xScroll, yScroll + cmy, wScroll, hScroll);
				mFont mFont2 = mFont.tahoma_7b_dark;
				if (member.role == 0)
				{
					mFont2 = mFont.tahoma_7b_red;
				}
				else if (member.role == 1)
				{
					mFont2 = mFont.tahoma_7b_green;
				}
				else if (member.role == 2)
				{
					mFont2 = mFont.tahoma_7b_green2;
				}
				mFont2.drawString(g, member.name, num6 + 5, num7, 0);
				mFont.tahoma_7_blue.drawString(g, mResources.power + ": " + member.powerPoint, num6 + 5, num7 + 11, 0);
				SmallImage.drawSmallImage(g, 7223, num6 + num8 - 7, num7 + 12, 0, 3);
				mFont.tahoma_7_blue.drawString(g, string.Empty + member.clanPoint, num6 + num8 - 15, num7 + 6, mFont.RIGHT);
				continue;
			}
			if (!isMessage || ClanMessage.vMessage.size() == 0)
			{
				continue;
			}
			ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(j - 2);
			g.setColor((j != selected || clanMessage.option != null) ? 15196114 : 16383818);
			g.fillRect(num2, num3, num8 + num4, num9);
			clanMessage.paint(g, num2, num3);
			if (clanMessage.option == null)
			{
				continue;
			}
			int num10 = xScroll + wScroll - 2 - clanMessage.option.Length * 40;
			for (int m = 0; m < clanMessage.option.Length; m++)
			{
				if (m == cSelected && j == selected)
				{
					g.drawImage(GameScr.imgLbtnFocus2, num10 + m * 40 + 20, num7 + num9 / 2, StaticObj.VCENTER_HCENTER);
					mFont.tahoma_7b_green2.drawString(g, clanMessage.option[m], num10 + m * 40 + 20, num7 + 6, mFont.CENTER);
				}
				else
				{
					g.drawImage(GameScr.imgLbtn2, num10 + m * 40 + 20, num7 + num9 / 2, StaticObj.VCENTER_HCENTER);
					mFont.tahoma_7b_dark.drawString(g, clanMessage.option[m], num10 + m * 40 + 20, num7 + 6, mFont.CENTER);
				}
			}
		}
		paintScrollArrow(g);
	}

	private void paintArchivement(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		g.setColor(0);
		if (currentListLength == 0)
		{
			mFont.tahoma_7_green2.drawString(g, mResources.no_mission, xScroll + wScroll / 2, yScroll + hScroll / 2 - mFont.tahoma_7.getHeight() / 2, 2);
		}
		else
		{
			if (Char.myCharz().arrArchive == null || Char.myCharz().arrArchive.Length != currentListLength)
			{
				return;
			}
			for (int i = 0; i < currentListLength; i++)
			{
				int num = xScroll;
				int num2 = yScroll + i * ITEM_HEIGHT;
				int num3 = wScroll;
				int num4 = ITEM_HEIGHT - 1;
				Archivement archivement = Char.myCharz().arrArchive[i];
				g.setColor((i != selected || ((archivement.isRecieve || archivement.isFinish) && (!archivement.isRecieve || !archivement.isFinish))) ? 15196114 : 16383818);
				g.fillRect(num, num2, num3, num4);
				if (archivement == null)
				{
					continue;
				}
				if (!archivement.isFinish)
				{
					mFont.tahoma_7.drawString(g, archivement.info1, num + 5, num2, 0);
					mFont.tahoma_7_green.drawString(g, archivement.money + " " + mResources.RUBY, num + num3 - 5, num2, mFont.RIGHT);
					mFont.tahoma_7_red.drawString(g, archivement.info2, num + 5, num2 + 11, 0);
				}
				else if (archivement.isFinish && !archivement.isRecieve)
				{
					mFont.tahoma_7.drawString(g, archivement.info1, num + 5, num2, 0);
					mFont.tahoma_7_blue.drawString(g, mResources.reward_mission + archivement.money + " " + mResources.RUBY, num + 5, num2 + 11, 0);
					if (i == selected)
					{
						mFont.tahoma_7b_green2.drawString(g, mResources.receive_upper, num + num3 - 20, num2 + 6, mFont.CENTER);
						mFont.tahoma_7b_dark.drawString(g, mResources.receive_upper, num + num3 - 20, num2 + 6, mFont.CENTER);
					}
					else
					{
						g.drawImage(GameScr.imgLbtn2, num + num3 - 20, num2 + num4 / 2, StaticObj.VCENTER_HCENTER);
						mFont.tahoma_7b_dark.drawString(g, mResources.receive_upper, num + num3 - 20, num2 + 6, mFont.CENTER);
					}
				}
				else if (archivement.isFinish && archivement.isRecieve)
				{
					mFont.tahoma_7_green.drawString(g, archivement.info1, num + 5, num2, 0);
					mFont.tahoma_7_green.drawString(g, archivement.info2, num + 5, num2 + 11, 0);
				}
			}
			paintScrollArrow(g);
		}
	}

	private void paintCombine(mGraphics g)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		if (vItemCombine.size() == 0)
		{
			if (combineInfo != null)
			{
				for (int i = 0; i < combineInfo.Length; i++)
				{
					mFont.tahoma_7b_dark.drawString(g, combineInfo[i], xScroll + wScroll / 2, yScroll + hScroll / 2 - combineInfo.Length * 14 / 2 + i * 14 + 5, 2);
				}
			}
			return;
		}
		for (int j = 0; j < vItemCombine.size() + 1; j++)
		{
			int num = xScroll + 36;
			int num2 = yScroll + j * ITEM_HEIGHT;
			int num3 = wScroll - 36;
			int num4 = ITEM_HEIGHT - 1;
			int num5 = xScroll;
			int num6 = yScroll + j * ITEM_HEIGHT;
			int num7 = 34;
			int num8 = ITEM_HEIGHT - 1;
			if (num2 - cmy > yScroll + hScroll || num2 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			if (j == vItemCombine.size())
			{
				if (vItemCombine.size() > 0)
				{
					if (!GameCanvas.isTouch && j == selected)
					{
						g.setColor(16383818);
						g.fillRect(num5, num2, wScroll, num4 + 2);
					}
					if ((j == selected && keyTouchCombine == 1) || (!GameCanvas.isTouch && j == selected))
					{
						g.drawImage(GameScr.imgLbtnFocus, xScroll + wScroll / 2, num2 + num4 / 2 + 1, StaticObj.VCENTER_HCENTER);
						mFont.tahoma_7b_green2.drawString(g, mResources.UPGRADE, xScroll + wScroll / 2, num2 + num4 / 2 - 4, mFont.CENTER);
					}
					else
					{
						g.drawImage(GameScr.imgLbtn, xScroll + wScroll / 2, num2 + num4 / 2 + 1, StaticObj.VCENTER_HCENTER);
						mFont.tahoma_7b_dark.drawString(g, mResources.UPGRADE, xScroll + wScroll / 2, num2 + num4 / 2 - 4, mFont.CENTER);
					}
				}
				continue;
			}
			g.setColor((j != selected) ? 15196114 : 16383818);
			g.fillRect(num, num2, num3, num4);
			g.setColor((j != selected) ? 9993045 : 9541120);
			Item item = (Item)vItemCombine.elementAt(j);
			if (item != null)
			{
				for (int k = 0; k < item.itemOption.Length; k++)
				{
					if (item.itemOption[k].optionTemplate.id == 72 && item.itemOption[k].param > 0)
					{
						sbyte color_Item_Upgrade = GetColor_Item_Upgrade(item.itemOption[k].param);
						int color_ItemBg = GetColor_ItemBg(color_Item_Upgrade);
						if (color_ItemBg != -1)
						{
							g.setColor((j != selected) ? GetColor_ItemBg(color_Item_Upgrade) : GetColor_ItemBg(color_Item_Upgrade));
						}
					}
				}
			}
			g.fillRect(num5, num6, num7, num8);
			if (item == null)
			{
				continue;
			}
			string text = string.Empty;
			if (item.itemOption != null)
			{
				for (int l = 0; l < item.itemOption.Length; l++)
				{
					if (item.itemOption[l].optionTemplate.id == 72)
					{
						text = " [+" + item.itemOption[l].param + "]";
					}
				}
			}
			mFont.tahoma_7_green2.drawString(g, item.template.name + text, num + 5, num2 + 1, 0);
			string text2 = string.Empty;
			if (item.itemOption != null)
			{
				if (item.itemOption.Length > 0 && item.itemOption[0] != null && item.itemOption[0].optionTemplate.id != 102 && item.itemOption[0].optionTemplate.id != 107)
				{
					text2 += item.itemOption[0].getOptionString();
				}
				mFont mFont2 = mFont.tahoma_7_blue;
				if (item.compare < 0 && item.template.type != 5)
				{
					mFont2 = mFont.tahoma_7_red;
				}
				if (item.itemOption.Length > 1)
				{
					for (int m = 1; m < item.itemOption.Length; m++)
					{
						if (item.itemOption[m] != null && item.itemOption[m].optionTemplate.id != 102 && item.itemOption[m].optionTemplate.id != 107)
						{
							text2 = text2 + "," + item.itemOption[m].getOptionString();
						}
					}
				}
				g.setClip(num, num2, num3, num4);
				mFont2.drawString(g, text2, num + 5, num2 + 11, mFont.LEFT);
				g.setClip(xScroll, yScroll, wScroll, hScroll);
			}
			SmallImage.drawSmallImage(g, item.template.iconID, num5 + num7 / 2 - ((item.quantity > 1) ? 8 : 0), num6 + num8 / 2, 0, 3);
			if (item.itemOption != null)
			{
				for (int n = 0; n < item.itemOption.Length; n++)
				{
					paintOptItem(g, item.itemOption[n].optionTemplate.id, item.itemOption[n].param, num5, num6, num7, num8);
				}
				for (int num9 = 0; num9 < item.itemOption.Length; num9++)
				{
					paintOptSlotItem(g, item.itemOption[num9].optionTemplate.id, item.itemOption[num9].param, num5, num6, num7, num8);
				}
			}
			if (item.quantity > 1)
			{
				mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num5 + num7 - 15, num6 + 6, 0);
			}
		}
		paintScrollArrow(g);
	}

	private void paintInventory(mGraphics g)
	{
		g.setColor(16711680);
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		Item[] arrItemBody = Char.myCharz().arrItemBody;
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		for (int i = 0; i < arrItemBody.Length + arrItemBag.Length; i++)
		{
			bool flag = i < arrItemBody.Length;
			int num = i;
			int num2 = i - arrItemBody.Length;
			int num3 = xScroll + 36;
			int num4 = yScroll + i * ITEM_HEIGHT;
			int num5 = wScroll - 36;
			int h = ITEM_HEIGHT - 1;
			int num6 = xScroll;
			int num7 = yScroll + i * ITEM_HEIGHT;
			int num8 = 34;
			int num9 = ITEM_HEIGHT - 1;
			if (num4 - cmy > yScroll + hScroll || num4 - cmy < yScroll - ITEM_HEIGHT)
			{
				continue;
			}
			Item item = ((!flag) ? arrItemBag[num2] : arrItemBody[num]);
			g.setColor((i == selected) ? 16383818 : ((!flag) ? 15723751 : 15196114));
			g.fillRect(num3, num4, num5, h);
			g.setColor((i == selected) ? 9541120 : ((!flag) ? 11837316 : 9993045));
			if (item != null)
			{
				for (int j = 0; j < item.itemOption.Length; j++)
				{
					if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
					{
						sbyte color_Item_Upgrade = GetColor_Item_Upgrade(item.itemOption[j].param);
						int color_ItemBg = GetColor_ItemBg(color_Item_Upgrade);
						if (color_ItemBg != -1)
						{
							g.setColor((i != selected) ? GetColor_ItemBg(color_Item_Upgrade) : GetColor_ItemBg(color_Item_Upgrade));
						}
					}
				}
			}
			g.fillRect(num6, num7, num8, num9);
			if (item != null && item.isSelect && GameCanvas.panel.type == 12)
			{
				g.setColor((i != selected) ? 6047789 : 7040779);
				g.fillRect(num6, num7, num8, num9);
			}
			if (item == null)
			{
				continue;
			}
			string text = string.Empty;
			if (item.itemOption != null)
			{
				for (int k = 0; k < item.itemOption.Length; k++)
				{
					if (item.itemOption[k].optionTemplate.id == 72)
					{
						text = " [+" + item.itemOption[k].param + "]";
					}
				}
			}
			mFont.tahoma_7_green2.drawString(g, item.template.name + text, num3 + 5, num4 + 1, 0);
			string text2 = string.Empty;
			if (item.itemOption != null)
			{
				if (item.itemOption.Length > 0 && item.itemOption[0] != null && item.itemOption[0].optionTemplate.id != 102 && item.itemOption[0].optionTemplate.id != 107)
				{
					text2 += item.itemOption[0].getOptionString();
				}
				mFont mFont2 = mFont.tahoma_7_blue;
				if (item.compare < 0 && item.template.type != 5)
				{
					mFont2 = mFont.tahoma_7_red;
				}
				if (item.itemOption.Length > 1)
				{
					for (int l = 1; l < 2; l++)
					{
						if (item.itemOption[l] != null && item.itemOption[l].optionTemplate.id != 102 && item.itemOption[l].optionTemplate.id != 107)
						{
							text2 = text2 + "," + item.itemOption[l].getOptionString();
						}
					}
				}
				mFont2.drawString(g, text2, num3 + 5, num4 + 11, mFont.LEFT);
			}
			SmallImage.drawSmallImage(g, item.template.iconID, num6 + num8 / 2, num7 + num9 / 2, 0, 3);
			if (item.itemOption != null)
			{
				for (int m = 0; m < item.itemOption.Length; m++)
				{
					paintOptItem(g, item.itemOption[m].optionTemplate.id, item.itemOption[m].param, num6, num7, num8, num9);
				}
				for (int n = 0; n < item.itemOption.Length; n++)
				{
					paintOptSlotItem(g, item.itemOption[n].optionTemplate.id, item.itemOption[n].param, num6, num7, num8, num9);
				}
			}
			if (item.quantity > 1)
			{
				mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num6 + num8, num7 + num9 - mFont.tahoma_7_yellow.getHeight(), 1);
			}
			mFont.tahoma_7_yellow.drawStringBorder(g, i.ToString(), num3 - 35, num4, 0, mFont.tahoma_7b_dark);
		}
		paintScrollArrow(g);
	}

	private void paintTab(mGraphics g)
	{
		if (type == 23 || type == 24)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.gameInfo, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 20)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.account, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 22)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.autoFunction, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 19)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.option, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 18)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.change_flag, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 13 && Equals(GameCanvas.panel2))
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.item_receive2, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 12 && GameCanvas.panel2 != null)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.UPGRADE, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 11)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.friend, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 16)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.enemy, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 15)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, topName, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 2 && GameCanvas.panel2 != null)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.chest, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 9)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.achievement_mission, xScroll + wScroll / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 3)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.select_zone, startTabPos + TAB_W / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 14)
		{
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			mFont.tahoma_7b_dark.drawString(g, mResources.select_map, startTabPos + TAB_W / 2, 59, mFont.CENTER);
			return;
		}
		if (type == 4)
		{
			mFont.tahoma_7b_dark.drawString(g, mResources.map, startTabPos + TAB_W / 2, 59, mFont.CENTER);
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			return;
		}
		if (type == 7)
		{
			mFont.tahoma_7b_dark.drawString(g, mResources.trangbi, startTabPos + TAB_W / 2, 59, mFont.CENTER);
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			return;
		}
		if (type == 17)
		{
			mFont.tahoma_7b_dark.drawString(g, mResources.kigui, startTabPos + TAB_W / 2, 59, mFont.CENTER);
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			return;
		}
		if (type == 8)
		{
			mFont.tahoma_7b_dark.drawString(g, mResources.msg, startTabPos + TAB_W / 2, 59, mFont.CENTER);
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			return;
		}
		if (type == 10)
		{
			mFont.tahoma_7b_dark.drawString(g, mResources.wat_do_u_want, startTabPos + TAB_W / 2, 59, mFont.CENTER);
			g.setColor(13524492);
			g.fillRect(X + 1, 78, W - 2, 1);
			return;
		}
		if (currentTabIndex == 3 && mainTabName.Length != 4)
		{
			g.translate(-cmx, 0);
		}
		for (int i = 0; i < currentTabName.Length; i++)
		{
			g.setColor((i != currentTabIndex) ? 16773296 : 6805896);
			PopUp.paintPopUp(g, startTabPos + i * TAB_W, 52, TAB_W - 1, 25, (i == currentTabIndex) ? 1 : 0, isButton: true);
			if (i == keyTouchTab)
			{
				g.drawImage(ItemMap.imageFlare, startTabPos + i * TAB_W + TAB_W / 2, 62, 3);
			}
			mFont mFont2 = ((i != currentTabIndex) ? mFont.tahoma_7_grey : mFont.tahoma_7_green2);
			if (!currentTabName[i][1].Equals(string.Empty))
			{
				mFont2.drawString(g, currentTabName[i][0], startTabPos + i * TAB_W + TAB_W / 2, 53, mFont.CENTER);
				mFont2.drawString(g, currentTabName[i][1], startTabPos + i * TAB_W + TAB_W / 2, 64, mFont.CENTER);
			}
			else
			{
				mFont2.drawString(g, currentTabName[i][0], startTabPos + i * TAB_W + TAB_W / 2, 59, mFont.CENTER);
			}
			if (type == 0 && currentTabName.Length == 5 && GameScr.isNewClanMessage && GameCanvas.gameTick % 4 == 0)
			{
				g.drawImage(ItemMap.imageFlare, startTabPos + 3 * TAB_W + TAB_W / 2, 77, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
		}
		g.setColor(13524492);
		g.fillRect(1, 78, W - 2, 1);
	}

	private void paintBottomMoneyInfo(mGraphics g)
	{
		if (type != 13 || (currentTabIndex != 2 && !Equals(GameCanvas.panel2)))
		{
			g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
			g.setColor(11837316);
			g.fillRect(X + 1, H - 15, W - 2, 14);
			g.setColor(13524492);
			g.fillRect(X + 1, H - 15, W - 2, 1);
			g.drawImage(imgXu, X + 11, H - 7, 3);
			g.drawImage(imgLuong, X + 75, H - 8, 3);
			mFont.tahoma_7_yellow.drawString(g, NinjaUtil.getMoneys(Char.myCharz().xu) + string.Empty, X + 24, H - 13, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, NinjaUtil.getMoneys(Char.myCharz().luong) + string.Empty, X + 85, H - 13, mFont.LEFT, mFont.tahoma_7_grey);
			g.drawImage(imgLuongKhoa, X + 130, H - 8, 3);
			mFont.tahoma_7_yellow.drawString(g, NinjaUtil.getMoneys(Char.myCharz().luongKhoa) + string.Empty, X + 140, H - 13, mFont.LEFT, mFont.tahoma_7_grey);
		}
	}

	private void paintClanInfo(mGraphics g)
	{
		if (Char.myCharz().clan == null)
		{
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), 25, 50, 0, 33);
			mFont.tahoma_7b_white.drawString(g, mResources.not_join_clan, (wScroll - 50) / 2 + 50, 20, mFont.CENTER);
		}
		else if (!isViewMember)
		{
			Clan clan = Char.myCharz().clan;
			if (clan != null)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), 25, 50, 0, 33);
				mFont.tahoma_7b_white.drawString(g, clan.name, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
				mFont.tahoma_7_yellow.drawString(g, mResources.achievement_point + ": " + clan.powerPoint, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
				mFont.tahoma_7_yellow.drawString(g, mResources.clan_point + ": " + clan.clanPoint, 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
				mFont.tahoma_7_yellow.drawString(g, mResources.level + ": " + clan.level, 60, 38, mFont.LEFT, mFont.tahoma_7_grey);
				TextInfo.paint(g, clan.slogan, 60, 38, wScroll - 70, ITEM_HEIGHT, mFont.tahoma_7_yellow);
			}
		}
		else
		{
			Clan clan2 = ((currClan == null) ? Char.myCharz().clan : currClan);
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), 25, 51, 0, 33);
			mFont.tahoma_7b_white.drawString(g, clan2.name, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
			mFont.tahoma_7_yellow.drawString(g, mResources.member + ": " + clan2.currMember + "/" + clan2.maxMember, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, mResources.clan_leader + ": " + clan2.leaderName, 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
			TextInfo.paint(g, clan2.slogan, 60, 38, wScroll - 70, ITEM_HEIGHT, mFont.tahoma_7_yellow);
		}
	}

	private void paintToolInfo(mGraphics g)
	{
		mFont.tahoma_7b_white.drawString(g, mResources.dragon_ball + " " + GameMidlet.VERSION, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
		mFont.tahoma_7_yellow.drawString(g, mResources.character + ": " + Char.myCharz().cName, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.account_server + " " + ServerListScreen.nameServer[ServerListScreen.ipSelect] + ":", 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, (!GameCanvas.loginScr.tfUser.getText().Equals(string.Empty)) ? GameCanvas.loginScr.tfUser.getText() : mResources.not_register_yet, 60, 39, mFont.LEFT, mFont.tahoma_7_grey);
	}

	private void paintGiaoDichInfo(mGraphics g)
	{
		mFont.tahoma_7_yellow.drawString(g, mResources.select_item, 60, 4, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.lock_trade, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.wait_opp_lock_trade, 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.press_done, 60, 38, mFont.LEFT, mFont.tahoma_7_grey);
	}

	private void paintMyInfo(mGraphics g)
	{
		paintCharInfo(g, Char.myCharz());
	}

	private void paintPetInfo(mGraphics g)
	{
		mFont.tahoma_7_yellow.drawString(g, mResources.power + ": " + NinjaUtil.getMoneys(Char.myPetz().cPower), X + 60, 4, mFont.LEFT, mFont.tahoma_7_grey);
		if (Char.myPetz().cPower > 0)
		{
			mFont.tahoma_7_yellow.drawString(g, (!Char.myPetz().me) ? Char.myPetz().currStrLevel : Char.myPetz().getStrLevel(), X + 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
		}
		if (Char.myPetz().cDamFull > 0)
		{
			mFont.tahoma_7_yellow.drawString(g, mResources.hit_point + " :" + Char.myPetz().cDamFull, X + 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
		}
		if (Char.myPetz().cMaxStamina > 0)
		{
			mFont.tahoma_7_yellow.drawString(g, mResources.vitality, X + 60, 38, mFont.LEFT, mFont.tahoma_7_grey);
			g.drawImage(GameScr.imgMPLost, X + 100, 41, 0);
			int num = Char.myPetz().cStamina * mGraphics.getImageWidth(GameScr.imgMP) / Char.myPetz().cMaxStamina;
			g.setClip(100, X + 41, num, 20);
			g.drawImage(GameScr.imgMP, X + 100, 41, 0);
		}
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
	}

	private void paintCharInfo(mGraphics g, Char c)
	{
		mFont.tahoma_7b_white.drawString(g, ((GameScr.isNewMember == 1) ? "       " : string.Empty) + c.cName, X + 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
		if (GameScr.isNewMember == 1)
		{
			SmallImage.drawSmallImage(g, 5427, X + 55, 4, 0, 0);
		}
		if (c.cMaxStamina > 0)
		{
			mFont.tahoma_7_yellow.drawString(g, mResources.vitality, X + 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
			g.drawImage(GameScr.imgMPLost, X + 95, 19, 0);
			int num = c.cStamina * mGraphics.getImageWidth(GameScr.imgMP) / c.cMaxStamina;
			g.setClip(95, X + 19, num, 20);
			g.drawImage(GameScr.imgMP, X + 95, 19, 0);
		}
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		if (c.cPower > 0)
		{
			mFont.tahoma_7_yellow.drawString(g, (!c.me) ? c.currStrLevel : c.getStrLevel(), X + 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
		}
		mFont.tahoma_7_yellow.drawString(g, mResources.power + ": " + NinjaUtil.getMoneys(c.cPower), X + 60, 38, mFont.LEFT, mFont.tahoma_7_grey);
	}

	private void paintZoneInfo(mGraphics g)
	{
		mFont.tahoma_7b_white.drawString(g, mResources.zone + " " + TileMap.zoneID, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
		mFont.tahoma_7_yellow.drawString(g, TileMap.mapName, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7b_white.drawString(g, TileMap.zoneID + string.Empty, 25, 27, mFont.CENTER);
	}

	public int getCompare(Item item)
	{
		if (item == null)
		{
			return -1;
		}
		if (item.isTypeBody())
		{
			if (item.itemOption == null)
			{
				return -1;
			}
			ItemOption itemOption = item.itemOption[0];
			if (itemOption.optionTemplate.id == 22)
			{
				itemOption.optionTemplate = GameScr.gI().iOptionTemplates[6];
				itemOption.param *= 1000;
			}
			if (itemOption.optionTemplate.id == 23)
			{
				itemOption.optionTemplate = GameScr.gI().iOptionTemplates[7];
				itemOption.param *= 1000;
			}
			Item item2 = null;
			for (int i = 0; i < Char.myCharz().arrItemBody.Length; i++)
			{
				Item item3 = Char.myCharz().arrItemBody[i];
				if (itemOption.optionTemplate.id == 22)
				{
					itemOption.optionTemplate = GameScr.gI().iOptionTemplates[6];
					itemOption.param *= 1000;
				}
				if (itemOption.optionTemplate.id == 23)
				{
					itemOption.optionTemplate = GameScr.gI().iOptionTemplates[7];
					itemOption.param *= 1000;
				}
				if (item3 != null && item3.itemOption != null && item3.template.type == item.template.type)
				{
					item2 = item3;
					break;
				}
			}
			if (item2 == null)
			{
				Res.outz("5");
				isUp = true;
				return itemOption.param;
			}
			int num = 0;
			num = ((item2 == null || item2.itemOption == null) ? itemOption.param : (itemOption.param - item2.itemOption[0].param));
			if (num < 0)
			{
				isUp = false;
			}
			else
			{
				isUp = true;
			}
			return num;
		}
		return 0;
	}

	private void paintMapInfo(mGraphics g)
	{
		mFont.tahoma_7b_white.drawString(g, mResources.MENUGENDER[TileMap.planetID], 60, 4, mFont.LEFT);
		string text = string.Empty;
		if (TileMap.mapID >= 135 && TileMap.mapID <= 138)
		{
			text = " " + mResources.tang + TileMap.zoneID;
		}
		mFont.tahoma_7_yellow.drawString(g, TileMap.mapName + text, 60, 16, mFont.LEFT);
		mFont.tahoma_7b_white.drawString(g, mResources.quest_place + ": ", 60, 27, mFont.LEFT);
		if (GameScr.getTaskMapId() >= 0 && GameScr.getTaskMapId() <= TileMap.mapNames.Length - 1)
		{
			mFont.tahoma_7_yellow.drawString(g, TileMap.mapNames[GameScr.getTaskMapId()], 60, 38, mFont.LEFT);
		}
		else
		{
			mFont.tahoma_7_yellow.drawString(g, mResources.random, 60, 38, mFont.LEFT);
		}
	}

	private void paintShopInfo(mGraphics g)
	{
		if (currentTabIndex == currentTabName.Length - 1 && GameCanvas.panel2 == null)
		{
			paintMyInfo(g);
		}
		else if (selected < 0)
		{
			if (typeShop != 2)
			{
				mFont.tahoma_7_white.drawString(g, mResources.say_hello, X + 60, 14, 0);
				mFont.tahoma_7_white.drawString(g, strWantToBuy, X + 60, 26, 0);
				return;
			}
			mFont.tahoma_7_white.drawString(g, mResources.say_hello, X + 60, 5, 0);
			mFont.tahoma_7_white.drawString(g, strWantToBuy, X + 60, 17, 0);
			mFont.tahoma_7_white.drawString(g, mResources.page + " " + (currPageShop[currentTabIndex] + 1) + "/" + maxPageShop[currentTabIndex], X + 60, 29, 0);
		}
		else
		{
			if (currentTabIndex < 0 || currentTabIndex > Char.myCharz().arrItemShop.Length - 1 || selected < 0 || selected > Char.myCharz().arrItemShop[currentTabIndex].Length - 1)
			{
				return;
			}
			Item item = Char.myCharz().arrItemShop[currentTabIndex][selected];
			if (item != null)
			{
				if (Equals(GameCanvas.panel) && currentTabIndex <= 3 && typeShop == 2)
				{
					mFont.tahoma_7b_white.drawString(g, mResources.page + " " + (currPageShop[currentTabIndex] + 1) + "/" + maxPageShop[currentTabIndex], X + 55, 4, 0);
				}
				mFont.tahoma_7b_white.drawString(g, item.template.name, X + 55, 24, 0);
				string st = mResources.pow_request + " " + Res.formatNumber(item.template.strRequire);
				if (item.template.strRequire > Char.myCharz().cPower)
				{
					mFont.tahoma_7_yellow.drawString(g, st, X + 55, 35, 0);
				}
				else
				{
					mFont.tahoma_7_green.drawString(g, st, X + 55, 35, 0);
				}
			}
		}
	}

	private void paintItemBoxInfo(mGraphics g)
	{
		string st = mResources.used + ": " + hasUse + "/" + Char.myCharz().arrItemBox.Length + " " + mResources.place;
		mFont.tahoma_7b_white.drawString(g, mResources.chest, 60, 4, 0);
		mFont.tahoma_7_yellow.drawString(g, st, 60, 16, 0);
	}

	private void paintSkillInfo(mGraphics g)
	{
		mFont.tahoma_7_white.drawString(g, "Top " + Char.myCharz().rank, X + 45 + (W - 50) / 2, 2, mFont.CENTER);
		mFont.tahoma_7_yellow.drawString(g, mResources.potential_point, X + 45 + (W - 50) / 2, 14, mFont.CENTER);
		mFont.tahoma_7_white.drawString(g, string.Empty + NinjaUtil.getMoneys(Char.myCharz().cTiemNang), X + ((GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0) + 45 + (W - 50) / 2, 26, mFont.CENTER);
		mFont.tahoma_7_yellow.drawString(g, mResources.active_point + ": " + NinjaUtil.getMoneys(Char.myCharz().cNangdong), X + ((GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0) + 45 + (W - 50) / 2, 38, mFont.CENTER);
	}

	private void paintItemBodyBagInfo(mGraphics g)
	{
		mFont.tahoma_7_yellow.drawString(g, mResources.HP + ": " + Char.myCharz().cHP + " / " + Char.myCharz().cHPFull, X + 60, 2, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.KI + ": " + Char.myCharz().cMP + " / " + Char.myCharz().cMPFull, X + 60, 14, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.hit_point + ": " + Char.myCharz().cDamFull, X + 60, 26, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.armor + ": " + Char.myCharz().cDefull + ", " + mResources.critical + ": " + Char.myCharz().cCriticalFull + "%", X + 60, 38, mFont.LEFT, mFont.tahoma_7_grey);
	}

	private void paintTopInfo(mGraphics g)
	{
		g.setClip(X + 1, Y, W - 2, yScroll - 2);
		g.setColor(9993045);
		g.fillRect(X, Y, W - 2, 50);
		switch (type)
		{
		case 13:
			if (currentTabIndex == 0 || currentTabIndex == 1)
			{
				if (Equals(GameCanvas.panel))
				{
					SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
					paintGiaoDichInfo(g);
				}
				if (Equals(GameCanvas.panel2) && charMenu != null)
				{
					SmallImage.drawSmallImage(g, charMenu.avatarz(), X + 25, 50, 0, 33);
					paintCharInfo(g, charMenu);
				}
			}
			if (currentTabIndex == 2 && charMenu != null)
			{
				SmallImage.drawSmallImage(g, charMenu.avatarz(), X + 25, 50, 0, 33);
				paintCharInfo(g, charMenu);
			}
			break;
		case 12:
			if (currentTabIndex == 0)
			{
				SmallImage.drawSmallImage(g, 1410, X + 25, 50, 0, 33);
				paintCombineInfo(g);
			}
			if (currentTabIndex == 1)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
				paintMyInfo(g);
			}
			break;
		case 11:
		case 16:
		case 23:
		case 24:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMyInfo(g);
			break;
		case 15:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMyInfo(g);
			break;
		case 9:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMyInfo(g);
			break;
		case 21:
			if (currentTabIndex == 0)
			{
				SmallImage.drawSmallImage(g, Char.myPetz().avatarz(), X + 25, 50, 0, 33);
				paintPetInfo(g);
			}
			if (currentTabIndex == 1)
			{
				SmallImage.drawSmallImage(g, Char.myPetz().avatarz(), X + 25, 50, 0, 33);
				paintPetStatusInfo(g);
			}
			if (currentTabIndex == 2)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
				paintItemBodyBagInfo(g);
			}
			break;
		case 0:
			if (currentTabIndex == 0)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
				paintMyInfo(g);
			}
			if (currentTabIndex == 1)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
				paintItemBodyBagInfo(g);
			}
			if (currentTabIndex == 2)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
				paintSkillInfo(g);
			}
			if (currentTabIndex == 3)
			{
				if (mainTabName.Length == 5)
				{
					paintClanInfo(g);
				}
				else
				{
					SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
					paintToolInfo(g);
				}
			}
			if (currentTabIndex == 4)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
				paintToolInfo(g);
			}
			break;
		case 25:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMyInfo(g);
			break;
		case 2:
			if (currentTabIndex == 0)
			{
				SmallImage.drawSmallImage(g, 526, X + 25, 50, 0, 33);
				paintItemBoxInfo(g);
			}
			if (currentTabIndex == 1)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
				paintItemBodyBagInfo(g);
			}
			break;
		case 3:
			SmallImage.drawSmallImage(g, 561, X + 25, 50, 0, 33);
			paintZoneInfo(g);
			break;
		case 1:
			if (currentTabIndex == currentTabName.Length - 1 && GameCanvas.panel2 == null)
			{
				SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			}
			else
			{
				SmallImage.drawSmallImage(g, Char.myCharz().npcFocus.avatar, X + 25, 50, 0, 33);
			}
			paintShopInfo(g);
			break;
		case 4:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMapInfo(g);
			break;
		case 7:
		case 17:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMyInfo(g);
			break;
		case 8:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMyInfo(g);
			break;
		case 10:
			if (charMenu != null)
			{
				SmallImage.drawSmallImage(g, charMenu.avatarz(), X + 25, 50, 0, 33);
				paintCharInfo(g, charMenu);
			}
			break;
		case 14:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMapInfo(g);
			break;
		case 18:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintMyInfo(g);
			break;
		case 19:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintToolInfo(g);
			break;
		case 20:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintToolInfo(g);
			break;
		case 22:
			SmallImage.drawSmallImage(g, Char.myCharz().avatarz(), X + 25, 50, 0, 33);
			paintToolInfo(g);
			break;
		case 5:
		case 6:
			break;
		}
	}

	private string getStatus(int status)
	{
		return status switch
		{
			0 => mResources.follow, 
			1 => mResources.defend, 
			2 => mResources.attack, 
			3 => mResources.gohome, 
			_ => "aaa", 
		};
	}

	private void paintPetStatusInfo(mGraphics g)
	{
		mFont.tahoma_7b_white.drawString(g, "HP: " + Char.myPetz().cHP + "/" + Char.myPetz().cHPFull, X + 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
		mFont.tahoma_7b_white.drawString(g, "MP: " + Char.myPetz().cMP + "/" + Char.myPetz().cMPFull, X + 60, 16, mFont.LEFT, mFont.tahoma_7b_dark);
		mFont.tahoma_7_yellow.drawString(g, mResources.critical + ": " + Char.myPetz().cCriticalFull + "   " + mResources.armor + ": " + Char.myPetz().cDefull, X + 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
		mFont.tahoma_7_yellow.drawString(g, mResources.status + " :" + strStatus[Char.myPetz().petStatus], X + 60, 38, mFont.LEFT, mFont.tahoma_7_grey);
	}

	private void paintCombineInfo(mGraphics g)
	{
		if (combineTopInfo != null)
		{
			for (int i = 0; i < combineTopInfo.Length; i++)
			{
				mFont.tahoma_7_white.drawString(g, combineTopInfo[i], X + 45 + (W - 50) / 2, 5 + i * 14, mFont.CENTER);
			}
		}
	}

	private void paintInfomation(mGraphics g)
	{
	}

	public void paintMap(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(-cmxMap, -cmyMap);
		g.drawImage(imgMap, xScroll, yScroll, 0);
		int head = Char.myCharz().head;
		Part part = GameScr.parts[head];
		SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, xMap, yMap + 5, 0, 3);
		int align = mFont.CENTER;
		if (xMap <= 40)
		{
			align = mFont.LEFT;
		}
		if (xMap >= 220)
		{
			align = mFont.RIGHT;
		}
		mFont.tahoma_7b_yellow.drawString(g, TileMap.mapName, xMap, yMap - 12, align, mFont.tahoma_7_grey);
		int num = -1;
		if (GameScr.getTaskMapId() != -1)
		{
			for (int i = 0; i < mapId[TileMap.planetID].Length; i++)
			{
				if (mapId[TileMap.planetID][i] == GameScr.getTaskMapId())
				{
					num = i;
					break;
				}
				num = 4;
			}
			if (GameCanvas.gameTick % 4 > 0)
			{
				g.drawImage(ItemMap.imageFlare, xScroll + mapX[TileMap.planetID][num], yScroll + mapY[TileMap.planetID][num], 3);
			}
		}
		if (!GameCanvas.isTouch)
		{
			g.drawImage(imgBantay, xMove, yMove, StaticObj.TOP_RIGHT);
			for (int j = 0; j < mapX[TileMap.planetID].Length; j++)
			{
				int num2 = mapX[TileMap.planetID][j] + xScroll;
				int num3 = mapY[TileMap.planetID][j] + yScroll;
				if (Res.inRect(num2 - 15, num3 - 15, 30, 30, xMove, yMove))
				{
					align = mFont.CENTER;
					if (num2 <= 20)
					{
						align = mFont.LEFT;
					}
					if (num2 >= 220)
					{
						align = mFont.RIGHT;
					}
					mFont.tahoma_7b_yellow.drawString(g, TileMap.mapNames[mapId[TileMap.planetID][j]], num2, num3 - 12, align, mFont.tahoma_7_grey);
					break;
				}
			}
		}
		else if (!trans)
		{
			for (int k = 0; k < mapX[TileMap.planetID].Length; k++)
			{
				int num4 = mapX[TileMap.planetID][k] + xScroll;
				int num5 = mapY[TileMap.planetID][k] + yScroll;
				if (Res.inRect(num4 - 15, num5 - 15, 30, 30, pX, pY))
				{
					align = mFont.CENTER;
					if (num4 <= 30)
					{
						align = mFont.LEFT;
					}
					if (num4 >= 220)
					{
						align = mFont.RIGHT;
					}
					g.drawImage(imgBantay, num4, num5, StaticObj.TOP_RIGHT);
					mFont.tahoma_7b_yellow.drawString(g, TileMap.mapNames[mapId[TileMap.planetID][k]], num4, num5 - 12, align, mFont.tahoma_7_grey);
					break;
				}
			}
		}
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		if (num != -1)
		{
			if (mapX[TileMap.planetID][num] + xScroll < cmxMap)
			{
				g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 5, xScroll + 5, yScroll + hScroll / 2 - 4, 0);
			}
			if (cmxMap + wScroll < mapX[TileMap.planetID][num] + xScroll)
			{
				g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 6, xScroll + wScroll - 5, yScroll + hScroll / 2 - 4, StaticObj.TOP_RIGHT);
			}
			if (mapY[TileMap.planetID][num] < cmyMap)
			{
				g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 1, xScroll + wScroll / 2, yScroll + 5, StaticObj.TOP_CENTER);
			}
			if (mapY[TileMap.planetID][num] > cmyMap + hScroll)
			{
				g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, xScroll + wScroll / 2, yScroll + hScroll - 5, StaticObj.BOTTOM_HCENTER);
			}
		}
	}

	public void paintTask(mGraphics g)
	{
		int num = ((GameCanvas.h <= 300) ? 15 : 20);
		if (isPaintMap && !GameScr.gI().isMapDocNhan() && !GameScr.gI().isMapFize())
		{
			g.drawImage((keyTouchMapButton != 1) ? GameScr.imgLbtn : GameScr.imgLbtnFocus, xScroll + wScroll / 2, yScroll + hScroll - num, 3);
			mFont.tahoma_7b_dark.drawString(g, mResources.map, xScroll + wScroll / 2, yScroll + hScroll - (num + 5), mFont.CENTER);
		}
		xstart = xScroll + 5;
		ystart = yScroll + 14;
		yPaint = ystart;
		g.setClip(xScroll, yScroll, wScroll, hScroll - 35);
		if (scroll != null)
		{
			if (scroll.cmy > 0)
			{
				g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 1, xScroll + wScroll - 12, yScroll + 3, 0);
			}
			if (scroll.cmy < scroll.cmyLim)
			{
				g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, xScroll + wScroll - 12, yScroll + hScroll - 45, 0);
			}
			g.translate(0, -scroll.cmy);
		}
		indexRowMax = 0;
		if (indexMenu == 0)
		{
			bool flag = false;
			if (Char.myCharz().taskMaint != null)
			{
				for (int i = 0; i < Char.myCharz().taskMaint.names.Length; i++)
				{
					mFont.tahoma_7_grey.drawString(g, Char.myCharz().taskMaint.names[i], xScroll + wScroll / 2, yPaint - 5 + i * 12, mFont.CENTER);
					indexRowMax++;
				}
				yPaint += (Char.myCharz().taskMaint.names.Length - 1) * 12;
				int num2 = 0;
				string empty = string.Empty;
				for (int j = 0; j < Char.myCharz().taskMaint.subNames.Length; j++)
				{
					if (Char.myCharz().taskMaint.subNames[j] != null)
					{
						num2 = j;
						empty = "- " + Char.myCharz().taskMaint.subNames[j];
						if (Char.myCharz().taskMaint.counts[j] != -1)
						{
							if (Char.myCharz().taskMaint.index == j)
							{
								if (Char.myCharz().taskMaint.counts[j] != 1)
								{
									string text = empty;
									empty = text + " (" + Char.myCharz().taskMaint.count + "/" + Char.myCharz().taskMaint.counts[j] + ")";
								}
								if (Char.myCharz().taskMaint.count == Char.myCharz().taskMaint.counts[j])
								{
									mFont.tahoma_7.drawString(g, empty, xstart + 5, yPaint += 12, 0);
								}
								else
								{
									mFont tahoma_7_grey = mFont.tahoma_7_grey;
									if (!flag)
									{
										flag = true;
										tahoma_7_grey = mFont.tahoma_7_blue;
										tahoma_7_grey.drawString(g, empty, xstart + 5 + ((tahoma_7_grey == mFont.tahoma_7_blue && GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), yPaint += 12, 0);
									}
									else
									{
										tahoma_7_grey.drawString(g, "- ...", xstart + 5 + ((tahoma_7_grey == mFont.tahoma_7_blue && GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), yPaint += 12, 0);
									}
								}
							}
							else if (Char.myCharz().taskMaint.index > j)
							{
								if (Char.myCharz().taskMaint.counts[j] != 1)
								{
									string text = empty;
									empty = text + " (" + Char.myCharz().taskMaint.counts[j] + "/" + Char.myCharz().taskMaint.counts[j] + ")";
								}
								mFont.tahoma_7_white.drawString(g, empty, xstart + 5, yPaint += 12, 0);
							}
							else
							{
								if (Char.myCharz().taskMaint.counts[j] != 1)
								{
									empty = empty + " 0/" + Char.myCharz().taskMaint.counts[j];
								}
								mFont tahoma_7_grey2 = mFont.tahoma_7_grey;
								if (!flag)
								{
									flag = true;
									tahoma_7_grey2 = mFont.tahoma_7_blue;
									tahoma_7_grey2.drawString(g, empty, xstart + 5 + ((tahoma_7_grey2 == mFont.tahoma_7_blue && GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), yPaint += 12, 0);
								}
								else
								{
									tahoma_7_grey2.drawString(g, "- ...", xstart + 5 + ((tahoma_7_grey2 == mFont.tahoma_7_blue && GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), yPaint += 12, 0);
								}
							}
						}
						else if (Char.myCharz().taskMaint.index > j)
						{
							mFont.tahoma_7_white.drawString(g, empty, xstart + 5, yPaint += 12, 0);
						}
						else
						{
							mFont tahoma_7_grey3 = mFont.tahoma_7_grey;
							if (!flag)
							{
								flag = true;
								tahoma_7_grey3 = mFont.tahoma_7_blue;
								tahoma_7_grey3.drawString(g, empty, xstart + 5 + ((tahoma_7_grey3 == mFont.tahoma_7_blue && GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), yPaint += 12, 0);
							}
							else
							{
								tahoma_7_grey3.drawString(g, "- ...", xstart + 5 + ((tahoma_7_grey3 == mFont.tahoma_7_blue && GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), yPaint += 12, 0);
							}
						}
						indexRowMax++;
					}
					else if (Char.myCharz().taskMaint.index <= j)
					{
						empty = "- " + Char.myCharz().taskMaint.subNames[num2];
						mFont mFont2 = mFont.tahoma_7_grey;
						if (!flag)
						{
							flag = true;
							mFont2 = mFont.tahoma_7_blue;
						}
						mFont2.drawString(g, empty, xstart + 5 + ((mFont2 == mFont.tahoma_7_blue && GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), yPaint += 12, 0);
					}
				}
				yPaint += 5;
				for (int k = 0; k < Char.myCharz().taskMaint.details.Length; k++)
				{
					mFont.tahoma_7_green2.drawString(g, Char.myCharz().taskMaint.details[k], xstart + 5, yPaint += 12, 0);
					indexRowMax++;
				}
			}
			else
			{
				int taskMapId = GameScr.getTaskMapId();
				sbyte taskNpcId = GameScr.getTaskNpcId();
				string empty2 = string.Empty;
				if (taskMapId == -3 || taskNpcId == -3)
				{
					empty2 = mResources.DES_TASK[3];
				}
				else if (Char.myCharz().taskMaint == null && Char.myCharz().ctaskId == 9 && Char.myCharz().nClass.classId == 0)
				{
					empty2 = mResources.TASK_INPUT_CLASS;
				}
				else
				{
					if (taskNpcId < 0 || taskMapId < 0)
					{
						return;
					}
					empty2 = mResources.DES_TASK[0] + Npc.arrNpcTemplate[taskNpcId].name + mResources.DES_TASK[1] + TileMap.mapNames[taskMapId] + mResources.DES_TASK[2];
				}
				string[] array = mFont.tahoma_7_white.splitFontArray(empty2, 150);
				for (int l = 0; l < array.Length; l++)
				{
					if (l == 0)
					{
						mFont.tahoma_7_white.drawString(g, array[l], xstart + 5, yPaint = ystart, 0);
					}
					else
					{
						mFont.tahoma_7_white.drawString(g, array[l], xstart + 5, yPaint += 12, 0);
					}
				}
			}
		}
		else if (indexMenu == 1)
		{
			yPaint = ystart - 12;
			for (int m = 0; m < Char.myCharz().taskOrders.size(); m++)
			{
				TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(m);
				mFont.tahoma_7_white.drawString(g, taskOrder.name, xstart + 5, yPaint += 12, 0);
				if (taskOrder.count == taskOrder.maxCount)
				{
					mFont.tahoma_7_white.drawString(g, ((taskOrder.taskId != 0) ? mResources.KILLBOSS : mResources.KILL) + " " + Mob.arrMobTemplate[taskOrder.killId].name + " (" + taskOrder.count + "/" + taskOrder.maxCount + ")", xstart + 5, yPaint += 12, 0);
				}
				else
				{
					mFont.tahoma_7_blue.drawString(g, ((taskOrder.taskId != 0) ? mResources.KILLBOSS : mResources.KILL) + " " + Mob.arrMobTemplate[taskOrder.killId].name + " (" + taskOrder.count + "/" + taskOrder.maxCount + ")", xstart + 5, yPaint += 12, 0);
				}
				indexRowMax += 3;
				inforW = popupW - 25;
				paintMultiLine(g, mFont.tahoma_7_grey, taskOrder.description, xstart + 5, yPaint += 12, 0);
				yPaint += 12;
			}
		}
		if (scroll == null)
		{
			scroll = new Scroll();
			scroll.setStyle(indexRowMax, 12, xScroll, yScroll, wScroll, hScroll - num - 40, styleUPDOWN: true, 1);
		}
	}

	public void paintMultiLine(mGraphics g, mFont f, string[] arr, string str, int x, int y, int align)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			string text = arr[i];
			if (text.StartsWith("c"))
			{
				if (text.StartsWith("c0"))
				{
					text = text.Substring(2);
					f = mFont.tahoma_7b_dark;
				}
				else if (text.StartsWith("c1"))
				{
					text = text.Substring(2);
					f = mFont.tahoma_7b_yellow;
				}
				else if (text.StartsWith("c2"))
				{
					text = text.Substring(2);
					f = mFont.tahoma_7b_green;
				}
			}
			if (i == 0)
			{
				f.drawString(g, text, x, y, align);
				continue;
			}
			if (i < indexRow + 30 && i > indexRow - 30)
			{
				f.drawString(g, text, x, y += 12, align);
			}
			else
			{
				y += 12;
			}
			yPaint += 12;
			indexRowMax++;
		}
	}

	public void paintMultiLine(mGraphics g, mFont f, string str, int x, int y, int align)
	{
		int num = ((!GameCanvas.isTouch || GameCanvas.w < 320) ? 10 : 20);
		string[] array = f.splitFontArray(str, inforW - num);
		for (int i = 0; i < array.Length; i++)
		{
			if (i == 0)
			{
				f.drawString(g, array[i], x, y, align);
				continue;
			}
			if (i < indexRow + 15 && i > indexRow - 15)
			{
				f.drawString(g, array[i], x, y += 12, align);
			}
			else
			{
				y += 12;
			}
			yPaint += 12;
			indexRowMax++;
		}
	}

	public void cleanCombine()
	{
		for (int i = 0; i < vItemCombine.size(); i++)
		{
			((Item)vItemCombine.elementAt(i)).isSelect = false;
		}
		vItemCombine.removeAllElements();
	}

	public void hideNow()
	{
		if (timeShow > 0)
		{
			isClose = false;
			return;
		}
		if (isTypeShop())
		{
			Char.myCharz().resetPartTemp();
		}
		if (chatTField != null && type == 13 && chatTField.isShow)
		{
			chatTField = null;
		}
		if (type == 13 && !isAccept)
		{
			Service.gI().giaodich(3, -1, -1, -1);
		}
		Res.outz("HIDE PANELLLLLLLLLLLLLLLLLLLLLL");
		SoundMn.gI().buttonClose();
		GameScr.isPaint = true;
		TileMap.lastPlanetId = -1;
		imgMap = null;
		mSystem.gcc();
		isClanOption = false;
		isClose = true;
		Hint.clickNpc();
		GameCanvas.panel2 = null;
		GameCanvas.clearAllPointerEvent();
		GameCanvas.clearKeyPressed();
		pointerDownTime = (pointerDownFirstX = 0);
		pointerIsDowning = false;
		isShow = false;
		if ((Char.myCharz().cHP <= 0 || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5) && Char.myCharz().meDead)
		{
			Command center = new Command(mResources.DIES[0], 11038, GameScr.gI());
			GameScr.gI().center = center;
			Char.myCharz().cHP = 0;
		}
	}

	public void hide()
	{
		if (timeShow > 0)
		{
			isClose = false;
			return;
		}
		if (isTypeShop())
		{
			Char.myCharz().resetPartTemp();
		}
		if (chatTField != null && type == 13 && chatTField.isShow)
		{
			chatTField = null;
		}
		if (type == 13 && !isAccept)
		{
			Service.gI().giaodich(3, -1, -1, -1);
		}
		if (type == 15)
		{
			Service.gI().sendThachDau(-1);
		}
		SoundMn.gI().buttonClose();
		GameScr.isPaint = true;
		TileMap.lastPlanetId = -1;
		if (imgMap != null)
		{
			imgMap.texture = null;
			imgMap = null;
		}
		mSystem.gcc();
		isClanOption = false;
		if (type != 4)
		{
			if (type == 24)
			{
				setTypeGameInfo();
			}
			else if (type == 23)
			{
				setTypeMain();
			}
			else if (type == 3 || type == 14)
			{
				if (isChangeZone)
				{
					isClose = true;
				}
				else
				{
					setTypeMain();
					cmx = (cmtoX = 0);
				}
			}
			else if (type == 18 || type == 19 || type == 20 || type == 21)
			{
				setTypeMain();
				cmx = (cmtoX = 0);
			}
			else if (type == 8 || type == 11 || type == 16)
			{
				setTypeAccount();
				cmx = (cmtoX = 0);
			}
			else
			{
				isClose = true;
			}
		}
		else
		{
			setTypeMain();
			cmx = (cmtoX = 0);
		}
		Hint.clickNpc();
		GameCanvas.panel2 = null;
		GameCanvas.clearAllPointerEvent();
		GameCanvas.clearKeyPressed();
		pointerDownTime = (pointerDownFirstX = 0);
		pointerIsDowning = false;
		if ((Char.myCharz().cHP <= 0 || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5) && Char.myCharz().meDead)
		{
			Command center = new Command(mResources.DIES[0], 11038, GameScr.gI());
			GameScr.gI().center = center;
			Char.myCharz().cHP = 0;
		}
	}

	public void update()
	{
		if (chatTField != null && chatTField.isShow)
		{
			chatTField.update();
			return;
		}
		if (isKiguiXu)
		{
			delayKigui++;
			if (delayKigui == 10)
			{
				delayKigui = 0;
				isKiguiXu = false;
				chatTField.tfChat.setText(string.Empty);
				chatTField.strChat = mResources.kiguiXuchat + " ";
				chatTField.tfChat.name = mResources.input_money;
				chatTField.to = string.Empty;
				chatTField.isShow = true;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
				chatTField.tfChat.setMaxTextLenght(9);
				if (GameCanvas.isTouch)
				{
					chatTField.tfChat.doChangeToTextBox();
				}
				if (Main.isWindowsPhone)
				{
					chatTField.tfChat.strInfo = chatTField.strChat;
				}
				if (!Main.isPC)
				{
					chatTField.startChat2(this, string.Empty);
				}
			}
			return;
		}
		if (isKiguiLuong)
		{
			delayKigui++;
			if (delayKigui == 10)
			{
				delayKigui = 0;
				isKiguiLuong = false;
				chatTField.tfChat.setText(string.Empty);
				chatTField.strChat = mResources.kiguiLuongchat + "  ";
				chatTField.tfChat.name = mResources.input_money;
				chatTField.to = string.Empty;
				chatTField.isShow = true;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
				chatTField.tfChat.setMaxTextLenght(9);
				if (GameCanvas.isTouch)
				{
					chatTField.tfChat.doChangeToTextBox();
				}
				if (Main.isWindowsPhone)
				{
					chatTField.tfChat.strInfo = chatTField.strChat;
				}
				if (!Main.isPC)
				{
					chatTField.startChat2(this, string.Empty);
				}
			}
			return;
		}
		if (scroll != null)
		{
			scroll.updatecm();
		}
		if (tabIcon != null && tabIcon.isShow)
		{
			tabIcon.update();
			return;
		}
		moveCamera();
		if (waitToPerform > 0)
		{
			waitToPerform--;
			if (waitToPerform == 0)
			{
				lastSelect[currentTabIndex] = selected;
				switch (type)
				{
				case 23:
					doFireGameInfo();
					break;
				case 21:
					doFirePetMain();
					break;
				case 0:
					doFireMain();
					break;
				case 2:
					doFireBox();
					break;
				case 3:
					doFireZone();
					break;
				case 1:
				case 17:
					doFireShop();
					break;
				case 25:
					doSpeacialSkill();
					break;
				case 4:
					doFireMap();
					break;
				case 14:
					doFireMapTrans();
					break;
				case 7:
					if (Equals(GameCanvas.panel2) && GameCanvas.panel.type == 2)
					{
						doFireBox();
						return;
					}
					doFireInventory();
					break;
				case 8:
					doFireLogMessage();
					break;
				case 9:
					doFireArchivement();
					break;
				case 10:
					doFirePlayerMenu();
					break;
				case 11:
					doFireFriend();
					break;
				case 16:
					doFireEnemy();
					break;
				case 15:
					doFireTop();
					break;
				case 12:
					doFireCombine();
					break;
				case 13:
					doFireGiaoDich();
					break;
				case 18:
					doFireChangeFlag();
					break;
				case 19:
					doFireOption();
					break;
				case 20:
					doFireAccount();
					break;
				case 22:
					doFireAuto();
					break;
				}
			}
		}
		for (int i = 0; i < ClanMessage.vMessage.size(); i++)
		{
			((ClanMessage)ClanMessage.vMessage.elementAt(i)).update();
		}
		updateCombineEff();
	}

	private void doSpeacialSkill()
	{
	}

	private void doFireGameInfo()
	{
		if (selected != -1)
		{
			infoSelect = selected;
			((GameInfo)vGameInfo.elementAt(infoSelect)).hasRead = true;
			Rms.saveRMSInt(((GameInfo)vGameInfo.elementAt(infoSelect)).id + string.Empty, 1);
			setTypeGameSubInfo();
		}
	}

	private void doFireAuto()
	{
	}

	private void doFirePetMain()
	{
		if (currentTabIndex == 0)
		{
			if (selected == -1 || selected > Char.myPetz().arrItemBody.Length - 1)
			{
				return;
			}
			MyVector myVector = new MyVector(string.Empty);
			Item item = (currItem = Char.myPetz().arrItemBody[selected]);
			if (currItem != null)
			{
				myVector.addElement(new Command(mResources.MOVEOUT, this, 2006, currItem));
				GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
				addItemDetail(currItem);
			}
			else
			{
				cp = null;
			}
		}
		if (currentTabIndex == 1)
		{
			doFirePetStatus();
		}
		if (currentTabIndex == 2)
		{
			doFireInventory();
		}
	}

	private void doFirePetStatus()
	{
		if (selected == -1)
		{
			return;
		}
		if (selected == 5)
		{
			GameCanvas.startYesNoDlg(mResources.sure_fusion, new Command(mResources.YES, 888351), new Command(mResources.NO, 2001));
			return;
		}
		Service.gI().petStatus((sbyte)selected);
		if (selected < 4)
		{
			Char.myPetz().petStatus = (sbyte)selected;
		}
	}

	private void doFireTop()
	{
		if (selected >= -1)
		{
			if (isThachDau)
			{
				Service.gI().sendTop(topName, (sbyte)selected);
				return;
			}
			MyVector myVector = new MyVector(string.Empty);
			myVector.addElement(new Command(mResources.CHAR_ORDER[0], this, 9999, (TopInfo)vTop.elementAt(selected)));
			GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
			addThachDauDetail((TopInfo)vTop.elementAt(selected));
		}
	}

	private void doFireMapTrans()
	{
		doFireZone();
	}

	private void doFireGiaoDich()
	{
		if (currentTabIndex == 0 && Equals(GameCanvas.panel))
		{
			doFireInventory();
			return;
		}
		if ((currentTabIndex == 0 && Equals(GameCanvas.panel2)) || currentTabIndex == 2)
		{
			if (Equals(GameCanvas.panel2))
			{
				currItem = (Item)GameCanvas.panel2.vFriendGD.elementAt(selected);
			}
			else
			{
				currItem = (Item)GameCanvas.panel.vFriendGD.elementAt(selected);
			}
			Res.outz2("toi day select= " + selected);
			MyVector myVector = new MyVector();
			myVector.addElement(new Command(mResources.CLOSE, this, 8000, currItem));
			if (currItem != null)
			{
				GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
				addItemDetail(currItem);
			}
			else
			{
				cp = null;
			}
		}
		if (currentTabIndex == 1)
		{
			if (selected == currentListLength - 3)
			{
				if (isLock)
				{
					return;
				}
				putMoney();
			}
			else if (selected == currentListLength - 2)
			{
				if (!isAccept)
				{
					isLock = !isLock;
					if (isLock)
					{
						Service.gI().giaodich(5, -1, -1, -1);
					}
					else
					{
						hide();
						InfoDlg.showWait();
						Service.gI().giaodich(3, -1, -1, -1);
					}
				}
				else
				{
					isAccept = false;
				}
			}
			else if (selected == currentListLength - 1)
			{
				if (isLock && !isAccept && isFriendLock)
				{
					GameCanvas.startYesNoDlg(mResources.do_u_sure_to_trade, new Command(mResources.YES, this, 7002, null), new Command(mResources.NO, this, 4005, null));
				}
			}
			else
			{
				if (isLock)
				{
					return;
				}
				currItem = (Item)GameCanvas.panel.vMyGD.elementAt(selected);
				MyVector myVector2 = new MyVector();
				myVector2.addElement(new Command(mResources.CLOSE, this, 8000, currItem));
				if (currItem != null)
				{
					GameCanvas.menu.startAt(myVector2, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addItemDetail(currItem);
				}
				else
				{
					cp = null;
				}
			}
		}
		if (GameCanvas.isTouch)
		{
			selected = -1;
		}
	}

	private void doFireCombine()
	{
		if (currentTabIndex == 0)
		{
			if (selected == -1 || vItemCombine.size() == 0)
			{
				return;
			}
			if (selected == vItemCombine.size())
			{
				keyTouchCombine = -1;
				selected = (GameCanvas.isTouch ? (-1) : 0);
				InfoDlg.showWait();
				Service.gI().combine(1, vItemCombine);
				return;
			}
			if (selected > vItemCombine.size() - 1)
			{
				return;
			}
			currItem = (Item)GameCanvas.panel.vItemCombine.elementAt(selected);
			MyVector myVector = new MyVector();
			myVector.addElement(new Command(mResources.GETOUT, this, 6001, currItem));
			if (currItem != null)
			{
				GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
				addItemDetail(currItem);
			}
			else
			{
				cp = null;
			}
		}
		if (currentTabIndex == 1)
		{
			doFireInventory();
		}
	}

	private void doFirePlayerMenu()
	{
		if (selected != -1)
		{
			isSelectPlayerMenu = true;
			hide();
		}
	}

	private void doFireShop()
	{
		currItem = null;
		if (selected < 0)
		{
			return;
		}
		MyVector myVector = new MyVector();
		if (currentTabIndex < currentTabName.Length - ((GameCanvas.panel2 == null) ? 1 : 0) && type != 17)
		{
			currItem = Char.myCharz().arrItemShop[currentTabIndex][selected];
			if (currItem != null)
			{
				if (currItem.isBuySpec)
				{
					if (currItem.buySpec > 0)
					{
						myVector.addElement(new Command(mResources.buy_with + "\n" + Res.formatNumber2(currItem.buySpec), this, 3005, currItem));
					}
				}
				else if (typeShop == 4)
				{
					myVector.addElement(new Command(mResources.receive_upper, this, 30001, currItem));
					myVector.addElement(new Command(mResources.DELETE, this, 30002, currItem));
					myVector.addElement(new Command(mResources.receive_all, this, 30003, currItem));
				}
				else if (currItem.buyCoin == 0 && currItem.buyGold == 0)
				{
					if (currItem.powerRequire != 0)
					{
						myVector.addElement(new Command(mResources.learn_with + "\n" + Res.formatNumber(currItem.powerRequire) + " \n" + mResources.potential, this, 3004, currItem));
					}
					else
					{
						myVector.addElement(new Command(mResources.receive_upper + "\n" + mResources.free, this, 3000, currItem));
					}
				}
				else if (typeShop == 8)
				{
					if (currItem.buyCoin > 0)
					{
						myVector.addElement(new Command(mResources.buy_with + "\n" + Res.formatNumber2(currItem.buyCoin) + "\n" + mResources.XU, this, 30001, currItem));
					}
					if (currItem.buyGold > 0)
					{
						myVector.addElement(new Command(mResources.buy_with + "\n" + Res.formatNumber2(currItem.buyGold) + "\n" + mResources.LUONG, this, 30002, currItem));
					}
				}
				else if (typeShop != 2)
				{
					if (currItem.buyCoin > 0)
					{
						myVector.addElement(new Command(mResources.buy_with + "\n" + Res.formatNumber2(currItem.buyCoin) + "\n" + mResources.XU, this, 3000, currItem));
					}
					if (currItem.buyGold > 0)
					{
						myVector.addElement(new Command(mResources.buy_with + "\n" + Res.formatNumber2(currItem.buyGold) + "\n" + mResources.LUONG, this, 3001, currItem));
					}
				}
				else
				{
					Debug.LogError("--3");
					if (currItem.buyCoin != -1)
					{
						myVector.addElement(new Command(mResources.buy_with + "\n" + Res.formatNumber2(currItem.buyCoin) + "\n" + mResources.XU, this, 10016, currItem));
					}
					if (currItem.buyGold != -1)
					{
						myVector.addElement(new Command(mResources.buy_with + "\n" + Res.formatNumber2(currItem.buyGold) + "\n" + mResources.LUONG, this, 10017, currItem));
					}
				}
			}
		}
		else if (typeShop == 0)
		{
			Item[] arrItemBody = Char.myCharz().arrItemBody;
			if (selected >= arrItemBody.Length)
			{
				sbyte b = (sbyte)(selected - arrItemBody.Length);
				Item item = Char.myCharz().arrItemBag[b];
				if (item != null)
				{
					currItem = item;
				}
			}
			else
			{
				Item item2 = Char.myCharz().arrItemBody[selected];
				if (item2 != null)
				{
					currItem = item2;
				}
			}
			if (currItem != null)
			{
				myVector.addElement(new Command(mResources.SALE, this, 3002, currItem));
			}
		}
		else
		{
			if (type == 17)
			{
				currItem = Char.myCharz().arrItemShop[4][selected];
			}
			else
			{
				currItem = Char.myCharz().arrItemShop[currentTabIndex][selected];
			}
			if (currItem.buyType == 0)
			{
				if (currItem.isHaveOption(87))
				{
					myVector.addElement(new Command(mResources.kiguiLuong, this, 10013, currItem));
				}
				else
				{
					myVector.addElement(new Command(mResources.kiguiXu, this, 10012, currItem));
				}
			}
			else if (currItem.buyType == 1)
			{
				myVector.addElement(new Command(mResources.huykigui, this, 10014, currItem));
				myVector.addElement(new Command(mResources.upTop, this, 10018, currItem));
			}
			else if (currItem.buyType == 2)
			{
				myVector.addElement(new Command(mResources.nhantien, this, 10015, currItem));
			}
		}
		if (currItem != null)
		{
			Char.myCharz().setPartTemp(currItem.headTemp, currItem.bodyTemp, currItem.legTemp, currItem.bagTemp);
			GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
			addItemDetail(currItem);
		}
		else
		{
			cp = null;
		}
	}

	private void doFireArchivement()
	{
		if (selected >= 0 && Char.myCharz().arrArchive[selected].isFinish && !Char.myCharz().arrArchive[selected].isRecieve)
		{
			if (!GameCanvas.isTouch)
			{
				Service.gI().getArchivemnt(selected);
			}
			else if (GameCanvas.px > xScroll + wScroll - 40)
			{
				Service.gI().getArchivemnt(selected);
			}
		}
	}

	private void doFireInventory()
	{
		Res.outz("fire inventory");
		if (Char.myCharz().statusMe == 14)
		{
			GameCanvas.startOKDlg(mResources.can_not_do_when_die);
		}
		else
		{
			if (selected == -1)
			{
				return;
			}
			currItem = null;
			MyVector myVector = new MyVector();
			Item[] arrItemBody = Char.myCharz().arrItemBody;
			if (selected >= arrItemBody.Length)
			{
				sbyte b = (sbyte)(selected - arrItemBody.Length);
				Item item = Char.myCharz().arrItemBag[b];
				if (item != null)
				{
					currItem = item;
					if (GameCanvas.panel.type == 12)
					{
						myVector.addElement(new Command(mResources.use_for_combine, this, 6000, currItem));
					}
					else if (GameCanvas.panel.type == 13)
					{
						myVector.addElement(new Command(mResources.use_for_trade, this, 7000, currItem));
					}
					else if (item.isTypeBody())
					{
						myVector.addElement(new Command(mResources.USE, this, 2000, currItem));
						if (Char.myCharz().havePet)
						{
							myVector.addElement(new Command(mResources.MOVEFORPET, this, 2005, currItem));
						}
					}
					else
					{
						myVector.addElement(new Command(mResources.USE, this, 2001, currItem));
					}
				}
			}
			else
			{
				Item item2 = Char.myCharz().arrItemBody[selected];
				if (item2 != null)
				{
					currItem = item2;
					myVector.addElement(new Command(mResources.GETOUT, this, 2002, currItem));
				}
			}
			if (currItem != null)
			{
				Char.myCharz().setPartTemp(currItem.headTemp, currItem.bodyTemp, currItem.legTemp, currItem.bagTemp);
				if (GameCanvas.panel.type != 12 && GameCanvas.panel.type != 13)
				{
					if (position == 0)
					{
						myVector.addElement(new Command(mResources.MOVEOUT, this, 2003, currItem));
					}
					if (position == 1)
					{
						myVector.addElement(new Command(mResources.SALE, this, 3002, currItem));
					}
				}
				GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
				addItemDetail(currItem);
			}
			else
			{
				cp = null;
			}
		}
	}

	private void doRada()
	{
		hide();
		if (RadarScr.list == null || RadarScr.list.size() == 0)
		{
			Service.gI().SendRada(0, -1);
			RadarScr.gI().switchToMe();
		}
		else
		{
			RadarScr.gI().switchToMe();
		}
	}

	private void doFireTool()
	{
		if (selected < 0)
		{
			return;
		}
		if (!Char.myCharz().havePet)
		{
			switch (selected)
			{
			case 0:
				doRada();
				break;
			case 1:
				hide();
				Service.gI().openMenu(54);
				break;
			case 2:
				setTypeGameInfo();
				break;
			case 3:
				Service.gI().getFlag(0, -1);
				InfoDlg.showWait();
				break;
			case 4:
				if (Char.myCharz().statusMe == 14)
				{
					GameCanvas.startOKDlg(mResources.can_not_do_when_die);
				}
				else
				{
					Service.gI().openUIZone();
				}
				break;
			case 5:
				GameCanvas.endDlg();
				if (Char.myCharz().checkLuong() < 5)
				{
					GameCanvas.startOKDlg(mResources.not_enough_luong_world_channel);
					break;
				}
				if (chatTField == null)
				{
					chatTField = new ChatTextField();
					chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
					chatTField.initChatTextField();
					chatTField.parentScreen = GameCanvas.panel;
				}
				chatTField.strChat = mResources.world_channel_5_luong;
				chatTField.tfChat.name = mResources.CHAT;
				chatTField.to = string.Empty;
				chatTField.isShow = true;
				chatTField.tfChat.isFocus = true;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				if (Main.isWindowsPhone)
				{
					chatTField.tfChat.strInfo = chatTField.strChat;
				}
				if (!Main.isPC)
				{
					chatTField.startChat2(this, string.Empty);
				}
				else if (GameCanvas.isTouch)
				{
					chatTField.tfChat.doChangeToTextBox();
				}
				break;
			case 6:
				setTypeAccount();
				break;
			case 7:
				setTypeOption();
				break;
			case 8:
				GameCanvas.loginScr.backToRegister();
				break;
			case 9:
				if (GameCanvas.loginScr.isLogin2)
				{
					SoundMn.gI().backToRegister();
				}
				break;
			}
			return;
		}
		switch (selected)
		{
		case 0:
			doRada();
			break;
		case 1:
			hide();
			Service.gI().openMenu(54);
			break;
		case 2:
			setTypeGameInfo();
			break;
		case 3:
			doFirePet();
			break;
		case 4:
			Service.gI().getFlag(0, -1);
			InfoDlg.showWait();
			break;
		case 5:
			if (Char.myCharz().statusMe == 14)
			{
				GameCanvas.startOKDlg(mResources.can_not_do_when_die);
			}
			else
			{
				Service.gI().openUIZone();
			}
			break;
		case 6:
			GameCanvas.endDlg();
			if (Char.myCharz().checkLuong() < 5)
			{
				GameCanvas.startOKDlg(mResources.not_enough_luong_world_channel);
				break;
			}
			if (chatTField == null)
			{
				chatTField = new ChatTextField();
				chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
				chatTField.initChatTextField();
				chatTField.parentScreen = GameCanvas.panel;
			}
			chatTField.strChat = mResources.world_channel_5_luong;
			chatTField.tfChat.name = mResources.CHAT;
			chatTField.to = string.Empty;
			chatTField.isShow = true;
			chatTField.tfChat.isFocus = true;
			chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
			if (Main.isWindowsPhone)
			{
				chatTField.tfChat.strInfo = chatTField.strChat;
			}
			if (!Main.isPC)
			{
				chatTField.startChat2(this, string.Empty);
			}
			else if (GameCanvas.isTouch)
			{
				chatTField.tfChat.doChangeToTextBox();
			}
			break;
		case 7:
			setTypeAccount();
			break;
		case 8:
			setTypeOption();
			break;
		case 9:
			GameCanvas.loginScr.backToRegister();
			break;
		case 10:
			if (GameCanvas.loginScr.isLogin2)
			{
				SoundMn.gI().backToRegister();
			}
			break;
		}
	}

	private void setTypeGameSubInfo()
	{
		string content = ((GameInfo)vGameInfo.elementAt(infoSelect)).content;
		contenInfo = mFont.tahoma_7_grey.splitFontArray(content, wScroll - 40);
		currentListLength = contenInfo.Length;
		ITEM_HEIGHT = 16;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		type = 24;
		setType(0);
	}

	private void setTypeGameInfo()
	{
		currentListLength = vGameInfo.size();
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		type = 23;
		setType(0);
	}

	private void doFirePet()
	{
		InfoDlg.showWait();
		Service.gI().petInfo();
		timeShow = 20;
	}

	private void searchClan()
	{
		chatTField.strChat = mResources.input_clan_name;
		chatTField.tfChat.name = mResources.clan_name;
		chatTField.to = string.Empty;
		chatTField.isShow = true;
		chatTField.tfChat.isFocus = true;
		chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			chatTField.tfChat.strInfo = chatTField.strChat;
		}
		if (!Main.isPC)
		{
			chatTField.startChat2(this, string.Empty);
		}
	}

	private void chatClan()
	{
		chatTField.strChat = mResources.chat_clan;
		chatTField.tfChat.name = mResources.CHAT;
		chatTField.to = string.Empty;
		chatTField.isShow = true;
		chatTField.tfChat.isFocus = true;
		chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			chatTField.tfChat.strInfo = chatTField.strChat;
		}
		if (!Main.isPC)
		{
			chatTField.startChat2(this, string.Empty);
		}
	}

	public void creatClan()
	{
		chatTField.strChat = mResources.input_clan_name_to_create;
		chatTField.tfChat.name = mResources.input_clan_name;
		chatTField.to = string.Empty;
		chatTField.isShow = true;
		chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			chatTField.tfChat.strInfo = chatTField.strChat;
		}
		if (!Main.isPC)
		{
			chatTField.startChat2(this, string.Empty);
		}
	}

	public void putMoney()
	{
		if (chatTField == null)
		{
			chatTField = new ChatTextField();
			chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
			chatTField.initChatTextField();
			chatTField.parentScreen = GameCanvas.panel;
		}
		chatTField.strChat = mResources.input_money_to_trade;
		chatTField.tfChat.name = mResources.input_money;
		chatTField.to = string.Empty;
		chatTField.isShow = true;
		chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
		chatTField.tfChat.setMaxTextLenght(9);
		if (GameCanvas.isTouch)
		{
			chatTField.tfChat.doChangeToTextBox();
		}
		if (Main.isWindowsPhone)
		{
			chatTField.tfChat.strInfo = chatTField.strChat;
		}
		if (!Main.isPC)
		{
			chatTField.startChat2(this, string.Empty);
		}
	}

	public void putQuantily()
	{
		if (chatTField == null)
		{
			chatTField = new ChatTextField();
			chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
			chatTField.initChatTextField();
			chatTField.parentScreen = GameCanvas.panel;
		}
		chatTField.strChat = mResources.input_quantity_to_trade;
		chatTField.tfChat.name = mResources.input_quantity;
		chatTField.to = string.Empty;
		chatTField.isShow = true;
		chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
		if (GameCanvas.isTouch)
		{
			chatTField.tfChat.doChangeToTextBox();
		}
		if (Main.isWindowsPhone)
		{
			chatTField.tfChat.strInfo = chatTField.strChat;
		}
		if (!Main.isPC)
		{
			chatTField.startChat2(this, string.Empty);
		}
	}

	public void chagenSlogan()
	{
		chatTField.strChat = mResources.input_clan_slogan;
		chatTField.tfChat.name = mResources.input_clan_slogan;
		chatTField.to = string.Empty;
		chatTField.isShow = true;
		chatTField.tfChat.isFocus = true;
		chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			chatTField.tfChat.strInfo = chatTField.strChat;
		}
		if (!Main.isPC)
		{
			chatTField.startChat2(this, string.Empty);
		}
	}

	public void changeIcon()
	{
		if (tabIcon == null)
		{
			tabIcon = new TabClanIcon();
		}
		tabIcon.text = chatTField.tfChat.getText();
		tabIcon.show(isGetName: false);
		chatTField.isShow = false;
	}

	private void addFriend(InfoItem info)
	{
		string text = "|0|1|" + info.charInfo.cName;
		text += "\n";
		text = ((!info.isOnline) ? (text + "|3|1|" + mResources.is_offline) : (text + "|4|1|" + mResources.is_online));
		text += "\n--";
		string text2 = text;
		text = text2 + "\n|5|" + mResources.power + ": " + info.s;
		cp = new ChatPopup();
		popUpDetailInit(cp, text);
		charInfo = info.charInfo;
		currItem = null;
	}

	private void doFireEnemy()
	{
		if (selected >= 0 && vEnemy.size() != 0)
		{
			MyVector myVector = new MyVector();
			currInfoItem = selected;
			myVector.addElement(new Command(mResources.REVENGE, this, 10000, (InfoItem)vEnemy.elementAt(currInfoItem)));
			myVector.addElement(new Command(mResources.DELETE, this, 10001, (InfoItem)vEnemy.elementAt(currInfoItem)));
			GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
			addFriend((InfoItem)vEnemy.elementAt(selected));
		}
	}

	private void doFireFriend()
	{
		if (selected >= 0 && vFriend.size() != 0)
		{
			MyVector myVector = new MyVector();
			currInfoItem = selected;
			myVector.addElement(new Command(mResources.CHAT, this, 8001, (InfoItem)vFriend.elementAt(currInfoItem)));
			myVector.addElement(new Command(mResources.DELETE, this, 8002, (InfoItem)vFriend.elementAt(currInfoItem)));
			myVector.addElement(new Command(mResources.den, this, 8004, (InfoItem)vFriend.elementAt(currInfoItem)));
			GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
			addFriend((InfoItem)vFriend.elementAt(selected));
		}
	}

	private void doFireChangeFlag()
	{
		if (selected >= 0)
		{
			MyVector myVector = new MyVector();
			currInfoItem = selected;
			myVector.addElement(new Command(mResources.change_flag, this, 10030, null));
			myVector.addElement(new Command(mResources.BACK, this, 10031, null));
			GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
		}
	}

	private void doFireLogMessage()
	{
		if (selected == 0)
		{
			isViewChatServer = !isViewChatServer;
			Rms.saveRMSInt("viewchat", isViewChatServer ? 1 : 0);
			if (GameCanvas.isTouch)
			{
				selected = -1;
			}
		}
		else if (selected >= 0 && logChat.size() != 0)
		{
			MyVector myVector = new MyVector();
			currInfoItem = selected - 1;
			myVector.addElement(new Command(mResources.CHAT, this, 8001, (InfoItem)logChat.elementAt(currInfoItem)));
			myVector.addElement(new Command(mResources.make_friend, this, 8003, (InfoItem)logChat.elementAt(currInfoItem)));
			GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
			addLogMessage((InfoItem)logChat.elementAt(selected - 1));
		}
	}

	private void doFireClanOption()
	{
		partID = null;
		charInfo = null;
		Res.outz("cSelect= " + cSelected);
		if (selected < 0)
		{
			cSelected = -1;
			return;
		}
		if (Char.myCharz().clan == null)
		{
			if (selected == 0)
			{
				if (cSelected == 0)
				{
					searchClan();
				}
				else if (cSelected == 1)
				{
					InfoDlg.showWait();
					creatClan();
					Service.gI().getClan(1, -1, null);
				}
			}
			else if (selected != -1)
			{
				if (selected == 1)
				{
					if (isSearchClan)
					{
						Service.gI().searchClan(string.Empty);
					}
					else if (isViewMember && currClan != null)
					{
						GameCanvas.startYesNoDlg(mResources.do_u_want_join_clan + currClan.name, new Command(mResources.YES, this, 4000, currClan), new Command(mResources.NO, this, 4005, currClan));
					}
				}
				else if (isSearchClan)
				{
					currClan = getCurrClan();
					if (currClan != null)
					{
						MyVector myVector = new MyVector();
						myVector.addElement(new Command(mResources.request_join_clan, this, 4000, currClan));
						myVector.addElement(new Command(mResources.view_clan_member, this, 4001, currClan));
						GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
						addClanDetail(getCurrClan());
					}
				}
				else if (isViewMember)
				{
					currMem = getCurrMember();
					if (currMem != null)
					{
						MyVector myVector2 = new MyVector();
						myVector2.addElement(new Command(mResources.CLOSE, this, 8000, currClan));
						GameCanvas.menu.startAt(myVector2, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
						GameCanvas.menu.startAt(myVector2, 0, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
						addClanMemberDetail(currMem);
					}
				}
			}
		}
		else if (selected == 0)
		{
			if (isMessage)
			{
				if (cSelected == 0)
				{
					if (myMember.size() > 1)
					{
						chatClan();
					}
					else
					{
						member = null;
						isSearchClan = false;
						isViewMember = true;
						isMessage = false;
						currentListLength = myMember.size() + 2;
						initTabClans();
					}
				}
				if (cSelected == 1)
				{
					Service.gI().clanMessage(1, null, -1);
				}
				if (cSelected == 2)
				{
					member = null;
					isSearchClan = false;
					isViewMember = true;
					isMessage = false;
					currentListLength = myMember.size() + 2;
					initTabClans();
					getCurrClanOtion();
				}
			}
			else if (isViewMember)
			{
				if (cSelected == 0)
				{
					isSearchClan = false;
					isViewMember = false;
					isMessage = true;
					currentListLength = ClanMessage.vMessage.size() + 2;
					initTabClans();
				}
				if (cSelected == 1)
				{
					if (myMember.size() > 1)
					{
						Service.gI().leaveClan();
					}
					else
					{
						chagenSlogan();
					}
				}
				if (cSelected == 2)
				{
					if (myMember.size() > 1)
					{
						chagenSlogan();
					}
					else
					{
						Service.gI().getClan(3, -1, null);
					}
				}
				if (cSelected == 3)
				{
					Service.gI().getClan(3, -1, null);
				}
			}
		}
		else if (selected == 1)
		{
			if (isSearchClan)
			{
				Service.gI().searchClan(string.Empty);
			}
		}
		else if (isSearchClan)
		{
			currClan = getCurrClan();
			if (currClan != null)
			{
				MyVector myVector3 = new MyVector();
				myVector3.addElement(new Command(mResources.view_clan_member, this, 4001, currClan));
				GameCanvas.menu.startAt(myVector3, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
				addClanDetail(getCurrClan());
			}
		}
		else if (isViewMember)
		{
			Res.outz("TOI DAY 1");
			currMem = getCurrMember();
			if (currMem != null)
			{
				MyVector myVector4 = new MyVector();
				Res.outz("TOI DAY 2");
				if (member != null)
				{
					myVector4.addElement(new Command(mResources.CLOSE, this, 8000, null));
					Res.outz("TOI DAY 3");
				}
				else if (myMember != null)
				{
					Res.outz("TOI DAY 4");
					Res.outz("my role= " + Char.myCharz().role);
					if (Char.myCharz().charID == currMem.ID || Char.myCharz().role == 2)
					{
						myVector4.addElement(new Command(mResources.CLOSE, this, 8000, currMem));
					}
					if (Char.myCharz().role < 2 && Char.myCharz().charID != currMem.ID)
					{
						Res.outz("TOI DAY");
						if (currMem.role == 0 || currMem.role == 1)
						{
							myVector4.addElement(new Command(mResources.CLOSE, this, 8000, currMem));
						}
						if (currMem.role == 2)
						{
							myVector4.addElement(new Command(mResources.create_clan_co_leader, this, 5002, currMem));
						}
						if (Char.myCharz().role == 0)
						{
							myVector4.addElement(new Command(mResources.create_clan_leader, this, 5001, currMem));
							if (currMem.role == 1)
							{
								myVector4.addElement(new Command(mResources.disable_clan_mastership, this, 5003, currMem));
							}
						}
					}
					if (Char.myCharz().role < currMem.role)
					{
						myVector4.addElement(new Command(mResources.kick_clan_mem, this, 5004, currMem));
					}
				}
				GameCanvas.menu.startAt(myVector4, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
				addClanMemberDetail(currMem);
			}
		}
		else if (isMessage)
		{
			currMess = getCurrMessage();
			if (currMess != null)
			{
				if (currMess.type == 0)
				{
					MyVector myVector5 = new MyVector();
					myVector5.addElement(new Command(mResources.CLOSE, this, 8000, currMess));
					GameCanvas.menu.startAt(myVector5, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addMessageDetail(currMess);
				}
				else if (currMess.type == 1)
				{
					if (currMess.playerId != Char.myCharz().charID && cSelected != -1)
					{
						Service.gI().clanDonate(currMess.id);
					}
				}
				else if (currMess.type == 2 && currMess.option != null)
				{
					if (cSelected == 0)
					{
						Service.gI().joinClan(currMess.id, 1);
					}
					else if (cSelected == 1)
					{
						Service.gI().joinClan(currMess.id, 0);
					}
				}
			}
		}
		if (GameCanvas.isTouch)
		{
			cSelected = -1;
			selected = -1;
		}
	}

	private void doFireMain()
	{
		if (currentTabIndex == 0)
		{
			setTypeMap();
		}
		if (currentTabIndex == 1)
		{
			doFireInventory();
		}
		if (currentTabIndex == 2)
		{
			doFireSkill();
		}
		if (currentTabIndex == 3)
		{
			if (mainTabName.Length == 4)
			{
				doFireTool();
			}
			else
			{
				doFireClanOption();
			}
		}
		if (currentTabIndex == 4)
		{
			doFireTool();
		}
	}

	private void doFireSkill()
	{
		if (selected < 0)
		{
			return;
		}
		if (Char.myCharz().statusMe == 14)
		{
			GameCanvas.startOKDlg(mResources.can_not_do_when_die);
			return;
		}
		if (selected == 0 || selected == 1 || selected == 2 || selected == 3 || selected == 4 || selected == 5)
		{
			long cTiemNang = Char.myCharz().cTiemNang;
			int cHPGoc = Char.myCharz().cHPGoc;
			int cMPGoc = Char.myCharz().cMPGoc;
			int cDamGoc = Char.myCharz().cDamGoc;
			int cDefGoc = Char.myCharz().cDefGoc;
			int cCriticalGoc = Char.myCharz().cCriticalGoc;
			int num = 0;
			int num2 = 1000;
			if (selected == 0)
			{
				if (cTiemNang < Char.myCharz().cHPGoc + num2)
				{
					GameCanvas.startOKDlg(mResources.not_enough_potential_point1 + Char.myCharz().cTiemNang + mResources.not_enough_potential_point2 + (Char.myCharz().cHPGoc + num2), isError: false);
					return;
				}
				if (cTiemNang > cHPGoc && cTiemNang < 10 * (2 * (cHPGoc + num2) + 180) / 2)
				{
					GameCanvas.startYesNoDlg(mResources.use_potential_point_for1 + (cHPGoc + num2) + mResources.use_potential_point_for2 + Char.myCharz().hpFrom1000TiemNang + mResources.for_HP, new Command(mResources.increase_upper, this, 9000, null), new Command(mResources.CANCEL, this, 4007, null));
					return;
				}
				if (cTiemNang >= 10 * (2 * (cHPGoc + num2) + 180) / 2 && cTiemNang < 100 * (2 * (cHPGoc + num2) + 1980) / 2)
				{
					MyVector myVector = new MyVector();
					myVector.addElement(new Command(mResources.increase_upper + "\n" + Char.myCharz().hpFrom1000TiemNang + mResources.HP + "\n-" + (cHPGoc + num2), this, 9000, null));
					myVector.addElement(new Command(mResources.increase_upper + "\n" + 10 * Char.myCharz().hpFrom1000TiemNang + mResources.HP + "\n-" + 10 * (2 * (cHPGoc + num2) + 180) / 2, this, 9006, null));
					GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addSkillDetail2(selected);
				}
				if (cTiemNang >= 100 * (2 * (cHPGoc + num2) + 1980) / 2)
				{
					MyVector myVector2 = new MyVector();
					myVector2.addElement(new Command(mResources.increase_upper + "\n" + Char.myCharz().hpFrom1000TiemNang + mResources.HP + "\n-" + (cHPGoc + num2), this, 9000, null));
					myVector2.addElement(new Command(mResources.increase_upper + "\n" + 10 * Char.myCharz().hpFrom1000TiemNang + mResources.HP + "\n-" + 10 * (2 * (cHPGoc + num2) + 180) / 2, this, 9006, null));
					myVector2.addElement(new Command(mResources.increase_upper + "\n" + 100 * Char.myCharz().hpFrom1000TiemNang + mResources.HP + "\n-" + 100 * (2 * (cHPGoc + num2) + 1980) / 2, this, 9007, null));
					GameCanvas.menu.startAt(myVector2, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addSkillDetail2(selected);
				}
			}
			if (selected == 1)
			{
				if (Char.myCharz().cTiemNang < Char.myCharz().cMPGoc + num2)
				{
					GameCanvas.startOKDlg(mResources.not_enough_potential_point1 + Char.myCharz().cTiemNang + mResources.not_enough_potential_point2 + (Char.myCharz().cMPGoc + num2), isError: false);
					return;
				}
				if (cTiemNang > cMPGoc && cTiemNang < 10 * (2 * (cMPGoc + num2) + 180) / 2)
				{
					GameCanvas.startYesNoDlg(mResources.use_potential_point_for1 + (cMPGoc + num2) + mResources.use_potential_point_for2 + Char.myCharz().mpFrom1000TiemNang + mResources.for_KI, new Command(mResources.increase_upper, this, 9000, null), new Command(mResources.CANCEL, this, 4007, null));
					return;
				}
				if (cTiemNang >= 10 * (2 * (cMPGoc + num2) + 180) / 2 && cTiemNang < 100 * (2 * (cMPGoc + num2) + 1980) / 2)
				{
					MyVector myVector3 = new MyVector();
					myVector3.addElement(new Command(mResources.increase_upper + "\n" + Char.myCharz().mpFrom1000TiemNang + mResources.KI + "\n-" + (cHPGoc + num2), this, 9000, null));
					myVector3.addElement(new Command(mResources.increase_upper + "\n" + 10 * Char.myCharz().mpFrom1000TiemNang + mResources.KI + "\n-" + 10 * (2 * (cHPGoc + num2) + 180) / 2, this, 9006, null));
					GameCanvas.menu.startAt(myVector3, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addSkillDetail2(selected);
				}
				if (cTiemNang >= 100 * (2 * (cMPGoc + num2) + 1980) / 2)
				{
					MyVector myVector4 = new MyVector();
					myVector4.addElement(new Command(mResources.increase_upper + "\n" + Char.myCharz().mpFrom1000TiemNang + mResources.KI + "\n-" + (cMPGoc + num2), this, 9000, null));
					myVector4.addElement(new Command(mResources.increase_upper + "\n" + 10 * Char.myCharz().mpFrom1000TiemNang + mResources.KI + "\n-" + 10 * (2 * (cMPGoc + num2) + 180) / 2, this, 9006, null));
					myVector4.addElement(new Command(mResources.increase_upper + "\n" + 100 * Char.myCharz().mpFrom1000TiemNang + mResources.KI + "\n-" + 100 * (2 * (cMPGoc + num2) + 1980) / 2, this, 9007, null));
					GameCanvas.menu.startAt(myVector4, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addSkillDetail2(selected);
				}
			}
			if (selected == 2)
			{
				if (Char.myCharz().cTiemNang < Char.myCharz().cDamGoc * Char.myCharz().expForOneAdd)
				{
					GameCanvas.startOKDlg(mResources.not_enough_potential_point1 + Char.myCharz().cTiemNang + mResources.not_enough_potential_point2 + cDamGoc * 100, isError: false);
					return;
				}
				if (cTiemNang > cDamGoc && cTiemNang < 10 * (2 * cDamGoc + 9) / 2 * Char.myCharz().expForOneAdd)
				{
					GameCanvas.startYesNoDlg(mResources.use_potential_point_for1 + cDamGoc * 100 + mResources.use_potential_point_for2 + Char.myCharz().damFrom1000TiemNang + mResources.for_hit_point, new Command(mResources.increase_upper, this, 9000, null), new Command(mResources.CANCEL, this, 4007, null));
					return;
				}
				if (cTiemNang >= 10 * (2 * cDamGoc + 9) / 2 * Char.myCharz().expForOneAdd && cTiemNang < 100 * (2 * cDamGoc + 99) / 2 * Char.myCharz().expForOneAdd)
				{
					MyVector myVector5 = new MyVector();
					myVector5.addElement(new Command(mResources.increase_upper + "\n" + Char.myCharz().damFrom1000TiemNang + "\n" + mResources.hit_point + "\n-" + cDamGoc * 100, this, 9000, null));
					myVector5.addElement(new Command(mResources.increase_upper + "\n" + 10 * Char.myCharz().damFrom1000TiemNang + "\n" + mResources.hit_point + "\n-" + 10 * (2 * cDamGoc + 9) / 2 * Char.myCharz().expForOneAdd, this, 9006, null));
					GameCanvas.menu.startAt(myVector5, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addSkillDetail2(selected);
				}
				if (cTiemNang >= 100 * (2 * cDamGoc + 99) / 2 * Char.myCharz().expForOneAdd)
				{
					MyVector myVector6 = new MyVector();
					myVector6.addElement(new Command(mResources.increase_upper + "\n" + Char.myCharz().damFrom1000TiemNang + "\n" + mResources.hit_point + "\n-" + cDamGoc * 100, this, 9000, null));
					myVector6.addElement(new Command(mResources.increase_upper + "\n" + 10 * Char.myCharz().damFrom1000TiemNang + "\n" + mResources.hit_point + "\n-" + 10 * (2 * cDamGoc + 9) / 2 * Char.myCharz().expForOneAdd, this, 9006, null));
					myVector6.addElement(new Command(mResources.increase_upper + "\n" + 100 * Char.myCharz().damFrom1000TiemNang + "\n" + mResources.hit_point + "\n-" + 100 * (2 * cDamGoc + 99) / 2 * Char.myCharz().expForOneAdd, this, 9007, null));
					GameCanvas.menu.startAt(myVector6, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
					addSkillDetail2(selected);
				}
			}
			if (selected == 3)
			{
				if (Char.myCharz().cTiemNang < 50000 + Char.myCharz().cDefGoc * 1000)
				{
					GameCanvas.startOKDlg(mResources.not_enough_potential_point1 + NinjaUtil.getMoneys(Char.myCharz().cTiemNang) + mResources.not_enough_potential_point2 + NinjaUtil.getMoneys(50000 + Char.myCharz().cDefGoc * 1000), isError: false);
					return;
				}
				int num3 = 2 * (cDefGoc + 5) / 2 * 100000;
				GameCanvas.startYesNoDlg(mResources.use_potential_point_for1 + NinjaUtil.getMoneys(num3) + mResources.use_potential_point_for2 + Char.myCharz().defFrom1000TiemNang + mResources.for_armor, new Command(mResources.increase_upper, this, 9000, null), new Command(mResources.CANCEL, this, 4007, null));
			}
			else if (selected == 4)
			{
				long num4 = 50000000L;
				for (int i = 0; i < Char.myCharz().cCriticalGoc; i++)
				{
					num4 *= 5;
				}
				if (Char.myCharz().cTiemNang < num4)
				{
					GameCanvas.startOKDlg(mResources.not_enough_potential_point1 + NinjaUtil.getMoneys(Char.myCharz().cTiemNang) + mResources.not_enough_potential_point2 + NinjaUtil.getMoneys(num4), isError: false);
					return;
				}
				GameCanvas.startYesNoDlg(mResources.use_potential_point_for1 + NinjaUtil.getMoneys(num4) + mResources.use_potential_point_for2 + Char.myCharz().criticalFrom1000Tiemnang + mResources.for_crit, new Command(mResources.increase_upper, this, 9000, null), new Command(mResources.CANCEL, this, 4007, null));
			}
			else if (selected == 5)
			{
				Service.gI().speacialSkill(0);
			}
			return;
		}
		int num5 = selected - 6;
		SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[num5];
		Skill skill = Char.myCharz().getSkill(skillTemplate);
		Skill skill2 = null;
		MyVector myVector7 = new MyVector();
		if (skill != null)
		{
			if (skill.point == skillTemplate.maxPoint)
			{
				myVector7.addElement(new Command(mResources.make_shortcut, this, 9003, skill.template));
				myVector7.addElement(new Command(mResources.CLOSE, 2));
			}
			else
			{
				skill2 = skillTemplate.skills[skill.point];
				myVector7.addElement(new Command(mResources.UPGRADE, this, 9002, skill2));
				myVector7.addElement(new Command(mResources.make_shortcut, this, 9003, skill.template));
			}
		}
		else
		{
			skill2 = skillTemplate.skills[0];
			myVector7.addElement(new Command(mResources.learn, this, 9004, skill2));
		}
		GameCanvas.menu.startAt(myVector7, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
		addSkillDetail(skillTemplate, skill, skill2);
	}

	private void addLogMessage(InfoItem info)
	{
		string text = "|0|1|" + info.charInfo.cName;
		text += "\n";
		text += "\n--";
		text = text + "\n|5|" + Res.split(info.s, "|", 0)[2];
		cp = new ChatPopup();
		popUpDetailInit(cp, text);
		charInfo = info.charInfo;
		currItem = null;
	}

	private void addSkillDetail2(int type)
	{
		string empty = string.Empty;
		int num = 0;
		if (selected == 0)
		{
			num = Char.myCharz().cHPGoc + 1000;
		}
		if (selected == 1)
		{
			num = Char.myCharz().cMPGoc + 1000;
		}
		if (selected == 2)
		{
			num = Char.myCharz().cDamGoc * Char.myCharz().expForOneAdd;
		}
		string text = empty;
		empty = text + "|5|2|" + mResources.USE + " " + num + " " + mResources.potential;
		if (type == 0)
		{
			empty = empty + "\n|5|2|" + mResources.to_gain_20hp;
		}
		if (type == 1)
		{
			empty = empty + "\n|5|2|" + mResources.to_gain_20mp;
		}
		if (type == 2)
		{
			empty = empty + "\n|5|2|" + mResources.to_gain_1pow;
		}
		currItem = null;
		partID = null;
		charInfo = null;
		idIcon = -1;
		cp = new ChatPopup();
		popUpDetailInit(cp, empty);
	}

	private void doFireClanIcon()
	{
	}

	private void doFireMap()
	{
		if (imgMap != null)
		{
			imgMap.texture = null;
			imgMap = null;
		}
		TileMap.lastPlanetId = -1;
		mSystem.gcc();
		SmallImage.loadBigRMS();
		setTypeMain();
		cmx = (cmtoX = 0);
	}

	private void doFireZone()
	{
		if (selected != -1)
		{
			Res.outz("FIRE ZONE");
			isChangeZone = true;
			GameCanvas.panel.hide();
		}
	}

	public void updateRequest(int recieve, int maxCap)
	{
		cp.says[cp.says.Length - 1] = mResources.received + " " + recieve + "/" + maxCap;
	}

	private void doFireBox()
	{
		if (selected < 0)
		{
			return;
		}
		currItem = null;
		MyVector myVector = new MyVector();
		if (currentTabIndex == 0 && !Equals(GameCanvas.panel2))
		{
			Item item = Char.myCharz().arrItemBox[selected];
			if (item != null)
			{
				if (item.isTypeBody())
				{
					if (Char.myCharz().arrItemBody[item.itemOption[0].optionTemplate.type] != null)
					{
						myVector.addElement(new Command(mResources.GETOUT, this, 1000, item));
					}
					else
					{
						myVector.addElement(new Command(mResources.GETOUT, this, 1000, item));
					}
				}
				else if (isBoxClan)
				{
					myVector.addElement(new Command(mResources.GETOUT, this, 1000, item));
					myVector.addElement(new Command(mResources.USE, this, 2010, item));
				}
				else
				{
					myVector.addElement(new Command(mResources.GETOUT, this, 1000, item));
					myVector.addElement(new Command(mResources.USE, this, 2010, item));
				}
				currItem = item;
			}
		}
		if (currentTabIndex == 1 || Equals(GameCanvas.panel2))
		{
			Item[] arrItemBody = Char.myCharz().arrItemBody;
			if (selected >= arrItemBody.Length)
			{
				sbyte b = (sbyte)(selected - arrItemBody.Length);
				Item item2 = Char.myCharz().arrItemBag[b];
				if (item2 != null)
				{
					myVector.addElement(new Command(mResources.move_to_chest, this, 1001, item2));
					if (item2.isTypeBody())
					{
						myVector.addElement(new Command(mResources.USE, this, 2000, item2));
					}
					else
					{
						myVector.addElement(new Command(mResources.USE, this, 2001, item2));
					}
					currItem = item2;
				}
			}
			else
			{
				Item item3 = Char.myCharz().arrItemBody[selected];
				if (item3 != null)
				{
					myVector.addElement(new Command(mResources.move_to_chest2, this, 1002, item3));
					currItem = item3;
				}
			}
		}
		if (currItem != null)
		{
			Char.myCharz().setPartTemp(currItem.headTemp, currItem.bodyTemp, currItem.legTemp, currItem.bagTemp);
			if (isBoxClan)
			{
				myVector.addElement(new Command(mResources.MOVEOUT, this, 2011, currItem));
			}
			else
			{
				myVector.addElement(new Command(mResources.MOVEOUT, this, 2003, currItem));
			}
			GameCanvas.menu.startAt(myVector, X, (selected + 1) * ITEM_HEIGHT - cmy + yScroll);
			addItemDetail(currItem);
		}
		else
		{
			cp = null;
		}
	}

	public void itemRequest(sbyte itemAction, string info, sbyte where, sbyte index)
	{
		GameCanvas.endDlg();
		ItemObject itemObject = new ItemObject();
		itemObject.type = itemAction;
		itemObject.id = index;
		itemObject.where = where;
		GameCanvas.startYesNoDlg(info, new Command(mResources.YES, this, 2004, itemObject), new Command(mResources.NO, this, 4005, null));
	}

	public void saleRequest(sbyte type, string info, short id)
	{
		ItemObject itemObject = new ItemObject();
		itemObject.type = type;
		itemObject.id = id;
		GameCanvas.startYesNoDlg(info, new Command(mResources.YES, this, 3003, itemObject), new Command(mResources.NO, this, 4005, null));
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 9999)
		{
			TopInfo topInfo = (TopInfo)p;
			Service.gI().sendThachDau(topInfo.pId);
		}
		if (idAction == 170391)
		{
            Rms.clearAll();
            Rms.saveRMSInt("levelScreenKN", Rms.loadRMSInt("levelScreenKN") == 1 ? 0 : 1);
            GameMidlet.instance.exit();
		}
        if (idAction == 170392)
        {
            Rms.saveRMSInt("lowGraphic", Rms.loadRMSInt("lowGraphic") == 1 ? 0 : 1);
            GameMidlet.instance.exit();
        }
        if (idAction == 6001)
		{
			Item item = (Item)p;
			item.isSelect = false;
			GameCanvas.panel.vItemCombine.removeElement(item);
			if (GameCanvas.panel.currentTabIndex == 0)
			{
				GameCanvas.panel.setTabCombine();
			}
		}
		if (idAction == 6000)
		{
			Item item2 = (Item)p;
			for (int i = 0; i < GameCanvas.panel.vItemCombine.size(); i++)
			{
				Item item3 = (Item)GameCanvas.panel.vItemCombine.elementAt(i);
				if (item3.template.id == item2.template.id)
				{
					GameCanvas.startOKDlg(mResources.already_has_item);
					return;
				}
			}
			item2.isSelect = true;
			GameCanvas.panel.vItemCombine.addElement(item2);
			if (GameCanvas.panel.currentTabIndex == 0)
			{
				GameCanvas.panel.setTabCombine();
			}
		}
		if (idAction == 7000)
		{
			if (isLock)
			{
				GameCanvas.startOKDlg(mResources.unlock_item_to_trade);
				return;
			}
			Item item4 = (Item)p;
			for (int j = 0; j < GameCanvas.panel.vMyGD.size(); j++)
			{
				Item item5 = (Item)GameCanvas.panel.vMyGD.elementAt(j);
				if (item5.indexUI == item4.indexUI)
				{
					GameCanvas.startOKDlg(mResources.already_has_item);
					return;
				}
			}
			if (item4.quantity > 1)
			{
				putQuantily();
				return;
			}
			item4.isSelect = true;
			Item item6 = new Item();
			item6.template = item4.template;
			item6.itemOption = item4.itemOption;
			item6.indexUI = item4.indexUI;
			GameCanvas.panel.vMyGD.addElement(item6);
			Service.gI().giaodich(2, -1, (sbyte)item6.indexUI, item6.quantity);
		}
		if (idAction == 7001)
		{
			Item item7 = (Item)p;
			item7.isSelect = false;
			GameCanvas.panel.vMyGD.removeElement(item7);
			if (GameCanvas.panel.currentTabIndex == 1)
			{
				GameCanvas.panel.setTabGiaoDich(isMe: true);
			}
			Service.gI().giaodich(4, -1, (sbyte)item7.indexUI, -1);
		}
		if (idAction == 7002)
		{
			isAccept = true;
			GameCanvas.endDlg();
			Service.gI().giaodich(7, -1, -1, -1);
			hide();
		}
		if (idAction == 8003)
		{
			InfoItem infoItem = (InfoItem)p;
			Service.gI().friend(1, infoItem.charInfo.charID);
			if (type != 8)
			{
			}
		}
		if (idAction == 8002)
		{
			InfoItem infoItem2 = (InfoItem)p;
			Service.gI().friend(2, infoItem2.charInfo.charID);
		}
		if (idAction == 8004)
		{
			InfoItem infoItem3 = (InfoItem)p;
			Service.gI().gotoPlayer(infoItem3.charInfo.charID);
		}
		if (idAction == 8001)
		{
			Res.outz("chat player");
			InfoItem infoItem4 = (InfoItem)p;
			if (chatTField == null)
			{
				chatTField = new ChatTextField();
				chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
				chatTField.initChatTextField();
				chatTField.parentScreen = GameCanvas.panel;
			}
			chatTField.strChat = mResources.chat_player;
			chatTField.tfChat.name = mResources.chat_with + " " + infoItem4.charInfo.cName;
			chatTField.to = string.Empty;
			chatTField.isShow = true;
			chatTField.tfChat.isFocus = true;
			chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
			if (Main.isWindowsPhone)
			{
				chatTField.tfChat.strInfo = chatTField.strChat;
			}
			if (!Main.isPC)
			{
				chatTField.startChat2(this, string.Empty);
			}
		}
		if (idAction == 1000)
		{
			Service.gI().getItem(BOX_BAG, (sbyte)selected);
		}
		if (idAction == 1001)
		{
			Item[] arrItemBody = Char.myCharz().arrItemBody;
			sbyte id = (sbyte)(selected - arrItemBody.Length);
			Service.gI().getItem(BAG_BOX, id);
		}
		if (idAction == 1003)
		{
			hide();
		}
		if (idAction == 1002)
		{
			Service.gI().getItem(BODY_BOX, (sbyte)selected);
		}
		if (idAction == 2011)
		{
			Service.gI().useItem(1, 2, (sbyte)selected, -1);
		}
		if (idAction == 2010)
		{
			Service.gI().useItem(0, 2, (sbyte)selected, -1);
			Item item8 = (Item)p;
			if (item8 != null && (item8.template.id == 193 || item8.template.id == 194))
			{
				GameCanvas.panel.hide();
			}
		}
		if (idAction == 2000)
		{
			Item[] arrItemBody2 = Char.myCharz().arrItemBody;
			sbyte id2 = (sbyte)(selected - arrItemBody2.Length);
			Service.gI().getItem(BAG_BODY, id2);
		}
		if (idAction == 2001)
		{
			Res.outz("use item");
			Item item9 = (Item)p;
			bool flag = selected < Char.myCharz().arrItemBody.Length;
			sbyte b = 0;
			if (!flag)
			{
				b = (sbyte)(selected - Char.myCharz().arrItemBody.Length);
			}
			Service.gI().useItem(0, (sbyte)((!flag) ? 1 : 0), (sbyte)((!flag) ? b : selected), -1);
			if (item9.template.id == 193 || item9.template.id == 194)
			{
				GameCanvas.panel.hide();
			}
		}
		if (idAction == 2005)
		{
			Item[] arrItemBody3 = Char.myCharz().arrItemBody;
			sbyte id3 = (sbyte)(selected - arrItemBody3.Length);
			Service.gI().getItem(BAG_PET, id3);
		}
		if (idAction == 2006)
		{
			Item[] arrItemBody4 = Char.myPetz().arrItemBody;
			sbyte id4 = (sbyte)selected;
			Service.gI().getItem(PET_BAG, id4);
		}
		if (idAction == 2002)
		{
			Service.gI().getItem(BODY_BAG, (sbyte)selected);
		}
		if (idAction == 2003)
		{
			Res.outz("remove item");
			bool flag2 = selected < Char.myCharz().arrItemBody.Length;
			sbyte b2 = 0;
			if (!flag2)
			{
				b2 = (sbyte)(selected - Char.myCharz().arrItemBody.Length);
			}
			Service.gI().useItem(1, (sbyte)((!flag2) ? 1 : 0), (sbyte)((!flag2) ? b2 : selected), -1);
		}
		if (idAction == 2004)
		{
			GameCanvas.endDlg();
			ItemObject itemObject = (ItemObject)p;
			sbyte where = (sbyte)itemObject.where;
			sbyte index = (sbyte)itemObject.id;
			Service.gI().useItem((sbyte)((itemObject.type != 0) ? 2 : 3), where, index, -1);
		}
		if (idAction == 30001)
		{
			Res.outz("nhan do");
			Service.gI().buyItem(0, selected, 0);
		}
		if (idAction == 30002)
		{
			Res.outz("xoa do");
			Service.gI().buyItem(1, selected, 0);
		}
		if (idAction == 30003)
		{
			Res.outz("nhan tat");
			Service.gI().buyItem(2, selected, 0);
		}
		if (idAction == 3000)
		{
			Res.outz("mua do");
			Item item10 = (Item)p;
			Service.gI().buyItem(0, item10.template.id, 0);
		}
		if (idAction == 3001)
		{
			Item item11 = (Item)p;
			GameCanvas.msgdlg.pleasewait();
			Service.gI().buyItem(1, item11.template.id, 0);
		}
		if (idAction == 3002)
		{
			GameCanvas.endDlg();
			Item item12 = (Item)p;
			Service.gI().saleItem(0, (selected > 6) ? ((sbyte)1) : ((sbyte)0), (short)((selected >= 7) ? (selected - 6 - 1) : selected));
		}
		if (idAction == 3003)
		{
			GameCanvas.endDlg();
			ItemObject itemObject2 = (ItemObject)p;
			Service.gI().saleItem(1, (sbyte)itemObject2.type, (short)itemObject2.id);
		}
		if (idAction == 3004)
		{
			Item item13 = (Item)p;
			Service.gI().buyItem(3, item13.template.id, 0);
		}
		if (idAction == 3005)
		{
			Res.outz("mua do");
			Item item14 = (Item)p;
			Service.gI().buyItem(3, item14.template.id, 0);
		}
		if (idAction == 4000)
		{
			Clan clan = (Clan)p;
			if (clan != null)
			{
				GameCanvas.endDlg();
				Service.gI().clanMessage(2, null, clan.ID);
			}
		}
		if (idAction == 4001)
		{
			Clan clan2 = (Clan)p;
			if (clan2 != null)
			{
				InfoDlg.showWait();
				clanReport = mResources.PLEASEWAIT;
				Service.gI().clanMember(clan2.ID);
			}
		}
		if (idAction == 4005)
		{
			GameCanvas.endDlg();
		}
		if (idAction == 4007)
		{
			GameCanvas.endDlg();
		}
		if (idAction == 4006)
		{
			ClanMessage clanMessage = (ClanMessage)p;
			Service.gI().clanDonate(clanMessage.id);
		}
		if (idAction == 5001)
		{
			Member member = (Member)p;
			Service.gI().clanRemote(member.ID, 0);
		}
		if (idAction == 5002)
		{
			Member member2 = (Member)p;
			Service.gI().clanRemote(member2.ID, 1);
		}
		if (idAction == 5003)
		{
			Member member3 = (Member)p;
			Service.gI().clanRemote(member3.ID, 2);
		}
		if (idAction == 5004)
		{
			Member member4 = (Member)p;
			Service.gI().clanRemote(member4.ID, -1);
		}
		if (idAction == 9000)
		{
			Service.gI().upPotential(selected, 1);
			GameCanvas.endDlg();
			InfoDlg.showWait();
		}
		if (idAction == 9006)
		{
			Service.gI().upPotential(selected, 10);
			GameCanvas.endDlg();
			InfoDlg.showWait();
		}
		if (idAction == 9007)
		{
			Service.gI().upPotential(selected, 100);
			GameCanvas.endDlg();
			InfoDlg.showWait();
		}
		if (idAction == 9002)
		{
			Skill skill = (Skill)p;
			GameCanvas.startOKDlg(mResources.can_buy_from_Uron1 + skill.powRequire + mResources.can_buy_from_Uron2 + skill.moreInfo + mResources.can_buy_from_Uron3);
		}
		if (idAction == 9003)
		{
			if (GameCanvas.isTouch && !Main.isPC)
			{
				GameScr.gI().doSetOnScreenSkill((SkillTemplate)p);
			}
			else
			{
				GameScr.gI().doSetKeySkill((SkillTemplate)p);
			}
		}
		if (idAction == 9004)
		{
			Skill skill2 = (Skill)p;
			GameCanvas.startOKDlg(mResources.can_buy_from_Uron1 + skill2.powRequire + mResources.can_buy_from_Uron2 + skill2.moreInfo + mResources.can_buy_from_Uron3);
		}
		if (idAction == 10000)
		{
			InfoItem infoItem5 = (InfoItem)p;
			Service.gI().enemy(1, infoItem5.charInfo.charID);
			GameCanvas.panel.hideNow();
		}
		if (idAction == 10001)
		{
			InfoItem infoItem6 = (InfoItem)p;
			Service.gI().enemy(2, infoItem6.charInfo.charID);
			InfoDlg.showWait();
		}
		if (idAction == 10021)
		{
		}
		if (idAction == 10012)
		{
			if (chatTField == null)
			{
				chatTField = new ChatTextField();
				chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
				chatTField.initChatTextField();
				chatTField.parentScreen = ((GameCanvas.panel2 != null) ? GameCanvas.panel2 : GameCanvas.panel);
			}
			if (currItem.quantity == 1)
			{
				chatTField.strChat = mResources.kiguiXuchat;
				chatTField.tfChat.name = mResources.input_money;
			}
			else
			{
				chatTField.strChat = mResources.input_quantity + " ";
				chatTField.tfChat.name = mResources.input_quantity;
			}
			chatTField.tfChat.setMaxTextLenght(9);
			chatTField.to = string.Empty;
			chatTField.isShow = true;
			chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			if (GameCanvas.isTouch)
			{
				chatTField.tfChat.doChangeToTextBox();
			}
			if (Main.isWindowsPhone)
			{
				chatTField.tfChat.strInfo = chatTField.strChat;
			}
			if (!Main.isPC)
			{
				chatTField.startChat2(this, string.Empty);
			}
		}
		if (idAction == 10013)
		{
			if (chatTField == null)
			{
				chatTField = new ChatTextField();
				chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
				chatTField.initChatTextField();
				chatTField.parentScreen = ((GameCanvas.panel2 != null) ? GameCanvas.panel2 : GameCanvas.panel);
			}
			if (currItem.quantity == 1)
			{
				chatTField.strChat = mResources.kiguiLuongchat;
				chatTField.tfChat.name = mResources.input_money;
			}
			else
			{
				chatTField.strChat = mResources.input_quantity + "  ";
				chatTField.tfChat.name = mResources.input_quantity;
			}
			chatTField.to = string.Empty;
			chatTField.isShow = true;
			chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			if (GameCanvas.isTouch)
			{
				chatTField.tfChat.doChangeToTextBox();
			}
			if (Main.isWindowsPhone)
			{
				chatTField.tfChat.strInfo = chatTField.strChat;
			}
			if (!Main.isPC)
			{
				chatTField.startChat2(this, string.Empty);
			}
		}
		if (idAction == 10014)
		{
			Item item15 = (Item)p;
			Service.gI().kigui(1, item15.itemId, -1, -1, -1);
			InfoDlg.showWait();
		}
		if (idAction == 10015)
		{
			Item item16 = (Item)p;
			Service.gI().kigui(2, item16.itemId, -1, -1, -1);
			InfoDlg.showWait();
		}
		if (idAction == 10016)
		{
			Item item17 = (Item)p;
			Service.gI().kigui(3, item17.itemId, 0, item17.buyCoin, -1);
			InfoDlg.showWait();
		}
		if (idAction == 10017)
		{
			Item item18 = (Item)p;
			Service.gI().kigui(3, item18.itemId, 1, item18.buyGold, -1);
			InfoDlg.showWait();
		}
		if (idAction == 10018)
		{
			Item item19 = (Item)p;
			Service.gI().kigui(5, item19.itemId, -1, -1, -1);
			InfoDlg.showWait();
		}
		if (idAction == 10019)
		{
			Session_ME.gI().close();
			Rms.saveRMSString("acc", string.Empty);
			Rms.saveRMSString("pass", string.Empty);
			GameCanvas.loginScr.tfPass.setText(string.Empty);
			GameCanvas.loginScr.tfUser.setText(string.Empty);
			GameCanvas.loginScr.isLogin2 = false;
			GameCanvas.loginScr.switchToMe();
			GameCanvas.endDlg();
			hide();
		}
		if (idAction == 10020)
		{
			GameCanvas.endDlg();
		}
		if (idAction == 10030)
		{
			Service.gI().getFlag(1, (sbyte)selected);
			GameCanvas.panel.hideNow();
		}
		if (idAction == 10031)
		{
			Session_ME.gI().close();
		}
		if (idAction == 11000)
		{
			Service.gI().kigui(0, currItem.itemId, 1, currItem.buyRuby, 1);
			GameCanvas.endDlg();
		}
		if (idAction == 11001)
		{
			Service.gI().kigui(0, currItem.itemId, 1, currItem.buyRuby, (byte)currItem.quantilyToBuy);
			GameCanvas.endDlg();
		}
		if (idAction == 11002)
		{
			chatTField.isShow = false;
			GameCanvas.endDlg();
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (chatTField.tfChat.getText() == null || chatTField.tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
		{
			chatTField.isShow = false;
			return;
		}
		if (chatTField.strChat.Equals(mResources.input_clan_name))
		{
			InfoDlg.showWait();
			chatTField.isShow = false;
			Service.gI().searchClan(text);
			return;
		}
		if (chatTField.strChat.Equals(mResources.chat_clan))
		{
			InfoDlg.showWait();
			chatTField.isShow = false;
			Service.gI().clanMessage(0, text, -1);
			return;
		}
		if (chatTField.strChat.Equals(mResources.input_clan_name_to_create))
		{
			if (chatTField.tfChat.getText() == string.Empty)
			{
				GameScr.info1.addInfo(mResources.clan_name_blank, 0);
				return;
			}
			if (tabIcon == null)
			{
				tabIcon = new TabClanIcon();
			}
			tabIcon.text = chatTField.tfChat.getText();
			tabIcon.show(isGetName: false);
			chatTField.isShow = false;
			return;
		}
		if (chatTField.strChat.Equals(mResources.input_clan_slogan))
		{
			if (chatTField.tfChat.getText() == string.Empty)
			{
				GameScr.info1.addInfo(mResources.clan_slogan_blank, 0);
				return;
			}
			Service.gI().getClan(4, (sbyte)Char.myCharz().clan.imgID, chatTField.tfChat.getText());
			chatTField.isShow = false;
			return;
		}
		if (chatTField.strChat.Equals(mResources.input_Inventory_Pass))
		{
			try
			{
				int lockInventory = int.Parse(chatTField.tfChat.getText());
				if (chatTField.tfChat.getText().Length != 6 || chatTField.tfChat.getText().Equals(string.Empty))
				{
					GameCanvas.startOKDlg(mResources.input_Inventory_Pass_wrong);
				}
				else
				{
					Service.gI().setLockInventory(lockInventory);
					chatTField.isShow = false;
					chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
					hide();
				}
				return;
			}
			catch (Exception)
			{
				GameCanvas.startOKDlg(mResources.ALERT_PRIVATE_PASS_2);
				return;
			}
		}
		if (chatTField.strChat.Equals(mResources.world_channel_5_luong))
		{
			if (!chatTField.tfChat.getText().Equals(string.Empty))
			{
				Service.gI().chatGlobal(chatTField.tfChat.getText());
				chatTField.isShow = false;
				hide();
			}
		}
		else if (chatTField.strChat.Equals(mResources.chat_player))
		{
			chatTField.isShow = false;
			InfoItem infoItem = null;
			if (type == 8)
			{
				infoItem = (InfoItem)logChat.elementAt(currInfoItem);
			}
			else if (type == 11)
			{
				infoItem = (InfoItem)vFriend.elementAt(currInfoItem);
			}
			if (infoItem.charInfo.charID != Char.myCharz().charID)
			{
				Service.gI().chatPlayer(text, infoItem.charInfo.charID);
			}
		}
		else if (chatTField.strChat.Equals(mResources.input_quantity_to_trade))
		{
			int num = 0;
			try
			{
				num = int.Parse(chatTField.tfChat.getText());
			}
			catch (Exception)
			{
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				chatTField.isShow = false;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			if (num <= 0 || num > currItem.quantity)
			{
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				chatTField.isShow = false;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			currItem.isSelect = true;
			Item item = new Item();
			item.template = currItem.template;
			item.quantity = num;
			item.indexUI = currItem.indexUI;
			item.itemOption = currItem.itemOption;
			GameCanvas.panel.vMyGD.addElement(item);
			Service.gI().giaodich(2, -1, (sbyte)item.indexUI, item.quantity);
			chatTField.isShow = false;
			chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
		}
		else if (chatTField.strChat == mResources.input_money_to_trade)
		{
			int num2 = 0;
			try
			{
				num2 = int.Parse(chatTField.tfChat.getText());
			}
			catch (Exception)
			{
				GameCanvas.startOKDlg(mResources.input_money_wrong);
				chatTField.isShow = false;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
				return;
			}
			if (num2 > Char.myCharz().xu)
			{
				GameCanvas.startOKDlg(mResources.not_enough_money);
				chatTField.isShow = false;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
			}
			else
			{
				moneyGD = num2;
				Service.gI().giaodich(2, -1, -1, num2);
				chatTField.isShow = false;
				chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
			}
		}
		else if (chatTField.strChat.Equals(mResources.kiguiXuchat))
		{
			Service.gI().kigui(0, currItem.itemId, 0, int.Parse(chatTField.tfChat.getText()), 1);
			chatTField.isShow = false;
		}
		else if (chatTField.strChat.Equals(mResources.kiguiXuchat + " "))
		{
			Service.gI().kigui(0, currItem.itemId, 0, int.Parse(chatTField.tfChat.getText()), (sbyte)currItem.quantilyToBuy);
			chatTField.isShow = false;
		}
		else if (chatTField.strChat.Equals(mResources.kiguiLuongchat))
		{
			doNotiRuby(0);
			chatTField.isShow = false;
		}
		else if (chatTField.strChat.Equals(mResources.kiguiLuongchat + "  "))
		{
			doNotiRuby(1);
			chatTField.isShow = false;
		}
		else if (chatTField.strChat.Equals(mResources.input_quantity + " "))
		{
			currItem.quantilyToBuy = int.Parse(chatTField.tfChat.getText());
			if (currItem.quantilyToBuy > currItem.quantity)
			{
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				return;
			}
			isKiguiXu = true;
			chatTField.isShow = false;
		}
		else if (chatTField.strChat.Equals(mResources.input_quantity + "  "))
		{
			currItem.quantilyToBuy = int.Parse(chatTField.tfChat.getText());
			if (currItem.quantilyToBuy > currItem.quantity)
			{
				GameCanvas.startOKDlg(mResources.input_quantity_wrong);
				return;
			}
			isKiguiLuong = true;
			chatTField.isShow = false;
		}
	}

	public void onCancelChat()
	{
		chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
	}

	public void setCombineEff(int type)
	{
		typeCombine = type;
		rS = 90;
		if (typeCombine == 0)
		{
			iDotS = 5;
			angleS = (angleO = 90);
			time = 2;
			for (int i = 0; i < vItemCombine.size(); i++)
			{
				Item item = (Item)vItemCombine.elementAt(i);
				if (item != null)
				{
					if (item.template.type == 14)
					{
						iconID2 = item.template.iconID;
					}
					else
					{
						iconID1 = item.template.iconID;
					}
				}
			}
		}
		else if (typeCombine == 1)
		{
			iDotS = 2;
			angleS = (angleO = 0);
			time = 1;
			for (int j = 0; j < vItemCombine.size(); j++)
			{
				Item item2 = (Item)vItemCombine.elementAt(j);
				if (item2 != null)
				{
					if (j == 0)
					{
						iconID1 = item2.template.iconID;
					}
					else
					{
						iconID2 = item2.template.iconID;
					}
				}
			}
		}
		else if (typeCombine == 2)
		{
			iDotS = 7;
			angleS = (angleO = 25);
			time = 1;
			for (int k = 0; k < vItemCombine.size(); k++)
			{
				Item item3 = (Item)vItemCombine.elementAt(k);
				if (item3 != null)
				{
					iconID1 = item3.template.iconID;
				}
			}
		}
		else if (typeCombine == 3)
		{
			xS = GameCanvas.hw;
			yS = GameCanvas.hh;
			iDotS = 1;
			angleS = (angleO = 1);
			time = 4;
			for (int l = 0; l < vItemCombine.size(); l++)
			{
				Item item4 = (Item)vItemCombine.elementAt(l);
				if (item4 != null)
				{
					iconID1 = item4.template.iconID;
				}
			}
		}
		speed = 1;
		isSpeedCombine = true;
		isDoneCombine = false;
		isCompleteEffCombine = false;
		iAngleS = 360 / iDotS;
		xArgS = new int[iDotS];
		yArgS = new int[iDotS];
		xDotS = new int[iDotS];
		yDotS = new int[iDotS];
		setDotStar();
		isPaintCombine = true;
		countUpdate = 10;
		countR = 30;
		countWait = 10;
		addTextCombineNPC(idNPC, mResources.combineSpell);
	}

	private void updateCombineEff()
	{
		countUpdate--;
		if (countUpdate < 0)
		{
			countUpdate = 0;
		}
		countR--;
		if (countR < 0)
		{
			countR = 0;
		}
		if (countUpdate != 0)
		{
			return;
		}
		if (!isCompleteEffCombine)
		{
			if (time > 0)
			{
				if (combineSuccess != -1)
				{
					if (typeCombine == 3)
					{
						if (GameCanvas.gameTick % 10 == 0)
						{
							Effect me = new Effect(21, xS - 10, yS + 25, 4, 1, 1);
							EffecMn.addEff(me);
							time--;
						}
					}
					else
					{
						if (GameCanvas.gameTick % 2 == 0)
						{
							if (isSpeedCombine)
							{
								if (speed < 40)
								{
									speed += 2;
								}
							}
							else if (speed > 10)
							{
								speed -= 2;
							}
						}
						if (countR == 0)
						{
							if (isSpeedCombine)
							{
								if (rS > 0)
								{
									rS -= 5;
								}
								else if (GameCanvas.gameTick % 10 == 0)
								{
									isSpeedCombine = false;
									time--;
									countR = 5;
									countWait = 10;
								}
							}
							else if (rS < 90)
							{
								rS += 5;
							}
							else if (GameCanvas.gameTick % 10 == 0)
							{
								isSpeedCombine = true;
								countR = 10;
							}
						}
						angleS = angleO;
						angleS -= speed;
						if (angleS >= 360)
						{
							angleS -= 360;
						}
						if (angleS < 0)
						{
							angleS = 360 + angleS;
						}
						angleO = angleS;
						setDotStar();
					}
				}
			}
			else if (GameCanvas.gameTick % 20 == 0)
			{
				isCompleteEffCombine = true;
			}
			if (GameCanvas.gameTick % 20 == 0)
			{
				if (typeCombine != 3)
				{
					EffectPanel.addServerEffect(132, xS, yS, 2);
				}
				EffectPanel.addServerEffect(114, xS, yS + 20, 2);
			}
		}
		else
		{
			if (!isCompleteEffCombine)
			{
				return;
			}
			if (combineSuccess == 1)
			{
				if (countWait == 10)
				{
					Effect me2 = new Effect(22, xS - 3, yS + 25, 4, 1, 1);
					EffecMn.addEff(me2);
				}
				countWait--;
				if (countWait < 0)
				{
					countWait = 0;
				}
				if (rS < 300)
				{
					rS = Res.abs(rS + 10);
					if (rS == 20)
					{
						addTextCombineNPC(idNPC, mResources.combineFail);
					}
				}
				else if (GameCanvas.gameTick % 20 == 0)
				{
					if (GameCanvas.w > 2 * WIDTH_PANEL)
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
						GameCanvas.panel2.setTypeBodyOnly();
						GameCanvas.panel2.show();
					}
					combineSuccess = -1;
					isDoneCombine = true;
				}
				setDotStar();
			}
			else
			{
				if (combineSuccess != 0)
				{
					return;
				}
				if (countWait == 10)
				{
					if (typeCombine == 2)
					{
						Effect me3 = new Effect(20, xS - 3, yS + 15, 4, 2, 1);
						EffecMn.addEff(me3);
					}
					else
					{
						Effect me4 = new Effect(21, xS - 10, yS + 25, 4, 1, 1);
						EffecMn.addEff(me4);
					}
					addTextCombineNPC(idNPC, mResources.combineSuccess);
					isPaintCombine = false;
				}
				if (isPaintCombine)
				{
					return;
				}
				countWait--;
				if (countWait < -50)
				{
					countWait = -50;
					if (typeCombine < 3 && GameCanvas.w > 2 * WIDTH_PANEL)
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
						GameCanvas.panel2.setTypeBodyOnly();
						GameCanvas.panel2.show();
					}
					combineSuccess = -1;
					isDoneCombine = true;
				}
			}
		}
	}

	public void paintCombineEff(mGraphics g)
	{
		GameScr.gI().paintBlackSky(g);
		paintCombineNPC(g);
		if (typeCombine == 0)
		{
			for (int i = 0; i < yArgS.Length; i++)
			{
				SmallImage.drawSmallImage(g, iconID1, xS, yS, 0, mGraphics.VCENTER | mGraphics.HCENTER);
				if (isPaintCombine)
				{
					SmallImage.drawSmallImage(g, iconID2, xDotS[i], yDotS[i], 0, mGraphics.VCENTER | mGraphics.HCENTER);
				}
			}
		}
		else if (typeCombine == 1)
		{
			if (!isPaintCombine)
			{
				SmallImage.drawSmallImage(g, iconID3, xS, yS, 0, mGraphics.VCENTER | mGraphics.HCENTER);
				return;
			}
			for (int j = 0; j < yArgS.Length; j++)
			{
				SmallImage.drawSmallImage(g, iconID1, xDotS[0], yDotS[0], 0, mGraphics.VCENTER | mGraphics.HCENTER);
				SmallImage.drawSmallImage(g, iconID2, xDotS[1], yDotS[1], 0, mGraphics.VCENTER | mGraphics.HCENTER);
			}
		}
		else if (typeCombine == 2)
		{
			if (!isPaintCombine)
			{
				SmallImage.drawSmallImage(g, iconID3, xS, yS, 0, mGraphics.VCENTER | mGraphics.HCENTER);
				return;
			}
			for (int k = 0; k < yArgS.Length; k++)
			{
				SmallImage.drawSmallImage(g, iconID1, xDotS[k], yDotS[k], 0, mGraphics.VCENTER | mGraphics.HCENTER);
			}
		}
		else if (typeCombine == 3)
		{
			if (!isPaintCombine)
			{
				SmallImage.drawSmallImage(g, iconID3, xS, yS, 0, mGraphics.VCENTER | mGraphics.HCENTER);
			}
			else
			{
				SmallImage.drawSmallImage(g, iconID1, xS, yS, 0, mGraphics.VCENTER | mGraphics.HCENTER);
			}
		}
	}

	private void setDotStar()
	{
		for (int i = 0; i < yArgS.Length; i++)
		{
			if (angleS >= 360)
			{
				angleS -= 360;
			}
			if (angleS < 0)
			{
				angleS = 360 + angleS;
			}
			yArgS[i] = Res.abs(rS * Res.sin(angleS) / 1024);
			xArgS[i] = Res.abs(rS * Res.cos(angleS) / 1024);
			if (angleS < 90)
			{
				xDotS[i] = xS + xArgS[i];
				yDotS[i] = yS - yArgS[i];
			}
			else if (angleS >= 90 && angleS < 180)
			{
				xDotS[i] = xS - xArgS[i];
				yDotS[i] = yS - yArgS[i];
			}
			else if (angleS >= 180 && angleS < 270)
			{
				xDotS[i] = xS - xArgS[i];
				yDotS[i] = yS + yArgS[i];
			}
			else
			{
				xDotS[i] = xS + xArgS[i];
				yDotS[i] = yS + yArgS[i];
			}
			angleS -= iAngleS;
		}
	}

	public void paintCombineNPC(mGraphics g)
	{
		g.translate(-GameScr.cmx, -GameScr.cmy);
		if (typeCombine < 3)
		{
			for (int i = 0; i < GameScr.vNpc.size(); i++)
			{
				Npc npc = (Npc)GameScr.vNpc.elementAt(i);
				if (npc.template.npcTemplateId == idNPC)
				{
					npc.paint(g);
					if (npc.chatInfo != null)
					{
						npc.chatInfo.paint(g, npc.cx, npc.cy - npc.ch, npc.cdir);
					}
				}
			}
		}
		GameCanvas.resetTrans(g);
		if (GameCanvas.gameTick % 4 == 0)
		{
			g.drawImage(ItemMap.imageFlare, xS - 5, yS + 15, mGraphics.BOTTOM | mGraphics.HCENTER);
			g.drawImage(ItemMap.imageFlare, xS + 5, yS + 15, mGraphics.BOTTOM | mGraphics.HCENTER);
			g.drawImage(ItemMap.imageFlare, xS, yS + 15, mGraphics.BOTTOM | mGraphics.HCENTER);
		}
		for (int j = 0; j < Effect2.vEffect3.size(); j++)
		{
			Effect2 effect = (Effect2)Effect2.vEffect3.elementAt(j);
			effect.paint(g);
		}
	}

	public void addTextCombineNPC(int idNPC, string text)
	{
		if (typeCombine >= 3)
		{
			return;
		}
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(i);
			if (npc.template.npcTemplateId == idNPC)
			{
				npc.addInfo(text);
			}
		}
	}

	public void setTypeOption()
	{
		type = 19;
		setType(0);
		setTabOption();
		cmx = (cmtoX = 0);
	}

	private void setTabOption()
	{
		SoundMn.gI().getStrOption();
		currentListLength = strCauhinh.Length;
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
	}

	private void paintOption(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < strCauhinh.Length; i++)
		{
			int x = xScroll;
			int num = yScroll + i * ITEM_HEIGHT;
			int num2 = wScroll - 1;
			int h = ITEM_HEIGHT - 1;
			if (num - cmy <= yScroll + hScroll && num - cmy >= yScroll - ITEM_HEIGHT)
			{
				g.setColor((i != selected) ? 15196114 : 16383818);
				g.fillRect(x, num, num2, h);
				mFont.tahoma_7b_dark.drawString(g, strCauhinh[i].Replace("\r\n", ""), xScroll + wScroll / 2, num + 6, mFont.CENTER);
			}
		}
		paintScrollArrow(g);
	}

	private void doFireOption()
	{
		if (selected < 0)
		{
			return;
		}
		switch (selected)
		{
		case 0:
			SoundMn.gI().AuraToolOption();
			break;
		case 1:
			SoundMn.gI().soundToolOption();
			break;
		case 2:
            //if (Main.isPC)
            //{
            //GameCanvas.startYesNoDlg(mResources.changeSizeScreen, new Command(mResources.YES, this, 170391, null), new Command(mResources.NO, this, 4005, null));
            //}
            //else if (GameScr.isAnalog == 0)
            //{
            //	strCauhinh[2] = mResources.turnOffAnalog;
            //	GameScr.isAnalog = 1;
            //	Rms.saveRMSInt("analog", GameScr.isAnalog);
            //	GameScr.setSkillBarPosition();
            //}
            //else
            //{
            //	strCauhinh[2] = mResources.turnOnAnalog;
            //	GameScr.isAnalog = 0;
            //	Rms.saveRMSInt("analog", GameScr.isAnalog);
            //	GameScr.setSkillBarPosition();
            //}
            GameCanvas.startYesNoDlg(mResources.changeCauHinh, new Command(mResources.YES, this, 170392, null), new Command(mResources.NO, this, 4005, null));
            break;
		case 3:
            GameCanvas.startYesNoDlg(mResources.changeSizeScreen, new Command(mResources.YES, this, 170391, null), new Command(mResources.NO, this, 4005, null));
			break;
        }
	}

	public void setTypeAccount()
	{
		type = 20;
		setType(0);
		setTabAccount();
		cmx = (cmtoX = 0);
	}

	private void setTabAccount()
	{
		if (Main.IphoneVersionApp)
		{
			strAccount = new string[4]
			{
				mResources.inventory_Pass,
				mResources.friend,
				mResources.enemy,
				mResources.msg
			};
			if (GameScr.canAutoPlay)
			{
				strAccount = new string[5]
				{
					mResources.inventory_Pass,
					mResources.friend,
					mResources.enemy,
					mResources.msg,
					mResources.autoFunction
				};
			}
		}
		else
		{
			strAccount = new string[5]
			{
				mResources.inventory_Pass,
				mResources.friend,
				mResources.enemy,
				mResources.msg,
				mResources.charger
			};
			if (GameScr.canAutoPlay)
			{
				strAccount = new string[6]
				{
					mResources.inventory_Pass,
					mResources.friend,
					mResources.enemy,
					mResources.msg,
					mResources.charger,
					mResources.autoFunction
				};
			}
			if ((mSystem.clientType == 2 || mSystem.clientType == 7) && mResources.language != 2)
			{
				strAccount = new string[5]
				{
					mResources.inventory_Pass,
					mResources.friend,
					mResources.enemy,
					mResources.msg,
					mResources.charger
				};
				if (GameScr.canAutoPlay)
				{
					strAccount = new string[6]
					{
						mResources.inventory_Pass,
						mResources.friend,
						mResources.enemy,
						mResources.msg,
						mResources.charger,
						mResources.autoFunction
					};
				}
			}
		}
		currentListLength = strAccount.Length;
		ITEM_HEIGHT = 24;
		selected = (GameCanvas.isTouch ? (-1) : 0);
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
	}

	private void paintAccount(mGraphics g)
	{
		g.setClip(xScroll, yScroll, wScroll, hScroll);
		g.translate(0, -cmy);
		for (int i = 0; i < strAccount.Length; i++)
		{
			int x = xScroll;
			int num = yScroll + i * ITEM_HEIGHT;
			int num2 = wScroll - 1;
			int h = ITEM_HEIGHT - 1;
			if (num - cmy <= yScroll + hScroll && num - cmy >= yScroll - ITEM_HEIGHT)
			{
				g.setColor((i != selected) ? 15196114 : 16383818);
				g.fillRect(x, num, num2, h);
				mFont.tahoma_7b_dark.drawString(g, strAccount[i], xScroll + wScroll / 2, num + 6, mFont.CENTER);
			}
		}
		paintScrollArrow(g);
	}

	private void doFireAccount()
	{
		if (selected < 0)
		{
			return;
		}
		switch (selected)
		{
		case 0:
			GameCanvas.endDlg();
			if (chatTField == null)
			{
				chatTField = new ChatTextField();
				chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
				chatTField.initChatTextField();
				chatTField.parentScreen = GameCanvas.panel;
			}
			chatTField.tfChat.setText(string.Empty);
			chatTField.strChat = mResources.input_Inventory_Pass;
			chatTField.tfChat.name = mResources.input_Inventory_Pass;
			chatTField.to = string.Empty;
			chatTField.isShow = true;
			chatTField.tfChat.isFocus = true;
			chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			if (GameCanvas.isTouch)
			{
				chatTField.tfChat.doChangeToTextBox();
			}
			if (!Main.isPC)
			{
				chatTField.startChat2(this, string.Empty);
			}
			if (Main.isWindowsPhone)
			{
				chatTField.tfChat.strInfo = chatTField.strChat;
			}
			break;
		case 1:
			Service.gI().friend(0, -1);
			InfoDlg.showWait();
			break;
		case 2:
			Service.gI().enemy(0, -1);
			InfoDlg.showWait();
			break;
		case 3:
			setTypeMessage();
			if (chatTField == null)
			{
				chatTField = new ChatTextField();
				chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
				chatTField.initChatTextField();
				chatTField.parentScreen = GameCanvas.panel;
			}
			break;
		case 4:
			if (mResources.language == 2)
			{
				string url = "http://dragonball.indonaga.com/coda/?username=" + GameCanvas.loginScr.tfUser.getText();
				hideNow();
				try
				{
					GameMidlet.instance.platformRequest(url);
					break;
				}
				catch (Exception ex)
				{
					ex.StackTrace.ToString();
					break;
				}
			}
			hideNow();
			if (Char.myCharz().taskMaint.taskId <= 10)
			{
				GameCanvas.startOKDlg(mResources.finishBomong);
			}
			else
			{
				MoneyCharge.gI().switchToMe();
			}
			break;
		case 5:
			setTypeAuto();
			break;
		}
	}

	private void updateKeyOption()
	{
		updateKeyScrollView();
	}

	public void setTypeSpeacialSkill()
	{
		type = 25;
		setType(0);
		setTabSpeacialSkill();
		currentTabIndex = 0;
	}

	private void setTabSpeacialSkill()
	{
		ITEM_HEIGHT = 24;
		currentListLength = Char.myCharz().infoSpeacialSkill[currentTabIndex].Length;
		cmyLim = currentListLength * ITEM_HEIGHT - hScroll;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = cmyLast[currentTabIndex]);
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		selected = (GameCanvas.isTouch ? (-1) : 0);
	}

	public bool isTypeShop()
	{
		if (type == 1)
		{
			return true;
		}
		return false;
	}

	private void doNotiRuby(int type)
	{
		try
		{
			currItem.buyRuby = int.Parse(chatTField.tfChat.getText());
		}
		catch (Exception)
		{
			GameCanvas.startOKDlg(mResources.input_money_wrong);
			chatTField.isShow = false;
			return;
		}
		Command cmdYes = new Command(mResources.YES, this, (type != 0) ? 11001 : 11000, null);
		Command cmdNo = new Command(mResources.NO, this, 11002, null);
		GameCanvas.startYesNoDlg(mResources.notiRuby, cmdYes, cmdNo);
	}

	public static void paintUpgradeEffect(int x, int y, int wItem, int hItem, int nline, int cl, mGraphics g)
	{
		try
		{
			int num = (wItem << 1) + (hItem << 1);
			int num2 = num / nline;
			nsize = sizeUpgradeEff.Length;
			if (nline > 4)
			{
				nsize = 2;
			}
			for (int i = 0; i < nline; i++)
			{
				for (int j = 0; j < nsize; j++)
				{
					int wSize = ((sizeUpgradeEff[j] <= 1) ? 1 : ((sizeUpgradeEff[j] >> 1) + 1));
					int x2 = x + upgradeEffectX(num2 * i, GameCanvas.gameTick - j * 4, wItem, hItem, wSize);
					int y2 = y + upgradeEffectY(num2 * i, GameCanvas.gameTick - j * 4, wItem, hItem, wSize);
					g.setColor(colorUpgradeEffect[cl][j]);
					g.fillRect(x2, y2, sizeUpgradeEff[j], sizeUpgradeEff[j]);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private static int upgradeEffectX(int dk, int tick, int wItem, int hitem, int wSize)
	{
		int num = (tick + dk) % ((wItem << 1) + (hitem << 1));
		if (0 <= num && num < wItem)
		{
			return num % wItem;
		}
		if (wItem <= num && num < wItem + hitem)
		{
			return wItem - wSize;
		}
		if (wItem + hitem <= num && num < (wItem << 1) + hitem)
		{
			return wItem - (num - hitem) % wItem - wSize;
		}
		return 0;
	}

	private static int upgradeEffectY(int dk, int tick, int wItem, int hitem, int wSize)
	{
		int num = (tick + dk) % ((wItem << 1) + (hitem << 1));
		if (0 <= num && num < wItem)
		{
			return 0;
		}
		if (wItem <= num && num < wItem + hitem)
		{
			return num % wItem;
		}
		if (wItem + hitem <= num && num < (wItem << 1) + hitem)
		{
			return hitem - wSize;
		}
		return hitem - (num - (wItem << 1)) % hitem - wSize;
	}

	public static int GetColor_ItemBg(int id)
	{
		return id switch
		{
			4 => 1269146, 
			1 => 2786816, 
			5 => 13279744, 
			3 => 12537346, 
			2 => 7078041, 
			6 => 11599872, 
			_ => -1, 
		};
	}

	public static sbyte GetColor_Item_Upgrade(int lv)
	{
		if (lv < 0)
		{
			return 0;
		}
		switch (lv)
		{
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
			return 0;
		case 9:
			return 4;
		case 10:
			return 1;
		case 11:
			return 5;
		case 12:
			return 3;
		case 13:
			return 2;
		default:
			return 6;
		}
	}

	public void paintOptItem(mGraphics g, int idOpt, int param, int x, int y, int w, int h)
	{
		switch (idOpt)
		{
		case 34:
			if (imgo_0 != null)
			{
				g.drawImage(imgo_0, x, y + h - imgo_0.getHeight());
			}
			else
			{
				imgo_0 = mSystem.loadImage("/mainImage/o_0.png");
			}
			if (imgo_1 != null)
			{
				g.drawImage(imgo_1, x, y + h - imgo_1.getHeight());
			}
			else
			{
				imgo_1 = mSystem.loadImage("/mainImage/o_1.png");
			}
			break;
		case 35:
			if (imgo_0 != null)
			{
				g.drawImage(imgo_0, x, y + h - imgo_0.getHeight());
			}
			else
			{
				imgo_0 = mSystem.loadImage("/mainImage/o_0.png");
			}
			if (imgo_2 != null)
			{
				g.drawImage(imgo_2, x, y + h - imgo_2.getHeight());
			}
			else
			{
				imgo_2 = mSystem.loadImage("/mainImage/o_2.png");
			}
			break;
		case 36:
			if (imgo_0 != null)
			{
				g.drawImage(imgo_0, x, y + h - imgo_0.getHeight());
			}
			else
			{
				imgo_0 = mSystem.loadImage("/mainImage/o_0.png");
			}
			if (imgo_3 != null)
			{
				g.drawImage(imgo_3, x, y + h - imgo_3.getHeight());
			}
			else
			{
				imgo_3 = mSystem.loadImage("/mainImage/o_3.png");
			}
			break;
		}
	}

	public void paintOptSlotItem(mGraphics g, int idOpt, int param, int x, int y, int w, int h)
	{
		if (idOpt == 102 && param > ChatPopup.numSlot)
		{
			sbyte color_Item_Upgrade = GetColor_Item_Upgrade(param);
			int nline = param - ChatPopup.numSlot;
			paintUpgradeEffect(x, y, w, h, nline, color_Item_Upgrade, g);
		}
	}

	public static mFont setTextColor(int id, int type)
	{
		if (type == 0)
		{
			return id switch
			{
				0 => mFont.bigNumber_While, 
				1 => mFont.bigNumber_green, 
				3 => mFont.bigNumber_orange, 
				4 => mFont.bigNumber_blue, 
				5 => mFont.bigNumber_yellow, 
				6 => mFont.bigNumber_red, 
				_ => mFont.bigNumber_While, 
			};
		}
		return id switch
		{
			0 => mFont.tahoma_7b_white, 
			1 => mFont.tahoma_7b_green, 
			3 => mFont.tahoma_7b_yellowSmall2, 
			4 => mFont.tahoma_7b_blue, 
			5 => mFont.tahoma_7b_yellow, 
			6 => mFont.tahoma_7b_red, 
			7 => mFont.tahoma_7b_dark, 
			_ => mFont.tahoma_7b_white, 
		};
	}
}
