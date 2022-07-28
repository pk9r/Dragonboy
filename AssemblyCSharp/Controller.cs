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
		{
			me = new Controller();
		}
		return me;
	}

	public static Controller gI2()
	{
		if (me2 == null)
		{
			me2 = new Controller();
		}
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
			int num = msg.reader().readUnsignedByte();
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
			switch (msg.command)
			{
			case 20:
				phuban_Info(msg);
				break;
			case 66:
				readGetImgByName(msg);
				break;
			case 65:
			{
				sbyte b18 = msg.reader().readSByte();
				string text = msg.reader().readUTF();
				short num53 = msg.reader().readShort();
				if (ItemTime.isExistMessage(b18))
				{
					if (num53 != 0)
					{
						ItemTime.getMessageById(b18).initTimeText(b18, text, num53);
					}
					else
					{
						GameScr.textTime.removeElement(ItemTime.getMessageById(b18));
					}
				}
				else
				{
					ItemTime itemTime = new ItemTime();
					itemTime.initTimeText(b18, text, num53);
					GameScr.textTime.addElement(itemTime);
				}
				break;
			}
			case 112:
			{
				sbyte b21 = msg.reader().readByte();
				Res.outz("spec type= " + b21);
				if (b21 == 0)
				{
					Panel.spearcialImage = msg.reader().readShort();
					Panel.specialInfo = msg.reader().readUTF();
				}
				else
				{
					if (b21 != 1)
					{
						break;
					}
					sbyte b22 = msg.reader().readByte();
					Char.myCharz().infoSpeacialSkill = new string[b22][];
					Char.myCharz().imgSpeacialSkill = new short[b22][];
					GameCanvas.panel.speacialTabName = new string[b22][];
					for (int num55 = 0; num55 < b22; num55++)
					{
						GameCanvas.panel.speacialTabName[num55] = new string[2];
						string[] array6 = Res.split(msg.reader().readUTF(), "\n", 0);
						if (array6.Length == 2)
						{
							GameCanvas.panel.speacialTabName[num55] = array6;
						}
						if (array6.Length == 1)
						{
							GameCanvas.panel.speacialTabName[num55][0] = array6[0];
							GameCanvas.panel.speacialTabName[num55][1] = string.Empty;
						}
						int num56 = msg.reader().readByte();
						Char.myCharz().infoSpeacialSkill[num55] = new string[num56];
						Char.myCharz().imgSpeacialSkill[num55] = new short[num56];
						for (int num57 = 0; num57 < num56; num57++)
						{
							Char.myCharz().imgSpeacialSkill[num55][num57] = msg.reader().readShort();
							Char.myCharz().infoSpeacialSkill[num55][num57] = msg.reader().readUTF();
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
				sbyte b19 = msg.reader().readByte();
				GameCanvas.menu.showMenu = false;
				if (b19 == 0)
				{
					GameCanvas.startYesNoDlg(msg.reader().readUTF(), new Command(mResources.YES, GameCanvas.instance, 888397, msg.reader().readUTF()), new Command(mResources.NO, GameCanvas.instance, 888396, null));
				}
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
				sbyte b30 = msg.reader().readByte();
				for (int num81 = 0; num81 < b30; num81++)
				{
					int rank = msg.reader().readInt();
					int pId = msg.reader().readInt();
					short headID = msg.reader().readShort();
					short headICON = msg.reader().readShort();
					short body = msg.reader().readShort();
					short leg = msg.reader().readShort();
					string name = msg.reader().readUTF();
					string info4 = msg.reader().readUTF();
					TopInfo topInfo = new TopInfo();
					topInfo.rank = rank;
					topInfo.headID = headID;
					topInfo.headICON = headICON;
					topInfo.body = body;
					topInfo.leg = leg;
					topInfo.name = name;
					topInfo.info = info4;
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
					short num115 = msg.reader().readShort();
					int num116 = msg.reader().readInt();
					for (int num117 = 0; num117 < Char.myCharz().vSkill.size(); num117++)
					{
						Skill skill = (Skill)Char.myCharz().vSkill.elementAt(num117);
						if (skill != null && skill.skillId == num115)
						{
							if (num116 < skill.coolDown)
							{
								skill.lastTimeUseThisSkill = mSystem.currentTimeMillis() - (skill.coolDown - num116);
							}
							Res.outz("1 chieu id= " + skill.template.id + " cooldown= " + num116 + "curr cool down= " + skill.coolDown);
						}
					}
				}
				break;
			case -95:
			{
				sbyte b66 = msg.reader().readByte();
				Res.outz("type= " + b66);
				if (b66 == 0)
				{
					int num146 = msg.reader().readInt();
					short templateId = msg.reader().readShort();
					int num147 = msg.readInt3Byte();
					SoundMn.gI().explode_1();
					if (num146 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe = new Mob(num146, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num147, 0, num147, (short)(Char.myCharz().cx + ((Char.myCharz().cdir != 1) ? (-40) : 40)), (short)Char.myCharz().cy, 4, 0);
						Char.myCharz().mobMe.isMobMe = true;
						EffecMn.addEff(new Effect(18, Char.myCharz().mobMe.x, Char.myCharz().mobMe.y, 2, 10, -1));
						Char.myCharz().tMobMeBorn = 30;
						GameScr.vMob.addElement(Char.myCharz().mobMe);
					}
					else
					{
						@char = GameScr.findCharInMap(num146);
						if (@char != null)
						{
							Mob mob6 = new Mob(num146, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num147, 0, num147, (short)@char.cx, (short)@char.cy, 4, 0);
							mob6.isMobMe = true;
							@char.mobMe = mob6;
							GameScr.vMob.addElement(@char.mobMe);
						}
						else
						{
							Mob mob7 = GameScr.findMobInMap(num146);
							if (mob7 == null)
							{
								mob7 = new Mob(num146, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num147, 0, num147, -100, -100, 4, 0);
								mob7.isMobMe = true;
								GameScr.vMob.addElement(mob7);
							}
						}
					}
				}
				if (b66 == 1)
				{
					int num148 = msg.reader().readInt();
					int mobId = msg.reader().readByte();
					Res.outz("mod attack id= " + num148);
					if (num148 == Char.myCharz().charID)
					{
						if (GameScr.findMobInMap(mobId) != null)
						{
							Char.myCharz().mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
					else
					{
						@char = GameScr.findCharInMap(num148);
						if (@char != null && GameScr.findMobInMap(mobId) != null)
						{
							@char.mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
				}
				if (b66 == 2)
				{
					int num149 = msg.reader().readInt();
					int num150 = msg.reader().readInt();
					int num151 = msg.readInt3Byte();
					int cHPNew = msg.readInt3Byte();
					if (num149 == Char.myCharz().charID)
					{
						Res.outz("mob dame= " + num151);
						@char = GameScr.findCharInMap(num150);
						if (@char != null)
						{
							@char.cHPNew = cHPNew;
							if (Char.myCharz().mobMe.isBusyAttackSomeOne)
							{
								@char.doInjure(num151, 0, isCrit: false, isMob: true);
							}
							else
							{
								Char.myCharz().mobMe.dame = num151;
								Char.myCharz().mobMe.setAttack(@char);
							}
						}
					}
					else
					{
						mob = GameScr.findMobInMap(num149);
						if (mob != null)
						{
							if (num150 == Char.myCharz().charID)
							{
								Char.myCharz().cHPNew = cHPNew;
								if (mob.isBusyAttackSomeOne)
								{
									Char.myCharz().doInjure(num151, 0, isCrit: false, isMob: true);
								}
								else
								{
									mob.dame = num151;
									mob.setAttack(Char.myCharz());
								}
							}
							else
							{
								@char = GameScr.findCharInMap(num150);
								if (@char != null)
								{
									@char.cHPNew = cHPNew;
									if (mob.isBusyAttackSomeOne)
									{
										@char.doInjure(num151, 0, isCrit: false, isMob: true);
									}
									else
									{
										mob.dame = num151;
										mob.setAttack(@char);
									}
								}
							}
						}
					}
				}
				if (b66 == 3)
				{
					int num152 = msg.reader().readInt();
					int mobId2 = msg.reader().readInt();
					int hp = msg.readInt3Byte();
					int num153 = msg.readInt3Byte();
					@char = null;
					@char = ((Char.myCharz().charID != num152) ? GameScr.findCharInMap(num152) : Char.myCharz());
					if (@char != null)
					{
						mob = GameScr.findMobInMap(mobId2);
						if (@char.mobMe != null)
						{
							@char.mobMe.attackOtherMob(mob);
						}
						if (mob != null)
						{
							mob.hp = hp;
							if (num153 == 0)
							{
								mob.x = mob.xFirst;
								mob.y = mob.yFirst;
								GameScr.startFlyText(mResources.miss, mob.x, mob.y - mob.h, 0, -2, mFont.MISS);
							}
							else
							{
								GameScr.startFlyText("-" + num153, mob.x, mob.y - mob.h, 0, -2, mFont.ORANGE);
							}
						}
					}
				}
				if (b66 == 4)
				{
				}
				if (b66 == 5)
				{
					int num154 = msg.reader().readInt();
					sbyte b67 = msg.reader().readByte();
					int mobId3 = msg.reader().readInt();
					int num155 = msg.readInt3Byte();
					int hp2 = msg.readInt3Byte();
					@char = null;
					@char = ((num154 != Char.myCharz().charID) ? GameScr.findCharInMap(num154) : Char.myCharz());
					if (@char == null)
					{
						return;
					}
					if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
					{
						@char.setSkillPaint(GameScr.sks[b67], 0);
					}
					else
					{
						@char.setSkillPaint(GameScr.sks[b67], 1);
					}
					Mob mob8 = GameScr.findMobInMap(mobId3);
					if (@char.cx <= mob8.x)
					{
						@char.cdir = 1;
					}
					else
					{
						@char.cdir = -1;
					}
					@char.mobFocus = mob8;
					mob8.hp = hp2;
					GameCanvas.debug("SA83v2", 2);
					if (num155 == 0)
					{
						mob8.x = mob8.xFirst;
						mob8.y = mob8.yFirst;
						GameScr.startFlyText(mResources.miss, mob8.x, mob8.y - mob8.h, 0, -2, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num155, mob8.x, mob8.y - mob8.h, 0, -2, mFont.ORANGE);
					}
				}
				if (b66 == 6)
				{
					int num156 = msg.reader().readInt();
					if (num156 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe.startDie();
					}
					else
					{
						GameScr.findCharInMap(num156)?.mobMe.startDie();
					}
				}
				if (b66 != 7)
				{
					break;
				}
				int num157 = msg.reader().readInt();
				if (num157 == Char.myCharz().charID)
				{
					Char.myCharz().mobMe = null;
					for (int num158 = 0; num158 < GameScr.vMob.size(); num158++)
					{
						if (((Mob)GameScr.vMob.elementAt(num158)).mobId == num157)
						{
							GameScr.vMob.removeElementAt(num158);
						}
					}
					break;
				}
				@char = GameScr.findCharInMap(num157);
				for (int num159 = 0; num159 < GameScr.vMob.size(); num159++)
				{
					if (((Mob)GameScr.vMob.elementAt(num159)).mobId == num157)
					{
						GameScr.vMob.removeElementAt(num159);
					}
				}
				if (@char != null)
				{
					@char.mobMe = null;
				}
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
				sbyte b29 = msg.reader().readByte();
				GameCanvas.panel.mapNames = new string[b29];
				GameCanvas.panel.planetNames = new string[b29];
				for (int num72 = 0; num72 < b29; num72++)
				{
					GameCanvas.panel.mapNames[num72] = msg.reader().readUTF();
					GameCanvas.panel.planetNames[num72] = msg.reader().readUTF();
				}
				Pk9rXmap.ShowPanelMapTrans();
				//GameCanvas.panel.setTypeMapTrans();
				//GameCanvas.panel.show();
				break;
			}
			case -90:
			{
				sbyte b15 = msg.reader().readByte();
				Res.outz("type = " + b15);
				int num43 = msg.reader().readInt();
				if (b15 != -1)
				{
					short num44 = msg.reader().readShort();
					short num45 = msg.reader().readShort();
					short num46 = msg.reader().readShort();
					sbyte b16 = msg.reader().readByte();
					Res.outz("is Monkey = " + b16);
					if (Char.myCharz().charID == num43)
					{
						Char.myCharz().isMask = true;
						Char.myCharz().isMonkey = b16;
						if (Char.myCharz().isMonkey != 0)
						{
							Char.myCharz().isWaitMonkey = false;
							Char.myCharz().isLockMove = false;
						}
					}
					else if (GameScr.findCharInMap(num43) != null)
					{
						GameScr.findCharInMap(num43).isMask = true;
						GameScr.findCharInMap(num43).isMonkey = b16;
					}
					if (num44 != -1)
					{
						if (num43 == Char.myCharz().charID)
						{
							Char.myCharz().head = num44;
						}
						else if (GameScr.findCharInMap(num43) != null)
						{
							GameScr.findCharInMap(num43).head = num44;
						}
					}
					if (num45 != -1)
					{
						if (num43 == Char.myCharz().charID)
						{
							Char.myCharz().body = num45;
						}
						else if (GameScr.findCharInMap(num43) != null)
						{
							GameScr.findCharInMap(num43).body = num45;
						}
					}
					if (num46 != -1)
					{
						if (num43 == Char.myCharz().charID)
						{
							Char.myCharz().leg = num46;
						}
						else if (GameScr.findCharInMap(num43) != null)
						{
							GameScr.findCharInMap(num43).leg = num46;
						}
					}
				}
				if (b15 == -1)
				{
					if (Char.myCharz().charID == num43)
					{
						Char.myCharz().isMask = false;
						Char.myCharz().isMonkey = 0;
					}
					else if (GameScr.findCharInMap(num43) != null)
					{
						GameScr.findCharInMap(num43).isMask = false;
						GameScr.findCharInMap(num43).isMonkey = 0;
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
				createData(msg.reader(), isSaveRMS: true);
				msg.reader().reset();
				sbyte[] data2 = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data2);
				sbyte[] data3 = new sbyte[1] { GameScr.vcData };
				Rms.saveRMS("NRdataVersion", data3);
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
				sbyte b31 = msg.reader().readByte();
				Res.outz("server gui ve giao dich action = " + b31);
				if (b31 == 0)
				{
					int playerID = msg.reader().readInt();
					GameScr.gI().giaodich(playerID);
				}
				if (b31 == 1)
				{
					int num82 = msg.reader().readInt();
					Char char10 = GameScr.findCharInMap(num82);
					if (char10 == null)
					{
						return;
					}
					GameCanvas.panel.setTypeGiaoDich(char10);
					GameCanvas.panel.show();
					Service.gI().getPlayerMenu(num82);
				}
				if (b31 == 2)
				{
					sbyte b32 = msg.reader().readByte();
					for (int num83 = 0; num83 < GameCanvas.panel.vMyGD.size(); num83++)
					{
						Item item = (Item)GameCanvas.panel.vMyGD.elementAt(num83);
						if (item.indexUI == b32)
						{
							GameCanvas.panel.vMyGD.removeElement(item);
							break;
						}
					}
				}
				if (b31 == 5)
				{
				}
				if (b31 == 6)
				{
					GameCanvas.panel.isFriendLock = true;
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.isFriendLock = true;
					}
					GameCanvas.panel.vFriendGD.removeAllElements();
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.vFriendGD.removeAllElements();
					}
					int friendMoneyGD = msg.reader().readInt();
					sbyte b33 = msg.reader().readByte();
					Res.outz("item size = " + b33);
					for (int num84 = 0; num84 < b33; num84++)
					{
						Item item2 = new Item();
						item2.template = ItemTemplates.get(msg.reader().readShort());
						item2.quantity = msg.reader().readByte();
						int num85 = msg.reader().readUnsignedByte();
						if (num85 != 0)
						{
							item2.itemOption = new ItemOption[num85];
							for (int num86 = 0; num86 < item2.itemOption.Length; num86++)
							{
								int num87 = msg.reader().readUnsignedByte();
								int param4 = msg.reader().readUnsignedShort();
								if (num87 != -1)
								{
									item2.itemOption[num86] = new ItemOption(num87, param4);
									item2.compare = GameCanvas.panel.getCompare(item2);
								}
							}
						}
						if (GameCanvas.panel2 != null)
						{
							GameCanvas.panel2.vFriendGD.addElement(item2);
						}
						else
						{
							GameCanvas.panel.vFriendGD.addElement(item2);
						}
					}
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.setTabGiaoDich(isMe: false);
						GameCanvas.panel2.friendMoneyGD = friendMoneyGD;
					}
					else
					{
						GameCanvas.panel.friendMoneyGD = friendMoneyGD;
						if (GameCanvas.panel.currentTabIndex == 2)
						{
							GameCanvas.panel.setTabGiaoDich(isMe: false);
						}
					}
				}
				if (b31 == 7)
				{
					InfoDlg.hide();
					if (GameCanvas.panel.isShow)
					{
						GameCanvas.panel.hide();
					}
				}
				break;
			}
			case -85:
			{
				Res.outz("CAP CHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
				sbyte b36 = msg.reader().readByte();
				if (b36 == 0)
				{
					int num93 = msg.reader().readUnsignedShort();
					Res.outz("lent =" + num93);
					sbyte[] data4 = new sbyte[num93];
					msg.reader().read(ref data4, 0, num93);
					GameScr.imgCapcha = Image.createImage(data4, 0, num93);
					GameScr.gI().keyInput = "-----";
					GameScr.gI().strCapcha = msg.reader().readUTF();
					GameScr.gI().keyCapcha = new int[GameScr.gI().strCapcha.Length];
					GameScr.gI().mobCapcha = new Mob();
					GameScr.gI().right = null;
				}
				if (b36 == 1)
				{
					MobCapcha.isAttack = true;
				}
				if (b36 == 2)
				{
					MobCapcha.explode = true;
					GameScr.gI().right = GameScr.gI().cmdFocus;
				}
				break;
			}
			case -112:
			{
				sbyte b8 = msg.reader().readByte();
				if (b8 == 0)
				{
					sbyte mobIndex = msg.reader().readByte();
					GameScr.findMobInMap(mobIndex).clearBody();
				}
				if (b8 == 1)
				{
					sbyte mobIndex2 = msg.reader().readByte();
					GameScr.findMobInMap(mobIndex2).setBody(msg.reader().readShort());
				}
				break;
			}
			case -84:
			{
				int index2 = msg.reader().readUnsignedByte();
				Mob mob4 = null;
				try
				{
					mob4 = (Mob)GameScr.vMob.elementAt(index2);
				}
				catch (Exception)
				{
				}
				if (mob4 != null)
				{
					mob4.maxHp = msg.reader().readInt();
				}
				break;
			}
			case -83:
			{
				sbyte b60 = msg.reader().readByte();
				if (b60 == 0)
				{
					int num133 = msg.reader().readShort();
					int bgRID = msg.reader().readShort();
					int num134 = msg.reader().readUnsignedByte();
					int num135 = msg.reader().readInt();
					string text5 = msg.reader().readUTF();
					int num136 = msg.reader().readShort();
					int num137 = msg.reader().readShort();
					sbyte b61 = msg.reader().readByte();
					if (b61 == 1)
					{
						GameScr.gI().isRongNamek = true;
					}
					else
					{
						GameScr.gI().isRongNamek = false;
					}
					GameScr.gI().xR = num136;
					GameScr.gI().yR = num137;
					Res.outz("xR= " + num136 + " yR= " + num137 + " +++++++++++++++++++++++++++++++++++++++");
					if (Char.myCharz().charID == num135)
					{
						GameCanvas.panel.hideNow();
						GameScr.gI().activeRongThanEff(isMe: true);
					}
					else if (TileMap.mapID == num133 && TileMap.zoneID == num134)
					{
						GameScr.gI().activeRongThanEff(isMe: false);
					}
					else if (mGraphics.zoomLevel > 1)
					{
						GameScr.gI().doiMauTroi();
					}
					GameScr.gI().mapRID = num133;
					GameScr.gI().bgRID = bgRID;
					GameScr.gI().zoneRID = num134;
				}
				if (b60 == 1)
				{
					Res.outz("map RID = " + GameScr.gI().mapRID + " zone RID= " + GameScr.gI().zoneRID);
					Res.outz("map ID = " + TileMap.mapID + " zone ID= " + TileMap.zoneID);
					if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
					{
						GameScr.gI().hideRongThanEff();
					}
					else
					{
						GameScr.gI().isRongThanXuatHien = false;
						if (GameScr.gI().isRongNamek)
						{
							GameScr.gI().isRongNamek = false;
						}
					}
				}
				if (b60 != 2)
				{
				}
				break;
			}
			case -82:
			{
				sbyte b44 = msg.reader().readByte();
				TileMap.tileIndex = new int[b44][][];
				TileMap.tileType = new int[b44][];
				for (int num103 = 0; num103 < b44; num103++)
				{
					sbyte b45 = msg.reader().readByte();
					TileMap.tileType[num103] = new int[b45];
					TileMap.tileIndex[num103] = new int[b45][];
					for (int num104 = 0; num104 < b45; num104++)
					{
						TileMap.tileType[num103][num104] = msg.reader().readInt();
						sbyte b46 = msg.reader().readByte();
						TileMap.tileIndex[num103][num104] = new int[b46];
						for (int num105 = 0; num105 < b46; num105++)
						{
							TileMap.tileIndex[num103][num104][num105] = msg.reader().readByte();
						}
					}
				}
				break;
			}
			case -81:
			{
				sbyte b41 = msg.reader().readByte();
				if (b41 == 0)
				{
					string src = msg.reader().readUTF();
					string src2 = msg.reader().readUTF();
					GameCanvas.panel.setTypeCombine();
					GameCanvas.panel.combineInfo = mFont.tahoma_7b_blue.splitFontArray(src, Panel.WIDTH_PANEL);
					GameCanvas.panel.combineTopInfo = mFont.tahoma_7.splitFontArray(src2, Panel.WIDTH_PANEL);
					GameCanvas.panel.show();
				}
				if (b41 == 1)
				{
					GameCanvas.panel.vItemCombine.removeAllElements();
					sbyte b42 = msg.reader().readByte();
					for (int num98 = 0; num98 < b42; num98++)
					{
						sbyte b43 = msg.reader().readByte();
						for (int num99 = 0; num99 < Char.myCharz().arrItemBag.Length; num99++)
						{
							Item item3 = Char.myCharz().arrItemBag[num99];
							if (item3 != null && item3.indexUI == b43)
							{
								item3.isSelect = true;
								GameCanvas.panel.vItemCombine.addElement(item3);
							}
						}
					}
					if (GameCanvas.panel.isShow)
					{
						GameCanvas.panel.setTabCombine();
					}
				}
				if (b41 > 1)
				{
					int num100 = 21;
					for (int num101 = 0; num101 < GameScr.vNpc.size(); num101++)
					{
						Npc npc6 = (Npc)GameScr.vNpc.elementAt(num101);
						if (npc6.template.npcTemplateId == num100)
						{
							GameCanvas.panel.xS = npc6.cx - GameScr.cmx;
							GameCanvas.panel.yS = npc6.cy - GameScr.cmy;
							GameCanvas.panel.idNPC = num100;
							break;
						}
					}
				}
				if (b41 == 2)
				{
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b41 == 3)
				{
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b41 == 4)
				{
					short iconID = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(1);
				}
				if (b41 == 5)
				{
					short iconID2 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID2;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(2);
				}
				if (b41 == 6)
				{
					short iconID3 = msg.reader().readShort();
					short iconID4 = msg.reader().readShort();
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(3);
					GameCanvas.panel.iconID1 = iconID3;
					GameCanvas.panel.iconID3 = iconID4;
				}
				break;
			}
			case -80:
			{
				sbyte b17 = msg.reader().readByte();
				InfoDlg.hide();
				if (b17 == 0)
				{
					GameCanvas.panel.vFriend.removeAllElements();
					int num47 = msg.reader().readUnsignedByte();
					for (int num48 = 0; num48 < num47; num48++)
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
				if (b17 == 3)
				{
					MyVector vFriend = GameCanvas.panel.vFriend;
					int num49 = msg.reader().readInt();
					Res.outz("online offline id=" + num49);
					for (int num50 = 0; num50 < vFriend.size(); num50++)
					{
						InfoItem infoItem2 = (InfoItem)vFriend.elementAt(num50);
						if (infoItem2.charInfo != null && infoItem2.charInfo.charID == num49)
						{
							Res.outz("online= " + infoItem2.isOnline);
							infoItem2.isOnline = msg.reader().readBoolean();
							break;
						}
					}
				}
				if (b17 != 2)
				{
					break;
				}
				MyVector vFriend2 = GameCanvas.panel.vFriend;
				int num51 = msg.reader().readInt();
				for (int num52 = 0; num52 < vFriend2.size(); num52++)
				{
					InfoItem infoItem3 = (InfoItem)vFriend2.elementAt(num52);
					if (infoItem3.charInfo != null && infoItem3.charInfo.charID == num51)
					{
						vFriend2.removeElement(infoItem3);
						break;
					}
				}
				if (GameCanvas.panel.isShow)
				{
					GameCanvas.panel.setTabFriend();
				}
				break;
			}
			case -99:
			{
				InfoDlg.hide();
				sbyte b27 = msg.reader().readByte();
				if (b27 == 0)
				{
					GameCanvas.panel.vEnemy.removeAllElements();
					int num70 = msg.reader().readUnsignedByte();
					for (int num71 = 0; num71 < num70; num71++)
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
						bool flag7 = msg.reader().readBoolean();
						infoItem4.charInfo = char9;
						infoItem4.isOnline = flag7;
						Res.outz("isonline = " + flag7);
						GameCanvas.panel.vEnemy.addElement(infoItem4);
					}
					GameCanvas.panel.setTypeEnemy();
					GameCanvas.panel.show();
				}
				break;
			}
			case -79:
			{
				InfoDlg.hide();
				int num32 = msg.reader().readInt();
				Char charMenu = GameCanvas.panel.charMenu;
				if (charMenu == null)
				{
					return;
				}
				charMenu.cPower = msg.reader().readLong();
				charMenu.currStrLevel = msg.reader().readUTF();
				break;
			}
			case -93:
			{
				short num96 = msg.reader().readShort();
				BgItem.newSmallVersion = new sbyte[num96];
				for (int num97 = 0; num97 < num96; num97++)
				{
					BgItem.newSmallVersion[num97] = msg.reader().readByte();
				}
				break;
			}
			case -77:
			{
				short num13 = msg.reader().readShort();
				SmallImage.newSmallVersion = new sbyte[num13];
				SmallImage.maxSmall = num13;
				SmallImage.imgNew = new Small[num13];
				for (int i = 0; i < num13; i++)
				{
					SmallImage.newSmallVersion[i] = msg.reader().readByte();
				}
				break;
			}
			case -76:
			{
				sbyte b38 = msg.reader().readByte();
				if (b38 == 0)
				{
					sbyte b39 = msg.reader().readByte();
					if (b39 <= 0)
					{
						return;
					}
					Char.myCharz().arrArchive = new Archivement[b39];
					for (int num94 = 0; num94 < b39; num94++)
					{
						Char.myCharz().arrArchive[num94] = new Archivement();
						Char.myCharz().arrArchive[num94].info1 = num94 + 1 + ". " + msg.reader().readUTF();
						Char.myCharz().arrArchive[num94].info2 = msg.reader().readUTF();
						Char.myCharz().arrArchive[num94].money = msg.reader().readShort();
						Char.myCharz().arrArchive[num94].isFinish = msg.reader().readBoolean();
						Char.myCharz().arrArchive[num94].isRecieve = msg.reader().readBoolean();
					}
					GameCanvas.panel.setTypeArchivement();
					GameCanvas.panel.show();
				}
				else if (b38 == 1)
				{
					int num95 = msg.reader().readUnsignedByte();
					if (Char.myCharz().arrArchive[num95] != null)
					{
						Char.myCharz().arrArchive[num95].isRecieve = true;
					}
				}
				break;
			}
			case -74:
			{
				if (ServerListScreen.stopDownload)
				{
					return;
				}
				if (!GameCanvas.isGetResourceFromServer())
				{
					Service.gI().getResource(3, null);
					SmallImage.loadBigRMS();
					SplashScr.imgLogo = null;
					if (Rms.loadRMSString("acc") != null || Rms.loadRMSString("userAo" + ServerListScreen.ipSelect) != null)
					{
						LoginScr.isContinueToLogin = true;
					}
					GameCanvas.loginScr = new LoginScr();
					GameCanvas.loginScr.switchToMe();
					return;
				}
				bool flag10 = true;
				sbyte b68 = msg.reader().readByte();
				Res.outz("action = " + b68);
				if (b68 == 0)
				{
					int num160 = msg.reader().readInt();
					string text6 = Rms.loadRMSString("ResVersion");
					int num161 = ((text6 == null || !(text6 != string.Empty)) ? (-1) : int.Parse(text6));
					if (num161 == -1 || num161 != num160)
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
						{
							GameCanvas.serverScreen.switchToMe();
						}
					}
				}
				if (b68 == 1)
				{
					ServerListScreen.strWait = mResources.downloading_data;
					short num162 = (short)(ServerListScreen.nBig = msg.reader().readShort());
					Service.gI().getResource(2, null);
				}
				if (b68 == 2)
				{
					try
					{
						isLoadingData = true;
						GameCanvas.endDlg();
						ServerListScreen.demPercent++;
						ServerListScreen.percent = ServerListScreen.demPercent * 100 / ServerListScreen.nBig;
						string original = msg.reader().readUTF();
						string[] array16 = Res.split(original, "/", 0);
						string filename = "x" + mGraphics.zoomLevel + array16[array16.Length - 1];
						int num163 = msg.reader().readInt();
						sbyte[] data5 = new sbyte[num163];
						msg.reader().read(ref data5, 0, num163);
						Rms.saveRMS(filename, data5);
					}
					catch (Exception)
					{
						GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
					}
				}
				if (b68 == 3 && flag10)
				{
					isLoadingData = false;
					int num164 = msg.reader().readInt();
					Res.outz("last version= " + num164);
					Rms.saveRMSString("ResVersion", num164 + string.Empty);
					Service.gI().getResource(3, null);
					GameCanvas.endDlg();
					SplashScr.imgLogo = null;
					SmallImage.loadBigRMS();
					mSystem.gcc();
					ServerListScreen.bigOk = true;
					ServerListScreen.loadScreen = true;
					GameScr.gI().loadGameScr();
					if (GameCanvas.currentScreen != GameCanvas.loginScr)
					{
						GameCanvas.serverScreen.switchToMe();
					}
				}
				break;
			}
			case -43:
			{
				sbyte itemAction = msg.reader().readByte();
				sbyte where = msg.reader().readByte();
				sbyte index = msg.reader().readByte();
				string info = msg.reader().readUTF();
				GameCanvas.panel.itemRequest(itemAction, info, where, index);
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
				int num88 = msg.reader().readUnsignedByte();
				sbyte b34 = msg.reader().readByte();
				if (b34 <= 0)
				{
					break;
				}
				ClanImage clanImage = ClanImage.getClanImage((sbyte)num88);
				if (clanImage == null)
				{
					break;
				}
				clanImage.idImage = new short[b34];
				for (int num89 = 0; num89 < b34; num89++)
				{
					clanImage.idImage[num89] = msg.reader().readShort();
					if (clanImage.idImage[num89] > 0)
					{
						SmallImage.vKeys.addElement(clanImage.idImage[num89] + string.Empty);
					}
				}
				break;
			}
			case -65:
			{
				Res.outz("TELEPORT ...................................................");
				InfoDlg.hide();
				int num18 = msg.reader().readInt();
				sbyte b7 = msg.reader().readByte();
				if (b7 == 0)
				{
					break;
				}
				if (Char.myCharz().charID == num18)
				{
					isStopReadMessage = true;
					GameScr.lockTick = 500;
					GameScr.gI().center = null;
					if (b7 == 0 || b7 == 1 || b7 == 3)
					{
						Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 0, isMe: true, (b7 != 1) ? b7 : Char.myCharz().cgender);
						Teleport.addTeleport(p);
					}
					if (b7 == 2)
					{
						GameScr.lockTick = 50;
						Char.myCharz().hide();
					}
				}
				else
				{
					Char char6 = GameScr.findCharInMap(num18);
					if ((b7 == 0 || b7 == 1 || b7 == 3) && char6 != null)
					{
						char6.isUsePlane = true;
						Teleport teleport = new Teleport(char6.cx, char6.cy, char6.head, char6.cdir, 0, isMe: false, (b7 != 1) ? b7 : char6.cgender);
						teleport.id = num18;
						Teleport.addTeleport(teleport);
					}
					if (b7 == 2)
					{
						char6.hide();
					}
				}
				break;
			}
			case -64:
			{
				int num14 = msg.reader().readInt();
				int bag = msg.reader().readUnsignedByte();
				if (num14 == Char.myCharz().charID)
				{
					Char.myCharz().bag = bag;
				}
				else if (GameScr.findCharInMap(num14) != null)
				{
					GameScr.findCharInMap(num14).bag = bag;
				}
				break;
			}
			case -63:
			{
				Res.outz("GET BAG");
				int num119 = msg.reader().readUnsignedByte();
				sbyte b51 = msg.reader().readByte();
				ClanImage clanImage3 = new ClanImage();
				clanImage3.ID = num119;
				if (b51 > 0)
				{
					clanImage3.idImage = new short[b51];
					for (int num120 = 0; num120 < b51; num120++)
					{
						clanImage3.idImage[num120] = msg.reader().readShort();
						Res.outz("ID=  " + num119 + " frame= " + clanImage3.idImage[num120]);
					}
					ClanImage.idImages.put(num119 + string.Empty, clanImage3);
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
				{
					GameCanvas.panel.initTabClans();
				}
				break;
			case -53:
			{
				Res.outz("MY CLAN INFO");
				InfoDlg.hide();
				bool flag3 = false;
				int num34 = msg.reader().readInt();
				Res.outz("clanId= " + num34);
				if (num34 == -1)
				{
					flag3 = true;
					Char.myCharz().clan = null;
					ClanMessage.vMessage.removeAllElements();
					if (GameCanvas.panel.member != null)
					{
						GameCanvas.panel.member.removeAllElements();
					}
					if (GameCanvas.panel.myMember != null)
					{
						GameCanvas.panel.myMember.removeAllElements();
					}
					if (GameCanvas.currentScreen == GameScr.gI())
					{
						GameCanvas.panel.setTabClans();
					}
					return;
				}
				GameCanvas.panel.tabIcon = null;
				if (Char.myCharz().clan == null)
				{
					Char.myCharz().clan = new Clan();
				}
				Char.myCharz().clan.ID = num34;
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
				for (int num35 = 0; num35 < Char.myCharz().clan.currMember; num35++)
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
					member.curClanPoint = msg.reader().readInt();
					member.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.myMember.addElement(member);
				}
				int num36 = msg.reader().readUnsignedByte();
				for (int num37 = 0; num37 < num36; num37++)
				{
					readClanMsg(msg, -1);
				}
				if (GameCanvas.panel.isSearchClan || GameCanvas.panel.isViewMember || GameCanvas.panel.isMessage)
				{
					GameCanvas.panel.setTabClans();
				}
				if (flag3)
				{
					GameCanvas.panel.setTabClans();
				}
				break;
			}
			case -52:
			{
				sbyte b69 = msg.reader().readByte();
				if (b69 == 0)
				{
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
					if (GameCanvas.panel.myMember == null)
					{
						GameCanvas.panel.myMember = new MyVector();
					}
					GameCanvas.panel.myMember.addElement(member3);
					GameCanvas.panel.initTabClans();
				}
				if (b69 == 1)
				{
					GameCanvas.panel.myMember.removeElementAt(msg.reader().readByte());
					GameCanvas.panel.currentListLength--;
					GameCanvas.panel.initTabClans();
				}
				if (b69 != 2)
				{
					break;
				}
				Member member4 = new Member();
				member4.ID = msg.reader().readInt();
				member4.head = msg.reader().readShort();
				member4.headICON = msg.reader().readShort();
				member4.leg = msg.reader().readShort();
				member4.body = msg.reader().readShort();
				member4.name = msg.reader().readUTF();
				member4.role = msg.reader().readByte();
				member4.powerPoint = msg.reader().readUTF();
				member4.donate = msg.reader().readInt();
				member4.receive_donate = msg.reader().readInt();
				member4.clanPoint = msg.reader().readInt();
				member4.joinTime = NinjaUtil.getDate(msg.reader().readInt());
				for (int num165 = 0; num165 < GameCanvas.panel.myMember.size(); num165++)
				{
					Member member5 = (Member)GameCanvas.panel.myMember.elementAt(num165);
					if (member5.ID == member4.ID)
					{
						if (Char.myCharz().charID == member4.ID)
						{
							Char.myCharz().role = member4.role;
						}
						Member o2 = member4;
						GameCanvas.panel.myMember.removeElement(member5);
						GameCanvas.panel.myMember.insertElementAt(o2, num165);
						return;
					}
				}
				break;
			}
			case -50:
			{
				InfoDlg.hide();
				GameCanvas.panel.member = new MyVector();
				sbyte b65 = msg.reader().readByte();
				for (int num144 = 0; num144 < b65; num144++)
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
					GameCanvas.panel.member.addElement(member2);
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
				sbyte b52 = msg.reader().readByte();
				Res.outz("clan = " + b52);
				if (b52 == 0)
				{
					GameCanvas.panel.clanReport = mResources.cannot_find_clan;
					GameCanvas.panel.clans = null;
				}
				else
				{
					GameCanvas.panel.clans = new Clan[b52];
					Res.outz("clan search lent= " + GameCanvas.panel.clans.Length);
					for (int num121 = 0; num121 < GameCanvas.panel.clans.Length; num121++)
					{
						GameCanvas.panel.clans[num121] = new Clan();
						GameCanvas.panel.clans[num121].ID = msg.reader().readInt();
						GameCanvas.panel.clans[num121].name = msg.reader().readUTF();
						GameCanvas.panel.clans[num121].slogan = msg.reader().readUTF();
						GameCanvas.panel.clans[num121].imgID = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num121].powerPoint = msg.reader().readUTF();
						GameCanvas.panel.clans[num121].leaderName = msg.reader().readUTF();
						GameCanvas.panel.clans[num121].currMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num121].maxMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num121].date = msg.reader().readInt();
					}
				}
				GameCanvas.panel.isSearchClan = true;
				GameCanvas.panel.isViewMember = false;
				GameCanvas.panel.isMessage = false;
				if (GameCanvas.panel.isSearchClan)
				{
					GameCanvas.panel.initTabClans();
				}
				break;
			}
			case -46:
			{
				InfoDlg.hide();
				sbyte b35 = msg.reader().readByte();
				if (b35 == 1 || b35 == 3)
				{
					GameCanvas.endDlg();
					ClanImage.vClanImage.removeAllElements();
					int num90 = msg.reader().readUnsignedByte();
					for (int num91 = 0; num91 < num90; num91++)
					{
						ClanImage clanImage2 = new ClanImage();
						clanImage2.ID = msg.reader().readUnsignedByte();
						clanImage2.name = msg.reader().readUTF();
						clanImage2.xu = msg.reader().readInt();
						clanImage2.luong = msg.reader().readInt();
						if (!ClanImage.isExistClanImage(clanImage2.ID))
						{
							ClanImage.addClanImage(clanImage2);
							continue;
						}
						ClanImage.getClanImage((sbyte)clanImage2.ID).name = clanImage2.name;
						ClanImage.getClanImage((sbyte)clanImage2.ID).xu = clanImage2.xu;
						ClanImage.getClanImage((sbyte)clanImage2.ID).luong = clanImage2.luong;
					}
					if (Char.myCharz().clan != null)
					{
						GameCanvas.panel.changeIcon();
					}
				}
				if (b35 == 4)
				{
					Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
					Char.myCharz().clan.slogan = msg.reader().readUTF();
				}
				break;
			}
			case -61:
			{
				int num73 = msg.reader().readInt();
				if (num73 != Char.myCharz().charID)
				{
					if (GameScr.findCharInMap(num73) != null)
					{
						GameScr.findCharInMap(num73).clanID = msg.reader().readInt();
						if (GameScr.findCharInMap(num73).clanID == -2)
						{
							GameScr.findCharInMap(num73).isCopy = true;
						}
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
				bool flag8 = msg.reader().readBool();
				Res.outz("isRes= " + flag8);
				if (!flag8)
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
				{
					GameScr.gI().initSelectChar();
				}
				BgItem.clearHashTable();
				GameCanvas.endDlg();
				CreateCharScr.isCreateChar = true;
				CreateCharScr.gI().switchToMe();
				break;
			case -107:
			{
				sbyte b9 = msg.reader().readByte();
				if (b9 == 0)
				{
					Char.myCharz().havePet = false;
				}
				if (b9 == 1)
				{
					Char.myCharz().havePet = true;
				}
				if (b9 != 2)
				{
					break;
				}
				InfoDlg.hide();
				Char.myPetz().head = msg.reader().readShort();
				Char.myPetz().setDefaultPart();
				int num20 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num20);
				Char.myPetz().arrItemBody = new Item[num20];
				for (int k = 0; k < num20; k++)
				{
					short num21 = msg.reader().readShort();
					Res.outz("template id= " + num21);
					if (num21 == -1)
					{
						continue;
					}
					Res.outz("1");
					Char.myPetz().arrItemBody[k] = new Item();
					Char.myPetz().arrItemBody[k].template = ItemTemplates.get(num21);
					int num22 = Char.myPetz().arrItemBody[k].template.type;
					Char.myPetz().arrItemBody[k].quantity = msg.reader().readInt();
					Res.outz("3");
					Char.myPetz().arrItemBody[k].info = msg.reader().readUTF();
					Char.myPetz().arrItemBody[k].content = msg.reader().readUTF();
					int num23 = msg.reader().readUnsignedByte();
					Res.outz("option size= " + num23);
					if (num23 != 0)
					{
						Char.myPetz().arrItemBody[k].itemOption = new ItemOption[num23];
						for (int l = 0; l < Char.myPetz().arrItemBody[k].itemOption.Length; l++)
						{
							int num24 = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							if (num24 != -1)
							{
								Char.myPetz().arrItemBody[k].itemOption[l] = new ItemOption(num24, param);
							}
						}
					}
					switch (num22)
					{
					case 0:
						Char.myPetz().body = Char.myPetz().arrItemBody[k].template.part;
						break;
					case 1:
						Char.myPetz().leg = Char.myPetz().arrItemBody[k].template.part;
						break;
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
				for (int m = 0; m < Char.myPetz().arrPetSkill.Length; m++)
				{
					short num25 = msg.reader().readShort();
					if (num25 != -1)
					{
						Char.myPetz().arrPetSkill[m] = Skills.get(num25);
						continue;
					}
					Char.myPetz().arrPetSkill[m] = new Skill();
					Char.myPetz().arrPetSkill[m].template = null;
					Char.myPetz().arrPetSkill[m].moreInfo = msg.reader().readUTF();
				}
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					GameCanvas.panel2 = new Panel();
					GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
					GameCanvas.panel2.setTypeBodyOnly();
					GameCanvas.panel2.show();
					GameCanvas.panel.setTypePetMain();
					GameCanvas.panel.show();
				}
				else
				{
					GameCanvas.panel.tabName[21] = mResources.petMainTab;
					GameCanvas.panel.setTypePetMain();
					GameCanvas.panel.show();
				}
				break;
			}
			case -37:
			{
				sbyte b10 = msg.reader().readByte();
				Res.outz("cAction= " + b10);
				if (b10 != 0)
				{
					break;
				}
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().setDefaultPart();
				int num26 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num26);
				Char.myCharz().arrItemBody = new Item[num26];
				for (int n = 0; n < num26; n++)
				{
					short num27 = msg.reader().readShort();
					if (num27 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBody[n] = new Item();
					Char.myCharz().arrItemBody[n].template = ItemTemplates.get(num27);
					int num28 = Char.myCharz().arrItemBody[n].template.type;
					Char.myCharz().arrItemBody[n].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBody[n].info = msg.reader().readUTF();
					Char.myCharz().arrItemBody[n].content = msg.reader().readUTF();
					int num29 = msg.reader().readUnsignedByte();
					if (num29 != 0)
					{
						Char.myCharz().arrItemBody[n].itemOption = new ItemOption[num29];
						for (int num30 = 0; num30 < Char.myCharz().arrItemBody[n].itemOption.Length; num30++)
						{
							int num31 = msg.reader().readUnsignedByte();
							int param2 = msg.reader().readUnsignedShort();
							if (num31 != -1)
							{
								Char.myCharz().arrItemBody[n].itemOption[num30] = new ItemOption(num31, param2);
							}
						}
					}
					switch (num28)
					{
					case 0:
						Char.myCharz().body = Char.myCharz().arrItemBody[n].template.part;
						break;
					case 1:
						Char.myCharz().leg = Char.myCharz().arrItemBody[n].template.part;
						break;
					}
				}
				break;
			}
			case -36:
			{
				sbyte b62 = msg.reader().readByte();
				Res.outz("cAction= " + b62);
				if (b62 == 0)
				{
					int num138 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBag = new Item[num138];
					GameScr.hpPotion = 0;
					Res.outz("numC=" + num138);
					for (int num139 = 0; num139 < num138; num139++)
					{
						short num140 = msg.reader().readShort();
						if (num140 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBag[num139] = new Item();
						Char.myCharz().arrItemBag[num139].template = ItemTemplates.get(num140);
						Char.myCharz().arrItemBag[num139].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBag[num139].info = msg.reader().readUTF();
						Char.myCharz().arrItemBag[num139].content = msg.reader().readUTF();
						Char.myCharz().arrItemBag[num139].indexUI = num139;
						int num141 = msg.reader().readUnsignedByte();
						if (num141 != 0)
						{
							Char.myCharz().arrItemBag[num139].itemOption = new ItemOption[num141];
							for (int num142 = 0; num142 < Char.myCharz().arrItemBag[num139].itemOption.Length; num142++)
							{
								int num143 = msg.reader().readUnsignedByte();
								int param6 = msg.reader().readUnsignedShort();
								if (num143 != -1)
								{
									Char.myCharz().arrItemBag[num139].itemOption[num142] = new ItemOption(num143, param6);
								}
							}
							Char.myCharz().arrItemBag[num139].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemBag[num139]);
						}
						if (Char.myCharz().arrItemBag[num139].template.type == 11)
						{
						}
						if (Char.myCharz().arrItemBag[num139].template.type == 6)
						{
							GameScr.hpPotion += Char.myCharz().arrItemBag[num139].quantity;
						}
					}
				}
				if (b62 == 2)
				{
					sbyte b63 = msg.reader().readByte();
					sbyte b64 = msg.reader().readByte();
					int quantity = Char.myCharz().arrItemBag[b63].quantity;
					Char.myCharz().arrItemBag[b63].quantity = b64;
					if (Char.myCharz().arrItemBag[b63].quantity < quantity && Char.myCharz().arrItemBag[b63].template.type == 6)
					{
						GameScr.hpPotion -= quantity - Char.myCharz().arrItemBag[b63].quantity;
					}
					if (Char.myCharz().arrItemBag[b63].quantity == 0)
					{
						Char.myCharz().arrItemBag[b63] = null;
					}
				}
				break;
			}
			case -35:
			{
				sbyte b47 = msg.reader().readByte();
				Res.outz("cAction= " + b47);
				if (b47 == 0)
				{
					int num108 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBox = new Item[num108];
					GameCanvas.panel.hasUse = 0;
					for (int num109 = 0; num109 < num108; num109++)
					{
						short num110 = msg.reader().readShort();
						if (num110 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBox[num109] = new Item();
						Char.myCharz().arrItemBox[num109].template = ItemTemplates.get(num110);
						Char.myCharz().arrItemBox[num109].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBox[num109].info = msg.reader().readUTF();
						Char.myCharz().arrItemBox[num109].content = msg.reader().readUTF();
						int num111 = msg.reader().readUnsignedByte();
						if (num111 != 0)
						{
							Char.myCharz().arrItemBox[num109].itemOption = new ItemOption[num111];
							for (int num112 = 0; num112 < Char.myCharz().arrItemBox[num109].itemOption.Length; num112++)
							{
								int num113 = msg.reader().readUnsignedByte();
								int param5 = msg.reader().readUnsignedShort();
								if (num113 != -1)
								{
									Char.myCharz().arrItemBox[num109].itemOption[num112] = new ItemOption(num113, param5);
								}
							}
						}
						GameCanvas.panel.hasUse++;
					}
				}
				if (b47 == 1)
				{
					bool isBoxClan = false;
					try
					{
						sbyte b48 = msg.reader().readByte();
						if (b48 == 1)
						{
							isBoxClan = true;
						}
					}
					catch (Exception)
					{
					}
					GameCanvas.panel.setTypeBox();
					GameCanvas.panel.isBoxClan = isBoxClan;
					GameCanvas.panel.show();
				}
				if (b47 == 2)
				{
					sbyte b49 = msg.reader().readByte();
					sbyte b50 = msg.reader().readByte();
					Char.myCharz().arrItemBox[b49].quantity = b50;
					if (Char.myCharz().arrItemBox[b49].quantity == 0)
					{
						Char.myCharz().arrItemBox[b49] = null;
					}
				}
				break;
			}
			case -45:
			{
				sbyte b54 = msg.reader().readByte();
				int num125 = msg.reader().readInt();
				short num126 = msg.reader().readShort();
				Res.outz("skill type= " + b54 + "   player use= " + num125);
				if (b54 == 0)
				{
					Res.outz("id use= " + num125);
					if (Char.myCharz().charID != num125)
					{
						@char = GameScr.findCharInMap(num125);
						if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
						{
							@char.setSkillPaint(GameScr.sks[num126], 0);
						}
						else
						{
							@char.setSkillPaint(GameScr.sks[num126], 1);
							@char.delayFall = 20;
						}
					}
					else
					{
						Char.myCharz().saveLoadPreviousSkill();
						Res.outz("LOAD LAST SKILL");
					}
					sbyte b55 = msg.reader().readByte();
					Res.outz("npc size= " + b55);
					for (int num127 = 0; num127 < b55; num127++)
					{
						sbyte b56 = msg.reader().readByte();
						sbyte b57 = msg.reader().readByte();
						Res.outz("index= " + b56);
						if (num126 >= 42 && num126 <= 48)
						{
							((Mob)GameScr.vMob.elementAt(b56)).isFreez = true;
							((Mob)GameScr.vMob.elementAt(b56)).seconds = b57;
							((Mob)GameScr.vMob.elementAt(b56)).last = (((Mob)GameScr.vMob.elementAt(b56)).cur = mSystem.currentTimeMillis());
						}
					}
					sbyte b58 = msg.reader().readByte();
					for (int num128 = 0; num128 < b58; num128++)
					{
						int num129 = msg.reader().readInt();
						sbyte b59 = msg.reader().readByte();
						Res.outz("player ID= " + num129 + " my ID= " + Char.myCharz().charID);
						if (num126 < 42 || num126 > 48)
						{
							continue;
						}
						if (num129 == Char.myCharz().charID)
						{
							if (!Char.myCharz().isFlyAndCharge && !Char.myCharz().isStandAndCharge)
							{
								GameScr.gI().isFreez = true;
								Char.myCharz().isFreez = true;
								Char.myCharz().freezSeconds = b59;
								Char.myCharz().lastFreez = (Char.myCharz().currFreez = mSystem.currentTimeMillis());
								Char.myCharz().isLockMove = true;
							}
						}
						else
						{
							@char = GameScr.findCharInMap(num129);
							if (@char != null && !@char.isFlyAndCharge && !@char.isStandAndCharge)
							{
								@char.isFreez = true;
								@char.seconds = b59;
								@char.freezSeconds = b59;
								@char.lastFreez = (GameScr.findCharInMap(num129).currFreez = mSystem.currentTimeMillis());
							}
						}
					}
				}
				if (b54 == 1 && num125 != Char.myCharz().charID)
				{
					GameScr.findCharInMap(num125).isCharge = true;
				}
				if (b54 == 3)
				{
					if (num125 == Char.myCharz().charID)
					{
						Char.myCharz().isCharge = false;
						SoundMn.gI().taitaoPause();
						Char.myCharz().saveLoadPreviousSkill();
					}
					else
					{
						GameScr.findCharInMap(num125).isCharge = false;
					}
				}
				if (b54 == 4)
				{
					if (num125 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort() - 1000;
						Char.myCharz().last = mSystem.currentTimeMillis();
						Res.outz("second= " + Char.myCharz().seconds + " last= " + Char.myCharz().last);
					}
					else if (GameScr.findCharInMap(num125) != null)
					{
						switch (GameScr.findCharInMap(num125).cgender)
						{
						case 0:
							GameScr.findCharInMap(num125).useChargeSkill(isGround: false);
							break;
						case 1:
							GameScr.findCharInMap(num125).useChargeSkill(isGround: true);
							break;
						}
						GameScr.findCharInMap(num125).skillTemplateId = num126;
						GameScr.findCharInMap(num125).isUseSkillAfterCharge = true;
						GameScr.findCharInMap(num125).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num125).last = mSystem.currentTimeMillis();
					}
				}
				if (b54 == 5)
				{
					if (num125 == Char.myCharz().charID)
					{
						Char.myCharz().stopUseChargeSkill();
					}
					else if (GameScr.findCharInMap(num125) != null)
					{
						GameScr.findCharInMap(num125).stopUseChargeSkill();
					}
				}
				if (b54 == 6)
				{
					if (num125 == Char.myCharz().charID)
					{
						Char.myCharz().setAutoSkillPaint(GameScr.sks[num126], 0);
					}
					else if (GameScr.findCharInMap(num125) != null)
					{
						GameScr.findCharInMap(num125).setAutoSkillPaint(GameScr.sks[num126], 0);
						SoundMn.gI().gong();
					}
				}
				if (b54 == 7)
				{
					if (num125 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort();
						Res.outz("second = " + Char.myCharz().seconds);
						Char.myCharz().last = mSystem.currentTimeMillis();
					}
					else if (GameScr.findCharInMap(num125) != null)
					{
						GameScr.findCharInMap(num125).useChargeSkill(isGround: true);
						GameScr.findCharInMap(num125).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num125).last = mSystem.currentTimeMillis();
						SoundMn.gI().gong();
					}
				}
				if (b54 == 8 && num125 != Char.myCharz().charID && GameScr.findCharInMap(num125) != null)
				{
					GameScr.findCharInMap(num125).setAutoSkillPaint(GameScr.sks[num126], 0);
				}
				break;
			}
			case -44:
			{
				bool flag6 = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag6 = true;
				}
				sbyte b24 = msg.reader().readByte();
				int num61 = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop = new Item[num61][];
				GameCanvas.panel.shopTabName = new string[num61 + ((!flag6) ? 1 : 0)][];
				for (int num62 = 0; num62 < GameCanvas.panel.shopTabName.Length; num62++)
				{
					GameCanvas.panel.shopTabName[num62] = new string[2];
				}
				if (b24 == 2)
				{
					GameCanvas.panel.maxPageShop = new int[num61];
					GameCanvas.panel.currPageShop = new int[num61];
				}
				if (!flag6)
				{
					GameCanvas.panel.shopTabName[num61] = mResources.inventory;
				}
				for (int num63 = 0; num63 < num61; num63++)
				{
					string[] array7 = Res.split(msg.reader().readUTF(), "\n", 0);
					if (b24 == 2)
					{
						GameCanvas.panel.maxPageShop[num63] = msg.reader().readUnsignedByte();
					}
					if (array7.Length == 2)
					{
						GameCanvas.panel.shopTabName[num63] = array7;
					}
					if (array7.Length == 1)
					{
						GameCanvas.panel.shopTabName[num63][0] = array7[0];
						GameCanvas.panel.shopTabName[num63][1] = string.Empty;
					}
					int num64 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[num63] = new Item[num64];
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					if (b24 == 1)
					{
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy2;
					}
					for (int num65 = 0; num65 < num64; num65++)
					{
						short num66 = msg.reader().readShort();
						if (num66 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemShop[num63][num65] = new Item();
						Char.myCharz().arrItemShop[num63][num65].template = ItemTemplates.get(num66);
						Res.outz("name " + num63 + " = " + Char.myCharz().arrItemShop[num63][num65].template.name + " id templat= " + Char.myCharz().arrItemShop[num63][num65].template.id);
						if (b24 == 8)
						{
							Char.myCharz().arrItemShop[num63][num65].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num63][num65].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num63][num65].quantity = msg.reader().readShort();
						}
						else if (b24 == 4)
						{
							Char.myCharz().arrItemShop[num63][num65].reason = msg.reader().readUTF();
						}
						else if (b24 == 0)
						{
							Char.myCharz().arrItemShop[num63][num65].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num63][num65].buyGold = msg.reader().readInt();
						}
						else if (b24 == 1)
						{
							Char.myCharz().arrItemShop[num63][num65].powerRequire = msg.reader().readLong();
						}
						else if (b24 == 2)
						{
							Char.myCharz().arrItemShop[num63][num65].itemId = msg.reader().readShort();
							Char.myCharz().arrItemShop[num63][num65].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num63][num65].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num63][num65].buyType = msg.reader().readByte();
							Char.myCharz().arrItemShop[num63][num65].quantity = msg.reader().readByte();
							Char.myCharz().arrItemShop[num63][num65].isMe = msg.reader().readByte();
						}
						else if (b24 == 3)
						{
							Char.myCharz().arrItemShop[num63][num65].isBuySpec = true;
							Char.myCharz().arrItemShop[num63][num65].iconSpec = msg.reader().readShort();
							Char.myCharz().arrItemShop[num63][num65].buySpec = msg.reader().readInt();
						}
						int num67 = msg.reader().readUnsignedByte();
						if (num67 != 0)
						{
							Char.myCharz().arrItemShop[num63][num65].itemOption = new ItemOption[num67];
							for (int num68 = 0; num68 < Char.myCharz().arrItemShop[num63][num65].itemOption.Length; num68++)
							{
								int num69 = msg.reader().readUnsignedByte();
								int param3 = msg.reader().readUnsignedShort();
								if (num69 != -1)
								{
									Char.myCharz().arrItemShop[num63][num65].itemOption[num68] = new ItemOption(num69, param3);
									Char.myCharz().arrItemShop[num63][num65].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[num63][num65]);
								}
							}
						}
						sbyte b25 = msg.reader().readByte();
						Char.myCharz().arrItemShop[num63][num65].newItem = ((b25 != 0) ? true : false);
						sbyte b26 = msg.reader().readByte();
						if (b26 == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[num63][num65].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
					}
				}
				if (flag6)
				{
					if (b24 != 2)
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
				if (b24 == 2)
				{
					string[][] array8 = GameCanvas.panel.tabName[1];
					if (flag6)
					{
						GameCanvas.panel.tabName[1] = new string[4][]
						{
							array8[0],
							array8[1],
							array8[2],
							array8[3]
						};
					}
					else
					{
						GameCanvas.panel.tabName[1] = new string[5][]
						{
							array8[0],
							array8[1],
							array8[2],
							array8[3],
							array8[4]
						};
					}
				}
				GameCanvas.panel.setTypeShop(b24);
				GameCanvas.panel.show();
				break;
			}
			case -41:
			{
				sbyte b20 = msg.reader().readByte();
				Char.myCharz().strLevel = new string[b20];
				for (int num54 = 0; num54 < b20; num54++)
				{
					string text2 = msg.reader().readUTF();
					Char.myCharz().strLevel[num54] = text2;
				}
				Res.outz("---   xong  level caption cmd : " + msg.command);
				break;
			}
			case -34:
			{
				sbyte b11 = msg.reader().readByte();
				Res.outz("act= " + b11);
				if (b11 == 0 && GameScr.gI().magicTree != null)
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
					sbyte b12 = msg.reader().readByte();
					magicTree.peaPostionX = new int[b12];
					magicTree.peaPostionY = new int[b12];
					for (int num33 = 0; num33 < b12; num33++)
					{
						magicTree.peaPostionX[num33] = msg.reader().readByte();
						magicTree.peaPostionY[num33] = msg.reader().readByte();
					}
					magicTree.isUpdate = msg.reader().readBool();
					magicTree.last = (magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
				}
				if (b11 == 1)
				{
					myVector = new MyVector();
					try
					{
						while (msg.reader().available() > 0)
						{
							string caption = msg.reader().readUTF();
							myVector.addElement(new Command(caption, GameCanvas.instance, 888392, null));
						}
					}
					catch (Exception ex5)
					{
						Cout.println("Loi MAGIC_TREE " + ex5.ToString());
					}
					GameCanvas.menu.startAt(myVector, 3);
				}
				if (b11 == 2)
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
				int num15 = msg.reader().readByte();
				sbyte b6 = msg.reader().readByte();
				if (b6 != 0)
				{
					Mob.arrMobTemplate[num15].data.readDataNewBoss(NinjaUtil.readByteArray(msg), b6);
				}
				else
				{
					Mob.arrMobTemplate[num15].data.readData(NinjaUtil.readByteArray(msg));
				}
				for (int j = 0; j < GameScr.vMob.size(); j++)
				{
					mob = (Mob)GameScr.vMob.elementAt(j);
					if (mob.templateId == num15)
					{
						mob.w = Mob.arrMobTemplate[num15].data.width;
						mob.h = Mob.arrMobTemplate[num15].data.height;
					}
				}
				sbyte[] array3 = NinjaUtil.readByteArray(msg);
				Image img = Image.createImage(array3, 0, array3.Length);
				Mob.arrMobTemplate[num15].data.img = img;
				int num16 = msg.reader().readByte();
				Mob.arrMobTemplate[num15].data.typeData = num16;
				if (num16 == 1 || num16 == 2)
				{
					readFrameBoss(msg, num15);
				}
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
				int num9 = msg.reader().readInt();
				sbyte[] array2 = null;
				try
				{
					array2 = NinjaUtil.readByteArray(msg);
					Res.outz("request hinh icon = " + num9);
					if (num9 == 3896)
					{
						Res.outz("SIZE CHECK= " + array2.Length);
					}
					SmallImage.imgNew[num9].img = createImage(array2);
				}
				catch (Exception)
				{
					array2 = null;
					SmallImage.imgNew[num9].img = Image.createRGBImage(new int[1], 1, 1, bl: true);
				}
				if (array2 != null && mGraphics.zoomLevel > 1)
				{
					Rms.saveRMS(mGraphics.zoomLevel + "Small" + num9, array2);
				}
				break;
			}
			case -66:
			{
				short id = msg.reader().readShort();
				sbyte[] data = NinjaUtil.readByteArray(msg);
				EffectData effDataById = Effect.getEffDataById(id);
				effDataById.readData(data);
				sbyte[] array4 = NinjaUtil.readByteArray(msg);
				effDataById.img = Image.createImage(array4, 0, array4.Length);
				break;
			}
			case -32:
			{
				if (GameCanvas.lowGraphic && TileMap.mapID != 51 && TileMap.mapID != 103)
				{
					return;
				}
				short num130 = msg.reader().readShort();
				int num131 = msg.reader().readInt();
				sbyte[] array14 = null;
				Image image = null;
				try
				{
					array14 = new sbyte[num131];
					for (int num132 = 0; num132 < num131; num132++)
					{
						array14[num132] = msg.reader().readByte();
					}
					image = Image.createImage(array14, 0, num131);
					BgItem.imgNew.put(num130 + string.Empty, image);
				}
				catch (Exception)
				{
					array14 = null;
					BgItem.imgNew.put(num130 + string.Empty, Image.createRGBImage(new int[1], 1, 1, bl: true));
				}
				if (array14 != null)
				{
					if (mGraphics.zoomLevel > 1)
					{
						Rms.saveRMS(mGraphics.zoomLevel + "bgItem" + num130, array14);
					}
					BgItemMn.blendcurrBg(num130, image);
				}
				break;
			}
			case 92:
			{
				if (GameCanvas.currentScreen == GameScr.instance)
				{
					GameCanvas.endDlg();
				}
				string text3 = msg.reader().readUTF();
				string str2 = msg.reader().readUTF();
				str2 = Res.changeString(str2);
				string empty = string.Empty;
				Char char11 = null;
				sbyte b40 = 0;
				if (!text3.Equals(string.Empty))
				{
					char11 = new Char();
					char11.charID = msg.reader().readInt();
					char11.head = msg.reader().readShort();
					char11.headICON = msg.reader().readShort();
					char11.body = msg.reader().readShort();
					char11.bag = msg.reader().readShort();
					char11.leg = msg.reader().readShort();
					b40 = msg.reader().readByte();
					char11.cName = text3;
				}
				empty += str2;
				InfoDlg.hide();
				if (text3.Equals(string.Empty))
				{
					GameScr.info1.addInfo(empty, 0);
					break;
				}
				GameScr.info2.addInfoWithChar(empty, char11, (b40 == 0) ? true : false);
				if (GameCanvas.panel.isShow && GameCanvas.panel.type == 8)
				{
					GameCanvas.panel.initLogMessage();
				}
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
				{
					GameCanvas.serverScreen.switchToMe();
				}
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
			{
				GameCanvas.debug("SXX4", 2);
				Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob5.isDisable = msg.reader().readBool();
				break;
			}
			case 82:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob5.isDontMove = msg.reader().readBool();
				break;
			}
			case 85:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob5.isFire = msg.reader().readBool();
				break;
			}
			case 86:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob5.isIce = msg.reader().readBool();
				if (!mob5.isIce)
				{
					ServerEffect.addServerEffect(77, mob5.x, mob5.y - 9, 1);
				}
				break;
			}
			case 87:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob5.isWind = msg.reader().readBool();
				break;
			}
			case 56:
			{
				GameCanvas.debug("SXX6", 2);
				@char = null;
				int num10 = msg.reader().readInt();
				if (num10 == Char.myCharz().charID)
				{
					bool flag4 = false;
					@char = Char.myCharz();
					@char.cHP = msg.readInt3Byte();
					int num39 = msg.readInt3Byte();
					Res.outz("dame hit = " + num39);
					if (num39 != 0)
					{
						@char.doInjure();
					}
					int num40 = 0;
					try
					{
						flag4 = msg.reader().readBoolean();
						sbyte b13 = msg.reader().readByte();
						if (b13 != -1)
						{
							Res.outz("hit eff= " + b13);
							EffecMn.addEff(new Effect(b13, @char.cx, @char.cy, 3, 1, -1));
						}
					}
					catch (Exception)
					{
					}
					num39 += num40;
					if (Char.myCharz().cTypePk != 4)
					{
						if (num39 == 0)
						{
							GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS_ME);
						}
						else
						{
							GameScr.startFlyText("-" + num39, @char.cx, @char.cy - @char.ch, 0, -3, flag4 ? mFont.FATAL : mFont.RED);
						}
					}
					break;
				}
				@char = GameScr.findCharInMap(num10);
				if (@char == null)
				{
					return;
				}
				@char.cHP = msg.readInt3Byte();
				bool flag5 = false;
				int num41 = msg.readInt3Byte();
				if (num41 != 0)
				{
					@char.doInjure();
				}
				int num42 = 0;
				try
				{
					flag5 = msg.reader().readBoolean();
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
				num41 += num42;
				if (@char.cTypePk != 4)
				{
					if (num41 == 0)
					{
						GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num41, @char.cx, @char.cy - @char.ch, 0, -3, flag5 ? mFont.FATAL : mFont.ORANGE);
					}
				}
				break;
			}
			case 83:
			{
				GameCanvas.debug("SXX8", 2);
				int num10 = msg.reader().readInt();
				@char = ((num10 != Char.myCharz().charID) ? GameScr.findCharInMap(num10) : Char.myCharz());
				if (@char == null)
				{
					return;
				}
				Mob mobToAttack = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				if (@char.mobMe != null)
				{
					@char.mobMe.attackOtherMob(mobToAttack);
				}
				break;
			}
			case 84:
			{
				int num10 = msg.reader().readInt();
				if (num10 == Char.myCharz().charID)
				{
					@char = Char.myCharz();
				}
				else
				{
					@char = GameScr.findCharInMap(num10);
					if (@char == null)
					{
						return;
					}
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
				catch (Exception ex3)
				{
					Cout.println("Loi CLEAR_CUU_SAT " + ex3.ToString());
				}
				@char.killCharId = -9999;
				break;
			case 39:
				GameCanvas.debug("SA49", 2);
				GameScr.gI().typeTradeOrder = 2;
				if (GameScr.gI().typeTrade >= 2 && GameScr.gI().typeTradeOrder >= 2)
				{
					InfoDlg.showWait();
				}
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
				int num10 = msg.reader().readInt();
				Char char4 = ((num10 != Char.myCharz().charID) ? GameScr.findCharInMap(num10) : Char.myCharz());
				char4.moveFast = new short[3];
				char4.moveFast[0] = 0;
				short num11 = msg.reader().readShort();
				short num12 = msg.reader().readShort();
				char4.moveFast[1] = num11;
				char4.moveFast[2] = num12;
				try
				{
					num10 = msg.reader().readInt();
					Char char5 = ((num10 != Char.myCharz().charID) ? GameScr.findCharInMap(num10) : Char.myCharz());
					char5.cx = num11;
					char5.cy = num12;
				}
				catch (Exception ex2)
				{
					Cout.println("Loi MOVE_FAST " + ex2.ToString());
				}
				break;
			}
			case 88:
			{
				string info2 = msg.reader().readUTF();
				short num17 = msg.reader().readShort();
				GameCanvas.inputDlg.show(info2, new Command(mResources.ACCEPT, GameCanvas.instance, 88818, num17), TField.INPUT_TYPE_ANY);
				break;
			}
			case 27:
			{
				myVector = new MyVector();
				string text7 = msg.reader().readUTF();
				int num166 = msg.reader().readByte();
				for (int num167 = 0; num167 < num166; num167++)
				{
					string caption4 = msg.reader().readUTF();
					short num168 = msg.reader().readShort();
					myVector.addElement(new Command(caption4, GameCanvas.instance, 88819, num168));
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
						string caption3 = msg.reader().readUTF();
						myVector.addElement(new Command(caption3, GameCanvas.instance, 88822, null));
					}
				}
				catch (Exception ex18)
				{
					Cout.println("Loi OPEN_UI_MENU " + ex18.ToString());
				}
				if (Char.myCharz().npcFocus == null)
				{
					return;
				}
				for (int num145 = 0; num145 < Char.myCharz().npcFocus.template.menu.Length; num145++)
				{
					string[] array15 = Char.myCharz().npcFocus.template.menu[num145];
					myVector.addElement(new Command(array15[0], GameCanvas.instance, 88820, array15));
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
				string str3 = msg.reader().readUTF();
				str3 = Res.changeString(str3);
				string str4 = msg.reader().readUTF();
				str4 = Res.changeString(str4);
				string[] array11 = new string[msg.reader().readByte()];
				string[] array12 = new string[array11.Length];
				GameScr.tasks = new int[array11.Length];
				GameScr.mapTasks = new int[array11.Length];
				short[] array13 = new short[array11.Length];
				short count = -1;
				for (int num123 = 0; num123 < array11.Length; num123++)
				{
					string str5 = msg.reader().readUTF();
					str5 = Res.changeString(str5);
					GameScr.tasks[num123] = msg.reader().readByte();
					GameScr.mapTasks[num123] = msg.reader().readShort();
					string str6 = msg.reader().readUTF();
					str6 = Res.changeString(str6);
					array13[num123] = -1;
					if (!str5.Equals(string.Empty))
					{
						array11[num123] = str5;
						array12[num123] = str6;
					}
				}
				try
				{
					count = msg.reader().readShort();
					for (int num124 = 0; num124 < array11.Length; num124++)
					{
						array13[num124] = msg.reader().readShort();
					}
				}
				catch (Exception ex16)
				{
					Cout.println("Loi TASK_GET " + ex16.ToString());
				}
				Char.myCharz().taskMaint = new Task(taskId, index3, str3, str4, array11, array13, count, array12);
				if (Char.myCharz().npcFocus != null)
				{
					Npc.clearEffTask();
				}
				Char.taskAction(isNextStep: false);
				break;
			}
			case 41:
				GameCanvas.debug("SA53", 2);
				GameCanvas.taskTick = 100;
				Res.outz("TASK NEXT");
				Char.myCharz().taskMaint.index++;
				Char.myCharz().taskMaint.count = 0;
				Npc.clearEffTask();
				Char.taskAction(isNextStep: true);
				break;
			case 50:
			{
				sbyte b53 = msg.reader().readByte();
				Panel.vGameInfo.removeAllElements();
				for (int num122 = 0; num122 < b53; num122++)
				{
					GameInfo gameInfo = new GameInfo();
					gameInfo.id = msg.reader().readShort();
					gameInfo.main = msg.reader().readUTF();
					gameInfo.content = msg.reader().readUTF();
					Panel.vGameInfo.addElement(gameInfo);
					bool flag9 = (gameInfo.hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1);
				}
				break;
			}
			case 43:
				GameCanvas.taskTick = 50;
				GameCanvas.debug("SA55", 2);
				Char.myCharz().taskMaint.count = msg.reader().readShort();
				if (Char.myCharz().npcFocus != null)
				{
					Npc.clearEffTask();
				}
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
				for (int num118 = 0; num118 < GameScr.vItemMap.size(); num118++)
				{
					if (((ItemMap)GameScr.vItemMap.elementAt(num118)).itemMapID == itemMapID)
					{
						GameScr.vItemMap.removeElementAt(num118);
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
				for (int num114 = 0; num114 < GameScr.vItemMap.size(); num114++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(num114);
					if (itemMap2.itemMapID != itemMapID)
					{
						continue;
					}
					itemMap2.setPoint(Char.myCharz().cx, Char.myCharz().cy - 10);
					string text4 = msg.reader().readUTF();
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
					if (text4.Equals(string.Empty))
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
					else if (text4.Length == 1)
					{
						Cout.LogError3("strInf.Length =1:  " + text4);
					}
					else
					{
						GameScr.info1.addInfo(text4, 0);
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
				for (int num107 = 0; num107 < GameScr.vItemMap.size(); num107++)
				{
					ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(num107);
					if (itemMap.itemMapID != itemMapID)
					{
						continue;
					}
					if (@char == null)
					{
						return;
					}
					itemMap.setPoint(@char.cx, @char.cy - 10);
					if (itemMap.x < @char.cx)
					{
						@char.cdir = -1;
					}
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
				int num106 = msg.reader().readByte();
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), Char.myCharz().arrItemBag[num106].template.id, Char.myCharz().cx, Char.myCharz().cy, msg.reader().readShort(), msg.reader().readShort()));
				Char.myCharz().arrItemBag[num106] = null;
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
				int num102 = msg.reader().readInt();
				short r = 0;
				if (num102 == -2)
				{
					r = msg.reader().readShort();
				}
				ItemMap o = new ItemMap(num102, itemMapID, itemTemplateID, x, y, r);
				GameScr.vItemMap.addElement(o);
				break;
			}
			case 69:
				GameCanvas.debug("SA633355", 2);
				Char.myCharz().arrItemBag[msg.reader().readByte()].quantity = msg.reader().readShort();
				break;
			case -14:
				GameCanvas.debug("SA64", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
				{
					return;
				}
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
				string chat3 = msg.reader().readUTF();
				Npc npc5 = new Npc(-1, 0, 0, 0, 0, 0);
				npc5.avatar = avatar;
				ChatPopup.addBigMessage(chat3, 100000, npc5);
				sbyte b37 = msg.reader().readByte();
				if (b37 == 0)
				{
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 35;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
				}
				if (b37 == 1)
				{
					string p2 = msg.reader().readUTF();
					string caption2 = msg.reader().readUTF();
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(caption2, ChatPopup.serverChatPopUp, 1000, p2);
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
				string str = msg.reader().readUTF();
				str = Res.changeString(str);
				for (int num92 = 0; num92 < GameScr.vNpc.size(); num92++)
				{
					Npc npc3 = (Npc)GameScr.vNpc.elementAt(num92);
					Res.outz("npc id= " + npc3.template.npcTemplateId);
					if (npc3.template.npcTemplateId == num76)
					{
						ChatPopup.addChatPopupMultiLine(str, 100000, npc3);
						GameCanvas.panel.hideNow();
						return;
					}
				}
				Npc npc4 = new Npc(num76, 0, 0, 0, num76, GameScr.info1.charId[Char.myCharz().cgender][2]);
				if (npc4.template.npcTemplateId == 5)
				{
					npc4.charID = 5;
				}
				try
				{
					npc4.avatar = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				ChatPopup.addChatPopupMultiLine(str, 100000, npc4);
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
						string[] array9 = new string[msg.reader().readByte()];
						for (int num78 = 0; num78 < array9.Length; num78++)
						{
							array9[num78] = msg.reader().readUTF();
						}
						GameScr.gI().createMenu(array9, npc);
						ChatPopup.addChatPopup(chat, 100000, npc);
						return;
					}
				}
				Npc npc2 = new Npc(num76, 0, -100, 100, num76, GameScr.info1.charId[Char.myCharz().cgender][2]);
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				string chat2 = msg.reader().readUTF();
				string[] array10 = new string[msg.reader().readByte()];
				for (int num79 = 0; num79 < array10.Length; num79++)
				{
					array10[num79] = msg.reader().readUTF();
				}
				try
				{
					short num80 = (short)(npc2.avatar = msg.reader().readShort());
				}
				catch (Exception)
				{
				}
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				GameScr.gI().createMenu(array10, npc2);
				ChatPopup.addChatPopup(chat2, 100000, npc2);
				break;
			}
			case 24:
			{
				GameCanvas.debug("SA69", 2);
				Char.myCharz().xuInBox = msg.reader().readInt();
				Char.myCharz().arrItemBox = new Item[msg.reader().readUnsignedByte()];
				for (int num74 = 0; num74 < Char.myCharz().arrItemBox.Length; num74++)
				{
					short num75 = msg.reader().readShort();
					if (num75 != -1)
					{
						Char.myCharz().arrItemBox[num74] = new Item();
						Char.myCharz().arrItemBox[num74].typeUI = 4;
						Char.myCharz().arrItemBox[num74].indexUI = num74;
						Char.myCharz().arrItemBox[num74].template = ItemTemplates.get(num75);
						Char.myCharz().arrItemBox[num74].isLock = msg.reader().readBool();
						if (Char.myCharz().arrItemBox[num74].isTypeBody())
						{
							Char.myCharz().arrItemBox[num74].upgrade = msg.reader().readByte();
						}
						Char.myCharz().arrItemBox[num74].isExpires = msg.reader().readBool();
						Char.myCharz().arrItemBox[num74].quantity = msg.reader().readShort();
					}
				}
				break;
			}
			case 7:
			{
				sbyte type = msg.reader().readByte();
				short id2 = msg.reader().readShort();
				string info3 = msg.reader().readUTF();
				GameCanvas.panel.saleRequest(type, info3, id2);
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
					sbyte b28 = msg.reader().readByte();
					TileMap.isMapDouble = ((b28 != 0) ? true : false);
				}
				catch (Exception)
				{
				}
				GameScr.cmx = GameScr.cmtoX;
				GameScr.cmy = GameScr.cmtoY;
				break;
			case -31:
			{
				if (GameCanvas.lowGraphic && TileMap.mapID != 51)
				{
					return;
				}
				TileMap.vItemBg.removeAllElements();
				short num58 = msg.reader().readShort();
				Cout.LogError2("nItem= " + num58);
				for (int num59 = 0; num59 < num58; num59++)
				{
					BgItem bgItem = new BgItem();
					bgItem.id = num59;
					bgItem.idImage = msg.reader().readShort();
					bgItem.layer = msg.reader().readByte();
					bgItem.dx = msg.reader().readShort();
					bgItem.dy = msg.reader().readShort();
					sbyte b23 = msg.reader().readByte();
					bgItem.tileX = new int[b23];
					bgItem.tileY = new int[b23];
					for (int num60 = 0; num60 < b23; num60++)
					{
						bgItem.tileX[num59] = msg.reader().readByte();
						bgItem.tileY[num59] = msg.reader().readByte();
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
				{
					return;
				}
				GameCanvas.debug("SA76v1", 2);
				if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
				{
					@char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 0);
				}
				else
				{
					@char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 1);
				}
				GameCanvas.debug("SA76v2", 2);
				@char.attMobs = new Mob[msg.reader().readByte()];
				for (int num38 = 0; num38 < @char.attMobs.Length; num38++)
				{
					Mob mob3 = (Mob)GameScr.vMob.elementAt(msg.reader().readByte());
					@char.attMobs[num38] = mob3;
					if (num38 == 0)
					{
						if (@char.cx <= mob3.x)
						{
							@char.cdir = 1;
						}
						else
						{
							@char.cdir = -1;
						}
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
						int num10 = msg.reader().readInt();
						Char char7 = (array[num] = ((num10 != Char.myCharz().charID) ? GameScr.findCharInMap(num10) : Char.myCharz()));
						if (num == 0)
						{
							if (@char.cx <= char7.cx)
							{
								@char.cdir = 1;
							}
							else
							{
								@char.cdir = -1;
							}
						}
					}
				}
				catch (Exception ex6)
				{
					Cout.println("Loi PLAYER_ATTACK_N_P " + ex6.ToString());
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
				{
					return;
				}
				int num19 = msg.reader().readUnsignedByte();
				if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
				{
					@char.setSkillPaint(GameScr.sks[num19], 0);
				}
				else
				{
					@char.setSkillPaint(GameScr.sks[num19], 1);
				}
				GameCanvas.debug("SA769991v2", 2);
				Mob[] array5 = new Mob[10];
				num = 0;
				try
				{
					GameCanvas.debug("SA769991v3", 2);
					for (num = 0; num < array5.Length; num++)
					{
						GameCanvas.debug("SA769991v4-num" + num, 2);
						Mob mob2 = (array5[num] = (Mob)GameScr.vMob.elementAt(msg.reader().readByte()));
						if (num == 0)
						{
							if (@char.cx <= mob2.x)
							{
								@char.cdir = 1;
							}
							else
							{
								@char.cdir = -1;
							}
						}
						GameCanvas.debug("SA769991v5-num" + num, 2);
					}
				}
				catch (Exception ex4)
				{
					Cout.println("Loi PLAYER_ATTACK_NPC " + ex4.ToString());
				}
				GameCanvas.debug("SA769992", 2);
				if (num > 0)
				{
					@char.attMobs = new Mob[num];
					for (num = 0; num < @char.attMobs.Length; num++)
					{
						@char.attMobs[num] = array5[num];
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
					{
						return;
					}
					if (char2.currentMovePoint != null)
					{
						char2.createShadow(char2.cx, char2.cy, 10);
						char2.cx = char2.currentMovePoint.xEnd;
						char2.cy = char2.currentMovePoint.yEnd;
					}
					int num4 = msg.reader().readUnsignedByte();
					Res.outz("player skill ID= " + num4);
					if ((TileMap.tileTypeAtPixel(char2.cx, char2.cy) & 2) == 2)
					{
						char2.setSkillPaint(GameScr.sks[num4], 0);
					}
					else
					{
						char2.setSkillPaint(GameScr.sks[num4], 1);
					}
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
						{
							char3 = GameScr.findCharInMap(num3);
						}
						array[num] = char3;
						if (num == 0)
						{
							if (char2.cx <= char3.cx)
							{
								char2.cdir = 1;
							}
							else
							{
								char2.cdir = -1;
							}
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
				sbyte b4 = msg.reader().readByte();
				Res.outz("isRead continue = " + b4);
				if (b4 != 1)
				{
					break;
				}
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
					{
						Char.isLockKey = true;
					}
					Res.outz("isDie=" + @char.isDie + "---------------------------------------");
					int num6 = 0;
					flag = (@char.isCrit = msg.reader().readBoolean());
					@char.isMob = false;
					num5 = (@char.damHP = num5 + num6);
					if (b5 == 0)
					{
						@char.doInjure(num5, 0, flag, isMob: false);
					}
				}
				else
				{
					@char = GameScr.findCharInMap(num3);
					if (@char == null)
					{
						return;
					}
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
					{
						@char.doInjure(num7, 0, flag2, isMob: false);
					}
				}
				break;
			}
			}
			switch (msg.command)
			{
			case -2:
			{
				GameCanvas.debug("SA77", 22);
				int num185 = msg.reader().readInt();
				Char.myCharz().yen += num185;
				GameScr.startFlyText((num185 <= 0) ? (string.Empty + num185) : ("+" + num185), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 95:
			{
				GameCanvas.debug("SA77", 22);
				int num176 = msg.reader().readInt();
				Char.myCharz().xu += num176;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				GameScr.startFlyText((num176 <= 0) ? (string.Empty + num176) : ("+" + num176), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 96:
				GameCanvas.debug("SA77a", 22);
				Char.myCharz().taskOrders.addElement(new TaskOrder(msg.reader().readByte(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readUTF(), msg.reader().readUTF(), msg.reader().readByte(), msg.reader().readByte()));
				break;
			case 97:
			{
				sbyte b71 = msg.reader().readByte();
				for (int num173 = 0; num173 < Char.myCharz().taskOrders.size(); num173++)
				{
					TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(num173);
					if (taskOrder.taskId == b71)
					{
						taskOrder.count = msg.reader().readShort();
						break;
					}
				}
				break;
			}
			case -1:
			{
				GameCanvas.debug("SA77", 222);
				int num184 = msg.reader().readInt();
				Char.myCharz().xu += num184;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().yen -= num184;
				GameScr.startFlyText("+" + num184, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case -3:
			{
				GameCanvas.debug("SA78", 2);
				sbyte b78 = msg.reader().readByte();
				int num190 = msg.reader().readInt();
				if (b78 == 0)
				{
					Char.myCharz().cPower += num190;
				}
				if (b78 == 1)
				{
					Char.myCharz().cTiemNang += num190;
				}
				if (b78 == 2)
				{
					Char.myCharz().cPower += num190;
					Char.myCharz().cTiemNang += num190;
				}
				Char.myCharz().applyCharLevelPercent();
				if (Char.myCharz().cTypePk != 3)
				{
					GameScr.startFlyText(((num190 <= 0) ? string.Empty : "+") + num190, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -4, mFont.GREEN);
					if (num190 > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5002)
					{
						ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
						ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
					}
				}
				break;
			}
			case -73:
			{
				sbyte b75 = msg.reader().readByte();
				for (int num183 = 0; num183 < GameScr.vNpc.size(); num183++)
				{
					Npc npc7 = (Npc)GameScr.vNpc.elementAt(num183);
					if (npc7.template.npcTemplateId == b75)
					{
						sbyte b76 = msg.reader().readByte();
						if (b76 == 0)
						{
							npc7.isHide = true;
						}
						else
						{
							npc7.isHide = false;
						}
						break;
					}
				}
				break;
			}
			case -5:
			{
				GameCanvas.debug("SA79", 2);
				int charID = msg.reader().readInt();
				int num178 = msg.reader().readInt();
				Char char14;
				if (num178 != -100)
				{
					char14 = new Char();
					char14.charID = charID;
					char14.clanID = num178;
				}
				else
				{
					char14 = new Mabu();
					char14.charID = charID;
					char14.clanID = num178;
				}
				if (char14.clanID == -2)
				{
					char14.isCopy = true;
				}
				if (readCharInfo(char14, msg))
				{
					sbyte b73 = msg.reader().readByte();
					if (char14.cy <= 10 && b73 != 0 && b73 != 2)
					{
						Res.outz("nhân vật bay trên trời xuống x= " + char14.cx + " y= " + char14.cy);
						Teleport teleport2 = new Teleport(char14.cx, char14.cy, char14.head, char14.cdir, 1, isMe: false, (b73 != 1) ? b73 : char14.cgender);
						teleport2.id = char14.charID;
						char14.isTeleport = true;
						Teleport.addTeleport(teleport2);
					}
					if (b73 == 2)
					{
						char14.show();
					}
					for (int num179 = 0; num179 < GameScr.vMob.size(); num179++)
					{
						Mob mob10 = (Mob)GameScr.vMob.elementAt(num179);
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
					{
						GameScr.vCharInMap.addElement(char14);
					}
					char14.isMonkey = msg.reader().readByte();
					short num180 = msg.reader().readShort();
					Res.outz("mount id= " + num180 + "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
					if (num180 != -1)
					{
						char14.isHaveMount = true;
						switch (num180)
						{
						case 346:
						case 347:
						case 348:
							char14.isMountVip = false;
							break;
						case 349:
						case 350:
						case 351:
							char14.isMountVip = true;
							break;
						case 396:
							char14.isEventMount = true;
							break;
						case 532:
							char14.isSpeacialMount = true;
							break;
						default:
							if (num180 >= Char.ID_NEW_MOUNT)
							{
								char14.idMount = num180;
							}
							break;
						}
					}
					else
					{
						char14.isHaveMount = false;
					}
				}
				sbyte b74 = msg.reader().readByte();
				Res.outz("addplayer:   " + b74);
				char14.cFlag = b74;
				char14.isNhapThe = msg.reader().readByte() == 1;
				try
				{
					char14.idAuraEff = msg.reader().readShort();
					char14.idEff_Set_Item = msg.reader().readSByte();
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
				int num174 = msg.reader().readInt();
				Cout.println("RECEVED MOVE OF " + num174);
				for (int num175 = 0; num175 < GameScr.vCharInMap.size(); num175++)
				{
					Char char13 = null;
					try
					{
						char13 = (Char)GameScr.vCharInMap.elementAt(num175);
					}
					catch (Exception ex23)
					{
						Cout.println("Loi PLAYER_MOVE " + ex23.ToString());
					}
					if (char13 == null)
					{
						break;
					}
					if (char13.charID == num174)
					{
						GameCanvas.debug("SA8x2y" + num175, 2);
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
				int num174 = msg.reader().readInt();
				for (int num189 = 0; num189 < GameScr.vCharInMap.size(); num189++)
				{
					Char char15 = (Char)GameScr.vCharInMap.elementAt(num189);
					if (char15 != null && char15.charID == num174)
					{
						if (!char15.isInvisiblez && !char15.isUsePlane)
						{
							ServerEffect.addServerEffect(60, char15.cx, char15.cy, 1);
						}
						if (!char15.isUsePlane)
						{
							GameScr.vCharInMap.removeElementAt(num189);
						}
						return;
					}
				}
				break;
			}
			case -13:
			{
				GameCanvas.debug("SA82", 2);
				int num181 = msg.reader().readUnsignedByte();
				if (num181 > GameScr.vMob.size() - 1 || num181 < 0)
				{
					return;
				}
				Mob mob9 = (Mob)GameScr.vMob.elementAt(num181);
				mob9.sys = msg.reader().readByte();
				mob9.levelBoss = msg.reader().readByte();
				if (mob9.levelBoss != 0)
				{
					mob9.typeSuperEff = Res.random(0, 3);
				}
				mob9.x = mob9.xFirst;
				mob9.y = mob9.yFirst;
				mob9.status = 5;
				mob9.injureThenDie = false;
				mob9.hp = msg.reader().readInt();
				mob9.maxHp = mob9.hp;
				ServerEffect.addServerEffect(60, mob9.x, mob9.y, 1);
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
					{
						mob9.typeSuperEff = Res.random(0, 3);
					}
				}
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
					int num177 = msg.readInt3Byte();
					if (num177 == 1)
					{
						return;
					}
					bool flag11 = false;
					try
					{
						flag11 = msg.reader().readBoolean();
					}
					catch (Exception)
					{
					}
					sbyte b72 = msg.reader().readByte();
					if (b72 != -1)
					{
						EffecMn.addEff(new Effect(b72, mob9.x, mob9.getY(), 3, 1, -1));
					}
					GameCanvas.debug("SA83v2", 2);
					if (flag11)
					{
						GameScr.startFlyText("-" + num177, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.FATAL);
					}
					else if (num177 == 0)
					{
						mob9.x = mob9.xFirst;
						mob9.y = mob9.yFirst;
						GameScr.startFlyText(mResources.miss, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num177, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.ORANGE);
					}
				}
				GameCanvas.debug("SA83v3", 2);
				break;
			}
			case 45:
			{
				GameCanvas.debug("SA84", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception ex22)
				{
					Cout.println("Loi tai NPC_MISS  " + ex22.ToString());
				}
				if (mob9 != null)
				{
					mob9.hp = msg.reader().readInt();
					GameScr.startFlyText(mResources.miss, mob9.x, mob9.y - mob9.h, 0, -2, mFont.MISS);
				}
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
				{
					break;
				}
				mob9.startDie();
				try
				{
					int num186 = msg.readInt3Byte();
					if (msg.reader().readBool())
					{
						GameScr.startFlyText("-" + num186, mob9.x, mob9.y - mob9.h, 0, -2, mFont.FATAL);
					}
					else
					{
						GameScr.startFlyText("-" + num186, mob9.x, mob9.y - mob9.h, 0, -2, mFont.ORANGE);
					}
					sbyte b77 = msg.reader().readByte();
					for (int num187 = 0; num187 < b77; num187++)
					{
						ItemMap itemMap4 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
						int num188 = (itemMap4.playerId = msg.reader().readInt());
						Res.outz("playerid= " + num188 + " my id= " + Char.myCharz().charID);
						GameScr.vItemMap.addElement(itemMap4);
						if (Res.abs(itemMap4.y - Char.myCharz().cy) < 24 && Res.abs(itemMap4.x - Char.myCharz().cx) < 24)
						{
							Char.myCharz().charFocus = null;
						}
					}
				}
				catch (Exception ex33)
				{
					Cout.println("LOi tai NPC_DIE " + ex33.ToString() + " cmd " + msg.command);
				}
				break;
			}
			case 74:
			{
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
					{
						Char.myCharz().charFocus = null;
					}
				}
				break;
			}
			case -11:
			{
				GameCanvas.debug("SA86", 2);
				Mob mob9 = null;
				try
				{
					int index4 = msg.reader().readUnsignedByte();
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
					int num170 = msg.readInt3Byte();
					int num171;
					try
					{
						num171 = msg.readInt3Byte();
					}
					catch (Exception)
					{
						num171 = 0;
					}
					if (mob9.isBusyAttackSomeOne)
					{
						Char.myCharz().doInjure(num170, num171, isCrit: false, isMob: true);
						break;
					}
					mob9.dame = num170;
					mob9.dameMp = num171;
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
					{
						return;
					}
					GameCanvas.debug("SA87x3", 2);
					int num182 = msg.readInt3Byte();
					mob9.dame = @char.cHP - num182;
					@char.cHPNew = num182;
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
					{
						@char.doInjure(mob9.dame, 0, isCrit: false, isMob: true);
					}
					else
					{
						mob9.setAttack(@char);
					}
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
			case 66:
				Res.outz("ME DIE XP DOWN NOT IMPLEMENT YET!!!!!!!!!!!!!!!!!!!!!!!!!!");
				break;
			case -8:
				GameCanvas.debug("SA89", 2);
				@char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
				{
					return;
				}
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
			case 44:
			{
				GameCanvas.debug("SA91", 2);
				int num172 = msg.reader().readInt();
				string text8 = msg.reader().readUTF();
				Res.outz("user id= " + num172 + " text= " + text8);
				@char = ((Char.myCharz().charID != num172) ? GameScr.findCharInMap(num172) : Char.myCharz());
				if (@char == null)
				{
					return;
				}
				@char.addInfo(text8);
				break;
			}
			case 18:
			{
				sbyte b70 = msg.reader().readByte();
				for (int num169 = 0; num169 < b70; num169++)
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
			case 19:
				Char.myCharz().countKill = msg.reader().readUnsignedShort();
				Char.myCharz().countKillMax = msg.reader().readUnsignedShort();
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
			ItemTemplate it = new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBool());
			ItemTemplates.add(it);
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
				{
					lineWidth = 100;
				}
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
			{
				num += 256;
			}
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
				{
					clanMessage.chat = mFont.tahoma_7.splitFontArray(text, Panel.WIDTH_PANEL - 10);
				}
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
				{
					GameScr.isNewClanMessage = true;
				}
				if (clanMessage.playerId != Char.myCharz().charID)
				{
					if (clanMessage.recieve < clanMessage.maxCap)
					{
						clanMessage.option = new string[1] { mResources.donate };
					}
					else
					{
						clanMessage.option = null;
					}
				}
				if (GameCanvas.panel.cp != null)
				{
					GameCanvas.panel.updateRequest(clanMessage.recieve, clanMessage.maxCap);
				}
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
			{
				GameScr.isNewClanMessage = false;
			}
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
		{
			GameScr.gI().initSelectChar();
		}
		GameScr.loadCamera(fullmScreen: false, (teleport3 != 1) ? (-1) : Char.myCharz().cx, (teleport3 == 0) ? (-1) : 0);
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
		Char.myCharz().setMabuHold(m: false);
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
			EffectChar effectChar = (EffectChar)Char.myCharz().vEff.elementAt(i);
			if (effectChar.template.type == 10)
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
			Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 1, isMe: true, (teleport3 != 1) ? teleport3 : Char.myCharz().cgender);
			Teleport.addTeleport(p);
			Char.myCharz().isTeleport = true;
		}
		if (teleport3 == 2)
		{
			Char.myCharz().show();
		}
		if (GameScr.gI().isRongThanXuatHien)
		{
			if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
			{
				GameScr.gI().callRongThan(GameScr.gI().xR, GameScr.gI().yR);
			}
			if (mGraphics.zoomLevel > 1)
			{
				GameScr.gI().doiMauTroi();
			}
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
			{
				SmallImage.clearHastable();
			}
			Char.myCharz().cx = (Char.myCharz().cxSend = (Char.myCharz().cxFocus = msg.reader().readShort()));
			Char.myCharz().cy = (Char.myCharz().cySend = (Char.myCharz().cyFocus = msg.reader().readShort()));
			Char.myCharz().xSd = Char.myCharz().cx;
			Char.myCharz().ySd = Char.myCharz().cy;
			Res.outz("head= " + Char.myCharz().head + " body= " + Char.myCharz().body + " left= " + Char.myCharz().leg + " x= " + Char.myCharz().cx + " y= " + Char.myCharz().cy + " chung toc= " + Char.myCharz().cgender);
			if (Char.myCharz().cx >= 0 && Char.myCharz().cx <= 100)
			{
				Char.myCharz().cdir = 1;
			}
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
				{
				}
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
					{
						mob.dir = -1;
					}
					else
					{
						mob.dir = 1;
					}
					mob.x += 10 - b % 20;
				}
				mob.isMobMe = false;
				BigBoss bigBoss = null;
				BachTuoc bachTuoc = null;
				BigBoss2 bigBoss2 = null;
				NewBoss newBoss = null;
				if (mob.templateId == 70)
				{
					bigBoss = new BigBoss(b, (short)mob.x, (short)mob.y, 70, mob.hp, mob.maxHp, mob.sys);
				}
				if (mob.templateId == 71)
				{
					bachTuoc = new BachTuoc(b, (short)mob.x, (short)mob.y, 71, mob.hp, mob.maxHp, mob.sys);
				}
				if (mob.templateId == 72)
				{
					bigBoss2 = new BigBoss2(b, (short)mob.x, (short)mob.y, 72, mob.hp, mob.maxHp, 3);
				}
				if (mob.isBoss)
				{
					newBoss = new NewBoss(b, (short)mob.x, (short)mob.y, mob.templateId, mob.hp, mob.maxHp, mob.sys);
				}
				if (newBoss != null)
				{
					GameScr.vMob.addElement(newBoss);
				}
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
				{
					r = msg.reader().readShort();
				}
				ItemMap itemMap = new ItemMap(num4, itemMapID, itemTemplateID, x, y, r);
				bool flag = false;
				for (int m = 0; m < GameScr.vItemMap.size(); m++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(m);
					if (itemMap2.itemMapID == itemMap.itemMapID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					GameScr.vItemMap.addElement(itemMap);
				}
			}
			if (!GameCanvas.lowGraphic || (GameCanvas.lowGraphic && (TileMap.mapID == 51 || TileMap.mapID == 103)))
			{
				short num5 = msg.reader().readShort();
				TileMap.vCurrItem.removeAllElements();
				if (mGraphics.zoomLevel == 1)
				{
					BgItem.clearHashTable();
				}
				BgItem.vKeysNew.removeAllElements();
				Res.outz("nItem= " + num5);
				for (int n = 0; n < num5; n++)
				{
					short id = msg.reader().readShort();
					short num6 = msg.reader().readShort();
					short num7 = msg.reader().readShort();
					if (TileMap.getBIById(id) == null)
					{
						continue;
					}
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
						{
							bgItem.trans = 0;
						}
					}
					Image image = null;
					if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
					{
						if (mGraphics.zoomLevel == 1)
						{
							image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
							if (image == null)
							{
								image = Image.createRGBImage(new int[1], 1, 1, bl: true);
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
									{
										flag2 = true;
									}
								}
								if (!flag2)
								{
									image = Image.createImage(array, 0, array.Length);
									if (image != null)
									{
										BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
									}
									else
									{
										flag2 = true;
									}
								}
							}
							else
							{
								flag2 = true;
							}
							if (flag2)
							{
								image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
								if (image == null)
								{
									image = Image.createRGBImage(new int[1], 1, 1, bl: true);
									Service.gI().getBgTemplate(bgItem.idImage);
								}
								BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
							}
						}
						BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
					}
					if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
					{
						BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
					}
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
						{
							BgItem.imgNew.remove(text2 + "blend" + 1);
						}
						if (BgItem.imgNew.containsKey(text2 + "blend" + 3))
						{
							BgItem.imgNew.remove(text2 + "blend" + 3);
						}
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
					string key = msg.reader().readUTF();
					string value = msg.reader().readUTF();
					keyValueAction(key, value);
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
			sbyte teleport = msg.reader().readByte();
			loadCurrMap(teleport);
			Char.isLoadingMap = false;
			GameCanvas.debug("SA75x8", 2);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			Cout.LogError("----------DA CHAY XONG LOAD INFO MAP");
		}
		catch (Exception ex)
		{
			Cout.LogError("LOI TAI LOADMAP INFO " + ex.ToString());
            Pk9rXmap.FixBlackScreen();
        }
		GameEvents.onInfoMapLoaded();
	}

	public void keyValueAction(string key, string value)
	{
		if (key.Equals("eff"))
		{
			if (Panel.graphics > 0)
			{
				return;
			}
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
			case 35:
				GameCanvas.endDlg();
				GameScr.gI().resetButton();
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 36:
				GameScr.typeActive = msg.reader().readByte();
				Res.outz("load Me Active: " + GameScr.typeActive);
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
				sbyte b3 = msg.reader().readByte();
				if (GameCanvas.loginScr.isLogin2)
				{
					Rms.saveRMSString("acc", string.Empty);
					Rms.saveRMSString("pass", string.Empty);
				}
				else
				{
					Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
				}
				if (GameScr.vsData != GameScr.vcData)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateData();
				}
				else
				{
					try
					{
						LoginScr.isUpdateData = false;
					}
					catch (Exception)
					{
						GameScr.vcData = -1;
						Service.gI().updateData();
					}
				}
				if (GameScr.vsMap != GameScr.vcMap)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateMap();
				}
				else
				{
					try
					{
						if (!GameScr.isLoadAllData)
						{
							DataInputStream dataInputStream = new DataInputStream(Rms.loadRMS("NRmap"));
							createMap(dataInputStream.r);
						}
						LoginScr.isUpdateMap = false;
					}
					catch (Exception)
					{
						GameScr.vcMap = -1;
						Service.gI().updateMap();
					}
				}
				if (GameScr.vsSkill != GameScr.vcSkill)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateSkill();
				}
				else
				{
					try
					{
						if (!GameScr.isLoadAllData)
						{
							DataInputStream dataInputStream2 = new DataInputStream(Rms.loadRMS("NRskill"));
							createSkill(dataInputStream2.r);
						}
						LoginScr.isUpdateSkill = false;
					}
					catch (Exception)
					{
						GameScr.vcSkill = -1;
						Service.gI().updateSkill();
					}
				}
				if (GameScr.vsItem != GameScr.vcItem)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateItem();
				}
				else
				{
					try
					{
						DataInputStream dataInputStream3 = new DataInputStream(Rms.loadRMS("NRitem0"));
						loadItemNew(dataInputStream3.r, 0, isSave: false);
						DataInputStream dataInputStream4 = new DataInputStream(Rms.loadRMS("NRitem1"));
						loadItemNew(dataInputStream4.r, 1, isSave: false);
						DataInputStream dataInputStream5 = new DataInputStream(Rms.loadRMS("NRitem2"));
						loadItemNew(dataInputStream5.r, 2, isSave: false);
						DataInputStream dataInputStream6 = new DataInputStream(Rms.loadRMS("NRitem100"));
						loadItemNew(dataInputStream6.r, 100, isSave: false);
						LoginScr.isUpdateItem = false;
					}
					catch (Exception)
					{
						GameScr.vcItem = -1;
						Service.gI().updateItem();
					}
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
				sbyte b4 = msg.reader().readByte();
				Res.outz("CAPTION LENT= " + b4);
				GameScr.exps = new long[b4];
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
				sbyte[] data3 = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data3);
				Rms.saveRMS("NRmap", data3);
				sbyte[] data4 = new sbyte[1] { GameScr.vcMap };
				Rms.saveRMS("NRmapVersion", data4);
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
				sbyte[] data2 = new sbyte[1] { GameScr.vcSkill };
				Rms.saveRMS("NRskillVersion", data2);
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
						{
							num += 256;
						}
						TileMap.maps[i] = (ushort)num;
					}
					TileMap.types = new int[TileMap.maps.Length];
					msg = messWait;
					loadInfoMap(msg);
					try
					{
						sbyte b2 = msg.reader().readByte();
						TileMap.isMapDouble = ((b2 != 0) ? true : false);
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
			{
				return;
			}
			string text = msg.reader().readUTF();
			if (mSystem.isTest)
			{
				text = "88:192.168.1.88:20000:0,53:112.213.85.53:20000:0," + text;
			}
			if (mSystem.clientType == 1)
			{
				ServerListScreen.linkDefault = text;
			}
			else
			{
				ServerListScreen.linkDefault = text;
			}
			ServerListScreen.getServerList(ServerListScreen.linkDefault);
			try
			{
				sbyte b2 = msg.reader().readByte();
				Panel.CanNapTien = b2 == 1;
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
			case 63:
			{
				sbyte b2 = msg.reader().readByte();
				if (b2 > 0)
				{
					InfoDlg.showWait();
					MyVector vPlayerMenu = GameCanvas.panel.vPlayerMenu;
					for (int n = 0; n < b2; n++)
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
			case 1:
				GameCanvas.debug("SA13", 2);
				Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
				Char.myCharz().cTiemNang = msg.reader().readLong();
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				Char.myCharz().myskill = null;
				break;
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
					short skillId2 = msg.reader().readShort();
					Skill skill5 = Skills.get(skillId2);
					useSkill(skill5);
				}
				GameScr.gI().sortSkill();
				if (GameScr.isPaintInfoMe)
				{
					GameScr.indexRow = -1;
					GameScr.gI().left = (GameScr.gI().center = null);
				}
				break;
			}
			case 19:
				GameCanvas.debug("SA17", 2);
				Char.myCharz().boxSort();
				break;
			case 21:
			{
				GameCanvas.debug("SA19", 2);
				int num21 = msg.reader().readInt();
				Char.myCharz().xuInBox -= num21;
				Char.myCharz().xu += num21;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				break;
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
					Skill skill4 = Skills.get(msg.reader().readShort());
					useSkill(skill4);
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
					for (int num2 = 0; num2 < Char.myCharz().arrItemBody.Length; num2++)
					{
						short num3 = msg.reader().readShort();
						if (num3 == -1)
						{
							continue;
						}
						ItemTemplate itemTemplate = ItemTemplates.get(num3);
						int num4 = itemTemplate.type;
						Char.myCharz().arrItemBody[num2] = new Item();
						Char.myCharz().arrItemBody[num2].template = itemTemplate;
						Char.myCharz().arrItemBody[num2].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBody[num2].info = msg.reader().readUTF();
						Char.myCharz().arrItemBody[num2].content = msg.reader().readUTF();
						int num5 = msg.reader().readUnsignedByte();
						if (num5 != 0)
						{
							Char.myCharz().arrItemBody[num2].itemOption = new ItemOption[num5];
							for (int num6 = 0; num6 < Char.myCharz().arrItemBody[num2].itemOption.Length; num6++)
							{
								int num7 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								if (num7 != -1)
								{
									Char.myCharz().arrItemBody[num2].itemOption[num6] = new ItemOption(num7, param);
								}
							}
						}
						switch (num4)
						{
						case 0:
							Res.outz("toi day =======================================" + Char.myCharz().body);
							Char.myCharz().body = Char.myCharz().arrItemBody[num2].template.part;
							break;
						case 1:
							Char.myCharz().leg = Char.myCharz().arrItemBody[num2].template.part;
							Res.outz("toi day =======================================" + Char.myCharz().leg);
							break;
						}
					}
				}
				catch (Exception)
				{
				}
				Char.myCharz().arrItemBag = new Item[msg.reader().readByte()];
				GameScr.hpPotion = 0;
				for (int num8 = 0; num8 < Char.myCharz().arrItemBag.Length; num8++)
				{
					short num9 = msg.reader().readShort();
					if (num9 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBag[num8] = new Item();
					Char.myCharz().arrItemBag[num8].template = ItemTemplates.get(num9);
					Char.myCharz().arrItemBag[num8].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBag[num8].info = msg.reader().readUTF();
					Char.myCharz().arrItemBag[num8].content = msg.reader().readUTF();
					Char.myCharz().arrItemBag[num8].indexUI = num8;
					sbyte b5 = msg.reader().readByte();
					if (b5 != 0)
					{
						Char.myCharz().arrItemBag[num8].itemOption = new ItemOption[b5];
						for (int num10 = 0; num10 < Char.myCharz().arrItemBag[num8].itemOption.Length; num10++)
						{
							int num11 = msg.reader().readUnsignedByte();
							int param2 = msg.reader().readUnsignedShort();
							if (num11 != -1)
							{
								Char.myCharz().arrItemBag[num8].itemOption[num10] = new ItemOption(num11, param2);
								Char.myCharz().arrItemBag[num8].getCompare();
							}
						}
					}
					if (Char.myCharz().arrItemBag[num8].template.type == 6)
					{
						GameScr.hpPotion += Char.myCharz().arrItemBag[num8].quantity;
					}
				}
				Char.myCharz().arrItemBox = new Item[msg.reader().readByte()];
				GameCanvas.panel.hasUse = 0;
				for (int num12 = 0; num12 < Char.myCharz().arrItemBox.Length; num12++)
				{
					short num13 = msg.reader().readShort();
					if (num13 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBox[num12] = new Item();
					Char.myCharz().arrItemBox[num12].template = ItemTemplates.get(num13);
					Char.myCharz().arrItemBox[num12].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBox[num12].info = msg.reader().readUTF();
					Char.myCharz().arrItemBox[num12].content = msg.reader().readUTF();
					Char.myCharz().arrItemBox[num12].itemOption = new ItemOption[msg.reader().readByte()];
					for (int num14 = 0; num14 < Char.myCharz().arrItemBox[num12].itemOption.Length; num14++)
					{
						int num15 = msg.reader().readUnsignedByte();
						int param3 = msg.reader().readUnsignedShort();
						if (num15 != -1)
						{
							Char.myCharz().arrItemBox[num12].itemOption[num14] = new ItemOption(num15, param3);
							Char.myCharz().arrItemBox[num12].getCompare();
						}
					}
					GameCanvas.panel.hasUse++;
				}
				Char.myCharz().statusMe = 4;
				int num16 = Rms.loadRMSInt(Char.myCharz().cName + "vci");
				if (num16 < 1)
				{
					GameScr.isViewClanInvite = false;
				}
				else
				{
					GameScr.isViewClanInvite = true;
				}
				short num17 = msg.reader().readShort();
				Char.idHead = new short[num17];
				Char.idAvatar = new short[num17];
				for (int num18 = 0; num18 < num17; num18++)
				{
					Char.idHead[num18] = msg.reader().readShort();
					Char.idAvatar[num18] = msg.reader().readShort();
				}
				for (int num19 = 0; num19 < GameScr.info1.charId.Length; num19++)
				{
					GameScr.info1.charId[num19] = new int[3];
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
					break;
				}
				catch (Exception)
				{
					break;
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
				break;
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
					{
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1, -1, Char.myCharz(), 29);
					}
				}
				if (Char.myCharz().cHP < cHP)
				{
					GameScr.startFlyText("-" + (cHP - Char.myCharz().cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
				}
				GameScr.gI().dHP = Char.myCharz().cHP;
				if (GameScr.isPaintInfoMe)
				{
				}
				break;
			}
			case 6:
			{
				GameCanvas.debug("SA25", 2);
				if (Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
				{
					break;
				}
				int cMP = Char.myCharz().cMP;
				Char.myCharz().cMP = msg.readInt3Byte();
				if (Char.myCharz().cMP > cMP)
				{
					GameScr.startFlyText("+" + (Char.myCharz().cMP - cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
					SoundMn.gI().HP_MPup();
					if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5001)
					{
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1, -1, Char.myCharz(), 29);
					}
				}
				if (Char.myCharz().cMP < cMP)
				{
					GameScr.startFlyText("-" + (cMP - Char.myCharz().cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
				}
				Res.outz("curr MP= " + Char.myCharz().cMP);
				GameScr.gI().dMP = Char.myCharz().cMP;
				if (GameScr.isPaintInfoMe)
				{
				}
				break;
			}
			case 7:
			{
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.clanID = msg.reader().readInt();
					if (@char.clanID == -2)
					{
						@char.isCopy = true;
					}
					readCharInfo(@char, msg);
					try
					{
						@char.idAuraEff = msg.reader().readShort();
						@char.idEff_Set_Item = msg.reader().readSByte();
						break;
					}
					catch (Exception)
					{
						break;
					}
				}
				break;
			}
			case 8:
			{
				GameCanvas.debug("SA26", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char != null)
				{
					@char.cspeed = msg.reader().readByte();
				}
				break;
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
				break;
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
					{
						@char.setDefaultWeapon();
					}
				}
				break;
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
					{
						@char.setDefaultBody();
					}
				}
				break;
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
					{
						@char.setDefaultLeg();
					}
				}
				break;
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
				break;
			}
			case 14:
			{
				GameCanvas.debug("SA32", 2);
				Char @char = GameScr.findCharInMap(msg.reader().readInt());
				if (@char == null)
				{
					break;
				}
				@char.cHP = msg.readInt3Byte();
				sbyte b7 = msg.reader().readByte();
				Res.outz("player load hp type= " + b7);
				if (b7 == 1)
				{
					ServerEffect.addServerEffect(11, @char, 5);
					ServerEffect.addServerEffect(104, @char, 4);
				}
				try
				{
					@char.cHPFull = msg.readInt3Byte();
					break;
				}
				catch (Exception)
				{
					break;
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
				break;
			}
			case 35:
			{
				GameCanvas.debug("SY3", 2);
				int num = msg.reader().readInt();
				Res.outz("CID = " + num);
				if (TileMap.mapID == 130)
				{
					GameScr.gI().starVS();
				}
				if (num == Char.myCharz().charID)
				{
					Char.myCharz().cTypePk = msg.reader().readByte();
					if (GameScr.gI().isVS() && Char.myCharz().cTypePk != 0)
					{
						GameScr.gI().starVS();
					}
					Res.outz("type pk= " + Char.myCharz().cTypePk);
					Char.myCharz().npcFocus = null;
					if (!GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
					{
						Char.myCharz().mobFocus = null;
					}
					Char.myCharz().itemFocus = null;
				}
				else
				{
					Char @char = GameScr.findCharInMap(num);
					if (@char != null)
					{
						Res.outz("type pk= " + @char.cTypePk);
						@char.cTypePk = msg.reader().readByte();
						if (@char.isAttacPlayerStatus())
						{
							Char.myCharz().charFocus = @char;
						}
					}
				}
				for (int m = 0; m < GameScr.vCharInMap.size(); m++)
				{
					Char char2 = GameScr.findCharInMap(m);
					if (char2 != null && char2.cTypePk != 0 && char2.cTypePk == Char.myCharz().cTypePk)
					{
						if (!Char.myCharz().mobFocus.isMobMe)
						{
							Char.myCharz().mobFocus = null;
						}
						Char.myCharz().npcFocus = null;
						Char.myCharz().itemFocus = null;
						break;
					}
				}
				Res.outz("update type pk= ");
				break;
			}
			case 61:
			{
				string text = msg.reader().readUTF();
				sbyte[] data = new sbyte[msg.reader().readInt()];
				msg.reader().read(ref data);
				if (data.Length == 0)
				{
					data = null;
				}
				if (text.Equals("KSkill"))
				{
					GameScr.gI().onKSkill(data);
				}
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
			case 23:
			{
				short num20 = msg.reader().readShort();
				Skill skill6 = Skills.get(num20);
				useSkill(skill6);
				if (num20 != 0 && num20 != 14 && num20 != 28)
				{
					GameScr.info1.addInfo(mResources.LEARN_SKILL + " " + skill6.template.name, 0);
				}
				break;
			}
			case 62:
			{
				Res.outz("ME UPDATE SKILL");
				short skillId = msg.reader().readShort();
				Skill skill = Skills.get(skillId);
				for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
				{
					Skill skill2 = (Skill)Char.myCharz().vSkill.elementAt(i);
					if (skill2.template.id == skill.template.id)
					{
						Char.myCharz().vSkill.setElementAt(skill, i);
						break;
					}
				}
				for (int j = 0; j < Char.myCharz().vSkillFight.size(); j++)
				{
					Skill skill3 = (Skill)Char.myCharz().vSkillFight.elementAt(j);
					if (skill3.template.id == skill.template.id)
					{
						Char.myCharz().vSkillFight.setElementAt(skill, j);
						break;
					}
				}
				for (int k = 0; k < GameScr.onScreenSkill.Length; k++)
				{
					if (GameScr.onScreenSkill[k] != null && GameScr.onScreenSkill[k].template.id == skill.template.id)
					{
						GameScr.onScreenSkill[k] = skill;
						break;
					}
				}
				for (int l = 0; l < GameScr.keySkill.Length; l++)
				{
					if (GameScr.keySkill[l] != null && GameScr.keySkill[l].template.id == skill.template.id)
					{
						GameScr.keySkill[l] = skill;
						break;
					}
				}
				if (Char.myCharz().myskill.template.id == skill.template.id)
				{
					Char.myCharz().myskill = skill;
				}
				GameScr.info1.addInfo(mResources.hasJustUpgrade1 + skill.template.name + mResources.hasJustUpgrade2 + skill.point, 0);
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
		{
			Char.myCharz().myskill = skill;
		}
		else if (skill.template.Equals(Char.myCharz().myskill.template))
		{
			Char.myCharz().myskill = skill;
		}
		Char.myCharz().vSkill.addElement(skill);
		if ((skill.template.type == 1 || skill.template.type == 4 || skill.template.type == 2 || skill.template.type == 3) && (skill.template.maxPoint == 0 || (skill.template.maxPoint > 0 && skill.point > 0)))
		{
			if (skill.template.id == Char.myCharz().skillTemplateId)
			{
				Service.gI().selectSkill(Char.myCharz().skillTemplateId);
			}
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
			{
				c.statusMe = 14;
			}
			c.cHPFull = msg.readInt3Byte();
			if (c.cy >= TileMap.pxh - 100)
			{
				c.isFlyUp = true;
			}
			c.body = msg.reader().readShort();
			c.leg = msg.reader().readShort();
			c.bag = msg.reader().readUnsignedByte();
			Res.outz(" body= " + c.body + " leg= " + c.leg + " bag=" + c.bag + "BAG ==" + c.bag + "*********************************");
			c.isShadown = true;
			sbyte b = msg.reader().readByte();
			if (c.wp == -1)
			{
				c.setDefaultWeapon();
			}
			if (c.body == -1)
			{
				c.setDefaultBody();
			}
			if (c.leg == -1)
			{
				c.setDefaultLeg();
			}
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
				{
					c.isInvisiblez = true;
				}
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
			Image img = createImage(array);
			ImgByName.SetImage(text, img, nFrame);
			if (array != null)
			{
				ImgByName.saveRMS(text, nFrame, array);
			}
		}
		catch (Exception)
		{
		}
	}

	private void createItemNew(myReader d)
	{
		try
		{
			loadItemNew(d, -1, isSave: true);
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
					ItemTemplate it = new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
					ItemTemplates.add(it);
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
					ItemTemplate it2 = new ItemTemplate((short)k, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
					ItemTemplates.add(it2);
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data3 = new sbyte[d.available()];
					d.readFully(ref data3);
					Rms.saveRMS("NRitem2", data3);
					sbyte[] data4 = new sbyte[1] { GameScr.vcItem };
					Rms.saveRMS("NRitemVersion", data4);
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
					sbyte[] data5 = new sbyte[d.available()];
					d.readFully(ref data5);
					Rms.saveRMS("NRitem100", data5);
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
			int num = d.readShort();
			array = new int[num][];
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = d.readByte();
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
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
			{
				readPhuBan_CHIENTRUONGNAMEK(msg, b);
			}
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
				{
					GameScr.phuban_Info.updatePoint(type_PB, pointTeam, pointTeam2);
				}
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
				{
					GameScr.phuban_Info.updateTime(type_PB, timeSecond2);
				}
			}
			else if (b == 4)
			{
				int lifeTeam = msg.reader().readByte();
				int lifeTeam2 = msg.reader().readByte();
				if (GameScr.phuban_Info != null)
				{
					GameScr.phuban_Info.updateLife(type_PB, lifeTeam, lifeTeam2);
				}
			}
		}
		catch (Exception)
		{
		}
	}
}
