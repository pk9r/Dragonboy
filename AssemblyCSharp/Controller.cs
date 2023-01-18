using System;
using Assets.src.e;
using Assets.src.f;
using Assets.src.g;
using Mod;
using Mod.Xmap;
using UnityEngine;

public class Controller : IMessageHandler
{
	protected static Controller me;

	protected static Controller me2;

	public Message messWait;

	public static bool isLoadingData;

	public static bool isConnectOK;

	public static bool isConnectionFail;

	public static bool isDisconnected;

	public static bool isMain;

	private float demCount;

	private int move;

	private int total;

	public static bool isStopReadMessage;

	public static MyHashTable frameHT_NEWBOSS = new MyHashTable();

	public const sbyte PHUBAN_TYPE_CHIENTRUONGNAMEK = 0;

	public const sbyte PHUBAN_START = 0;

	public const sbyte PHUBAN_UPDATE_POINT = 1;

	public const sbyte PHUBAN_END = 2;

	public const sbyte PHUBAN_LIFE = 4;

	public const sbyte PHUBAN_INFO = 5;

	public static Controller gI()
	{
		if (me == null)
			me = new Controller();
		return me;
	}

	public static Controller gI2()
	{
		if (me2 == null)
			me2 = new Controller();
		return me2;
	}

	public void onConnectOK(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onConnectOK();
	}

	public void onConnectionFail(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onConnectionFail();
	}

	public void onDisconnected(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onDisconnected();
	}

	public void requestItemPlayer(Message msg)
	{
		try
		{
			byte num = msg.reader().readUnsignedByte();
			Item item = GameScr.currentCharViewInfo.arrItemBody[num];
			item.saleCoinLock = msg.reader().readInt();
			item.sys = msg.reader().readByte();
			item.options = new MyVector();
			try
			{
				while (true)
				{
					item.options.addElement(new ItemOption(msg.reader().readUnsignedByte(), msg.reader().readUnsignedShort()));
				}
			}
			catch (Exception ex)
			{
				Cout.println("Loi tairequestItemPlayer 1" + ex.ToString());
			}
		}
		catch (Exception ex2)
		{
			Cout.println("Loi tairequestItemPlayer 2" + ex2.ToString());
		}
	}

	public void onMessage(Message msg)
	{
		GameCanvas.debugSession.removeAllElements();
		GameCanvas.debug("SA1", 2);
		try
		{
			mSystem.LogCMD(">>>cmd= " + msg.command);
			Char @char = null;
			Mob mob = null;
			MyVector myVector = new MyVector();
			int num = 0;
			Controller2.readMessage(msg);
			sbyte command = msg.command;
			switch (command)
			{
			default:
				if (command != -112)
				{
					if (command != -107)
						break;
					sbyte b10 = msg.reader().readByte();
					if (b10 == 0)
						Char.myCharz().havePet = false;
					if (b10 == 1)
						Char.myCharz().havePet = true;
					if (b10 != 2)
						break;
					InfoDlg.hide();
					Char.myPetz().head = msg.reader().readShort();
					Char.myPetz().setDefaultPart();
					int num21 = msg.reader().readUnsignedByte();
					Res.outz("num body = " + num21);
					Char.myPetz().arrItemBody = new Item[num21];
					for (int m = 0; m < num21; m++)
					{
						short num22 = msg.reader().readShort();
						Res.outz("template id= " + num22);
						if (num22 == -1)
							continue;
						Res.outz("1");
						Char.myPetz().arrItemBody[m] = new Item();
						Char.myPetz().arrItemBody[m].template = ItemTemplates.get(num22);
						int num23 = Char.myPetz().arrItemBody[m].template.type;
						Char.myPetz().arrItemBody[m].quantity = msg.reader().readInt();
						Res.outz("3");
						Char.myPetz().arrItemBody[m].info = msg.reader().readUTF();
						Char.myPetz().arrItemBody[m].content = msg.reader().readUTF();
						int num24 = msg.reader().readUnsignedByte();
						Res.outz("option size= " + num24);
						if (num24 != 0)
						{
							Char.myPetz().arrItemBody[m].itemOption = new ItemOption[num24];
							for (int n = 0; n < Char.myPetz().arrItemBody[m].itemOption.Length; n++)
							{
								int num25 = msg.reader().readUnsignedByte();
								int param2 = msg.reader().readUnsignedShort();
								if (num25 != -1)
									Char.myPetz().arrItemBody[m].itemOption[n] = new ItemOption(num25, param2);
							}
						}
						if (num23 == 0)
							Char.myPetz().body = Char.myPetz().arrItemBody[m].template.part;
						else if (num23 == 1)
						{
							Char.myPetz().leg = Char.myPetz().arrItemBody[m].template.part;
						}
					}
					Char.myPetz().cHP = msg.readInt3Byte();
					Char.myPetz().cHPFull = msg.readInt3Byte();
					Char.myPetz().cMP = msg.readInt3Byte();
					Char.myPetz().cMPFull = msg.readInt3Byte();
					Char.myPetz().cDamFull = msg.readInt3Byte();
					Char.myPetz().cName = msg.reader().readUTF();
					Char.myPetz().currStrLevel = msg.reader().readUTF();
					Char.myPetz().cPower = msg.reader().readLong();
					Char.myPetz().cTiemNang = msg.reader().readLong();
					Char.myPetz().petStatus = msg.reader().readByte();
					Char.myPetz().cStamina = msg.reader().readShort();
					Char.myPetz().cMaxStamina = msg.reader().readShort();
					Char.myPetz().cCriticalFull = msg.reader().readByte();
					Char.myPetz().cDefull = msg.reader().readShort();
					Char.myPetz().arrPetSkill = new Skill[msg.reader().readByte()];
					Res.outz("SKILLENT = " + Char.myPetz().arrPetSkill);
					for (int num26 = 0; num26 < Char.myPetz().arrPetSkill.Length; num26++)
					{
						short num27 = msg.reader().readShort();
						if (num27 != -1)
						{
							Char.myPetz().arrPetSkill[num26] = Skills.get(num27);
							continue;
						}
						Char.myPetz().arrPetSkill[num26] = new Skill();
						Char.myPetz().arrPetSkill[num26].template = null;
						Char.myPetz().arrPetSkill[num26].moreInfo = msg.reader().readUTF();
					}
					// if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
					// {
					// 	GameCanvas.panel2 = new Panel();
					// 	GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
					// 	GameCanvas.panel2.setTypeBodyOnly();
					// 	GameCanvas.panel2.show();
					// 	GameCanvas.panel.setTypePetMain();
					// 	GameCanvas.panel.show();
					// }
					// else
					// {
					// 	GameCanvas.panel.tabName[21] = mResources.petMainTab;
					// 	GameCanvas.panel.setTypePetMain();
					// 	GameCanvas.panel.show();
					// }
				}
				else
				{
					sbyte b11 = msg.reader().readByte();
					if (b11 == 0)
						GameScr.findMobInMap(msg.reader().readByte()).clearBody();
					if (b11 == 1)
						GameScr.findMobInMap(msg.reader().readByte()).setBody(msg.reader().readShort());
				}
				break;
			case 24:
				read_opt(msg);
				break;
			case 20:
				phuban_Info(msg);
				break;
			case 66:
				readGetImgByName(msg);
				break;
			case 65:
			{
				sbyte b36 = msg.reader().readSByte();
				string text2 = msg.reader().readUTF();
				short num87 = msg.reader().readShort();
				if (ItemTime.isExistMessage(b36))
				{
					if (num87 != 0)
						ItemTime.getMessageById(b36).initTimeText(b36, text2, num87);
					else
						GameScr.textTime.removeElement(ItemTime.getMessageById(b36));
				}
				else
				{
					ItemTime itemTime = new ItemTime();
					itemTime.initTimeText(b36, text2, num87);
					GameScr.textTime.addElement(itemTime);
				}
				break;
			}
			case 112:
			{
				sbyte b16 = msg.reader().readByte();
				Res.outz("spec type= " + b16);
				if (b16 == 0)
				{
					Panel.spearcialImage = msg.reader().readShort();
					Panel.specialInfo = msg.reader().readUTF();
				}
				else
				{
					if (b16 != 1)
						break;
					sbyte b17 = msg.reader().readByte();
					Char.myCharz().infoSpeacialSkill = new string[b17][];
					Char.myCharz().imgSpeacialSkill = new short[b17][];
					GameCanvas.panel.speacialTabName = new string[b17][];
					for (int num35 = 0; num35 < b17; num35++)
					{
						GameCanvas.panel.speacialTabName[num35] = new string[2];
						string[] array4 = Res.split(msg.reader().readUTF(), "\n", 0);
						if (array4.Length == 2)
							GameCanvas.panel.speacialTabName[num35] = array4;
						if (array4.Length == 1)
						{
							GameCanvas.panel.speacialTabName[num35][0] = array4[0];
							GameCanvas.panel.speacialTabName[num35][1] = string.Empty;
						}
						int num36 = msg.reader().readByte();
						Char.myCharz().infoSpeacialSkill[num35] = new string[num36];
						Char.myCharz().imgSpeacialSkill[num35] = new short[num36];
						for (int num37 = 0; num37 < num36; num37++)
						{
							Char.myCharz().imgSpeacialSkill[num35][num37] = msg.reader().readShort();
							Char.myCharz().infoSpeacialSkill[num35][num37] = msg.reader().readUTF();
						}
					}
					GameCanvas.panel.tabName[25] = GameCanvas.panel.speacialTabName;
					GameCanvas.panel.setTypeSpeacialSkill();
					GameCanvas.panel.show();
				}
				break;
			}
			case -98:
			{
				sbyte b7 = msg.reader().readByte();
				GameCanvas.menu.showMenu = false;
				if (b7 == 0)
					GameCanvas.startYesNoDlg(msg.reader().readUTF(), new Command(mResources.YES, GameCanvas.instance, 888397, msg.reader().readUTF()), new Command(mResources.NO, GameCanvas.instance, 888396, null));
				break;
			}
			case -97:
				Char.myCharz().cNangdong = msg.reader().readInt();
				break;
			case -96:
			{
				sbyte typeTop = msg.reader().readByte();
				GameCanvas.panel.vTop.removeAllElements();
				string topName = msg.reader().readUTF();
				sbyte b22 = msg.reader().readByte();
				for (int num54 = 0; num54 < b22; num54++)
				{
					int rank = msg.reader().readInt();
					int pId = msg.reader().readInt();
					short headID = msg.reader().readShort();
					short headICON = msg.reader().readShort();
					short body = msg.reader().readShort();
					short leg = msg.reader().readShort();
					string name = msg.reader().readUTF();
					string info = msg.reader().readUTF();
					TopInfo topInfo = new TopInfo();
					topInfo.rank = rank;
					topInfo.headID = headID;
					topInfo.headICON = headICON;
					topInfo.body = body;
					topInfo.leg = leg;
					topInfo.name = name;
					topInfo.info = info;
					topInfo.info2 = msg.reader().readUTF();
					topInfo.pId = pId;
					GameCanvas.panel.vTop.addElement(topInfo);
				}
				GameCanvas.panel.topName = topName;
				GameCanvas.panel.setTypeTop(typeTop);
				GameCanvas.panel.show();
				break;
			}
			case -94:
				while (msg.reader().available() > 0)
				{
					short num11 = msg.reader().readShort();
					int num12 = msg.reader().readInt();
					for (int j = 0; j < Char.myCharz().vSkill.size(); j++)
					{
						Skill skill = (Skill)Char.myCharz().vSkill.elementAt(j);
						if (skill != null && skill.skillId == num11)
						{
							if (num12 < skill.coolDown)
								skill.lastTimeUseThisSkill = mSystem.currentTimeMillis() - (skill.coolDown - num12);
							Res.outz("1 chieu id= " + skill.template.id + " cooldown= " + num12 + "curr cool down= " + skill.coolDown);
						}
					}
				}
				break;
			case -95:
			{
				sbyte b18 = msg.reader().readByte();
				Res.outz("type= " + b18);
				if (b18 == 0)
				{
					int num38 = msg.reader().readInt();
					short templateId = msg.reader().readShort();
					int num39 = msg.readInt3Byte();
					SoundMn.gI().explode_1();
					if (num38 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe = new Mob(num38, false, false, false, false, false, templateId, 1, num39, 0, num39, (short)(Char.myCharz().cx + ((Char.myCharz().cdir != 1) ? (-40) : 40)), (short)Char.myCharz().cy, 4, 0);
						Char.myCharz().mobMe.isMobMe = true;
						EffecMn.addEff(new Effect(18, Char.myCharz().mobMe.x, Char.myCharz().mobMe.y, 2, 10, -1));
						Char.myCharz().tMobMeBorn = 30;
						GameScr.vMob.addElement(Char.myCharz().mobMe);
					}
					else
					{
						@char = GameScr.findCharInMap(num38);
						if (@char != null)
						{
							Mob mob4 = new Mob(num38, false, false, false, false, false, templateId, 1, num39, 0, num39, (short)@char.cx, (short)@char.cy, 4, 0);
							mob4.isMobMe = true;
							@char.mobMe = mob4;
							GameScr.vMob.addElement(@char.mobMe);
						}
						else if (GameScr.findMobInMap(num38) == null)
						{
							Mob mob5 = new Mob(num38, false, false, false, false, false, templateId, 1, num39, 0, num39, -100, -100, 4, 0);
							mob5.isMobMe = true;
							GameScr.vMob.addElement(mob5);
						}
					}
				}
				if (b18 == 1)
				{
					int num40 = msg.reader().readInt();
					int mobId = msg.reader().readByte();
					Res.outz("mod attack id= " + num40);
					if (num40 == Char.myCharz().charID)
					{
						if (GameScr.findMobInMap(mobId) != null)
							Char.myCharz().mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
					}
					else
					{
						@char = GameScr.findCharInMap(num40);
						if (@char != null && GameScr.findMobInMap(mobId) != null)
							@char.mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
					}
				}
				if (b18 == 2)
				{
					int num41 = msg.reader().readInt();
					int num42 = msg.reader().readInt();
					int num43 = msg.readInt3Byte();
					int cHPNew = msg.readInt3Byte();
					if (num41 == Char.myCharz().charID)
					{
						Res.outz("mob dame= " + num43);
						@char = GameScr.findCharInMap(num42);
						if (@char != null)
						{
							@char.cHPNew = cHPNew;
							if (Char.myCharz().mobMe.isBusyAttackSomeOne)
								@char.doInjure(num43, 0, false, true);
							else
							{
								Char.myCharz().mobMe.dame = num43;
								Char.myCharz().mobMe.setAttack(@char);
							}
						}
					}
					else
					{
						mob = GameScr.findMobInMap(num41);
						if (mob != null)
						{
							if (num42 == Char.myCharz().charID)
							{
								Char.myCharz().cHPNew = cHPNew;
								if (mob.isBusyAttackSomeOne)
									Char.myCharz().doInjure(num43, 0, false, true);
								else
								{
									mob.dame = num43;
									mob.setAttack(Char.myCharz());
								}
							}
							else
							{
								@char = GameScr.findCharInMap(num42);
								if (@char != null)
								{
									@char.cHPNew = cHPNew;
									if (mob.isBusyAttackSomeOne)
										@char.doInjure(num43, 0, false, true);
									else
									{
										mob.dame = num43;
										mob.setAttack(@char);
									}
								}
							}
						}
					}
				}
				if (b18 == 3)
				{
					int num44 = msg.reader().readInt();
					int mobId2 = msg.reader().readInt();
					int hp = msg.readInt3Byte();
					int num45 = msg.readInt3Byte();
					@char = null;
					@char = ((Char.myCharz().charID != num44) ? GameScr.findCharInMap(num44) : Char.myCharz());
					if (@char != null)
					{
						mob = GameScr.findMobInMap(mobId2);
						if (@char.mobMe != null)
							@char.mobMe.attackOtherMob(mob);
						if (mob != null)
						{
							mob.hp = hp;
							mob.updateHp_bar();
							if (num45 == 0)
							{
								mob.x = mob.xFirst;
								mob.y = mob.yFirst;
								GameScr.startFlyText(mResources.miss, mob.x, mob.y - mob.h, 0, -2, mFont.MISS);
							}
							else
								GameScr.startFlyText("-" + num45, mob.x, mob.y - mob.h, 0, -2, mFont.ORANGE);
						}
					}
				}
				if (b18 == 4)
					;
				if (b18 == 5)
				{
					int num46 = msg.reader().readInt();
					sbyte b19 = msg.reader().readByte();
					int mobId3 = msg.reader().readInt();
					int num47 = msg.readInt3Byte();
					int hp2 = msg.readInt3Byte();
					@char = null;
					@char = ((num46 != Char.myCharz().charID) ? GameScr.findCharInMap(num46) : Char.myCharz());
					if (@char == null)
						return;
					if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
						@char.setSkillPaint(GameScr.sks[b19], 0);
					else
						@char.setSkillPaint(GameScr.sks[b19], 1);
					Mob mob6 = GameScr.findMobInMap(mobId3);
					if (@char.cx <= mob6.x)
						@char.cdir = 1;
					else
						@char.cdir = -1;
					@char.mobFocus = mob6;
					mob6.hp = hp2;
					mob6.updateHp_bar();
					GameCanvas.debug("SA83v2", 2);
					if (num47 == 0)
					{
						mob6.x = mob6.xFirst;
						mob6.y = mob6.yFirst;
						GameScr.startFlyText(mResources.miss, mob6.x, mob6.y - mob6.h, 0, -2, mFont.MISS);
					}
					else
						GameScr.startFlyText("-" + num47, mob6.x, mob6.y - mob6.h, 0, -2, mFont.ORANGE);
				}
				if (b18 == 6)
				{
					int num48 = msg.reader().readInt();
					if (num48 == Char.myCharz().charID)
						Char.myCharz().mobMe.startDie();
					else
						GameScr.findCharInMap(num48)?.mobMe.startDie();
				}
				if (b18 != 7)
					break;
				int num49 = msg.reader().readInt();
				if (num49 == Char.myCharz().charID)
				{
					Char.myCharz().mobMe = null;
					for (int num50 = 0; num50 < GameScr.vMob.size(); num50++)
					{
						if (((Mob)GameScr.vMob.elementAt(num50)).mobId == num49)
							GameScr.vMob.removeElementAt(num50);
					}
					break;
				}
				@char = GameScr.findCharInMap(num49);
				for (int num51 = 0; num51 < GameScr.vMob.size(); num51++)
				{
					if (((Mob)GameScr.vMob.elementAt(num51)).mobId == num49)
						GameScr.vMob.removeElementAt(num51);
				}
				if (@char != null)
					@char.mobMe = null;
				break;
			}
			case -92:
				Main.typeClient = msg.reader().readByte();
				Rms.clearAll();
				Rms.saveRMSInt("clienttype", Main.typeClient);
				Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
				GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
				break;
			case -91:
			{
				sbyte b55 = msg.reader().readByte();
				GameCanvas.panel.mapNames = new string[b55];
				GameCanvas.panel.planetNames = new string[b55];
				for (int num128 = 0; num128 < b55; num128++)
				{
					GameCanvas.panel.mapNames[num128] = msg.reader().readUTF();
					GameCanvas.panel.planetNames[num128] = msg.reader().readUTF();
				}
				Pk9rXmap.showPanelMapTrans();
				//GameCanvas.panel.setTypeMapTrans();
				//GameCanvas.panel.show();
				break;
			}
			case -90:
			{
				sbyte b46 = msg.reader().readByte();
				Res.outz("type = " + b46);
				int num113 = msg.reader().readInt();
				if (b46 != -1)
				{
					short num114 = msg.reader().readShort();
					short num115 = msg.reader().readShort();
					short num116 = msg.reader().readShort();
					sbyte b47 = msg.reader().readByte();
					Res.outz("is Monkey = " + b47);
					if (Char.myCharz().charID == num113)
					{
						Char.myCharz().isMask = true;
						Char.myCharz().isMonkey = b47;
						if (Char.myCharz().isMonkey != 0)
						{
							Char.myCharz().isWaitMonkey = false;
							Char.myCharz().isLockMove = false;
						}
					}
					else if (GameScr.findCharInMap(num113) != null)
					{
						GameScr.findCharInMap(num113).isMask = true;
						GameScr.findCharInMap(num113).isMonkey = b47;
					}
					if (num114 != -1)
					{
						if (num113 == Char.myCharz().charID)
							Char.myCharz().head = num114;
						else if (GameScr.findCharInMap(num113) != null)
						{
							GameScr.findCharInMap(num113).head = num114;
						}
					}
					if (num115 != -1)
					{
						if (num113 == Char.myCharz().charID)
							Char.myCharz().body = num115;
						else if (GameScr.findCharInMap(num113) != null)
						{
							GameScr.findCharInMap(num113).body = num115;
						}
					}
					if (num116 != -1)
					{
						if (num113 == Char.myCharz().charID)
							Char.myCharz().leg = num116;
						else if (GameScr.findCharInMap(num113) != null)
						{
							GameScr.findCharInMap(num113).leg = num116;
						}
					}
				}
				if (b46 == -1)
				{
					if (Char.myCharz().charID == num113)
					{
						Char.myCharz().isMask = false;
						Char.myCharz().isMonkey = 0;
					}
					else if (GameScr.findCharInMap(num113) != null)
					{
						GameScr.findCharInMap(num113).isMask = false;
						GameScr.findCharInMap(num113).isMonkey = 0;
					}
				}
				break;
			}
			case -88:
				GameCanvas.endDlg();
				GameCanvas.serverScreen.switchToMe();
				break;
			case -87:
			{
				Res.outz("GET UPDATE_DATA " + msg.reader().available() + " bytes");
				msg.reader().mark(100000);
				createData(msg.reader(), true);
				msg.reader().reset();
				sbyte[] data2 = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data2);
				Rms.saveRMS("NRdataVersion", new sbyte[1] { GameScr.vcData });
				LoginScr.isUpdateData = false;
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					Res.outz(GameScr.vsData + "," + GameScr.vsMap + "," + GameScr.vsSkill + "," + GameScr.vsItem);
					GameScr.gI().readDart();
					GameScr.gI().readEfect();
					GameScr.gI().readArrow();
					GameScr.gI().readSkill();
					Service.gI().clientOk();
					return;
				}
				break;
			}
			case -86:
			{
				sbyte b24 = msg.reader().readByte();
				Res.outz("server gui ve giao dich action = " + b24);
				if (b24 == 0)
				{
					int playerID = msg.reader().readInt();
					GameScr.gI().giaodich(playerID);
				}
				if (b24 == 1)
				{
					int num56 = msg.reader().readInt();
					Char char5 = GameScr.findCharInMap(num56);
					if (char5 == null)
						return;
					GameCanvas.panel.setTypeGiaoDich(char5);
					GameCanvas.panel.show();
					Service.gI().getPlayerMenu(num56);
				}
				if (b24 == 2)
				{
					sbyte b25 = msg.reader().readByte();
					for (int num57 = 0; num57 < GameCanvas.panel.vMyGD.size(); num57++)
					{
						Item item = (Item)GameCanvas.panel.vMyGD.elementAt(num57);
						if (item.indexUI == b25)
						{
							GameCanvas.panel.vMyGD.removeElement(item);
							break;
						}
					}
				}
				if (b24 == 5)
					;
				if (b24 == 6)
				{
					GameCanvas.panel.isFriendLock = true;
					if (GameCanvas.panel2 != null)
						GameCanvas.panel2.isFriendLock = true;
					GameCanvas.panel.vFriendGD.removeAllElements();
					if (GameCanvas.panel2 != null)
						GameCanvas.panel2.vFriendGD.removeAllElements();
					int friendMoneyGD = msg.reader().readInt();
					sbyte b26 = msg.reader().readByte();
					Res.outz("item size = " + b26);
					for (int num58 = 0; num58 < b26; num58++)
					{
						Item item2 = new Item();
						item2.template = ItemTemplates.get(msg.reader().readShort());
						item2.quantity = msg.reader().readInt();
						int num59 = msg.reader().readUnsignedByte();
						if (num59 != 0)
						{
							item2.itemOption = new ItemOption[num59];
							for (int num60 = 0; num60 < item2.itemOption.Length; num60++)
							{
								int num61 = msg.reader().readUnsignedByte();
								int param3 = msg.reader().readUnsignedShort();
								if (num61 != -1)
								{
									item2.itemOption[num60] = new ItemOption(num61, param3);
									item2.compare = GameCanvas.panel.getCompare(item2);
								}
							}
						}
						if (GameCanvas.panel2 != null)
							GameCanvas.panel2.vFriendGD.addElement(item2);
						else
							GameCanvas.panel.vFriendGD.addElement(item2);
					}
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.setTabGiaoDich(false);
						GameCanvas.panel2.friendMoneyGD = friendMoneyGD;
					}
					else
					{
						GameCanvas.panel.friendMoneyGD = friendMoneyGD;
						if (GameCanvas.panel.currentTabIndex == 2)
							GameCanvas.panel.setTabGiaoDich(false);
					}
				}
				if (b24 == 7)
				{
					InfoDlg.hide();
					if (GameCanvas.panel.isShow)
						GameCanvas.panel.hide();
				}
				break;
			}
			case -85:
			{
				Res.outz("CAP CHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
				sbyte b8 = msg.reader().readByte();
				if (b8 == 0)
				{
					int num15 = msg.reader().readUnsignedShort();
					Res.outz("lent =" + num15);
					sbyte[] data = new sbyte[num15];
					msg.reader().read(ref data, 0, num15);
					GameScr.imgCapcha = Image.createImage(data, 0, num15);
					GameScr.gI().keyInput = "-----";
					GameScr.gI().strCapcha = msg.reader().readUTF();
					GameScr.gI().keyCapcha = new int[GameScr.gI().strCapcha.Length];
					GameScr.gI().mobCapcha = new Mob();
					GameScr.gI().right = null;
				}
				if (b8 == 1)
					MobCapcha.isAttack = true;
				if (b8 == 2)
				{
					MobCapcha.explode = true;
					GameScr.gI().right = GameScr.gI().cmdFocus;
				}
				break;
			}
			case -84:
			{
				int index2 = msg.reader().readUnsignedByte();
				Mob mob8 = null;
				try
				{
					mob8 = (Mob)GameScr.vMob.elementAt(index2);
				}
				catch (Exception)
				{
				}
				if (mob8 != null)
					mob8.maxHp = msg.reader().readInt();
				break;
			}
			case -83:
			{
				sbyte b63 = msg.reader().readByte();
				if (b63 == 0)
				{
					int num155 = msg.reader().readShort();
					int bgRID = msg.reader().readShort();
					int num156 = msg.reader().readUnsignedByte();
					int num157 = msg.reader().readInt();
					string text10 = msg.reader().readUTF();
					int num158 = msg.reader().readShort();
					int num159 = msg.reader().readShort();
					if (msg.reader().readByte() == 1)
						GameScr.gI().isRongNamek = true;
					else
						GameScr.gI().isRongNamek = false;
					GameScr.gI().xR = num158;
					GameScr.gI().yR = num159;
					Res.outz("xR= " + num158 + " yR= " + num159 + " +++++++++++++++++++++++++++++++++++++++");
					if (Char.myCharz().charID == num157)
					{
						GameCanvas.panel.hideNow();
						GameScr.gI().activeRongThanEff(true);
					}
					else if (TileMap.mapID == num155 && TileMap.zoneID == num156)
					{
						GameScr.gI().activeRongThanEff(false);
					}
					else if (mGraphics.zoomLevel > 1)
					{
						GameScr.gI().doiMauTroi();
					}
					GameScr.gI().mapRID = num155;
					GameScr.gI().bgRID = bgRID;
					GameScr.gI().zoneRID = num156;
				}
				if (b63 == 1)
				{
					Res.outz("map RID = " + GameScr.gI().mapRID + " zone RID= " + GameScr.gI().zoneRID);
					Res.outz("map ID = " + TileMap.mapID + " zone ID= " + TileMap.zoneID);
					if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
						GameScr.gI().hideRongThanEff();
					else
					{
						GameScr.gI().isRongThanXuatHien = false;
						if (GameScr.gI().isRongNamek)
							GameScr.gI().isRongNamek = false;
					}
				}
				if (b63 != 2)
					;
				break;
			}
			case -82:
			{
				sbyte b58 = msg.reader().readByte();
				TileMap.tileIndex = new int[b58][][];
				TileMap.tileType = new int[b58][];
				for (int num140 = 0; num140 < b58; num140++)
				{
					sbyte b59 = msg.reader().readByte();
					TileMap.tileType[num140] = new int[b59];
					TileMap.tileIndex[num140] = new int[b59][];
					for (int num141 = 0; num141 < b59; num141++)
					{
						TileMap.tileType[num140][num141] = msg.reader().readInt();
						sbyte b60 = msg.reader().readByte();
						TileMap.tileIndex[num140][num141] = new int[b60];
						for (int num142 = 0; num142 < b60; num142++)
						{
							TileMap.tileIndex[num140][num141][num142] = msg.reader().readByte();
						}
					}
				}
				break;
			}
			case -81:
			{
				sbyte b31 = msg.reader().readByte();
				if (b31 == 0)
				{
					string src = msg.reader().readUTF();
					string src2 = msg.reader().readUTF();
					GameCanvas.panel.setTypeCombine();
					GameCanvas.panel.combineInfo = mFont.tahoma_7b_blue.splitFontArray(src, Panel.WIDTH_PANEL);
					GameCanvas.panel.combineTopInfo = mFont.tahoma_7.splitFontArray(src2, Panel.WIDTH_PANEL);
					GameCanvas.panel.show();
				}
				if (b31 == 1)
				{
					GameCanvas.panel.vItemCombine.removeAllElements();
					sbyte b32 = msg.reader().readByte();
					for (int num80 = 0; num80 < b32; num80++)
					{
						sbyte b33 = msg.reader().readByte();
						for (int num81 = 0; num81 < Char.myCharz().arrItemBag.Length; num81++)
						{
							Item item3 = Char.myCharz().arrItemBag[num81];
							if (item3 != null && item3.indexUI == b33)
							{
								item3.isSelect = true;
								GameCanvas.panel.vItemCombine.addElement(item3);
							}
						}
					}
					if (GameCanvas.panel.isShow)
						GameCanvas.panel.setTabCombine();
				}
				if (b31 == 2)
				{
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b31 == 3)
				{
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b31 == 4)
				{
					short iconID = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(1);
				}
				if (b31 == 5)
				{
					short iconID2 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID2;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(2);
				}
				if (b31 == 6)
				{
					short iconID3 = msg.reader().readShort();
					short iconID4 = msg.reader().readShort();
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(3);
					GameCanvas.panel.iconID1 = iconID3;
					GameCanvas.panel.iconID3 = iconID4;
				}
				if (b31 == 7)
				{
					short iconID5 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID5;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(4);
				}
				if (b31 == 8)
				{
					GameCanvas.panel.iconID3 = -1;
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(4);
				}
				short num82 = 21;
				try
				{
					num82 = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				for (int num83 = 0; num83 < GameScr.vNpc.size(); num83++)
				{
					Npc npc3 = (Npc)GameScr.vNpc.elementAt(num83);
					if (npc3.template.npcTemplateId == num82)
					{
						GameCanvas.panel.xS = npc3.cx - GameScr.cmx;
						GameCanvas.panel.yS = npc3.cy - GameScr.cmy;
						GameCanvas.panel.idNPC = num82;
						break;
					}
				}
				break;
			}
			case -80:
			{
				sbyte b56 = msg.reader().readByte();
				InfoDlg.hide();
				if (b56 == 0)
				{
					GameCanvas.panel.vFriend.removeAllElements();
					int num129 = msg.reader().readUnsignedByte();
					for (int num130 = 0; num130 < num129; num130++)
					{
						Char char8 = new Char();
						char8.charID = msg.reader().readInt();
						char8.head = msg.reader().readShort();
						char8.headICON = msg.reader().readShort();
						char8.body = msg.reader().readShort();
						char8.leg = msg.reader().readShort();
						char8.bag = msg.reader().readUnsignedByte();
						char8.cName = msg.reader().readUTF();
						bool isOnline = msg.reader().readBoolean();
						InfoItem infoItem = new InfoItem(mResources.power + ": " + msg.reader().readUTF());
						infoItem.charInfo = char8;
						infoItem.isOnline = isOnline;
						GameCanvas.panel.vFriend.addElement(infoItem);
					}
					GameCanvas.panel.setTypeFriend();
					GameCanvas.panel.show();
				}
				if (b56 == 3)
				{
					MyVector vFriend = GameCanvas.panel.vFriend;
					int num131 = msg.reader().readInt();
					Res.outz("online offline id=" + num131);
					for (int num132 = 0; num132 < vFriend.size(); num132++)
					{
						InfoItem infoItem2 = (InfoItem)vFriend.elementAt(num132);
						if (infoItem2.charInfo != null && infoItem2.charInfo.charID == num131)
						{
							Res.outz("online= " + infoItem2.isOnline);
							infoItem2.isOnline = msg.reader().readBoolean();
							break;
						}
					}
				}
				if (b56 != 2)
					break;
				MyVector vFriend2 = GameCanvas.panel.vFriend;
				int num133 = msg.reader().readInt();
				for (int num134 = 0; num134 < vFriend2.size(); num134++)
				{
					InfoItem infoItem3 = (InfoItem)vFriend2.elementAt(num134);
					if (infoItem3.charInfo != null && infoItem3.charInfo.charID == num133)
					{
						vFriend2.removeElement(infoItem3);
						break;
					}
				}
				if (GameCanvas.panel.isShow)
					GameCanvas.panel.setTabFriend();
				break;
			}
			case -99:
				InfoDlg.hide();
				if (msg.reader().readByte() == 0)
				{
					GameCanvas.panel.vEnemy.removeAllElements();
					int num136 = msg.reader().readUnsignedByte();
					for (int num137 = 0; num137 < num136; num137++)
					{
						Char char9 = new Char();
						char9.charID = msg.reader().readInt();
						char9.head = msg.reader().readShort();
						char9.headICON = msg.reader().readShort();
						char9.body = msg.reader().readShort();
						char9.leg = msg.reader().readShort();
						char9.bag = msg.reader().readShort();
						char9.cName = msg.reader().readUTF();
						InfoItem infoItem4 = new InfoItem(msg.reader().readUTF());
						bool flag8 = msg.reader().readBoolean();
						infoItem4.charInfo = char9;
						infoItem4.isOnline = flag8;
						Res.outz("isonline = " + flag8);
						GameCanvas.panel.vEnemy.addElement(infoItem4);
					}
					GameCanvas.panel.setTypeEnemy();
					GameCanvas.panel.show();
				}
				break;
			case -79:
			{
				InfoDlg.hide();
				int num110 = msg.reader().readInt();
				Char charMenu = GameCanvas.panel.charMenu;
				if (charMenu == null)
					return;
				charMenu.cPower = msg.reader().readLong();
				charMenu.currStrLevel = msg.reader().readUTF();
				break;
			}
			case -93:
			{
				short num107 = msg.reader().readShort();
				BgItem.newSmallVersion = new sbyte[num107];
				for (int num108 = 0; num108 < num107; num108++)
				{
					BgItem.newSmallVersion[num108] = msg.reader().readByte();
				}
				break;
			}
			case -77:
			{
				short num138 = msg.reader().readShort();
				SmallImage.newSmallVersion = new sbyte[num138];
				SmallImage.maxSmall = num138;
				SmallImage.imgNew = new Small[num138];
				for (int num139 = 0; num139 < num138; num139++)
				{
					SmallImage.newSmallVersion[num139] = msg.reader().readByte();
				}
				break;
			}
			case -76:
			{
				sbyte b20 = msg.reader().readByte();
				if (b20 == 0)
				{
					sbyte b21 = msg.reader().readByte();
					if (b21 <= 0)
						return;
					Char.myCharz().arrArchive = new Archivement[b21];
					for (int num52 = 0; num52 < b21; num52++)
					{
						Char.myCharz().arrArchive[num52] = new Archivement();
						Char.myCharz().arrArchive[num52].info1 = num52 + 1 + ". " + msg.reader().readUTF();
						Char.myCharz().arrArchive[num52].info2 = msg.reader().readUTF();
						Char.myCharz().arrArchive[num52].money = msg.reader().readShort();
						Char.myCharz().arrArchive[num52].isFinish = msg.reader().readBoolean();
						Char.myCharz().arrArchive[num52].isRecieve = msg.reader().readBoolean();
					}
					GameCanvas.panel.setTypeArchivement();
					GameCanvas.panel.show();
				}
				else if (b20 == 1)
				{
					int num53 = msg.reader().readUnsignedByte();
					if (Char.myCharz().arrArchive[num53] != null)
						Char.myCharz().arrArchive[num53].isRecieve = true;
				}
				break;
			}
			case -74:
			{
				if (ServerListScreen.stopDownload)
					return;
				if (!GameCanvas.isGetResourceFromServer())
				{
					Service.gI().getResource(3, null);
					SmallImage.loadBigRMS();
					SplashScr.imgLogo = null;
					if (Rms.loadRMSString("acc") != null || Rms.loadRMSString("userAo" + ServerListScreen.ipSelect) != null)
						LoginScr.isContinueToLogin = true;
					GameCanvas.loginScr = new LoginScr();
					GameCanvas.loginScr.switchToMe();
					return;
				}
				bool flag7 = true;
				sbyte b38 = msg.reader().readByte();
				Res.outz("action = " + b38);
				if (b38 == 0)
				{
					int num89 = msg.reader().readInt();
					string text3 = Rms.loadRMSString("ResVersion");
					int num90 = ((text3 == null || !(text3 != string.Empty)) ? (-1) : int.Parse(text3));
					if (num90 == -1 || num90 != num89)
					{
						ServerListScreen.loadScreen = false;
						GameCanvas.serverScreen.show2();
					}
					else
					{
						Res.outz("login ngay");
						SmallImage.loadBigRMS();
						SplashScr.imgLogo = null;
						ServerListScreen.loadScreen = true;
						if (GameCanvas.currentScreen != GameCanvas.loginScr)
							GameCanvas.serverScreen.switchToMe();
					}
				}
				if (b38 == 1)
				{
					ServerListScreen.strWait = mResources.downloading_data;
					ServerListScreen.nBig = msg.reader().readShort();
					Service.gI().getResource(2, null);
				}
				if (b38 == 2)
					try
					{
						isLoadingData = true;
						GameCanvas.endDlg();
						ServerListScreen.demPercent++;
						ServerListScreen.percent = ServerListScreen.demPercent * 100 / ServerListScreen.nBig;
						string[] array9 = Res.split(msg.reader().readUTF(), "/", 0);
						string filename = "x" + mGraphics.zoomLevel + array9[array9.Length - 1];
						int num91 = msg.reader().readInt();
						sbyte[] data3 = new sbyte[num91];
						msg.reader().read(ref data3, 0, num91);
						Rms.saveRMS(filename, data3);
					}
					catch (Exception)
					{
						GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
					}
				if (b38 == 3 && flag7)
				{
					isLoadingData = false;
					int num92 = msg.reader().readInt();
					Res.outz("last version= " + num92);
					Rms.saveRMSString("ResVersion", num92 + string.Empty);
					Service.gI().getResource(3, null);
					GameCanvas.endDlg();
					SplashScr.imgLogo = null;
					SmallImage.loadBigRMS();
					mSystem.gcc();
					ServerListScreen.bigOk = true;
					ServerListScreen.loadScreen = true;
					GameScr.gI().loadGameScr();
					if (GameCanvas.currentScreen != GameCanvas.loginScr)
						GameCanvas.serverScreen.switchToMe();
				}
				break;
			}
			case -43:
			{
				sbyte itemAction = msg.reader().readByte();
				sbyte where = msg.reader().readByte();
				sbyte index = msg.reader().readByte();
				string info3 = msg.reader().readUTF();
				GameCanvas.panel.itemRequest(itemAction, info3, where, index);
				break;
			}
			case -59:
			{
				sbyte typePK = msg.reader().readByte();
				GameScr.gI().player_vs_player(msg.reader().readInt(), msg.reader().readInt(), msg.reader().readUTF(), typePK);
				break;
			}
			case -62:
			{
				int num85 = msg.reader().readUnsignedByte();
				sbyte b35 = msg.reader().readByte();
				if (b35 <= 0)
					break;
				ClanImage clanImage2 = ClanImage.getClanImage((sbyte)num85);
				if (clanImage2 == null)
					break;
				clanImage2.idImage = new short[b35];
				for (int num86 = 0; num86 < b35; num86++)
				{
					clanImage2.idImage[num86] = msg.reader().readShort();
					if (clanImage2.idImage[num86] > 0)
						SmallImage.vKeys.addElement(clanImage2.idImage[num86] + string.Empty);
				}
				break;
			}
			case -65:
			{
				Res.outz("TELEPORT ...................................................");
				InfoDlg.hide();
				int num127 = msg.reader().readInt();
				sbyte b54 = msg.reader().readByte();
				if (b54 == 0)
					break;
				if (Char.myCharz().charID == num127)
				{
					isStopReadMessage = true;
					GameScr.lockTick = 500;
					GameScr.gI().center = null;
					if (b54 == 0 || b54 == 1 || b54 == 3)
						Teleport.addTeleport(new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 0, true, (b54 != 1) ? b54 : Char.myCharz().cgender));
					if (b54 == 2)
					{
						GameScr.lockTick = 50;
						Char.myCharz().hide();
					}
				}
				else
				{
					Char char7 = GameScr.findCharInMap(num127);
					if ((b54 == 0 || b54 == 1 || b54 == 3) && char7 != null)
					{
						char7.isUsePlane = true;
						Teleport teleport = new Teleport(char7.cx, char7.cy, char7.head, char7.cdir, 0, false, (b54 != 1) ? b54 : char7.cgender);
						teleport.id = num127;
						Teleport.addTeleport(teleport);
					}
					if (b54 == 2)
						char7.hide();
				}
				break;
			}
			case -64:
			{
				int num101 = msg.reader().readInt();
				int bag = msg.reader().readUnsignedByte();
				if (num101 == Char.myCharz().charID)
					Char.myCharz().bag = bag;
				else if (GameScr.findCharInMap(num101) != null)
				{
					GameScr.findCharInMap(num101).bag = bag;
				}
				break;
			}
			case -63:
			{
				Res.outz("GET BAG");
				int num104 = msg.reader().readUnsignedByte();
				sbyte b43 = msg.reader().readByte();
				ClanImage clanImage3 = new ClanImage();
				clanImage3.ID = num104;
				if (b43 > 0)
				{
					clanImage3.idImage = new short[b43];
					for (int num105 = 0; num105 < b43; num105++)
					{
						clanImage3.idImage[num105] = msg.reader().readShort();
						Res.outz("ID=  " + num104 + " frame= " + clanImage3.idImage[num105]);
					}
					ClanImage.idImages.put(num104 + string.Empty, clanImage3);
				}
				break;
			}
			case -57:
			{
				string strInvite = msg.reader().readUTF();
				int clanID = msg.reader().readInt();
				int code = msg.reader().readInt();
				GameScr.gI().clanInvite(strInvite, clanID, code);
				break;
			}
			case -51:
				InfoDlg.hide();
				readClanMsg(msg, 0);
				if (GameCanvas.panel.isMessage && GameCanvas.panel.type == 5)
					GameCanvas.panel.initTabClans();
				break;
			case -53:
			{
				Res.outz("MY CLAN INFO");
				InfoDlg.hide();
				bool flag9 = false;
				int num160 = msg.reader().readInt();
				Res.outz("clanId= " + num160);
				if (num160 == -1)
				{
					flag9 = true;
					Char.myCharz().clan = null;
					ClanMessage.vMessage.removeAllElements();
					if (GameCanvas.panel.member != null)
						GameCanvas.panel.member.removeAllElements();
					if (GameCanvas.panel.myMember != null)
						GameCanvas.panel.myMember.removeAllElements();
					if (GameCanvas.currentScreen == GameScr.gI())
						GameCanvas.panel.setTabClans();
					return;
				}
				GameCanvas.panel.tabIcon = null;
				if (Char.myCharz().clan == null)
					Char.myCharz().clan = new Clan();
				Char.myCharz().clan.ID = num160;
				Char.myCharz().clan.name = msg.reader().readUTF();
				Char.myCharz().clan.slogan = msg.reader().readUTF();
				Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
				Char.myCharz().clan.powerPoint = msg.reader().readUTF();
				Char.myCharz().clan.leaderName = msg.reader().readUTF();
				Char.myCharz().clan.currMember = msg.reader().readUnsignedByte();
				Char.myCharz().clan.maxMember = msg.reader().readUnsignedByte();
				Char.myCharz().role = msg.reader().readByte();
				Char.myCharz().clan.clanPoint = msg.reader().readInt();
				Char.myCharz().clan.level = msg.reader().readByte();
				GameCanvas.panel.myMember = new MyVector();
				for (int num161 = 0; num161 < Char.myCharz().clan.currMember; num161++)
				{
					Member member5 = new Member();
					member5.ID = msg.reader().readInt();
					member5.head = msg.reader().readShort();
					member5.headICON = msg.reader().readShort();
					member5.leg = msg.reader().readShort();
					member5.body = msg.reader().readShort();
					member5.name = msg.reader().readUTF();
					member5.role = msg.reader().readByte();
					member5.powerPoint = msg.reader().readUTF();
					member5.donate = msg.reader().readInt();
					member5.receive_donate = msg.reader().readInt();
					member5.clanPoint = msg.reader().readInt();
					member5.curClanPoint = msg.reader().readInt();
					member5.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.myMember.addElement(member5);
				}
				int num162 = msg.reader().readUnsignedByte();
				for (int num163 = 0; num163 < num162; num163++)
				{
					readClanMsg(msg, -1);
				}
				if (GameCanvas.panel.isSearchClan || GameCanvas.panel.isViewMember || GameCanvas.panel.isMessage)
					GameCanvas.panel.setTabClans();
				if (flag9)
					GameCanvas.panel.setTabClans();
				break;
			}
			case -52:
			{
				sbyte b44 = msg.reader().readByte();
				if (b44 == 0)
				{
					Member member2 = new Member();
					member2.ID = msg.reader().readInt();
					member2.head = msg.reader().readShort();
					member2.headICON = msg.reader().readShort();
					member2.leg = msg.reader().readShort();
					member2.body = msg.reader().readShort();
					member2.name = msg.reader().readUTF();
					member2.role = msg.reader().readByte();
					member2.powerPoint = msg.reader().readUTF();
					member2.donate = msg.reader().readInt();
					member2.receive_donate = msg.reader().readInt();
					member2.clanPoint = msg.reader().readInt();
					member2.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					if (GameCanvas.panel.myMember == null)
						GameCanvas.panel.myMember = new MyVector();
					GameCanvas.panel.myMember.addElement(member2);
					GameCanvas.panel.initTabClans();
				}
				if (b44 == 1)
				{
					GameCanvas.panel.myMember.removeElementAt(msg.reader().readByte());
					GameCanvas.panel.currentListLength--;
					GameCanvas.panel.initTabClans();
				}
				if (b44 != 2)
					break;
				Member member3 = new Member();
				member3.ID = msg.reader().readInt();
				member3.head = msg.reader().readShort();
				member3.headICON = msg.reader().readShort();
				member3.leg = msg.reader().readShort();
				member3.body = msg.reader().readShort();
				member3.name = msg.reader().readUTF();
				member3.role = msg.reader().readByte();
				member3.powerPoint = msg.reader().readUTF();
				member3.donate = msg.reader().readInt();
				member3.receive_donate = msg.reader().readInt();
				member3.clanPoint = msg.reader().readInt();
				member3.joinTime = NinjaUtil.getDate(msg.reader().readInt());
				for (int num111 = 0; num111 < GameCanvas.panel.myMember.size(); num111++)
				{
					Member member4 = (Member)GameCanvas.panel.myMember.elementAt(num111);
					if (member4.ID == member3.ID)
					{
						if (Char.myCharz().charID == member3.ID)
							Char.myCharz().role = member3.role;
						Member o2 = member3;
						GameCanvas.panel.myMember.removeElement(member4);
						GameCanvas.panel.myMember.insertElementAt(o2, num111);
						return;
					}
				}
				break;
			}
			case -50:
			{
				InfoDlg.hide();
				GameCanvas.panel.member = new MyVector();
				sbyte b40 = msg.reader().readByte();
				for (int num93 = 0; num93 < b40; num93++)
				{
					Member member = new Member();
					member.ID = msg.reader().readInt();
					member.head = msg.reader().readShort();
					member.headICON = msg.reader().readShort();
					member.leg = msg.reader().readShort();
					member.body = msg.reader().readShort();
					member.name = msg.reader().readUTF();
					member.role = msg.reader().readByte();
					member.powerPoint = msg.reader().readUTF();
					member.donate = msg.reader().readInt();
					member.receive_donate = msg.reader().readInt();
					member.clanPoint = msg.reader().readInt();
					member.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.member.addElement(member);
				}
				GameCanvas.panel.isViewMember = true;
				GameCanvas.panel.isSearchClan = false;
				GameCanvas.panel.isMessage = false;
				GameCanvas.panel.currentListLength = GameCanvas.panel.member.size() + 2;
				GameCanvas.panel.initTabClans();
				break;
			}
			case -47:
			{
				InfoDlg.hide();
				sbyte b34 = msg.reader().readByte();
				Res.outz("clan = " + b34);
				if (b34 == 0)
				{
					GameCanvas.panel.clanReport = mResources.cannot_find_clan;
					GameCanvas.panel.clans = null;
				}
				else
				{
					GameCanvas.panel.clans = new Clan[b34];
					Res.outz("clan search lent= " + GameCanvas.panel.clans.Length);
					for (int num84 = 0; num84 < GameCanvas.panel.clans.Length; num84++)
					{
						GameCanvas.panel.clans[num84] = new Clan();
						GameCanvas.panel.clans[num84].ID = msg.reader().readInt();
						GameCanvas.panel.clans[num84].name = msg.reader().readUTF();
						GameCanvas.panel.clans[num84].slogan = msg.reader().readUTF();
						GameCanvas.panel.clans[num84].imgID = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num84].powerPoint = msg.reader().readUTF();
						GameCanvas.panel.clans[num84].leaderName = msg.reader().readUTF();
						GameCanvas.panel.clans[num84].currMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num84].maxMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num84].date = msg.reader().readInt();
					}
				}
				GameCanvas.panel.isSearchClan = true;
				GameCanvas.panel.isViewMember = false;
				GameCanvas.panel.isMessage = false;
				if (GameCanvas.panel.isSearchClan)
					GameCanvas.panel.initTabClans();
				break;
			}
			case -46:
			{
				InfoDlg.hide();
				sbyte b30 = msg.reader().readByte();
				if (b30 == 1 || b30 == 3)
				{
					GameCanvas.endDlg();
					ClanImage.vClanImage.removeAllElements();
					int num74 = msg.reader().readUnsignedByte();
					for (int num75 = 0; num75 < num74; num75++)
					{
						ClanImage clanImage = new ClanImage();
						clanImage.ID = msg.reader().readUnsignedByte();
						clanImage.name = msg.reader().readUTF();
						clanImage.xu = msg.reader().readInt();
						clanImage.luong = msg.reader().readInt();
						if (!ClanImage.isExistClanImage(clanImage.ID))
						{
							ClanImage.addClanImage(clanImage);
							continue;
						}
						ClanImage.getClanImage((sbyte)clanImage.ID).name = clanImage.name;
						ClanImage.getClanImage((sbyte)clanImage.ID).xu = clanImage.xu;
						ClanImage.getClanImage((sbyte)clanImage.ID).luong = clanImage.luong;
					}
					if (Char.myCharz().clan != null)
						GameCanvas.panel.changeIcon();
				}
				if (b30 == 4)
				{
					Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
					Char.myCharz().clan.slogan = msg.reader().readUTF();
				}
				break;
			}
			case -61:
			{
				int num34 = msg.reader().readInt();
				if (num34 != Char.myCharz().charID)
				{
					if (GameScr.findCharInMap(num34) != null)
					{
						GameScr.findCharInMap(num34).clanID = msg.reader().readInt();
						if (GameScr.findCharInMap(num34).clanID == -2)
							GameScr.findCharInMap(num34).isCopy = true;
					}
				}
				else if (Char.myCharz().clan != null)
				{
					Char.myCharz().clan.ID = msg.reader().readInt();
				}
				break;
			}
			case -42:
				Char.myCharz().cHPGoc = msg.readInt3Byte();
				Char.myCharz().cMPGoc = msg.readInt3Byte();
				Char.myCharz().cDamGoc = msg.reader().readInt();
				Char.myCharz().cHPFull = msg.readInt3Byte();
				Char.myCharz().cMPFull = msg.readInt3Byte();
				Char.myCharz().cHP = msg.readInt3Byte();
				Char.myCharz().cMP = msg.readInt3Byte();
				Char.myCharz().cspeed = msg.reader().readByte();
				Char.myCharz().hpFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().mpFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().damFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().cDamFull = msg.reader().readInt();
				Char.myCharz().cDefull = msg.reader().readInt();
				Char.myCharz().cCriticalFull = msg.reader().readByte();
				Char.myCharz().cTiemNang = msg.reader().readLong();
				Char.myCharz().expForOneAdd = msg.reader().readShort();
				Char.myCharz().cDefGoc = msg.reader().readShort();
				Char.myCharz().cCriticalGoc = msg.reader().readByte();
				InfoDlg.hide();
				break;
			case 1:
			{
				bool flag3 = msg.reader().readBool();
				Res.outz("isRes= " + flag3);
				if (!flag3)
				{
					GameCanvas.startOKDlg(msg.reader().readUTF());
					break;
				}
				GameCanvas.loginScr.isLogin2 = false;
				Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
				GameCanvas.endDlg();
				GameCanvas.loginScr.doLogin();
				break;
			}
			case 2:
				Char.isLoadingMap = true;
				LoginScr.isLoggingIn = false;
				if (!GameScr.isLoadAllData)
					GameScr.gI().initSelectChar();
				BgItem.clearHashTable();
				GameCanvas.endDlg();
				CreateCharScr.isCreateChar = true;
				CreateCharScr.gI().switchToMe();
				break;
			case -37:
			{
				sbyte b9 = msg.reader().readByte();
				Res.outz("cAction= " + b9);
				if (b9 != 0)
					break;
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().setDefaultPart();
				int num16 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num16);
				Char.myCharz().arrItemBody = new Item[num16];
				for (int k = 0; k < num16; k++)
				{
					short num17 = msg.reader().readShort();
					if (num17 == -1)
						continue;
					Char.myCharz().arrItemBody[k] = new Item();
					Char.myCharz().arrItemBody[k].template = ItemTemplates.get(num17);
					int num18 = Char.myCharz().arrItemBody[k].template.type;
					Char.myCharz().arrItemBody[k].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBody[k].info = msg.reader().readUTF();
					Char.myCharz().arrItemBody[k].content = msg.reader().readUTF();
					int num19 = msg.reader().readUnsignedByte();
					if (num19 != 0)
					{
						Char.myCharz().arrItemBody[k].itemOption = new ItemOption[num19];
						for (int l = 0; l < Char.myCharz().arrItemBody[k].itemOption.Length; l++)
						{
							int num20 = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							if (num20 != -1)
								Char.myCharz().arrItemBody[k].itemOption[l] = new ItemOption(num20, param);
						}
					}
					if (num18 == 0)
						Char.myCharz().body = Char.myCharz().arrItemBody[k].template.part;
					else if (num18 == 1)
					{
						Char.myCharz().leg = Char.myCharz().arrItemBody[k].template.part;
					}
				}
				break;
			}
			case -36:
			{
				sbyte b61 = msg.reader().readByte();
				Res.outz("cAction= " + b61);
				if (b61 == 0)
				{
					int num145 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBag = new Item[num145];
					GameScr.hpPotion = 0;
					Res.outz("numC=" + num145);
					for (int num146 = 0; num146 < num145; num146++)
					{
						short num147 = msg.reader().readShort();
						if (num147 == -1)
							continue;
						Char.myCharz().arrItemBag[num146] = new Item();
						Char.myCharz().arrItemBag[num146].template = ItemTemplates.get(num147);
						Char.myCharz().arrItemBag[num146].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBag[num146].info = msg.reader().readUTF();
						Char.myCharz().arrItemBag[num146].content = msg.reader().readUTF();
						Char.myCharz().arrItemBag[num146].indexUI = num146;
						int num148 = msg.reader().readUnsignedByte();
						if (num148 != 0)
						{
							Char.myCharz().arrItemBag[num146].itemOption = new ItemOption[num148];
							for (int num149 = 0; num149 < Char.myCharz().arrItemBag[num146].itemOption.Length; num149++)
							{
								int num150 = msg.reader().readUnsignedByte();
								int param6 = msg.reader().readUnsignedShort();
								if (num150 != -1)
									Char.myCharz().arrItemBag[num146].itemOption[num149] = new ItemOption(num150, param6);
							}
							Char.myCharz().arrItemBag[num146].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemBag[num146]);
						}
						if (Char.myCharz().arrItemBag[num146].template.type == 11)
							;
						if (Char.myCharz().arrItemBag[num146].template.type == 6)
							GameScr.hpPotion += Char.myCharz().arrItemBag[num146].quantity;
					}
				}
				if (b61 == 2)
				{
					sbyte b62 = msg.reader().readByte();
					int quantity2 = msg.reader().readInt();
					int quantity3 = Char.myCharz().arrItemBag[b62].quantity;
					Char.myCharz().arrItemBag[b62].quantity = quantity2;
					if (Char.myCharz().arrItemBag[b62].quantity < quantity3 && Char.myCharz().arrItemBag[b62].template.type == 6)
						GameScr.hpPotion -= quantity3 - Char.myCharz().arrItemBag[b62].quantity;
					if (Char.myCharz().arrItemBag[b62].quantity == 0)
						Char.myCharz().arrItemBag[b62] = null;
				}
				break;
			}
			case -35:
			{
				sbyte b41 = msg.reader().readByte();
				Res.outz("cAction= " + b41);
				if (b41 == 0)
				{
					int num95 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBox = new Item[num95];
					GameCanvas.panel.hasUse = 0;
					for (int num96 = 0; num96 < num95; num96++)
					{
						short num97 = msg.reader().readShort();
						if (num97 == -1)
							continue;
						Char.myCharz().arrItemBox[num96] = new Item();
						Char.myCharz().arrItemBox[num96].template = ItemTemplates.get(num97);
						Char.myCharz().arrItemBox[num96].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBox[num96].info = msg.reader().readUTF();
						Char.myCharz().arrItemBox[num96].content = msg.reader().readUTF();
						int num98 = msg.reader().readUnsignedByte();
						if (num98 != 0)
						{
							Char.myCharz().arrItemBox[num96].itemOption = new ItemOption[num98];
							for (int num99 = 0; num99 < Char.myCharz().arrItemBox[num96].itemOption.Length; num99++)
							{
								int num100 = msg.reader().readUnsignedByte();
								int param5 = msg.reader().readUnsignedShort();
								if (num100 != -1)
									Char.myCharz().arrItemBox[num96].itemOption[num99] = new ItemOption(num100, param5);
							}
						}
						GameCanvas.panel.hasUse++;
					}
				}
				if (b41 == 1)
				{
					bool isBoxClan = false;
					try
					{
						if (msg.reader().readByte() == 1)
							isBoxClan = true;
					}
					catch (Exception)
					{
					}
					GameCanvas.panel.setTypeBox();
					GameCanvas.panel.isBoxClan = isBoxClan;
					GameCanvas.panel.show();
				}
				if (b41 == 2)
				{
					sbyte b42 = msg.reader().readByte();
					int quantity = msg.reader().readInt();
					Char.myCharz().arrItemBox[b42].quantity = quantity;
					if (Char.myCharz().arrItemBox[b42].quantity == 0)
						Char.myCharz().arrItemBox[b42] = null;
				}
				break;
			}
			case -45:
			{
				sbyte b48 = msg.reader().readByte();
				int num119 = msg.reader().readInt();
				short num120 = msg.reader().readShort();
				Res.outz("skill type= " + b48 + "   player use= " + num119);
				if (b48 == 0)
				{
					Res.outz("id use= " + num119);
					if (Char.myCharz().charID != num119)
					{
						@char = GameScr.findCharInMap(num119);
						if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
							@char.setSkillPaint(GameScr.sks[num120], 0);
						else
						{
							@char.setSkillPaint(GameScr.sks[num120], 1);
							@char.delayFall = 20;
						}
					}
					else
					{
						Char.myCharz().saveLoadPreviousSkill();
						Res.outz("LOAD LAST SKILL");
					}
					sbyte b49 = msg.reader().readByte();
					Res.outz("npc size= " + b49);
					for (int num121 = 0; num121 < b49; num121++)
					{
						sbyte b50 = msg.reader().readByte();
						sbyte b51 = msg.reader().readByte();
						Res.outz("index= " + b50);
						if (num120 >= 42 && num120 <= 48)
						{
							((Mob)GameScr.vMob.elementAt(b50)).isFreez = true;
							((Mob)GameScr.vMob.elementAt(b50)).seconds = b51;
							((Mob)GameScr.vMob.elementAt(b50)).last = (((Mob)GameScr.vMob.elementAt(b50)).cur = mSystem.currentTimeMillis());
						}
					}
					sbyte b52 = msg.reader().readByte();
					for (int num122 = 0; num122 < b52; num122++)
					{
						int num123 = msg.reader().readInt();
						sbyte b53 = msg.reader().readByte();
						Res.outz("player ID= " + num123 + " my ID= " + Char.myCharz().charID);
						if (num120 < 42 || num120 > 48)
							continue;
						if (num123 == Char.myCharz().charID)
						{
							if (!Char.myCharz().isFlyAndCharge && !Char.myCharz().isStandAndCharge)
							{
								GameScr.gI().isFreez = true;
								Char.myCharz().isFreez = true;
								Char.myCharz().freezSeconds = b53;
								Char.myCharz().lastFreez = (Char.myCharz().currFreez = mSystem.currentTimeMillis());
								Char.myCharz().isLockMove = true;
							}
						}
						else
						{
							@char = GameScr.findCharInMap(num123);
							if (@char != null && !@char.isFlyAndCharge && !@char.isStandAndCharge)
							{
								@char.isFreez = true;
								@char.seconds = b53;
								@char.freezSeconds = b53;
								@char.lastFreez = (GameScr.findCharInMap(num123).currFreez = mSystem.currentTimeMillis());
							}
						}
					}
				}
				if (b48 == 1 && num119 != Char.myCharz().charID)
					GameScr.findCharInMap(num119).isCharge = true;
				if (b48 == 3)
				{
					if (num119 == Char.myCharz().charID)
					{
						Char.myCharz().isCharge = false;
						SoundMn.gI().taitaoPause();
						Char.myCharz().saveLoadPreviousSkill();
					}
					else
						GameScr.findCharInMap(num119).isCharge = false;
				}
				if (b48 == 4)
				{
					if (num119 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort() - 1000;
						Char.myCharz().last = mSystem.currentTimeMillis();
						Res.outz("second= " + Char.myCharz().seconds + " last= " + Char.myCharz().last);
					}
					else if (GameScr.findCharInMap(num119) != null)
					{
						int cgender = GameScr.findCharInMap(num119).cgender;
						if (cgender == 0)
							GameScr.findCharInMap(num119).useChargeSkill(false);
						else if (cgender == 1)
						{
							GameScr.findCharInMap(num119).useChargeSkill(true);
						}
						GameScr.findCharInMap(num119).skillTemplateId = num120;
						GameScr.findCharInMap(num119).isUseSkillAfterCharge = true;
						GameScr.findCharInMap(num119).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num119).last = mSystem.currentTimeMillis();
					}
				}
				if (b48 == 5)
				{
					if (num119 == Char.myCharz().charID)
						Char.myCharz().stopUseChargeSkill();
					else if (GameScr.findCharInMap(num119) != null)
					{
						GameScr.findCharInMap(num119).stopUseChargeSkill();
					}
				}
				if (b48 == 6)
				{
					if (num119 == Char.myCharz().charID)
						Char.myCharz().setAutoSkillPaint(GameScr.sks[num120], 0);
					else if (GameScr.findCharInMap(num119) != null)
					{
						GameScr.findCharInMap(num119).setAutoSkillPaint(GameScr.sks[num120], 0);
						SoundMn.gI().gong();
					}
				}
				if (b48 == 7)
				{
					if (num119 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort();
						Res.outz("second = " + Char.myCharz().seconds);
						Char.myCharz().last = mSystem.currentTimeMillis();
					}
					else if (GameScr.findCharInMap(num119) != null)
					{
						GameScr.findCharInMap(num119).useChargeSkill(true);
						GameScr.findCharInMap(num119).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num119).last = mSystem.currentTimeMillis();
						SoundMn.gI().gong();
					}
				}
				if (b48 == 8 && num119 != Char.myCharz().charID && GameScr.findCharInMap(num119) != null)
					GameScr.findCharInMap(num119).setAutoSkillPaint(GameScr.sks[num120], 0);
				break;
			}
			case -44:
			{
				bool flag6 = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
					flag6 = true;
				sbyte b28 = msg.reader().readByte();
				int num65 = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop = new Item[num65][];
				GameCanvas.panel.shopTabName = new string[num65 + ((!flag6) ? 1 : 0)][];
				for (int num66 = 0; num66 < GameCanvas.panel.shopTabName.Length; num66++)
				{
					GameCanvas.panel.shopTabName[num66] = new string[2];
				}
				if (b28 == 2)
				{
					GameCanvas.panel.maxPageShop = new int[num65];
					GameCanvas.panel.currPageShop = new int[num65];
				}
				if (!flag6)
					GameCanvas.panel.shopTabName[num65] = mResources.inventory;
				for (int num67 = 0; num67 < num65; num67++)
				{
					string[] array5 = Res.split(msg.reader().readUTF(), "\n", 0);
					if (b28 == 2)
						GameCanvas.panel.maxPageShop[num67] = msg.reader().readUnsignedByte();
					if (array5.Length == 2)
						GameCanvas.panel.shopTabName[num67] = array5;
					if (array5.Length == 1)
					{
						GameCanvas.panel.shopTabName[num67][0] = array5[0];
						GameCanvas.panel.shopTabName[num67][1] = string.Empty;
					}
					int num68 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[num67] = new Item[num68];
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					if (b28 == 1)
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy2;
					for (int num69 = 0; num69 < num68; num69++)
					{
						short num70 = msg.reader().readShort();
						if (num70 == -1)
							continue;
						Char.myCharz().arrItemShop[num67][num69] = new Item();
						Char.myCharz().arrItemShop[num67][num69].template = ItemTemplates.get(num70);
						Res.outz("name " + num67 + " = " + Char.myCharz().arrItemShop[num67][num69].template.name + " id templat= " + Char.myCharz().arrItemShop[num67][num69].template.id);
						if (b28 == 8)
						{
							Char.myCharz().arrItemShop[num67][num69].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].quantity = msg.reader().readInt();
						}
						else if (b28 == 4)
						{
							Char.myCharz().arrItemShop[num67][num69].reason = msg.reader().readUTF();
						}
						else if (b28 == 0)
						{
							Char.myCharz().arrItemShop[num67][num69].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyGold = msg.reader().readInt();
						}
						else if (b28 == 1)
						{
							Char.myCharz().arrItemShop[num67][num69].powerRequire = msg.reader().readLong();
						}
						else if (b28 == 2)
						{
							Char.myCharz().arrItemShop[num67][num69].itemId = msg.reader().readShort();
							Char.myCharz().arrItemShop[num67][num69].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyType = msg.reader().readByte();
							Char.myCharz().arrItemShop[num67][num69].quantity = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].isMe = msg.reader().readByte();
						}
						else if (b28 == 3)
						{
							Char.myCharz().arrItemShop[num67][num69].isBuySpec = true;
							Char.myCharz().arrItemShop[num67][num69].iconSpec = msg.reader().readShort();
							Char.myCharz().arrItemShop[num67][num69].buySpec = msg.reader().readInt();
						}
						int num71 = msg.reader().readUnsignedByte();
						if (num71 != 0)
						{
							Char.myCharz().arrItemShop[num67][num69].itemOption = new ItemOption[num71];
							for (int num72 = 0; num72 < Char.myCharz().arrItemShop[num67][num69].itemOption.Length; num72++)
							{
								int num73 = msg.reader().readUnsignedByte();
								int param4 = msg.reader().readUnsignedShort();
								if (num73 != -1)
								{
									Char.myCharz().arrItemShop[num67][num69].itemOption[num72] = new ItemOption(num73, param4);
									Char.myCharz().arrItemShop[num67][num69].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[num67][num69]);
								}
							}
						}
						sbyte b29 = msg.reader().readByte();
						Char.myCharz().arrItemShop[num67][num69].newItem = ((b29 != 0) ? true : false);
						if (msg.reader().readByte() == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							short bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[num67][num69].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
					}
				}
				if (flag6)
				{
					if (b28 != 2)
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
						GameCanvas.panel2.setTypeBodyOnly();
						GameCanvas.panel2.show();
					}
					else
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.setTypeKiGuiOnly();
						GameCanvas.panel2.show();
					}
				}
				GameCanvas.panel.tabName[1] = GameCanvas.panel.shopTabName;
				if (b28 == 2)
				{
					string[][] array6 = GameCanvas.panel.tabName[1];
					if (flag6)
						GameCanvas.panel.tabName[1] = new string[4][]
						{
							array6[0],
							array6[1],
							array6[2],
							array6[3]
						};
					else
						GameCanvas.panel.tabName[1] = new string[5][]
						{
							array6[0],
							array6[1],
							array6[2],
							array6[3],
							array6[4]
						};
				}
				GameCanvas.panel.setTypeShop(b28);
				GameCanvas.panel.show();
				break;
			}
			case -41:
			{
				sbyte b23 = msg.reader().readByte();
				Char.myCharz().strLevel = new string[b23];
				for (int num55 = 0; num55 < b23; num55++)
				{
					string text = msg.reader().readUTF();
					Char.myCharz().strLevel[num55] = text;
				}
				Res.outz("---   xong  level caption cmd : " + msg.command);
				break;
			}
			case -34:
			{
				sbyte b12 = msg.reader().readByte();
				Res.outz("act= " + b12);
				if (b12 == 0 && GameScr.gI().magicTree != null)
				{
					Res.outz("toi duoc day");
					MagicTree magicTree = GameScr.gI().magicTree;
					magicTree.id = msg.reader().readShort();
					magicTree.name = msg.reader().readUTF();
					magicTree.name = Res.changeString(magicTree.name);
					magicTree.x = msg.reader().readShort();
					magicTree.y = msg.reader().readShort();
					magicTree.level = msg.reader().readByte();
					magicTree.currPeas = msg.reader().readShort();
					magicTree.maxPeas = msg.reader().readShort();
					Res.outz("curr Peas= " + magicTree.currPeas);
					magicTree.strInfo = msg.reader().readUTF();
					magicTree.seconds = msg.reader().readInt();
					magicTree.timeToRecieve = magicTree.seconds;
					sbyte b13 = msg.reader().readByte();
					magicTree.peaPostionX = new int[b13];
					magicTree.peaPostionY = new int[b13];
					for (int num28 = 0; num28 < b13; num28++)
					{
						magicTree.peaPostionX[num28] = msg.reader().readByte();
						magicTree.peaPostionY[num28] = msg.reader().readByte();
					}
					magicTree.isUpdate = msg.reader().readBool();
					magicTree.last = (magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
				}
				if (b12 == 1)
				{
					myVector = new MyVector();
					try
					{
						while (msg.reader().available() > 0)
						{
							myVector.addElement(new Command(msg.reader().readUTF(), GameCanvas.instance, 888392, null));
						}
					}
					catch (Exception ex4)
					{
						Cout.println("Loi MAGIC_TREE " + ex4.ToString());
					}
					GameCanvas.menu.startAt(myVector, 3);
				}
				if (b12 == 2)
				{
					GameScr.gI().magicTree.remainPeas = msg.reader().readShort();
					GameScr.gI().magicTree.seconds = msg.reader().readInt();
					GameScr.gI().magicTree.last = (GameScr.gI().magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
					GameScr.gI().magicTree.isPeasEffect = true;
				}
				break;
			}
			case 11:
			{
				GameCanvas.debug("SA9", 2);
				int num9 = msg.reader().readByte();
				sbyte b6 = msg.reader().readByte();
				if (b6 != 0)
					Mob.arrMobTemplate[num9].data.readDataNewBoss(NinjaUtil.readByteArray(msg), b6);
				else
					Mob.arrMobTemplate[num9].data.readData(NinjaUtil.readByteArray(msg));
				for (int i = 0; i < GameScr.vMob.size(); i++)
				{
					mob = (Mob)GameScr.vMob.elementAt(i);
					if (mob.templateId == num9)
					{
						mob.w = Mob.arrMobTemplate[num9].data.width;
						mob.h = Mob.arrMobTemplate[num9].data.height;
					}
				}
				sbyte[] array2 = NinjaUtil.readByteArray(msg);
				Image img = Image.createImage(array2, 0, array2.Length);
				Mob.arrMobTemplate[num9].data.img = img;
				int num10 = msg.reader().readByte();
				Mob.arrMobTemplate[num9].data.typeData = num10;
				if (num10 == 1 || num10 == 2)
					readFrameBoss(msg, num9);
				break;
			}
			case -69:
				Char.myCharz().cMaxStamina = msg.reader().readShort();
				break;
			case -68:
				Char.myCharz().cStamina = msg.reader().readShort();
				break;
			case -67:
			{
				Res.outz("RECIEVE ICON");
				demCount += 1f;
				int num151 = msg.reader().readInt();
				sbyte[] array16 = null;
				try
				{
					array16 = NinjaUtil.readByteArray(msg);
					Res.outz("request hinh icon = " + num151);
					if (num151 == 3896)
						Res.outz("SIZE CHECK= " + array16.Length);
					SmallImage.imgNew[num151].img = createImage(array16);
				}
				catch (Exception)
				{
					array16 = null;
					SmallImage.imgNew[num151].img = Image.createRGBImage(new int[1], 1, 1, true);
				}
				if (array16 != null && mGraphics.zoomLevel > 1)
					Rms.saveRMS(mGraphics.zoomLevel + "Small" + num151, array16);
				break;
			}
			case -66:
			{
				short id2 = msg.reader().readShort();
				sbyte[] data4 = NinjaUtil.readByteArray(msg);
				EffectData effDataById = Effect.getEffDataById(id2);
				sbyte b57 = msg.reader().readSByte();
				if (b57 == 0)
					effDataById.readData(data4);
				else
					effDataById.readDataNewBoss(data4, b57);
				sbyte[] array15 = NinjaUtil.readByteArray(msg);
				effDataById.img = Image.createImage(array15, 0, array15.Length);
				break;
			}
			case -32:
			{
				short num124 = msg.reader().readShort();
				int num125 = msg.reader().readInt();
				sbyte[] array13 = null;
				Image image = null;
				try
				{
					array13 = new sbyte[num125];
					for (int num126 = 0; num126 < num125; num126++)
					{
						array13[num126] = msg.reader().readByte();
					}
					image = Image.createImage(array13, 0, num125);
					BgItem.imgNew.put(num124 + string.Empty, image);
				}
				catch (Exception)
				{
					array13 = null;
					BgItem.imgNew.put(num124 + string.Empty, Image.createRGBImage(new int[1], 1, 1, true));
				}
				if (array13 != null)
				{
					if (mGraphics.zoomLevel > 1)
						Rms.saveRMS(mGraphics.zoomLevel + "bgItem" + num124, array13);
					BgItemMn.blendcurrBg(num124, image);
				}
				break;
			}
			case 92:
			{
				if (GameCanvas.currentScreen == GameScr.instance)
					GameCanvas.endDlg();
				string text4 = msg.reader().readUTF();
				string text5 = Res.changeString(msg.reader().readUTF());
				string empty = string.Empty;
				Char char6 = null;
				sbyte b39 = 0;
				if (!text4.Equals(string.Empty))
				{
					char6 = new Char();
					char6.charID = msg.reader().readInt();
					char6.head = msg.reader().readShort();
					char6.headICON = msg.reader().readShort();
					char6.body = msg.reader().readShort();
					char6.bag = msg.reader().readShort();
					char6.leg = msg.reader().readShort();
					b39 = msg.reader().readByte();
					char6.cName = text4;
				}
				empty += text5;
				InfoDlg.hide();
				if (text4.Equals(string.Empty))
				{
					GameScr.info1.addInfo(empty, 0);
					break;
				}
				GameScr.info2.addInfoWithChar(empty, char6, (b39 == 0) ? true : false);
				if (GameCanvas.panel.isShow && GameCanvas.panel.type == 8)
					GameCanvas.panel.initLogMessage();
				break;
			}
			case -26:
				ServerListScreen.testConnect = 2;
				GameCanvas.debug("SA2", 2);
				GameCanvas.startOKDlg(msg.reader().readUTF());
				InfoDlg.hide();
				LoginScr.isContinueToLogin = false;
				Char.isLoadingMap = false;
				if (GameCanvas.currentScreen == GameCanvas.loginScr)
					GameCanvas.serverScreen.switchToMe();
				break;
			case -25:
				GameCanvas.debug("SA3", 2);
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 94:
				GameCanvas.debug("SA3", 2);
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 47:
				GameCanvas.debug("SA4", 2);
				GameScr.gI().resetButton();
				break;
			case 81:
				GameCanvas.debug("SXX4", 2);
				((Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte())).isDisable = msg.reader().readBool();
				break;
			case 82:
				GameCanvas.debug("SXX5", 2);
				((Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte())).isDontMove = msg.reader().readBool();
				break;
			case 85:
				GameCanvas.debug("SXX5", 2);
				((Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte())).isFire = msg.reader().readBool();
				break;
			case 86:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob7 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob7.isIce = msg.reader().readBool();
				if (!mob7.isIce)
					ServerEffect.addServerEffect(77, mob7.x, mob7.y - 9, 1);
				break;
			}
			case 87:
				GameCanvas.debug("SXX5", 2);
				((Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte())).isWind = msg.reader().readBool();
				break;
			case 56:
			{
				GameCanvas.debug("SXX6", 2);
				@char = null;
				int num13 = msg.reader().readInt();
				if (num13 == Char.myCharz().charID)
				{
					bool flag4 = false;
					@char = Char.myCharz();
					@char.cHP = msg.readInt3Byte();
					int num30 = msg.readInt3Byte();
					Res.outz("dame hit = " + num30);
					if (num30 != 0)
						@char.doInjure();
					int num31 = 0;
					try
					{
						flag4 = msg.reader().readBoolean();
						sbyte b14 = msg.reader().readByte();
						if (b14 != -1)
						{
							Res.outz("hit eff= " + b14);
							EffecMn.addEff(new Effect(b14, @char.cx, @char.cy, 3, 1, -1));
						}
					}
					catch (Exception)
					{
					}
					num30 += num31;
					if (Char.myCharz().cTypePk != 4)
					{
						if (num30 == 0)
							GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS_ME);
						else
							GameScr.startFlyText("-" + num30, @char.cx, @char.cy - @char.ch, 0, -3, flag4 ? mFont.FATAL : mFont.RED);
					}
					break;
				}
				@char = GameScr.findCharInMap(num13);
				if (@char == null)
					return;
				@char.cHP = msg.readInt3Byte();
				bool flag5 = false;
				int num32 = msg.readInt3Byte();
				if (num32 != 0)
					@char.doInjure();
				int num33 = 0;
				try
				{
					flag5 = msg.reader().readBoolean();
					sbyte b15 = msg.reader().readByte();
					if (b15 != -1)
					{
						Res.outz("hit eff= " + b15);
						EffecMn.addEff(new Effect(b15, @char.cx, @char.cy, 3, 1, -1));
					}
				}
				catch (Exception)
				{
				}
				num32 += num33;
				if (@char.cTypePk != 4)
				{
					if (num32 == 0)
						GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS);
					else
						GameScr.startFlyText("-" + num32, @char.cx, @char.cy - @char.ch, 0, -3, flag5 ? mFont.FATAL : mFont.ORANGE);
				}
				break;
			}
			case 83:
			{
				GameCanvas.debug("SXX8", 2);
				int num13 = msg.reader().readInt();
				@char = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz());
				if (@char == null)
					return;
				Mob mobToAttack = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				if (@char.mobMe != null)
					@char.mobMe.attackOtherMob(mobToAttack);
				break;
			}
			case 84:
			{
				int num13 = msg.reader().readInt();
				if (num13 == Char.myCharz().charID)
					@char = Char.myCharz();
				else
				{
					@char = GameScr.findCharInMap(num13);
					if (@char == null)
						return;
				}
				@char.cHP = @char.cHPFull;
				@char.cMP = @char.cMPFull;
				@char.cx = msg.reader().readShort();
				@char.cy = msg.reader().readShort();
				@char.liveFromDead();
				break;
			}
			case 46:
				GameCanvas.debug("SA5", 2);
				Cout.LogWarning("Controler RESET_POINT  " + Char.ischangingMap);
				Char.isLockKey = false;
				Char.myCharz().setResetPoint(msg.reader().readShort(), msg.reader().readShort());
				break;
			case -29:
				messageNotLogin(msg);
				break;
			case -28:
				messageNotMap(msg);
				break;
			case -30:
				messageSubCommand(msg);
				break;
			case 62:
				GameCanvas.debug("SZ3", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.killCharId = Char.myCharz().charID;
					Char.myCharz().npcFocus = null;
					Char.myCharz().mobFocus = null;
					Char.myCharz().itemFocus = null;
					Char.myCharz().charFocus = @char;
					Char.isManualFocus = true;
					GameScr.info1.addInfo(@char.cName + mResources.CUU_SAT, 0);
				}
				break;
			case 63:
				GameCanvas.debug("SZ4", 2);
				Char.myCharz().killCharId = msg.reader().readInt();
				Char.myCharz().npcFocus = null;
				Char.myCharz().mobFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().charFocus = GameScr.findCharInMap(Char.myCharz().killCharId);
				Char.isManualFocus = true;
				break;
			case 64:
				GameCanvas.debug("SZ5", 2);
				@char = Char.myCharz();
				try
				{
					@char = GameScr.findCharInMap(msg.reader().readInt());
				}
				catch (Exception ex2)
				{
					Cout.println("Loi CLEAR_CUU_SAT " + ex2.ToString());
				}
				@char.killCharId = -9999;
				break;
			case 39:
				GameCanvas.debug("SA49", 2);
				GameScr.gI().typeTradeOrder = 2;
				if (GameScr.gI().typeTrade >= 2 && GameScr.gI().typeTradeOrder >= 2)
					InfoDlg.showWait();
				break;
			case 57:
			{
				GameCanvas.debug("SZ6", 2);
				MyVector myVector2 = new MyVector();
				myVector2.addElement(new Command(msg.reader().readUTF(), GameCanvas.instance, 88817, null));
				GameCanvas.menu.startAt(myVector2, 3);
				break;
			}
			case 58:
			{
				GameCanvas.debug("SZ7", 2);
				int num13 = msg.reader().readInt();
				Char char10 = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz());
				char10.moveFast = new short[3];
				char10.moveFast[0] = 0;
				short num153 = msg.reader().readShort();
				short num154 = msg.reader().readShort();
				char10.moveFast[1] = num153;
				char10.moveFast[2] = num154;
				try
				{
					num13 = msg.reader().readInt();
					Char char11 = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz());
					char11.cx = num153;
					char11.cy = num154;
				}
				catch (Exception ex21)
				{
					Cout.println("Loi MOVE_FAST " + ex21.ToString());
				}
				break;
			}
			case 88:
			{
				string info4 = msg.reader().readUTF();
				short num152 = msg.reader().readShort();
				GameCanvas.inputDlg.show(info4, new Command(mResources.ACCEPT, GameCanvas.instance, 88818, num152), TField.INPUT_TYPE_ANY);
				break;
			}
			case 27:
			{
				myVector = new MyVector();
				string text9 = msg.reader().readUTF();
				int num143 = msg.reader().readByte();
				for (int num144 = 0; num144 < num143; num144++)
				{
					myVector.addElement(new Command(msg.reader().readUTF(), p: msg.reader().readShort(), actionListener: GameCanvas.instance, action: 88819));
				}
				GameCanvas.menu.startWithoutCloseButton(myVector, 3);
				break;
			}
			case 33:
			{
				GameCanvas.debug("SA51", 2);
				InfoDlg.hide();
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				myVector = new MyVector();
				try
				{
					while (true)
					{
						myVector.addElement(new Command(msg.reader().readUTF(), GameCanvas.instance, 88822, null));
					}
				}
				catch (Exception ex19)
				{
					Cout.println("Loi OPEN_UI_MENU " + ex19.ToString());
				}
				if (Char.myCharz().npcFocus == null)
					return;
				for (int num135 = 0; num135 < Char.myCharz().npcFocus.template.menu.Length; num135++)
				{
					string[] array14 = Char.myCharz().npcFocus.template.menu[num135];
					myVector.addElement(new Command(array14[0], GameCanvas.instance, 88820, array14));
				}
				GameCanvas.menu.startAt(myVector, 3);
				break;
			}
			case 40:
			{
				GameCanvas.debug("SA52", 2);
				GameCanvas.taskTick = 150;
				short taskId = msg.reader().readShort();
				sbyte index3 = msg.reader().readByte();
				string name2 = Res.changeString(msg.reader().readUTF());
				string detail = Res.changeString(msg.reader().readUTF());
				string[] array10 = new string[msg.reader().readByte()];
				string[] array11 = new string[array10.Length];
				GameScr.tasks = new int[array10.Length];
				GameScr.mapTasks = new int[array10.Length];
				short[] array12 = new short[array10.Length];
				short count = -1;
				for (int num117 = 0; num117 < array10.Length; num117++)
				{
					string text7 = Res.changeString(msg.reader().readUTF());
					GameScr.tasks[num117] = msg.reader().readByte();
					GameScr.mapTasks[num117] = msg.reader().readShort();
					string text8 = Res.changeString(msg.reader().readUTF());
					array12[num117] = -1;
					if (!text7.Equals(string.Empty))
					{
						array10[num117] = text7;
						array11[num117] = text8;
					}
				}
				try
				{
					count = msg.reader().readShort();
					for (int num118 = 0; num118 < array10.Length; num118++)
					{
						array12[num118] = msg.reader().readShort();
					}
				}
				catch (Exception ex17)
				{
					Cout.println("Loi TASK_GET " + ex17.ToString());
				}
				Char.myCharz().taskMaint = new Task(taskId, index3, name2, detail, array10, array12, count, array11);
				if (Char.myCharz().npcFocus != null)
					Npc.clearEffTask();
				Char.taskAction(false);
				break;
			}
			case 41:
				GameCanvas.debug("SA53", 2);
				GameCanvas.taskTick = 100;
				Res.outz("TASK NEXT");
				Char.myCharz().taskMaint.index++;
				Char.myCharz().taskMaint.count = 0;
				Npc.clearEffTask();
				Char.taskAction(true);
				break;
			case 50:
			{
				sbyte b45 = msg.reader().readByte();
				Panel.vGameInfo.removeAllElements();
				for (int num112 = 0; num112 < b45; num112++)
				{
					GameInfo gameInfo = new GameInfo();
					gameInfo.id = msg.reader().readShort();
					gameInfo.main = msg.reader().readUTF();
					gameInfo.content = msg.reader().readUTF();
					Panel.vGameInfo.addElement(gameInfo);
					gameInfo.hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1;
				}
				break;
			}
			case 43:
				GameCanvas.taskTick = 50;
				GameCanvas.debug("SA55", 2);
				Char.myCharz().taskMaint.count = msg.reader().readShort();
				if (Char.myCharz().npcFocus != null)
					Npc.clearEffTask();
				break;
			case 90:
				GameCanvas.debug("SA577", 2);
				requestItemPlayer(msg);
				break;
			case 29:
				GameCanvas.debug("SA58", 2);
				GameScr.gI().openUIZone(msg);
				break;
			case -21:
			{
				GameCanvas.debug("SA60", 2);
				short itemMapID = msg.reader().readShort();
				for (int num109 = 0; num109 < GameScr.vItemMap.size(); num109++)
				{
					if (((ItemMap)GameScr.vItemMap.elementAt(num109)).itemMapID == itemMapID)
					{
						GameScr.vItemMap.removeElementAt(num109);
						break;
					}
				}
				break;
			}
			case -20:
			{
				GameCanvas.debug("SA61", 2);
				Char.myCharz().itemFocus = null;
				short itemMapID = msg.reader().readShort();
				for (int num106 = 0; num106 < GameScr.vItemMap.size(); num106++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(num106);
					if (itemMap2.itemMapID != itemMapID)
						continue;
					itemMap2.setPoint(Char.myCharz().cx, Char.myCharz().cy - 10);
					string text6 = msg.reader().readUTF();
					num = 0;
					try
					{
						num = msg.reader().readShort();
						if (itemMap2.template.type == 9)
						{
							num = msg.reader().readShort();
							Char.myCharz().xu += num;
							Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
						}
						else if (itemMap2.template.type == 10)
						{
							num = msg.reader().readShort();
							Char.myCharz().luong += num;
							Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
						}
						else if (itemMap2.template.type == 34)
						{
							num = msg.reader().readShort();
							Char.myCharz().luongKhoa += num;
							Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
						}
					}
					catch (Exception)
					{
					}
					if (text6.Equals(string.Empty))
					{
						if (itemMap2.template.type == 9)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.YELLOW);
							SoundMn.gI().getItem();
						}
						else if (itemMap2.template.type == 10)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.GREEN);
							SoundMn.gI().getItem();
						}
						else if (itemMap2.template.type == 34)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.RED);
							SoundMn.gI().getItem();
						}
						else
						{
							GameScr.info1.addInfo(mResources.you_receive + " " + ((num <= 0) ? string.Empty : (num + " ")) + itemMap2.template.name, 0);
							SoundMn.gI().getItem();
						}
						if (num > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 4683)
						{
							ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
							ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
						}
					}
					else if (text6.Length == 1)
					{
						Cout.LogError3("strInf.Length =1:  " + text6);
					}
					else
					{
						GameScr.info1.addInfo(text6, 0);
					}
					break;
				}
				break;
			}
			case -19:
			{
				GameCanvas.debug("SA62", 2);
				short itemMapID = msg.reader().readShort();
				@char = GameScr.findCharInMap(msg.reader().readInt());
				for (int num103 = 0; num103 < GameScr.vItemMap.size(); num103++)
				{
					ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(num103);
					if (itemMap.itemMapID != itemMapID)
						continue;
					if (@char == null)
						return;
					itemMap.setPoint(@char.cx, @char.cy - 10);
					if (itemMap.x < @char.cx)
						@char.cdir = -1;
					else if (itemMap.x > @char.cx)
					{
						@char.cdir = 1;
					}
					break;
				}
				break;
			}
			case -18:
			{
				GameCanvas.debug("SA63", 2);
				int num102 = msg.reader().readByte();
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), Char.myCharz().arrItemBag[num102].template.id, Char.myCharz().cx, Char.myCharz().cy, msg.reader().readShort(), msg.reader().readShort()));
				Char.myCharz().arrItemBag[num102] = null;
				break;
			}
			case 68:
			{
				Res.outz("ADD ITEM TO MAP --------------------------------------");
				GameCanvas.debug("SA6333", 2);
				short itemMapID = msg.reader().readShort();
				short itemTemplateID = msg.reader().readShort();
				int x = msg.reader().readShort();
				int y = msg.reader().readShort();
				int num94 = msg.reader().readInt();
				short r = 0;
				if (num94 == -2)
					r = msg.reader().readShort();
				ItemMap o = new ItemMap(num94, itemMapID, itemTemplateID, x, y, r);
				GameScr.vItemMap.addElement(o);
				break;
			}
			case 69:
				SoundMn.IsDelAcc = ((msg.reader().readByte() != 0) ? true : false);
				break;
			case -14:
				GameCanvas.debug("SA64", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
					return;
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), msg.reader().readShort(), @char.cx, @char.cy, msg.reader().readShort(), msg.reader().readShort()));
				break;
			case -22:
				GameCanvas.debug("SA65", 2);
				Char.isLockKey = true;
				Char.ischangingMap = true;
				GameScr.gI().timeStartMap = 0;
				GameScr.gI().timeLengthMap = 0;
				Char.myCharz().mobFocus = null;
				Char.myCharz().npcFocus = null;
				Char.myCharz().charFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().focus.removeAllElements();
				Char.myCharz().testCharId = -9999;
				Char.myCharz().killCharId = -9999;
				GameCanvas.resetBg();
				GameScr.gI().resetButton();
				GameScr.gI().center = null;
				break;
			case -70:
			{
				Res.outz("BIG MESSAGE .......................................");
				GameCanvas.endDlg();
				int avatar = msg.reader().readShort();
				string chat4 = msg.reader().readUTF();
				Npc npc6 = new Npc(-1, 0, 0, 0, 0, 0);
				npc6.avatar = avatar;
				ChatPopup.addBigMessage(chat4, 100000, npc6);
				sbyte b37 = msg.reader().readByte();
				if (b37 == 0)
				{
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 35;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
				}
				if (b37 == 1)
				{
					string p = msg.reader().readUTF();
					string caption = msg.reader().readUTF();
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(caption, ChatPopup.serverChatPopUp, 1000, p);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 75;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
					ChatPopup.serverChatPopUp.cmdMsg2 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg2.x = GameCanvas.w / 2 + 11;
					ChatPopup.serverChatPopUp.cmdMsg2.y = GameCanvas.h - 35;
				}
				break;
			}
			case 38:
			{
				GameCanvas.debug("SA67", 2);
				InfoDlg.hide();
				int num76 = msg.reader().readShort();
				Res.outz("OPEN_UI_SAY ID= " + num76);
				string chat3 = Res.changeString(msg.reader().readUTF());
				for (int num88 = 0; num88 < GameScr.vNpc.size(); num88++)
				{
					Npc npc4 = (Npc)GameScr.vNpc.elementAt(num88);
					Res.outz("npc id= " + npc4.template.npcTemplateId);
					if (npc4.template.npcTemplateId == num76)
					{
						ChatPopup.addChatPopupMultiLine(chat3, 100000, npc4);
						GameCanvas.panel.hideNow();
						return;
					}
				}
				Npc npc5 = new Npc(num76, 0, 0, 0, num76, GameScr.info1.charId[Char.myCharz().cgender][2]);
				if (npc5.template.npcTemplateId == 5)
					npc5.charID = 5;
				try
				{
					npc5.avatar = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				ChatPopup.addChatPopupMultiLine(chat3, 100000, npc5);
				GameCanvas.panel.hideNow();
				break;
			}
			case 32:
			{
				GameCanvas.debug("SA68", 2);
				int num76 = msg.reader().readShort();
				for (int num77 = 0; num77 < GameScr.vNpc.size(); num77++)
				{
					Npc npc = (Npc)GameScr.vNpc.elementAt(num77);
					if (npc.template.npcTemplateId == num76 && npc.Equals(Char.myCharz().npcFocus))
					{
						string chat = msg.reader().readUTF();
						string[] array7 = new string[msg.reader().readByte()];
						for (int num78 = 0; num78 < array7.Length; num78++)
						{
							array7[num78] = msg.reader().readUTF();
						}
						GameScr.gI().createMenu(array7, npc);
						ChatPopup.addChatPopup(chat, 100000, npc);
						return;
					}
				}
				Npc npc2 = new Npc(num76, 0, -100, 100, num76, GameScr.info1.charId[Char.myCharz().cgender][2]);
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				string chat2 = msg.reader().readUTF();
				string[] array8 = new string[msg.reader().readByte()];
				for (int num79 = 0; num79 < array8.Length; num79++)
				{
					array8[num79] = msg.reader().readUTF();
				}
				try
				{
					npc2.avatar = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				GameScr.gI().createMenu(array8, npc2);
				ChatPopup.addChatPopup(chat2, 100000, npc2);
				break;
			}
			case 7:
			{
				sbyte type = msg.reader().readByte();
				short id = msg.reader().readShort();
				string info2 = msg.reader().readUTF();
				GameCanvas.panel.saleRequest(type, info2, id);
				break;
			}
			case 6:
				GameCanvas.debug("SA70", 2);
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				GameCanvas.endDlg();
				break;
			case -24:
				Char.isLoadingMap = true;
				Cout.println("GET MAP INFO");
				GameScr.gI().magicTree = null;
				GameCanvas.isLoading = true;
				GameCanvas.debug("SA75", 2);
				GameScr.resetAllvector();
				GameCanvas.endDlg();
				TileMap.vGo.removeAllElements();
				PopUp.vPopups.removeAllElements();
				mSystem.gcc();
				TileMap.mapID = msg.reader().readUnsignedByte();
				TileMap.planetID = msg.reader().readByte();
				TileMap.tileID = msg.reader().readByte();
				TileMap.bgID = msg.reader().readByte();
				Cout.println("load planet from server: " + TileMap.planetID + "bgType= " + TileMap.bgType + ".............................");
				TileMap.typeMap = msg.reader().readByte();
				TileMap.mapName = msg.reader().readUTF();
				TileMap.zoneID = msg.reader().readByte();
				GameCanvas.debug("SA75x1", 2);
				try
				{
					TileMap.loadMapFromResource(TileMap.mapID);
				}
				catch (Exception)
				{
					Service.gI().requestMaptemplate(TileMap.mapID);
					messWait = msg;
					return;
				}
				loadInfoMap(msg);
				try
				{
					TileMap.isMapDouble = ((msg.reader().readByte() != 0) ? true : false);
				}
				catch (Exception)
				{
				}
				GameScr.cmx = GameScr.cmtoX;
				GameScr.cmy = GameScr.cmtoY;
				break;
			case -31:
			{
				TileMap.vItemBg.removeAllElements();
				short num62 = msg.reader().readShort();
				Cout.LogError2("nItem= " + num62);
				for (int num63 = 0; num63 < num62; num63++)
				{
					BgItem bgItem = new BgItem();
					bgItem.id = num63;
					bgItem.idImage = msg.reader().readShort();
					bgItem.layer = msg.reader().readByte();
					bgItem.dx = msg.reader().readShort();
					bgItem.dy = msg.reader().readShort();
					sbyte b27 = msg.reader().readByte();
					bgItem.tileX = new int[b27];
					bgItem.tileY = new int[b27];
					for (int num64 = 0; num64 < b27; num64++)
					{
						bgItem.tileX[num63] = msg.reader().readByte();
						bgItem.tileY[num63] = msg.reader().readByte();
					}
					TileMap.vItemBg.addElement(bgItem);
				}
				break;
			}
			case -4:
			{
				GameCanvas.debug("SA76", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
					return;
				GameCanvas.debug("SA76v1", 2);
				if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
					@char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 0);
				else
					@char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 1);
				GameCanvas.debug("SA76v2", 2);
				@char.attMobs = new Mob[msg.reader().readByte()];
				for (int num29 = 0; num29 < @char.attMobs.Length; num29++)
				{
					Mob mob3 = (Mob)GameScr.vMob.elementAt(msg.reader().readByte());
					@char.attMobs[num29] = mob3;
					if (num29 == 0)
					{
						if (@char.cx <= mob3.x)
							@char.cdir = 1;
						else
							@char.cdir = -1;
					}
				}
				GameCanvas.debug("SA76v3", 2);
				@char.charFocus = null;
				@char.mobFocus = @char.attMobs[0];
				Char[] array = new Char[10];
				num = 0;
				try
				{
					for (num = 0; num < array.Length; num++)
					{
						int num13 = msg.reader().readInt();
						Char char4 = (array[num] = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz()));
						if (num == 0)
						{
							if (@char.cx <= char4.cx)
								@char.cdir = 1;
							else
								@char.cdir = -1;
						}
					}
				}
				catch (Exception ex5)
				{
					Cout.println("Loi PLAYER_ATTACK_N_P " + ex5.ToString());
				}
				GameCanvas.debug("SA76v4", 2);
				if (num > 0)
				{
					@char.attChars = new Char[num];
					for (num = 0; num < @char.attChars.Length; num++)
					{
						@char.attChars[num] = array[num];
					}
					@char.charFocus = @char.attChars[0];
					@char.mobFocus = null;
				}
				GameCanvas.debug("SA76v5", 2);
				break;
			}
			case 54:
			{
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
					return;
				int num14 = msg.reader().readUnsignedByte();
				if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
					@char.setSkillPaint(GameScr.sks[num14], 0);
				else
					@char.setSkillPaint(GameScr.sks[num14], 1);
				GameCanvas.debug("SA769991v2", 2);
				Mob[] array3 = new Mob[10];
				num = 0;
				try
				{
					GameCanvas.debug("SA769991v3", 2);
					for (num = 0; num < array3.Length; num++)
					{
						GameCanvas.debug("SA769991v4-num" + num, 2);
						Mob mob2 = (array3[num] = (Mob)GameScr.vMob.elementAt(msg.reader().readByte()));
						if (num == 0)
						{
							if (@char.cx <= mob2.x)
								@char.cdir = 1;
							else
								@char.cdir = -1;
						}
						GameCanvas.debug("SA769991v5-num" + num, 2);
					}
				}
				catch (Exception ex3)
				{
					Cout.println("Loi PLAYER_ATTACK_NPC " + ex3.ToString());
				}
				GameCanvas.debug("SA769992", 2);
				if (num > 0)
				{
					@char.attMobs = new Mob[num];
					for (num = 0; num < @char.attMobs.Length; num++)
					{
						@char.attMobs[num] = array3[num];
					}
					@char.charFocus = null;
					@char.mobFocus = @char.attMobs[0];
				}
				break;
			}
			case -60:
			{
				GameCanvas.debug("SA7666", 2);
				int num2 = msg.reader().readInt();
				int num3 = -1;
				if (num2 != Char.myCharz().charID)
				{
					Char char2 = GameScr.findCharInMap(num2);
					if (char2 == null)
						return;
					if (char2.currentMovePoint != null)
					{
						char2.createShadow(char2.cx, char2.cy, 10);
						char2.cx = char2.currentMovePoint.xEnd;
						char2.cy = char2.currentMovePoint.yEnd;
					}
					int num4 = msg.reader().readUnsignedByte();
					Res.outz("player skill ID= " + num4);
					if ((TileMap.tileTypeAtPixel(char2.cx, char2.cy) & 2) == 2)
						char2.setSkillPaint(GameScr.sks[num4], 0);
					else
						char2.setSkillPaint(GameScr.sks[num4], 1);
					sbyte b = msg.reader().readByte();
					Res.outz("nAttack = " + b);
					Char[] array = new Char[b];
					for (num = 0; num < array.Length; num++)
					{
						num3 = msg.reader().readInt();
						Char char3;
						if (num3 == Char.myCharz().charID)
						{
							char3 = Char.myCharz();
							if (!GameScr.isChangeZone && GameScr.isAutoPlay && GameScr.canAutoPlay)
							{
								Service.gI().requestChangeZone(-1, -1);
								GameScr.isChangeZone = true;
							}
						}
						else
							char3 = GameScr.findCharInMap(num3);
						array[num] = char3;
						if (num == 0)
						{
							if (char2.cx <= char3.cx)
								char2.cdir = 1;
							else
								char2.cdir = -1;
						}
					}
					if (num > 0)
					{
						char2.attChars = new Char[num];
						for (num = 0; num < char2.attChars.Length; num++)
						{
							char2.attChars[num] = array[num];
						}
						char2.mobFocus = null;
						char2.charFocus = char2.attChars[0];
					}
				}
				else
				{
					sbyte b2 = msg.reader().readByte();
					sbyte b3 = msg.reader().readByte();
					num3 = msg.reader().readInt();
				}
				try
				{
					sbyte b4 = msg.reader().readByte();
					Res.outz("isRead continue = " + b4);
					if (b4 != 1)
						break;
					sbyte b5 = msg.reader().readByte();
					Res.outz("type skill = " + b5);
					if (num3 == Char.myCharz().charID)
					{
						bool flag = false;
						@char = Char.myCharz();
						int num5 = msg.readInt3Byte();
						Res.outz("dame hit = " + num5);
						@char.isDie = msg.reader().readBoolean();
						if (@char.isDie)
							Char.isLockKey = true;
						Res.outz("isDie=" + @char.isDie + "---------------------------------------");
						int num6 = 0;
						flag = (@char.isCrit = msg.reader().readBoolean());
						@char.isMob = false;
						num5 = (@char.damHP = num5 + num6);
						if (b5 == 0)
							@char.doInjure(num5, 0, flag, false);
					}
					else
					{
						@char = GameScr.findCharInMap(num3);
						if (@char == null)
							return;
						bool flag2 = false;
						int num7 = msg.readInt3Byte();
						Res.outz("dame hit= " + num7);
						@char.isDie = msg.reader().readBoolean();
						Res.outz("isDie=" + @char.isDie + "---------------------------------------");
						int num8 = 0;
						flag2 = (@char.isCrit = msg.reader().readBoolean());
						@char.isMob = false;
						num7 = (@char.damHP = num7 + num8);
						if (b5 == 0)
							@char.doInjure(num7, 0, flag2, false);
					}
				}
				catch (Exception)
				{
				}
				break;
			}
			case -58:
			case 78:
			case 79:
				break;
			}
			sbyte command2 = msg.command;
			switch (command2)
			{
			default:
				switch (command2)
				{
				default:
					switch (command2)
					{
					default:
					{
						if (command2 != 18)
						{
							if (command2 != 19)
							{
								if (command2 != 44)
								{
									if (command2 != 45)
									{
										if (command2 != 66)
										{
											if (command2 != 74)
												break;
											GameCanvas.debug("SA85", 2);
											Mob mob9 = null;
											try
											{
												mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
											}
											catch (Exception)
											{
												Cout.println("Loi tai NPC CHANGE " + msg.command);
											}
											if (mob9 != null && mob9.status != 0 && mob9.status != 0)
											{
												mob9.status = 0;
												ServerEffect.addServerEffect(60, mob9.x, mob9.y, 1);
												ItemMap itemMap3 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
												GameScr.vItemMap.addElement(itemMap3);
												if (Res.abs(itemMap3.y - Char.myCharz().cy) < 24 && Res.abs(itemMap3.x - Char.myCharz().cx) < 24)
													Char.myCharz().charFocus = null;
											}
										}
										else
											Res.outz("ME DIE XP DOWN NOT IMPLEMENT YET!!!!!!!!!!!!!!!!!!!!!!!!!!");
									}
									else
									{
										GameCanvas.debug("SA84", 2);
										Mob mob9 = null;
										try
										{
											mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
										}
										catch (Exception ex28)
										{
											Cout.println("Loi tai NPC_MISS  " + ex28.ToString());
										}
										if (mob9 != null)
										{
											mob9.hp = msg.reader().readInt();
											mob9.updateHp_bar();
											GameScr.startFlyText(mResources.miss, mob9.x, mob9.y - mob9.h, 0, -2, mFont.MISS);
										}
									}
								}
								else
								{
									GameCanvas.debug("SA91", 2);
									int num169 = msg.reader().readInt();
									string text11 = msg.reader().readUTF();
									Res.outz("user id= " + num169 + " text= " + text11);
									@char = ((Char.myCharz().charID != num169) ? GameScr.findCharInMap(num169) : Char.myCharz());
									if (@char == null)
										return;
									@char.addInfo(text11);
								}
							}
							else
							{
								Char.myCharz().countKill = msg.reader().readUnsignedShort();
								Char.myCharz().countKillMax = msg.reader().readUnsignedShort();
							}
							break;
						}
						sbyte b67 = msg.reader().readByte();
						for (int num170 = 0; num170 < b67; num170++)
						{
							int charId = msg.reader().readInt();
							int cx = msg.reader().readShort();
							int cy = msg.reader().readShort();
							int cHPShow = msg.readInt3Byte();
							Char char12 = GameScr.findCharInMap(charId);
							if (char12 != null)
							{
								char12.cx = cx;
								char12.cy = cy;
								char12.cHP = (char12.cHPShow = cHPShow);
								char12.lastUpdateTime = mSystem.currentTimeMillis();
							}
						}
						break;
					}
					case -73:
					{
						sbyte b68 = msg.reader().readByte();
						for (int num171 = 0; num171 < GameScr.vNpc.size(); num171++)
						{
							Npc npc7 = (Npc)GameScr.vNpc.elementAt(num171);
							if (npc7.template.npcTemplateId == b68)
							{
								if (msg.reader().readByte() == 0)
									npc7.isHide = true;
								else
									npc7.isHide = false;
								break;
							}
						}
						break;
					}
					case -75:
					{
						Mob mob9 = null;
						try
						{
							mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
						}
						catch (Exception)
						{
						}
						if (mob9 != null)
						{
							mob9.levelBoss = msg.reader().readByte();
							if (mob9.levelBoss > 0)
								mob9.typeSuperEff = Res.random(0, 3);
						}
						break;
					}
					}
					break;
				case 95:
				{
					GameCanvas.debug("SA77", 22);
					int num172 = msg.reader().readInt();
					Char.myCharz().xu += num172;
					Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
					GameScr.startFlyText((num172 <= 0) ? (string.Empty + num172) : ("+" + num172), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
					break;
				}
				case 96:
					GameCanvas.debug("SA77a", 22);
					Char.myCharz().taskOrders.addElement(new TaskOrder(msg.reader().readByte(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readUTF(), msg.reader().readUTF(), msg.reader().readByte(), msg.reader().readByte()));
					break;
				case 97:
				{
					sbyte b66 = msg.reader().readByte();
					for (int num168 = 0; num168 < Char.myCharz().taskOrders.size(); num168++)
					{
						TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(num168);
						if (taskOrder.taskId == b66)
						{
							taskOrder.count = msg.reader().readShort();
							break;
						}
					}
					break;
				}
				}
				break;
			case -2:
			{
				GameCanvas.debug("SA77", 22);
				int num177 = msg.reader().readInt();
				Char.myCharz().yen += num177;
				GameScr.startFlyText((num177 <= 0) ? (string.Empty + num177) : ("+" + num177), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case -1:
			{
				GameCanvas.debug("SA77", 222);
				int num181 = msg.reader().readInt();
				Char.myCharz().xu += num181;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().yen -= num181;
				GameScr.startFlyText("+" + num181, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case -3:
			{
				GameCanvas.debug("SA78", 2);
				sbyte b65 = msg.reader().readByte();
				int num167 = msg.reader().readInt();
				if (b65 == 0)
					Char.myCharz().cPower += num167;
				if (b65 == 1)
					Char.myCharz().cTiemNang += num167;
				if (b65 == 2)
				{
					Char.myCharz().cPower += num167;
					Char.myCharz().cTiemNang += num167;
				}
				Char.myCharz().applyCharLevelPercent();
				if (Char.myCharz().cTypePk != 3)
				{
					GameScr.startFlyText(((num167 <= 0) ? string.Empty : "+") + num167, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -4, mFont.GREEN);
					if (num167 > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5002)
					{
						ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
						ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
					}
				}
				break;
			}
			case -5:
			{
				GameCanvas.debug("SA79", 2);
				int charID = msg.reader().readInt();
				int num182 = msg.reader().readInt();
				Char char14;
				if (num182 != -100)
				{
					char14 = new Char();
					char14.charID = charID;
					char14.clanID = num182;
				}
				else
				{
					char14 = new Mabu();
					char14.charID = charID;
					char14.clanID = num182;
				}
				if (char14.clanID == -2)
					char14.isCopy = true;
				if (readCharInfo(char14, msg))
				{
					sbyte b70 = msg.reader().readByte();
					if (char14.cy <= 10 && b70 != 0 && b70 != 2)
					{
						Res.outz("nhân vật bay trên trời xuống x= " + char14.cx + " y= " + char14.cy);
						Teleport teleport2 = new Teleport(char14.cx, char14.cy, char14.head, char14.cdir, 1, false, (b70 != 1) ? b70 : char14.cgender);
						teleport2.id = char14.charID;
						char14.isTeleport = true;
						Teleport.addTeleport(teleport2);
					}
					if (b70 == 2)
						char14.show();
					for (int num183 = 0; num183 < GameScr.vMob.size(); num183++)
					{
						Mob mob10 = (Mob)GameScr.vMob.elementAt(num183);
						if (mob10 != null && mob10.isMobMe && mob10.mobId == char14.charID)
						{
							Res.outz("co 1 con quai");
							char14.mobMe = mob10;
							char14.mobMe.x = char14.cx;
							char14.mobMe.y = char14.cy - 40;
							break;
						}
					}
					if (GameScr.findCharInMap(char14.charID) == null)
						GameScr.vCharInMap.addElement(char14);
					char14.isMonkey = msg.reader().readByte();
					short num184 = msg.reader().readShort();
					Res.outz("mount id= " + num184 + "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
					if (num184 != -1)
					{
						char14.isHaveMount = true;
						if (num184 == 346 || num184 == 347 || num184 == 348)
							char14.isMountVip = false;
						else if (num184 == 349 || num184 == 350 || num184 == 351)
						{
							char14.isMountVip = true;
						}
						else if (num184 == 396)
						{
							char14.isEventMount = true;
						}
						else if (num184 == 532)
						{
							char14.isSpeacialMount = true;
						}
						else if (num184 >= Char.ID_NEW_MOUNT)
						{
							char14.idMount = num184;
						}
					}
					else
						char14.isHaveMount = false;
				}
				sbyte b71 = msg.reader().readByte();
				Res.outz("addplayer:   " + b71);
				char14.cFlag = b71;
				char14.isNhapThe = msg.reader().readByte() == 1;
				try
				{
					char14.idAuraEff = msg.reader().readShort();
					char14.idEff_Set_Item = msg.reader().readSByte();
					char14.idHat = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				GameScr.gI().getFlagImage(char14.charID, char14.cFlag);
				break;
			}
			case -7:
			{
				GameCanvas.debug("SA80", 2);
				int num173 = msg.reader().readInt();
				Cout.println("RECEVED MOVE OF " + num173);
				for (int num174 = 0; num174 < GameScr.vCharInMap.size(); num174++)
				{
					Char char13 = null;
					try
					{
						char13 = (Char)GameScr.vCharInMap.elementAt(num174);
					}
					catch (Exception ex30)
					{
						Cout.println("Loi PLAYER_MOVE " + ex30.ToString());
					}
					if (char13 == null)
						break;
					if (char13.charID == num173)
					{
						GameCanvas.debug("SA8x2y" + num174, 2);
						char13.moveTo(msg.reader().readShort(), msg.reader().readShort(), 0);
						char13.lastUpdateTime = mSystem.currentTimeMillis();
						break;
					}
				}
				GameCanvas.debug("SA80x3", 2);
				break;
			}
			case -6:
			{
				GameCanvas.debug("SA81", 2);
				int num173 = msg.reader().readInt();
				for (int num185 = 0; num185 < GameScr.vCharInMap.size(); num185++)
				{
					Char char15 = (Char)GameScr.vCharInMap.elementAt(num185);
					if (char15 != null && char15.charID == num173)
					{
						if (!char15.isInvisiblez && !char15.isUsePlane)
							ServerEffect.addServerEffect(60, char15.cx, char15.cy, 1);
						if (!char15.isUsePlane)
							GameScr.vCharInMap.removeElementAt(num185);
						return;
					}
				}
				break;
			}
			case -13:
			{
				GameCanvas.debug("SA82", 2);
				int num176 = msg.reader().readUnsignedByte();
				if (num176 > GameScr.vMob.size() - 1 || num176 < 0)
					return;
				Mob mob9 = (Mob)GameScr.vMob.elementAt(num176);
				mob9.sys = msg.reader().readByte();
				mob9.levelBoss = msg.reader().readByte();
				if (mob9.levelBoss != 0)
					mob9.typeSuperEff = Res.random(0, 3);
				mob9.x = mob9.xFirst;
				mob9.y = mob9.yFirst;
				mob9.status = 5;
				mob9.injureThenDie = false;
				mob9.hp = msg.reader().readInt();
				mob9.maxHp = mob9.hp;
				mob9.updateHp_bar();
				ServerEffect.addServerEffect(60, mob9.x, mob9.y, 1);
				break;
			}
			case -9:
			{
				GameCanvas.debug("SA83", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				GameCanvas.debug("SA83v1", 2);
				if (mob9 != null)
				{
					mob9.hp = msg.readInt3Byte();
					mob9.updateHp_bar();
					int num164 = msg.readInt3Byte();
					if (num164 == 1)
						return;
					bool flag10 = false;
					try
					{
						flag10 = msg.reader().readBoolean();
					}
					catch (Exception)
					{
					}
					sbyte b64 = msg.reader().readByte();
					if (b64 != -1)
						EffecMn.addEff(new Effect(b64, mob9.x, mob9.getY(), 3, 1, -1));
					GameCanvas.debug("SA83v2", 2);
					if (flag10)
						GameScr.startFlyText("-" + num164, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.FATAL);
					else if (num164 == 0)
					{
						mob9.x = mob9.xFirst;
						mob9.y = mob9.yFirst;
						GameScr.startFlyText(mResources.miss, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num164, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.ORANGE);
					}
				}
				GameCanvas.debug("SA83v3", 2);
				break;
			}
			case -12:
			{
				Res.outz("SERVER SEND MOB DIE");
				GameCanvas.debug("SA85", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
					Cout.println("LOi tai NPC_DIE cmd " + msg.command);
				}
				if (mob9 == null || mob9.status == 0 || mob9.status == 0)
					break;
				mob9.startDie();
				try
				{
					int num178 = msg.readInt3Byte();
					if (msg.reader().readBool())
						GameScr.startFlyText("-" + num178, mob9.x, mob9.y - mob9.h, 0, -2, mFont.FATAL);
					else
						GameScr.startFlyText("-" + num178, mob9.x, mob9.y - mob9.h, 0, -2, mFont.ORANGE);
					sbyte b69 = msg.reader().readByte();
					for (int num179 = 0; num179 < b69; num179++)
					{
						ItemMap itemMap4 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
						int num180 = (itemMap4.playerId = msg.reader().readInt());
						Res.outz("playerid= " + num180 + " my id= " + Char.myCharz().charID);
						GameScr.vItemMap.addElement(itemMap4);
						if (Res.abs(itemMap4.y - Char.myCharz().cy) < 24 && Res.abs(itemMap4.x - Char.myCharz().cx) < 24)
							Char.myCharz().charFocus = null;
					}
				}
				catch (Exception ex34)
				{
					Cout.println("LOi tai NPC_DIE " + ex34.ToString() + " cmd " + msg.command);
				}
				break;
			}
			case -11:
			{
				GameCanvas.debug("SA86", 2);
				Mob mob9 = null;
				try
				{
					byte index4 = msg.reader().readUnsignedByte();
					mob9 = (Mob)GameScr.vMob.elementAt(index4);
				}
				catch (Exception)
				{
					Cout.println("Loi tai NPC_ATTACK_ME " + msg.command);
				}
				if (mob9 != null)
				{
					Char.myCharz().isDie = false;
					Char.isLockKey = false;
					int num165 = msg.readInt3Byte();
					int num166;
					try
					{
						num166 = msg.readInt3Byte();
					}
					catch (Exception)
					{
						num166 = 0;
					}
					if (mob9.isBusyAttackSomeOne)
					{
						Char.myCharz().doInjure(num165, num166, false, true);
						break;
					}
					mob9.dame = num165;
					mob9.dameMp = num166;
					mob9.setAttack(Char.myCharz());
				}
				break;
			}
			case -10:
			{
				GameCanvas.debug("SA87", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				GameCanvas.debug("SA87x1", 2);
				if (mob9 != null)
				{
					GameCanvas.debug("SA87x2", 2);
					@char = GameScr.findCharInMap(msg.reader().readInt());
					if (@char == null)
						return;
					GameCanvas.debug("SA87x3", 2);
					int num175 = msg.readInt3Byte();
					mob9.dame = @char.cHP - num175;
					@char.cHPNew = num175;
					GameCanvas.debug("SA87x4", 2);
					try
					{
						@char.cMP = msg.readInt3Byte();
					}
					catch (Exception)
					{
					}
					GameCanvas.debug("SA87x5", 2);
					if (mob9.isBusyAttackSomeOne)
						@char.doInjure(mob9.dame, 0, false, true);
					else
						mob9.setAttack(@char);
					GameCanvas.debug("SA87x6", 2);
				}
				break;
			}
			case -17:
				GameCanvas.debug("SA88", 2);
				Char.myCharz().meDead = true;
				Char.myCharz().cPk = msg.reader().readByte();
				Char.myCharz().startDie(msg.reader().readShort(), msg.reader().readShort());
				try
				{
					Char.myCharz().cPower = msg.reader().readLong();
					Char.myCharz().applyCharLevelPercent();
				}
				catch (Exception)
				{
					Cout.println("Loi tai ME_DIE " + msg.command);
				}
				Char.myCharz().countKill = 0;
				break;
			case -8:
				GameCanvas.debug("SA89", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
					return;
				@char.cPk = msg.reader().readByte();
				@char.waitToDie(msg.reader().readShort(), msg.reader().readShort());
				break;
			case -16:
				GameCanvas.debug("SA90", 2);
				if (Char.myCharz().wdx != 0 || Char.myCharz().wdy != 0)
				{
					Char.myCharz().cx = Char.myCharz().wdx;
					Char.myCharz().cy = Char.myCharz().wdy;
					Char.myCharz().wdx = (Char.myCharz().wdy = 0);
				}
				Char.myCharz().liveFromDead();
				Char.myCharz().isLockMove = false;
				Char.myCharz().meDead = false;
				break;
			}
			GameCanvas.debug("SA92", 2);
		}
		catch (Exception)
		{
		}
		finally
		{
			msg?.cleanup();
		}
	}

	private void createItem(myReader d)
	{
		GameScr.vcItem = d.readByte();
		ItemTemplates.itemTemplates.clear();
		GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readUnsignedByte()];
		for (int i = 0; i < GameScr.gI().iOptionTemplates.Length; i++)
		{
			GameScr.gI().iOptionTemplates[i] = new ItemOptionTemplate();
			GameScr.gI().iOptionTemplates[i].id = i;
			GameScr.gI().iOptionTemplates[i].name = d.readUTF();
			GameScr.gI().iOptionTemplates[i].type = d.readByte();
		}
		int num = d.readShort();
		for (int j = 0; j < num; j++)
		{
			ItemTemplates.add(new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBool()));
		}
	}

	private void createSkill(myReader d)
	{
		GameScr.vcSkill = d.readByte();
		GameScr.gI().sOptionTemplates = new SkillOptionTemplate[d.readByte()];
		for (int i = 0; i < GameScr.gI().sOptionTemplates.Length; i++)
		{
			GameScr.gI().sOptionTemplates[i] = new SkillOptionTemplate();
			GameScr.gI().sOptionTemplates[i].id = i;
			GameScr.gI().sOptionTemplates[i].name = d.readUTF();
		}
		GameScr.nClasss = new NClass[d.readByte()];
		for (int j = 0; j < GameScr.nClasss.Length; j++)
		{
			GameScr.nClasss[j] = new NClass();
			GameScr.nClasss[j].classId = j;
			GameScr.nClasss[j].name = d.readUTF();
			GameScr.nClasss[j].skillTemplates = new SkillTemplate[d.readByte()];
			for (int k = 0; k < GameScr.nClasss[j].skillTemplates.Length; k++)
			{
				GameScr.nClasss[j].skillTemplates[k] = new SkillTemplate();
				GameScr.nClasss[j].skillTemplates[k].id = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].name = d.readUTF();
				GameScr.nClasss[j].skillTemplates[k].maxPoint = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].manaUseType = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].type = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].iconId = d.readShort();
				GameScr.nClasss[j].skillTemplates[k].damInfo = d.readUTF();
				int lineWidth = 130;
				if (GameCanvas.w == 128 || GameCanvas.h <= 208)
					lineWidth = 100;
				GameScr.nClasss[j].skillTemplates[k].description = mFont.tahoma_7_green2.splitFontArray(d.readUTF(), lineWidth);
				GameScr.nClasss[j].skillTemplates[k].skills = new Skill[d.readByte()];
				for (int l = 0; l < GameScr.nClasss[j].skillTemplates[k].skills.Length; l++)
				{
					GameScr.nClasss[j].skillTemplates[k].skills[l] = new Skill();
					GameScr.nClasss[j].skillTemplates[k].skills[l].skillId = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].template = GameScr.nClasss[j].skillTemplates[k];
					GameScr.nClasss[j].skillTemplates[k].skills[l].point = d.readByte();
					GameScr.nClasss[j].skillTemplates[k].skills[l].powRequire = d.readLong();
					GameScr.nClasss[j].skillTemplates[k].skills[l].manaUse = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].coolDown = d.readInt();
					GameScr.nClasss[j].skillTemplates[k].skills[l].dx = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].dy = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].maxFight = d.readByte();
					GameScr.nClasss[j].skillTemplates[k].skills[l].damage = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].price = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].moreInfo = d.readUTF();
					Skills.add(GameScr.nClasss[j].skillTemplates[k].skills[l]);
				}
			}
		}
	}

	private void createMap(myReader d)
	{
		GameScr.vcMap = d.readByte();
		TileMap.mapNames = new string[d.readUnsignedByte()];
		for (int i = 0; i < TileMap.mapNames.Length; i++)
		{
			TileMap.mapNames[i] = d.readUTF();
		}
		Npc.arrNpcTemplate = new NpcTemplate[d.readByte()];
		for (sbyte b = 0; b < Npc.arrNpcTemplate.Length; b = (sbyte)(b + 1))
		{
			Npc.arrNpcTemplate[b] = new NpcTemplate();
			Npc.arrNpcTemplate[b].npcTemplateId = b;
			Npc.arrNpcTemplate[b].name = d.readUTF();
			Npc.arrNpcTemplate[b].headId = d.readShort();
			Npc.arrNpcTemplate[b].bodyId = d.readShort();
			Npc.arrNpcTemplate[b].legId = d.readShort();
			Npc.arrNpcTemplate[b].menu = new string[d.readByte()][];
			for (int j = 0; j < Npc.arrNpcTemplate[b].menu.Length; j++)
			{
				Npc.arrNpcTemplate[b].menu[j] = new string[d.readByte()];
				for (int k = 0; k < Npc.arrNpcTemplate[b].menu[j].Length; k++)
				{
					Npc.arrNpcTemplate[b].menu[j][k] = d.readUTF();
				}
			}
		}
		Mob.arrMobTemplate = new MobTemplate[d.readByte()];
		for (sbyte b2 = 0; b2 < Mob.arrMobTemplate.Length; b2 = (sbyte)(b2 + 1))
		{
			Mob.arrMobTemplate[b2] = new MobTemplate();
			Mob.arrMobTemplate[b2].mobTemplateId = b2;
			Mob.arrMobTemplate[b2].type = d.readByte();
			Mob.arrMobTemplate[b2].name = d.readUTF();
			Mob.arrMobTemplate[b2].hp = d.readInt();
			Mob.arrMobTemplate[b2].rangeMove = d.readByte();
			Mob.arrMobTemplate[b2].speed = d.readByte();
			Mob.arrMobTemplate[b2].dartType = d.readByte();
		}
	}

	private void createData(myReader d, bool isSaveRMS)
	{
		GameScr.vcData = d.readByte();
		if (isSaveRMS)
		{
			Rms.saveRMS("NR_dart", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_arrow", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_effect", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_image", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_part", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_skill", NinjaUtil.readByteArray(d));
			Rms.DeleteStorage("NRdata");
		}
	}

	private Image createImage(sbyte[] arr)
	{
		try
		{
			return Image.createImage(arr, 0, arr.Length);
		}
		catch (Exception)
		{
		}
		return null;
	}

	public int[] arrayByte2Int(sbyte[] b)
	{
		int[] array = new int[b.Length];
		for (int i = 0; i < b.Length; i++)
		{
			int num = b[i];
			if (num < 0)
				num += 256;
			array[i] = num;
		}
		return array;
	}

	public void readClanMsg(Message msg, int index)
	{
		try
		{
			ClanMessage clanMessage = new ClanMessage();
			sbyte b = msg.reader().readByte();
			clanMessage.type = b;
			clanMessage.id = msg.reader().readInt();
			clanMessage.playerId = msg.reader().readInt();
			clanMessage.playerName = msg.reader().readUTF();
			clanMessage.role = msg.reader().readByte();
			clanMessage.time = msg.reader().readInt() + 1000000000;
			bool flag = false;
			GameScr.isNewClanMessage = false;
			if (b == 0)
			{
				string text = msg.reader().readUTF();
				GameScr.isNewClanMessage = true;
				if (mFont.tahoma_7.getWidth(text) > Panel.WIDTH_PANEL - 60)
					clanMessage.chat = mFont.tahoma_7.splitFontArray(text, Panel.WIDTH_PANEL - 10);
				else
				{
					clanMessage.chat = new string[1];
					clanMessage.chat[0] = text;
				}
				clanMessage.color = msg.reader().readByte();
			}
			else if (b == 1)
			{
				clanMessage.recieve = msg.reader().readByte();
				clanMessage.maxCap = msg.reader().readByte();
				flag = msg.reader().readByte() == 1;
				if (flag)
					GameScr.isNewClanMessage = true;
				if (clanMessage.playerId != Char.myCharz().charID)
				{
					if (clanMessage.recieve < clanMessage.maxCap)
						clanMessage.option = new string[1] { mResources.donate };
					else
						clanMessage.option = null;
				}
				if (GameCanvas.panel.cp != null)
					GameCanvas.panel.updateRequest(clanMessage.recieve, clanMessage.maxCap);
			}
			else if (b == 2 && Char.myCharz().role == 0)
			{
				GameScr.isNewClanMessage = true;
				clanMessage.option = new string[2]
				{
					mResources.CANCEL,
					mResources.receive
				};
			}
			if (GameCanvas.currentScreen != GameScr.instance)
				GameScr.isNewClanMessage = false;
			else if (GameCanvas.panel.isShow && GameCanvas.panel.type == 0 && GameCanvas.panel.currentTabIndex == 3)
			{
				GameScr.isNewClanMessage = false;
			}
			ClanMessage.addMessage(clanMessage, index, flag);
		}
		catch (Exception)
		{
			Cout.println("LOI TAI CMD -= " + msg.command);
		}
	}

	public void loadCurrMap(sbyte teleport3)
	{
		Res.outz("is loading map = " + Char.isLoadingMap);
		GameScr.gI().auto = 0;
		GameScr.isChangeZone = false;
		CreateCharScr.instance = null;
		GameScr.info1.isUpdate = false;
		GameScr.info2.isUpdate = false;
		GameScr.lockTick = 0;
		GameCanvas.panel.isShow = false;
		SoundMn.gI().stopAll();
		if (!GameScr.isLoadAllData && !CreateCharScr.isCreateChar)
			GameScr.gI().initSelectChar();
		GameScr.loadCamera(false, (teleport3 != 1) ? (-1) : Char.myCharz().cx, (teleport3 == 0) ? (-1) : 0);
		TileMap.loadMainTile();
		TileMap.loadMap(TileMap.tileID);
		Res.outz("LOAD GAMESCR 2");
		Char.myCharz().cvx = 0;
		Char.myCharz().statusMe = 4;
		Char.myCharz().currentMovePoint = null;
		Char.myCharz().mobFocus = null;
		Char.myCharz().charFocus = null;
		Char.myCharz().npcFocus = null;
		Char.myCharz().itemFocus = null;
		Char.myCharz().skillPaint = null;
		Char.myCharz().setMabuHold(false);
		Char.myCharz().skillPaintRandomPaint = null;
		GameCanvas.clearAllPointerEvent();
		if (Char.myCharz().cy >= TileMap.pxh - 100)
		{
			Char.myCharz().isFlyUp = true;
			Char.myCharz().cx += Res.abs(Res.random(0, 80));
			Service.gI().charMove();
		}
		GameScr.gI().loadGameScr();
		GameCanvas.loadBG(TileMap.bgID);
		Char.isLockKey = false;
		Res.outz("cy= " + Char.myCharz().cy + "---------------------------------------------");
		for (int i = 0; i < Char.myCharz().vEff.size(); i++)
		{
			if (((EffectChar)Char.myCharz().vEff.elementAt(i)).template.type == 10)
			{
				Char.isLockKey = true;
				break;
			}
		}
		GameCanvas.clearKeyHold();
		GameCanvas.clearKeyPressed();
		GameScr.gI().dHP = Char.myCharz().cHP;
		GameScr.gI().dMP = Char.myCharz().cMP;
		Char.ischangingMap = false;
		GameScr.gI().switchToMe();
		if (Char.myCharz().cy <= 10 && teleport3 != 0 && teleport3 != 2)
		{
			Teleport.addTeleport(new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 1, true, (teleport3 != 1) ? teleport3 : Char.myCharz().cgender));
			Char.myCharz().isTeleport = true;
		}
		if (teleport3 == 2)
			Char.myCharz().show();
		if (GameScr.gI().isRongThanXuatHien)
		{
			if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
				GameScr.gI().callRongThan(GameScr.gI().xR, GameScr.gI().yR);
			if (mGraphics.zoomLevel > 1)
				GameScr.gI().doiMauTroi();
		}
		InfoDlg.hide();
		InfoDlg.show(TileMap.mapName, mResources.zone + " " + TileMap.zoneID, 30);
		GameCanvas.endDlg();
		GameCanvas.isLoading = false;
		Hint.clickMob();
		Hint.clickNpc();
		GameCanvas.debug("SA75x9", 2);
	}

	public void loadInfoMap(Message msg)
	{
		try
		{
			if (mGraphics.zoomLevel == 1)
				SmallImage.clearHastable();
			Char.myCharz().cx = (Char.myCharz().cxSend = (Char.myCharz().cxFocus = msg.reader().readShort()));
			Char.myCharz().cy = (Char.myCharz().cySend = (Char.myCharz().cyFocus = msg.reader().readShort()));
			Char.myCharz().xSd = Char.myCharz().cx;
			Char.myCharz().ySd = Char.myCharz().cy;
			Res.outz("head= " + Char.myCharz().head + " body= " + Char.myCharz().body + " left= " + Char.myCharz().leg + " x= " + Char.myCharz().cx + " y= " + Char.myCharz().cy + " chung toc= " + Char.myCharz().cgender);
			if (Char.myCharz().cx >= 0 && Char.myCharz().cx <= 100)
				Char.myCharz().cdir = 1;
			else if (Char.myCharz().cx >= TileMap.tmw - 100 && Char.myCharz().cx <= TileMap.tmw)
			{
				Char.myCharz().cdir = -1;
			}
			GameCanvas.debug("SA75x4", 2);
			int num = msg.reader().readByte();
			Res.outz("vGo size= " + num);
			if (!GameScr.info1.isDone)
			{
				GameScr.info1.cmx = Char.myCharz().cx - GameScr.cmx;
				GameScr.info1.cmy = Char.myCharz().cy - GameScr.cmy;
			}
			for (int i = 0; i < num; i++)
			{
				Waypoint waypoint = new Waypoint(msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readUTF());
				if ((TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23) || waypoint.minX < 0 || waypoint.minX <= 24)
					;
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
			GameCanvas.debug("SA75x5", 2);
			num = msg.reader().readByte();
			Mob.newMob.removeAllElements();
			for (sbyte b = 0; b < num; b = (sbyte)(b + 1))
			{
				Mob mob = new Mob(b, msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readByte(), msg.reader().readByte(), msg.reader().readInt(), msg.reader().readByte(), msg.reader().readInt(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readByte(), msg.reader().readByte());
				mob.xSd = mob.x;
				mob.ySd = mob.y;
				mob.isBoss = msg.reader().readBoolean();
				if (Mob.arrMobTemplate[mob.templateId].type != 0)
				{
					if (b % 3 == 0)
						mob.dir = -1;
					else
						mob.dir = 1;
					mob.x += 10 - b % 20;
				}
				mob.isMobMe = false;
				BigBoss bigBoss = null;
				BachTuoc bachTuoc = null;
				BigBoss2 bigBoss2 = null;
				NewBoss newBoss = null;
				if (mob.templateId == 70)
					bigBoss = new BigBoss(b, (short)mob.x, (short)mob.y, 70, mob.hp, mob.maxHp, mob.sys);
				if (mob.templateId == 71)
					bachTuoc = new BachTuoc(b, (short)mob.x, (short)mob.y, 71, mob.hp, mob.maxHp, mob.sys);
				if (mob.templateId == 72)
					bigBoss2 = new BigBoss2(b, (short)mob.x, (short)mob.y, 72, mob.hp, mob.maxHp, 3);
				if (mob.isBoss)
					newBoss = new NewBoss(b, (short)mob.x, (short)mob.y, mob.templateId, mob.hp, mob.maxHp, mob.sys);
				if (newBoss != null)
					GameScr.vMob.addElement(newBoss);
				else if (bigBoss != null)
				{
					GameScr.vMob.addElement(bigBoss);
				}
				else if (bachTuoc != null)
				{
					GameScr.vMob.addElement(bachTuoc);
				}
				else if (bigBoss2 != null)
				{
					GameScr.vMob.addElement(bigBoss2);
				}
				else
				{
					GameScr.vMob.addElement(mob);
				}
			}
			for (int j = 0; j < Mob.lastMob.size(); j++)
			{
				string text = (string)Mob.lastMob.elementAt(j);
				if (!Mob.isExistNewMob(text))
				{
					Mob.arrMobTemplate[int.Parse(text)].data = null;
					Mob.lastMob.removeElementAt(j);
					j--;
				}
			}
			if (Char.myCharz().mobMe != null && GameScr.findMobInMap(Char.myCharz().mobMe.mobId) == null)
			{
				Char.myCharz().mobMe.getData();
				Char.myCharz().mobMe.x = Char.myCharz().cx;
				Char.myCharz().mobMe.y = Char.myCharz().cy - 40;
				GameScr.vMob.addElement(Char.myCharz().mobMe);
			}
			num = msg.reader().readByte();
			for (byte b2 = 0; b2 < num; b2 = (byte)(b2 + 1))
			{
			}
			GameCanvas.debug("SA75x6", 2);
			num = msg.reader().readByte();
			Res.outz("NPC size= " + num);
			for (int k = 0; k < num; k++)
			{
				sbyte b3 = msg.reader().readByte();
				short cx = msg.reader().readShort();
				short num2 = msg.reader().readShort();
				sbyte b4 = msg.reader().readByte();
				short num3 = msg.reader().readShort();
				if (b4 != 6 && ((Char.myCharz().taskMaint.taskId >= 7 && (Char.myCharz().taskMaint.taskId != 7 || Char.myCharz().taskMaint.index > 1)) || (b4 != 7 && b4 != 8 && b4 != 9)) && (Char.myCharz().taskMaint.taskId >= 6 || b4 != 16))
				{
					if (b4 == 4)
					{
						GameScr.gI().magicTree = new MagicTree(k, b3, cx, num2, b4, num3);
						Service.gI().magicTree(2);
						GameScr.vNpc.addElement(GameScr.gI().magicTree);
					}
					else
					{
						Npc o = new Npc(k, b3, cx, num2 + 3, b4, num3);
						GameScr.vNpc.addElement(o);
					}
				}
			}
			GameCanvas.debug("SA75x7", 2);
			num = msg.reader().readByte();
			Res.outz("item size = " + num);
			for (int l = 0; l < num; l++)
			{
				short itemMapID = msg.reader().readShort();
				short itemTemplateID = msg.reader().readShort();
				int x = msg.reader().readShort();
				int y = msg.reader().readShort();
				int num4 = msg.reader().readInt();
				short r = 0;
				if (num4 == -2)
					r = msg.reader().readShort();
				ItemMap itemMap = new ItemMap(num4, itemMapID, itemTemplateID, x, y, r);
				bool flag = false;
				for (int m = 0; m < GameScr.vItemMap.size(); m++)
				{
					if (((ItemMap)GameScr.vItemMap.elementAt(m)).itemMapID == itemMap.itemMapID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
					GameScr.vItemMap.addElement(itemMap);
			}
			TileMap.vCurrItem.removeAllElements();
			if (mGraphics.zoomLevel == 1)
				BgItem.clearHashTable();
			BgItem.vKeysNew.removeAllElements();
			if (!GameCanvas.lowGraphic || (GameCanvas.lowGraphic && TileMap.isVoDaiMap()) || TileMap.mapID == 45 || TileMap.mapID == 46 || TileMap.mapID == 47 || TileMap.mapID == 48)
			{
				short num5 = msg.reader().readShort();
				Res.outz("nItem= " + num5);
				for (int n = 0; n < num5; n++)
				{
					short id = msg.reader().readShort();
					short num6 = msg.reader().readShort();
					short num7 = msg.reader().readShort();
					if (TileMap.getBIById(id) == null)
						continue;
					BgItem bIById = TileMap.getBIById(id);
					BgItem bgItem = new BgItem();
					bgItem.id = id;
					bgItem.idImage = bIById.idImage;
					bgItem.dx = bIById.dx;
					bgItem.dy = bIById.dy;
					bgItem.x = num6 * TileMap.size;
					bgItem.y = num7 * TileMap.size;
					bgItem.layer = bIById.layer;
					if (TileMap.isExistMoreOne(bgItem.id))
					{
						bgItem.trans = ((n % 2 != 0) ? 2 : 0);
						if (TileMap.mapID == 45)
							bgItem.trans = 0;
					}
					Image image = null;
					if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
					{
						if (mGraphics.zoomLevel == 1)
						{
							image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
							if (image == null)
							{
								image = Image.createRGBImage(new int[1], 1, 1, true);
								Service.gI().getBgTemplate(bgItem.idImage);
							}
							BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
						}
						else
						{
							bool flag2 = false;
							sbyte[] array = Rms.loadRMS(mGraphics.zoomLevel + "bgItem" + bgItem.idImage);
							if (array != null)
							{
								if (BgItem.newSmallVersion != null)
								{
									Res.outz("Small  last= " + array.Length % 127 + "new Version= " + BgItem.newSmallVersion[bgItem.idImage]);
									if (array.Length % 127 != BgItem.newSmallVersion[bgItem.idImage])
										flag2 = true;
								}
								if (!flag2)
								{
									image = Image.createImage(array, 0, array.Length);
									if (image != null)
										BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
									else
										flag2 = true;
								}
							}
							else
								flag2 = true;
							if (flag2)
							{
								image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
								if (image == null)
								{
									image = Image.createRGBImage(new int[1], 1, 1, true);
									Service.gI().getBgTemplate(bgItem.idImage);
								}
								BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
							}
						}
						BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
					}
					if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
						BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
					bgItem.changeColor();
					TileMap.vCurrItem.addElement(bgItem);
				}
				for (int num8 = 0; num8 < BgItem.vKeysLast.size(); num8++)
				{
					string text2 = (string)BgItem.vKeysLast.elementAt(num8);
					if (!BgItem.isExistKeyNews(text2))
					{
						BgItem.imgNew.remove(text2);
						if (BgItem.imgNew.containsKey(text2 + "blend" + 1))
							BgItem.imgNew.remove(text2 + "blend" + 1);
						if (BgItem.imgNew.containsKey(text2 + "blend" + 3))
							BgItem.imgNew.remove(text2 + "blend" + 3);
						BgItem.vKeysLast.removeElementAt(num8);
						num8--;
					}
				}
				BackgroudEffect.isFog = false;
				BackgroudEffect.nCloud = 0;
				EffecMn.vEff.removeAllElements();
				BackgroudEffect.vBgEffect.removeAllElements();
				Effect.newEff.removeAllElements();
				short num9 = msg.reader().readShort();
				for (int num10 = 0; num10 < num9; num10++)
				{
					keyValueAction(msg.reader().readUTF(), msg.reader().readUTF());
				}
				for (int num11 = 0; num11 < Effect.lastEff.size(); num11++)
				{
					string text3 = (string)Effect.lastEff.elementAt(num11);
					if (!Effect.isExistNewEff(text3))
					{
						Effect.removeEffData(int.Parse(text3));
						Effect.lastEff.removeElementAt(num11);
						num11--;
					}
				}
			}
			else
			{
				short num12 = msg.reader().readShort();
				for (int num13 = 0; num13 < num12; num13++)
				{
					short num14 = msg.reader().readShort();
					short num15 = msg.reader().readShort();
					short num16 = msg.reader().readShort();
				}
				short num17 = msg.reader().readShort();
				for (int num18 = 0; num18 < num17; num18++)
				{
					string text4 = msg.reader().readUTF();
					string text5 = msg.reader().readUTF();
				}
			}
			TileMap.bgType = msg.reader().readByte();
			loadCurrMap(msg.reader().readByte());
			Char.isLoadingMap = false;
			GameCanvas.debug("SA75x8", 2);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			Cout.LogError("----------DA CHAY XONG LOAD INFO MAP");
		}
		catch (Exception ex)
		{
			Cout.LogError("LOI TAI LOADMAP INFO " + ex.ToString());
            Pk9rXmap.fixBlackScreen();
        }
		GameEvents.onInfoMapLoaded();
	}

	public void keyValueAction(string key, string value)
	{
		if (key.Equals("eff"))
		{
			if (Panel.graphics > 0)
				return;
			string[] array = Res.split(value, ".", 0);
			int id = int.Parse(array[0]);
			int layer = int.Parse(array[1]);
			int x = int.Parse(array[2]);
			int y = int.Parse(array[3]);
			int loop;
			int loopCount;
			if (array.Length <= 4)
			{
				loop = -1;
				loopCount = 1;
			}
			else
			{
				loop = int.Parse(array[4]);
				loopCount = int.Parse(array[5]);
			}
			Effect effect = new Effect(id, x, y, layer, loop, loopCount);
			if (array.Length > 6)
			{
				effect.typeEff = int.Parse(array[6]);
				if (array.Length > 7)
				{
					effect.indexFrom = int.Parse(array[7]);
					effect.indexTo = int.Parse(array[8]);
				}
			}
			EffecMn.addEff(effect);
		}
		else if (key.Equals("beff") && Panel.graphics <= 1)
		{
			BackgroudEffect.addEffect(int.Parse(value));
		}
	}

	public void messageNotMap(Message msg)
	{
		GameCanvas.debug("SA6", 2);
		try
		{
			sbyte b = msg.reader().readByte();
			mSystem.LogCMD("---messageNotMap : " + b);
			switch (b)
			{
			default:
				switch (b)
				{
				case 33:
					break;
				case 35:
					GameCanvas.endDlg();
					GameScr.gI().resetButton();
					GameScr.info1.addInfo(msg.reader().readUTF(), 0);
					break;
				case 36:
					GameScr.typeActive = msg.reader().readByte();
					Res.outz("load Me Active: " + GameScr.typeActive);
					break;
				case 34:
					break;
				}
				break;
			case 16:
				MoneyCharge.gI().switchToMe();
				break;
			case 17:
				GameCanvas.debug("SYB123", 2);
				Char.myCharz().clearTask();
				break;
			case 18:
			{
				GameCanvas.isLoading = false;
				GameCanvas.endDlg();
				int num2 = msg.reader().readInt();
				GameCanvas.inputDlg.show(mResources.changeNameChar, new Command(mResources.OK, GameCanvas.instance, 88829, num2), TField.INPUT_TYPE_ANY);
				break;
			}
			case 20:
				Char.myCharz().cPk = msg.reader().readByte();
				GameScr.info1.addInfo(mResources.PK_NOW + " " + Char.myCharz().cPk, 0);
				break;
			case 4:
			{
				GameCanvas.debug("SA8", 2);
				GameCanvas.loginScr.savePass();
				GameScr.isAutoPlay = false;
				GameScr.canAutoPlay = false;
				LoginScr.isUpdateAll = true;
				LoginScr.isUpdateData = true;
				LoginScr.isUpdateMap = true;
				LoginScr.isUpdateSkill = true;
				LoginScr.isUpdateItem = true;
				GameScr.vsData = msg.reader().readByte();
				GameScr.vsMap = msg.reader().readByte();
				GameScr.vsSkill = msg.reader().readByte();
				GameScr.vsItem = msg.reader().readByte();
				sbyte b2 = msg.reader().readByte();
				if (GameCanvas.loginScr.isLogin2)
				{
					Rms.saveRMSString("acc", string.Empty);
					Rms.saveRMSString("pass", string.Empty);
				}
				else
					Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
				if (GameScr.vsData != GameScr.vcData)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateData();
				}
				else
					try
					{
						LoginScr.isUpdateData = false;
					}
					catch (Exception)
					{
						GameScr.vcData = -1;
						Service.gI().updateData();
					}
				if (GameScr.vsMap != GameScr.vcMap)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateMap();
				}
				else
					try
					{
						if (!GameScr.isLoadAllData)
							createMap(new DataInputStream(Rms.loadRMS("NRmap")).r);
						LoginScr.isUpdateMap = false;
					}
					catch (Exception)
					{
						GameScr.vcMap = -1;
						Service.gI().updateMap();
					}
				if (GameScr.vsSkill != GameScr.vcSkill)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateSkill();
				}
				else
					try
					{
						if (!GameScr.isLoadAllData)
							createSkill(new DataInputStream(Rms.loadRMS("NRskill")).r);
						LoginScr.isUpdateSkill = false;
					}
					catch (Exception)
					{
						GameScr.vcSkill = -1;
						Service.gI().updateSkill();
					}
				if (GameScr.vsItem != GameScr.vcItem)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateItem();
				}
				else
					try
					{
						loadItemNew(new DataInputStream(Rms.loadRMS("NRitem0")).r, 0, false);
						loadItemNew(new DataInputStream(Rms.loadRMS("NRitem1")).r, 1, false);
						loadItemNew(new DataInputStream(Rms.loadRMS("NRitem2")).r, 2, false);
						loadItemNew(new DataInputStream(Rms.loadRMS("NRitem100")).r, 100, false);
						LoginScr.isUpdateItem = false;
					}
					catch (Exception)
					{
						GameScr.vcItem = -1;
						Service.gI().updateItem();
					}
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					if (!GameScr.isLoadAllData)
					{
						GameScr.gI().readDart();
						GameScr.gI().readEfect();
						GameScr.gI().readArrow();
						GameScr.gI().readSkill();
					}
					Service.gI().clientOk();
				}
				sbyte b3 = msg.reader().readByte();
				Res.outz("CAPTION LENT= " + b3);
				GameScr.exps = new long[b3];
				for (int j = 0; j < GameScr.exps.Length; j++)
				{
					GameScr.exps[j] = msg.reader().readLong();
				}
				break;
			}
			case 6:
			{
				Res.outz("GET UPDATE_MAP " + msg.reader().available() + " bytes");
				msg.reader().mark(100000);
				createMap(msg.reader());
				msg.reader().reset();
				sbyte[] data2 = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data2);
				Rms.saveRMS("NRmap", data2);
				Rms.saveRMS("NRmapVersion", new sbyte[1] { GameScr.vcMap });
				LoginScr.isUpdateMap = false;
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					GameScr.gI().readDart();
					GameScr.gI().readEfect();
					GameScr.gI().readArrow();
					GameScr.gI().readSkill();
					Service.gI().clientOk();
				}
				break;
			}
			case 7:
			{
				Res.outz("GET UPDATE_SKILL " + msg.reader().available() + " bytes");
				msg.reader().mark(100000);
				createSkill(msg.reader());
				msg.reader().reset();
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data);
				Rms.saveRMS("NRskill", data);
				Rms.saveRMS("NRskillVersion", new sbyte[1] { GameScr.vcSkill });
				LoginScr.isUpdateSkill = false;
				if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
				{
					GameScr.gI().readDart();
					GameScr.gI().readEfect();
					GameScr.gI().readArrow();
					GameScr.gI().readSkill();
					Service.gI().clientOk();
				}
				break;
			}
			case 8:
				Res.outz("GET UPDATE_ITEM " + msg.reader().available() + " bytes");
				createItemNew(msg.reader());
				break;
			case 10:
				try
				{
					Char.isLoadingMap = true;
					Res.outz("REQUEST MAP TEMPLATE");
					GameCanvas.isLoading = true;
					TileMap.maps = null;
					TileMap.types = null;
					mSystem.gcc();
					GameCanvas.debug("SA99", 2);
					TileMap.tmw = msg.reader().readByte();
					TileMap.tmh = msg.reader().readByte();
					TileMap.maps = new int[TileMap.tmw * TileMap.tmh];
					Res.outz("   M apsize= " + TileMap.tmw * TileMap.tmh);
					for (int i = 0; i < TileMap.maps.Length; i++)
					{
						int num = msg.reader().readByte();
						if (num < 0)
							num += 256;
						TileMap.maps[i] = (ushort)num;
					}
					TileMap.types = new int[TileMap.maps.Length];
					msg = messWait;
					loadInfoMap(msg);
					try
					{
						TileMap.isMapDouble = ((msg.reader().readByte() != 0) ? true : false);
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex2)
				{
					Cout.LogError("LOI TAI CASE REQUEST_MAPTEMPLATE " + ex2.ToString());
				}
				msg.cleanup();
				messWait.cleanup();
				msg = (messWait = null);
				break;
			case 12:
				GameCanvas.debug("SA10", 2);
				break;
			case 9:
				GameCanvas.debug("SA11", 2);
				break;
			}
		}
		catch (Exception)
		{
			Cout.LogError("LOI TAI messageNotMap + " + msg.command);
		}
		finally
		{
			msg?.cleanup();
		}
	}

	public void messageNotLogin(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			mSystem.LogCMD("---messageNotLogin : " + b);
			if (b != 2)
				return;
			string text = msg.reader().readUTF();
			if (mSystem.isTest)
				text = "88:192.168.1.88:20000:0,53:112.213.85.53:20000:0," + text;
			if (mSystem.clientType == 1)
				ServerListScreen.linkDefault = text;
			else
				ServerListScreen.linkDefault = text;
			ServerListScreen.getServerList(ServerListScreen.linkDefault);
			try
			{
				Panel.CanNapTien = msg.reader().readByte() == 1;
			}
			catch (Exception)
			{
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			msg?.cleanup();
		}
	}

	public void messageSubCommand(Message msg)
	{
		try
		{
			GameCanvas.debug("SA12", 2);
			sbyte b = msg.reader().readByte();
			mSystem.LogCMD("---messageSubCommand : " + b);
			switch (b)
			{
			case 1:
				GameCanvas.debug("SA13", 2);
				Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
				Char.myCharz().cTiemNang = msg.reader().readLong();
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				Char.myCharz().myskill = null;
				return;
			case 2:
			{
				GameCanvas.debug("SA14", 2);
				if (Char.myCharz().statusMe != 14 && Char.myCharz().statusMe != 5)
				{
					Char.myCharz().cHP = Char.myCharz().cHPFull;
					Char.myCharz().cMP = Char.myCharz().cMPFull;
					Cout.LogError2(" ME_LOAD_SKILL");
				}
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				sbyte b3 = msg.reader().readByte();
				for (sbyte b6 = 0; b6 < b3; b6 = (sbyte)(b6 + 1))
				{
					useSkill(Skills.get(msg.reader().readShort()));
				}
				GameScr.gI().sortSkill();
				if (GameScr.isPaintInfoMe)
				{
					GameScr.indexRow = -1;
					GameScr.gI().left = (GameScr.gI().center = null);
				}
				return;
			}
			case 19:
				GameCanvas.debug("SA17", 2);
				Char.myCharz().boxSort();
				return;
			case 21:
			{
				GameCanvas.debug("SA19", 2);
				int num2 = msg.reader().readInt();
				Char.myCharz().xuInBox -= num2;
				Char.myCharz().xu += num2;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				return;
			}
			case 0:
			{
				GameCanvas.debug("SA21", 2);
				RadarScr.list = new MyVector();
				Teleport.vTeleport.removeAllElements();
				GameScr.vCharInMap.removeAllElements();
				GameScr.vItemMap.removeAllElements();
				Char.vItemTime.removeAllElements();
				GameScr.loadImg();
				GameScr.currentCharViewInfo = Char.myCharz();
				Char.myCharz().charID = msg.reader().readInt();
				Char.myCharz().ctaskId = msg.reader().readByte();
				Char.myCharz().cgender = msg.reader().readByte();
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().cName = msg.reader().readUTF();
				Char.myCharz().cPk = msg.reader().readByte();
				Char.myCharz().cTypePk = msg.reader().readByte();
				Char.myCharz().cPower = msg.reader().readLong();
				Char.myCharz().applyCharLevelPercent();
				Char.myCharz().eff5BuffHp = msg.reader().readShort();
				Char.myCharz().eff5BuffMp = msg.reader().readShort();
				Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				GameScr.gI().dHP = Char.myCharz().cHP;
				GameScr.gI().dMP = Char.myCharz().cMP;
				sbyte b3 = msg.reader().readByte();
				for (sbyte b4 = 0; b4 < b3; b4 = (sbyte)(b4 + 1))
				{
					useSkill(Skills.get(msg.reader().readShort()));
				}
				GameScr.gI().sortSkill();
				GameScr.gI().loadSkillShortcut();
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				Char.myCharz().arrItemBody = new Item[msg.reader().readByte()];
				try
				{
					Char.myCharz().setDefaultPart();
					for (int j = 0; j < Char.myCharz().arrItemBody.Length; j++)
					{
						short num4 = msg.reader().readShort();
						if (num4 == -1)
							continue;
						ItemTemplate itemTemplate = ItemTemplates.get(num4);
						int num5 = itemTemplate.type;
						Char.myCharz().arrItemBody[j] = new Item();
						Char.myCharz().arrItemBody[j].template = itemTemplate;
						Char.myCharz().arrItemBody[j].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBody[j].info = msg.reader().readUTF();
						Char.myCharz().arrItemBody[j].content = msg.reader().readUTF();
						int num6 = msg.reader().readUnsignedByte();
						if (num6 != 0)
						{
							Char.myCharz().arrItemBody[j].itemOption = new ItemOption[num6];
							for (int k = 0; k < Char.myCharz().arrItemBody[j].itemOption.Length; k++)
							{
								int num7 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								if (num7 != -1)
									Char.myCharz().arrItemBody[j].itemOption[k] = new ItemOption(num7, param);
							}
						}
						if (num5 == 0)
						{
							Res.outz("toi day =======================================" + Char.myCharz().body);
							Char.myCharz().body = Char.myCharz().arrItemBody[j].template.part;
						}
						else if (num5 == 1)
						{
							Char.myCharz().leg = Char.myCharz().arrItemBody[j].template.part;
							Res.outz("toi day =======================================" + Char.myCharz().leg);
						}
					}
				}
				catch (Exception)
				{
				}
				Char.myCharz().arrItemBag = new Item[msg.reader().readByte()];
				GameScr.hpPotion = 0;
				for (int l = 0; l < Char.myCharz().arrItemBag.Length; l++)
				{
					short num8 = msg.reader().readShort();
					if (num8 == -1)
						continue;
					Char.myCharz().arrItemBag[l] = new Item();
					Char.myCharz().arrItemBag[l].template = ItemTemplates.get(num8);
					Char.myCharz().arrItemBag[l].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBag[l].info = msg.reader().readUTF();
					Char.myCharz().arrItemBag[l].content = msg.reader().readUTF();
					Char.myCharz().arrItemBag[l].indexUI = l;
					sbyte b5 = msg.reader().readByte();
					if (b5 != 0)
					{
						Char.myCharz().arrItemBag[l].itemOption = new ItemOption[b5];
						for (int m = 0; m < Char.myCharz().arrItemBag[l].itemOption.Length; m++)
						{
							int num9 = msg.reader().readUnsignedByte();
							int param2 = msg.reader().readUnsignedShort();
							if (num9 != -1)
							{
								Char.myCharz().arrItemBag[l].itemOption[m] = new ItemOption(num9, param2);
								Char.myCharz().arrItemBag[l].getCompare();
							}
						}
					}
					if (Char.myCharz().arrItemBag[l].template.type == 6)
						GameScr.hpPotion += Char.myCharz().arrItemBag[l].quantity;
				}
				Char.myCharz().arrItemBox = new Item[msg.reader().readByte()];
				GameCanvas.panel.hasUse = 0;
				for (int n = 0; n < Char.myCharz().arrItemBox.Length; n++)
				{
					short num10 = msg.reader().readShort();
					if (num10 == -1)
						continue;
					Char.myCharz().arrItemBox[n] = new Item();
					Char.myCharz().arrItemBox[n].template = ItemTemplates.get(num10);
					Char.myCharz().arrItemBox[n].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBox[n].info = msg.reader().readUTF();
					Char.myCharz().arrItemBox[n].content = msg.reader().readUTF();
					Char.myCharz().arrItemBox[n].itemOption = new ItemOption[msg.reader().readByte()];
					for (int num11 = 0; num11 < Char.myCharz().arrItemBox[n].itemOption.Length; num11++)
					{
						int num12 = msg.reader().readUnsignedByte();
						int param3 = msg.reader().readUnsignedShort();
						if (num12 != -1)
						{
							Char.myCharz().arrItemBox[n].itemOption[num11] = new ItemOption(num12, param3);
							Char.myCharz().arrItemBox[n].getCompare();
						}
					}
					GameCanvas.panel.hasUse++;
				}
				Char.myCharz().statusMe = 4;
				if (Rms.loadRMSInt(Char.myCharz().cName + "vci") < 1)
					GameScr.isViewClanInvite = false;
				else
					GameScr.isViewClanInvite = true;
				short num13 = msg.reader().readShort();
				Char.idHead = new short[num13];
				Char.idAvatar = new short[num13];
				for (int num14 = 0; num14 < num13; num14++)
				{
					Char.idHead[num14] = msg.reader().readShort();
					Char.idAvatar[num14] = msg.reader().readShort();
				}
				for (int num15 = 0; num15 < GameScr.info1.charId.Length; num15++)
				{
					GameScr.info1.charId[num15] = new int[3];
				}
				GameScr.info1.charId[Char.myCharz().cgender][0] = msg.reader().readShort();
				GameScr.info1.charId[Char.myCharz().cgender][1] = msg.reader().readShort();
				GameScr.info1.charId[Char.myCharz().cgender][2] = msg.reader().readShort();
				Char.myCharz().isNhapThe = msg.reader().readByte() == 1;
				Res.outz("NHAP THE= " + Char.myCharz().isNhapThe);
				GameScr.deltaTime = mSystem.currentTimeMillis() - (long)msg.reader().readInt() * 1000L;
				GameScr.isNewMember = msg.reader().readByte();
				Service.gI().updateCaption((sbyte)Char.myCharz().cgender);
				Service.gI().androidPack();
				try
				{
					Char.myCharz().idAuraEff = msg.reader().readShort();
					Char.myCharz().idEff_Set_Item = msg.reader().readSByte();
					Char.myCharz().idHat = msg.reader().readShort();
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			case 4:
				GameCanvas.debug("SA23", 2);
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().cHP = msg.readInt3Byte();
				Char.myCharz().cMP = msg.readInt3Byte();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				return;
			case 5:
			{
				GameCanvas.debug("SA24", 2);
				int cHP = Char.myCharz().cHP;
				Char.myCharz().cHP = msg.readInt3Byte();
				if (Char.myCharz().cHP > cHP && Char.myCharz().cTypePk != 4)
				{
					GameScr.startFlyText("+" + (Char.myCharz().cHP - cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
					SoundMn.gI().HP_MPup();
					if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5003)
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, true, -1, -1, Char.myCharz(), 29);
				}
				if (Char.myCharz().cHP < cHP)
					GameScr.startFlyText("-" + (cHP - Char.myCharz().cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
				GameScr.gI().dHP = Char.myCharz().cHP;
				if (GameScr.isPaintInfoMe)
					;
				return;
			}
			case 6:
			{
				GameCanvas.debug("SA25", 2);
				if (Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
					return;
				int cMP = Char.myCharz().cMP;
				Char.myCharz().cMP = msg.readInt3Byte();
				if (Char.myCharz().cMP > cMP)
				{
					GameScr.startFlyText("+" + (Char.myCharz().cMP - cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
					SoundMn.gI().HP_MPup();
					if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5001)
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, true, -1, -1, Char.myCharz(), 29);
				}
				if (Char.myCharz().cMP < cMP)
					GameScr.startFlyText("-" + (cMP - Char.myCharz().cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
				Res.outz("curr MP= " + Char.myCharz().cMP);
				GameScr.gI().dMP = Char.myCharz().cMP;
				if (GameScr.isPaintInfoMe)
					;
				return;
			}
			case 7:
			{
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.clanID = msg.reader().readInt();
					if (@char.clanID == -2)
						@char.isCopy = true;
					readCharInfo(@char, msg);
					try
					{
						@char.idAuraEff = msg.reader().readShort();
						@char.idEff_Set_Item = msg.reader().readSByte();
						@char.idHat = msg.reader().readShort();
						return;
					}
					catch (Exception)
					{
						return;
					}
				}
				return;
			}
			case 8:
			{
				GameCanvas.debug("SA26", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
					@char.cspeed = msg.reader().readByte();
				return;
			}
			case 9:
			{
				GameCanvas.debug("SA27", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.cHP = msg.readInt3Byte();
					@char.cHPFull = msg.readInt3Byte();
				}
				return;
			}
			case 10:
			{
				GameCanvas.debug("SA28", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.cHP = msg.readInt3Byte();
					@char.cHPFull = msg.readInt3Byte();
					@char.eff5BuffHp = msg.reader().readShort();
					@char.eff5BuffMp = msg.reader().readShort();
					@char.wp = msg.reader().readShort();
					if (@char.wp == -1)
						@char.setDefaultWeapon();
				}
				return;
			}
			case 11:
			{
				GameCanvas.debug("SA29", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.cHP = msg.readInt3Byte();
					@char.cHPFull = msg.readInt3Byte();
					@char.eff5BuffHp = msg.reader().readShort();
					@char.eff5BuffMp = msg.reader().readShort();
					@char.body = msg.reader().readShort();
					if (@char.body == -1)
						@char.setDefaultBody();
				}
				return;
			}
			case 12:
			{
				GameCanvas.debug("SA30", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.cHP = msg.readInt3Byte();
					@char.cHPFull = msg.readInt3Byte();
					@char.eff5BuffHp = msg.reader().readShort();
					@char.eff5BuffMp = msg.reader().readShort();
					@char.leg = msg.reader().readShort();
					if (@char.leg == -1)
						@char.setDefaultLeg();
				}
				return;
			}
			case 13:
			{
				GameCanvas.debug("SA31", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.cHP = msg.readInt3Byte();
					@char.cHPFull = msg.readInt3Byte();
					@char.eff5BuffHp = msg.reader().readShort();
					@char.eff5BuffMp = msg.reader().readShort();
				}
				return;
			}
			case 14:
			{
				GameCanvas.debug("SA32", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
					return;
				@char.cHP = msg.readInt3Byte();
				sbyte b2 = msg.reader().readByte();
				Res.outz("player load hp type= " + b2);
				if (b2 == 1)
				{
					ServerEffect.addServerEffect(11, @char, 5);
					ServerEffect.addServerEffect(104, @char, 4);
				}
				try
				{
					@char.cHPFull = msg.readInt3Byte();
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			case 15:
			{
				GameCanvas.debug("SA33", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.cHP = msg.readInt3Byte();
					@char.cHPFull = msg.readInt3Byte();
					@char.cx = msg.reader().readShort();
					@char.cy = msg.reader().readShort();
					@char.statusMe = 1;
					@char.cp3 = 3;
					ServerEffect.addServerEffect(109, @char, 2);
				}
				return;
			}
			case 35:
			{
				GameCanvas.debug("SY3", 2);
				int num3 = msg.reader().readInt();
				Res.outz("CID = " + num3);
				if (TileMap.mapID == 130)
					GameScr.gI().starVS();
				if (num3 == Char.myCharz().charID)
				{
					Char.myCharz().cTypePk = msg.reader().readByte();
					if (GameScr.gI().isVS() && Char.myCharz().cTypePk != 0)
						GameScr.gI().starVS();
					Res.outz("type pk= " + Char.myCharz().cTypePk);
					Char.myCharz().npcFocus = null;
					if (!GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
						Char.myCharz().mobFocus = null;
					Char.myCharz().itemFocus = null;
				}
				else
				{
					Char @char = GameScr.findCharInMap(num3);
					if (@char != null)
					{
						Res.outz("type pk= " + @char.cTypePk);
						@char.cTypePk = msg.reader().readByte();
						if (@char.isAttacPlayerStatus())
							Char.myCharz().charFocus = @char;
					}
				}
				for (int i = 0; i < GameScr.vCharInMap.size(); i++)
				{
					Char char2 = GameScr.findCharInMap(i);
					if (char2 != null && char2.cTypePk != 0 && char2.cTypePk == Char.myCharz().cTypePk)
					{
						if (!Char.myCharz().mobFocus.isMobMe)
							Char.myCharz().mobFocus = null;
						Char.myCharz().npcFocus = null;
						Char.myCharz().itemFocus = null;
						break;
					}
				}
				Res.outz("update type pk= ");
				return;
			}
			case 23:
			{
				short num = msg.reader().readShort();
				Skill skill = Skills.get(num);
				useSkill(skill);
				if (num != 0 && num != 14 && num != 28)
					GameScr.info1.addInfo(mResources.LEARN_SKILL + " " + skill.template.name, 0);
				return;
			}
			}
			switch (b)
			{
			case 63:
			{
				sbyte b7 = msg.reader().readByte();
				if (b7 > 0)
				{
					InfoDlg.showWait();
					MyVector vPlayerMenu = GameCanvas.panel.vPlayerMenu;
					for (int num20 = 0; num20 < b7; num20++)
					{
						string caption = msg.reader().readUTF();
						string caption2 = msg.reader().readUTF();
						short menuSelect = msg.reader().readShort();
						Char.myCharz().charFocus.menuSelect = menuSelect;
						Command command = new Command(caption, 11115, Char.myCharz().charFocus);
						command.caption2 = caption2;
						vPlayerMenu.addElement(command);
					}
					InfoDlg.hide();
					GameCanvas.panel.setTabPlayerMenu();
				}
				break;
			}
			case 61:
			{
				string text = msg.reader().readUTF();
				sbyte[] data = new sbyte[msg.reader().readInt()];
				msg.reader().read(ref data);
				if (data.Length == 0)
					data = null;
				if (text.Equals("KSkill"))
					GameScr.gI().onKSkill(data);
				else if (text.Equals("OSkill"))
				{
					GameScr.gI().onOSkill(data);
				}
				else if (text.Equals("CSkill"))
				{
					GameScr.gI().onCSkill(data);
				}
				break;
			}
			case 62:
			{
				Res.outz("ME UPDATE SKILL");
				Skill skill2 = Skills.get(msg.reader().readShort());
				for (int num16 = 0; num16 < Char.myCharz().vSkill.size(); num16++)
				{
					if (((Skill)Char.myCharz().vSkill.elementAt(num16)).template.id == skill2.template.id)
					{
						Char.myCharz().vSkill.setElementAt(skill2, num16);
						break;
					}
				}
				for (int num17 = 0; num17 < Char.myCharz().vSkillFight.size(); num17++)
				{
					if (((Skill)Char.myCharz().vSkillFight.elementAt(num17)).template.id == skill2.template.id)
					{
						Char.myCharz().vSkillFight.setElementAt(skill2, num17);
						break;
					}
				}
				for (int num18 = 0; num18 < GameScr.onScreenSkill.Length; num18++)
				{
					if (GameScr.onScreenSkill[num18] != null && GameScr.onScreenSkill[num18].template.id == skill2.template.id)
					{
						GameScr.onScreenSkill[num18] = skill2;
						break;
					}
				}
				for (int num19 = 0; num19 < GameScr.keySkill.Length; num19++)
				{
					if (GameScr.keySkill[num19] != null && GameScr.keySkill[num19].template.id == skill2.template.id)
					{
						GameScr.keySkill[num19] = skill2;
						break;
					}
				}
				if (Char.myCharz().myskill.template.id == skill2.template.id)
					Char.myCharz().myskill = skill2;
				GameScr.info1.addInfo(mResources.hasJustUpgrade1 + skill2.template.name + mResources.hasJustUpgrade2 + skill2.point, 0);
				break;
			}
			}
		}
		catch (Exception ex5)
		{
			Cout.println("Loi tai Sub : " + ex5.ToString());
		}
		finally
		{
			msg?.cleanup();
		}
	}

	private void useSkill(Skill skill)
	{
		if (Char.myCharz().myskill == null)
			Char.myCharz().myskill = skill;
		else if (skill.template.Equals(Char.myCharz().myskill.template))
		{
			Char.myCharz().myskill = skill;
		}
		Char.myCharz().vSkill.addElement(skill);
		if ((skill.template.type == 1 || skill.template.type == 4 || skill.template.type == 2 || skill.template.type == 3) && (skill.template.maxPoint == 0 || (skill.template.maxPoint > 0 && skill.point > 0)))
		{
			if (skill.template.id == Char.myCharz().skillTemplateId)
				Service.gI().selectSkill(Char.myCharz().skillTemplateId);
			Char.myCharz().vSkillFight.addElement(skill);
		}
	}

	public bool readCharInfo(Char c, Message msg)
	{
		try
		{
			c.clevel = msg.reader().readByte();
			c.isInvisiblez = msg.reader().readBoolean();
			c.cTypePk = msg.reader().readByte();
			Res.outz("ADD TYPE PK= " + c.cTypePk + " to player " + c.charID + " @@ " + c.cName);
			c.nClass = GameScr.nClasss[msg.reader().readByte()];
			c.cgender = msg.reader().readByte();
			c.head = msg.reader().readShort();
			c.cName = msg.reader().readUTF();
			c.cHP = msg.readInt3Byte();
			c.dHP = c.cHP;
			if (c.cHP == 0)
				c.statusMe = 14;
			c.cHPFull = msg.readInt3Byte();
			if (c.cy >= TileMap.pxh - 100)
				c.isFlyUp = true;
			c.body = msg.reader().readShort();
			c.leg = msg.reader().readShort();
			c.bag = msg.reader().readUnsignedByte();
			Res.outz(" body= " + c.body + " leg= " + c.leg + " bag=" + c.bag + "BAG ==" + c.bag + "*********************************");
			c.isShadown = true;
			sbyte b = msg.reader().readByte();
			if (c.wp == -1)
				c.setDefaultWeapon();
			if (c.body == -1)
				c.setDefaultBody();
			if (c.leg == -1)
				c.setDefaultLeg();
			c.cx = msg.reader().readShort();
			c.cy = msg.reader().readShort();
			c.xSd = c.cx;
			c.ySd = c.cy;
			c.eff5BuffHp = msg.reader().readShort();
			c.eff5BuffMp = msg.reader().readShort();
			int num = msg.reader().readByte();
			for (int i = 0; i < num; i++)
			{
				EffectChar effectChar = new EffectChar(msg.reader().readByte(), msg.reader().readInt(), msg.reader().readInt(), msg.reader().readShort());
				c.vEff.addElement(effectChar);
				if (effectChar.template.type == 12 || effectChar.template.type == 11)
					c.isInvisiblez = true;
			}
			return true;
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		return false;
	}

	private void readGetImgByName(Message msg)
	{
		try
		{
			string text = msg.reader().readUTF();
			sbyte nFrame = msg.reader().readByte();
			sbyte[] array = null;
			array = NinjaUtil.readByteArray(msg);
			ImgByName.SetImage(text, createImage(array), nFrame);
			if (array != null)
				ImgByName.saveRMS(text, nFrame, array);
		}
		catch (Exception)
		{
		}
	}

	private void createItemNew(myReader d)
	{
		try
		{
			loadItemNew(d, -1, true);
		}
		catch (Exception)
		{
		}
	}

	private void loadItemNew(myReader d, sbyte type, bool isSave)
	{
		try
		{
			d.mark(100000);
			GameScr.vcItem = d.readByte();
			type = d.readByte();
			if (type == 0)
			{
				GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readUnsignedByte()];
				for (int i = 0; i < GameScr.gI().iOptionTemplates.Length; i++)
				{
					GameScr.gI().iOptionTemplates[i] = new ItemOptionTemplate();
					GameScr.gI().iOptionTemplates[i].id = i;
					GameScr.gI().iOptionTemplates[i].name = d.readUTF();
					GameScr.gI().iOptionTemplates[i].type = d.readByte();
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data = new sbyte[d.available()];
					d.readFully(ref data);
					Rms.saveRMS("NRitem0", data);
				}
			}
			else if (type == 1)
			{
				ItemTemplates.itemTemplates.clear();
				int num = d.readShort();
				for (int j = 0; j < num; j++)
				{
					ItemTemplates.add(new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean()));
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data2 = new sbyte[d.available()];
					d.readFully(ref data2);
					Rms.saveRMS("NRitem1", data2);
				}
			}
			else if (type == 2)
			{
				int num2 = d.readShort();
				int num3 = d.readShort();
				for (int k = num2; k < num3; k++)
				{
					ItemTemplates.add(new ItemTemplate((short)k, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean()));
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data3 = new sbyte[d.available()];
					d.readFully(ref data3);
					Rms.saveRMS("NRitem2", data3);
					Rms.saveRMS("NRitemVersion", new sbyte[1] { GameScr.vcItem });
					LoginScr.isUpdateItem = false;
					if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
					{
						GameScr.gI().readDart();
						GameScr.gI().readEfect();
						GameScr.gI().readArrow();
						GameScr.gI().readSkill();
						Service.gI().clientOk();
					}
				}
			}
			else if (type == 100)
			{
				Char.Arr_Head_2Fr = readArrHead(d);
				if (isSave)
				{
					d.reset();
					sbyte[] data4 = new sbyte[d.available()];
					d.readFully(ref data4);
					Rms.saveRMS("NRitem100", data4);
				}
			}
		}
		catch (Exception ex)
		{
			ex.ToString();
		}
	}

	private void readFrameBoss(Message msg, int mobTemplateId)
	{
		try
		{
			int num = msg.reader().readByte();
			int[][] array = new int[num][];
			for (int i = 0; i < num; i++)
			{
				int num2 = msg.reader().readByte();
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
				{
					array[i][j] = msg.reader().readByte();
				}
			}
			frameHT_NEWBOSS.put(mobTemplateId + string.Empty, array);
		}
		catch (Exception)
		{
		}
	}

	private int[][] readArrHead(myReader d)
	{
		int[][] array = new int[1][] { new int[2] { 542, 543 } };
		try
		{
			array = new int[d.readShort()][];
			for (int i = 0; i < array.Length; i++)
			{
				int num = d.readByte();
				array[i] = new int[num];
				for (int j = 0; j < num; j++)
				{
					array[i][j] = d.readShort();
				}
			}
			return array;
		}
		catch (Exception)
		{
			return array;
		}
	}

	public void phuban_Info(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
				readPhuBan_CHIENTRUONGNAMEK(msg, b);
		}
		catch (Exception)
		{
		}
	}

	private void readPhuBan_CHIENTRUONGNAMEK(Message msg, int type_PB)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
			{
				short idmapPaint = msg.reader().readShort();
				string nameTeam = msg.reader().readUTF();
				string nameTeam2 = msg.reader().readUTF();
				int maxPoint = msg.reader().readInt();
				short timeSecond = msg.reader().readShort();
				int maxLife = msg.reader().readByte();
				GameScr.phuban_Info = new InfoPhuBan(type_PB, idmapPaint, nameTeam, nameTeam2, maxPoint, timeSecond);
				GameScr.phuban_Info.maxLife = maxLife;
				GameScr.phuban_Info.updateLife(type_PB, 0, 0);
			}
			else if (b == 1)
			{
				int pointTeam = msg.reader().readInt();
				int pointTeam2 = msg.reader().readInt();
				if (GameScr.phuban_Info != null)
					GameScr.phuban_Info.updatePoint(type_PB, pointTeam, pointTeam2);
			}
			else if (b == 2)
			{
				sbyte b2 = msg.reader().readByte();
				short type = 0;
				short num = -1;
				if (b2 == 1)
				{
					type = 1;
					num = 3;
				}
				else if (b2 == 2)
				{
					type = 2;
				}
				num = -1;
				GameScr.phuban_Info = null;
				GameScr.addEffectEnd(type, num, GameCanvas.hw, GameCanvas.hh, 0, 0);
			}
			else if (b == 5)
			{
				short timeSecond2 = msg.reader().readShort();
				if (GameScr.phuban_Info != null)
					GameScr.phuban_Info.updateTime(type_PB, timeSecond2);
			}
			else if (b == 4)
			{
				int lifeTeam = msg.reader().readByte();
				int lifeTeam2 = msg.reader().readByte();
				if (GameScr.phuban_Info != null)
					GameScr.phuban_Info.updateLife(type_PB, lifeTeam, lifeTeam2);
			}
		}
		catch (Exception)
		{
		}
	}

	public void read_opt(Message msg)
	{
		try
		{
			if (msg.reader().readByte() == 0)
			{
				short idHat = msg.reader().readShort();
				Char.myCharz().idHat = idHat;
				SoundMn.gI().getStrOption();
			}
		}
		catch (Exception)
		{
		}
	}
}
