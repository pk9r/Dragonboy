using System;
using Assets.src.e;
using Assets.src.g;
using Mod;
using UnityEngine;

public class Char : IMapObject
{
	public CharEffectTime charEffectTime = new CharEffectTime(); 

	public string xuStr;

	public string luongStr;

	public string luongKhoaStr;

	public long lastUpdateTime;

	public bool meLive;

	public bool isMask;

	public bool isTeleport;

	public bool isUsePlane;

	public int shadowX;

	public int shadowY;

	public int shadowLife;

	public bool isNhapThe;

	public PetFollow petFollow;

	public int rank;

	public const sbyte A_STAND = 1;

	public const sbyte A_RUN = 2;

	public const sbyte A_JUMP = 3;

	public const sbyte A_FALL = 4;

	public const sbyte A_DEADFLY = 5;

	public const sbyte A_NOTHING = 6;

	public const sbyte A_ATTK = 7;

	public const sbyte A_INJURE = 8;

	public const sbyte A_AUTOJUMP = 9;

	public const sbyte A_FLY = 10;

	public const sbyte SKILL_STAND = 12;

	public const sbyte SKILL_FALL = 13;

	public const sbyte A_DEAD = 14;

	public const sbyte A_HIDE = 15;

	public const sbyte A_RESETPOINT = 16;

	public static ChatPopup chatPopup;

	public long cPower;

	public Info chatInfo;

	public sbyte petStatus;

	public int cx = 24;

	public int cy = 24;

	public int cvx;

	public int cvy;

	public int cp1;

	public int cp2;

	public int cp3;

	public int statusMe = 5;

	public int cdir = 1;

	public int charID;

	public int cgender;

	public int ctaskId;

	public int menuSelect;

	public int cBonusSpeed;

	public int cspeed = 4;

	public int ccurrentAttack;

	public int cDamFull;

	public int cDefull;

	public int cCriticalFull;

	public int clevel;

	public int cMP;

	public int cHP;

	public int cHPNew;

	public int cMaxEXP;

	public int cHPShow;

	public int xReload;

	public int yReload;

	public int cyStartFall;

	public int saveStatus;

	public int eff5BuffHp;

	public int eff5BuffMp;

	public int cHPFull;

	public int cMPFull;

	public int cdameDown;

	public int cStr;

	public long cLevelPercent;

	public long cTiemNang;

	public long cNangdong;

	public int damHP;

	public int damMP;

	public bool isMob;

	public bool isCrit;

	public bool isDie;

	public int pointUydanh;

	public int pointNon;

	public int pointVukhi;

	public int pointAo;

	public int pointLien;

	public int pointGangtay;

	public int pointNhan;

	public int pointQuan;

	public int pointNgocboi;

	public int pointGiay;

	public int pointPhu;

	public int countFinishDay;

	public int countLoopBoos;

	public int limitTiemnangso;

	public int limitKynangso;

	public short[] potential = new short[4];

	public string cName = string.Empty;

	public int clanID;

	public sbyte ctypeClan;

	public Clan clan;

	public sbyte role;

	public int cw = 22;

	public int ch = 32;

	public int chw = 11;

	public int chh = 16;

	public Command cmdMenu;

	public bool canFly = true;

	public bool cmtoChar;

	public bool me;

	public bool cFinishedAttack;

	public bool cchistlast;

	public bool isAttack;

	public bool isAttFly;

	public int cwpt;

	public int cwplv;

	public int cf;

	public int tick;

	public static bool fallAttack;

	public bool isJump;

	public bool autoFall;

	public bool attack = true;

	public long xu;

	public int xuInBox;

	public int yen;

	public int gold_lock;

	public int luong;

	public int luongKhoa;

	public NClass nClass;

	public Command endMovePointCommand;

	public MyVector vSkill = new MyVector();

	public MyVector vSkillFight = new MyVector();

	public MyVector vEff = new MyVector();

	public Skill myskill;

	public Task taskMaint;

	public bool paintName = true;

	public Archivement[] arrArchive;

	public Item[] arrItemBag;

	public Item[] arrItemBox;

	public Item[] arrItemBody;

	public Skill[] arrPetSkill;

	public Item[][] arrItemShop;

	public string[][] infoSpeacialSkill;

	public short[][] imgSpeacialSkill;

	public short cResFire;

	public short cResIce;

	public short cResWind;

	public short cMiss;

	public short cExactly;

	public short cFatal;

	public sbyte cPk;

	public sbyte cTypePk;

	public short cReactDame;

	public short sysUp;

	public short sysDown;

	public int avatar;

	public int skillTemplateId;

	public Mob mobFocus;

	public Mob mobMe;

	public int tMobMeBorn;

	public Npc npcFocus;

	public Char charFocus;

	public ItemMap itemFocus;

	public MyVector focus = new MyVector();

	public Mob[] attMobs;

	public Char[] attChars;

	public short[] moveFast;

	public int testCharId = -9999;

	public int killCharId = -9999;

	public sbyte resultTest;

	public int countKill;

	public int countKillMax;

	public bool isInvisiblez;

	public bool isShadown = true;

	public const sbyte PK_NORMAL = 0;

	public const sbyte PK_PHE = 1;

	public const sbyte PK_BANG = 2;

	public const sbyte PK_THIDAU = 3;

	public const sbyte PK_LUYENTAP = 4;

	public const sbyte PK_TUDO = 5;

	public MyVector taskOrders = new MyVector();

	public int cStamina;

	public static short[] idHead;

	public static short[] idAvatar;

	public int exp;

	public string[] strLevel;

	public string currStrLevel;

	public static Image eyeTraiDat = GameCanvas.loadImage("/mainImage/myTexture2dmat-trai-dat.png");

	public static Image eyeNamek = GameCanvas.loadImage("/mainImage/myTexture2dmat-namek.png");

	public bool isFreez;

	public bool isCharge;

	public int seconds;

	public int freezSeconds;

	public long last;

	public long cur;

	public long lastFreez;

	public long currFreez;

	public bool isFlyUp;

	public static MyVector vItemTime = new MyVector();

	public static short ID_NEW_MOUNT = 30000;

	public short idMount;

	public bool isHaveMount;

	public bool isMountVip;

	public bool isEventMount;

	public bool isSpeacialMount;

	public static Image imgMount_TD = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi10.png");

	public static Image imgMount_NM = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi20.png");

	public static Image imgMount_NM_1 = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi21.png");

	public static Image imgMount_XD = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi30.png");

	public static Image imgMount_TD_VIP = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi11.png");

	public static Image imgMount_NM_VIP = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi22.png");

	public static Image imgMount_NM_1_VIP = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi23.png");

	public static Image imgMount_XD_VIP = GameCanvas.loadImage("/mainImage/myTexture2dthucuoi31.png");

	public static Image imgEventMount = GameCanvas.loadImage("/mainImage/myTexture2drong.png");

	public static Image imgEventMountWing = GameCanvas.loadImage("/mainImage/myTexture2dcanhrong.png");

	public sbyte[] FrameMount = new sbyte[8] { 0, 0, 1, 1, 2, 2, 1, 1 };

	public int frameMount;

	public int frameNewMount;

	public int transMount;

	public int genderMount;

	public int idcharMount;

	public int xMount;

	public int yMount;

	public int dxMount;

	public int dyMount;

	public int xChar;

	public int xdis;

	public int speedMount;

	public bool isStartMount;

	public bool isMount;

	public bool isEndMount;

	public sbyte cFlag;

	public int flagImage;

	public static int[][][] CharInfo = new int[33][][]
	{
		new int[4][]
		{
			new int[3] { 0, -13, 34 },
			new int[3] { 1, -8, 10 },
			new int[3] { 1, -9, 16 },
			new int[3] { 1, -9, 45 }
		},
		new int[4][]
		{
			new int[3] { 0, -13, 35 },
			new int[3] { 1, -8, 10 },
			new int[3] { 1, -9, 17 },
			new int[3] { 1, -9, 46 }
		},
		new int[4][]
		{
			new int[3] { 1, -10, 33 },
			new int[3] { 2, -10, 11 },
			new int[3] { 2, -8, 16 },
			new int[3] { 1, -12, 49 }
		},
		new int[4][]
		{
			new int[3] { 1, -10, 32 },
			new int[3] { 3, -12, 10 },
			new int[3] { 3, -11, 15 },
			new int[3] { 1, -13, 47 }
		},
		new int[4][]
		{
			new int[3] { 1, -10, 34 },
			new int[3] { 4, -8, 11 },
			new int[3] { 4, -7, 17 },
			new int[3] { 1, -12, 47 }
		},
		new int[4][]
		{
			new int[3] { 1, -10, 34 },
			new int[3] { 5, -12, 11 },
			new int[3] { 5, -9, 17 },
			new int[3] { 1, -13, 49 }
		},
		new int[4][]
		{
			new int[3] { 1, -10, 33 },
			new int[3] { 6, -10, 10 },
			new int[3] { 6, -8, 16 },
			new int[3] { 1, -12, 47 }
		},
		new int[4][]
		{
			new int[3] { 0, -9, 36 },
			new int[3] { 7, -5, 17 },
			new int[3] { 7, -11, 25 },
			new int[3] { 1, -8, 49 }
		},
		new int[4][]
		{
			new int[3] { 0, -7, 35 },
			new int[3] { 0, -18, 22 },
			new int[3] { 7, -10, 25 },
			new int[3] { 1, -7, 48 }
		},
		new int[4][]
		{
			new int[3] { 1, -11, 35 },
			new int[3] { 10, -3, 25 },
			new int[3] { 12, -10, 26 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -11, 37 },
			new int[3] { 11, -3, 25 },
			new int[3] { 12, -11, 27 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -14, 34 },
			new int[3] { 12, -8, 21 },
			new int[3] { 9, -7, 31 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -12, 35 },
			new int[3] { 8, -5, 14 },
			new int[3] { 8, -15, 29 },
			new int[3] { 1, -9, 49 }
		},
		new int[4][]
		{
			new int[3] { 1, -9, 34 },
			new int[3] { 9, -12, 9 },
			new int[3] { 10, -7, 19 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -13, 34 },
			new int[3] { 9, -12, 9 },
			new int[3] { 11, -10, 19 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -8, 32 },
			new int[3] { 9, -12, 9 },
			new int[3] { 2, -6, 15 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -8, 32 },
			new int[3] { 9, -12, 9 },
			new int[3] { 13, -12, 16 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -10, 31 },
			new int[3] { 9, -12, 9 },
			new int[3] { 7, -13, 20 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -11, 32 },
			new int[3] { 9, -12, 9 },
			new int[3] { 8, -15, 26 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -9, 33 },
			new int[3] { 9, -12, 9 },
			new int[3] { 14, -8, 18 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -11, 33 },
			new int[3] { 9, -12, 9 },
			new int[3] { 15, -6, 19 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -16, 31 },
			new int[3] { 9, -12, 9 },
			new int[3] { 9, -8, 28 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -14, 34 },
			new int[3] { 1, -8, 10 },
			new int[3] { 8, -16, 28 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -8, 36 },
			new int[3] { 7, -5, 17 },
			new int[3] { 0, -5, 25 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -9, 31 },
			new int[3] { 9, -12, 9 },
			new int[3] { 0, -6, 20 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 2, -9, 36 },
			new int[3] { 13, -5, 17 },
			new int[3] { 16, -11, 25 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -9, 34 },
			new int[3] { 8, -5, 13 },
			new int[3] { 10, -7, 19 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -13, 34 },
			new int[3] { 8, -5, 13 },
			new int[3] { 11, -10, 19 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -8, 32 },
			new int[3] { 8, -5, 13 },
			new int[3] { 2, -6, 15 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 1, -8, 32 },
			new int[3] { 8, -5, 13 },
			new int[3] { 13, -12, 16 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -9, 33 },
			new int[3] { 8, -5, 13 },
			new int[3] { 14, -8, 18 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -11, 33 },
			new int[3] { 8, -5, 13 },
			new int[3] { 15, -6, 19 },
			new int[3]
		},
		new int[4][]
		{
			new int[3] { 0, -16, 32 },
			new int[3] { 8, -5, 13 },
			new int[3] { 9, -8, 29 },
			new int[3]
		}
	};

	public static int[] CHAR_WEAPONX = new int[11]
	{
		-2, -6, 22, 21, 19, 22, 10, -2, -2, 5,
		19
	};

	public static int[] CHAR_WEAPONY = new int[11]
	{
		9, 22, 25, 17, 26, 37, 36, 49, 50, 52,
		36
	};

	private static Char myChar;

	private static Char myPet;

	public static int[] listAttack;

	public static int[][] listIonC;

	public int cvyJump;

	private int indexUseSkill = -1;

	public int cxSend;

	public int cySend;

	public int cdirSend = 1;

	public int cxFocus;

	public int cyFocus;

	public int cactFirst = 5;

	public MyVector vMovePoints = new MyVector();

	public static string[][] inforClass = new string[2][]
	{
		new string[4] { "1", "1", "chiêu 1", "0" },
		new string[4] { "2", "2", "chiêu 2", "5" }
	};

	public static int[][] inforSkill = new int[10][]
	{
		new int[12]
		{
			1, 0, 1, 1000, 40, 1, 0, 20, 0, 0,
			0, 0
		},
		new int[12]
		{
			2, 1, 10, 1000, 100, 1, 0, 40, 0, 0,
			0, 0
		},
		new int[12]
		{
			2, 2, 11, 800, 100, 1, 0, 45, 0, 0,
			0, 0
		},
		new int[12]
		{
			2, 3, 12, 600, 100, 1, 0, 50, 0, 0,
			0, 0
		},
		new int[12]
		{
			2, 4, 13, 500, 100, 1, 0, 55, 0, 0,
			0, 0
		},
		new int[12]
		{
			3, 1, 14, 500, 100, 1, 0, 60, 0, 0,
			0, 0
		},
		new int[12]
		{
			3, 2, 14, 500, 100, 1, 0, 60, 0, 0,
			0, 0
		},
		new int[12]
		{
			3, 3, 14, 500, 100, 1, 0, 60, 0, 0,
			0, 0
		},
		new int[12]
		{
			3, 4, 14, 500, 100, 1, 0, 60, 0, 0,
			0, 0
		},
		new int[12]
		{
			3, 5, 14, 500, 100, 1, 0, 60, 0, 0,
			0, 0
		}
	};

	public static bool flag;

	public static bool ischangingMap;

	public static bool isLockKey;

	public static bool isLoadingMap;

	public bool isLockMove;

	public bool isLockAttack;

	public string strInfo;

	public short powerPoint;

	public short maxPowerPoint;

	public short secondPower;

	public long lastS;

	public long currS;

	public bool havePet = true;

	public MovePoint currentMovePoint;

	public int bom;

	public int delayFall;

	private bool isSoundJump;

	public int lastFrame;

	private Effect eProtect;

	private int twHp;

	public bool isInjureHp;

	public bool changePos;

	private bool isHide;

	private bool wy;

	public int wt;

	public int fy;

	public int ty;

	private int t;

	private int fM;

	public int[] move = new int[15]
	{
		1, 1, 1, 1, 2, 2, 2, 2, 3, 3,
		3, 3, 2, 2, 2
	};

	private string strMount = "mount_";

	public int headICON = -1;

	public int head;

	public int leg;

	public int body;

	public int bag;

	public int wp;

	public int indexEff = -1;

	public int indexEffTask = -1;

	public EffectCharPaint eff;

	public EffectCharPaint effTask;

	public int indexSkill;

	public int i0;

	public int i1;

	public int i2;

	public int dx0;

	public int dx1;

	public int dx2;

	public int dy0;

	public int dy1;

	public int dy2;

	public EffectCharPaint eff0;

	public EffectCharPaint eff1;

	public EffectCharPaint eff2;

	public Arrow arr;

	public PlayerDart dart;

	public bool isCreateDark;

	public SkillPaint skillPaint;

	public SkillPaint skillPaintRandomPaint;

	public EffectPaint[] effPaints;

	public int sType;

	public sbyte isInjure;

	public bool isUseSkillAfterCharge;

	public bool isFlyAndCharge;

	public bool isStandAndCharge;

	private bool isFlying;

	public int posDisY;

	private int chargeCount;

	private bool hasSendAttack;

	public bool isMabuHold;

	private long timeBlue;

	private int tBlue;

	private bool IsAddDust1;

	private bool IsAddDust2;

	private bool isPet;

	private bool isMiniPet;

	public int xSd;

	public int ySd;

	private bool isOutMap;

	private int statusBeforeNothing;

	private int timeFocusToMob;

	public static bool isManualFocus = false;

	private Char charHold;

	private Mob mobHold;

	private int nInjure;

	public short wdx;

	public short wdy;

	public bool isDirtyPostion;

	public Skill lastNormalSkill;

	public bool currentFireByShortcut;

	public int cDamGoc;

	public int cHPGoc;

	public int cMPGoc;

	public int cDefGoc;

	public int cCriticalGoc;

	public sbyte hpFrom1000TiemNang;

	public sbyte mpFrom1000TiemNang;

	public sbyte damFrom1000TiemNang;

	public sbyte defFrom1000TiemNang = 1;

	public sbyte criticalFrom1000Tiemnang = 1;

	public short cMaxStamina;

	public short expForOneAdd;

	public sbyte isMonkey;

	public bool isCopy;

	public bool isWaitMonkey;

	private bool isFeetEff;

	public bool meDead;

	public int holdEffID;

	public bool holder;

	public bool protectEff;

	private bool isSetPos;

	private int tpos;

	private short xPos;

	private short yPos;

	private sbyte typePos;

	private bool isMyFusion;

	public bool isFusion;

	public int tFusion;

	public bool huytSao;

	public bool blindEff;

	public bool telePortSkill;

	public bool sleepEff;

	public bool stone;

	public int perCentMp = 100;

	public int dHP;

	public int headTemp = -1;

	public int bodyTemp = -1;

	public int legTemp = -1;

	public int bagTemp = -1;

	public int wpTemp = -1;

	public MyVector vEffChar = new MyVector("vEff");

	public static FrameImage fraRedEye;

	private int fChopmat;

	private bool isAddChopMat;

	private long timeAddChopmat;

	private int[] frChopNhanh = new int[34]
	{
		-1, -1, -1, -1, 0, 0, 1, 1, 0, 0,
		1, 1, 0, 0, 1, 1, 0, 0, 1, 1,
		0, 0, 1, 1, 0, 0, 1, 1, 0, 0,
		-1, -1, -1, -1
	};

	private int[] frChopCham = new int[23]
	{
		-1, -1, -1, -1, 0, 0, 1, 1, 1, 0,
		0, 1, 1, 1, 0, 0, 1, 1, 1, -1,
		-1, -1, -1
	};

	private int[] frEye = new int[30]
	{
		-1, -1, 0, 0, 1, 1, 0, 0, 1, 1,
		0, 0, 1, 1, 0, 0, 1, 1, 0, 0,
		1, 1, 0, 0, 1, 1, 0, 0, -1, -1
	};

	public static int[][] Arr_Head_2Fr = new int[1][] { new int[2] { 542, 543 } };

	private int fHead;

	private string strEffAura = "aura_";

	public short idAuraEff = -1;

	public static bool isPaintAura = true;

	private FrameImage fraEff;

	private FrameImage fraEffSub;

	private string strEff_Set_Item = "set_eff_";

	public short idEff_Set_Item = -1;

	public Char()
	{
		statusMe = 6;
	}

	public void applyCharLevelPercent()
	{
		try
		{
			long num = 1L;
			long num2 = 0L;
			int num3 = 0;
			for (int num4 = GameScr.exps.Length - 1; num4 >= 0; num4--)
			{
				if (cPower >= GameScr.exps[num4])
				{
					num = ((num4 != GameScr.exps.Length - 1) ? (GameScr.exps[num4 + 1] - GameScr.exps[num4]) : 1);
					num2 = cPower - GameScr.exps[num4];
					num3 = num4;
					break;
				}
			}
			clevel = num3;
			cLevelPercent = (int)(num2 * 10000 / num);
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi char level percent: " + ex.ToString());
		}
	}

	public int getdxSkill()
	{
		if (myskill != null)
		{
			return myskill.dx;
		}
		return 0;
	}

	public int getdySkill()
	{
		if (myskill != null)
		{
			return myskill.dy;
		}
		return 0;
	}

	public static void taskAction(bool isNextStep)
	{
		Task task = myCharz().taskMaint;
		if (task.index > task.contentInfo.Length - 1)
		{
			task.index = task.contentInfo.Length - 1;
		}
		string text = task.contentInfo[task.index];
		if (text != null && !text.Equals(string.Empty))
		{
			if (text.StartsWith("#"))
			{
				text = NinjaUtil.replace(text, "#", string.Empty);
				Npc npc = new Npc(5, 0, -100, -100, 5, GameScr.info1.charId[myCharz().cgender][2]);
				npc.cx = (npc.cy = -100);
				npc.avatar = GameScr.info1.charId[myCharz().cgender][2];
				npc.charID = 5;
				if (GameCanvas.currentScreen == GameScr.instance)
				{
					ChatPopup.addNextPopUpMultiLine(text, npc);
				}
			}
			else if (isNextStep)
			{
				GameScr.info1.addInfo(text, 0);
			}
		}
		GameScr.isHaveSelectSkill = true;
		Cout.println("TASKx " + myCharz().taskMaint.taskId);
		if (myCharz().taskMaint.taskId <= 2)
		{
			myCharz().canFly = false;
		}
		else
		{
			myCharz().canFly = true;
		}
		GameScr.gI().left = null;
		if (task.taskId == 0)
		{
			Hint.isViewMap = false;
			GameScr.gI().right = null;
			GameScr.isHaveSelectSkill = false;
			GameScr.gI().left = null;
			if (task.index < 4)
			{
				MagicTree.isPaint = false;
				GameScr.isPaintRada = -1;
			}
			if (task.index == 4)
			{
				GameScr.isPaintRada = 1;
				MagicTree.isPaint = true;
			}
			if (task.index >= 5)
			{
				GameScr.gI().right = GameScr.gI().cmdFocus;
			}
		}
		if (task.taskId == 1)
		{
			GameScr.isHaveSelectSkill = true;
		}
		if (task.taskId >= 1)
		{
			GameScr.gI().right = GameScr.gI().cmdFocus;
			GameScr.gI().left = GameScr.gI().cmdMenu;
		}
		if (task.taskId >= 0)
		{
			Panel.isPaintMap = true;
		}
		else
		{
			Panel.isPaintMap = false;
		}
		if (task.taskId < 12)
		{
			GameCanvas.panel.mainTabName = mResources.mainTab1;
		}
		else
		{
			GameCanvas.panel.mainTabName = mResources.mainTab2;
		}
		GameCanvas.panel.tabName[0] = GameCanvas.panel.mainTabName;
		if (myChar.taskMaint.taskId > 10)
		{
			Rms.saveRMSString("fake", "aa");
		}
	}

	public string getStrLevel()
	{
		return strLevel[clevel] + "+" + cLevelPercent / 100 + "." + cLevelPercent % 100 + "%";
	}

	public int avatarz()
	{
		return getAvatar(head);
	}

	public int getAvatar(int headId)
	{
		for (int i = 0; i < idHead.Length; i++)
		{
			if (headId == idHead[i])
			{
				return idAvatar[i];
			}
		}
		return -1;
	}

	public void setPowerInfo(string info, short p, short maxP, short sc)
	{
		powerPoint = p;
		strInfo = info;
		maxPowerPoint = maxP;
		secondPower = sc;
		lastS = (currS = mSystem.currentTimeMillis());
	}

	public void addInfo(string info)
	{
		if (chatInfo == null)
		{
			chatInfo = new Info();
		}
		Char cInfo = null;
		chatInfo.addInfo(info, 0, cInfo, isChatServer: false);
	}

	public int getSys()
	{
		if (nClass.classId == 1 || nClass.classId == 2)
		{
			return 1;
		}
		if (nClass.classId == 3 || nClass.classId == 4)
		{
			return 2;
		}
		if (nClass.classId == 5 || nClass.classId == 6)
		{
			return 3;
		}
		return 0;
	}

	public static Char myCharz()
	{
		if (myChar == null)
		{
			myChar = new Char();
			myChar.me = true;
			myChar.cmtoChar = true;
		}
		return myChar;
	}

	public static Char myPetz()
	{
		if (myPet == null)
		{
			myPet = new Char();
			myPet.me = false;
		}
		return myPet;
	}

	public static void clearMyChar()
	{
		myChar = null;
	}

	public void bagSort()
	{
		try
		{
			MyVector myVector = new MyVector();
			for (int i = 0; i < arrItemBag.Length; i++)
			{
				Item item = arrItemBag[i];
				if (item != null && item.template.isUpToUp && !item.isExpires)
				{
					myVector.addElement(item);
				}
			}
			for (int j = 0; j < myVector.size(); j++)
			{
				Item item2 = (Item)myVector.elementAt(j);
				if (item2 == null)
				{
					continue;
				}
				for (int k = j + 1; k < myVector.size(); k++)
				{
					Item item3 = (Item)myVector.elementAt(k);
					if (item3 != null && item2.template.Equals(item3.template) && item2.isLock == item3.isLock)
					{
						item2.quantity += item3.quantity;
						arrItemBag[item3.indexUI] = null;
						myVector.setElementAt(null, k);
					}
				}
			}
			for (int l = 0; l < arrItemBag.Length; l++)
			{
				if (arrItemBag[l] == null)
				{
					continue;
				}
				for (int m = 0; m <= l; m++)
				{
					if (arrItemBag[m] == null)
					{
						arrItemBag[m] = arrItemBag[l];
						arrItemBag[m].indexUI = m;
						arrItemBag[l] = null;
						break;
					}
				}
			}
		}
		catch (Exception)
		{
			Cout.println("Char.bagSort()");
		}
	}

	public void boxSort()
	{
		try
		{
			MyVector myVector = new MyVector();
			for (int i = 0; i < arrItemBox.Length; i++)
			{
				Item item = arrItemBox[i];
				if (item != null && item.template.isUpToUp && !item.isExpires)
				{
					myVector.addElement(item);
				}
			}
			for (int j = 0; j < myVector.size(); j++)
			{
				Item item2 = (Item)myVector.elementAt(j);
				if (item2 == null)
				{
					continue;
				}
				for (int k = j + 1; k < myVector.size(); k++)
				{
					Item item3 = (Item)myVector.elementAt(k);
					if (item3 != null && item2.template.Equals(item3.template) && item2.isLock == item3.isLock)
					{
						item2.quantity += item3.quantity;
						arrItemBox[item3.indexUI] = null;
						myVector.setElementAt(null, k);
					}
				}
			}
			for (int l = 0; l < arrItemBox.Length; l++)
			{
				if (arrItemBox[l] == null)
				{
					continue;
				}
				for (int m = 0; m <= l; m++)
				{
					if (arrItemBox[m] == null)
					{
						arrItemBox[m] = arrItemBox[l];
						arrItemBox[m].indexUI = m;
						arrItemBox[l] = null;
						break;
					}
				}
			}
		}
		catch (Exception)
		{
			Cout.println("Char.boxSort()");
		}
	}

	public void useItem(int indexUI)
	{
		Item item = arrItemBag[indexUI];
		if (!item.isTypeBody())
		{
			return;
		}
		item.isLock = true;
		item.typeUI = 5;
		Item item2 = arrItemBody[item.template.type];
		arrItemBag[indexUI] = null;
		if (item2 != null)
		{
			item2.typeUI = 3;
			arrItemBody[item.template.type] = null;
			item2.indexUI = indexUI;
			arrItemBag[indexUI] = item2;
		}
		item.indexUI = item.template.type;
		arrItemBody[item.indexUI] = item;
		for (int i = 0; i < arrItemBody.Length; i++)
		{
			Item item3 = arrItemBody[i];
			if (item3 != null)
			{
				if (item3.template.type == 0)
				{
					body = item3.template.part;
				}
				else if (item3.template.type == 1)
				{
					leg = item3.template.part;
				}
			}
		}
	}

	public Skill getSkill(SkillTemplate skillTemplate)
	{
		for (int i = 0; i < vSkill.size(); i++)
		{
			if (((Skill)vSkill.elementAt(i)).template.id == skillTemplate.id)
			{
				return (Skill)vSkill.elementAt(i);
			}
		}
		return null;
	}

	public Waypoint isInEnterOfflinePoint()
	{
		Task task = myChar.taskMaint;
		if (task != null && task.taskId == 0 && task.index < 6)
		{
			return null;
		}
		int num = TileMap.vGo.size();
		for (sbyte b = 0; b < num; b = (sbyte)(b + 1))
		{
			Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(b);
			if (PopUp.vPopups.size() >= num)
			{
				PopUp popUp = (PopUp)PopUp.vPopups.elementAt(b);
				if (!popUp.isPaint)
				{
					return null;
				}
			}
			if (cx >= waypoint.minX && cx <= waypoint.maxX && cy >= waypoint.minY && cy <= waypoint.maxY && waypoint.isEnter && waypoint.isOffline)
			{
				return waypoint;
			}
		}
		return null;
	}

	public Waypoint isInEnterOnlinePoint()
	{
		Task task = myChar.taskMaint;
		if (task != null && task.taskId == 0 && task.index < 6)
		{
			return null;
		}
		int num = TileMap.vGo.size();
		for (sbyte b = 0; b < num; b = (sbyte)(b + 1))
		{
			Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(b);
			if (PopUp.vPopups.size() >= num)
			{
				PopUp popUp = (PopUp)PopUp.vPopups.elementAt(b);
				if (!popUp.isPaint)
				{
					return null;
				}
			}
			if (cx >= waypoint.minX && cx <= waypoint.maxX && cy >= waypoint.minY && cy <= waypoint.maxY && waypoint.isEnter && !waypoint.isOffline)
			{
				return waypoint;
			}
		}
		return null;
	}

	public bool isInWaypoint()
	{
		if (TileMap.isInAirMap() && cy >= TileMap.pxh - 48)
		{
			return true;
		}
		if (isTeleport || isUsePlane)
		{
			return false;
		}
		int num = TileMap.vGo.size();
		for (sbyte b = 0; b < num; b = (sbyte)(b + 1))
		{
			Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(b);
			if ((TileMap.mapID == 47 || TileMap.isInAirMap()) && cy <= waypoint.minY + waypoint.maxY && cx > waypoint.minX && cx < waypoint.maxX)
			{
				if (TileMap.isInAirMap() && cTypePk != 0)
				{
					return false;
				}
				return true;
			}
			if (cx >= waypoint.minX && cx <= waypoint.maxX && cy >= waypoint.minY && cy <= waypoint.maxY && !waypoint.isEnter)
			{
				return true;
			}
		}
		return false;
	}

	public bool isPunchKickSkill()
	{
		if (skillPaint == null)
		{
			return false;
		}
		if (skillPaint.id >= 0 && skillPaint.id <= 6)
		{
			return true;
		}
		if (skillPaint.id >= 14 && skillPaint.id <= 20)
		{
			return true;
		}
		if (skillPaint.id >= 28 && skillPaint.id <= 34)
		{
			return true;
		}
		if (skillPaint.id >= 63 && skillPaint.id <= 69)
		{
			return true;
		}
		return false;
	}

	public void soundUpdate()
	{
		if (me && statusMe == 10 && cf == 8 && ty > 20 && GameCanvas.gameTick % 20 == 0)
		{
			SoundMn.gI().charFly();
		}
		if (skillPaint != null && skillInfoPaint() != null && indexSkill < skillInfoPaint().Length && isPunchKickSkill() && (me || (!me && cx >= GameScr.cmx && cx <= GameScr.cmx + GameCanvas.w)) && GameCanvas.gameTick % 5 == 0)
		{
			if (cf == 9 || cf == 10 || cf == 11)
			{
				SoundMn.gI().charPunch(isKick: true, (!me) ? 0.05f : 0.1f);
			}
			else
			{
				SoundMn.gI().charPunch(isKick: false, (!me) ? 0.05f : 0.1f);
			}
		}
	}

	public void updateChargeSkill()
	{
	}

	public virtual void update()
	{
		if (charEffectTime.HasAnyEffect() && !CharEffect.isContains(charID)) CharEffect.storedChars.Add(this);
		charEffectTime.Update();
		if (head == 412 && body == 413 && leg == 414)
		{
			if (!charEffectTime.isChocolate)
			{
				charEffectTime.isChocolate = true;
				charEffectTime.timeChocolate = 32;
			}
		}
		else
		{
			charEffectTime.isChocolate = false;
			charEffectTime.timeChocolate = 0;
		}
        if (Utilities.isMeInNRDMap() && bag >= 0 && ClanImage.idImages.containsKey(bag.ToString() + string.Empty))
        {
            ClanImage clanImage = (ClanImage)ClanImage.idImages.get(bag.ToString() + string.Empty);
            bool isResetNRD = true;
            if (clanImage.idImage != null)
            {                
                for (int i = 0; i < clanImage.idImage.Length; i++)
                {
                    if (clanImage.idImage[i] == 2322)
                    {
                        charEffectTime.hasNRD = true;
                        isResetNRD = false;
                        if (charEffectTime.timeHoldingNRD == 0)
                        {
                            charEffectTime.timeHoldingNRD = 302;
                        }
                        break;
                    }
                }
            }
            if (isResetNRD)
            {
                charEffectTime.hasNRD = false;
                charEffectTime.timeHoldingNRD = 0;
            }
        }
        if (isHide || isMabuHold)
		{
			return;
		}
		if ((!isCopy && clevel < 14) || statusMe == 1 || statusMe == 6)
		{
		}
		if (petFollow != null)
		{
			if (GameCanvas.gameTick % 3 == 0)
			{
				if (myCharz().cdir == 1)
				{
					petFollow.cmtoX = cx - 20;
				}
				if (myCharz().cdir == -1)
				{
					petFollow.cmtoX = cx + 20;
				}
				petFollow.cmtoY = cy - 40;
				if (petFollow.cmx > cx)
				{
					petFollow.dir = -1;
				}
				else
				{
					petFollow.dir = 1;
				}
				if (petFollow.cmtoX < 100)
				{
					petFollow.cmtoX = 100;
				}
				if (petFollow.cmtoX > TileMap.pxw - 100)
				{
					petFollow.cmtoX = TileMap.pxw - 100;
				}
			}
			petFollow.update();
		}
		if (!me && cHP <= 0 && clanID != -100 && statusMe != 14 && statusMe != 5)
		{
			startDie((short)cx, (short)cy);
		}
		if (isInjureHp)
		{
			twHp++;
			if (twHp == 20)
			{
				twHp = 0;
				isInjureHp = false;
			}
		}
		else if (dHP > cHP)
		{
			int num = dHP - cHP >> 1;
			if (num < 1)
			{
				num = 1;
			}
			dHP -= num;
		}
		else
		{
			dHP = cHP;
		}
		if (secondPower != 0)
		{
			currS = mSystem.currentTimeMillis();
			if (currS - lastS >= 1000)
			{
				lastS = mSystem.currentTimeMillis();
				secondPower--;
			}
		}
		if (!me && GameScr.notPaint)
		{
			return;
		}
        if (sleepEff)
        {
            if (!charEffectTime.isHypnotized)
            {
                charEffectTime.isHypnotized = true;
                if (charEffectTime.timeHypnotized <= 0) charEffectTime.timeHypnotized = CharExtensions.getTimeHypnotize(this);
            }
            if (GameCanvas.gameTick % 10 == 0)
            {
                EffecMn.addEff(new Effect(41, this.cx, this.cy, 3, 1, 1));
            }
        }
        else
        {
            charEffectTime.isHypnotized = false;
			charEffectTime.timeHypnotized = 0;
        }
        if (isMonkey == 1)
        {
            if (!charEffectTime.hasMonkey)
            {
                charEffectTime.hasMonkey = true;
                if (charEffectTime.timeMonkey <= 0) charEffectTime.timeMonkey = CharExtensions.getTimeMonkey(this);
            }
        }
        else
        {
            charEffectTime.hasMonkey = false;
			charEffectTime.timeMonkey = 0;
        }
        if (huytSao)
		{
			huytSao = false;
			if (!charEffectTime.hasHuytSao) charEffectTime.hasHuytSao = true;
            if (charEffectTime.timeHuytSao <= 0) charEffectTime.timeHuytSao = 32;
            EffecMn.addEff(new Effect(39, cx, cy, 3, 3, 1));
		}
        if (blindEff)
        {
            if (!charEffectTime.isTeleported)
            {
                charEffectTime.isTeleported = true;
                if (charEffectTime.timeTeleported <= 0) charEffectTime.timeTeleported = 6;
            }
            if (GameCanvas.gameTick % 5 == 0)
            {
                ServerEffect.addServerEffect(113, this, 1);
            }
        }
        else
        {
            charEffectTime.isTeleported = false;
			charEffectTime.timeTeleported = 0;
        }
        if (protectEff)
        {
            if (!charEffectTime.hasShield)
            {
                charEffectTime.hasShield = true;
                if (charEffectTime.timeShield <= 0) charEffectTime.timeShield = CharExtensions.getTimeShield(this);
            }
            if (GameCanvas.gameTick % 5 == 0)
            {
                eProtect = new Effect(33, cx, cy + 37, 3, 3, 1);
            }
            if (eProtect != null)
            {
                eProtect.update();
                eProtect.x = cx;
                eProtect.y = cy + 37;
            }
        }
        else
        {
            charEffectTime.hasShield = false;
			charEffectTime.timeShield = 0;
        }
        if (charFocus != null && charFocus.cy < 0)
		{
			charFocus = null;
		}
		if (isFusion)
		{
			tFusion++;
		}
		if (isNhapThe)
		{
			int num2 = 0;
			if (GameCanvas.gameTick % 25 == 0)
			{
				num2 = 114;
				ServerEffect.addServerEffect(num2, this, 1);
			}
		}
		if (isSetPos)
		{
			tpos++;
			if (tpos != 1)
			{
				return;
			}
			tpos = 0;
			isSetPos = false;
			cx = xPos;
			cy = yPos;
			cp1 = (cp2 = (cp3 = 0));
			if (typePos == 1)
			{
				if (me)
				{
					cxSend = cx;
					cySend = cy;
				}
				currentMovePoint = null;
				telePortSkill = false;
				ServerEffect.addServerEffect(173, cx, cy, 1);
			}
			else
			{
				ServerEffect.addServerEffect(60, cx, cy, 1);
			}
			if ((TileMap.tileTypeAtPixel(cx, cy) & 2) == 2)
			{
				statusMe = 1;
			}
			else
			{
				statusMe = 4;
			}
			return;
		}
		soundUpdate();
		if (stone)
		{
			if (!charEffectTime.isStone)
			{
				charEffectTime.isStone = true;
				if (charEffectTime.timeStone <= 0) charEffectTime.timeStone = CharExtensions.getTimeStone(this);
			}
			return;
		}
		else
		{
			charEffectTime.isStone = false;
            charEffectTime.timeStone = 0;
		}
		if (isFreez)
		{
			charEffectTime.isTDHS = true;
			if (me && meDead)
			{
				freezSeconds = 0;
				charEffectTime.isTDHS = false;
				charEffectTime.timeTDHS = 0;
			}
			else charEffectTime.timeTDHS = freezSeconds + 1;
			if (GameCanvas.gameTick % 5 == 0)
			{
				ServerEffect.addServerEffect(113, cx, cy, 1);
			}
			cf = 23;
			long num3 = mSystem.currentTimeMillis();
			if (num3 - lastFreez >= 1000)
			{
				freezSeconds--;
				lastFreez = num3;
				if (freezSeconds < 0)
				{
					isFreez = false;
					seconds = 0;
					if (me)
					{
						myCharz().isLockMove = false;
						GameScr.gI().dem = 0;
						GameScr.gI().isFreez = false;
					}
				}
			}
			if (TileMap.tileTypeAt(cx / TileMap.size, cy / TileMap.size) == 0)
			{
				ty++;
				wt++;
				fy += ((!wy) ? 1 : (-1));
				if (wt == 10)
				{
					wt = 0;
					wy = !wy;
				}
			}
			return;
		}
		else
		{
			charEffectTime.isTDHS = false;
			charEffectTime.timeTDHS = 0;
		}
        if (isWaitMonkey)
		{
			isLockMove = true;
			cf = 17;
			if (GameCanvas.gameTick % 5 == 0)
			{
				ServerEffect.addServerEffect(154, cx, cy - 10, 2);
			}
			if (GameCanvas.gameTick % 5 == 0)
			{
				ServerEffect.addServerEffect(1, cx, cy + 10, 1);
			}
			chargeCount++;
			if (chargeCount == 500)
			{
				isWaitMonkey = false;
				isLockMove = false;
			}
			return;
		}
		if (isStandAndCharge)
		{
			chargeCount++;
			bool flag = !TileMap.tileTypeAt(myCharz().cx, myCharz().cy, 2);
			updateEffect();
			updateSkillPaint();
			moveFast = null;
			currentMovePoint = null;
			cf = 17;
			if (flag && cgender != 2)
			{
				cf = 12;
			}
			if (cgender == 2)
			{
				if (GameCanvas.gameTick % 3 == 0)
				{
					ServerEffect.addServerEffect(154, cx, cy - ch / 2 + 10, 1);
				}
				if (GameCanvas.gameTick % 5 == 0)
				{
					ServerEffect.addServerEffect(114, cx + Res.random(-20, 20), cy + Res.random(-20, 20), 1);
				}
			}
			if (cgender == 1)
			{
				if (GameCanvas.gameTick % 4 == 0)
				{
				}
				if (GameCanvas.gameTick % 2 == 0)
				{
					if (cdir == 1)
					{
						ServerEffect.addServerEffect(70, cx - 18, cy - ch / 2 + 8, 1);
						ServerEffect.addServerEffect(70, cx + 23, cy - ch / 2 + 15, 1);
					}
					else
					{
						ServerEffect.addServerEffect(70, cx + 18, cy - ch / 2 + 8, 1);
						ServerEffect.addServerEffect(70, cx - 23, cy - ch / 2 + 15, 1);
					}
				}
			}
			cur = mSystem.currentTimeMillis();
			if (cur - last > seconds || cur - last > 10000)
			{
				stopUseChargeSkill();
				if (me)
				{
					GameScr.gI().auto = 0;
					if (cgender == 2)
					{
						myCharz().setAutoSkillPaint(GameScr.sks[myCharz().myskill.skillId], flag ? 1 : 0);
						Service.gI().skill_not_focus(8);
					}
					if (cgender == 1)
					{
						Res.outz("set skipp paint");
						isCreateDark = true;
						myCharz().setSkillPaint(GameScr.sks[myCharz().myskill.skillId], flag ? 1 : 0);
					}
				}
				else if (cgender == 2)
				{
					setAutoSkillPaint(GameScr.sks[skillTemplateId], flag ? 1 : 0);
				}
				if (cgender == 2 && statusMe != 14 && statusMe != 5)
				{
					GameScr.gI().activeSuperPower(cx, cy);
				}
			}
			chargeCount++;
			if (chargeCount == 500)
			{
				stopUseChargeSkill();
			}
			return;
		}
		if (isFlyAndCharge)
		{
			updateEffect();
			updateSkillPaint();
			moveFast = null;
			currentMovePoint = null;
			posDisY++;
			if (TileMap.tileTypeAt(cx, cy - ch, 8192))
			{
				stopUseChargeSkill();
				return;
			}
			if (posDisY == 20)
			{
				last = mSystem.currentTimeMillis();
			}
			if (posDisY > 20)
			{
				cur = mSystem.currentTimeMillis();
				if (cur - last > seconds || cur - last > 10000)
				{
					isFlyAndCharge = false;
					if (me)
					{
						isCreateDark = true;
						bool flag2 = TileMap.tileTypeAt(myCharz().cx, myCharz().cy, 2);
						isUseSkillAfterCharge = true;
						myCharz().setSkillPaint(GameScr.sks[myCharz().myskill.skillId], (!flag2) ? 1 : 0);
					}
					return;
				}
				cf = 32;
				if (cgender == 0 && GameCanvas.gameTick % 3 == 0)
				{
					ServerEffect.addServerEffect(153, cx, cy - ch, 2);
				}
				chargeCount++;
				if (chargeCount == 500)
				{
					stopUseChargeSkill();
				}
			}
			else
			{
				if (statusMe != 14)
				{
					statusMe = 3;
				}
				cvy = -3;
				cy += cvy;
				cf = 7;
			}
			return;
		}
		if (me && GameCanvas.isTouch)
		{
			if (charFocus != null && charFocus.charID >= 0 && charFocus.cx > 100 && charFocus.cx < TileMap.pxw - 100 && isInEnterOnlinePoint() == null && isInEnterOfflinePoint() == null && !isAttacPlayerStatus() && TileMap.mapID != 51 && TileMap.mapID != 52 && GameCanvas.panel.vPlayerMenu.size() > 0 && GameScr.gI().popUpYesNo == null)
			{
				int num4 = Math.abs(cx - charFocus.cx);
				int num5 = Math.abs(cy - charFocus.cy);
				if (num4 < 60 && num5 < 40)
				{
					if (cmdMenu == null)
					{
						cmdMenu = new Command(mResources.MENU, 11111);
						cmdMenu.isPlaySoundButton = false;
					}
					cmdMenu.x = charFocus.cx - GameScr.cmx;
					cmdMenu.y = charFocus.cy - charFocus.ch - 30 - GameScr.cmy;
				}
				else
				{
					cmdMenu = null;
				}
			}
			else
			{
				cmdMenu = null;
			}
		}
		if (isShadown)
		{
			updateShadown();
		}
		if (isTeleport)
		{
			return;
		}
		if (chatInfo != null)
		{
			chatInfo.update();
		}
		if (shadowLife > 0)
		{
			shadowLife--;
		}
		if (resultTest > 0 && GameCanvas.gameTick % 2 == 0)
		{
			resultTest = (sbyte)(resultTest - 1);
			if (resultTest == 30 || resultTest == 60)
			{
				resultTest = 0;
			}
		}
		updateSkillPaint();
		if (mobMe != null)
		{
			updateMobMe();
			if (mobMe.isDie || mobMe.hp <= 0)
			{
				charEffectTime.hasMobMe = false;
				mobMe = null;
			}
			else if (!charEffectTime.hasMobMe)
			{
				charEffectTime.hasMobMe = true;
				if (charEffectTime.timeMobMe <= 0) charEffectTime.timeMobMe = CharExtensions.getTimeMobMe(this);
			}
		}
		else
		{
			charEffectTime.hasMobMe = false;
        }
        if (arr != null)
		{
			arr.update();
		}
		if (dart != null)
		{
			dart.update();
		}
		updateEffect();
		if (holdEffID != 0)
		{
			if (GameCanvas.gameTick % 5 == 0)
			{
				EffecMn.addEff(new Effect(32, cx, cy + 24, 3, 5, 1));
			}
		}
		else
		{
			if (blindEff || sleepEff)
			{
				return;
			}
			if (holder)
			{
				if (charHold != null && (charHold.statusMe == 14 || charHold.statusMe == 5))
				{
					removeHoleEff();
				}
				if (mobHold != null && mobHold.status == 1)
				{
					removeHoleEff();
				}
				if (me && statusMe == 2 && currentMovePoint != null)
				{
					holder = false;
					charHold = null;
					mobHold = null;
					charEffectTime.isTied = false;
					charEffectTime.isTiedByMe = false;
				}
				if (TileMap.tileTypeAt(cx, cy, 2))
				{
					cf = 16;
				}
				else
				{
					cf = 31;
				}
				return;
			}
			if (cHP > 0)
			{
				for (int i = 0; i < vEff.size(); i++)
				{
					EffectChar effectChar = (EffectChar)vEff.elementAt(i);
					if (effectChar.template.type == 0 || effectChar.template.type == 12)
					{
						if (GameCanvas.isEff1)
						{
							cHP += effectChar.param;
							cMP += effectChar.param;
						}
					}
					else if (effectChar.template.type == 4 || effectChar.template.type == 17)
					{
						if (GameCanvas.isEff1)
						{
							cHP += effectChar.param;
						}
					}
					else if (effectChar.template.type == 13 && GameCanvas.isEff1)
					{
						cHP -= cHPFull * 3 / 100;
						if (cHP < 1)
						{
							cHP = 1;
						}
					}
				}
				if (eff5BuffHp > 0 && GameCanvas.isEff2)
				{
					cHP += eff5BuffHp;
				}
				if (eff5BuffMp > 0 && GameCanvas.isEff2)
				{
					cMP += eff5BuffMp;
				}
				if (cHP > cHPFull)
				{
					cHP = cHPFull;
				}
				if (cMP > cMPFull)
				{
					cMP = cMPFull;
				}
			}
			if (cmtoChar)
			{
				GameScr.cmtoX = cx - GameScr.gW2;
				GameScr.cmtoY = cy - GameScr.gH23;
				if (!GameCanvas.isTouchControl)
				{
					GameScr.cmtoX += GameScr.gW6 * cdir;
				}
			}
			tick = (tick + 1) % 100;
			if (me)
			{
				if (charFocus != null && !GameScr.vCharInMap.contains(charFocus))
				{
					charFocus = null;
				}
				if (cx < 10)
				{
					cvx = 0;
					cx = 10;
				}
				else if (cx > TileMap.pxw - 10)
				{
					cx = TileMap.pxw - 10;
					cvx = 0;
				}
				if (me && !ischangingMap && isInWaypoint())
				{
					Service.gI().charMove();
					if (TileMap.isTrainingMap())
					{
						Service.gI().getMapOffline();
						ischangingMap = true;
					}
					else
					{
						Service.gI().requestChangeMap();
					}
					isLockKey = true;
					ischangingMap = true;
					GameCanvas.clearKeyHold();
					GameCanvas.clearKeyPressed();
					InfoDlg.showWait();
					return;
				}
				if (statusMe != 4 && Res.abs(cx - cxSend) + Res.abs(cy - cySend) >= 70 && cy - cySend <= 0 && me)
				{
					Service.gI().charMove();
				}
				if (isLockMove)
				{
					currentMovePoint = null;
				}
				if (currentMovePoint != null)
				{
					if (abs(cx - currentMovePoint.xEnd) <= 16 && abs(cy - currentMovePoint.yEnd) <= 16)
					{
						cx = (currentMovePoint.xEnd + cx) / 2;
						cy = currentMovePoint.yEnd;
						currentMovePoint = null;
						GameScr.instance.clickMoving = false;
						checkPerformEndMovePointAction();
						cvx = (cvy = 0);
						if ((TileMap.tileTypeAtPixel(cx, cy) & 2) == 2)
						{
							statusMe = 1;
						}
						else
						{
							setCharFallFromJump();
						}
						Service.gI().charMove();
					}
					else
					{
						cdir = ((currentMovePoint.xEnd > cx) ? 1 : (-1));
						if (TileMap.tileTypeAt(cx, cy, 2))
						{
							statusMe = 2;
							if (currentMovePoint != null)
							{
								cvx = cspeed * cdir;
								cvy = 0;
							}
							if (abs(cx - currentMovePoint.xEnd) <= 10)
							{
								if (currentMovePoint.yEnd > cy)
								{
									currentMovePoint = null;
									GameScr.instance.clickMoving = false;
									statusMe = 1;
									cvx = (cvy = 0);
									checkPerformEndMovePointAction();
								}
								else
								{
									SoundMn.gI().charJump();
									cx = currentMovePoint.xEnd;
									statusMe = 10;
									cvy = -5;
									cvx = 0;
								}
							}
							if (cdir == 1)
							{
								if (TileMap.tileTypeAt(cx + chw, cy - chh, 4))
								{
									cvx = cspeed * cdir;
									statusMe = 10;
									cvy = -5;
								}
							}
							else if (TileMap.tileTypeAt(cx - chw - 1, cy - chh, 8))
							{
								cvx = cspeed * cdir;
								statusMe = 10;
								cvy = -5;
							}
						}
						else
						{
							if (currentMovePoint.yEnd < cy + 10)
							{
								statusMe = 10;
								cvy = -5;
								if (abs(cy - currentMovePoint.yEnd) <= 10)
								{
									cy = currentMovePoint.yEnd;
									cvy = 0;
								}
								if (abs(cx - currentMovePoint.xEnd) <= 10)
								{
									cvx = 0;
								}
								else
								{
									cvx = cspeed * cdir;
								}
							}
							else if (TileMap.tileTypeAt(cx, cy, 2))
							{
								currentMovePoint = null;
								GameScr.instance.clickMoving = false;
								statusMe = 1;
								cvx = (cvy = 0);
								checkPerformEndMovePointAction();
							}
							else
							{
								if (statusMe == 10 || statusMe == 2)
								{
									cvy = 0;
								}
								statusMe = 4;
							}
							if (currentMovePoint.yEnd > cy)
							{
								if (cdir == 1)
								{
									if (TileMap.tileTypeAt(cx + chw, cy - chh, 4))
									{
										cvx = (cvy = 0);
										statusMe = 4;
										currentMovePoint = null;
										GameScr.instance.clickMoving = false;
										checkPerformEndMovePointAction();
									}
								}
								else if (TileMap.tileTypeAt(cx - chw - 1, cy - chh, 8))
								{
									cvx = (cvy = 0);
									statusMe = 4;
									currentMovePoint = null;
									GameScr.instance.clickMoving = false;
									checkPerformEndMovePointAction();
								}
							}
						}
					}
				}
				searchFocus();
			}
			else
			{
				checkHideCharName();
				if (statusMe == 1 || statusMe == 6)
				{
					bool flag3 = false;
					if (currentMovePoint != null)
					{
						if (abs(currentMovePoint.xEnd - cx) < 17 && abs(currentMovePoint.yEnd - cy) < 25)
						{
							cx = currentMovePoint.xEnd;
							cy = currentMovePoint.yEnd;
							currentMovePoint = null;
							if ((TileMap.tileTypeAtPixel(cx, cy) & 2) == 2)
							{
								statusMe = 1;
								cp3 = 0;
								GameCanvas.gI().startDust(-1, cx - -8, cy);
								GameCanvas.gI().startDust(1, cx - 8, cy);
							}
							else
							{
								statusMe = 4;
								cvy = 0;
								cp1 = 0;
							}
							flag3 = true;
						}
						else if ((statusBeforeNothing == 10 || cf == 8) && vMovePoints.size() > 0)
						{
							flag3 = true;
						}
						else if (cy == currentMovePoint.yEnd)
						{
							if (cx != currentMovePoint.xEnd)
							{
								cx = (cx + currentMovePoint.xEnd) / 2;
								cf = GameCanvas.gameTick % 5 + 2;
							}
						}
						else if (cy < currentMovePoint.yEnd)
						{
							cf = 12;
							cx = (cx + currentMovePoint.xEnd) / 2;
							if (cvy < 0)
							{
								cvy = 0;
							}
							cy += cvy;
							if ((TileMap.tileTypeAtPixel(cx, cy) & 2) == 2)
							{
								GameCanvas.gI().startDust(-1, cx - -8, cy);
								GameCanvas.gI().startDust(1, cx - 8, cy);
							}
							cvy++;
							if (cvy > 16)
							{
								cy = (cy + currentMovePoint.yEnd) / 2;
							}
						}
						else
						{
							cf = 7;
							cx = (cx + currentMovePoint.xEnd) / 2;
							cy = (cy + currentMovePoint.yEnd) / 2;
						}
					}
					else
					{
						flag3 = true;
					}
					if (flag3 && vMovePoints.size() > 0)
					{
						currentMovePoint = (MovePoint)vMovePoints.firstElement();
						vMovePoints.removeElementAt(0);
						if (currentMovePoint.status == 2)
						{
							if ((TileMap.tileTypeAtPixel(cx, cy + 12) & 2) != 2)
							{
								statusMe = 10;
								cp1 = 0;
								cp2 = 0;
								cvx = -(cx - currentMovePoint.xEnd) / 10;
								cvy = -(cy - currentMovePoint.yEnd) / 10;
								if (cx - currentMovePoint.xEnd > 0)
								{
									cdir = -1;
								}
								else if (cx - currentMovePoint.xEnd < 0)
								{
									cdir = 1;
								}
							}
							else
							{
								statusMe = 2;
								if (cx - currentMovePoint.xEnd > 0)
								{
									cdir = -1;
								}
								else if (cx - currentMovePoint.xEnd < 0)
								{
									cdir = 1;
								}
								cvx = cspeed * cdir;
								cvy = 0;
							}
						}
						else if (currentMovePoint.status == 3)
						{
							if ((TileMap.tileTypeAtPixel(cx, cy + 23) & 2) != 2)
							{
								statusMe = 10;
								cp1 = 0;
								cp2 = 0;
								cvx = -(cx - currentMovePoint.xEnd) / 10;
								cvy = -(cy - currentMovePoint.yEnd) / 10;
								if (cx - currentMovePoint.xEnd > 0)
								{
									cdir = -1;
								}
								else if (cx - currentMovePoint.xEnd < 0)
								{
									cdir = 1;
								}
							}
							else
							{
								statusMe = 3;
								GameCanvas.gI().startDust(-1, cx - -8, cy);
								GameCanvas.gI().startDust(1, cx - 8, cy);
								if (cx - currentMovePoint.xEnd > 0)
								{
									cdir = -1;
								}
								else if (cx - currentMovePoint.xEnd < 0)
								{
									cdir = 1;
								}
								cvx = abs(cx - currentMovePoint.xEnd) / 10 * cdir;
								cvy = -10;
							}
						}
						else if (currentMovePoint.status == 4)
						{
							statusMe = 4;
							if (cx - currentMovePoint.xEnd > 0)
							{
								cdir = -1;
							}
							else if (cx - currentMovePoint.xEnd < 0)
							{
								cdir = 1;
							}
							cvx = abs(cx - currentMovePoint.xEnd) / 9 * cdir;
							cvy = 0;
						}
						else
						{
							cx = currentMovePoint.xEnd;
							cy = currentMovePoint.yEnd;
							currentMovePoint = null;
						}
					}
				}
			}
			switch (statusMe)
			{
			case 1:
				updateCharStand();
				break;
			case 2:
				updateCharRun();
				break;
			case 3:
				updateCharJump();
				break;
			case 4:
				updateCharFall();
				break;
			case 5:
				updateCharDeadFly();
				break;
			case 16:
				updateResetPoint();
				break;
			case 9:
				updateCharAutoJump();
				break;
			case 10:
				updateCharFly();
				break;
			case 12:
				updateSkillStand();
				break;
			case 13:
				updateSkillFall();
				break;
			case 14:
				cp1++;
				if (cp1 > 30)
				{
					cp1 = 0;
				}
				if (cp1 % 15 < 5)
				{
					cf = 0;
				}
				else
				{
					cf = 1;
				}
				break;
			case 6:
				if (isInjure <= 0)
				{
					cf = 0;
				}
				else if (statusBeforeNothing == 10)
				{
					cx += cvx;
				}
				else if (cf <= 1)
				{
					cp1++;
					if (cp1 > 6)
					{
						cf = 0;
					}
					else
					{
						cf = 1;
					}
					if (cp1 > 10)
					{
						cp1 = 0;
					}
				}
				if (cf != 7 && cf != 12 && (TileMap.tileTypeAtPixel(cx, cy + 1) & 2) != 2)
				{
					cvx = 0;
					cvy = 0;
					statusMe = 4;
					cf = 7;
				}
				if (me)
				{
					break;
				}
				cp3++;
				if (cp3 > 10)
				{
					if ((TileMap.tileTypeAtPixel(cx, cy + 1) & 2) != 2)
					{
						cy += 5;
					}
					else
					{
						cf = 0;
					}
				}
				if (cp3 > 50)
				{
					cp3 = 0;
					currentMovePoint = null;
				}
				break;
			}
			if (isInjure > 0)
			{
				cf = 23;
				isInjure = (sbyte)(isInjure - 1);
			}
			if (wdx != 0 || wdy != 0)
			{
				startDie(wdx, wdy);
				wdx = 0;
				wdy = 0;
			}
			if (moveFast != null)
			{
				if (moveFast[0] == 0)
				{
					moveFast[0]++;
					ServerEffect.addServerEffect(60, this, 1);
				}
				else if (moveFast[0] < 10)
				{
					moveFast[0]++;
				}
				else
				{
					cx = moveFast[1];
					cy = moveFast[2];
					moveFast = null;
					ServerEffect.addServerEffect(60, this, 1);
					if (me)
					{
						if ((TileMap.tileTypeAtPixel(cx, cy) & 2) != 2)
						{
							statusMe = 4;
							myCharz().setAutoSkillPaint(GameScr.sks[38], 1);
						}
						else
						{
							Service.gI().charMove();
							myCharz().setAutoSkillPaint(GameScr.sks[38], 0);
						}
					}
				}
			}
			if (statusMe != 10)
			{
				fy = 0;
			}
			if (isCharge)
			{
				cf = 17;
				if (GameCanvas.gameTick % 4 == 0)
				{
					ServerEffect.addServerEffect(1, cx, cy + GameCanvas.transY, 1);
				}
				if (me)
				{
					long num6 = mSystem.currentTimeMillis();
					if (num6 - last >= 1000)
					{
						Res.outz("%= " + myskill.damage);
						last = num6;
						cHP += cHPFull * myskill.damage / 100;
						cMP += cMPFull * myskill.damage / 100;
						if (cHP < cHPFull)
						{
							GameScr.startFlyText("+" + cHPFull * myskill.damage / 100 + " " + mResources.HP, cx, cy - ch - 20, 0, -1, mFont.HP);
						}
						if (cMP < cMPFull)
						{
							GameScr.startFlyText("+" + cMPFull * myskill.damage / 100 + " " + mResources.KI, cx, cy - ch - 20, 0, -2, mFont.MP);
						}
						Service.gI().skill_not_focus(2);
					}
				}
			}
			if (isFlyUp)
			{
				if (me)
				{
					isLockKey = true;
					statusMe = 3;
					cvy = -8;
					if (cy <= TileMap.pxh - 240)
					{
						isFlyUp = false;
						isLockKey = false;
						statusMe = 4;
					}
				}
				else
				{
					statusMe = 3;
					cvy = -8;
					if (cy <= TileMap.pxh - 240)
					{
						cvy = 0;
						isFlyUp = false;
						cvy = 0;
						statusMe = 1;
					}
				}
			}
			updateMount();
			updEffChar();
			updateEye();
			updateFHead();
		}
	}

	private void updateEffect()
	{
		if (effPaints != null)
		{
			for (int i = 0; i < effPaints.Length; i++)
			{
				if (effPaints[i] == null)
				{
					continue;
				}
				if (effPaints[i].eMob != null)
				{
					if (!effPaints[i].isFly)
					{
						effPaints[i].eMob.setInjure();
						effPaints[i].eMob.injureBy = this;
						if (me)
						{
							effPaints[i].eMob.hpInjure = myCharz().cDamFull / 2 - myCharz().cDamFull * NinjaUtil.randomNumber(11) / 100;
						}
						int num = effPaints[i].eMob.h >> 1;
						if (effPaints[i].eMob.isBigBoss())
						{
							num = effPaints[i].eMob.getY() + 20;
						}
						GameScr.startSplash(effPaints[i].eMob.x, effPaints[i].eMob.y - num, cdir);
						effPaints[i].isFly = true;
					}
				}
				else if (effPaints[i].eChar != null && !effPaints[i].isFly)
				{
					if (effPaints[i].eChar.charID >= 0)
					{
						effPaints[i].eChar.doInjure();
					}
					GameScr.startSplash(effPaints[i].eChar.cx, effPaints[i].eChar.cy - (effPaints[i].eChar.ch >> 1), cdir);
					effPaints[i].isFly = true;
				}
				effPaints[i].index++;
				if (effPaints[i].index >= effPaints[i].effCharPaint.arrEfInfo.Length)
				{
					effPaints[i] = null;
				}
			}
		}
		if (indexEff >= 0 && eff != null && GameCanvas.gameTick % 2 == 0)
		{
			indexEff++;
			if (indexEff >= eff.arrEfInfo.Length)
			{
				indexEff = -1;
				eff = null;
			}
		}
		if (indexEffTask >= 0 && effTask != null && GameCanvas.gameTick % 2 == 0)
		{
			indexEffTask++;
			if (indexEffTask >= effTask.arrEfInfo.Length)
			{
				indexEffTask = -1;
				effTask = null;
			}
		}
	}

	private void checkPerformEndMovePointAction()
	{
		if (endMovePointCommand != null)
		{
			Command command = endMovePointCommand;
			endMovePointCommand = null;
			command.performAction();
		}
	}

	private void checkHideCharName()
	{
		if (GameCanvas.gameTick % 20 != 0 || charID < 0)
		{
			return;
		}
		paintName = true;
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = null;
			try
			{
				@char = (Char)GameScr.vCharInMap.elementAt(i);
			}
			catch (Exception)
			{
			}
			if (@char != null && !@char.Equals(this) && ((@char.cy == cy && Res.abs(@char.cx - cx) < 35) || (cy - @char.cy < 32 && cy - @char.cy > 0 && Res.abs(@char.cx - cx) < 24)))
			{
				paintName = false;
			}
		}
		for (int j = 0; j < GameScr.vNpc.size(); j++)
		{
			Npc npc = null;
			try
			{
				npc = (Npc)GameScr.vNpc.elementAt(j);
			}
			catch (Exception)
			{
			}
			if (npc != null && npc.cy == cy && Res.abs(npc.cx - cx) < 24)
			{
				paintName = false;
			}
		}
	}

	private void updateMobMe()
	{
		if (tMobMeBorn != 0)
		{
			tMobMeBorn--;
        }
		if (tMobMeBorn == 0)
		{
			mobMe.xFirst = ((cdir != 1) ? (cx + 30) : (cx - 30));
			mobMe.yFirst = cy - 60;
			int num = mobMe.xFirst - mobMe.x;
			int num2 = mobMe.yFirst - mobMe.y;
			mobMe.x += num / 4;
			mobMe.y += num2 / 4;
			mobMe.dir = cdir;
		}
	}

	private void updateSkillPaint()
	{
		if (statusMe == 14 || statusMe == 5)
		{
			return;
		}
		if (skillPaint != null && ((charFocus != null && isMeCanAttackOtherPlayer(charFocus) && charFocus.statusMe == 14) || (mobFocus != null && mobFocus.status == 0)))
		{
			if (!me)
			{
				if ((TileMap.tileTypeAtPixel(cx, cy) & 2) == 2)
				{
					statusMe = 1;
				}
				else
				{
					statusMe = 6;
				}
				cp3 = 0;
			}
			indexSkill = 0;
			skillPaint = null;
			skillPaintRandomPaint = null;
			eff0 = (eff1 = (eff2 = null));
			i0 = (i1 = (i2 = 0));
			mobFocus = null;
			charFocus = null;
			effPaints = null;
			currentMovePoint = null;
			arr = null;
			if ((TileMap.tileTypeAtPixel(cx, cy) & 2) != 2)
			{
				delayFall = 5;
			}
		}
		if (skillPaint != null && arr == null && skillInfoPaint() != null && indexSkill >= skillInfoPaint().Length)
		{
			if (!me)
			{
				if ((TileMap.tileTypeAtPixel(cx, cy) & 2) == 2)
				{
					statusMe = 1;
				}
				else
				{
					statusMe = 6;
				}
				cp3 = 0;
			}
			indexSkill = 0;
			Res.outz("remove 2");
			skillPaint = null;
			skillPaintRandomPaint = null;
			eff0 = (eff1 = (eff2 = null));
			i0 = (i1 = (i2 = 0));
			arr = null;
			if ((TileMap.tileTypeAtPixel(cx, cy) & 2) != 2)
			{
				delayFall = 5;
			}
		}
		SkillInfoPaint[] array = skillInfoPaint();
		if (array == null || indexSkill < 0 || indexSkill > array.Length - 1)
		{
			return;
		}
		if (array[indexSkill].effS0Id != 0)
		{
			eff0 = GameScr.efs[array[indexSkill].effS0Id - 1];
			i0 = (dx0 = (dy0 = 0));
		}
		if (array[indexSkill].effS1Id != 0)
		{
			eff1 = GameScr.efs[array[indexSkill].effS1Id - 1];
			i1 = (dx1 = (dy1 = 0));
		}
		if (array[indexSkill].effS2Id != 0)
		{
			eff2 = GameScr.efs[array[indexSkill].effS2Id - 1];
			i2 = (dx2 = (dy2 = 0));
		}
		SkillInfoPaint[] array2 = array;
		int num = indexSkill;
		if (array2 != null && array2[num] != null && num >= 0 && num <= array2.Length - 1 && array2[num].arrowId != 0)
		{
			int arrowId = array2[num].arrowId;
			if (arrowId >= 100)
			{
				object obj;
				if (mobFocus == null)
				{
					IMapObject mapObject = charFocus;
					obj = mapObject;
				}
				else
				{
					obj = mobFocus;
				}
				IMapObject mapObject2 = (IMapObject)obj;
				if (mapObject2 != null)
				{
					int num2 = 0;
					int num3 = Res.abs(mapObject2.getX() - cx);
					int num4 = Res.abs(mapObject2.getY() - cy);
					if (num3 > 4 * num4)
					{
						num2 = 0;
					}
					else
					{
						num2 = ((mapObject2.getY() >= cy) ? 3 : (-3));
						if (mapObject2 is BigBoss)
						{
							BigBoss bigBoss = (BigBoss)mapObject2;
							if (bigBoss.haftBody)
							{
								num2 = -20;
							}
						}
					}
					dart = new PlayerDart(this, arrowId - 100, skillPaintRandomPaint, cx + (array2[num].adx - 10) * cdir, cy + array2[num].ady + num2);
					if (myskill != null)
					{
						if (myskill.template.id == 1)
						{
							SoundMn.gI().traidatKame();
						}
						else if (myskill.template.id == 3)
						{
							SoundMn.gI().namekKame();
						}
						else if (myskill.template.id == 5)
						{
							SoundMn.gI().xaydaKame();
						}
						else if (myskill.template.id == 11)
						{
							SoundMn.gI().nameLazer();
						}
					}
				}
				else if (isFlyAndCharge || isUseSkillAfterCharge)
				{
					stopUseChargeSkill();
				}
			}
			else
			{
				Res.outz("g");
				arr = new Arrow(this, GameScr.arrs[arrowId - 1]);
				arr.life = 10;
				arr.ax = cx + array2[num].adx;
				arr.ay = cy + array2[num].ady;
			}
		}
		if ((mobFocus != null || (!me && charFocus != null) || (me && charFocus != null && (isMeCanAttackOtherPlayer(charFocus) || isSelectingSkillBuffToPlayer()) && arr == null && dart == null)) && indexSkill == array.Length - 1)
		{
			setAttack();
			if (me && myskill.template.isAttackSkill())
			{
				saveLoadPreviousSkill();
			}
		}
		if (me)
		{
			return;
		}
		IMapObject mapObject3 = null;
		if (mobFocus != null)
		{
			mapObject3 = mobFocus;
		}
		else if (charFocus != null)
		{
			mapObject3 = charFocus;
		}
		if (mapObject3 == null)
		{
			return;
		}
		if (Res.abs(mapObject3.getX() - cx) < 10)
		{
			if (mapObject3.getX() > cx)
			{
				cx -= 10;
			}
			else
			{
				cx += 10;
			}
		}
		if (mapObject3.getX() > cx)
		{
			cdir = 1;
		}
		else
		{
			cdir = -1;
		}
	}

	public void saveLoadPreviousSkill()
	{
	}

	public void setResetPoint(int x, int y)
	{
		InfoDlg.hide();
		currentMovePoint = null;
		int num = cx - x;
		if (cy - y == 0)
		{
			cx = x;
			ischangingMap = false;
			isLockKey = false;
			return;
		}
		statusMe = 16;
		cp2 = x;
		cp3 = y;
		cp1 = 0;
		myCharz().cxSend = x;
		myCharz().cySend = y;
	}

	private void updateCharDeadFly()
	{
		isFreez = false;
		if (isCharge)
		{
			isCharge = false;
			SoundMn.gI().taitaoPause();
			Service.gI().skill_not_focus(3);
		}
		cp1++;
		cx += (cp2 - cx) / 4;
		if (cp1 > 7)
		{
			cy += (cp3 - cy) / 4;
		}
		else
		{
			cy += cp1 - 10;
		}
		if (Res.abs(cp2 - cx) < 4 && Res.abs(cp3 - cy) < 10)
		{
			cx = cp2;
			cy = cp3;
			statusMe = 14;
			if (me)
			{
				GameScr.gI().resetButton();
				Service.gI().charMove();
			}
		}
		cf = 23;
	}

	private void updateResetPoint()
	{
		InfoDlg.hide();
		GameCanvas.clearAllPointerEvent();
		currentMovePoint = null;
		cp1++;
		cx += (cp2 - cx) / 4;
		if (cp1 > 7)
		{
			cy += (cp3 - cy) / 4;
		}
		else
		{
			cy += cp1 - 10;
		}
		if (Res.abs(cp2 - cx) < 4 && Res.abs(cp3 - cy) < 10)
		{
			cx = cp2;
			cy = cp3;
			statusMe = 1;
			cp3 = 0;
			ischangingMap = false;
			Service.gI().charMove();
		}
		cf = 23;
	}

	public void updateSkillFall()
	{
	}

	public void updateSkillStand()
	{
		ty = 0;
		cp1++;
		if (cdir == 1)
		{
			if ((TileMap.tileTypeAtPixel(cx + chw, cy - chh) & 4) == 4)
			{
				cvx = 0;
			}
		}
		else if ((TileMap.tileTypeAtPixel(cx - chw, cy - chh) & 8) == 8)
		{
			cvx = 0;
		}
		if (cy > ch && TileMap.tileTypeAt(cx, cy - ch + 24, 8192))
		{
			if (!TileMap.tileTypeAt(cx, cy, 2))
			{
				statusMe = 4;
				cp1 = 0;
				cp2 = 0;
				cvy = 1;
			}
			else
			{
				cy = TileMap.tileYofPixel(cy);
			}
		}
		cx += cvx;
		cy += cvy;
		if (cy < 0)
		{
			cy = (cvy = 0);
		}
		if (cvy == 0)
		{
			if ((TileMap.tileTypeAtPixel(cx, cy) & 2) != 2)
			{
				statusMe = 4;
				cvx = (cspeed >> 1) * cdir;
				cp1 = (cp2 = 0);
			}
		}
		else if (cvy < 0)
		{
			cvy++;
			if (cvy == 0)
			{
				cvy = 1;
			}
		}
		else
		{
			if (cvy < 20 && cp1 % 5 == 0)
			{
				cvy++;
			}
			if (cvy > 3)
			{
				cvy = 3;
			}
			if ((TileMap.tileTypeAtPixel(cx, cy + 3) & 2) == 2 && cy <= TileMap.tileXofPixel(cy + 3))
			{
				cvx = (cvy = 0);
				cy = TileMap.tileXofPixel(cy + 3);
			}
		}
		if (cvx > 0)
		{
			cvx--;
		}
		else if (cvx < 0)
		{
			cvx++;
		}
	}

	public void updateCharAutoJump()
	{
		isFreez = false;
		if (isCharge)
		{
			isCharge = false;
			SoundMn.gI().taitaoPause();
			Service.gI().skill_not_focus(3);
		}
		cx += cvx * cdir;
		cy += cvyJump;
		cvyJump++;
		if (cp1 == 0)
		{
			cf = 7;
		}
		else
		{
			cf = 23;
		}
		if (cvyJump == -3)
		{
			cf = 8;
		}
		else if (cvyJump == -2)
		{
			cf = 9;
		}
		else if (cvyJump == -1)
		{
			cf = 10;
		}
		else if (cvyJump == 0)
		{
			cf = 11;
		}
		if (cvyJump == 0)
		{
			statusMe = 6;
			cp3 = 0;
			((MovePoint)vMovePoints.firstElement()).status = 4;
			isJump = true;
			cp1 = 0;
			cvy = 1;
		}
	}

	public int getVx(int size, int dx, int dy)
	{
		if (dy > 0 && !TileMap.tileTypeAt(cx, cy, 2))
		{
			if (dx - dy <= 10)
			{
				return 5;
			}
			if (dx - dy <= 30)
			{
				return 6;
			}
			if (dx - dy <= 50)
			{
				return 7;
			}
			if (dx - dy <= 70)
			{
				return 8;
			}
		}
		if (dx <= 30)
		{
			return 4;
		}
		if (dx <= 160)
		{
			return 5;
		}
		if (dx <= 270)
		{
			return 6;
		}
		if (dx <= 320)
		{
			return 7;
		}
		return 8;
	}

	public void hide()
	{
		isHide = true;
		EffecMn.addEff(new Effect(107, cx, cy + 25, 3, 15, 1));
	}

	public void show()
	{
		isHide = false;
		EffecMn.addEff(new Effect(107, cx, cy + 25, 3, 10, 1));
	}

	public int getVy(int size, int dx, int dy)
	{
		if (dy <= 10)
		{
			return 5;
		}
		if (dy <= 20)
		{
			return 6;
		}
		if (dy <= 30)
		{
			return 7;
		}
		if (dy <= 40)
		{
			return 8;
		}
		if (dy <= 50)
		{
			return 9;
		}
		return 10;
	}

	public int returnAct(int xFirst, int yFirst, int xEnd, int yEnd)
	{
		int num = xEnd - xFirst;
		int num2 = yEnd - yFirst;
		if (num == 0 && num2 == 0)
		{
			return 1;
		}
		if (num2 == 0 && yFirst % 24 == 0 && TileMap.tileTypeAt(xFirst, yFirst, 2))
		{
			return 2;
		}
		if (num2 > 0 && (yFirst % 24 != 0 || !TileMap.tileTypeAt(xFirst, yFirst, 2)))
		{
			return 4;
		}
		cvy = -10;
		cp1 = 0;
		cdir = ((num > 0) ? 1 : (-1));
		if (num <= 5)
		{
			cvx = 0;
		}
		else if (num <= 10)
		{
			cvx = 3;
		}
		else
		{
			cvx = 5;
		}
		return 9;
	}

	public void setAutoJump()
	{
		int num = ((MovePoint)vMovePoints.firstElement()).xEnd - cx;
		cvyJump = -10;
		cp1 = 0;
		cdir = ((num > 0) ? 1 : (-1));
		if (num <= 6)
		{
			cvx = 0;
		}
		else if (num <= 20)
		{
			cvx = 3;
		}
		else
		{
			cvx = 5;
		}
	}

	public void updateCharStand()
	{
		isSoundJump = false;
		isAttack = false;
		isAttFly = false;
		cvx = 0;
		cvy = 0;
		cp1++;
		if (cp1 > 30)
		{
			cp1 = 0;
		}
		if (cp1 % 15 < 5)
		{
			cf = 0;
		}
		else
		{
			cf = 1;
		}
		updateCharInBridge();
		if (!me)
		{
			cp3++;
			if (cp3 > 50)
			{
				cp3 = 0;
				currentMovePoint = null;
			}
		}
		updateSuperEff();
		if (!me || GameScr.vCharInMap.size() == 0 || TileMap.mapID != 50)
		{
			return;
		}
		Char @char = (Char)GameScr.vCharInMap.elementAt(0);
		if (!@char.changePos)
		{
			if (@char.statusMe != 2)
			{
				@char.moveTo(cx - 45, cy, 0);
			}
			@char.lastUpdateTime = mSystem.currentTimeMillis();
			if (Res.abs(cx - 45 - @char.cx) <= 10)
			{
				@char.changePos = true;
			}
		}
		else
		{
			if (@char.statusMe != 2)
			{
				@char.moveTo(cx + 45, cy, 0);
			}
			@char.lastUpdateTime = mSystem.currentTimeMillis();
			if (Res.abs(cx + 45 - @char.cx) <= 10)
			{
				@char.changePos = false;
			}
		}
		if (GameCanvas.gameTick % 100 == 0)
		{
			@char.addInfo("Cắc cùm cum");
		}
	}

	public void updateSuperEff()
	{
        if (ModMenu.getStatusInt("levelreducegraphics") > 1) return;
        if (GameCanvas.panel.isShow || isCopy || isFusion || isSetPos || isPet || isMiniPet || isMonkey == 1)
		{
			return;
		}
		if (me)
		{
			if (isPaintAura && idAuraEff > -1)
			{
				return;
			}
		}
		else if (idAuraEff > -1)
		{
			return;
		}
		ty++;
		if (clevel >= 14)
		{
			return;
		}
		if (clevel >= 9 && !GameCanvas.lowGraphic && (ty == 40 || ty == 50))
		{
			GameCanvas.gI().startDust(-1, cx - -8, cy);
			GameCanvas.gI().startDust(1, cx - 8, cy);
			addDustEff(1);
		}
		if (ty <= 50 || clevel < 9)
		{
			return;
		}
		int num = 0;
		if (cgender == 0)
		{
			if (GameCanvas.gameTick % 25 == 0)
			{
				num = 114;
				ServerEffect.addServerEffect(num, this, 1);
			}
			if (clevel >= 13 && GameCanvas.gameTick % 4 == 0)
			{
				num = 132;
				ServerEffect.addServerEffect(num, this, 1);
			}
		}
		if (cgender == 1)
		{
			if (GameCanvas.gameTick % 4 == 0)
			{
				num = 132;
				ServerEffect.addServerEffect(num, this, 1);
			}
			if (clevel >= 13 && GameCanvas.gameTick % 7 == 0)
			{
				num = 131;
				ServerEffect.addServerEffect(num, this, 1);
			}
		}
		if (cgender == 2)
		{
			if (GameCanvas.gameTick % 7 == 0)
			{
				num = 131;
				ServerEffect.addServerEffect(num, this, 1);
			}
			if (clevel >= 13 && GameCanvas.gameTick % 25 == 0)
			{
				num = 114;
				ServerEffect.addServerEffect(num, this, 1);
			}
		}
	}

	public float getSoundVolumn()
	{
		if (me)
		{
			return 0.1f;
		}
		int num = Res.abs(myChar.cx - cx);
		if (num >= 0 && num <= 50)
		{
			return 0.1f;
		}
		return 0.05f;
	}

	public void updateCharRun()
	{
		int num = ((isMonkey != 1 || me) ? 1 : 2);
		if (cx >= GameScr.cmx && cx <= GameScr.cmx + GameCanvas.w)
		{
			if (isMonkey == 0)
			{
				SoundMn.gI().charRun(getSoundVolumn());
			}
			else
			{
				SoundMn.gI().monkeyRun(getSoundVolumn());
			}
		}
		ty = 0;
		isFreez = false;
		if (isCharge)
		{
			isCharge = false;
			SoundMn.gI().taitaoPause();
			Service.gI().skill_not_focus(3);
		}
		int num2 = 0;
		if (!me && currentMovePoint != null)
		{
			num2 = abs(cx - currentMovePoint.xEnd);
		}
		cp1++;
		if (cp1 >= 10)
		{
			cp1 = 0;
			cBonusSpeed = 0;
		}
		cf = (cp1 >> 1) + 2;
		if ((TileMap.tileTypeAtPixel(cx, cy - 1) & 0x40) == 64)
		{
			cx += cvx * num >> 1;
		}
		else
		{
			cx += cvx * num;
		}
		if (cdir == 1)
		{
			if (TileMap.tileTypeAt(cx + chw, cy - chh, 4))
			{
				if (me)
				{
					cvx = 0;
					cx = TileMap.tileXofPixel(cx + chw) - chw;
				}
				else
				{
					stop();
				}
			}
		}
		else if (TileMap.tileTypeAt(cx - chw - 1, cy - chh, 8))
		{
			if (me)
			{
				cvx = 0;
				cx = TileMap.tileXofPixel(cx - chw - 1) + TileMap.size + chw;
			}
			else
			{
				stop();
			}
		}
		if (me)
		{
			if (cvx > 0)
			{
				cvx--;
			}
			else if (cvx < 0)
			{
				cvx++;
			}
			else
			{
				if (cx - cxSend != 0 && me)
				{
					Service.gI().charMove();
				}
				statusMe = 1;
				cBonusSpeed = 0;
			}
		}
		if ((TileMap.tileTypeAtPixel(cx, cy) & 2) != 2)
		{
			if (me)
			{
				if (cx - cxSend != 0 || cy - cySend != 0)
				{
					Service.gI().charMove();
				}
				cf = 7;
				statusMe = 4;
				delayFall = 0;
				cvx = 3 * cdir;
				cp2 = 0;
			}
			else
			{
				stop();
			}
		}
		if (!me && currentMovePoint != null)
		{
			int num3 = abs(cx - currentMovePoint.xEnd);
			if (num3 > num2)
			{
				stop();
			}
		}
		GameCanvas.gI().startDust(cdir, cx - (cdir << 3), cy);
		updateCharInBridge();
		addDustEff(2);
	}

	private void stop()
	{
		statusMe = 6;
		cp3 = 0;
		cvx = 0;
		cvy = 0;
		cp1 = (cp2 = 0);
	}

	public static int abs(int i)
	{
		return (i <= 0) ? (-i) : i;
	}

	public void updateCharJump()
	{
		setMountIsStart();
		ty = 0;
		isFreez = false;
		if (isCharge)
		{
			isCharge = false;
			SoundMn.gI().taitaoPause();
			Service.gI().skill_not_focus(3);
		}
		addDustEff(3);
		cx += cvx;
		cy += cvy;
		if (cy < 0)
		{
			cy = 0;
			cvy = -1;
		}
		cvy++;
		if (cvy > 0)
		{
			cvy = 0;
		}
		if (!me && currentMovePoint != null)
		{
			int num = currentMovePoint.xEnd - cx;
			if (num > 0)
			{
				if (cvx > num)
				{
					cvx = num;
				}
				if (cvx < 0)
				{
					cvx = num;
				}
			}
			else if (num < 0)
			{
				if (cvx < num)
				{
					cvx = num;
				}
				if (cvx > 0)
				{
					cvx = num;
				}
			}
			else
			{
				cvx = num;
			}
		}
		if (cdir == 1)
		{
			if ((TileMap.tileTypeAtPixel(cx + chw, cy - 1) & 4) == 4 && cx <= TileMap.tileXofPixel(cx + chw) + 12)
			{
				cx = TileMap.tileXofPixel(cx + chw) - chw;
				cvx = 0;
			}
		}
		else if ((TileMap.tileTypeAtPixel(cx - chw, cy - 1) & 8) == 8 && cx >= TileMap.tileXofPixel(cx - chw) + 12)
		{
			cx = TileMap.tileXofPixel(cx + 24 - chw) + chw;
			cvx = 0;
		}
		if (cvy == 0)
		{
			if (!isAttFly)
			{
				if (me)
				{
					setCharFallFromJump();
				}
				else
				{
					stop();
				}
			}
			else
			{
				setCharFallFromJump();
			}
		}
		if (me && !ischangingMap && isInWaypoint())
		{
			Service.gI().charMove();
			if (TileMap.isTrainingMap())
			{
				ischangingMap = true;
				Service.gI().getMapOffline();
			}
			else
			{
				Service.gI().requestChangeMap();
			}
			isLockKey = true;
			ischangingMap = true;
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
			InfoDlg.showWait();
			return;
		}
		if (statusMe != 16 && (TileMap.tileTypeAt(cx, cy - ch + 24, 8192) || cy < 0))
		{
			statusMe = 4;
			cp1 = 0;
			cp2 = 0;
			cvy = 1;
			delayFall = 0;
			if (cy < 0)
			{
				cy = 0;
			}
			cy = TileMap.tileYofPixel(cy + 25);
			GameCanvas.clearKeyHold();
		}
		if (cp3 < 0)
		{
			cp3++;
		}
		cf = 7;
		if (!me && currentMovePoint != null && cy < currentMovePoint.yEnd)
		{
			stop();
		}
	}

	public bool checkInRangeJump(int x1, int xw1, int xmob, int y1, int yh1, int ymob)
	{
		if (xmob > xw1 || xmob < x1 || ymob > y1 || ymob < yh1)
		{
			return false;
		}
		return true;
	}

	public void setCharFallFromJump()
	{
		cyStartFall = cy;
		cp1 = 0;
		cp2 = 0;
		statusMe = 10;
		cvx = cdir << 2;
		cvy = 0;
		cy = TileMap.tileYofPixel(cy) + 12;
		if (me && (cx - cxSend != 0 || cy - cySend != 0) && (Res.abs(myCharz().cx - myCharz().cxSend) > 96 || Res.abs(myCharz().cy - myCharz().cySend) > 24))
		{
			Service.gI().charMove();
		}
	}

	public void updateCharFall()
	{
		if (holder)
		{
			return;
		}
		ty = 0;
		if (cy + 4 >= TileMap.pxh)
		{
			statusMe = 1;
			if (me)
			{
				SoundMn.gI().charFall();
			}
			cvx = (cvy = 0);
			cp3 = 0;
			return;
		}
		if (cy % 24 == 0 && (TileMap.tileTypeAtPixel(cx, cy) & 2) == 2)
		{
			delayFall = 0;
			if (me)
			{
				if (cy - cySend > 0)
				{
					Service.gI().charMove();
				}
				else if (cx - cxSend != 0 || cy - cySend < 0)
				{
					Service.gI().charMove();
				}
				cvx = (cvy = 0);
				cp1 = (cp2 = 0);
				statusMe = 1;
				cp3 = 0;
				return;
			}
			stop();
			cf = 0;
			GameCanvas.gI().startDust(-1, cx - -8, cy);
			GameCanvas.gI().startDust(1, cx - 8, cy);
			addDustEff(1);
		}
		if (delayFall > 0)
		{
			delayFall--;
			if (delayFall % 10 > 5)
			{
				cy++;
			}
			else
			{
				cy--;
			}
			return;
		}
		if (cvy < -4)
		{
			cf = 7;
		}
		else
		{
			cf = 12;
		}
		cx += cvx;
		if (!me && currentMovePoint != null)
		{
			int num = currentMovePoint.xEnd - cx;
			if (num > 0)
			{
				if (cvx > num)
				{
					cvx = num;
				}
				if (cvx < 0)
				{
					cvx = num;
				}
			}
			else if (num < 0)
			{
				if (cvx < num)
				{
					cvx = num;
				}
				if (cvx > 0)
				{
					cvx = num;
				}
			}
			else
			{
				cvx = num;
			}
		}
		cvy++;
		if (cvy > 8)
		{
			cvy = 8;
		}
		if (skillPaintRandomPaint == null)
		{
			cy += cvy;
		}
		if (cdir == 1)
		{
			if ((TileMap.tileTypeAtPixel(cx + chw, cy - 1) & 4) == 4 && cx <= TileMap.tileXofPixel(cx + chw) + 12)
			{
				cx = TileMap.tileXofPixel(cx + chw) - chw;
				cvx = 0;
			}
		}
		else if ((TileMap.tileTypeAtPixel(cx - chw, cy - 1) & 8) == 8 && cx >= TileMap.tileXofPixel(cx - chw) + 12)
		{
			cx = TileMap.tileXofPixel(cx + 24 - chw) + chw;
			cvx = 0;
		}
		if (cvy > 3 && (cyStartFall == 0 || cyStartFall <= TileMap.tileYofPixel(cy + 3)) && (TileMap.tileTypeAtPixel(cx, cy + 3) & 2) == 2)
		{
			if (me)
			{
				cyStartFall = 0;
				cvx = (cvy = 0);
				cp1 = (cp2 = 0);
				cy = TileMap.tileXofPixel(cy + 3);
				statusMe = 1;
				if (me)
				{
					SoundMn.gI().charFall();
				}
				cp3 = 0;
				GameCanvas.gI().startDust(-1, cx - -8, cy);
				GameCanvas.gI().startDust(1, cx - 8, cy);
				addDustEff(1);
				if (cy - cySend > 0)
				{
					if (me)
					{
						Service.gI().charMove();
					}
				}
				else if ((cx - cxSend != 0 || cy - cySend < 0) && me)
				{
					Service.gI().charMove();
				}
			}
			else
			{
				stop();
				cy = TileMap.tileXofPixel(cy + 3);
				cf = 0;
				GameCanvas.gI().startDust(-1, cx - -8, cy);
				GameCanvas.gI().startDust(1, cx - 8, cy);
				addDustEff(1);
			}
			return;
		}
		cf = 12;
		if (me)
		{
			if (!isAttack)
			{
			}
			return;
		}
		if ((TileMap.tileTypeAtPixel(cx, cy + 1) & 2) == 2)
		{
			cf = 0;
		}
		if (currentMovePoint != null && cy > currentMovePoint.yEnd)
		{
			stop();
		}
	}

	public void updateCharFly()
	{
		int num = ((isMonkey != 1 || me) ? 1 : 2);
		setMountIsStart();
		if (statusMe != 16 && (TileMap.tileTypeAt(cx, cy - ch + 24, 8192) || cy < 0))
		{
			if (cy - ch < 0)
			{
				cy = ch;
			}
			cf = 7;
			statusMe = 4;
			cvx = 0;
			cp2 = 0;
			currentMovePoint = null;
			return;
		}
		int num2 = cy;
		cp1++;
		if (cp1 >= 9)
		{
			cp1 = 0;
			if (!me)
			{
				cvx = (cvy = 0);
			}
			cBonusSpeed = 0;
		}
		cf = 8;
		if (Res.abs(cvx) <= 4 && me)
		{
			if (currentMovePoint != null)
			{
				int num3 = abs(cx - currentMovePoint.xEnd);
				int num4 = abs(cy - currentMovePoint.yEnd);
				if (num3 > num4 * 10)
				{
					cf = 8;
				}
				else if (num3 > num4 && num3 > 48 && num4 > 32)
				{
					cf = 8;
				}
				else
				{
					cf = 7;
				}
			}
			else
			{
				if (cvy < 0)
				{
					cvy = 0;
				}
				if (cvy > 16)
				{
					cvy = 16;
				}
				cf = 7;
			}
		}
		if (!me)
		{
			if (abs(cvx) < 2)
			{
				cvx = (cdir << 1) * num;
			}
			if (cvy != 0)
			{
				cf = 7;
			}
			if (abs(cvx) <= 2)
			{
				cp2++;
				if (cp2 > 32)
				{
					statusMe = 4;
					cvx = 0;
					cvy = 0;
				}
			}
		}
		if (cdir == 1)
		{
			if (TileMap.tileTypeAt(cx + chw, cy - 1, 4))
			{
				cvx = 0;
				cx = TileMap.tileXofPixel(cx + chw) - chw;
				if (cvy == 0)
				{
					currentMovePoint = null;
				}
			}
		}
		else if (TileMap.tileTypeAt(cx - chw - 1, cy - 1, 8))
		{
			cvx = 0;
			cx = TileMap.tileXofPixel(cx - chw - 1) + TileMap.size + chw;
			if (cvy == 0)
			{
				currentMovePoint = null;
			}
		}
		cx += cvx * num;
		cy += cvy * num;
		if (!isMount && num2 - cy == 0)
		{
			ty++;
			wt++;
			fy += ((!wy) ? 1 : (-1));
			if (wt == 10)
			{
				wt = 0;
				wy = !wy;
			}
			if (ty > 20)
			{
				delayFall = 10;
				if (GameCanvas.gameTick % 3 == 0)
				{
					ServerEffect.addServerEffect(111, cx + ((cdir != 1) ? 27 : (-17)), cy + fy + 13, 1, (cdir != 1) ? 2 : 0);
				}
			}
		}
		if (!me)
		{
			return;
		}
		if (cvx > 0)
		{
			cvx--;
		}
		else if (cvx < 0)
		{
			cvx++;
		}
		else if (cvy == 0)
		{
			statusMe = 4;
			checkDelayFallIfTooHigh();
			Service.gI().charMove();
		}
		if ((TileMap.tileTypeAtPixel(cx, cy + 20) & 2) == 2 || (TileMap.tileTypeAtPixel(cx, cy + 40) & 2) == 2)
		{
			if (cvy == 0)
			{
				delayFall = 0;
			}
			cyStartFall = 0;
			cvx = (cvy = 0);
			cp1 = (cp2 = 0);
			statusMe = 4;
			addDustEff(3);
		}
		if (abs(cx - cxSend) > 96 || abs(cy - cySend) > 24)
		{
			Service.gI().charMove();
		}
	}

	public void setMount(int cid, int ctrans, int cgender)
	{
		idcharMount = cid;
		transMount = ctrans;
		genderMount = cgender;
		speedMount = 30;
		if (transMount < 0)
		{
			transMount = 0;
			xMount = GameScr.cmx + GameCanvas.w + 50;
			dxMount = -19;
		}
		else if (transMount == 1)
		{
			transMount = 2;
			xMount = GameScr.cmx - 100;
			dxMount = -33;
		}
		dyMount = -17;
		yMount = cy;
		frameMount = 0;
		frameNewMount = 0;
		isMount = false;
		isEndMount = false;
	}

	public void updateMount()
	{
		frameMount++;
		if (frameMount > FrameMount.Length - 1)
		{
			frameMount = 0;
		}
		frameNewMount++;
		if (frameNewMount > 1000)
		{
			frameNewMount = 0;
		}
		if (isStartMount && !isMount)
		{
			yMount = cy;
			if (transMount == 0)
			{
				if (xMount - cx >= speedMount)
				{
					xMount -= speedMount;
					return;
				}
				xMount = cx;
				isMount = true;
				isEndMount = false;
			}
			else if (transMount == 2)
			{
				if (cx - xMount >= speedMount)
				{
					xMount += speedMount;
					return;
				}
				xMount = cx;
				isMount = true;
				isEndMount = false;
			}
		}
		else if (isMount)
		{
			if (statusMe == 14 || ySd - cy < 24)
			{
				setMountIsEnd();
			}
			if (cp1 % 15 < 5)
			{
				cf = 0;
			}
			else
			{
				cf = 1;
			}
			transMount = cdir;
			updateSuperEff();
			if (transMount < 0)
			{
				transMount = 0;
				dxMount = -19;
			}
			else if (transMount == 1)
			{
				transMount = 2;
				dxMount = -31;
				if (isEventMount)
				{
					dxMount = -38;
				}
			}
			if (skillInfoPaint() != null)
			{
				dyMount = -15;
			}
			else
			{
				dyMount = -17;
			}
			yMount = cy;
			xMount = cx;
		}
		else if (isEndMount)
		{
			if (transMount == 0)
			{
				if (xMount > GameScr.cmx - 100)
				{
					xMount -= 20;
					return;
				}
				isStartMount = false;
				isMount = false;
				isEndMount = false;
			}
			else if (transMount == 2)
			{
				if (xMount < GameScr.cmx + GameCanvas.w + 50)
				{
					xMount += 20;
					return;
				}
				isStartMount = false;
				isMount = false;
				isEndMount = false;
			}
		}
		else if (!isStartMount || !isMount || !isEndMount)
		{
			xMount = GameScr.cmx - 100;
			yMount = GameScr.cmy - 100;
		}
	}

	public void getMountData()
	{
		if (Mob.arrMobTemplate[50].data == null)
		{
			Mob.arrMobTemplate[50].data = new EffectData();
			string text = "/Mob/" + 50;
			DataInputStream dataInputStream = null;
			dataInputStream = MyStream.readFile(text);
			if (dataInputStream != null)
			{
				Mob.arrMobTemplate[50].data.readData(text + "/data");
				Mob.arrMobTemplate[50].data.img = GameCanvas.loadImage(text + "/img.png");
			}
			else
			{
				Service.gI().requestModTemplate(50);
			}
			Mob.lastMob.addElement(50 + string.Empty);
		}
	}

	public void checkFrameTick(int[] array)
	{
		t++;
		if (t > array.Length - 1)
		{
			t = 0;
		}
		fM = array[t];
	}

	public void paintMount1(mGraphics g)
	{
		if (xMount <= GameScr.cmx || xMount >= GameScr.cmx + GameCanvas.w)
		{
			return;
		}
		if (me)
		{
			if (!isEndMount && !isStartMount && !isMount)
			{
				return;
			}
			if (idMount >= ID_NEW_MOUNT)
			{
				string nameImg = strMount + (idMount - ID_NEW_MOUNT) + "_0";
				FrameImage fraImage = mSystem.getFraImage(nameImg);
				fraImage?.drawFrame(frameNewMount / 2 % fraImage.nFrame, xMount, yMount + fy, transMount, 3, g);
			}
			else
			{
				if (isSpeacialMount)
				{
					return;
				}
				if (isEventMount)
				{
					g.drawRegion(imgEventMountWing, 0, FrameMount[frameMount] * 60, 60, 60, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
				else if (genderMount == 2)
				{
					if (!isMountVip)
					{
						g.drawRegion(imgMount_XD, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
					}
					else
					{
						g.drawRegion(imgMount_XD_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
					}
				}
				else if (genderMount == 1)
				{
					if (!isMountVip)
					{
						g.drawRegion(imgMount_NM, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
					}
					else
					{
						g.drawRegion(imgMount_NM_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
					}
				}
			}
		}
		else
		{
			if (me)
			{
				return;
			}
			if (idMount >= ID_NEW_MOUNT)
			{
				string nameImg2 = strMount + (idMount - ID_NEW_MOUNT) + "_0";
				FrameImage fraImage2 = mSystem.getFraImage(nameImg2);
				fraImage2?.drawFrame(frameNewMount / 2 % fraImage2.nFrame, xMount, yMount + fy, transMount, 3, g);
			}
			else
			{
				if (isSpeacialMount)
				{
					return;
				}
				if (isEventMount)
				{
					g.drawRegion(imgEventMountWing, 0, FrameMount[frameMount] * 60, 60, 60, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
				else
				{
					if (!isMount)
					{
						return;
					}
					if (genderMount == 2)
					{
						if (!isMountVip)
						{
							g.drawRegion(imgMount_XD, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
						}
						else
						{
							g.drawRegion(imgMount_XD_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
						}
					}
					else if (genderMount == 1)
					{
						if (!isMountVip)
						{
							g.drawRegion(imgMount_NM, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
						}
						else
						{
							g.drawRegion(imgMount_NM_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
						}
					}
				}
			}
		}
	}

	public void paintMount2(mGraphics g)
	{
		if (xMount <= GameScr.cmx || xMount >= GameScr.cmx + GameCanvas.w)
		{
			return;
		}
		if (me)
		{
			if (!isEndMount && !isStartMount && !isMount)
			{
				return;
			}
			if (idMount >= ID_NEW_MOUNT)
			{
				string nameImg = strMount + (idMount - ID_NEW_MOUNT) + "_1";
				FrameImage fraImage = mSystem.getFraImage(nameImg);
				fraImage?.drawFrame(frameNewMount / 2 % fraImage.nFrame, xMount, yMount + fy, transMount, 3, g);
			}
			else if (isSpeacialMount)
			{
				checkFrameTick(move);
				if (Mob.arrMobTemplate[50] != null && Mob.arrMobTemplate[50].data != null)
				{
					Mob.arrMobTemplate[50].data.paintFrame(g, fM, xMount + ((cdir != 1) ? 8 : (-8)), yMount + 35, (cdir != 1) ? 1 : 0, 0);
				}
				else
				{
					getMountData();
				}
			}
			else if (isEventMount)
			{
				g.drawRegion(imgEventMount, 0, FrameMount[frameMount] * 60, 60, 60, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
			}
			else if (genderMount == 0)
			{
				if (!isMountVip)
				{
					g.drawRegion(imgMount_TD, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
				else
				{
					g.drawRegion(imgMount_TD_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
			}
			else if (genderMount == 1)
			{
				if (!isMountVip)
				{
					g.drawRegion(imgMount_NM_1, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
				else
				{
					g.drawRegion(imgMount_NM_1_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
			}
		}
		else
		{
			if (me)
			{
				return;
			}
			if (idMount >= ID_NEW_MOUNT)
			{
				string nameImg2 = strMount + (idMount - ID_NEW_MOUNT) + "_1";
				FrameImage fraImage2 = mSystem.getFraImage(nameImg2);
				fraImage2?.drawFrame(frameNewMount / 2 % fraImage2.nFrame, xMount, yMount + fy, transMount, 3, g);
				return;
			}
			if (isSpeacialMount)
			{
				checkFrameTick(move);
				if (Mob.arrMobTemplate[50] != null && Mob.arrMobTemplate[50].data != null)
				{
					Mob.arrMobTemplate[50].data.paintFrame(g, fM, xMount + ((cdir != 1) ? 8 : (-8)), yMount + 35, (cdir != 1) ? 1 : 0, 0);
				}
				else
				{
					getMountData();
				}
				return;
			}
			if (isEventMount)
			{
				g.drawRegion(imgEventMount, 0, FrameMount[frameMount] * 60, 60, 60, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
			}
			if (!isMount)
			{
				return;
			}
			if (genderMount == 0)
			{
				if (!isMountVip)
				{
					g.drawRegion(imgMount_TD, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
				else
				{
					g.drawRegion(imgMount_TD_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
			}
			else if (genderMount == 1)
			{
				if (!isMountVip)
				{
					g.drawRegion(imgMount_NM_1, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
				else
				{
					g.drawRegion(imgMount_NM_1_VIP, 0, FrameMount[frameMount] * 40, 50, 40, transMount, xMount + dxMount, yMount + dyMount + fy, 0);
				}
			}
		}
	}

	public void setMountIsStart()
	{
		if (me)
		{
			isHaveMount = checkHaveMount();
			if (TileMap.mapID == 112 || TileMap.mapID == 113 || TileMap.mapID == 51 || TileMap.mapID == 103)
			{
				isHaveMount = false;
			}
		}
		if (isHaveMount)
		{
			if (ySd - cy <= 20)
			{
				xChar = cx;
			}
			if (xdis < 100)
			{
				xdis = Res.abs(xChar - cx);
			}
			if (xdis >= 70 && ySd - cy > 30 && !isStartMount && !isEndMount)
			{
				setMount(charID, cdir, cgender);
				isStartMount = true;
			}
		}
	}

	public void setMountIsEnd()
	{
		if (ySd - cy < 24 && !isEndMount)
		{
			isStartMount = false;
			isMount = false;
			isEndMount = true;
			xdis = 0;
		}
	}

	public bool checkHaveMount()
	{
		bool result = false;
		short num = -1;
		for (int i = 0; i < arrItemBag.Length; i++)
		{
			if (arrItemBag[i] != null && (arrItemBag[i].template.type == 24 || arrItemBag[i].template.type == 23))
			{
				num = ((arrItemBag[i].template.part < 0) ? arrItemBag[i].template.id : ((short)(ID_NEW_MOUNT + arrItemBag[i].template.part)));
				result = true;
				break;
			}
		}
		isMountVip = false;
		isSpeacialMount = false;
		isEventMount = false;
		idMount = -1;
		switch (num)
		{
		case 349:
		case 350:
		case 351:
			isMountVip = true;
			break;
		case 396:
			isEventMount = true;
			break;
		case 532:
			isSpeacialMount = true;
			break;
		default:
			if (num >= ID_NEW_MOUNT)
			{
				idMount = num;
			}
			break;
		}
		return result;
	}

	private void checkDelayFallIfTooHigh()
	{
		bool flag = true;
		for (int i = 0; i < 150; i += 24)
		{
			if ((TileMap.tileTypeAtPixel(cx, cy + i) & 2) == 2 || cy + i > TileMap.tmh * TileMap.size - 24)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			delayFall = 40;
		}
	}

	public void setDefaultPart()
	{
		setDefaultWeapon();
		setDefaultBody();
		setDefaultLeg();
	}

	public void setDefaultWeapon()
	{
		if (cgender == 0)
		{
			wp = 0;
		}
	}

	public void setDefaultBody()
	{
		if (cgender == 0)
		{
			body = 57;
		}
		else if (cgender == 1)
		{
			body = 59;
		}
		else if (cgender == 2)
		{
			body = 57;
		}
	}

	public void setDefaultLeg()
	{
		if (cgender == 0)
		{
			leg = 58;
		}
		else if (cgender == 1)
		{
			leg = 60;
		}
		else if (cgender == 2)
		{
			leg = 58;
		}
	}

	public bool isSelectingSkillUseAlone()
	{
		return myskill != null && myskill.template.isUseAlone();
	}

	public bool isSelectingSkillBuffToPlayer()
	{
		return myskill != null && myskill.template.isBuffToPlayer();
	}

	public bool isUseChargeSkill()
	{
		return !isUseSkillAfterCharge && myskill != null && (myskill.template.id == 10 || myskill.template.id == 11);
	}

	public void setSkillPaint(SkillPaint skillPaint, int sType)
	{
        if (me && GameEvents.onUseSkill(myskill)) return;
        hasSendAttack = false;
		if (stone || (me && myskill.template.id == 9 && cHP <= cHPFull / 10))
		{
			return;
		}
		if (me)
		{
			if (mobFocus == null && charFocus == null)
			{
				stopUseChargeSkill();
			}
			if (mobFocus != null && (mobFocus.status == 1 || mobFocus.status == 0))
			{
				stopUseChargeSkill();
			}
			if (charFocus != null && (charFocus.statusMe == 14 || charFocus.statusMe == 5))
			{
				stopUseChargeSkill();
			}
			if ((myskill.template.id == 23 && ((charFocus != null && charFocus.holdEffID != 0) || (mobFocus != null && mobFocus.holdEffID != 0) || holdEffID != 0)) || sleepEff || blindEff)
			{
				return;
			}
		}
		Res.outz("skill id= " + skillPaint.id);
		if ((me && dart != null) || TileMap.isOfflineMap())
		{
			return;
		}
		long num = mSystem.currentTimeMillis();
		if (me)
		{
			if (isSelectingSkillBuffToPlayer() && charFocus == null)
			{
				return;
			}
			if (num - myskill.lastTimeUseThisSkill < myskill.coolDown)
			{
				myskill.paintCanNotUseSkill = true;
				return;
			}
			myskill.lastTimeUseThisSkill = num;
			if (myskill.template.manaUseType == 2)
			{
				cMP = 1;
			}
			else if (myskill.template.manaUseType != 1)
			{
				cMP -= myskill.manaUse;
			}
			else
			{
				cMP -= myskill.manaUse * cMPFull / 100;
			}
			myCharz().cStamina--;
			GameScr.gI().isInjureMp = true;
			GameScr.gI().twMp = 0;
			if (cMP < 0)
			{
				cMP = 0;
			}
		}
		if (me)
		{
			if (myskill.template.id == 7)
			{
				SoundMn.gI().hoisinh();
			}
			if (myskill.template.id == 6)
			{
				Service.gI().skill_not_focus(0);
				GameScr.gI().isUseFreez = true;
				SoundMn.gI().thaiduonghasan();
			}
			if (myskill.template.id == 8)
			{
				if (!isCharge)
				{
					SoundMn.gI().taitaoPause();
					Service.gI().skill_not_focus(1);
					isCharge = true;
					last = (cur = mSystem.currentTimeMillis());
				}
				else
				{
					Service.gI().skill_not_focus(3);
					isCharge = false;
					SoundMn.gI().taitaoPause();
				}
			}
			if (myskill.template.id == 13)
			{
				if (isMonkey != 0)
				{
					GameScr.gI().auto = 0;
				}
				else if (!isCreateDark)
				{
					SoundMn.gI().gong();
					Service.gI().skill_not_focus(6);
					chargeCount = 0;
					isWaitMonkey = true;
				}
				return;
			}
			if (myskill.template.id == 14)
			{
				SoundMn.gI().gong();
				Service.gI().skill_not_focus(7);
				useChargeSkill(isGround: true);
			}
			if (myskill.template.id == 21)
			{
				Service.gI().skill_not_focus(10);
				return;
			}
			if (myskill.template.id == 12)
			{
				Service.gI().skill_not_focus(8);
			}
			if (myskill.template.id == 19)
			{
				Service.gI().skill_not_focus(9);
				return;
			}
		}
		if (isMonkey == 1 && skillPaint.id >= 35 && skillPaint.id <= 41)
		{
			skillPaint = GameScr.sks[106];
		}
		if (skillPaint.id >= 128 && skillPaint.id <= 134)
		{
			skillPaint = GameScr.sks[skillPaint.id - 65];
			if (charFocus != null)
			{
				cx = charFocus.cx;
				cy = charFocus.cy;
				currentMovePoint = null;
			}
			if (mobFocus != null)
			{
				cx = mobFocus.x;
				cy = mobFocus.y;
				currentMovePoint = null;
			}
			ServerEffect.addServerEffect(60, cx, cy, 1);
			telePortSkill = true;
		}
		if (skillPaint.id >= 107 && skillPaint.id <= 113)
		{
			skillPaint = GameScr.sks[skillPaint.id - 44];
			EffecMn.addEff(new Effect(23, cx, cy + ch / 2, 3, 2, 1));
		}
		setAutoSkillPaint(skillPaint, sType);
	}

	public void useSkillNotFocus()
	{
		GameScr.gI().auto = 0;
		myCharz().setSkillPaint(GameScr.sks[myCharz().myskill.skillId], (!TileMap.tileTypeAt(myCharz().cx, myCharz().cy, 2)) ? 1 : 0);
	}

	public void sendUseChargeSkill()
	{
		if (me && (isFreez || isUsePlane))
		{
			GameScr.gI().auto = 0;
			return;
		}
		long num = mSystem.currentTimeMillis();
		if (me && num - myskill.lastTimeUseThisSkill < myskill.coolDown)
		{
			myskill.paintCanNotUseSkill = true;
			return;
		}
		if (myskill.template.id == 10)
		{
			useChargeSkill(isGround: false);
		}
		if (myskill.template.id == 11)
		{
			useChargeSkill(isGround: true);
		}
	}

	public void stopUseChargeSkill()
	{
		isFlyAndCharge = false;
		isStandAndCharge = false;
		isUseSkillAfterCharge = false;
		isCreateDark = false;
		if (me && statusMe != 14 && statusMe != 5)
		{
			isLockMove = false;
		}
		GameScr.gI().auto = 0;
	}

	public void useChargeSkill(bool isGround)
	{
		if (isCreateDark)
		{
			return;
		}
		GameScr.gI().auto = 0;
		if (isGround)
		{
			if (isStandAndCharge)
			{
				return;
			}
			chargeCount = 0;
			seconds = 50000;
			posDisY = 0;
			last = mSystem.currentTimeMillis();
			if (me)
			{
				isLockMove = true;
				if (cgender == 1)
				{
					Service.gI().skill_not_focus(4);
				}
			}
			if (cgender == 1)
			{
				SoundMn.gI().gongName();
			}
			isStandAndCharge = true;
		}
		else if (!isFlyAndCharge)
		{
			if (me)
			{
				GameScr.gI().auto = 0;
				isLockMove = true;
				Service.gI().skill_not_focus(4);
			}
			isUseSkillAfterCharge = false;
			chargeCount = 0;
			isFlyAndCharge = true;
			posDisY = 0;
			seconds = 50000;
			isFlying = TileMap.tileTypeAt(cx, cy, 2);
		}
	}

	public void setAutoSkillPaint(SkillPaint skillPaint, int sType)
	{
		this.skillPaint = skillPaint;
		Res.outz("set auto skill " + ((skillPaint == null) ? "null" : "!null"));
		if (skillPaint.id >= 0 && skillPaint.id <= 6)
		{
			int num = Res.random(0, skillPaint.id + 4) - 1;
			if (num < 0)
			{
				num = 0;
			}
			if (num > 6)
			{
				num = 6;
			}
			skillPaintRandomPaint = GameScr.sks[num];
		}
		else if (skillPaint.id >= 14 && skillPaint.id <= 20)
		{
			int num2 = Res.random(0, skillPaint.id - 14 + 4) - 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > 6)
			{
				num2 = 6;
			}
			skillPaintRandomPaint = GameScr.sks[num2 + 14];
		}
		else if (skillPaint.id >= 28 && skillPaint.id <= 34)
		{
			int num3 = Res.random(0, ((isMonkey != 1) ? skillPaint.id : 105) - ((isMonkey != 1) ? 28 : 105) + 4) - 1;
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 > 6)
			{
				num3 = 6;
			}
			if (isMonkey == 1)
			{
				num3 = 0;
			}
			skillPaintRandomPaint = GameScr.sks[num3 + ((isMonkey != 1) ? 28 : 105)];
		}
		else if (skillPaint.id >= 63 && skillPaint.id <= 69)
		{
			int num4 = Res.random(0, skillPaint.id - 63 + 4) - 1;
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num4 > 6)
			{
				num4 = 6;
			}
			skillPaintRandomPaint = GameScr.sks[num4 + 63];
		}
		else if (skillPaint.id >= 107 && skillPaint.id <= 109)
		{
			int num5 = Res.random(0, skillPaint.id - 107 + 4) - 1;
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num5 > 6)
			{
				num5 = 6;
			}
			skillPaintRandomPaint = GameScr.sks[num5 + 107];
		}
		else
		{
			skillPaintRandomPaint = skillPaint;
		}
		this.sType = sType;
		indexSkill = 0;
		i0 = (i1 = (i2 = (dx0 = (dx1 = (dx2 = (dy0 = (dy1 = (dy2 = 0))))))));
		eff0 = null;
		eff1 = null;
		eff2 = null;
		cvy = 0;
	}

	public SkillInfoPaint[] skillInfoPaint()
	{
		if (skillPaint == null)
		{
			return null;
		}
		if (skillPaintRandomPaint == null)
		{
			return null;
		}
		if (sType == 0)
		{
			return skillPaintRandomPaint.skillStand;
		}
		return skillPaintRandomPaint.skillfly;
	}

	public void setAttack()
	{
		if (me)
		{
			SkillPaint skillPaint = skillPaintRandomPaint;
			if (dart != null)
			{
				skillPaint = dart.skillPaint;
			}
			if (skillPaint == null)
			{
				return;
			}
			MyVector myVector = new MyVector();
			MyVector myVector2 = new MyVector();
			if (charFocus != null)
			{
				myVector2.addElement(charFocus);
			}
			else if (mobFocus != null)
			{
				myVector.addElement(mobFocus);
			}
			effPaints = new EffectPaint[myVector.size() + myVector2.size()];
			for (int i = 0; i < myVector.size(); i++)
			{
				effPaints[i] = new EffectPaint();
				effPaints[i].effCharPaint = GameScr.efs[skillPaint.effectHappenOnMob - 1];
				if (!isSelectingSkillUseAlone())
				{
					effPaints[i].eMob = (Mob)myVector.elementAt(i);
				}
			}
			for (int j = 0; j < myVector2.size(); j++)
			{
				effPaints[j + myVector.size()] = new EffectPaint();
				effPaints[j + myVector.size()].effCharPaint = GameScr.efs[skillPaint.effectHappenOnMob - 1];
				effPaints[j + myVector.size()].eChar = (Char)myVector2.elementAt(j);
			}
			int type = 0;
			if (mobFocus != null)
			{
				type = 1;
			}
			else if (charFocus != null)
			{
				type = 2;
			}
			if (myVector.size() == 0 && myVector2.size() == 0)
			{
				stopUseChargeSkill();
			}
			if (me && !isSelectingSkillUseAlone() && !hasSendAttack)
			{
				Service.gI().sendPlayerAttack(myVector, myVector2, type);
				hasSendAttack = true;
			}
			return;
		}
		SkillPaint skillPaint2 = skillPaintRandomPaint;
		if (dart != null)
		{
			skillPaint2 = dart.skillPaint;
		}
		if (skillPaint2 == null)
		{
			return;
		}
		if (attMobs != null)
		{
			effPaints = new EffectPaint[attMobs.Length];
			for (int k = 0; k < attMobs.Length; k++)
			{
				effPaints[k] = new EffectPaint();
				effPaints[k].effCharPaint = GameScr.efs[skillPaint2.effectHappenOnMob - 1];
				effPaints[k].eMob = attMobs[k];
			}
			attMobs = null;
		}
		else if (attChars != null)
		{
			effPaints = new EffectPaint[attChars.Length];
			for (int l = 0; l < attChars.Length; l++)
			{
				effPaints[l] = new EffectPaint();
				effPaints[l].effCharPaint = GameScr.efs[skillPaint2.effectHappenOnMob - 1];
				effPaints[l].eChar = attChars[l];
			}
			attChars = null;
		}
	}

	public bool isOutX()
	{
		if (cx < GameScr.cmx)
		{
			return true;
		}
		if (cx > GameScr.cmx + GameScr.gW)
		{
			return true;
		}
		return false;
	}

	public bool isPaint()
	{
		if (cy < GameScr.cmy)
		{
			return false;
		}
		if (cy > GameScr.cmy + GameScr.gH + 30)
		{
			return false;
		}
		if (isOutX())
		{
			return false;
		}
		if (isSetPos)
		{
			return false;
		}
		if (isFusion)
		{
			return false;
		}
		return true;
	}

	public void createShadow(int x, int y, int life)
	{
		shadowX = x;
		shadowY = y;
		shadowLife = life;
	}

	public void setMabuHold(bool m)
	{
		isMabuHold = m;
	}

	public virtual void paint(mGraphics g)
	{
		if (isHide || ModMenu.getStatusInt("levelreducegraphics") > 2)
		{
			return;
		}
		if (isMabuHold)
		{
			if (cmtoChar)
			{
				GameScr.cmtoX = cx - GameScr.gW2;
				GameScr.cmtoY = cy - GameScr.gH23;
				if (!GameCanvas.isTouchControl)
				{
					GameScr.cmtoX += GameScr.gW6 * cdir;
				}
			}
		}
		else
		{
			if (!isPaint() || (!me && GameScr.notPaint))
			{
				return;
			}
			if (petFollow != null)
			{
				petFollow.paint(g);
			}
			if (ModMenu.getStatusInt("levelreducegraphics") > 1)
			{
                g.setColor(65421);
                g.drawRect(xMount - 20, yMount, 40, 15);
            }
			else paintMount1(g);
			if ((TileMap.isInAirMap() && cy >= TileMap.pxh - 48) || isTeleport)
			{
				return;
			}
			if (holder && GameCanvas.gameTick % 2 == 0)
			{
				g.setColor(16185600);
				if (charHold != null)
				{
					g.drawLine(cx, cy - ch / 2, charHold.cx, charHold.cy - charHold.ch / 2);
				}
				if (mobHold != null)
				{
					g.drawLine(cx, cy - ch / 2, mobHold.x, mobHold.y - mobHold.h / 2);
				}
			}
			if (ModMenu.getStatusInt("levelreducegraphics") <= 1)
			{
				paintSuperEffBehind(g);
				paintAuraBehind(g);
				paintEffBehind(g);
				paintEff_Lvup_behind(g);
			}
			if (shadowLife > 0)
			{
				if (GameCanvas.gameTick % 2 == 0)
				{
					paintCharBody(g, shadowX, shadowY, cdir, 25, isPaintBag: true);
				}
				else if (shadowLife > 5)
				{
					paintCharBody(g, shadowX, shadowY, cdir, 7, isPaintBag: true);
				}
			}
			if (!isPaint() && skillPaint != null && (skillPaint.id < 70 || skillPaint.id > 76) && (skillPaint.id < 77 || skillPaint.id > 83))
			{
				if (skillPaint != null)
				{
					indexSkill = skillInfoPaint().Length;
					skillPaint = null;
				}
				effPaints = null;
				eff = null;
				effTask = null;
				indexEff = -1;
				indexEffTask = -1;
			}
			else if (statusMe != 15 && (moveFast == null || moveFast[0] <= 0))
			{
				paintCharName_HP_MP_Overhead(g);
				if (skillPaint == null || skillInfoPaint() == null || indexSkill >= skillInfoPaint().Length)
				{
					paintCharWithoutSkill(g);
				}
				if (arr != null)
				{
					arr.paint(g);
				}
				if (dart != null)
				{
					dart.paint(g);
				}
                if (ModMenu.getStatusInt("levelreducegraphics") <= 1) paintEffect(g);
				if (mobMe != null)
				{
				}
				if (ModMenu.getStatusInt("levelreducegraphics") <= 1)
				{
					paintMount2(g);
					paintEff_Lvup_front(g);
					paintSuperEffFront(g);
					paintAuraFront(g);
					paintEffFront(g);
				}
			}
		}
	}

	private void paintSuperEffBehind(mGraphics g)
	{
		if (me)
		{
			if (isPaintAura && idAuraEff > -1)
			{
				return;
			}
		}
		else if (idAuraEff > -1)
		{
			return;
		}
		if ((statusMe != 1 && statusMe != 6) || GameCanvas.panel.isShow || mSystem.currentTimeMillis() - timeBlue <= 0 || isCopy || clevel < 16)
		{
			return;
		}
		int num = 7598;
		int num2 = 4;
		if (clevel >= 19)
		{
			num = 7676;
		}
		if (clevel >= 22)
		{
			num = 7677;
		}
		if (clevel >= 25)
		{
			num = 7678;
		}
		if (num != -1)
		{
			Small small = SmallImage.imgNew[num];
			if (small == null)
			{
				SmallImage.createImage(num);
				return;
			}
			int y = GameCanvas.gameTick / 4 % num2 * (mGraphics.getImageHeight(small.img) / num2);
			g.drawRegion(small.img, 0, y, mGraphics.getImageWidth(small.img), mGraphics.getImageHeight(small.img) / num2, 0, cx, cy + 2, mGraphics.BOTTOM | mGraphics.HCENTER);
		}
	}

	private void paintSuperEffFront(mGraphics g)
	{
		if (me)
		{
			if (isPaintAura && idAuraEff > -1)
			{
				return;
			}
		}
		else if (idAuraEff > -1)
		{
			return;
		}
		if (statusMe == 1 || statusMe == 6)
		{
			if (GameCanvas.panel.isShow || mSystem.currentTimeMillis() - timeBlue <= 0)
			{
				return;
			}
			if (isCopy)
			{
				if (GameCanvas.gameTick % 2 == 0)
				{
					tBlue++;
				}
				if (tBlue > 6)
				{
					tBlue = 0;
				}
				g.drawImage(GameCanvas.imgViolet[tBlue], cx, cy + 9, mGraphics.BOTTOM | mGraphics.HCENTER);
				return;
			}
			if (clevel >= 14 && !GameCanvas.lowGraphic)
			{
				bool flag = false;
				if (mSystem.currentTimeMillis() - timeBlue > -1000 && IsAddDust1)
				{
					flag = true;
					IsAddDust1 = false;
				}
				if (mSystem.currentTimeMillis() - timeBlue > -500 && IsAddDust2)
				{
					flag = true;
					IsAddDust2 = false;
				}
				if (flag)
				{
					GameCanvas.gI().startDust(-1, cx - -8, cy);
					GameCanvas.gI().startDust(1, cx - 8, cy);
					addDustEff(1);
				}
			}
			if (clevel == 14)
			{
				if (GameCanvas.gameTick % 2 == 0)
				{
					tBlue++;
				}
				if (tBlue > 6)
				{
					tBlue = 0;
				}
				g.drawImage(GameCanvas.imgBlue[tBlue], cx, cy + 9, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			else if (clevel == 15)
			{
				if (GameCanvas.gameTick % 2 == 0)
				{
					tBlue++;
				}
				if (tBlue > 6)
				{
					tBlue = 0;
				}
				g.drawImage(GameCanvas.imgViolet[tBlue], cx, cy + 9, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			else
			{
				if (clevel < 16)
				{
					return;
				}
				int num = -1;
				int num2 = 4;
				if (clevel >= 16 && clevel < 22)
				{
					num = 7599;
					num2 = 4;
				}
				if (num != -1)
				{
					Small small = SmallImage.imgNew[num];
					if (small == null)
					{
						SmallImage.createImage(num);
						return;
					}
					int y = GameCanvas.gameTick / 4 % num2 * (mGraphics.getImageHeight(small.img) / num2);
					g.drawRegion(small.img, 0, y, mGraphics.getImageWidth(small.img), mGraphics.getImageHeight(small.img) / num2, 0, cx, cy + 2, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
			}
		}
		else
		{
			timeBlue = mSystem.currentTimeMillis() + 1500;
			IsAddDust1 = true;
			IsAddDust2 = true;
		}
	}

	private void paintEffect(mGraphics g)
	{
		if (effPaints != null)
		{
			for (int i = 0; i < effPaints.Length; i++)
			{
				if (effPaints[i] == null)
				{
					continue;
				}
				if (effPaints[i].eMob != null)
				{
					int y = effPaints[i].eMob.y;
					if (effPaints[i].eMob is BigBoss)
					{
						y = effPaints[i].eMob.y - 60;
					}
					if (effPaints[i].eMob is BigBoss2)
					{
						y = effPaints[i].eMob.y - 50;
					}
					if (effPaints[i].eMob is BachTuoc)
					{
						y = effPaints[i].eMob.y - 40;
					}
					SmallImage.drawSmallImage(g, effPaints[i].getImgId(), effPaints[i].eMob.x, y, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
				else if (effPaints[i].eChar != null)
				{
					SmallImage.drawSmallImage(g, effPaints[i].getImgId(), effPaints[i].eChar.cx, effPaints[i].eChar.cy, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
			}
		}
		if (indexEff >= 0 && eff != null)
		{
			SmallImage.drawSmallImage(g, eff.arrEfInfo[indexEff].idImg, cx + eff.arrEfInfo[indexEff].dx, cy + eff.arrEfInfo[indexEff].dy, 0, mGraphics.VCENTER | mGraphics.HCENTER);
		}
		if (indexEffTask >= 0 && effTask != null)
		{
			SmallImage.drawSmallImage(g, effTask.arrEfInfo[indexEffTask].idImg, cx + effTask.arrEfInfo[indexEffTask].dx, cy + effTask.arrEfInfo[indexEffTask].dy, 0, mGraphics.VCENTER | mGraphics.HCENTER);
		}
	}

	private void paintArrowAttack(mGraphics g)
	{
	}

	public void paintHp(mGraphics g, int x, int y)
	{
		int num = cHP * 100 / cHPFull / 10 - 1;
		if (num < 0)
		{
			num = 0;
		}
		if (num > 9)
		{
			num = 9;
		}
		g.drawRegion(Mob.imgHP, 0, 6 * (9 - num), 9, 6, 0, x, y, 3);
	}

	public int getClassColor()
	{
		int result = 9145227;
		if (nClass.classId == 1 || nClass.classId == 2)
		{
			result = 16711680;
		}
		else if (nClass.classId == 3 || nClass.classId == 4)
		{
			result = 33023;
		}
		else if (nClass.classId == 5 || nClass.classId == 6)
		{
			result = 7443811;
		}
		return result;
	}

	public void paintNameInSameParty(mGraphics g)
	{
		if (cTypePk != 3 && cTypePk != 5 && isPaint())
		{
			if (myCharz().charFocus == null || !myCharz().charFocus.Equals(this))
			{
				mFont.tahoma_7_yellow.drawString(g, cName, cx, cy - ch - mFont.tahoma_7_green.getHeight() - 5, mFont.CENTER, mFont.tahoma_7_grey);
			}
			else if (myCharz().charFocus != null && myCharz().charFocus.Equals(this))
			{
				mFont.tahoma_7_yellow.drawString(g, cName, cx, cy - ch - mFont.tahoma_7_green.getHeight() - 10, mFont.CENTER, mFont.tahoma_7_grey);
			}
		}
	}

	private void paintCharName_HP_MP_Overhead(mGraphics g)
	{
		int num = ch + 5;
		if ((isInvisiblez && !me) || (!me && TileMap.mapID == 113 && cy >= 360) || me)
		{
			return;
		}
		bool flag = myChar.clan != null && clanID == myChar.clan.ID;
		bool flag2 = cTypePk == 3 || cTypePk == 5;
		bool flag3 = cTypePk == 4;
		if (cName.StartsWith("$"))
		{
			cName = cName.Substring(1);
			isPet = true;
		}
		if (cName.StartsWith("#"))
		{
			cName = cName.Substring(1);
			isMiniPet = true;
		}
		if (myCharz().charFocus != null && myCharz().charFocus.Equals(this))
		{
			num += 5;
			paintHp(g, cx, cy - num + 3);
		}
		num += mFont.tahoma_7b_white.getHeight();
		mFont mFont2 = mFont.tahoma_7_whiteSmall;
		if (isNhapThe)
		{
			num += 10;
		}
		if (isPet || isMiniPet)
		{
			mFont2 = mFont.tahoma_7_blue1Small;
		}
		else if (flag2)
		{
			mFont2 = mFont.nameFontRed;
		}
		else if (flag3)
		{
			mFont2 = mFont.nameFontYellow;
		}
		else if (flag)
		{
			mFont2 = mFont.nameFontGreen;
		}
		if ((paintName || flag2 || flag3) && !flag)
		{
			if (mSystem.clientType == 1)
			{
				mFont2.drawString(g, cName, cx, cy - num, mFont.CENTER, mFont.tahoma_7_greySmall);
			}
			else
			{
				mFont2.drawString(g, cName, cx, cy - num, mFont.CENTER);
			}
			num += mFont.tahoma_7.getHeight();
		}
		if (flag)
		{
			if (myCharz().charFocus != null && myCharz().charFocus.Equals(this))
			{
				mFont2.drawString(g, cName, cx, cy - num, mFont.CENTER, mFont.tahoma_7_greySmall);
			}
			else if (charFocus == null)
			{
				mFont2.drawString(g, cName, cx - 10, cy - num + 3, mFont.LEFT, mFont.tahoma_7_grey);
				paintHp(g, cx - 16, cy - num + 10);
			}
		}
	}

	public void paintShadow(mGraphics g)
	{
		if (isMabuHold || head == 377 || leg == 471 || isTeleport || isFlyUp)
		{
			return;
		}
		int num = TileMap.size;
		if ((TileMap.mapID < 114 || TileMap.mapID > 120) && TileMap.mapID != 127 && TileMap.mapID != 128)
		{
			if (TileMap.tileTypeAt(xSd + num / 2, ySd + 1, 4))
			{
				g.setClip(xSd / num * num, (ySd - 30) / num * num, num, 100);
			}
			else if (TileMap.tileTypeAt((xSd - num / 2) / num, (ySd + 1) / num) == 0)
			{
				g.setClip(xSd / num * num, (ySd - 30) / num * num, 100, 100);
			}
			else if (TileMap.tileTypeAt((xSd + num / 2) / num, (ySd + 1) / num) == 0)
			{
				g.setClip(xSd / num * num, (ySd - 30) / num * num, num, 100);
			}
			else if (TileMap.tileTypeAt(xSd - num / 2, ySd + 1, 8))
			{
				g.setClip(xSd / 24 * num, (ySd - 30) / num * num, num, 100);
			}
		}
		g.drawImage(TileMap.bong, xSd, ySd, 3);
		g.setClip(GameScr.cmx, GameScr.cmy - GameCanvas.transY, GameScr.gW, GameScr.gH + 2 * GameCanvas.transY);
	}

	public void updateShadown()
	{
		int num = 0;
		xSd = cx;
		if (TileMap.tileTypeAt(cx, cy, 2))
		{
			ySd = cy;
			return;
		}
		ySd = cy;
		while (num < 30)
		{
			num++;
			ySd += 24;
			if (TileMap.tileTypeAt(xSd, ySd, 2))
			{
				if (ySd % 24 != 0)
				{
					ySd -= ySd % 24;
				}
				break;
			}
		}
	}

	private void paintCharWithoutSkill(mGraphics g)
	{
		try
		{
			if (isInvisiblez)
			{
				if (me)
				{
					if (GameCanvas.gameTick % 50 == 48 || GameCanvas.gameTick % 50 == 90)
					{
						SmallImage.drawSmallImage(g, 1196, cx, cy - 18, 0, mGraphics.VCENTER | mGraphics.HCENTER);
					}
					else
					{
						SmallImage.drawSmallImage(g, 1195, cx, cy - 18, 0, mGraphics.VCENTER | mGraphics.HCENTER);
					}
				}
			}
			else
			{
				paintCharBody(g, cx, cy + fy, cdir, cf, isPaintBag: true);
			}
			if (isLockAttack)
			{
				SmallImage.drawSmallImage(g, 290, cx, cy, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi paint char without skill: " + ex.ToString());
		}
	}

	public void paintBag(mGraphics g, short[] id, int x, int y, int dir, bool isPaintChar)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		num4 = StaticObj.VCENTER_HCENTER;
		num3 = ((dir != 1) ? 2 : 0);
		if (statusMe == 6)
		{
			num = 8;
			num2 = 17;
		}
		if (statusMe == 1)
		{
			if (cp1 % 15 < 5)
			{
				num = 8;
				num2 = 17;
			}
			else
			{
				num = 8;
				num2 = 18;
			}
		}
		if (statusMe == 2)
		{
			if (cf <= 3)
			{
				num = 7;
				num2 = 17;
			}
			else
			{
				num = 7;
				num2 = 18;
			}
		}
		if (statusMe == 3 || statusMe == 9)
		{
			num = 5;
			num2 = 20;
		}
		if (statusMe == 4)
		{
			if (cf == 8)
			{
				num = 5;
				num2 = 16;
			}
			else
			{
				num = 5;
				num2 = 20;
			}
		}
		if (statusMe == 10)
		{
			Res.outz("cf= " + cf);
			if (cf == 8)
			{
				num = 0;
				num2 = 23;
			}
			else
			{
				num = 5;
				num2 = 22;
			}
		}
		if (isInjure > 0)
		{
			num = 5;
			num2 = 18;
		}
		if (skillPaint != null && skillInfoPaint() != null && indexSkill < skillInfoPaint().Length)
		{
			num = -1;
			num2 = 17;
		}
		sbyte b = (sbyte)(GameCanvas.gameTick / 4 % id.Length);
		if (!isPaintChar && b == 0)
		{
			b = 1;
		}
		SmallImage.drawSmallImage(g, id[b], x + ((dir != 1) ? num : (-num)), y - num2, num3, num4);
	}

	public bool isCharBodyImageID(int id)
	{
		Part part = GameScr.parts[head];
		Part part2 = GameScr.parts[leg];
		Part part3 = GameScr.parts[body];
		for (int i = 0; i < CharInfo.Length; i++)
		{
			if (id == part.pi[CharInfo[i][0][0]].id)
			{
				return true;
			}
			if (id == part2.pi[CharInfo[i][1][0]].id)
			{
				return true;
			}
			if (id == part3.pi[CharInfo[i][2][0]].id)
			{
				return true;
			}
		}
		return false;
	}

	public void paintHead(mGraphics g, int cx, int cy, int look)
	{
		Part part = GameScr.parts[head];
		SmallImage.drawSmallImage(g, part.pi[CharInfo[0][0][0]].id, cx, cy, (look != 0) ? 2 : 0, mGraphics.RIGHT | mGraphics.VCENTER);
	}

	public void paintHeadWithXY(mGraphics g, int x, int y, int look)
	{
		Part part = GameScr.parts[head];
		SmallImage.drawSmallImage(g, part.pi[CharInfo[0][0][0]].id, x + CharInfo[0][0][1] + part.pi[CharInfo[0][0][0]].dx - 3, y + 3, look, mGraphics.LEFT | mGraphics.BOTTOM);
	}

	public void paintCharBody(mGraphics g, int cx, int cy, int cdir, int cf, bool isPaintBag)
	{
		if (ModMenu.getStatusInt("levelreducegraphics") > 2) return;
		if (ModMenu.getStatusInt("levelreducegraphics") > 1)
		{
			if (bag >= 0 && statusMe != 14 && isMonkey == 0)
			{
				if (!ClanImage.idImages.containsKey(bag.ToString() + string.Empty))
				{
					ClanImage.idImages.put(bag.ToString() + string.Empty, new ClanImage());
					Service.gI().requestBagImage((sbyte)bag);
				}
				else
				{
					ClanImage clanImage = (ClanImage)ClanImage.idImages.get(bag.ToString() + string.Empty);
					if (clanImage.idImage != null && isPaintBag) paintBag(g, clanImage.idImage, cx, cy, cdir, true);
				}
			}
			g.setColor(Color.white);
			if (me) g.setColor(Color.blue);
			if (isPet || isMiniPet) g.setColor(Color.cyan);
			else if (cTypePk == 5)
			{
				g.setColor(Color.red);
				if ((isCharge || isFlyAndCharge) && GameCanvas.gameTick % 8 >= 4) g.setColor(Color.white);
			}
			int height = 35;
			int width = 12;
			if (isPet || isMiniPet) height = 30;
			if (cTypePk == 5)
			{
				width = 15;
				height = 40;
			}
			g.drawRect(cx - width, cy - height, width * 2, height);
			if (statusMe == 14)
			{
				if (GameCanvas.gameTick % 4 > 0) g.drawImage(ItemMap.imageFlare, cx, cy - ch - 11, mGraphics.HCENTER | mGraphics.VCENTER);
				SmallImage.drawSmallImage(g, 79, cx, cy - ch - 8, 0, mGraphics.HCENTER | mGraphics.BOTTOM);
			}
			if (eProtect != null) eProtect.paint(g);
			if (bag >= 0 && statusMe != 14 && isMonkey == 0)
			{
				if (!ClanImage.idImages.containsKey(bag.ToString() + string.Empty))
				{
					ClanImage.idImages.put(bag.ToString() + string.Empty, new ClanImage());
					Service.gI().requestBagImage((sbyte)bag);
				}
				ClanImage clanImage2 = (ClanImage)ClanImage.idImages.get(bag.ToString() + string.Empty);
				if (clanImage2.idImage != null && isPaintBag) paintBag(g, clanImage2.idImage, cx, cy, cdir, true);
			}
			paintPKFlag(g);
			return;
		}
        Part part = GameScr.parts[head];
		Part part2 = GameScr.parts[leg];
		Part part3 = GameScr.parts[body];
		if (bag >= 0 && statusMe != 14 && isMonkey == 0)
		{
			if (!ClanImage.idImages.containsKey(bag + string.Empty))
			{
				ClanImage.idImages.put(bag + string.Empty, new ClanImage());
				Service.gI().requestBagImage((sbyte)bag);
			}
			else
			{
				ClanImage clanImage = (ClanImage)ClanImage.idImages.get(bag + string.Empty);
				if (clanImage.idImage != null && isPaintBag)
				{
					paintBag(g, clanImage.idImage, cx, cy, cdir, isPaintChar: true);
				}
			}
		}
		int num = 2;
		int anchor = 24;
		int anchor2 = StaticObj.TOP_RIGHT;
		int num2 = -1;
		if (cdir == 1)
		{
			num = 0;
			anchor = 0;
			anchor2 = 0;
			num2 = 1;
		}
		if (statusMe == 14)
		{
			if (GameCanvas.gameTick % 4 > 0)
			{
				g.drawImage(ItemMap.imageFlare, cx, cy - ch - 11, mGraphics.HCENTER | mGraphics.VCENTER);
			}
			int num3 = 0;
			if (head == 89 || head == 457 || head == 460 || head == 461 || head == 462 || head == 463 || head == 464 || head == 465 || head == 466)
			{
				num3 = 15;
			}
			SmallImage.drawSmallImage(g, 834, cx, cy - CharInfo[cf][2][2] + part3.pi[CharInfo[cf][2][0]].dy - 2 + num3, num, StaticObj.TOP_CENTER);
			SmallImage.drawSmallImage(g, 79, cx, cy - ch - 8, 0, mGraphics.HCENTER | mGraphics.BOTTOM);
			if (isHead_2Fr(head))
			{
				Part part4 = GameScr.parts[getFHead(head)];
				SmallImage.drawSmallImage(g, part4.pi[CharInfo[cf][0][0]].id, cx + (CharInfo[cf][0][1] + part4.pi[CharInfo[cf][0][0]].dx) * num2, cy - CharInfo[cf][0][2] + part4.pi[CharInfo[cf][0][0]].dy, num, anchor);
			}
			else
			{
				SmallImage.drawSmallImage(g, part.pi[CharInfo[cf][0][0]].id, cx + (CharInfo[cf][0][1] + part.pi[CharInfo[cf][0][0]].dx) * num2, cy - CharInfo[cf][0][2] + part.pi[CharInfo[cf][0][0]].dy, num, anchor);
			}
			paintRedEye(g, cx + (CharInfo[cf][0][1] + part.pi[CharInfo[cf][0][0]].dx) * num2, cy - CharInfo[cf][0][2] + part.pi[CharInfo[cf][0][0]].dy, num, anchor);
		}
		else
		{
			if (isHead_2Fr(head))
			{
				Part part5 = GameScr.parts[getFHead(head)];
				SmallImage.drawSmallImage(g, part5.pi[CharInfo[cf][0][0]].id, cx + (CharInfo[cf][0][1] + part5.pi[CharInfo[cf][0][0]].dx) * num2, cy - CharInfo[cf][0][2] + part5.pi[CharInfo[cf][0][0]].dy, num, anchor);
			}
			else
			{
				SmallImage.drawSmallImage(g, part.pi[CharInfo[cf][0][0]].id, cx + (CharInfo[cf][0][1] + part.pi[CharInfo[cf][0][0]].dx) * num2, cy - CharInfo[cf][0][2] + part.pi[CharInfo[cf][0][0]].dy, num, anchor);
			}
			SmallImage.drawSmallImage(g, part2.pi[CharInfo[cf][1][0]].id, cx + (CharInfo[cf][1][1] + part2.pi[CharInfo[cf][1][0]].dx) * num2, cy - CharInfo[cf][1][2] + part2.pi[CharInfo[cf][1][0]].dy, num, anchor);
			SmallImage.drawSmallImage(g, part3.pi[CharInfo[cf][2][0]].id, cx + (CharInfo[cf][2][1] + part3.pi[CharInfo[cf][2][0]].dx) * num2, cy - CharInfo[cf][2][2] + part3.pi[CharInfo[cf][2][0]].dy, num, anchor);
			paintRedEye(g, cx + (CharInfo[cf][0][1] + part.pi[CharInfo[cf][0][0]].dx) * num2, cy - CharInfo[cf][0][2] + part.pi[CharInfo[cf][0][0]].dy, num, anchor);
		}
		ch = ((isMonkey != 1 && !isFusion) ? (CharInfo[0][0][2] + part.pi[CharInfo[0][0][0]].dy + 10) : 60);
		if (statusMe == 1 && charID > 0 && !isMask && !isUseChargeSkill() && !isWaitMonkey && skillPaint == null && cf != 23 && bag < 0 && ((GameCanvas.gameTick + charID) % 30 == 0 || isFreez))
		{
			g.drawImage((cgender != 1) ? eyeTraiDat : eyeNamek, cx + -((cgender != 1) ? 2 : 2) * num2, cy - 32 + ((cgender != 1) ? 11 : 10) - cf, anchor2);
		}
		if (eProtect != null)
		{
			eProtect.paint(g);
		}
		paintPKFlag(g);
	}

	public void paintCharWithSkill(mGraphics g)
	{
		ty = 0;
		SkillInfoPaint[] array = skillInfoPaint();
		cf = array[indexSkill].status;
		paintCharWithoutSkill(g);
		if (cdir == 1)
		{
			if (eff0 != null)
			{
				if (dx0 == 0)
				{
					dx0 = array[indexSkill].e0dx;
				}
				if (dy0 == 0)
				{
					dy0 = array[indexSkill].e0dy;
				}
				SmallImage.drawSmallImage(g, eff0.arrEfInfo[i0].idImg, cx + dx0 + eff0.arrEfInfo[i0].dx, cy + dy0 + eff0.arrEfInfo[i0].dy, 0, mGraphics.VCENTER | mGraphics.HCENTER);
				i0++;
				if (i0 >= eff0.arrEfInfo.Length)
				{
					eff0 = null;
					i0 = (dx0 = (dy0 = 0));
				}
			}
			if (eff1 != null)
			{
				if (dx1 == 0)
				{
					dx1 = array[indexSkill].e1dx;
				}
				if (dy1 == 0)
				{
					dy1 = array[indexSkill].e1dy;
				}
				SmallImage.drawSmallImage(g, eff1.arrEfInfo[i1].idImg, cx + dx1 + eff1.arrEfInfo[i1].dx, cy + dy1 + eff1.arrEfInfo[i1].dy, 0, mGraphics.VCENTER | mGraphics.HCENTER);
				i1++;
				if (i1 >= eff1.arrEfInfo.Length)
				{
					eff1 = null;
					i1 = (dx1 = (dy1 = 0));
				}
			}
			if (eff2 != null)
			{
				if (dx2 == 0)
				{
					dx2 = array[indexSkill].e2dx;
				}
				if (dy2 == 0)
				{
					dy2 = array[indexSkill].e2dy;
				}
				SmallImage.drawSmallImage(g, eff2.arrEfInfo[i2].idImg, cx + dx2 + eff2.arrEfInfo[i2].dx, cy + dy2 + eff2.arrEfInfo[i2].dy, 0, mGraphics.VCENTER | mGraphics.HCENTER);
				i2++;
				if (i2 >= eff2.arrEfInfo.Length)
				{
					eff2 = null;
					i2 = (dx2 = (dy2 = 0));
				}
			}
		}
		else
		{
			if (eff0 != null)
			{
				if (dx0 == 0)
				{
					dx0 = array[indexSkill].e0dx;
				}
				if (dy0 == 0)
				{
					dy0 = array[indexSkill].e0dy;
				}
				SmallImage.drawSmallImage(g, eff0.arrEfInfo[i0].idImg, cx - dx0 - eff0.arrEfInfo[i0].dx, cy + dy0 + eff0.arrEfInfo[i0].dy, 2, mGraphics.VCENTER | mGraphics.HCENTER);
				i0++;
				if (i0 >= eff0.arrEfInfo.Length)
				{
					eff0 = null;
					i0 = 0;
					dx0 = 0;
					dy0 = 0;
				}
			}
			if (eff1 != null)
			{
				if (dx1 == 0)
				{
					dx1 = array[indexSkill].e1dx;
				}
				if (dy1 == 0)
				{
					dy1 = array[indexSkill].e1dy;
				}
				SmallImage.drawSmallImage(g, eff1.arrEfInfo[i1].idImg, cx - dx1 - eff1.arrEfInfo[i1].dx, cy + dy1 + eff1.arrEfInfo[i1].dy, 2, mGraphics.VCENTER | mGraphics.HCENTER);
				i1++;
				if (i1 >= eff1.arrEfInfo.Length)
				{
					eff1 = null;
					i1 = 0;
					dx1 = 0;
					dy1 = 0;
				}
			}
			if (eff2 != null)
			{
				if (dx2 == 0)
				{
					dx2 = array[indexSkill].e2dx;
				}
				if (dy2 == 0)
				{
					dy2 = array[indexSkill].e2dy;
				}
				SmallImage.drawSmallImage(g, eff2.arrEfInfo[i2].idImg, cx - dx2 - eff2.arrEfInfo[i2].dx, cy + dy2 + eff2.arrEfInfo[i2].dy, 2, mGraphics.VCENTER | mGraphics.HCENTER);
				i2++;
				if (i2 >= eff2.arrEfInfo.Length)
				{
					eff2 = null;
					i2 = 0;
					dx2 = 0;
					dy2 = 0;
				}
			}
		}
		indexSkill++;
	}

	public static int getIndexChar(int ID)
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(i);
			if (@char.charID == ID)
			{
				return i;
			}
		}
		return -1;
	}

	public void moveTo(int toX, int toY, int type)
	{
		if (type == 1 || Res.abs(toX - cx) > 100 || Res.abs(toY - cy) > 300)
		{
			createShadow(cx, cy, 10);
			cx = toX;
			cy = toY;
			vMovePoints.removeAllElements();
			statusMe = 6;
			cp3 = 0;
			currentMovePoint = null;
			cf = 25;
			if (cName == "Tàu Pảy Pảy") AutoT77.isT77TeleToGround = true;	//t77 tele xuống đất
            return;
		}
		int dir = 0;
		int act = 0;
		int num = toX - cx;
		int num2 = toY - cy;
		if (num == 0 && num2 == 0)
		{
			act = 1;
			cp3 = 0;
		}
		else if (num2 == 0)
		{
			act = 2;
			if (num > 0)
			{
				dir = 1;
			}
			if (num < 0)
			{
				dir = -1;
			}
		}
		else if (num2 != 0)
		{
			if (num2 < 0)
			{
				act = 3;
			}
			if (num2 > 0)
			{
				act = 4;
			}
			if (num < 0)
			{
				dir = -1;
			}
			if (num > 0)
			{
				dir = 1;
			}
		}
		vMovePoints.addElement(new MovePoint(toX, toY, act, dir));
		if (statusMe != 6)
		{
			statusBeforeNothing = statusMe;
		}
		statusMe = 6;
		cp3 = 0;
	}

	public static void getcharInjure(int cID, int dx, int dy, int HP)
	{
		Char @char = (Char)GameScr.vCharInMap.elementAt(cID);
		if (@char.vMovePoints.size() != 0)
		{
			MovePoint movePoint = (MovePoint)@char.vMovePoints.lastElement();
			int xEnd = movePoint.xEnd + dx;
			int yEnd = movePoint.yEnd + dy;
			Char char2 = (Char)GameScr.vCharInMap.elementAt(cID);
			char2.cHP -= HP;
			if (char2.cHP < 0)
			{
				char2.cHP = 0;
			}
			char2.cHPShow = ((Char)GameScr.vCharInMap.elementAt(cID)).cHP - HP;
			char2.statusMe = 6;
			char2.cp3 = 0;
			char2.vMovePoints.addElement(new MovePoint(xEnd, yEnd, 8, char2.cdir));
		}
	}

	public bool isMagicTree()
	{
		if (GameScr.gI().magicTree != null)
		{
			int x = GameScr.gI().magicTree.x;
			int y = GameScr.gI().magicTree.y;
			if (cx > x - 30 && cx < x + 30 && cy > y - 30 && cy < y + 30)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public void searchItem()
	{
		int[] array = new int[4] { -1, -1, -1, -1 };
		if (itemFocus != null)
		{
			return;
		}
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			int num = Math.abs(myCharz().cx - itemMap.x);
			int num2 = Math.abs(myCharz().cy - itemMap.y);
			int num3 = ((num <= num2) ? num2 : num);
			if (num > 48 || num2 > 48 || (itemFocus != null && num3 >= array[3]))
			{
				continue;
			}
			if (GameScr.gI().auto != 0 && GameScr.gI().isBagFull())
			{
				if (itemMap.template.type == 9)
				{
					itemFocus = itemMap;
					array[3] = num3;
				}
			}
			else
			{
				itemFocus = itemMap;
				array[3] = num3;
			}
		}
	}

	public void searchFocus()
	{
		if (myCharz().skillPaint != null || myCharz().arr != null || myCharz().dart != null)
		{
			timeFocusToMob = 200;
			return;
		}
		if (timeFocusToMob > 0)
		{
			timeFocusToMob--;
			return;
		}
		if (isManualFocus && charFocus != null && (charFocus.statusMe == 15 || charFocus.isInvisiblez))
		{
			charFocus = null;
		}
		if (GameCanvas.gameTick % 2 == 0 || isMeCanAttackOtherPlayer(charFocus))
		{
			return;
		}
		int num = 0;
		if (nClass.classId == 0 || nClass.classId == 1 || nClass.classId == 3 || nClass.classId == 5)
		{
			num = 40;
		}
		int[] array = new int[4] { -1, -1, -1, -1 };
		int num2 = GameScr.cmx - 10;
		int num3 = GameScr.cmx + GameCanvas.w + 10;
		int cmy = GameScr.cmy;
		int num4 = GameScr.cmy + GameCanvas.h - GameScr.cmdBarH + 10;
		if (isManualFocus)
		{
			if ((mobFocus != null && mobFocus.status != 1 && mobFocus.status != 0 && num2 <= mobFocus.x && mobFocus.x <= num3 && cmy <= mobFocus.y && mobFocus.y <= num4) || (npcFocus != null && num2 <= npcFocus.cx && npcFocus.cx <= num3 && cmy <= npcFocus.cy && npcFocus.cy <= num4) || (charFocus != null && num2 <= charFocus.cx && charFocus.cx <= num3 && cmy <= charFocus.cy && charFocus.cy <= num4) || (itemFocus != null && num2 <= itemFocus.x && itemFocus.x <= num3 && cmy <= itemFocus.y && itemFocus.y <= num4))
			{
				return;
			}
			isManualFocus = false;
		}
		num2 = myCharz().cx - 80;
		num3 = myCharz().cx + 80;
		cmy = myCharz().cy - 30;
		num4 = myCharz().cy + 30;
		if (npcFocus != null && npcFocus.template.npcTemplateId == 6)
		{
			num2 = myCharz().cx - 20;
			num3 = myCharz().cx + 20;
			cmy = myCharz().cy - 10;
			num4 = myCharz().cy + 10;
		}
		if (npcFocus == null)
		{
			for (int i = 0; i < GameScr.vNpc.size(); i++)
			{
				Npc npc = (Npc)GameScr.vNpc.elementAt(i);
				if (npc.statusMe != 15)
				{
					int num5 = Math.abs(myCharz().cx - npc.cx);
					int num6 = Math.abs(myCharz().cy - npc.cy);
					int num7 = ((num5 <= num6) ? num6 : num5);
					num2 = myCharz().cx - 80;
					num3 = myCharz().cx + 80;
					cmy = myCharz().cy - 30;
					num4 = myCharz().cy + 30;
					if (npc.template.npcTemplateId == 6)
					{
						num2 = myCharz().cx - 20;
						num3 = myCharz().cx + 20;
						cmy = myCharz().cy - 10;
						num4 = myCharz().cy + 10;
					}
					if (num2 <= npc.cx && npc.cx <= num3 && cmy <= npc.cy && npc.cy <= num4 && (npcFocus == null || num7 < array[1]))
					{
						npcFocus = npc;
						array[1] = num7;
					}
				}
			}
		}
		else
		{
			if (num2 <= npcFocus.cx && npcFocus.cx <= num3 && cmy <= npcFocus.cy && npcFocus.cy <= num4)
			{
				clearFocus(1);
				return;
			}
			deFocusNPC();
			for (int j = 0; j < GameScr.vNpc.size(); j++)
			{
				Npc npc2 = (Npc)GameScr.vNpc.elementAt(j);
				if (npc2.statusMe != 15)
				{
					int num8 = Math.abs(myCharz().cx - npc2.cx);
					int num9 = Math.abs(myCharz().cy - npc2.cy);
					int num10 = ((num8 <= num9) ? num9 : num8);
					num2 = myCharz().cx - 80;
					num3 = myCharz().cx + 80;
					cmy = myCharz().cy - 30;
					num4 = myCharz().cy + 30;
					if (npc2.template.npcTemplateId == 6)
					{
						num2 = myCharz().cx - 20;
						num3 = myCharz().cx + 20;
						cmy = myCharz().cy - 10;
						num4 = myCharz().cy + 10;
					}
					if (num2 <= npc2.cx && npc2.cx <= num3 && cmy <= npc2.cy && npc2.cy <= num4 && (npcFocus == null || num10 < array[1]))
					{
						npcFocus = npc2;
						array[1] = num10;
					}
				}
			}
		}
		if (itemFocus == null)
		{
			for (int k = 0; k < GameScr.vItemMap.size(); k++)
			{
				ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(k);
				int num11 = Math.abs(myCharz().cx - itemMap.x);
				int num12 = Math.abs(myCharz().cy - itemMap.y);
				int num13 = ((num11 <= num12) ? num12 : num11);
				if (num11 > 48 || num12 > 48 || (itemFocus != null && num13 >= array[3]))
				{
					continue;
				}
				if (GameScr.gI().auto != 0 && GameScr.gI().isBagFull())
				{
					if (itemMap.template.type == 9)
					{
						itemFocus = itemMap;
						array[3] = num13;
					}
				}
				else
				{
					itemFocus = itemMap;
					array[3] = num13;
				}
			}
		}
		else
		{
			if (num2 <= itemFocus.x && itemFocus.x <= num3 && cmy <= itemFocus.y && itemFocus.y <= num4)
			{
				clearFocus(3);
				return;
			}
			itemFocus = null;
			for (int l = 0; l < GameScr.vItemMap.size(); l++)
			{
				ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(l);
				int num14 = Math.abs(myCharz().cx - itemMap2.x);
				int num15 = Math.abs(myCharz().cy - itemMap2.y);
				int num16 = ((num14 <= num15) ? num15 : num14);
				if (num2 > itemMap2.x || itemMap2.x > num3 || cmy > itemMap2.y || itemMap2.y > num4 || (itemFocus != null && num16 >= array[3]))
				{
					continue;
				}
				if (GameScr.gI().auto != 0 && GameScr.gI().isBagFull())
				{
					if (itemMap2.template.type == 9)
					{
						itemFocus = itemMap2;
						array[3] = num16;
					}
				}
				else
				{
					itemFocus = itemMap2;
					array[3] = num16;
				}
			}
		}
		num2 = myCharz().cx - myCharz().getdxSkill() - 10;
		num3 = myCharz().cx + myCharz().getdxSkill() + 10;
		cmy = myCharz().cy - myCharz().getdySkill() - num - 20;
		num4 = myCharz().cy + myCharz().getdySkill() + 20;
		if (num4 > myCharz().cy + 30)
		{
			num4 = myCharz().cy + 30;
		}
		if (mobFocus == null)
		{
			for (int m = 0; m < GameScr.vMob.size(); m++)
			{
				Mob mob = (Mob)GameScr.vMob.elementAt(m);
				int num17 = Math.abs(myCharz().cx - mob.x);
				int num18 = Math.abs(myCharz().cy - mob.y);
				int num19 = ((num17 <= num18) ? num18 : num17);
				if (num2 <= mob.x && mob.x <= num3 && cmy <= mob.y && mob.y <= num4 && (mobFocus == null || num19 < array[0]))
				{
					mobFocus = mob;
					array[0] = num19;
				}
			}
		}
		else
		{
			if (mobFocus.status != 1 && mobFocus.status != 0 && num2 <= mobFocus.x && mobFocus.x <= num3 && cmy <= mobFocus.y && mobFocus.y <= num4)
			{
				clearFocus(0);
				return;
			}
			mobFocus = null;
			for (int n = 0; n < GameScr.vMob.size(); n++)
			{
				Mob mob2 = (Mob)GameScr.vMob.elementAt(n);
				int num20 = Math.abs(myCharz().cx - mob2.x);
				int num21 = Math.abs(myCharz().cy - mob2.y);
				int num22 = ((num20 <= num21) ? num21 : num20);
				if (num2 <= mob2.x && mob2.x <= num3 && cmy <= mob2.y && mob2.y <= num4 && (mobFocus == null || num22 < array[0]))
				{
					mobFocus = mob2;
					array[0] = num22;
				}
			}
		}
		if (charFocus == null)
		{
			for (int num23 = 0; num23 < GameScr.vCharInMap.size(); num23++)
			{
				Char @char = (Char)GameScr.vCharInMap.elementAt(num23);
				if (@char.statusMe != 15 && !@char.isInvisiblez && wdx == 0 && wdy == 0)
				{
					int num24 = Math.abs(myCharz().cx - @char.cx);
					int num25 = Math.abs(myCharz().cy - @char.cy);
					int num26 = ((num24 <= num25) ? num25 : num24);
					if (num2 <= @char.cx && @char.cx <= num3 && cmy <= @char.cy && @char.cy <= num4 && (charFocus == null || num26 < array[2]))
					{
						charFocus = @char;
						array[2] = num26;
					}
				}
			}
		}
		else
		{
			if (num2 <= charFocus.cx && charFocus.cx <= num3 && cmy <= charFocus.cy && charFocus.cy <= num4 && charFocus.statusMe != 15 && !charFocus.isInvisiblez)
			{
				clearFocus(2);
				return;
			}
			//Không xóa charFocus khi khoảng cách quá xa
			//charFocus = null;
			for (int num27 = 0; num27 < GameScr.vCharInMap.size(); num27++)
			{
				Char char2 = (Char)GameScr.vCharInMap.elementAt(num27);
				if (char2.statusMe != 15 && !char2.isInvisiblez && wdx == 0 && wdy == 0)
				{
					int num28 = Math.abs(myCharz().cx - char2.cx);
					int num29 = Math.abs(myCharz().cy - char2.cy);
					int num30 = ((num28 <= num29) ? num29 : num28);
					if (num2 <= char2.cx && char2.cx <= num3 && cmy <= char2.cy && char2.cy <= num4 && (charFocus == null || num30 < array[2]))
					{
						charFocus = char2;
						array[2] = num30;
					}
				}
			}
		}
		int num31 = -1;
		for (int num32 = 0; num32 < array.Length; num32++)
		{
			if (num31 == -1)
			{
				if (array[num32] != -1)
				{
					num31 = num32;
				}
			}
			else if (array[num32] < array[num31] && array[num32] != -1)
			{
				num31 = num32;
			}
		}
		clearFocus(num31);
		if (me && isAttacPlayerStatus())
		{
			if (mobFocus != null && !mobFocus.isMobMe)
			{
				mobFocus = null;
			}
			npcFocus = null;
			itemFocus = null;
		}
	}

	public void clearFocus(int index)
	{
		switch (index)
		{
		case 0:
			deFocusNPC();
			//charFocus = null;
			itemFocus = null;
			break;
		case 1:
			mobFocus = null;
			//charFocus = null;
			itemFocus = null;
			break;
		case 2:
			mobFocus = null;
			deFocusNPC();
			itemFocus = null;
			break;
		case 3:
			mobFocus = null;
			deFocusNPC();
			//charFocus = null;
			break;
		}
	}

	public static bool isCharInScreen(Char c)
	{
		int cmx = GameScr.cmx;
		int num = GameScr.cmx + GameCanvas.w;
		int num2 = GameScr.cmy + 10;
		int num3 = GameScr.cmy + GameScr.gH;
		if (c.statusMe != 15 && !c.isInvisiblez && cmx <= c.cx && c.cx <= num && num2 <= c.cy && c.cy <= num3)
		{
			return true;
		}
		return false;
	}

	public bool isAttacPlayerStatus()
	{
		return cTypePk == 4 || cTypePk == 3;
	}

	public void setHoldChar(Char r)
	{
		if (cx < r.cx)
		{
			cdir = 1;
		}
		else
		{
			cdir = -1;
		}
		charHold = r;
		holder = true;
		r.charEffectTime.isTied = true;
		r.charEffectTime.timeTied = CharExtensions.getTimeHold(r);
    }

	public void setHoldMob(Mob r)
	{
		if (cx < r.x)
		{
			cdir = 1;
		}
		else
		{
			cdir = -1;
		}
		mobHold = r;
		holder = true;
    }

    public void findNextFocusByKey()
	{
		Res.outz("focus size= " + focus.size());
		if ((myCharz().skillPaint != null || myCharz().arr != null || myCharz().dart != null || myCharz().skillInfoPaint() != null) && focus.size() == 0)
		{
			return;
		}
		focus.removeAllElements();
		int num = 0;
		int num2 = GameScr.cmx + 10;
		int num3 = GameScr.cmx + GameCanvas.w - 10;
		int num4 = GameScr.cmy + 10;
		int num5 = GameScr.cmy + GameScr.gH;
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char @char = (Char)GameScr.vCharInMap.elementAt(i);
			if (@char.statusMe != 15 && !@char.isInvisiblez && num2 <= @char.cx && @char.cx <= num3 && num4 <= @char.cy && @char.cy <= num5 && @char.charID != -114 && (TileMap.mapID != 129 || (TileMap.mapID == 129 && myCharz().cy > 264)))
			{
				focus.addElement(@char);
				if (charFocus != null && @char.Equals(charFocus))
				{
					num = focus.size();
				}
			}
		}
		if (me && isAttacPlayerStatus())
		{
			Res.outz("co the tan cong nguoi");
			for (int j = 0; j < GameScr.vMob.size(); j++)
			{
				Mob mob = (Mob)GameScr.vMob.elementAt(j);
				if (!GameScr.gI().isMeCanAttackMob(mob))
				{
					Res.outz("khong the tan cong quai");
					mobFocus = null;
					continue;
				}
				Res.outz("co the tan ong quai");
				focus.addElement(mob);
				if (mobFocus != null)
				{
					num = focus.size();
				}
			}
			npcFocus = null;
			itemFocus = null;
			if (focus.size() > 0)
			{
				if (num >= focus.size())
				{
					num = 0;
				}
				focusManualTo(focus.elementAt(num));
			}
			else
			{
				mobFocus = null;
				deFocusNPC();
				charFocus = null;
				itemFocus = null;
				isManualFocus = false;
			}
			return;
		}
		for (int k = 0; k < GameScr.vItemMap.size(); k++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(k);
			if (num2 <= itemMap.x && itemMap.x <= num3 && num4 <= itemMap.y && itemMap.y <= num5)
			{
				focus.addElement(itemMap);
				if (itemFocus != null && itemMap.Equals(itemFocus))
				{
					num = focus.size();
				}
			}
		}
		for (int l = 0; l < GameScr.vMob.size(); l++)
		{
			Mob mob2 = (Mob)GameScr.vMob.elementAt(l);
			if (mob2.status != 1 && mob2.status != 0 && num2 <= mob2.x && mob2.x <= num3 && num4 <= mob2.y && mob2.y <= num5)
			{
				focus.addElement(mob2);
				if (mobFocus != null && mob2.Equals(mobFocus))
				{
					num = focus.size();
				}
			}
		}
		for (int m = 0; m < GameScr.vNpc.size(); m++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(m);
			if (npc.statusMe != 15 && num2 <= npc.cx && npc.cx <= num3 && num4 <= npc.cy && npc.cy <= num5)
			{
				focus.addElement(npc);
				if (npcFocus != null && npc.Equals(npcFocus))
				{
					num = focus.size();
				}
			}
		}
		if (focus.size() > 0)
		{
			if (num >= focus.size())
			{
				num = 0;
			}
			focusManualTo(focus.elementAt(num));
		}
		else
		{
			mobFocus = null;
			deFocusNPC();
			charFocus = null;
			itemFocus = null;
			isManualFocus = false;
		}
	}

	public void deFocusNPC()
	{
		if (me && npcFocus != null)
		{
			if (!GameCanvas.menu.showMenu)
			{
				chatPopup = null;
			}
			npcFocus = null;
		}
	}

	public void updateCharInBridge()
	{
		if (!GameCanvas.lowGraphic)
		{
			if (TileMap.tileTypeAt(cx, cy + 1, 1024))
			{
				TileMap.setTileTypeAtPixel(cx, cy + 1, 512);
				TileMap.setTileTypeAtPixel(cx, cy - 2, 512);
			}
			if (TileMap.tileTypeAt(cx - TileMap.size, cy + 1, 512))
			{
				TileMap.killTileTypeAt(cx - TileMap.size, cy + 1, 512);
				TileMap.killTileTypeAt(cx - TileMap.size, cy - 2, 512);
			}
			if (TileMap.tileTypeAt(cx + TileMap.size, cy + 1, 512))
			{
				TileMap.killTileTypeAt(cx + TileMap.size, cy + 1, 512);
				TileMap.killTileTypeAt(cx + TileMap.size, cy - 2, 512);
			}
		}
	}

	public static void sort(int[] data)
	{
		int num = 5;
		for (int i = 0; i < num - 1; i++)
		{
			for (int j = i + 1; j < num; j++)
			{
				if (data[i] < data[j])
				{
					int num2 = data[j];
					data[j] = data[i];
					data[i] = num2;
				}
			}
		}
	}

	public static bool setInsc(int cmX, int cmWx, int x, int cmy, int cmyH, int y)
	{
		if (x > cmWx || x < cmX || y > cmyH || y < cmy)
		{
			return false;
		}
		return true;
	}

	public void kickOption(Item item, int maxKick)
	{
		int num = 0;
		if (item == null || item.options == null)
		{
			return;
		}
		for (int i = 0; i < item.options.size(); i++)
		{
			ItemOption itemOption = (ItemOption)item.options.elementAt(i);
			itemOption.active = 0;
			if (itemOption.optionTemplate.type == 2)
			{
				if (num < maxKick)
				{
					itemOption.active = 1;
					num++;
				}
			}
			else if (itemOption.optionTemplate.type == 3 && item.upgrade >= 4)
			{
				itemOption.active = 1;
			}
			else if (itemOption.optionTemplate.type == 4 && item.upgrade >= 8)
			{
				itemOption.active = 1;
			}
			else if (itemOption.optionTemplate.type == 5 && item.upgrade >= 12)
			{
				itemOption.active = 1;
			}
			else if (itemOption.optionTemplate.type == 6 && item.upgrade >= 14)
			{
				itemOption.active = 1;
			}
			else if (itemOption.optionTemplate.type == 7 && item.upgrade >= 16)
			{
				itemOption.active = 1;
			}
		}
	}

	public void doInjure(int HPShow, int MPShow, bool isCrit, bool isMob)
	{
		this.isCrit = isCrit;
		this.isMob = isMob;
		Res.outz("CHP= " + cHP + " dame -= " + HPShow + " HP FULL= " + cHPFull);
		cHP -= HPShow;
		cMP -= MPShow;
		GameScr.gI().isInjureHp = true;
		GameScr.gI().twHp = 0;
		GameScr.gI().isInjureMp = true;
		GameScr.gI().twMp = 0;
		if (cHP < 0)
		{
			cHP = 0;
		}
		if (cMP < 0)
		{
			cMP = 0;
		}
		if (isMob || (!isMob && cTypePk != 4 && damMP != -100))
		{
			if (HPShow <= 0)
			{
				if (me)
				{
					GameScr.startFlyText(mResources.miss, cx, cy - ch, 0, -2, mFont.MISS_ME);
				}
				else
				{
					GameScr.startFlyText(mResources.miss, cx, cy - ch, 0, -2, mFont.MISS);
				}
			}
			else
			{
				GameScr.startFlyText("-" + HPShow, cx, cy - ch, 0, -2, isCrit ? mFont.FATAL : mFont.RED);
			}
		}
		if (HPShow > 0)
		{
			isInjure = 6;
		}
		ServerEffect.addServerEffect(80, this, 1);
		if (isDie)
		{
			isDie = false;
			isLockKey = false;
			startDie((short)xSd, (short)ySd);
		}
	}

	public void doInjure()
	{
		GameScr.gI().isInjureHp = true;
		GameScr.gI().twHp = 0;
		GameScr.gI().isInjureMp = true;
		GameScr.gI().twMp = 0;
		isInjure = 6;
		ServerEffect.addServerEffect(8, this, 1);
		isInjureHp = true;
		twHp = 0;
	}

	public void startDie(short toX, short toY)
	{
		isMonkey = 0;
		isWaitMonkey = false;
		if (me && isDie)
		{
			return;
		}
		if (me)
		{
			isLockMove = true;
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char @char = (Char)GameScr.vCharInMap.elementAt(i);
				@char.killCharId = -9999;
			}
			if (GameCanvas.panel != null && GameCanvas.panel.cp != null)
			{
				GameCanvas.panel.cp = null;
			}
			if (GameCanvas.panel2 != null && GameCanvas.panel2.cp != null)
			{
				GameCanvas.panel2.cp = null;
			}
		}
		statusMe = 5;
		cp2 = toX;
		cp3 = toY;
		cp1 = 0;
		cHP = 0;
		testCharId = -9999;
		killCharId = -9999;
		if (me && myskill != null && myskill.template.id != 14)
		{
			stopUseChargeSkill();
		}
		cTypePk = 0;
	}

	public void waitToDie(short toX, short toY)
	{
		wdx = toX;
		wdy = toY;
	}

	public void liveFromDead()
	{
		cHP = cHPFull;
		cMP = cMPFull;
		statusMe = 1;
		cp1 = (cp2 = (cp3 = 0));
		ServerEffect.addServerEffect(109, this, 2);
		GameScr.gI().center = null;
		GameScr.isHaveSelectSkill = true;
	}

	public bool doUsePotion()
	{
		if (arrItemBag == null)
		{
			return false;
		}
		for (int i = 0; i < arrItemBag.Length; i++)
		{
			if (arrItemBag[i] != null && arrItemBag[i].template.type == 6)
			{
				Service.gI().useItem(0, 1, -1, arrItemBag[i].template.id);
				return true;
			}
		}
		return false;
	}

	public bool isLang()
	{
		if (TileMap.mapID == 1 || TileMap.mapID == 27 || TileMap.mapID == 72 || TileMap.mapID == 10 || TileMap.mapID == 17 || TileMap.mapID == 22 || TileMap.mapID == 32 || TileMap.mapID == 38 || TileMap.mapID == 43 || TileMap.mapID == 48)
		{
			return true;
		}
		return false;
	}

	public bool isMeCanAttackOtherPlayer(Char cAtt)
	{
		if (cAtt == null || myCharz().myskill == null || myCharz().myskill.template.type == 2 || (myCharz().myskill.template.type == 4 && cAtt.statusMe != 14 && cAtt.statusMe != 5))
		{
			return false;
		}
		return ((cAtt.cTypePk == 3 && myCharz().cTypePk == 3) || myCharz().cTypePk == 5 || cAtt.cTypePk == 5 || (myCharz().cTypePk == 1 && cAtt.cTypePk == 1) || (myCharz().cTypePk == 4 && cAtt.cTypePk == 4) || (myCharz().testCharId >= 0 && myCharz().testCharId == cAtt.charID) || (myCharz().killCharId >= 0 && myCharz().killCharId == cAtt.charID && !isLang()) || (cAtt.killCharId >= 0 && cAtt.killCharId == myCharz().charID && !isLang()) || (myCharz().cFlag == 8 && cAtt.cFlag != 0) || (myCharz().cFlag != 0 && cAtt.cFlag == 8) || (myCharz().cFlag != cAtt.cFlag && myCharz().cFlag != 0 && cAtt.cFlag != 0)) && cAtt.statusMe != 14 && cAtt.statusMe != 5;
	}

	public void clearTask()
	{
		myCharz().taskMaint = null;
		for (int i = 0; i < myCharz().arrItemBag.Length; i++)
		{
			if (myCharz().arrItemBag[i] != null && myCharz().arrItemBag[i].template.type == 8)
			{
				myCharz().arrItemBag[i] = null;
			}
		}
		Npc.clearEffTask();
	}

	public int getX()
	{
		return cx;
	}

	public int getY()
	{
		return cy;
	}

	public int getH()
	{
		return 32;
	}

	public int getW()
	{
		return 24;
	}

	public void focusManualTo(object objectz)
	{
		if (objectz is Mob)
		{
			mobFocus = (Mob)objectz;
			deFocusNPC();
			charFocus = null;
			itemFocus = null;
		}
		else if (objectz is Npc)
		{
			myCharz().mobFocus = null;
			myCharz().deFocusNPC();
			myCharz().npcFocus = (Npc)objectz;
			myCharz().charFocus = null;
			myCharz().itemFocus = null;
		}
		else if (objectz is Char)
		{
			myCharz().mobFocus = null;
			myCharz().deFocusNPC();
			myCharz().charFocus = (Char)objectz;
			myCharz().itemFocus = null;
		}
		else if (objectz is ItemMap)
		{
			myCharz().mobFocus = null;
			myCharz().deFocusNPC();
			myCharz().charFocus = null;
			myCharz().itemFocus = (ItemMap)objectz;
		}
		isManualFocus = true;
	}

	public void stopMoving()
	{
	}

	public void cancelAttack()
	{
	}

	public bool isInvisible()
	{
		return false;
	}

	public bool focusToAttack()
	{
		return mobFocus != null || (charFocus != null && isMeCanAttackOtherPlayer(charFocus));
	}

	public void addDustEff(int type)
	{
		if (GameCanvas.lowGraphic)
		{
			return;
		}
		switch (type)
		{
		case 1:
			if (clevel >= 9)
			{
				Effect effect3 = new Effect(19, cx - 5, cy + 20, 2, 1, -1);
				EffecMn.addEff(effect3);
			}
			break;
		case 2:
			if ((!me || isMonkey != 1) && isNhapThe && GameCanvas.gameTick % 5 == 0)
			{
				Effect effect2 = new Effect(22, cx - 5, cy + 35, 2, 1, -1);
				EffecMn.addEff(effect2);
			}
			break;
		case 3:
			if (clevel >= 9 && ySd - cy <= 5)
			{
				Effect effect = new Effect(19, cx - 5, ySd + 20, 2, 1, -1);
				EffecMn.addEff(effect);
			}
			break;
		}
	}

	public bool isGetFlagImage(sbyte getFlag)
	{
		bool result = true;
		for (int i = 0; i < GameScr.vFlag.size(); i++)
		{
			PKFlag pKFlag = (PKFlag)GameScr.vFlag.elementAt(i);
			if (pKFlag != null)
			{
				if (pKFlag.cflag == getFlag)
				{
					return true;
				}
				result = false;
			}
		}
		return result;
	}

	private void paintPKFlag(mGraphics g)
	{
		if (cdir == 1)
		{
			if (cFlag != 0 && cFlag != -1)
			{
				SmallImage.drawSmallImage(g, flagImage, cx - 10, cy - ch - ((!me) ? 30 : 30) + ((GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), 2, 0);
			}
		}
		else if (cFlag != 0 && cFlag != -1)
		{
			SmallImage.drawSmallImage(g, flagImage, cx, cy - ch - ((!me) ? 30 : 30) + ((GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), 0, 0);
		}
	}

	public void removeHoleEff()
	{
		if (holder)
		{
			holder = false;
			charHold = null;
			mobHold = null;
		}
		else
		{
			holdEffID = 0;
			charHold = null;
			mobHold = null;
		}
		charEffectTime.isTied = false;
		charEffectTime.isTiedByMe = false;
	}

	public void removeProtectEff()
	{
		protectEff = false;
		eProtect = null;
	}

	public void removeBlindEff()
	{
		blindEff = false;
	}

	public void removeEffect()
	{
		if (holdEffID != 0)
		{
			holdEffID = 0;
		}
		if (holder)
		{
			holder = false;
			charEffectTime.isTied = false;
			charEffectTime.isTiedByMe = false;
		}
		if (protectEff)
		{
			protectEff = false;
			charEffectTime.hasShield = false;
		}
		eProtect = null;
		charHold = null;
		mobHold = null;
		blindEff = false;
		sleepEff = false;
	}

	public void setPos(short xPos, short yPos, sbyte typePos)
	{
		isSetPos = true;
		this.xPos = xPos;
		this.yPos = yPos;
		this.typePos = typePos;
		tpos = 0;
		if (me)
		{
			if (GameCanvas.panel != null)
			{
				GameCanvas.panel.hide();
			}
			if (GameCanvas.panel2 != null)
			{
				GameCanvas.panel2.hide();
			}
		}
	}

	public void removeHuytSao()
	{
		huytSao = false;
	}

	public void fusionComplete()
	{
		isFusion = false;
		isLockKey = false;
		tFusion = 0;
	}

	public void setFusion(sbyte fusion)
	{
		tFusion = 0;
		if (fusion == 4 || fusion == 5)
		{
			if (me)
			{
				Service.gI().funsion(fusion);
			}
			EffecMn.addEff(new Effect(34, cx, cy + 12, 2, 1, -1));
		}
		if (fusion == 6)
		{
			EffecMn.addEff(new Effect(38, cx, cy + 12, 2, 1, -1));
		}
		if (me)
		{
			GameCanvas.panel.hideNow();
			isLockKey = true;
		}
		isFusion = true;
		if (fusion == 1)
		{
			isNhapThe = false;
		}
		else
		{
			isNhapThe = true;
		}
	}

	public void removeSleepEff()
	{
		sleepEff = false;
	}

	public void setPartOld()
	{
		headTemp = head;
		bodyTemp = body;
		legTemp = leg;
		bagTemp = bag;
	}

	public void setPartTemp(int head, int body, int leg, int bag)
	{
		if (head != -1)
		{
			this.head = head;
		}
		if (body != -1)
		{
			this.body = body;
		}
		if (leg != -1)
		{
			this.leg = leg;
		}
		if (bag != -1)
		{
			this.bag = bag;
		}
	}

	public void resetPartTemp()
	{
		if (headTemp != -1)
		{
			head = headTemp;
			headTemp = -1;
		}
		if (bodyTemp != -1)
		{
			body = bodyTemp;
			bodyTemp = -1;
		}
		if (legTemp != -1)
		{
			leg = legTemp;
			legTemp = -1;
		}
		if (bagTemp != -1)
		{
			bag = bagTemp;
			bagTemp = -1;
		}
	}

	public Effect getEffById(int id)
	{
		for (int i = 0; i < vEffChar.size(); i++)
		{
			Effect effect = (Effect)vEffChar.elementAt(i);
			if (effect.effId == id)
			{
				return effect;
			}
		}
		return null;
	}

	public void addEffChar(Effect e)
	{
		removeEffChar(0, e.effId);
		vEffChar.addElement(e);
	}

	public void removeEffChar(int type, int id)
	{
		if (type == -1)
		{
			vEffChar.removeAllElements();
		}
		else if (getEffById(id) != null)
		{
			vEffChar.removeElement(getEffById(id));
		}
	}

	public void paintEffBehind(mGraphics g)
	{
		for (int i = 0; i < vEffChar.size(); i++)
		{
			Effect effect = (Effect)vEffChar.elementAt(i);
			if (effect.layer == 0)
			{
				bool flag = true;
				if (effect.isStand == 0)
				{
					flag = ((statusMe == 1 || statusMe == 6) ? true : false);
				}
				if (flag)
				{
					effect.paint(g);
				}
			}
		}
	}

	public void paintEffFront(mGraphics g)
	{
		for (int i = 0; i < vEffChar.size(); i++)
		{
			Effect effect = (Effect)vEffChar.elementAt(i);
			if (effect.layer == 1)
			{
				bool flag = true;
				if (effect.isStand == 0)
				{
					flag = ((statusMe == 1 || statusMe == 6) ? true : false);
				}
				if (flag)
				{
					effect.paint(g);
				}
			}
		}
	}

	public void updEffChar()
	{
		for (int i = 0; i < vEffChar.size(); i++)
		{
			((Effect)vEffChar.elementAt(i)).update();
		}
	}

	public int checkLuong()
	{
		return luong + luongKhoa;
	}

	public void updateEye()
	{
		if (head != 934)
		{
			return;
		}
		if (GameCanvas.timeNow - timeAddChopmat > 0)
		{
			fChopmat++;
			if (fChopmat > frEye.Length - 1)
			{
				fChopmat = 0;
				timeAddChopmat = GameCanvas.timeNow + Res.random(2000, 3500);
				frEye = frChopCham;
				if (Res.random(2) == 0)
				{
					frEye = frChopNhanh;
				}
			}
		}
		else
		{
			fChopmat = 0;
		}
	}

	private void paintRedEye(mGraphics g, int xx, int yy, int trans, int anchor)
	{
		if (head != 934 || (statusMe != 1 && statusMe != 6))
		{
			return;
		}
		if (fraRedEye == null || fraRedEye.imgFrame == null)
		{
			Image img = mSystem.loadImage("/redeye.png");
			fraRedEye = new FrameImage(img, 14, 10);
		}
		else if (frEye[fChopmat] != -1)
		{
			int num = 8;
			int num2 = 15;
			if (trans == 2)
			{
				num = -8;
			}
			fraRedEye.drawFrame(frEye[fChopmat], xx + num, yy + num2, trans, anchor, g);
		}
	}

	public bool isHead_2Fr(int idHead)
	{
		for (int i = 0; i < Arr_Head_2Fr.Length; i++)
		{
			if (Arr_Head_2Fr[i][0] == idHead)
			{
				return true;
			}
		}
		return false;
	}

	private void updateFHead()
	{
		if (isHead_2Fr(head))
		{
			fHead++;
			if (fHead > 10000)
			{
				fHead = 0;
			}
		}
		else
		{
			fHead = 0;
		}
	}

	private int getFHead(int idHead)
	{
		for (int i = 0; i < Arr_Head_2Fr.Length; i++)
		{
			if (Arr_Head_2Fr[i][0] == idHead)
			{
				return Arr_Head_2Fr[i][fHead / 4 % Arr_Head_2Fr[i].Length];
			}
		}
		return idHead;
	}

	public void paintAuraBehind(mGraphics g)
	{
		if ((!me || isPaintAura) && idAuraEff > -1 && (statusMe == 1 || statusMe == 6) && !GameCanvas.panel.isShow && mSystem.currentTimeMillis() - timeBlue > 0)
		{
			string nameImg = strEffAura + idAuraEff + "_0";
			FrameImage fraImage = mSystem.getFraImage(nameImg);
			fraImage?.drawFrame(GameCanvas.gameTick / 4 % fraImage.nFrame, cx, cy, (cdir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER, g);
		}
	}

	public void paintAuraFront(mGraphics g)
	{
		if ((me && !isPaintAura) || idAuraEff <= -1)
		{
			return;
		}
		if (statusMe == 1 || statusMe == 6)
		{
			if (!GameCanvas.panel.isShow && !GameCanvas.lowGraphic)
			{
				bool flag = false;
				if (mSystem.currentTimeMillis() - timeBlue > -1000 && IsAddDust1)
				{
					flag = true;
					IsAddDust1 = false;
				}
				if (mSystem.currentTimeMillis() - timeBlue > -500 && IsAddDust2)
				{
					flag = true;
					IsAddDust2 = false;
				}
				if (flag)
				{
					GameCanvas.gI().startDust(-1, cx - -8, cy);
					GameCanvas.gI().startDust(1, cx - 8, cy);
					addDustEff(1);
				}
				if (mSystem.currentTimeMillis() - timeBlue > 0)
				{
					string nameImg = strEffAura + idAuraEff + "_1";
					FrameImage fraImage = mSystem.getFraImage(nameImg);
					fraImage?.drawFrame(GameCanvas.gameTick / 4 % fraImage.nFrame, cx, cy + 2, (cdir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER, g);
				}
			}
		}
		else
		{
			timeBlue = mSystem.currentTimeMillis() + 1500;
			IsAddDust1 = true;
			IsAddDust2 = true;
		}
	}

	public void paintEff_Lvup_behind(mGraphics g)
	{
		if (idEff_Set_Item != -1)
		{
			if (fraEff != null)
			{
				fraEff.drawFrame(GameCanvas.gameTick / 4 % fraEff.nFrame, cx, cy + 3, (cdir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER, g);
			}
			else
			{
				fraEff = mSystem.getFraImage(strEff_Set_Item + idEff_Set_Item + "_0");
			}
		}
	}

	public void paintEff_Lvup_front(mGraphics g)
	{
		if (idEff_Set_Item != -1)
		{
			if (fraEffSub != null)
			{
				fraEffSub.drawFrame(GameCanvas.gameTick / 4 % fraEffSub.nFrame, cx, cy + 8, (cdir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER, g);
			}
			else
			{
				fraEffSub = mSystem.getFraImage(strEff_Set_Item + idEff_Set_Item + "_1");
			}
		}
	}
}
