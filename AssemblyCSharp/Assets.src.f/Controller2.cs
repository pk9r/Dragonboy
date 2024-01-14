using System;
using Assets.src.g;

namespace Assets.src.f
{
	internal class Controller2
	{
		public static void readMessage(Message msg)
		{
			try
			{
				sbyte command = msg.command;
				switch (command)
				{
				case sbyte.MinValue:
					readInfoEffChar(msg);
					return;
				case -89:
					GameCanvas.open3Hour = msg.reader().readByte() == 1;
					return;
				case -127:
					readLuckyRound(msg);
					return;
				case -126:
				{
					sbyte b16 = msg.reader().readByte();
					Res.outz("type quay= " + b16);
					if (b16 == 1)
					{
						sbyte b17 = msg.reader().readByte();
						string num24 = msg.reader().readUTF();
						string finish = msg.reader().readUTF();
						GameScr.gI().showWinNumber(num24, finish);
					}
					if (b16 == 0)
						GameScr.gI().showYourNumber(msg.reader().readUTF());
					return;
				}
				case -122:
				{
					Npc npc = GameScr.findNPCInMap(msg.reader().readShort());
					sbyte b4 = msg.reader().readByte();
					npc.duahau = new int[b4];
					Res.outz("N DUA HAU= " + b4);
					for (int k = 0; k < b4; k++)
					{
						npc.duahau[k] = msg.reader().readShort();
					}
					npc.setStatus(msg.reader().readByte(), msg.reader().readInt());
					return;
				}
				case -120:
					Service.logController = mSystem.currentTimeMillis() - Service.curCheckController;
					Service.gI().sendCheckController();
					return;
				case -121:
					Service.logMap = mSystem.currentTimeMillis() - Service.curCheckMap;
					Service.gI().sendCheckMap();
					return;
				case -123:
				{
					int charId = msg.reader().readInt();
					if (GameScr.findCharInMap(charId) != null)
						GameScr.findCharInMap(charId).perCentMp = msg.reader().readByte();
					return;
				}
				case -119:
					Char.myCharz().rank = msg.reader().readInt();
					return;
				case -117:
					GameScr.gI().tMabuEff = 0;
					GameScr.gI().percentMabu = msg.reader().readByte();
					if (GameScr.gI().percentMabu == 100)
						GameScr.gI().mabuEff = true;
					if (GameScr.gI().percentMabu == 101)
						Npc.mabuEff = true;
					return;
				case -116:
					GameScr.canAutoPlay = msg.reader().readByte() == 1;
					return;
				case -115:
					Char.myCharz().setPowerInfo(msg.reader().readUTF(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
					return;
				case -113:
				{
					sbyte[] array2 = new sbyte[10];
					for (int num22 = 0; num22 < 10; num22++)
					{
						array2[num22] = msg.reader().readByte();
						Res.outz("vlue i= " + array2[num22]);
					}
					GameScr.gI().onKSkill(array2);
					GameScr.gI().onOSkill(array2);
					GameScr.gI().onCSkill(array2);
					return;
				}
				case -111:
				{
					short num14 = msg.reader().readShort();
					ImageSource.vSource = new MyVector();
					for (int num15 = 0; num15 < num14; num15++)
					{
						string iD = msg.reader().readUTF();
						sbyte version = msg.reader().readByte();
						ImageSource.vSource.addElement(new ImageSource(iD, version));
					}
					ImageSource.checkRMS();
					ImageSource.saveRMS();
					return;
				}
				case -124:
				{
					sbyte b11 = msg.reader().readByte();
					sbyte b12 = msg.reader().readByte();
					if (b12 == 0)
					{
						if (b11 == 2)
						{
							int num16 = msg.reader().readInt();
							if (num16 == Char.myCharz().charID)
								Char.myCharz().removeEffect();
							else if (GameScr.findCharInMap(num16) != null)
							{
								GameScr.findCharInMap(num16).removeEffect();
							}
						}
						int num17 = msg.reader().readUnsignedByte();
						int num18 = msg.reader().readInt();
						if (num17 == 32)
						{
							if (b11 == 1)
							{
								int num19 = msg.reader().readInt();
								if (num18 == Char.myCharz().charID)
								{
									Char.myCharz().holdEffID = num17;
									GameScr.findCharInMap(num19).setHoldChar(Char.myCharz());
								}
								else if (GameScr.findCharInMap(num18) != null && num19 != Char.myCharz().charID)
								{
									GameScr.findCharInMap(num18).holdEffID = num17;
									GameScr.findCharInMap(num19).setHoldChar(GameScr.findCharInMap(num18));
								}
								else if (GameScr.findCharInMap(num18) != null && num19 == Char.myCharz().charID)
								{
									GameScr.findCharInMap(num18).holdEffID = num17;
									Char.myCharz().setHoldChar(GameScr.findCharInMap(num18));
								}
							}
							else if (num18 == Char.myCharz().charID)
							{
								Char.myCharz().removeHoleEff();
							}
							else if (GameScr.findCharInMap(num18) != null)
							{
								GameScr.findCharInMap(num18).removeHoleEff();
							}
						}
						if (num17 == 33)
						{
							if (b11 == 1)
							{
								if (num18 == Char.myCharz().charID)
									Char.myCharz().protectEff = true;
								else if (GameScr.findCharInMap(num18) != null)
								{
									GameScr.findCharInMap(num18).protectEff = true;
								}
							}
							else if (num18 == Char.myCharz().charID)
							{
								Char.myCharz().removeProtectEff();
							}
							else if (GameScr.findCharInMap(num18) != null)
							{
								GameScr.findCharInMap(num18).removeProtectEff();
							}
						}
						if (num17 == 39)
						{
							if (b11 == 1)
							{
								if (num18 == Char.myCharz().charID)
									Char.myCharz().huytSao = true;
								else if (GameScr.findCharInMap(num18) != null)
								{
									GameScr.findCharInMap(num18).huytSao = true;
								}
							}
							else if (num18 == Char.myCharz().charID)
							{
								Char.myCharz().removeHuytSao();
							}
							else if (GameScr.findCharInMap(num18) != null)
							{
								GameScr.findCharInMap(num18).removeHuytSao();
							}
						}
						if (num17 == 40)
						{
							if (b11 == 1)
							{
								if (num18 == Char.myCharz().charID)
									Char.myCharz().blindEff = true;
								else if (GameScr.findCharInMap(num18) != null)
								{
									GameScr.findCharInMap(num18).blindEff = true;
								}
							}
							else if (num18 == Char.myCharz().charID)
							{
								Char.myCharz().removeBlindEff();
							}
							else if (GameScr.findCharInMap(num18) != null)
							{
								GameScr.findCharInMap(num18).removeBlindEff();
							}
						}
						if (num17 == 41)
						{
							if (b11 == 1)
							{
								if (num18 == Char.myCharz().charID)
									Char.myCharz().sleepEff = true;
								else if (GameScr.findCharInMap(num18) != null)
								{
									GameScr.findCharInMap(num18).sleepEff = true;
								}
							}
							else if (num18 == Char.myCharz().charID)
							{
								Char.myCharz().removeSleepEff();
							}
							else if (GameScr.findCharInMap(num18) != null)
							{
								GameScr.findCharInMap(num18).removeSleepEff();
							}
						}
						if (num17 == 42)
						{
							if (b11 == 1)
							{
								if (num18 == Char.myCharz().charID)
									Char.myCharz().stone = true;
							}
							else if (num18 == Char.myCharz().charID)
							{
								Char.myCharz().stone = false;
							}
						}
					}
					if (b12 != 1)
						return;
					int num20 = msg.reader().readUnsignedByte();
					sbyte b13 = msg.reader().readByte();
					Res.outz("modbHoldID= " + b13 + " skillID= " + num20 + "eff ID= " + b11);
					if (num20 == 32)
					{
						if (b11 == 1)
						{
							int num21 = msg.reader().readInt();
							if (num21 == Char.myCharz().charID)
							{
								GameScr.findMobInMap(b13).holdEffID = num20;
								Char.myCharz().setHoldMob(GameScr.findMobInMap(b13));
							}
							else if (GameScr.findCharInMap(num21) != null)
							{
								GameScr.findMobInMap(b13).holdEffID = num20;
								GameScr.findCharInMap(num21).setHoldMob(GameScr.findMobInMap(b13));
							}
						}
						else
							GameScr.findMobInMap(b13).removeHoldEff();
					}
					if (num20 == 40)
					{
						if (b11 == 1)
							GameScr.findMobInMap(b13).blindEff = true;
						else
							GameScr.findMobInMap(b13).removeBlindEff();
					}
					if (num20 == 41)
					{
						if (b11 == 1)
							GameScr.findMobInMap(b13).sleepEff = true;
						else
							GameScr.findMobInMap(b13).removeSleepEff();
					}
					return;
				}
				case -125:
				{
					ChatTextField.gI().isShow = false;
					string text2 = msg.reader().readUTF();
					Res.outz("titile= " + text2);
					sbyte b14 = msg.reader().readByte();
					ClientInput.gI().setInput(b14, text2);
					for (int num23 = 0; num23 < b14; num23++)
					{
						ClientInput.gI().tf[num23].name = msg.reader().readUTF();
						sbyte b15 = msg.reader().readByte();
						if (b15 == 0)
							ClientInput.gI().tf[num23].setIputType(TField.INPUT_TYPE_NUMERIC);
						if (b15 == 1)
							ClientInput.gI().tf[num23].setIputType(TField.INPUT_TYPE_ANY);
						if (b15 == 2)
							ClientInput.gI().tf[num23].setIputType(TField.INPUT_TYPE_PASSWORD);
					}
					return;
				}
				case -110:
				{
					sbyte b10 = msg.reader().readByte();
					if (b10 == 1)
					{
						int num11 = msg.reader().readInt();
						sbyte[] array = Rms.loadRMS(num11 + string.Empty);
						if (array == null)
							Service.gI().sendServerData(1, -1, null);
						else
							Service.gI().sendServerData(1, num11, array);
					}
					if (b10 == 0)
					{
						int num12 = msg.reader().readInt();
						short num13 = msg.reader().readShort();
						sbyte[] data = new sbyte[num13];
						msg.reader().read(ref data, 0, num13);
						Rms.saveRMS(num12 + string.Empty, data);
					}
					return;
				}
				case -106:
				{
					short num4 = msg.reader().readShort();
					int num5 = msg.reader().readShort();
					if (ItemTime.isExistItem(num4))
					{
						ItemTime.getItemById(num4).initTime(num5);
						return;
					}
					ItemTime o = new ItemTime(num4, num5);
					Char.vItemTime.addElement(o);
					return;
				}
				case -105:
					TransportScr.gI().time = 0;
					TransportScr.gI().maxTime = msg.reader().readShort();
					TransportScr.gI().last = (TransportScr.gI().curr = mSystem.currentTimeMillis());
					TransportScr.gI().type = msg.reader().readByte();
					TransportScr.gI().switchToMe();
					return;
				case -103:
				{
					sbyte b5 = msg.reader().readByte();
					if (b5 == 0)
					{
						GameCanvas.panel.vFlag.removeAllElements();
						sbyte b6 = msg.reader().readByte();
						for (int l = 0; l < b6; l++)
						{
							Item item = new Item();
							short num6 = msg.reader().readShort();
							if (num6 != -1)
							{
								item.template = ItemTemplates.get(num6);
								sbyte b7 = msg.reader().readByte();
								if (b7 != -1)
								{
									item.itemOption = new ItemOption[b7];
									for (int m = 0; m < item.itemOption.Length; m++)
									{
										int num7 = msg.reader().readUnsignedByte();
										int param2 = msg.reader().readUnsignedShort();
										if (num7 != -1)
											item.itemOption[m] = new ItemOption(num7, param2);
									}
								}
							}
							GameCanvas.panel.vFlag.addElement(item);
						}
						GameCanvas.panel.setTypeFlag();
						GameCanvas.panel.show();
					}
					else if (b5 == 1)
					{
						int num8 = msg.reader().readInt();
						sbyte b8 = msg.reader().readByte();
						Res.outz("---------------actionFlag1:  " + num8 + " : " + b8);
						if (num8 == Char.myCharz().charID)
							Char.myCharz().cFlag = b8;
						else if (GameScr.findCharInMap(num8) != null)
						{
							GameScr.findCharInMap(num8).cFlag = b8;
						}
						GameScr.gI().getFlagImage(num8, b8);
					}
					else
					{
						if (b5 != 2)
							return;
						sbyte b9 = msg.reader().readByte();
						int num9 = msg.reader().readShort();
						PKFlag pKFlag = new PKFlag();
						pKFlag.cflag = b9;
						pKFlag.IDimageFlag = num9;
						GameScr.vFlag.addElement(pKFlag);
						for (int n = 0; n < GameScr.vFlag.size(); n++)
						{
							PKFlag pKFlag2 = (PKFlag)GameScr.vFlag.elementAt(n);
							Res.outz("i: " + n + "  cflag: " + pKFlag2.cflag + "   IDimageFlag: " + pKFlag2.IDimageFlag);
						}
						for (int num10 = 0; num10 < GameScr.vCharInMap.size(); num10++)
						{
							Char @char = (Char)GameScr.vCharInMap.elementAt(num10);
							if (@char != null && @char.cFlag == b9)
								@char.flagImage = num9;
						}
						if (Char.myCharz().cFlag == b9)
							Char.myCharz().flagImage = num9;
					}
					return;
				}
				case -102:
				{
					sbyte b3 = msg.reader().readByte();
					if (b3 != 0 && b3 == 1)
					{
						GameCanvas.loginScr.isLogin2 = false;
						Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
						LoginScr.isLoggingIn = true;
					}
					return;
				}
				case -101:
				{
					GameCanvas.loginScr.isLogin2 = true;
					GameCanvas.connect();
					string text = msg.reader().readUTF();
					Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, text);
					Service.gI().setClientType();
					Service.gI().login(text, string.Empty, GameMidlet.VERSION, 1);
					return;
				}
				case -100:
				{
					InfoDlg.hide();
					bool flag = false;
					if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
						flag = true;
					sbyte b = msg.reader().readByte();
					Res.outz("t Indxe= " + b);
					GameCanvas.panel.maxPageShop[b] = msg.reader().readByte();
					GameCanvas.panel.currPageShop[b] = msg.reader().readByte();
					Res.outz("max page= " + GameCanvas.panel.maxPageShop[b] + " curr page= " + GameCanvas.panel.currPageShop[b]);
					int num = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[b] = new Item[num];
					for (int i = 0; i < num; i++)
					{
						short num2 = msg.reader().readShort();
						if (num2 == -1)
							continue;
						Res.outz("template id= " + num2);
						Char.myCharz().arrItemShop[b][i] = new Item();
						Char.myCharz().arrItemShop[b][i].template = ItemTemplates.get(num2);
						Char.myCharz().arrItemShop[b][i].itemId = msg.reader().readShort();
						Char.myCharz().arrItemShop[b][i].buyCoin = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].buyGold = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].buyType = msg.reader().readByte();
						Char.myCharz().arrItemShop[b][i].quantity = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].isMe = msg.reader().readByte();
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
						sbyte b2 = msg.reader().readByte();
						if (b2 != -1)
						{
							Char.myCharz().arrItemShop[b][i].itemOption = new ItemOption[b2];
							for (int j = 0; j < Char.myCharz().arrItemShop[b][i].itemOption.Length; j++)
							{
								int num3 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								if (num3 != -1)
								{
									Char.myCharz().arrItemShop[b][i].itemOption[j] = new ItemOption(num3, param);
									Char.myCharz().arrItemShop[b][i].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[b][i]);
								}
							}
						}
						if (msg.reader().readByte() == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[b][i].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
						if (GameMidlet.intVERSION >= 237)
						{
							Char.myCharz().arrItemShop[b][i].nameNguoiKyGui = msg.reader().readUTF();
							Res.err("nguoi ki gui  " + Char.myCharz().arrItemShop[b][i].nameNguoiKyGui);
						}
					}
					if (flag)
						GameCanvas.panel2.setTabKiGui();
					GameCanvas.panel.setTabShop();
					GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
					return;
				}
				}
				switch (command)
				{
				case sbyte.MaxValue:
					readInfoRada(msg);
					return;
				case 125:
				{
					sbyte fusion = msg.reader().readByte();
					int num27 = msg.reader().readInt();
					if (num27 == Char.myCharz().charID)
						Char.myCharz().setFusion(fusion);
					else if (GameScr.findCharInMap(num27) != null)
					{
						GameScr.findCharInMap(num27).setFusion(fusion);
					}
					return;
				}
				case 124:
				{
					short num28 = msg.reader().readShort();
					string text3 = msg.reader().readUTF();
					Res.outz("noi chuyen = " + text3 + "npc ID= " + num28);
					GameScr.findNPCInMap(num28)?.addInfo(text3);
					return;
				}
				case 123:
				{
					Res.outz("SET POSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss");
					int num26 = msg.reader().readInt();
					short xPos = msg.reader().readShort();
					short yPos = msg.reader().readShort();
					sbyte b18 = msg.reader().readByte();
					Char char2 = null;
					if (num26 == Char.myCharz().charID)
						char2 = Char.myCharz();
					else if (GameScr.findCharInMap(num26) != null)
					{
						char2 = GameScr.findCharInMap(num26);
					}
					if (char2 != null)
					{
						ServerEffect.addServerEffect((b18 != 0) ? 173 : 60, char2, 1);
						char2.setPos(xPos, yPos, b18);
					}
					return;
				}
				case 122:
				{
					short num25 = msg.reader().readShort();
					Res.outz("second login = " + num25);
					LoginScr.timeLogin = num25;
					LoginScr.currTimeLogin = (LoginScr.lastTimeLogin = mSystem.currentTimeMillis());
					GameCanvas.endDlg();
					return;
				}
				case 121:
					mSystem.publicID = msg.reader().readUTF();
					mSystem.strAdmob = msg.reader().readUTF();
					Res.outz("SHOW AD public ID= " + mSystem.publicID);
					mSystem.createAdmob();
					return;
				}
				switch (command)
				{
				case 48:
					ServerListScreen.ipSelect = msg.reader().readByte();
					GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
					Session_ME.gI().close();
					GameCanvas.endDlg();
					ServerListScreen.waitToLogin = true;
					return;
				case 52:
				{
					sbyte b20 = msg.reader().readByte();
					if (b20 == 1)
					{
						int num31 = msg.reader().readInt();
						if (num31 == Char.myCharz().charID)
						{
							Char.myCharz().setMabuHold(true);
							Char.myCharz().cx = msg.reader().readShort();
							Char.myCharz().cy = msg.reader().readShort();
						}
						else
						{
							Char char3 = GameScr.findCharInMap(num31);
							if (char3 != null)
							{
								char3.setMabuHold(true);
								char3.cx = msg.reader().readShort();
								char3.cy = msg.reader().readShort();
							}
						}
					}
					if (b20 == 0)
					{
						int num32 = msg.reader().readInt();
						if (num32 == Char.myCharz().charID)
							Char.myCharz().setMabuHold(false);
						else
							GameScr.findCharInMap(num32)?.setMabuHold(false);
					}
					if (b20 == 2)
					{
						int charId2 = msg.reader().readInt();
						int id2 = msg.reader().readInt();
						((Mabu)GameScr.findCharInMap(charId2)).eat(id2);
					}
					if (b20 == 3)
						GameScr.mabuPercent = msg.reader().readByte();
					return;
				}
				case 51:
				{
					Mabu mabu = (Mabu)GameScr.findCharInMap(msg.reader().readInt());
					sbyte id = msg.reader().readByte();
					short x = msg.reader().readShort();
					short y = msg.reader().readShort();
					sbyte b19 = msg.reader().readByte();
					Char[] array3 = new Char[b19];
					int[] array4 = new int[b19];
					for (int num29 = 0; num29 < b19; num29++)
					{
						int num30 = msg.reader().readInt();
						Res.outz("char ID=" + num30);
						array3[num29] = null;
						if (num30 != Char.myCharz().charID)
							array3[num29] = GameScr.findCharInMap(num30);
						else
							array3[num29] = Char.myCharz();
						array4[num29] = msg.reader().readInt();
					}
					mabu.setSkill(id, x, y, array3, array4);
					return;
				}
				}
				switch (command)
				{
				case 102:
				{
					sbyte b26 = msg.reader().readByte();
					if (b26 == 0 || b26 == 1 || b26 == 2 || b26 == 6)
					{
						BigBoss2 bigBoss2 = Mob.getBigBoss2();
						if (bigBoss2 == null)
							return;
						if (b26 == 6)
						{
							bigBoss2.x = (bigBoss2.y = (bigBoss2.xTo = (bigBoss2.yTo = (bigBoss2.xFirst = (bigBoss2.yFirst = -1000)))));
							return;
						}
						sbyte b27 = msg.reader().readByte();
						Char[] array7 = new Char[b27];
						int[] array8 = new int[b27];
						for (int num38 = 0; num38 < b27; num38++)
						{
							int num39 = msg.reader().readInt();
							array7[num38] = null;
							if (num39 != Char.myCharz().charID)
								array7[num38] = GameScr.findCharInMap(num39);
							else
								array7[num38] = Char.myCharz();
							array8[num38] = msg.reader().readInt();
						}
						bigBoss2.setAttack(array7, array8, b26);
					}
					if (b26 == 3 || b26 == 4 || b26 == 5 || b26 == 7)
					{
						BachTuoc bachTuoc = Mob.getBachTuoc();
						if (bachTuoc == null)
							return;
						if (b26 == 7)
						{
							bachTuoc.x = (bachTuoc.y = (bachTuoc.xTo = (bachTuoc.yTo = (bachTuoc.xFirst = (bachTuoc.yFirst = -1000)))));
							return;
						}
						if (b26 == 3 || b26 == 4)
						{
							sbyte b28 = msg.reader().readByte();
							Char[] array9 = new Char[b28];
							int[] array10 = new int[b28];
							for (int num40 = 0; num40 < b28; num40++)
							{
								int num41 = msg.reader().readInt();
								array9[num40] = null;
								if (num41 != Char.myCharz().charID)
									array9[num40] = GameScr.findCharInMap(num41);
								else
									array9[num40] = Char.myCharz();
								array10[num40] = msg.reader().readInt();
							}
							bachTuoc.setAttack(array9, array10, b26);
						}
						if (b26 == 5)
							bachTuoc.move(msg.reader().readShort());
					}
					if (b26 > 9 && b26 < 30)
						readActionBoss(msg, b26);
					return;
				}
				case 101:
				{
					Res.outz("big boss--------------------------------------------------");
					BigBoss bigBoss = Mob.getBigBoss();
					if (bigBoss == null)
						return;
					sbyte b24 = msg.reader().readByte();
					if (b24 == 0 || b24 == 1 || b24 == 2 || b24 == 4 || b24 == 3)
					{
						if (b24 == 3)
						{
							bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
							bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
							bigBoss.setFly();
						}
						else
						{
							sbyte b25 = msg.reader().readByte();
							Res.outz("CHUONG nChar= " + b25);
							Char[] array5 = new Char[b25];
							int[] array6 = new int[b25];
							for (int num36 = 0; num36 < b25; num36++)
							{
								int num37 = msg.reader().readInt();
								Res.outz("char ID=" + num37);
								array5[num36] = null;
								if (num37 != Char.myCharz().charID)
									array5[num36] = GameScr.findCharInMap(num37);
								else
									array5[num36] = Char.myCharz();
								array6[num36] = msg.reader().readInt();
							}
							bigBoss.setAttack(array5, array6, b24);
						}
					}
					if (b24 == 5)
					{
						bigBoss.haftBody = true;
						bigBoss.status = 2;
					}
					if (b24 == 6)
					{
						bigBoss.getDataB2();
						bigBoss.x = msg.reader().readShort();
						bigBoss.y = msg.reader().readShort();
					}
					if (b24 == 7)
						bigBoss.setAttack(null, null, b24);
					if (b24 == 8)
					{
						bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
						bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
						bigBoss.status = 2;
					}
					if (b24 == 9)
						bigBoss.x = (bigBoss.y = (bigBoss.xTo = (bigBoss.yTo = (bigBoss.xFirst = (bigBoss.yFirst = -1000)))));
					return;
				}
				case 100:
				{
					sbyte b21 = msg.reader().readByte();
					sbyte b22 = msg.reader().readByte();
					Item item2 = null;
					if (b21 == 0)
						item2 = Char.myCharz().arrItemBody[b22];
					if (b21 == 1)
						item2 = Char.myCharz().arrItemBag[b22];
					short num33 = msg.reader().readShort();
					if (num33 == -1)
						return;
					item2.template = ItemTemplates.get(num33);
					item2.quantity = msg.reader().readInt();
					item2.info = msg.reader().readUTF();
					item2.content = msg.reader().readUTF();
					sbyte b23 = msg.reader().readByte();
					if (b23 != 0)
					{
						item2.itemOption = new ItemOption[b23];
						for (int num34 = 0; num34 < item2.itemOption.Length; num34++)
						{
							int num35 = msg.reader().readUnsignedByte();
							Res.outz("id o= " + num35);
							int param3 = msg.reader().readUnsignedShort();
							if (num35 != -1)
								item2.itemOption[num34] = new ItemOption(num35, param3);
						}
					}
					if (item2.quantity <= 0)
						item2 = null;
					return;
				}
				}
				if (command != 113)
				{
					if (command == 114)
						try
						{
							string text4 = msg.reader().readUTF();
							mSystem.curINAPP = msg.reader().readByte();
							mSystem.maxINAPP = msg.reader().readByte();
							return;
						}
						catch (Exception)
						{
							return;
						}
					if (command != 31)
					{
						if (command != 42)
						{
							if (command == 93)
							{
								string chatVip = Res.changeString(msg.reader().readUTF());
								GameScr.gI().chatVip(chatVip);
							}
							return;
						}
						GameCanvas.endDlg();
						LoginScr.isContinueToLogin = false;
						Char.isLoadingMap = false;
						sbyte haveName = msg.reader().readByte();
						if (GameCanvas.registerScr == null)
							GameCanvas.registerScr = new RegisterScreen(haveName);
						GameCanvas.registerScr.switchToMe();
						return;
					}
					int num42 = msg.reader().readInt();
					if (msg.reader().readByte() == 1)
					{
						short smallID = msg.reader().readShort();
						sbyte b29 = -1;
						int[] array11 = null;
						short wimg = 0;
						short himg = 0;
						try
						{
							b29 = msg.reader().readByte();
							if (b29 > 0)
							{
								sbyte b30 = msg.reader().readByte();
								array11 = new int[b30];
								for (int num43 = 0; num43 < b30; num43++)
								{
									array11[num43] = msg.reader().readByte();
								}
								wimg = msg.reader().readShort();
								himg = msg.reader().readShort();
							}
						}
						catch (Exception)
						{
						}
						if (num42 == Char.myCharz().charID)
						{
							Char.myCharz().petFollow = new PetFollow();
							Char.myCharz().petFollow.smallID = smallID;
							if (b29 > 0)
								Char.myCharz().petFollow.SetImg(b29, array11, wimg, himg);
							return;
						}
						Char char4 = GameScr.findCharInMap(num42);
						char4.petFollow = new PetFollow();
						char4.petFollow.smallID = smallID;
						if (b29 > 0)
							char4.petFollow.SetImg(b29, array11, wimg, himg);
					}
					else if (num42 == Char.myCharz().charID)
					{
						Char.myCharz().petFollow.remove();
						Char.myCharz().petFollow = null;
					}
					else
					{
						Char char5 = GameScr.findCharInMap(num42);
						char5.petFollow.remove();
						char5.petFollow = null;
					}
				}
				else
				{
					int loop = 0;
					int layer = 0;
					int id3 = 0;
					short x2 = 0;
					short y2 = 0;
					short loopCount = -1;
					try
					{
						loop = msg.reader().readByte();
						layer = msg.reader().readByte();
						id3 = msg.reader().readUnsignedByte();
						x2 = msg.reader().readShort();
						y2 = msg.reader().readShort();
						loopCount = msg.reader().readShort();
					}
					catch (Exception)
					{
					}
					EffecMn.addEff(new Effect(id3, x2, y2, layer, loop, loopCount));
				}
			}
			catch (Exception ex4)
			{
				Res.outz("=====> Controller2 " + ex4.StackTrace);
			}
		}

		private static void readLuckyRound(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				if (b == 0)
				{
					sbyte b2 = msg.reader().readByte();
					short[] array = new short[b2];
					for (int i = 0; i < b2; i++)
					{
						array[i] = msg.reader().readShort();
					}
					sbyte b3 = msg.reader().readByte();
					int price = msg.reader().readInt();
					short idTicket = msg.reader().readShort();
					CrackBallScr.gI().SetCrackBallScr(array, (byte)b3, price, idTicket);
				}
				else if (b == 1)
				{
					sbyte b4 = msg.reader().readByte();
					short[] array2 = new short[b4];
					for (int j = 0; j < b4; j++)
					{
						array2[j] = msg.reader().readShort();
					}
					CrackBallScr.gI().DoneCrackBallScr(array2);
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readInfoRada(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				if (b == 0)
				{
					RadarScr.gI();
					MyVector myVector = new MyVector(string.Empty);
					short num = msg.reader().readShort();
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						Info_RadaScr info_RadaScr = new Info_RadaScr();
						int id = msg.reader().readShort();
						int no = i + 1;
						int idIcon = msg.reader().readShort();
						sbyte rank = msg.reader().readByte();
						sbyte amount = msg.reader().readByte();
						sbyte max_amount = msg.reader().readByte();
						short templateId = -1;
						Char charInfo = null;
						sbyte b2 = msg.reader().readByte();
						if (b2 == 0)
							templateId = msg.reader().readShort();
						else
							charInfo = Info_RadaScr.SetCharInfo(msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
						string name = msg.reader().readUTF();
						string info = msg.reader().readUTF();
						sbyte b3 = msg.reader().readByte();
						sbyte use = msg.reader().readByte();
						sbyte b4 = msg.reader().readByte();
						ItemOption[] array = null;
						if (b4 != 0)
						{
							array = new ItemOption[b4];
							for (int j = 0; j < array.Length; j++)
							{
								int num3 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								sbyte activeCard = msg.reader().readByte();
								if (num3 != -1)
								{
									array[j] = new ItemOption(num3, param);
									array[j].activeCard = activeCard;
								}
							}
						}
						info_RadaScr.SetInfo(id, no, idIcon, rank, b2, templateId, name, info, charInfo, array);
						info_RadaScr.SetLevel(b3);
						info_RadaScr.SetUse(use);
						info_RadaScr.SetAmount(amount, max_amount);
						myVector.addElement(info_RadaScr);
						if (b3 > 0)
							num2++;
					}
					RadarScr.gI().SetRadarScr(myVector, num2, num);
					RadarScr.gI().switchToMe();
				}
				else if (b == 1)
				{
					int id2 = msg.reader().readShort();
					sbyte use2 = msg.reader().readByte();
					if (Info_RadaScr.GetInfo(RadarScr.list, id2) != null)
						Info_RadaScr.GetInfo(RadarScr.list, id2).SetUse(use2);
					RadarScr.SetListUse();
				}
				else if (b == 2)
				{
					int num4 = msg.reader().readShort();
					sbyte level = msg.reader().readByte();
					int num5 = 0;
					for (int k = 0; k < RadarScr.list.size(); k++)
					{
						Info_RadaScr info_RadaScr2 = (Info_RadaScr)RadarScr.list.elementAt(k);
						if (info_RadaScr2 != null)
						{
							if (info_RadaScr2.id == num4)
								info_RadaScr2.SetLevel(level);
							if (info_RadaScr2.level > 0)
								num5++;
						}
					}
					RadarScr.SetNum(num5, RadarScr.list.size());
					if (Info_RadaScr.GetInfo(RadarScr.listUse, num4) != null)
						Info_RadaScr.GetInfo(RadarScr.listUse, num4).SetLevel(level);
				}
				else if (b == 3)
				{
					int id3 = msg.reader().readShort();
					sbyte amount2 = msg.reader().readByte();
					sbyte max_amount2 = msg.reader().readByte();
					if (Info_RadaScr.GetInfo(RadarScr.list, id3) != null)
						Info_RadaScr.GetInfo(RadarScr.list, id3).SetAmount(amount2, max_amount2);
					if (Info_RadaScr.GetInfo(RadarScr.listUse, id3) != null)
						Info_RadaScr.GetInfo(RadarScr.listUse, id3).SetAmount(amount2, max_amount2);
				}
				else if (b == 4)
				{
					int num6 = msg.reader().readInt();
					short idAuraEff = msg.reader().readShort();
					Char @char = null;
					@char = ((num6 != Char.myCharz().charID) ? GameScr.findCharInMap(num6) : Char.myCharz());
					if (@char != null)
					{
						@char.idAuraEff = idAuraEff;
						@char.idEff_Set_Item = msg.reader().readByte();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readInfoEffChar(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				int num = msg.reader().readInt();
				Char @char = null;
				@char = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
				if (b == 0)
				{
					int id = msg.reader().readShort();
					int layer = msg.reader().readByte();
					int loop = msg.reader().readByte();
					short loopCount = msg.reader().readShort();
					sbyte isStand = msg.reader().readByte();
					@char?.addEffChar(new Effect(id, @char, layer, loop, loopCount, isStand));
				}
				else if (b == 1)
				{
					int id2 = msg.reader().readShort();
					@char?.removeEffChar(0, id2);
				}
				else if (b == 2)
				{
					@char?.removeEffChar(-1, 0);
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readActionBoss(Message msg, int actionBoss)
		{
			try
			{
				NewBoss newBoss = Mob.getNewBoss(msg.reader().readByte());
				if (newBoss == null)
					return;
				if (actionBoss == 10)
					newBoss.move(msg.reader().readShort(), msg.reader().readShort());
				if (actionBoss >= 11 && actionBoss <= 20)
				{
					sbyte b = msg.reader().readByte();
					Char[] array = new Char[b];
					int[] array2 = new int[b];
					for (int i = 0; i < b; i++)
					{
						int num = msg.reader().readInt();
						array[i] = null;
						if (num != Char.myCharz().charID)
							array[i] = GameScr.findCharInMap(num);
						else
							array[i] = Char.myCharz();
						array2[i] = msg.reader().readInt();
					}
					newBoss.setAttack(array, array2, (sbyte)(actionBoss - 10), msg.reader().readByte());
				}
				if (actionBoss == 21)
				{
					newBoss.xTo = msg.reader().readShort();
					newBoss.yTo = msg.reader().readShort();
					newBoss.setFly();
				}
				if (actionBoss == 22)
					;
				if (actionBoss == 23)
					newBoss.setDie();
			}
			catch (Exception)
			{
			}
		}
	}
}
