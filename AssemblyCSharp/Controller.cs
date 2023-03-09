using System;
using Assets.src.e;
using Assets.src.f;
using Assets.src.g;
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
				string text3 = msg.reader().readUTF();
				short num90 = msg.reader().readShort();
				if (ItemTime.isExistMessage(b36))
				{
					if (num90 != 0)
					{
						ItemTime.getMessageById(b36).initTimeText(b36, text3, num90);
					}
					else
					{
						GameScr.textTime.removeElement(ItemTime.getMessageById(b36));
					}
				}
				else
				{
					ItemTime itemTime = new ItemTime();
					itemTime.initTimeText(b36, text3, num90);
					GameScr.textTime.addElement(itemTime);
				}
				break;
			}
			case 112:
			{
				sbyte b43 = msg.reader().readByte();
				Res.outz("spec type= " + b43);
				if (b43 == 0)
				{
					Panel.spearcialImage = msg.reader().readShort();
					Panel.specialInfo = msg.reader().readUTF();
				}
				else
				{
					if (b43 != 1)
					{
						break;
					}
					sbyte b44 = msg.reader().readByte();
					Char.myCharz().infoSpeacialSkill = new string[b44][];
					Char.myCharz().imgSpeacialSkill = new short[b44][];
					GameCanvas.panel.speacialTabName = new string[b44][];
					for (int num108 = 0; num108 < b44; num108++)
					{
						GameCanvas.panel.speacialTabName[num108] = new string[2];
						string[] array8 = Res.split(msg.reader().readUTF(), "\n", 0);
						if (array8.Length == 2)
						{
							GameCanvas.panel.speacialTabName[num108] = array8;
						}
						if (array8.Length == 1)
						{
							GameCanvas.panel.speacialTabName[num108][0] = array8[0];
							GameCanvas.panel.speacialTabName[num108][1] = string.Empty;
						}
						int num109 = msg.reader().readByte();
						Char.myCharz().infoSpeacialSkill[num108] = new string[num109];
						Char.myCharz().imgSpeacialSkill[num108] = new short[num109];
						for (int num110 = 0; num110 < num109; num110++)
						{
							Char.myCharz().imgSpeacialSkill[num108][num110] = msg.reader().readShort();
							Char.myCharz().infoSpeacialSkill[num108][num110] = msg.reader().readUTF();
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
				sbyte b39 = msg.reader().readByte();
				GameCanvas.menu.showMenu = false;
				if (b39 == 0)
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
				sbyte b64 = msg.reader().readByte();
				for (int num148 = 0; num148 < b64; num148++)
				{
					int rank = msg.reader().readInt();
					int pId = msg.reader().readInt();
					short headID = msg.reader().readShort();
					short headICON = msg.reader().readShort();
					short body = msg.reader().readShort();
					short leg = msg.reader().readShort();
					string name = msg.reader().readUTF();
					string info3 = msg.reader().readUTF();
					TopInfo topInfo = new TopInfo();
					topInfo.rank = rank;
					topInfo.headID = headID;
					topInfo.headICON = headICON;
					topInfo.body = body;
					topInfo.leg = leg;
					topInfo.name = name;
					topInfo.info = info3;
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
					short num37 = msg.reader().readShort();
					int num38 = msg.reader().readInt();
					for (int num39 = 0; num39 < Char.myCharz().vSkill.size(); num39++)
					{
						Skill skill = (Skill)Char.myCharz().vSkill.elementAt(num39);
						if (skill != null && skill.skillId == num37)
						{
							if (num38 < skill.coolDown)
							{
								skill.lastTimeUseThisSkill = mSystem.currentTimeMillis() - (skill.coolDown - num38);
							}
							Res.outz("1 chieu id= " + skill.template.id + " cooldown= " + num38 + "curr cool down= " + skill.coolDown);
						}
					}
				}
				break;
			case -95:
			{
				sbyte b37 = msg.reader().readByte();
				Res.outz("type= " + b37);
				if (b37 == 0)
				{
					int num91 = msg.reader().readInt();
					short templateId = msg.reader().readShort();
					int num92 = msg.readInt3Byte();
					SoundMn.gI().explode_1();
					if (num91 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe = new Mob(num91, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num92, 0, num92, (short)(Char.myCharz().cx + ((Char.myCharz().cdir != 1) ? (-40) : 40)), (short)Char.myCharz().cy, 4, 0);
						Char.myCharz().mobMe.isMobMe = true;
						EffecMn.addEff(new Effect(18, Char.myCharz().mobMe.x, Char.myCharz().mobMe.y, 2, 10, -1));
						Char.myCharz().tMobMeBorn = 30;
						GameScr.vMob.addElement(Char.myCharz().mobMe);
					}
					else
					{
						@char = GameScr.findCharInMap(num91);
						if (@char != null)
						{
							Mob mob5 = new Mob(num91, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num92, 0, num92, (short)@char.cx, (short)@char.cy, 4, 0);
							mob5.isMobMe = true;
							@char.mobMe = mob5;
							GameScr.vMob.addElement(@char.mobMe);
						}
						else
						{
							Mob mob6 = GameScr.findMobInMap(num91);
							if (mob6 == null)
							{
								mob6 = new Mob(num91, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num92, 0, num92, -100, -100, 4, 0);
								mob6.isMobMe = true;
								GameScr.vMob.addElement(mob6);
							}
						}
					}
				}
				if (b37 == 1)
				{
					int num93 = msg.reader().readInt();
					int mobId = msg.reader().readByte();
					Res.outz("mod attack id= " + num93);
					if (num93 == Char.myCharz().charID)
					{
						if (GameScr.findMobInMap(mobId) != null)
						{
							Char.myCharz().mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
					else
					{
						@char = GameScr.findCharInMap(num93);
						if (@char != null && GameScr.findMobInMap(mobId) != null)
						{
							@char.mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
				}
				if (b37 == 2)
				{
					int num94 = msg.reader().readInt();
					int num95 = msg.reader().readInt();
					int num96 = msg.readInt3Byte();
					int cHPNew = msg.readInt3Byte();
					if (num94 == Char.myCharz().charID)
					{
						Res.outz("mob dame= " + num96);
						@char = GameScr.findCharInMap(num95);
						if (@char != null)
						{
							@char.cHPNew = cHPNew;
							if (Char.myCharz().mobMe.isBusyAttackSomeOne)
							{
								@char.doInjure(num96, 0, isCrit: false, isMob: true);
							}
							else
							{
								Char.myCharz().mobMe.dame = num96;
								Char.myCharz().mobMe.setAttack(@char);
							}
						}
					}
					else
					{
						mob = GameScr.findMobInMap(num94);
						if (mob != null)
						{
							if (num95 == Char.myCharz().charID)
							{
								Char.myCharz().cHPNew = cHPNew;
								if (mob.isBusyAttackSomeOne)
								{
									Char.myCharz().doInjure(num96, 0, isCrit: false, isMob: true);
								}
								else
								{
									mob.dame = num96;
									mob.setAttack(Char.myCharz());
								}
							}
							else
							{
								@char = GameScr.findCharInMap(num95);
								if (@char != null)
								{
									@char.cHPNew = cHPNew;
									if (mob.isBusyAttackSomeOne)
									{
										@char.doInjure(num96, 0, isCrit: false, isMob: true);
									}
									else
									{
										mob.dame = num96;
										mob.setAttack(@char);
									}
								}
							}
						}
					}
				}
				if (b37 == 3)
				{
					int num97 = msg.reader().readInt();
					int mobId2 = msg.reader().readInt();
					int hp = msg.readInt3Byte();
					int num98 = msg.readInt3Byte();
					@char = null;
					@char = ((Char.myCharz().charID != num97) ? GameScr.findCharInMap(num97) : Char.myCharz());
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
							mob.updateHp_bar();
							if (num98 == 0)
							{
								mob.x = mob.xFirst;
								mob.y = mob.yFirst;
								GameScr.startFlyText(mResources.miss, mob.x, mob.y - mob.h, 0, -2, mFont.MISS);
							}
							else
							{
								GameScr.startFlyText("-" + num98, mob.x, mob.y - mob.h, 0, -2, mFont.ORANGE);
							}
						}
					}
				}
				if (b37 == 4)
				{
				}
				if (b37 == 5)
				{
					int num99 = msg.reader().readInt();
					sbyte b38 = msg.reader().readByte();
					int mobId3 = msg.reader().readInt();
					int num100 = msg.readInt3Byte();
					int hp2 = msg.readInt3Byte();
					@char = null;
					@char = ((num99 != Char.myCharz().charID) ? GameScr.findCharInMap(num99) : Char.myCharz());
					if (@char == null)
					{
						return;
					}
					if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
					{
						@char.setSkillPaint(GameScr.sks[b38], 0);
					}
					else
					{
						@char.setSkillPaint(GameScr.sks[b38], 1);
					}
					Mob mob7 = GameScr.findMobInMap(mobId3);
					if (@char.cx <= mob7.x)
					{
						@char.cdir = 1;
					}
					else
					{
						@char.cdir = -1;
					}
					@char.mobFocus = mob7;
					mob7.hp = hp2;
					mob7.updateHp_bar();
					GameCanvas.debug("SA83v2", 2);
					if (num100 == 0)
					{
						mob7.x = mob7.xFirst;
						mob7.y = mob7.yFirst;
						GameScr.startFlyText(mResources.miss, mob7.x, mob7.y - mob7.h, 0, -2, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num100, mob7.x, mob7.y - mob7.h, 0, -2, mFont.ORANGE);
					}
				}
				if (b37 == 6)
				{
					int num101 = msg.reader().readInt();
					if (num101 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe.startDie();
					}
					else
					{
						@char = GameScr.findCharInMap(num101);
						@char?.mobMe.startDie();
					}
				}
				if (b37 != 7)
				{
					break;
				}
				int num102 = msg.reader().readInt();
				if (num102 == Char.myCharz().charID)
				{
					Char.myCharz().mobMe = null;
					for (int num103 = 0; num103 < GameScr.vMob.size(); num103++)
					{
						if (((Mob)GameScr.vMob.elementAt(num103)).mobId == num102)
						{
							GameScr.vMob.removeElementAt(num103);
						}
					}
					break;
				}
				@char = GameScr.findCharInMap(num102);
				for (int num104 = 0; num104 < GameScr.vMob.size(); num104++)
				{
					if (((Mob)GameScr.vMob.elementAt(num104)).mobId == num102)
					{
						GameScr.vMob.removeElementAt(num104);
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
				sbyte b61 = msg.reader().readByte();
				GameCanvas.panel.mapNames = new string[b61];
				GameCanvas.panel.planetNames = new string[b61];
				for (int num142 = 0; num142 < b61; num142++)
				{
					GameCanvas.panel.mapNames[num142] = msg.reader().readUTF();
					GameCanvas.panel.planetNames[num142] = msg.reader().readUTF();
				}
				GameCanvas.panel.setTypeMapTrans();
				GameCanvas.panel.show();
				break;
			}
			case -90:
			{
				sbyte b34 = msg.reader().readByte();
				int num86 = msg.reader().readInt();
				Res.outz("===> UPDATE_BODY:    type = " + b34);
				@char = ((Char.myCharz().charID != num86) ? GameScr.findCharInMap(num86) : Char.myCharz());
				if (b34 != -1)
				{
					short num87 = msg.reader().readShort();
					short num88 = msg.reader().readShort();
					short num89 = msg.reader().readShort();
					sbyte b35 = msg.reader().readByte();
					Res.err("====> Cmd: -90 UPDATE_BODY   \n  isMonkey= " + b35 + " head=  " + num87 + " body= " + num88 + " legU= " + num89);
					if (@char != null)
					{
						if (@char.charID == num86)
						{
							@char.isMask = true;
							@char.isMonkey = b35;
							if (@char.isMonkey != 0)
							{
								@char.isWaitMonkey = false;
								@char.isLockMove = false;
							}
						}
						else if (@char != null)
						{
							@char.isMask = true;
							@char.isMonkey = b35;
						}
						if (num87 != -1)
						{
							@char.head = num87;
						}
						if (num88 != -1)
						{
							@char.body = num88;
						}
						if (num89 != -1)
						{
							@char.leg = num89;
						}
					}
				}
				if (b34 == -1 && @char != null)
				{
					@char.isMask = false;
					@char.isMonkey = 0;
				}
				if (@char == null)
				{
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
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data);
				sbyte[] data2 = new sbyte[1] { GameScr.vcData };
				Rms.saveRMS("NRdataVersion", data2);
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
				sbyte b11 = msg.reader().readByte();
				Res.outz("server gui ve giao dich action = " + b11);
				if (b11 == 0)
				{
					int playerID = msg.reader().readInt();
					GameScr.gI().giaodich(playerID);
				}
				if (b11 == 1)
				{
					int num15 = msg.reader().readInt();
					Char char4 = GameScr.findCharInMap(num15);
					if (char4 == null)
					{
						return;
					}
					GameCanvas.panel.setTypeGiaoDich(char4);
					GameCanvas.panel.show();
					Service.gI().getPlayerMenu(num15);
				}
				if (b11 == 2)
				{
					sbyte b12 = msg.reader().readByte();
					for (int n = 0; n < GameCanvas.panel.vMyGD.size(); n++)
					{
						Item item2 = (Item)GameCanvas.panel.vMyGD.elementAt(n);
						if (item2.indexUI == b12)
						{
							GameCanvas.panel.vMyGD.removeElement(item2);
							break;
						}
					}
				}
				if (b11 == 5)
				{
				}
				if (b11 == 6)
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
					sbyte b13 = msg.reader().readByte();
					Res.outz("item size = " + b13);
					for (int num16 = 0; num16 < b13; num16++)
					{
						Item item3 = new Item();
						item3.template = ItemTemplates.get(msg.reader().readShort());
						item3.quantity = msg.reader().readInt();
						int num17 = msg.reader().readUnsignedByte();
						if (num17 != 0)
						{
							item3.itemOption = new ItemOption[num17];
							for (int num18 = 0; num18 < item3.itemOption.Length; num18++)
							{
								int num19 = msg.reader().readUnsignedByte();
								int param2 = msg.reader().readUnsignedShort();
								if (num19 != -1)
								{
									item3.itemOption[num18] = new ItemOption(num19, param2);
									item3.compare = GameCanvas.panel.getCompare(item3);
								}
							}
						}
						if (GameCanvas.panel2 != null)
						{
							GameCanvas.panel2.vFriendGD.addElement(item3);
						}
						else
						{
							GameCanvas.panel.vFriendGD.addElement(item3);
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
				if (b11 == 7)
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
				sbyte b54 = msg.reader().readByte();
				if (b54 == 0)
				{
					int num131 = msg.reader().readUnsignedShort();
					Res.outz("lent =" + num131);
					sbyte[] data3 = new sbyte[num131];
					msg.reader().read(ref data3, 0, num131);
					GameScr.imgCapcha = Image.createImage(data3, 0, num131);
					GameScr.gI().keyInput = "-----";
					GameScr.gI().strCapcha = msg.reader().readUTF();
					GameScr.gI().keyCapcha = new int[GameScr.gI().strCapcha.Length];
					GameScr.gI().mobCapcha = new Mob();
					GameScr.gI().right = null;
				}
				if (b54 == 1)
				{
					MobCapcha.isAttack = true;
				}
				if (b54 == 2)
				{
					MobCapcha.explode = true;
					GameScr.gI().right = GameScr.gI().cmdFocus;
				}
				break;
			}
			case -112:
			{
				sbyte b48 = msg.reader().readByte();
				if (b48 == 0)
				{
					sbyte mobIndex = msg.reader().readByte();
					GameScr.findMobInMap(mobIndex).clearBody();
				}
				if (b48 == 1)
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
				sbyte b27 = msg.reader().readByte();
				if (b27 == 0)
				{
					int num64 = msg.reader().readShort();
					int bgRID = msg.reader().readShort();
					int num65 = msg.reader().readUnsignedByte();
					int num66 = msg.reader().readInt();
					string text2 = msg.reader().readUTF();
					int num67 = msg.reader().readShort();
					int num68 = msg.reader().readShort();
					sbyte b28 = msg.reader().readByte();
					if (b28 == 1)
					{
						GameScr.gI().isRongNamek = true;
					}
					else
					{
						GameScr.gI().isRongNamek = false;
					}
					GameScr.gI().xR = num67;
					GameScr.gI().yR = num68;
					Res.outz("xR= " + num67 + " yR= " + num68 + " +++++++++++++++++++++++++++++++++++++++");
					if (Char.myCharz().charID == num66)
					{
						GameCanvas.panel.hideNow();
						GameScr.gI().activeRongThanEff(isMe: true);
					}
					else if (TileMap.mapID == num64 && TileMap.zoneID == num65)
					{
						GameScr.gI().activeRongThanEff(isMe: false);
					}
					else if (mGraphics.zoomLevel > 1)
					{
						GameScr.gI().doiMauTroi();
					}
					GameScr.gI().mapRID = num64;
					GameScr.gI().bgRID = bgRID;
					GameScr.gI().zoneRID = num65;
				}
				if (b27 == 1)
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
				if (b27 != 2)
				{
				}
				break;
			}
			case -82:
			{
				sbyte b18 = msg.reader().readByte();
				TileMap.tileIndex = new int[b18][][];
				TileMap.tileType = new int[b18][];
				for (int num45 = 0; num45 < b18; num45++)
				{
					sbyte b19 = msg.reader().readByte();
					TileMap.tileType[num45] = new int[b19];
					TileMap.tileIndex[num45] = new int[b19][];
					for (int num46 = 0; num46 < b19; num46++)
					{
						TileMap.tileType[num45][num46] = msg.reader().readInt();
						sbyte b20 = msg.reader().readByte();
						TileMap.tileIndex[num45][num46] = new int[b20];
						for (int num47 = 0; num47 < b20; num47++)
						{
							TileMap.tileIndex[num45][num46][num47] = msg.reader().readByte();
						}
					}
				}
				break;
			}
			case -81:
			{
				sbyte b8 = msg.reader().readByte();
				if (b8 == 0)
				{
					string src = msg.reader().readUTF();
					string src2 = msg.reader().readUTF();
					GameCanvas.panel.setTypeCombine();
					GameCanvas.panel.combineInfo = mFont.tahoma_7b_blue.splitFontArray(src, Panel.WIDTH_PANEL);
					GameCanvas.panel.combineTopInfo = mFont.tahoma_7.splitFontArray(src2, Panel.WIDTH_PANEL);
					GameCanvas.panel.show();
				}
				if (b8 == 1)
				{
					GameCanvas.panel.vItemCombine.removeAllElements();
					sbyte b9 = msg.reader().readByte();
					for (int k = 0; k < b9; k++)
					{
						sbyte b10 = msg.reader().readByte();
						for (int l = 0; l < Char.myCharz().arrItemBag.Length; l++)
						{
							Item item = Char.myCharz().arrItemBag[l];
							if (item != null && item.indexUI == b10)
							{
								item.isSelect = true;
								GameCanvas.panel.vItemCombine.addElement(item);
							}
						}
					}
					if (GameCanvas.panel.isShow)
					{
						GameCanvas.panel.setTabCombine();
					}
				}
				if (b8 == 2)
				{
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b8 == 3)
				{
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b8 == 4)
				{
					short iconID = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(1);
				}
				if (b8 == 5)
				{
					short iconID2 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID2;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(2);
				}
				if (b8 == 6)
				{
					short iconID3 = msg.reader().readShort();
					short iconID4 = msg.reader().readShort();
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(3);
					GameCanvas.panel.iconID1 = iconID3;
					GameCanvas.panel.iconID3 = iconID4;
				}
				if (b8 == 7)
				{
					short iconID5 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID5;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(4);
				}
				if (b8 == 8)
				{
					GameCanvas.panel.iconID3 = -1;
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(4);
				}
				short num14 = 21;
				try
				{
					num14 = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				for (int m = 0; m < GameScr.vNpc.size(); m++)
				{
					Npc npc = (Npc)GameScr.vNpc.elementAt(m);
					if (npc.template.npcTemplateId == num14)
					{
						GameCanvas.panel.xS = npc.cx - GameScr.cmx;
						GameCanvas.panel.yS = npc.cy - GameScr.cmy;
						GameCanvas.panel.idNPC = num14;
						break;
					}
				}
				break;
			}
			case -80:
			{
				sbyte b25 = msg.reader().readByte();
				InfoDlg.hide();
				if (b25 == 0)
				{
					GameCanvas.panel.vFriend.removeAllElements();
					int num55 = msg.reader().readUnsignedByte();
					for (int num56 = 0; num56 < num55; num56++)
					{
						Char char7 = new Char();
						char7.charID = msg.reader().readInt();
						char7.head = msg.reader().readShort();
						char7.headICON = msg.reader().readShort();
						char7.body = msg.reader().readShort();
						char7.leg = msg.reader().readShort();
						char7.bag = msg.reader().readUnsignedByte();
						char7.cName = msg.reader().readUTF();
						bool isOnline = msg.reader().readBoolean();
						InfoItem infoItem = new InfoItem(mResources.power + ": " + msg.reader().readUTF());
						infoItem.charInfo = char7;
						infoItem.isOnline = isOnline;
						GameCanvas.panel.vFriend.addElement(infoItem);
					}
					GameCanvas.panel.setTypeFriend();
					GameCanvas.panel.show();
				}
				if (b25 == 3)
				{
					MyVector vFriend = GameCanvas.panel.vFriend;
					int num57 = msg.reader().readInt();
					Res.outz("online offline id=" + num57);
					for (int num58 = 0; num58 < vFriend.size(); num58++)
					{
						InfoItem infoItem2 = (InfoItem)vFriend.elementAt(num58);
						if (infoItem2.charInfo != null && infoItem2.charInfo.charID == num57)
						{
							Res.outz("online= " + infoItem2.isOnline);
							infoItem2.isOnline = msg.reader().readBoolean();
							break;
						}
					}
				}
				if (b25 != 2)
				{
					break;
				}
				MyVector vFriend2 = GameCanvas.panel.vFriend;
				int num59 = msg.reader().readInt();
				for (int num60 = 0; num60 < vFriend2.size(); num60++)
				{
					InfoItem infoItem3 = (InfoItem)vFriend2.elementAt(num60);
					if (infoItem3.charInfo != null && infoItem3.charInfo.charID == num59)
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
				sbyte b33 = msg.reader().readByte();
				if (b33 == 0)
				{
					GameCanvas.panel.vEnemy.removeAllElements();
					int num83 = msg.reader().readUnsignedByte();
					for (int num84 = 0; num84 < num83; num84++)
					{
						Char char8 = new Char();
						char8.charID = msg.reader().readInt();
						char8.head = msg.reader().readShort();
						char8.headICON = msg.reader().readShort();
						char8.body = msg.reader().readShort();
						char8.leg = msg.reader().readShort();
						char8.bag = msg.reader().readShort();
						char8.cName = msg.reader().readUTF();
						InfoItem infoItem4 = new InfoItem(msg.reader().readUTF());
						bool flag8 = msg.reader().readBoolean();
						infoItem4.charInfo = char8;
						infoItem4.isOnline = flag8;
						Res.outz("isonline = " + flag8);
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
				int num69 = msg.reader().readInt();
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
				short num52 = msg.reader().readShort();
				BgItem.newSmallVersion = new sbyte[num52];
				for (int num53 = 0; num53 < num52; num53++)
				{
					BgItem.newSmallVersion[num53] = msg.reader().readByte();
				}
				break;
			}
			case -77:
			{
				short num50 = msg.reader().readShort();
				SmallImage.newSmallVersion = new sbyte[num50];
				SmallImage.maxSmall = num50;
				SmallImage.imgNew = new Small[num50];
				for (int num51 = 0; num51 < num50; num51++)
				{
					SmallImage.newSmallVersion[num51] = msg.reader().readByte();
				}
				break;
			}
			case -76:
			{
				sbyte b49 = msg.reader().readByte();
				if (b49 == 0)
				{
					sbyte b50 = msg.reader().readByte();
					if (b50 <= 0)
					{
						return;
					}
					Char.myCharz().arrArchive = new Archivement[b50];
					for (int num122 = 0; num122 < b50; num122++)
					{
						Char.myCharz().arrArchive[num122] = new Archivement();
						Char.myCharz().arrArchive[num122].info1 = num122 + 1 + ". " + msg.reader().readUTF();
						Char.myCharz().arrArchive[num122].info2 = msg.reader().readUTF();
						Char.myCharz().arrArchive[num122].money = msg.reader().readShort();
						Char.myCharz().arrArchive[num122].isFinish = msg.reader().readBoolean();
						Char.myCharz().arrArchive[num122].isRecieve = msg.reader().readBoolean();
					}
					GameCanvas.panel.setTypeArchivement();
					GameCanvas.panel.show();
				}
				else if (b49 == 1)
				{
					int num123 = msg.reader().readUnsignedByte();
					if (Char.myCharz().arrArchive[num123] != null)
					{
						Char.myCharz().arrArchive[num123].isRecieve = true;
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
				bool flag9 = true;
				sbyte b69 = msg.reader().readByte();
				Res.outz("action = " + b69);
				if (b69 == 0)
				{
					int num170 = msg.reader().readInt();
					string text7 = Rms.loadRMSString("ResVersion");
					int num171 = ((text7 == null || !(text7 != string.Empty)) ? (-1) : int.Parse(text7));
					if (num171 == -1 || num171 != num170)
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
				if (b69 == 1)
				{
					ServerListScreen.strWait = mResources.downloading_data;
					short nBig = msg.reader().readShort();
					ServerListScreen.nBig = nBig;
					Service.gI().getResource(2, null);
				}
				if (b69 == 2)
				{
					try
					{
						isLoadingData = true;
						GameCanvas.endDlg();
						ServerListScreen.demPercent++;
						ServerListScreen.percent = ServerListScreen.demPercent * 100 / ServerListScreen.nBig;
						string original = msg.reader().readUTF();
						string[] array17 = Res.split(original, "/", 0);
						string filename = "x" + mGraphics.zoomLevel + array17[array17.Length - 1];
						int num172 = msg.reader().readInt();
						sbyte[] data5 = new sbyte[num172];
						msg.reader().read(ref data5, 0, num172);
						Rms.saveRMS(filename, data5);
					}
					catch (Exception)
					{
						GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
					}
				}
				if (b69 == 3 && flag9)
				{
					isLoadingData = false;
					int num173 = msg.reader().readInt();
					Res.outz("last version= " + num173);
					Rms.saveRMSString("ResVersion", num173 + string.Empty);
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
				int num149 = msg.reader().readUnsignedByte();
				sbyte b65 = msg.reader().readByte();
				if (b65 <= 0)
				{
					break;
				}
				ClanImage clanImage3 = ClanImage.getClanImage((short)num149);
				if (clanImage3 == null)
				{
					break;
				}
				clanImage3.idImage = new short[b65];
				for (int num150 = 0; num150 < b65; num150++)
				{
					clanImage3.idImage[num150] = msg.reader().readShort();
					if (clanImage3.idImage[num150] > 0)
					{
						SmallImage.vKeys.addElement(clanImage3.idImage[num150] + string.Empty);
					}
				}
				break;
			}
			case -65:
			{
				Res.outz("TELEPORT ...................................................");
				InfoDlg.hide();
				int num48 = msg.reader().readInt();
				sbyte b21 = msg.reader().readByte();
				if (b21 == 0)
				{
					break;
				}
				if (Char.myCharz().charID == num48)
				{
					isStopReadMessage = true;
					GameScr.lockTick = 500;
					GameScr.gI().center = null;
					if (b21 == 0 || b21 == 1 || b21 == 3)
					{
						Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 0, isMe: true, (b21 != 1) ? b21 : Char.myCharz().cgender);
						Teleport.addTeleport(p);
					}
					if (b21 == 2)
					{
						GameScr.lockTick = 50;
						Char.myCharz().hide();
					}
				}
				else
				{
					Char char6 = GameScr.findCharInMap(num48);
					if ((b21 == 0 || b21 == 1 || b21 == 3) && char6 != null)
					{
						char6.isUsePlane = true;
						Teleport teleport = new Teleport(char6.cx, char6.cy, char6.head, char6.cdir, 0, isMe: false, (b21 != 1) ? b21 : char6.cgender);
						teleport.id = num48;
						Teleport.addTeleport(teleport);
					}
					if (b21 == 2)
					{
						char6.hide();
					}
				}
				break;
			}
			case -64:
			{
				int num156 = msg.reader().readInt();
				int num157 = msg.reader().readUnsignedByte();
				@char = null;
				@char = ((num156 != Char.myCharz().charID) ? GameScr.findCharInMap(num156) : Char.myCharz());
				@char.bag = num157;
				if (@char.bag >= 201 && @char.bag < 255)
				{
					Effect effect = new Effect(@char.bag, @char, 2, -1, 10, 1);
					effect.typeEff = 5;
					@char.addEffChar(effect);
				}
				else
				{
					for (int num158 = 0; num158 < 54; num158++)
					{
						@char.removeEffChar(0, 201 + num158);
					}
				}
				Res.outz("cmd:-64 UPDATE BAG PLAER = " + ((@char != null) ? @char.cName : string.Empty) + num156 + " BAG ID= " + num157);
				break;
			}
			case -63:
			{
				Res.outz("GET BAG");
				int num127 = msg.reader().readUnsignedByte();
				sbyte b51 = msg.reader().readByte();
				ClanImage clanImage2 = new ClanImage();
				clanImage2.ID = num127;
				if (b51 > 0)
				{
					clanImage2.idImage = new short[b51];
					for (int num128 = 0; num128 < b51; num128++)
					{
						clanImage2.idImage[num128] = msg.reader().readShort();
						Res.outz("ID=  " + num127 + " frame= " + clanImage2.idImage[num128]);
					}
					ClanImage.idImages.put(num127 + string.Empty, clanImage2);
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
				InfoDlg.hide();
				bool flag3 = false;
				int num33 = msg.reader().readInt();
				Res.outz("clanId= " + num33);
				if (num33 == -1)
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
				Char.myCharz().clan.ID = num33;
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
				for (int num34 = 0; num34 < Char.myCharz().clan.currMember; num34++)
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
				int num35 = msg.reader().readUnsignedByte();
				for (int num36 = 0; num36 < num35; num36++)
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
				Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -537 MY CLAN INFO");
				break;
			}
			case -52:
			{
				sbyte b66 = msg.reader().readByte();
				if (b66 == 0)
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
					{
						GameCanvas.panel.myMember = new MyVector();
					}
					GameCanvas.panel.myMember.addElement(member2);
					GameCanvas.panel.initTabClans();
				}
				if (b66 == 1)
				{
					GameCanvas.panel.myMember.removeElementAt(msg.reader().readByte());
					GameCanvas.panel.currentListLength--;
					GameCanvas.panel.initTabClans();
				}
				if (b66 == 2)
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
					for (int num151 = 0; num151 < GameCanvas.panel.myMember.size(); num151++)
					{
						Member member4 = (Member)GameCanvas.panel.myMember.elementAt(num151);
						if (member4.ID == member3.ID)
						{
							if (Char.myCharz().charID == member3.ID)
							{
								Char.myCharz().role = member3.role;
							}
							Member o2 = member3;
							GameCanvas.panel.myMember.removeElement(member4);
							GameCanvas.panel.myMember.insertElementAt(o2, num151);
							return;
						}
					}
				}
				Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -52  MY CLAN UPDSTE");
				break;
			}
			case -50:
			{
				InfoDlg.hide();
				GameCanvas.panel.member = new MyVector();
				sbyte b67 = msg.reader().readByte();
				for (int num155 = 0; num155 < b67; num155++)
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
					member5.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.member.addElement(member5);
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
					for (int num129 = 0; num129 < GameCanvas.panel.clans.Length; num129++)
					{
						GameCanvas.panel.clans[num129] = new Clan();
						GameCanvas.panel.clans[num129].ID = msg.reader().readInt();
						GameCanvas.panel.clans[num129].name = msg.reader().readUTF();
						GameCanvas.panel.clans[num129].slogan = msg.reader().readUTF();
						GameCanvas.panel.clans[num129].imgID = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num129].powerPoint = msg.reader().readUTF();
						GameCanvas.panel.clans[num129].leaderName = msg.reader().readUTF();
						GameCanvas.panel.clans[num129].currMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num129].maxMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[num129].date = msg.reader().readInt();
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
				sbyte b41 = msg.reader().readByte();
				if (b41 == 1 || b41 == 3)
				{
					GameCanvas.endDlg();
					ClanImage.vClanImage.removeAllElements();
					int num106 = msg.reader().readUnsignedByte();
					for (int num107 = 0; num107 < num106; num107++)
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
						ClanImage.getClanImage((short)clanImage.ID).name = clanImage.name;
						ClanImage.getClanImage((short)clanImage.ID).xu = clanImage.xu;
						ClanImage.getClanImage((short)clanImage.ID).luong = clanImage.luong;
					}
					if (Char.myCharz().clan != null)
					{
						GameCanvas.panel.changeIcon();
					}
				}
				if (b41 == 4)
				{
					Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
					Char.myCharz().clan.slogan = msg.reader().readUTF();
				}
				break;
			}
			case -61:
			{
				int num85 = msg.reader().readInt();
				if (num85 != Char.myCharz().charID)
				{
					if (GameScr.findCharInMap(num85) != null)
					{
						GameScr.findCharInMap(num85).clanID = msg.reader().readInt();
						if (GameScr.findCharInMap(num85).clanID == -2)
						{
							GameScr.findCharInMap(num85).isCopy = true;
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
				bool flag6 = msg.reader().readBool();
				Res.outz("isRes= " + flag6);
				if (!flag6)
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
				sbyte b15 = msg.reader().readByte();
				if (b15 == 0)
				{
					Char.myCharz().havePet = false;
				}
				if (b15 == 1)
				{
					Char.myCharz().havePet = true;
				}
				if (b15 != 2)
				{
					break;
				}
				InfoDlg.hide();
				Char.myPetz().head = msg.reader().readShort();
				Char.myPetz().setDefaultPart();
				int num24 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num24);
				Char.myPetz().arrItemBody = new Item[num24];
				for (int num25 = 0; num25 < num24; num25++)
				{
					short num26 = msg.reader().readShort();
					Res.outz("template id= " + num26);
					if (num26 == -1)
					{
						continue;
					}
					Res.outz("1");
					Char.myPetz().arrItemBody[num25] = new Item();
					Char.myPetz().arrItemBody[num25].template = ItemTemplates.get(num26);
					int num27 = Char.myPetz().arrItemBody[num25].template.type;
					Char.myPetz().arrItemBody[num25].quantity = msg.reader().readInt();
					Res.outz("3");
					Char.myPetz().arrItemBody[num25].info = msg.reader().readUTF();
					Char.myPetz().arrItemBody[num25].content = msg.reader().readUTF();
					int num28 = msg.reader().readUnsignedByte();
					Res.outz("option size= " + num28);
					if (num28 != 0)
					{
						Char.myPetz().arrItemBody[num25].itemOption = new ItemOption[num28];
						for (int num29 = 0; num29 < Char.myPetz().arrItemBody[num25].itemOption.Length; num29++)
						{
							int num30 = msg.reader().readUnsignedByte();
							int param3 = msg.reader().readUnsignedShort();
							if (num30 != -1)
							{
								Char.myPetz().arrItemBody[num25].itemOption[num29] = new ItemOption(num30, param3);
							}
						}
					}
					switch (num27)
					{
					case 0:
						Char.myPetz().body = Char.myPetz().arrItemBody[num25].template.part;
						break;
					case 1:
						Char.myPetz().leg = Char.myPetz().arrItemBody[num25].template.part;
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
				for (int num31 = 0; num31 < Char.myPetz().arrPetSkill.Length; num31++)
				{
					short num32 = msg.reader().readShort();
					if (num32 != -1)
					{
						Char.myPetz().arrPetSkill[num31] = Skills.get(num32);
						continue;
					}
					Char.myPetz().arrPetSkill[num31] = new Skill();
					Char.myPetz().arrPetSkill[num31].template = null;
					Char.myPetz().arrPetSkill[num31].moreInfo = msg.reader().readUTF();
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
				sbyte b68 = msg.reader().readByte();
				Res.outz("cAction= " + b68);
				if (b68 != 0)
				{
					break;
				}
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().setDefaultPart();
				int num163 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num163);
				Char.myCharz().arrItemBody = new Item[num163];
				for (int num164 = 0; num164 < num163; num164++)
				{
					short num165 = msg.reader().readShort();
					if (num165 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBody[num164] = new Item();
					Char.myCharz().arrItemBody[num164].template = ItemTemplates.get(num165);
					int num166 = Char.myCharz().arrItemBody[num164].template.type;
					Char.myCharz().arrItemBody[num164].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBody[num164].info = msg.reader().readUTF();
					Char.myCharz().arrItemBody[num164].content = msg.reader().readUTF();
					int num167 = msg.reader().readUnsignedByte();
					if (num167 != 0)
					{
						Char.myCharz().arrItemBody[num164].itemOption = new ItemOption[num167];
						for (int num168 = 0; num168 < Char.myCharz().arrItemBody[num164].itemOption.Length; num168++)
						{
							int num169 = msg.reader().readUnsignedByte();
							int param6 = msg.reader().readUnsignedShort();
							if (num169 != -1)
							{
								Char.myCharz().arrItemBody[num164].itemOption[num168] = new ItemOption(num169, param6);
							}
						}
					}
					switch (num166)
					{
					case 0:
						Char.myCharz().body = Char.myCharz().arrItemBody[num164].template.part;
						break;
					case 1:
						Char.myCharz().leg = Char.myCharz().arrItemBody[num164].template.part;
						break;
					}
				}
				break;
			}
			case -36:
			{
				sbyte b6 = msg.reader().readByte();
				Res.outz("cAction= " + b6);
				if (b6 == 0)
				{
					int num9 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBag = new Item[num9];
					GameScr.hpPotion = 0;
					Res.outz("numC=" + num9);
					for (int i = 0; i < num9; i++)
					{
						short num10 = msg.reader().readShort();
						if (num10 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBag[i] = new Item();
						Char.myCharz().arrItemBag[i].template = ItemTemplates.get(num10);
						Char.myCharz().arrItemBag[i].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBag[i].info = msg.reader().readUTF();
						Char.myCharz().arrItemBag[i].content = msg.reader().readUTF();
						Char.myCharz().arrItemBag[i].indexUI = i;
						int num11 = msg.reader().readUnsignedByte();
						if (num11 != 0)
						{
							Char.myCharz().arrItemBag[i].itemOption = new ItemOption[num11];
							for (int j = 0; j < Char.myCharz().arrItemBag[i].itemOption.Length; j++)
							{
								int num12 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								if (num12 != -1)
								{
									Char.myCharz().arrItemBag[i].itemOption[j] = new ItemOption(num12, param);
								}
							}
							Char.myCharz().arrItemBag[i].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemBag[i]);
						}
						if (Char.myCharz().arrItemBag[i].template.type == 11)
						{
						}
						if (Char.myCharz().arrItemBag[i].template.type == 6)
						{
							GameScr.hpPotion += Char.myCharz().arrItemBag[i].quantity;
						}
					}
				}
				if (b6 == 2)
				{
					sbyte b7 = msg.reader().readByte();
					int quantity = msg.reader().readInt();
					int quantity2 = Char.myCharz().arrItemBag[b7].quantity;
					Char.myCharz().arrItemBag[b7].quantity = quantity;
					if (Char.myCharz().arrItemBag[b7].quantity < quantity2 && Char.myCharz().arrItemBag[b7].template.type == 6)
					{
						GameScr.hpPotion -= quantity2 - Char.myCharz().arrItemBag[b7].quantity;
					}
					if (Char.myCharz().arrItemBag[b7].quantity == 0)
					{
						Char.myCharz().arrItemBag[b7] = null;
					}
				}
				break;
			}
			case -35:
			{
				sbyte b45 = msg.reader().readByte();
				Res.outz("cAction= " + b45);
				if (b45 == 0)
				{
					int num111 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBox = new Item[num111];
					GameCanvas.panel.hasUse = 0;
					for (int num112 = 0; num112 < num111; num112++)
					{
						short num113 = msg.reader().readShort();
						if (num113 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBox[num112] = new Item();
						Char.myCharz().arrItemBox[num112].template = ItemTemplates.get(num113);
						Char.myCharz().arrItemBox[num112].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBox[num112].info = msg.reader().readUTF();
						Char.myCharz().arrItemBox[num112].content = msg.reader().readUTF();
						int num114 = msg.reader().readUnsignedByte();
						if (num114 != 0)
						{
							Char.myCharz().arrItemBox[num112].itemOption = new ItemOption[num114];
							for (int num115 = 0; num115 < Char.myCharz().arrItemBox[num112].itemOption.Length; num115++)
							{
								int num116 = msg.reader().readUnsignedByte();
								int param5 = msg.reader().readUnsignedShort();
								if (num116 != -1)
								{
									Char.myCharz().arrItemBox[num112].itemOption[num115] = new ItemOption(num116, param5);
								}
							}
						}
						GameCanvas.panel.hasUse++;
					}
				}
				if (b45 == 1)
				{
					bool isBoxClan = false;
					try
					{
						sbyte b46 = msg.reader().readByte();
						if (b46 == 1)
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
				if (b45 == 2)
				{
					sbyte b47 = msg.reader().readByte();
					int quantity3 = msg.reader().readInt();
					Char.myCharz().arrItemBox[b47].quantity = quantity3;
					if (Char.myCharz().arrItemBox[b47].quantity == 0)
					{
						Char.myCharz().arrItemBox[b47] = null;
					}
				}
				break;
			}
			case -45:
			{
				sbyte b55 = msg.reader().readByte();
				int num137 = msg.reader().readInt();
				short num138 = msg.reader().readShort();
				Res.outz("skill type= " + b55 + "   player use= " + num137);
				if (b55 == 0)
				{
					Res.outz("id use= " + num137);
					if (Char.myCharz().charID != num137)
					{
						@char = GameScr.findCharInMap(num137);
						if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
						{
							@char.setSkillPaint(GameScr.sks[num138], 0);
						}
						else
						{
							@char.setSkillPaint(GameScr.sks[num138], 1);
							@char.delayFall = 20;
						}
					}
					else
					{
						Char.myCharz().saveLoadPreviousSkill();
						Res.outz("LOAD LAST SKILL");
					}
					sbyte b56 = msg.reader().readByte();
					Res.outz("npc size= " + b56);
					for (int num139 = 0; num139 < b56; num139++)
					{
						sbyte b57 = msg.reader().readByte();
						sbyte b58 = msg.reader().readByte();
						Res.outz("index= " + b57);
						if (num138 >= 42 && num138 <= 48)
						{
							((Mob)GameScr.vMob.elementAt(b57)).isFreez = true;
							((Mob)GameScr.vMob.elementAt(b57)).seconds = b58;
							((Mob)GameScr.vMob.elementAt(b57)).last = (((Mob)GameScr.vMob.elementAt(b57)).cur = mSystem.currentTimeMillis());
						}
					}
					sbyte b59 = msg.reader().readByte();
					for (int num140 = 0; num140 < b59; num140++)
					{
						int num141 = msg.reader().readInt();
						sbyte b60 = msg.reader().readByte();
						Res.outz("player ID= " + num141 + " my ID= " + Char.myCharz().charID);
						if (num138 < 42 || num138 > 48)
						{
							continue;
						}
						if (num141 == Char.myCharz().charID)
						{
							if (!Char.myCharz().isFlyAndCharge && !Char.myCharz().isStandAndCharge)
							{
								GameScr.gI().isFreez = true;
								Char.myCharz().isFreez = true;
								Char.myCharz().freezSeconds = b60;
								Char.myCharz().lastFreez = (Char.myCharz().currFreez = mSystem.currentTimeMillis());
								Char.myCharz().isLockMove = true;
							}
						}
						else
						{
							@char = GameScr.findCharInMap(num141);
							if (@char != null && !@char.isFlyAndCharge && !@char.isStandAndCharge)
							{
								@char.isFreez = true;
								@char.seconds = b60;
								@char.freezSeconds = b60;
								@char.lastFreez = (GameScr.findCharInMap(num141).currFreez = mSystem.currentTimeMillis());
							}
						}
					}
				}
				if (b55 == 1 && num137 != Char.myCharz().charID)
				{
					GameScr.findCharInMap(num137).isCharge = true;
				}
				if (b55 == 3)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().isCharge = false;
						SoundMn.gI().taitaoPause();
						Char.myCharz().saveLoadPreviousSkill();
					}
					else
					{
						GameScr.findCharInMap(num137).isCharge = false;
					}
				}
				if (b55 == 4)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort() - 1000;
						Char.myCharz().last = mSystem.currentTimeMillis();
						Res.outz("second= " + Char.myCharz().seconds + " last= " + Char.myCharz().last);
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						switch (GameScr.findCharInMap(num137).cgender)
						{
						case 0:
							GameScr.findCharInMap(num137).useChargeSkill(isGround: false);
							break;
						case 1:
							GameScr.findCharInMap(num137).useChargeSkill(isGround: true);
							break;
						}
						GameScr.findCharInMap(num137).skillTemplateId = num138;
						GameScr.findCharInMap(num137).isUseSkillAfterCharge = true;
						GameScr.findCharInMap(num137).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num137).last = mSystem.currentTimeMillis();
					}
				}
				if (b55 == 5)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().stopUseChargeSkill();
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						GameScr.findCharInMap(num137).stopUseChargeSkill();
					}
				}
				if (b55 == 6)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().setAutoSkillPaint(GameScr.sks[num138], 0);
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						GameScr.findCharInMap(num137).setAutoSkillPaint(GameScr.sks[num138], 0);
						SoundMn.gI().gong();
					}
				}
				if (b55 == 7)
				{
					if (num137 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort();
						Res.outz("second = " + Char.myCharz().seconds);
						Char.myCharz().last = mSystem.currentTimeMillis();
					}
					else if (GameScr.findCharInMap(num137) != null)
					{
						GameScr.findCharInMap(num137).useChargeSkill(isGround: true);
						GameScr.findCharInMap(num137).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num137).last = mSystem.currentTimeMillis();
						SoundMn.gI().gong();
					}
				}
				if (b55 == 8 && num137 != Char.myCharz().charID && GameScr.findCharInMap(num137) != null)
				{
					GameScr.findCharInMap(num137).setAutoSkillPaint(GameScr.sks[num138], 0);
				}
				break;
			}
			case -44:
			{
				bool flag7 = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag7 = true;
				}
				sbyte b30 = msg.reader().readByte();
				int num70 = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop = new Item[num70][];
				GameCanvas.panel.shopTabName = new string[num70 + ((!flag7) ? 1 : 0)][];
				for (int num71 = 0; num71 < GameCanvas.panel.shopTabName.Length; num71++)
				{
					GameCanvas.panel.shopTabName[num71] = new string[2];
				}
				if (b30 == 2)
				{
					GameCanvas.panel.maxPageShop = new int[num70];
					GameCanvas.panel.currPageShop = new int[num70];
				}
				if (!flag7)
				{
					GameCanvas.panel.shopTabName[num70] = mResources.inventory;
				}
				for (int num72 = 0; num72 < num70; num72++)
				{
					string[] array4 = Res.split(msg.reader().readUTF(), "\n", 0);
					if (b30 == 2)
					{
						GameCanvas.panel.maxPageShop[num72] = msg.reader().readUnsignedByte();
					}
					if (array4.Length == 2)
					{
						GameCanvas.panel.shopTabName[num72] = array4;
					}
					if (array4.Length == 1)
					{
						GameCanvas.panel.shopTabName[num72][0] = array4[0];
						GameCanvas.panel.shopTabName[num72][1] = string.Empty;
					}
					int num73 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[num72] = new Item[num73];
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					if (b30 == 1)
					{
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy2;
					}
					for (int num74 = 0; num74 < num73; num74++)
					{
						short num75 = msg.reader().readShort();
						if (num75 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemShop[num72][num74] = new Item();
						Char.myCharz().arrItemShop[num72][num74].template = ItemTemplates.get(num75);
						Res.outz("name " + num72 + " = " + Char.myCharz().arrItemShop[num72][num74].template.name + " id templat= " + Char.myCharz().arrItemShop[num72][num74].template.id);
						if (b30 == 8)
						{
							Char.myCharz().arrItemShop[num72][num74].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num72][num74].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num72][num74].quantity = msg.reader().readInt();
						}
						else if (b30 == 4)
						{
							Char.myCharz().arrItemShop[num72][num74].reason = msg.reader().readUTF();
						}
						else if (b30 == 0)
						{
							Char.myCharz().arrItemShop[num72][num74].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num72][num74].buyGold = msg.reader().readInt();
						}
						else if (b30 == 1)
						{
							Char.myCharz().arrItemShop[num72][num74].powerRequire = msg.reader().readLong();
						}
						else if (b30 == 2)
						{
							Char.myCharz().arrItemShop[num72][num74].itemId = msg.reader().readShort();
							Char.myCharz().arrItemShop[num72][num74].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num72][num74].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num72][num74].buyType = msg.reader().readByte();
							Char.myCharz().arrItemShop[num72][num74].quantity = msg.reader().readInt();
							Char.myCharz().arrItemShop[num72][num74].isMe = msg.reader().readByte();
						}
						else if (b30 == 3)
						{
							Char.myCharz().arrItemShop[num72][num74].isBuySpec = true;
							Char.myCharz().arrItemShop[num72][num74].iconSpec = msg.reader().readShort();
							Char.myCharz().arrItemShop[num72][num74].buySpec = msg.reader().readInt();
						}
						int num76 = msg.reader().readUnsignedByte();
						if (num76 != 0)
						{
							Char.myCharz().arrItemShop[num72][num74].itemOption = new ItemOption[num76];
							for (int num77 = 0; num77 < Char.myCharz().arrItemShop[num72][num74].itemOption.Length; num77++)
							{
								int num78 = msg.reader().readUnsignedByte();
								int param4 = msg.reader().readUnsignedShort();
								if (num78 != -1)
								{
									Char.myCharz().arrItemShop[num72][num74].itemOption[num77] = new ItemOption(num78, param4);
									Char.myCharz().arrItemShop[num72][num74].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[num72][num74]);
								}
							}
						}
						sbyte b31 = msg.reader().readByte();
						Char.myCharz().arrItemShop[num72][num74].newItem = ((b31 != 0) ? true : false);
						sbyte b32 = msg.reader().readByte();
						if (b32 == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[num72][num74].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
					}
				}
				if (flag7)
				{
					if (b30 != 2)
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
				if (b30 == 2)
				{
					string[][] array5 = GameCanvas.panel.tabName[1];
					if (flag7)
					{
						GameCanvas.panel.tabName[1] = new string[4][]
						{
							array5[0],
							array5[1],
							array5[2],
							array5[3]
						};
					}
					else
					{
						GameCanvas.panel.tabName[1] = new string[5][]
						{
							array5[0],
							array5[1],
							array5[2],
							array5[3],
							array5[4]
						};
					}
				}
				GameCanvas.panel.setTypeShop(b30);
				GameCanvas.panel.show();
				break;
			}
			case -41:
			{
				sbyte b24 = msg.reader().readByte();
				Char.myCharz().strLevel = new string[b24];
				for (int num54 = 0; num54 < b24; num54++)
				{
					string text = msg.reader().readUTF();
					Char.myCharz().strLevel[num54] = text;
				}
				Res.outz("---   xong  level caption cmd : " + msg.command);
				break;
			}
			case -34:
			{
				sbyte b22 = msg.reader().readByte();
				Res.outz("act= " + b22);
				if (b22 == 0 && GameScr.gI().magicTree != null)
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
					sbyte b23 = msg.reader().readByte();
					magicTree.peaPostionX = new int[b23];
					magicTree.peaPostionY = new int[b23];
					for (int num49 = 0; num49 < b23; num49++)
					{
						magicTree.peaPostionX[num49] = msg.reader().readByte();
						magicTree.peaPostionY[num49] = msg.reader().readByte();
					}
					magicTree.isUpdate = msg.reader().readBool();
					magicTree.last = (magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
				}
				if (b22 == 1)
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
					catch (Exception ex8)
					{
						Cout.println("Loi MAGIC_TREE " + ex8.ToString());
					}
					GameCanvas.menu.startAt(myVector, 3);
				}
				if (b22 == 2)
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
				int num21 = msg.reader().readByte();
				sbyte b14 = msg.reader().readByte();
				if (b14 != 0)
				{
					Mob.arrMobTemplate[num21].data.readDataNewBoss(NinjaUtil.readByteArray(msg), b14);
				}
				else
				{
					Mob.arrMobTemplate[num21].data.readData(NinjaUtil.readByteArray(msg));
				}
				for (int num22 = 0; num22 < GameScr.vMob.size(); num22++)
				{
					mob = (Mob)GameScr.vMob.elementAt(num22);
					if (mob.templateId == num21)
					{
						mob.w = Mob.arrMobTemplate[num21].data.width;
						mob.h = Mob.arrMobTemplate[num21].data.height;
					}
				}
				sbyte[] array3 = NinjaUtil.readByteArray(msg);
				Image img = Image.createImage(array3, 0, array3.Length);
				Mob.arrMobTemplate[num21].data.img = img;
				int num23 = msg.reader().readByte();
				Mob.arrMobTemplate[num21].data.typeData = num23;
				if (num23 == 1 || num23 == 2)
				{
					readFrameBoss(msg, num21);
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
				int num162 = msg.reader().readInt();
				sbyte[] array16 = null;
				try
				{
					array16 = NinjaUtil.readByteArray(msg);
					Res.outz("request hinh icon = " + num162);
					if (num162 == 3896)
					{
						Res.outz("SIZE CHECK= " + array16.Length);
					}
					SmallImage.imgNew[num162].img = createImage(array16);
				}
				catch (Exception)
				{
					array16 = null;
					SmallImage.imgNew[num162].img = Image.createRGBImage(new int[1], 1, 1, bl: true);
				}
				if (array16 != null && mGraphics.zoomLevel > 1)
				{
					Rms.saveRMS(mGraphics.zoomLevel + "Small" + num162, array16);
				}
				break;
			}
			case -66:
			{
				short num144 = msg.reader().readShort();
				sbyte[] data4 = NinjaUtil.readByteArray(msg);
				EffectData effDataById = Effect.getEffDataById(num144);
				sbyte b62 = msg.reader().readSByte();
				if (b62 == 0)
				{
					effDataById.readData(data4);
				}
				else
				{
					effDataById.readDataNewBoss(data4, b62);
				}
				sbyte[] array14 = NinjaUtil.readByteArray(msg);
				effDataById.img = Image.createImage(array14, 0, array14.Length);
				Res.outz("err5 ");
				if (num144 != 78)
				{
					break;
				}
				sbyte b63 = msg.reader().readByte();
				short[][] array15 = new short[b63][];
				for (int num145 = 0; num145 < b63; num145++)
				{
					int num146 = msg.reader().readUnsignedByte();
					array15[num145] = new short[num146];
					for (int num147 = 0; num147 < num146; num147++)
					{
						array15[num145][num147] = msg.reader().readShort();
					}
				}
				effDataById.anim_data = array15;
				break;
			}
			case -32:
			{
				short num134 = msg.reader().readShort();
				int num135 = msg.reader().readInt();
				sbyte[] array12 = null;
				Image image = null;
				try
				{
					array12 = new sbyte[num135];
					for (int num136 = 0; num136 < num135; num136++)
					{
						array12[num136] = msg.reader().readByte();
					}
					image = Image.createImage(array12, 0, num135);
					BgItem.imgNew.put(num134 + string.Empty, image);
				}
				catch (Exception)
				{
					array12 = null;
					BgItem.imgNew.put(num134 + string.Empty, Image.createRGBImage(new int[1], 1, 1, bl: true));
				}
				if (array12 != null)
				{
					if (mGraphics.zoomLevel > 1)
					{
						Rms.saveRMS(mGraphics.zoomLevel + "bgItem" + num134, array12);
					}
					BgItemMn.blendcurrBg(num134, image);
				}
				break;
			}
			case 92:
			{
				if (GameCanvas.currentScreen == GameScr.instance)
				{
					GameCanvas.endDlg();
				}
				string text4 = msg.reader().readUTF();
				string str2 = msg.reader().readUTF();
				str2 = Res.changeString(str2);
				string empty = string.Empty;
				Char char9 = null;
				sbyte b42 = 0;
				if (!text4.Equals(string.Empty))
				{
					char9 = new Char();
					char9.charID = msg.reader().readInt();
					char9.head = msg.reader().readShort();
					char9.headICON = msg.reader().readShort();
					char9.body = msg.reader().readShort();
					char9.bag = msg.reader().readShort();
					char9.leg = msg.reader().readShort();
					b42 = msg.reader().readByte();
					char9.cName = text4;
				}
				empty += str2;
				InfoDlg.hide();
				if (text4.Equals(string.Empty))
				{
					GameScr.info1.addInfo(empty, 0);
					break;
				}
				GameScr.info2.addInfoWithChar(empty, char9, (b42 == 0) ? true : false);
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
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isDisable = msg.reader().readBool();
				break;
			}
			case 82:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isDontMove = msg.reader().readBool();
				break;
			}
			case 85:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isFire = msg.reader().readBool();
				break;
			}
			case 86:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isIce = msg.reader().readBool();
				if (!mob8.isIce)
				{
					ServerEffect.addServerEffect(77, mob8.x, mob8.y - 9, 1);
				}
				break;
			}
			case 87:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isWind = msg.reader().readBool();
				break;
			}
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
					int num41 = msg.readInt3Byte();
					Res.outz("dame hit = " + num41);
					if (num41 != 0)
					{
						@char.doInjure();
					}
					int num42 = 0;
					try
					{
						flag4 = msg.reader().readBoolean();
						sbyte b16 = msg.reader().readByte();
						if (b16 != -1)
						{
							Res.outz("hit eff= " + b16);
							EffecMn.addEff(new Effect(b16, @char.cx, @char.cy, 3, 1, -1));
						}
					}
					catch (Exception)
					{
					}
					num41 += num42;
					if (Char.myCharz().cTypePk != 4)
					{
						if (num41 == 0)
						{
							GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS_ME);
						}
						else
						{
							GameScr.startFlyText("-" + num41, @char.cx, @char.cy - @char.ch, 0, -3, flag4 ? mFont.FATAL : mFont.RED);
						}
					}
					break;
				}
				@char = GameScr.findCharInMap(num13);
				if (@char == null)
				{
					return;
				}
				@char.cHP = msg.readInt3Byte();
				bool flag5 = false;
				int num43 = msg.readInt3Byte();
				if (num43 != 0)
				{
					@char.doInjure();
				}
				int num44 = 0;
				try
				{
					flag5 = msg.reader().readBoolean();
					sbyte b17 = msg.reader().readByte();
					if (b17 != -1)
					{
						Res.outz("hit eff= " + b17);
						EffecMn.addEff(new Effect(b17, @char.cx, @char.cy, 3, 1, -1));
					}
				}
				catch (Exception)
				{
				}
				num43 += num44;
				if (@char.cTypePk != 4)
				{
					if (num43 == 0)
					{
						GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num43, @char.cx, @char.cy - @char.ch, 0, -3, flag5 ? mFont.FATAL : mFont.ORANGE);
					}
				}
				break;
			}
			case 83:
			{
				GameCanvas.debug("SXX8", 2);
				int num13 = msg.reader().readInt();
				@char = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz());
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
				int num13 = msg.reader().readInt();
				if (num13 == Char.myCharz().charID)
				{
					@char = Char.myCharz();
				}
				else
				{
					@char = GameScr.findCharInMap(num13);
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
				int num13 = msg.reader().readInt();
				Char char10 = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz());
				char10.moveFast = new short[3];
				char10.moveFast[0] = 0;
				short num160 = msg.reader().readShort();
				short num161 = msg.reader().readShort();
				char10.moveFast[1] = num160;
				char10.moveFast[2] = num161;
				try
				{
					num13 = msg.reader().readInt();
					Char char11 = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz());
					char11.cx = num160;
					char11.cy = num161;
				}
				catch (Exception ex20)
				{
					Cout.println("Loi MOVE_FAST " + ex20.ToString());
				}
				break;
			}
			case 88:
			{
				string info4 = msg.reader().readUTF();
				short num159 = msg.reader().readShort();
				GameCanvas.inputDlg.show(info4, new Command(mResources.ACCEPT, GameCanvas.instance, 88818, num159), TField.INPUT_TYPE_ANY);
				break;
			}
			case 27:
			{
				myVector = new MyVector();
				string text6 = msg.reader().readUTF();
				int num152 = msg.reader().readByte();
				for (int num153 = 0; num153 < num152; num153++)
				{
					string caption4 = msg.reader().readUTF();
					short num154 = msg.reader().readShort();
					myVector.addElement(new Command(caption4, GameCanvas.instance, 88819, num154));
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
				catch (Exception ex19)
				{
					Cout.println("Loi OPEN_UI_MENU " + ex19.ToString());
				}
				if (Char.myCharz().npcFocus == null)
				{
					return;
				}
				for (int num143 = 0; num143 < Char.myCharz().npcFocus.template.menu.Length; num143++)
				{
					string[] array13 = Char.myCharz().npcFocus.template.menu[num143];
					myVector.addElement(new Command(array13[0], GameCanvas.instance, 88820, array13));
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
				string[] array9 = new string[msg.reader().readByte()];
				string[] array10 = new string[array9.Length];
				GameScr.tasks = new int[array9.Length];
				GameScr.mapTasks = new int[array9.Length];
				short[] array11 = new short[array9.Length];
				short count = -1;
				for (int num132 = 0; num132 < array9.Length; num132++)
				{
					string str5 = msg.reader().readUTF();
					str5 = Res.changeString(str5);
					GameScr.tasks[num132] = msg.reader().readByte();
					GameScr.mapTasks[num132] = msg.reader().readShort();
					string str6 = msg.reader().readUTF();
					str6 = Res.changeString(str6);
					array11[num132] = -1;
					if (!str5.Equals(string.Empty))
					{
						array9[num132] = str5;
						array10[num132] = str6;
					}
				}
				try
				{
					count = msg.reader().readShort();
					for (int num133 = 0; num133 < array9.Length; num133++)
					{
						array11[num133] = msg.reader().readShort();
					}
				}
				catch (Exception ex17)
				{
					Cout.println("Loi TASK_GET " + ex17.ToString());
				}
				Char.myCharz().taskMaint = new Task(taskId, index3, str3, str4, array9, array11, count, array10);
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
				for (int num130 = 0; num130 < b53; num130++)
				{
					GameInfo gameInfo = new GameInfo();
					gameInfo.id = msg.reader().readShort();
					gameInfo.main = msg.reader().readUTF();
					gameInfo.content = msg.reader().readUTF();
					Panel.vGameInfo.addElement(gameInfo);
					bool hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1;
					gameInfo.hasRead = hasRead;
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
				try
				{
					short num124 = msg.reader().readShort();
					short num125 = msg.reader().readShort();
					Char.myCharz().x_hint = num124;
					Char.myCharz().y_hint = num125;
					Res.outz("CMD   TASK_UPDATE:43_mapID =    x|y " + num124 + "|" + num125);
					for (int num126 = 0; num126 < TileMap.vGo.size(); num126++)
					{
						Res.outz("===> " + TileMap.vGo.elementAt(num126));
					}
				}
				catch (Exception)
				{
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
				for (int num121 = 0; num121 < GameScr.vItemMap.size(); num121++)
				{
					if (((ItemMap)GameScr.vItemMap.elementAt(num121)).itemMapID == itemMapID)
					{
						GameScr.vItemMap.removeElementAt(num121);
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
				for (int num120 = 0; num120 < GameScr.vItemMap.size(); num120++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(num120);
					if (itemMap2.itemMapID != itemMapID)
					{
						continue;
					}
					itemMap2.setPoint(Char.myCharz().cx, Char.myCharz().cy - 10);
					string text5 = msg.reader().readUTF();
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
					if (text5.Equals(string.Empty))
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
					else if (text5.Length == 1)
					{
						Cout.LogError3("strInf.Length =1:  " + text5);
					}
					else
					{
						GameScr.info1.addInfo(text5, 0);
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
				for (int num119 = 0; num119 < GameScr.vItemMap.size(); num119++)
				{
					ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(num119);
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
				int num118 = msg.reader().readByte();
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), Char.myCharz().arrItemBag[num118].template.id, Char.myCharz().cx, Char.myCharz().cy, msg.reader().readShort(), msg.reader().readShort()));
				Char.myCharz().arrItemBag[num118] = null;
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
				int num117 = msg.reader().readInt();
				short r = 0;
				if (num117 == -2)
				{
					r = msg.reader().readShort();
				}
				ItemMap o = new ItemMap(num117, itemMapID, itemTemplateID, x, y, r);
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
				int avatar2 = msg.reader().readShort();
				string chat3 = msg.reader().readUTF();
				Npc npc6 = new Npc(-1, 0, 0, 0, 0, 0);
				npc6.avatar = avatar2;
				ChatPopup.addBigMessage(chat3, 100000, npc6);
				sbyte b40 = msg.reader().readByte();
				if (b40 == 0)
				{
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 35;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
				}
				if (b40 == 1)
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
				int num79 = msg.reader().readShort();
				Res.outz("OPEN_UI_SAY ID= " + num79);
				string str = msg.reader().readUTF();
				str = Res.changeString(str);
				for (int num105 = 0; num105 < GameScr.vNpc.size(); num105++)
				{
					Npc npc4 = (Npc)GameScr.vNpc.elementAt(num105);
					Res.outz("npc id= " + npc4.template.npcTemplateId);
					if (npc4.template.npcTemplateId == num79)
					{
						ChatPopup.addChatPopupMultiLine(str, 100000, npc4);
						GameCanvas.panel.hideNow();
						return;
					}
				}
				Npc npc5 = new Npc(num79, 0, 0, 0, num79, GameScr.info1.charId[Char.myCharz().cgender][2]);
				if (npc5.template.npcTemplateId == 5)
				{
					npc5.charID = 5;
				}
				try
				{
					npc5.avatar = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				ChatPopup.addChatPopupMultiLine(str, 100000, npc5);
				GameCanvas.panel.hideNow();
				break;
			}
			case 32:
			{
				GameCanvas.debug("SA68", 2);
				int num79 = msg.reader().readShort();
				for (int num80 = 0; num80 < GameScr.vNpc.size(); num80++)
				{
					Npc npc2 = (Npc)GameScr.vNpc.elementAt(num80);
					if (npc2.template.npcTemplateId == num79 && npc2.Equals(Char.myCharz().npcFocus))
					{
						string chat = msg.reader().readUTF();
						string[] array6 = new string[msg.reader().readByte()];
						for (int num81 = 0; num81 < array6.Length; num81++)
						{
							array6[num81] = msg.reader().readUTF();
						}
						GameScr.gI().createMenu(array6, npc2);
						ChatPopup.addChatPopup(chat, 100000, npc2);
						return;
					}
				}
				Npc npc3 = new Npc(num79, 0, -100, 100, num79, GameScr.info1.charId[Char.myCharz().cgender][2]);
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				string chat2 = msg.reader().readUTF();
				string[] array7 = new string[msg.reader().readByte()];
				for (int num82 = 0; num82 < array7.Length; num82++)
				{
					array7[num82] = msg.reader().readUTF();
				}
				try
				{
					short avatar = msg.reader().readShort();
					npc3.avatar = avatar;
				}
				catch (Exception)
				{
				}
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				GameScr.gI().createMenu(array7, npc3);
				ChatPopup.addChatPopup(chat2, 100000, npc3);
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
					sbyte b29 = msg.reader().readByte();
					TileMap.isMapDouble = ((b29 != 0) ? true : false);
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
				short num61 = msg.reader().readShort();
				Cout.LogError2("nItem= " + num61);
				for (int num62 = 0; num62 < num61; num62++)
				{
					BgItem bgItem = new BgItem();
					bgItem.id = num62;
					bgItem.idImage = msg.reader().readShort();
					bgItem.layer = msg.reader().readByte();
					bgItem.dx = msg.reader().readShort();
					bgItem.dy = msg.reader().readShort();
					sbyte b26 = msg.reader().readByte();
					bgItem.tileX = new int[b26];
					bgItem.tileY = new int[b26];
					for (int num63 = 0; num63 < b26; num63++)
					{
						bgItem.tileX[num62] = msg.reader().readByte();
						bgItem.tileY[num62] = msg.reader().readByte();
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
				for (int num40 = 0; num40 < @char.attMobs.Length; num40++)
				{
					Mob mob3 = (Mob)GameScr.vMob.elementAt(msg.reader().readByte());
					@char.attMobs[num40] = mob3;
					if (num40 == 0)
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
						int num13 = msg.reader().readInt();
						Char char5 = (array[num] = ((num13 != Char.myCharz().charID) ? GameScr.findCharInMap(num13) : Char.myCharz()));
						if (num == 0)
						{
							if (@char.cx <= char5.cx)
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
				{
					return;
				}
				int num20 = msg.reader().readUnsignedByte();
				if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
				{
					@char.setSkillPaint(GameScr.sks[num20], 0);
				}
				else
				{
					@char.setSkillPaint(GameScr.sks[num20], 1);
				}
				GameCanvas.debug("SA769991v2", 2);
				Mob[] array2 = new Mob[10];
				num = 0;
				try
				{
					GameCanvas.debug("SA769991v3", 2);
					for (num = 0; num < array2.Length; num++)
					{
						GameCanvas.debug("SA769991v4-num" + num, 2);
						Mob mob2 = (array2[num] = (Mob)GameScr.vMob.elementAt(msg.reader().readByte()));
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
						@char.attMobs[num] = array2[num];
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
				try
				{
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
				}
				catch (Exception)
				{
				}
				break;
			}
			}
			switch (msg.command)
			{
			case -2:
			{
				GameCanvas.debug("SA77", 22);
				int num191 = msg.reader().readInt();
				Char.myCharz().yen += num191;
				GameScr.startFlyText((num191 <= 0) ? (string.Empty + num191) : ("+" + num191), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 95:
			{
				GameCanvas.debug("SA77", 22);
				int num181 = msg.reader().readInt();
				Char.myCharz().xu += num181;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				GameScr.startFlyText((num181 <= 0) ? (string.Empty + num181) : ("+" + num181), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 96:
				GameCanvas.debug("SA77a", 22);
				Char.myCharz().taskOrders.addElement(new TaskOrder(msg.reader().readByte(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readUTF(), msg.reader().readUTF(), msg.reader().readByte(), msg.reader().readByte()));
				break;
			case 97:
			{
				sbyte b71 = msg.reader().readByte();
				for (int num178 = 0; num178 < Char.myCharz().taskOrders.size(); num178++)
				{
					TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(num178);
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
				int num190 = msg.reader().readInt();
				Char.myCharz().xu += num190;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				Char.myCharz().yen -= num190;
				GameScr.startFlyText("+" + num190, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case -3:
			{
				GameCanvas.debug("SA78", 2);
				sbyte b78 = msg.reader().readByte();
				int num196 = msg.reader().readInt();
				if (b78 == 0)
				{
					Char.myCharz().cPower += num196;
				}
				if (b78 == 1)
				{
					Char.myCharz().cTiemNang += num196;
				}
				if (b78 == 2)
				{
					Char.myCharz().cPower += num196;
					Char.myCharz().cTiemNang += num196;
				}
				Char.myCharz().applyCharLevelPercent();
				if (Char.myCharz().cTypePk != 3)
				{
					GameScr.startFlyText(((num196 <= 0) ? string.Empty : "+") + num196, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -4, mFont.GREEN);
					if (num196 > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5002)
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
				for (int num189 = 0; num189 < GameScr.vNpc.size(); num189++)
				{
					Npc npc7 = (Npc)GameScr.vNpc.elementAt(num189);
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
				int num183 = msg.reader().readInt();
				Char char14;
				if (num183 != -100)
				{
					char14 = new Char();
					char14.charID = charID;
					char14.clanID = num183;
				}
				else
				{
					char14 = new Mabu();
					char14.charID = charID;
					char14.clanID = num183;
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
					for (int num184 = 0; num184 < GameScr.vMob.size(); num184++)
					{
						Mob mob10 = (Mob)GameScr.vMob.elementAt(num184);
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
					short num185 = msg.reader().readShort();
					Res.outz("mount id= " + num185 + "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
					if (num185 != -1)
					{
						char14.isHaveMount = true;
						switch (num185)
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
							if (num185 >= Char.ID_NEW_MOUNT)
							{
								char14.idMount = num185;
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
					char14.idHat = msg.reader().readShort();
					if (char14.bag >= 201 && char14.bag < 255)
					{
						Effect effect2 = new Effect(char14.bag, char14, 2, -1, 10, 1);
						effect2.typeEff = 5;
						char14.addEffChar(effect2);
					}
					else
					{
						for (int num186 = 0; num186 < 54; num186++)
						{
							char14.removeEffChar(0, 201 + num186);
						}
					}
				}
				catch (Exception ex31)
				{
					Res.outz("cmd: -5 err: " + ex31.StackTrace);
				}
				GameScr.gI().getFlagImage(char14.charID, char14.cFlag);
				Res.outz("Cmd: -5 PLAYER_ADD: cID| cName| cFlag| cBag|    " + @char.charID + " | " + @char.cName + " | " + @char.cFlag + " | " + @char.bag);
				break;
			}
			case -7:
			{
				GameCanvas.debug("SA80", 2);
				int num179 = msg.reader().readInt();
				Cout.println("RECEVED MOVE OF " + num179);
				for (int num180 = 0; num180 < GameScr.vCharInMap.size(); num180++)
				{
					Char char13 = null;
					try
					{
						char13 = (Char)GameScr.vCharInMap.elementAt(num180);
					}
					catch (Exception ex26)
					{
						Cout.println("Loi PLAYER_MOVE " + ex26.ToString());
					}
					if (char13 == null)
					{
						break;
					}
					if (char13.charID == num179)
					{
						GameCanvas.debug("SA8x2y" + num180, 2);
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
				int num179 = msg.reader().readInt();
				for (int num195 = 0; num195 < GameScr.vCharInMap.size(); num195++)
				{
					Char char15 = (Char)GameScr.vCharInMap.elementAt(num195);
					if (char15 != null && char15.charID == num179)
					{
						if (!char15.isInvisiblez && !char15.isUsePlane)
						{
							ServerEffect.addServerEffect(60, char15.cx, char15.cy, 1);
						}
						if (!char15.isUsePlane)
						{
							GameScr.vCharInMap.removeElementAt(num195);
						}
						return;
					}
				}
				break;
			}
			case -13:
			{
				GameCanvas.debug("SA82", 2);
				int num187 = msg.reader().readUnsignedByte();
				if (num187 > GameScr.vMob.size() - 1 || num187 < 0)
				{
					return;
				}
				Mob mob9 = (Mob)GameScr.vMob.elementAt(num187);
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
				mob9.updateHp_bar();
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
					mob9.updateHp_bar();
					int num182 = msg.readInt3Byte();
					if (num182 == 1)
					{
						return;
					}
					bool flag10 = false;
					try
					{
						flag10 = msg.reader().readBoolean();
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
					if (flag10)
					{
						GameScr.startFlyText("-" + num182, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.FATAL);
					}
					else if (num182 == 0)
					{
						mob9.x = mob9.xFirst;
						mob9.y = mob9.yFirst;
						GameScr.startFlyText(mResources.miss, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num182, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.ORANGE);
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
				catch (Exception ex25)
				{
					Cout.println("Loi tai NPC_MISS  " + ex25.ToString());
				}
				if (mob9 != null)
				{
					mob9.hp = msg.reader().readInt();
					mob9.updateHp_bar();
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
					int num192 = msg.readInt3Byte();
					if (msg.reader().readBool())
					{
						GameScr.startFlyText("-" + num192, mob9.x, mob9.y - mob9.h, 0, -2, mFont.FATAL);
					}
					else
					{
						GameScr.startFlyText("-" + num192, mob9.x, mob9.y - mob9.h, 0, -2, mFont.ORANGE);
					}
					sbyte b77 = msg.reader().readByte();
					for (int num193 = 0; num193 < b77; num193++)
					{
						ItemMap itemMap4 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
						int num194 = (itemMap4.playerId = msg.reader().readInt());
						Res.outz("playerid= " + num194 + " my id= " + Char.myCharz().charID);
						GameScr.vItemMap.addElement(itemMap4);
						if (Res.abs(itemMap4.y - Char.myCharz().cy) < 24 && Res.abs(itemMap4.x - Char.myCharz().cx) < 24)
						{
							Char.myCharz().charFocus = null;
						}
					}
				}
				catch (Exception ex36)
				{
					Cout.println("LOi tai NPC_DIE " + ex36.ToString() + " cmd " + msg.command);
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
				catch (Exception ex23)
				{
					Res.outz("Loi tai NPC_ATTACK_ME " + msg.command + " err= " + ex23.StackTrace);
				}
				if (mob9 != null)
				{
					Char.myCharz().isDie = false;
					Char.isLockKey = false;
					int num175 = msg.readInt3Byte();
					int num176;
					try
					{
						num176 = msg.readInt3Byte();
					}
					catch (Exception)
					{
						num176 = 0;
					}
					if (mob9.isBusyAttackSomeOne)
					{
						Char.myCharz().doInjure(num175, num176, isCrit: false, isMob: true);
						break;
					}
					mob9.dame = num175;
					mob9.dameMp = num176;
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
					int num188 = msg.readInt3Byte();
					mob9.dame = @char.cHP - num188;
					@char.cHPNew = num188;
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
				int num177 = msg.reader().readInt();
				string text8 = msg.reader().readUTF();
				Res.outz("user id= " + num177 + " text= " + text8);
				@char = ((Char.myCharz().charID != num177) ? GameScr.findCharInMap(num177) : Char.myCharz());
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
				for (int num174 = 0; num174 < b70; num174++)
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
		catch (Exception ex37)
		{
			Res.outz("Controller = " + ex37.StackTrace);
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
			TileMap.vCurrItem.removeAllElements();
			if (mGraphics.zoomLevel == 1)
			{
				BgItem.clearHashTable();
			}
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
			Res.err("LOI TAI LOADMAP INFO " + ex.StackTrace);
		}
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
			if (Rms.loadRMSInt("AdminLink") == 1)
			{
				return;
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
				sbyte b3 = msg.reader().readByte();
				Rms.saveRMSInt("AdminLink", b3);
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
					Char.myCharz().idHat = msg.reader().readShort();
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
				if (@char == null)
				{
					break;
				}
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
					@char.idHat = msg.reader().readShort();
					if (@char.bag >= 201)
					{
						Effect effect = new Effect(@char.bag, @char, 2, -1, 10, 1);
						effect.typeEff = 5;
						@char.addEffChar(effect);
					}
					else
					{
						@char.removeEffChar(0, 201);
					}
					break;
				}
				catch (Exception)
				{
					break;
				}
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
			sbyte b = msg.reader().readByte();
			Res.outz("<<<readGetImgByName = " + text + "  " + b);
			sbyte[] array = null;
			array = NinjaUtil.readByteArray(msg);
			Image img = createImage(array);
			ImgByName.SetImage(text, img, b);
			if (array != null)
			{
				ImgByName.saveRMS(text, b, array);
			}
		}
		catch (Exception ex)
		{
			Res.outz("<<<readGetImgByName ex = " + ex.StackTrace);
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

	public void read_opt(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
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
