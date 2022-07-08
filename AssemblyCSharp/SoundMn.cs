using System;

public class SoundMn
{
	public class MediaPlayer
	{
	}

	public class SoundPool
	{
	}

	public class AssetManager
	{
	}

	public static SoundMn gIz;

	public static bool isSound = true;

	public static float volume = 0.5f;

	private static int MAX_VOLUME = 10;

	public static MediaPlayer[] music;

	public static SoundPool[] sound;

	public static int[] soundID;

	public static int AIR_SHIP;

	public static int RAIN = 1;

	public static int TAITAONANGLUONG = 2;

	public static int GET_ITEM;

	public static int MOVE = 1;

	public static int LOW_PUNCH = 2;

	public static int LOW_KICK = 3;

	public static int FLY = 4;

	public static int JUMP = 5;

	public static int PANEL_OPEN = 6;

	public static int BUTTON_CLOSE = 7;

	public static int BUTTON_CLICK = 8;

	public static int MEDIUM_PUNCH = 9;

	public static int MEDIUM_KICK = 10;

	public static int PANEL_CLICK = 11;

	public static int EAT_PEAN = 12;

	public static int OPEN_DIALOG = 13;

	public static int NORMAL_KAME = 14;

	public static int NAMEK_KAME = 15;

	public static int XAYDA_KAME = 16;

	public static int EXPLODE_1 = 17;

	public static int EXPLODE_2 = 18;

	public static int TRAIDAT_KAME = 19;

	public static int HP_UP = 20;

	public static int THAIDUONGHASAN = 21;

	public static int HOISINH = 22;

	public static int GONG = 23;

	public static int KHICHAY = 24;

	public static int BIG_EXPLODE = 25;

	public static int NAMEK_LAZER = 26;

	public static int NAMEK_CHARGE = 27;

	public static int RADAR_CLICK = 28;

	public static int RADAR_ITEM = 29;

	public static int FIREWORK = 30;

	public bool freePool;

	public int poolCount;

	public static int cout = 1;

	public static void init(AssetManager ac)
	{
		Sound.setActivity(ac);
	}

	public static SoundMn gI()
	{
		if (gIz == null)
		{
			gIz = new SoundMn();
		}
		return gIz;
	}

	public void loadSound(int mapID)
	{
		Sound.init(new int[3] { AIR_SHIP, RAIN, TAITAONANGLUONG }, new int[31]
		{
			GET_ITEM, MOVE, LOW_PUNCH, LOW_KICK, FLY, JUMP, PANEL_OPEN, BUTTON_CLOSE, BUTTON_CLICK, MEDIUM_PUNCH,
			MEDIUM_KICK, PANEL_OPEN, EAT_PEAN, OPEN_DIALOG, NORMAL_KAME, NAMEK_KAME, XAYDA_KAME, EXPLODE_1, EXPLODE_2, TRAIDAT_KAME,
			HP_UP, THAIDUONGHASAN, HOISINH, GONG, KHICHAY, BIG_EXPLODE, NAMEK_LAZER, NAMEK_CHARGE, RADAR_CLICK, RADAR_ITEM,
			FIREWORK
		});
	}

	public void getSoundOption()
	{
		if (GameCanvas.loginScr.isLogin2 && Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId >= 2)
		{
			Panel.strTool = new string[10]
			{
				mResources.radaCard,
				mResources.quayso,
				mResources.gameInfo,
				mResources.change_flag,
				mResources.change_zone,
				mResources.chat_world,
				mResources.account,
				mResources.option,
				mResources.change_account,
				mResources.REGISTOPROTECT
			};
			if (Char.myCharz().havePet)
			{
				Panel.strTool = new string[11]
				{
					mResources.radaCard,
					mResources.quayso,
					mResources.gameInfo,
					mResources.pet,
					mResources.change_flag,
					mResources.change_zone,
					mResources.chat_world,
					mResources.account,
					mResources.option,
					mResources.change_account,
					mResources.REGISTOPROTECT
				};
			}
		}
		else
		{
			Panel.strTool = new string[9]
			{
				mResources.radaCard,
				mResources.quayso,
				mResources.gameInfo,
				mResources.change_flag,
				mResources.change_zone,
				mResources.chat_world,
				mResources.account,
				mResources.option,
				mResources.change_account
			};
			if (Char.myCharz().havePet)
			{
				Panel.strTool = new string[10]
				{
					mResources.radaCard,
					mResources.quayso,
					mResources.gameInfo,
					mResources.pet,
					mResources.change_flag,
					mResources.change_zone,
					mResources.chat_world,
					mResources.account,
					mResources.option,
					mResources.change_account
				};
			}
		}
	}

	public void getStrOption()
	{
		if (Main.isPC)
		{
			Panel.strCauhinh = new string[4]
			{
				(!Char.isPaintAura) ? mResources.aura_on : mResources.aura_off,
				(!GameCanvas.isPlaySound) ? mResources.turnOnSound : mResources.turnOffSound,
				GameCanvas.lowGraphic ? mResources.cauhinhcao : mResources.cauhinhthap,
				(mGraphics.zoomLevel <= 1) ? mResources.x2Screen : mResources.x1Screen,
			};
		}
		else
		{
			Panel.strCauhinh = new string[3]
			{
				(!Char.isPaintAura) ? mResources.aura_on : mResources.aura_off,
				(!GameCanvas.isPlaySound) ? mResources.turnOnSound : mResources.turnOffSound,
				(GameScr.isAnalog != 0) ? mResources.turnOffAnalog : mResources.turnOnAnalog
			};
		}
	}

	public void HP_MPup()
	{
		Sound.playSound(HP_UP, 0.5f);
	}

	public void charPunch(bool isKick, float volumn)
	{
		if (!Char.myCharz().me)
		{
			volume /= 2f;
		}
		if (volumn <= 0f)
		{
			volumn = 0.01f;
		}
		int num = Res.random(0, 3);
		if (isKick)
		{
			Sound.playSound((num != 0) ? MEDIUM_KICK : LOW_KICK, 0.1f);
		}
		else
		{
			Sound.playSound((num != 0) ? MEDIUM_PUNCH : LOW_PUNCH, 0.1f);
		}
		poolCount++;
	}

	public void thaiduonghasan()
	{
		Sound.playSound(THAIDUONGHASAN, 0.5f);
		poolCount++;
	}

	public void rain()
	{
		Sound.playMus(RAIN, 0.3f, loop: true);
	}

	public void gongName()
	{
		Sound.playSound(NAMEK_CHARGE, 0.3f);
		poolCount++;
	}

	public void gong()
	{
		Sound.playSound(GONG, 0.2f);
		poolCount++;
	}

	public void getItem()
	{
		Sound.playSound(GET_ITEM, 0.3f);
		poolCount++;
	}

	public void soundToolOption()
	{
		GameCanvas.isPlaySound = !GameCanvas.isPlaySound;
		if (GameCanvas.isPlaySound)
		{
			gI().loadSound(TileMap.mapID);
			Rms.saveRMSInt("isPlaySound", 1);
		}
		else
		{
			gI().closeSound();
			Rms.saveRMSInt("isPlaySound", 0);
		}
		getStrOption();
	}

	public void AuraToolOption()
	{
		if (Char.isPaintAura)
		{
			Rms.saveRMSInt("isPaintAura", 0);
			Char.isPaintAura = false;
		}
		else
		{
			Rms.saveRMSInt("isPaintAura", 1);
			Char.isPaintAura = true;
		}
		getStrOption();
	}

	public void update()
	{
	}

	public void closeSound()
	{
		Sound.stopAll = true;
		stopAll();
	}

	public void openSound()
	{
		if (Sound.music == null)
		{
			loadSound(0);
		}
		Sound.stopAll = false;
	}

	public void bigeExlode()
	{
		Sound.playSound(BIG_EXPLODE, 0.5f);
		poolCount++;
	}

	public void explode_1()
	{
		Sound.playSound(EXPLODE_1, 0.5f);
		poolCount++;
	}

	public void explode_2()
	{
		Sound.playSound(EXPLODE_1, 0.5f);
		poolCount++;
	}

	public void traidatKame()
	{
		Sound.playSound(TRAIDAT_KAME, 1f);
		poolCount++;
	}

	public void namekKame()
	{
		Sound.playSound(NAMEK_KAME, 0.3f);
		poolCount++;
	}

	public void nameLazer()
	{
		Sound.playSound(NAMEK_LAZER, 0.3f);
		poolCount++;
	}

	public void xaydaKame()
	{
		Sound.playSound(XAYDA_KAME, 0.3f);
		poolCount++;
	}

	public void mobKame(int type)
	{
		int id = XAYDA_KAME;
		if (type == 13)
		{
			id = NORMAL_KAME;
		}
		Sound.playSound(id, 0.1f);
		poolCount++;
	}

	public void charRun(float volumn)
	{
		if (!Char.myCharz().me)
		{
			volume /= 2f;
			if (volumn <= 0f)
			{
				volumn = 0.01f;
			}
		}
		if (GameCanvas.gameTick % 8 == 0)
		{
			Sound.playSound(MOVE, volumn);
			poolCount++;
		}
	}

	public void monkeyRun(float volumn)
	{
		if (GameCanvas.gameTick % 8 == 0)
		{
			Sound.playSound(KHICHAY, 0.2f);
			poolCount++;
		}
	}

	public void charFall()
	{
		Sound.playSound(MOVE, 0.1f);
		poolCount++;
	}

	public void charJump()
	{
		Sound.playSound(MOVE, 0.2f);
		poolCount++;
	}

	public void panelOpen()
	{
		Sound.playSound(PANEL_OPEN, 0.5f);
		poolCount++;
	}

	public void buttonClose()
	{
		Sound.playSound(BUTTON_CLOSE, 0.5f);
		poolCount++;
	}

	public void buttonClick()
	{
		Sound.playSound(BUTTON_CLICK, 0.5f);
		poolCount++;
	}

	public void stopMove()
	{
	}

	public void charFly()
	{
		Sound.playSound(FLY, 0.2f);
		poolCount++;
	}

	public void stopFly()
	{
	}

	public void openMenu()
	{
		Sound.playSound(BUTTON_CLOSE, 0.5f);
		poolCount++;
	}

	public void panelClick()
	{
		Sound.playSound(PANEL_CLICK, 0.5f);
		poolCount++;
	}

	public void eatPeans()
	{
		Sound.playSound(EAT_PEAN, 0.5f);
		poolCount++;
	}

	public void openDialog()
	{
		Sound.playSound(OPEN_DIALOG, 0.5f);
	}

	public void hoisinh()
	{
		Sound.playSound(HOISINH, 0.5f);
		poolCount++;
	}

	public void taitao()
	{
		Sound.playMus(TAITAONANGLUONG, 0.5f, loop: true);
	}

	public void taitaoPause()
	{
	}

	public bool isPlayRain()
	{
		try
		{
			return Sound.isPlayingSound();
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool isPlayAirShip()
	{
		return false;
	}

	public void airShip()
	{
		cout++;
		if (cout % 2 == 0)
		{
			Sound.playMus(AIR_SHIP, 0.3f, loop: false);
		}
	}

	public void pauseAirShip()
	{
	}

	public void resumeAirShip()
	{
	}

	public void stopAll()
	{
		Sound.stopAllz();
	}

	public void backToRegister()
	{
		Session_ME.gI().close();
		GameCanvas.panel.hide();
		GameCanvas.loginScr.actRegister();
		GameCanvas.loginScr.switchToMe();
	}

	public void newKame()
	{
		poolCount++;
		if (poolCount % 15 == 0)
		{
			Sound.playSound(TRAIDAT_KAME, 0.5f);
		}
	}

	public void radarClick()
	{
		Sound.playSound(RADAR_CLICK, 0.5f);
	}

	public void radarItem()
	{
		Sound.playSound(RADAR_ITEM, 0.5f);
	}

	public static void playSound(int x, int y, int id, float volume)
	{
		Sound.playSound(id, volume);
	}
}
