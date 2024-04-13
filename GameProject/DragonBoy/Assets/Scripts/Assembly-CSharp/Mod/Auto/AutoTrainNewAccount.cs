using System;
using Assets.src.g;
using Mod.ModMenu;
using Mod.R;
using Mod.Xmap;
using UnityEngine;

namespace Mod.Auto
{
    internal class AutoTrainNewAccount
    {
        internal static bool isEnabled;

        internal static bool IsTanSat
        {
            get => _isTanSatInternal;
            set
            {
                if (value == _isTanSatInternal)
                    return;
                lastTimeCheckTN = mSystem.currentTimeMillis();
                _isTanSatInternal = value;
            }
        }
        static long lastTimeDoubleClick;
        internal static bool isPicking;
        static long lastTimePickedItem;
        internal static bool isNhapCodeTanThu;
        internal static bool isNeedMorePean;
        internal static bool isHarvestingPean;
        static int myMinMP = 15;
        static long lastTimeEatPean;
        static int myMinHP = 15;
        static int stepXuLyDo = 2;
        static long lastTimeXuLyDo;
        static long lastTimeAutoPoint;
        static int minHPMob;
        static int maxHPMob = int.MaxValue;
        internal static bool isNhanBua;
        static bool isTeleT77;
        static long lastTN;
        static long lastTimeCheckTN;
        static bool _isTanSatInternal;

        internal static void Update()
        {
            if (!isEnabled)
				return;
            if (Char.myCharz().taskMaint.taskId > 11)
            {
                GameScr.info1.addInfo(Strings.completed + '!', 0);
                isEnabled = false;
                return;
            }
            if (Char.myCharz().taskMaint.taskId < 9)
            {
                for (int i = GameScr.vNpc.size() - 1; i >= 0; i--)
                    if (string.IsNullOrEmpty(((Npc)GameScr.vNpc.elementAt(i)).template.name))
                        GameScr.vNpc.removeElementAt(i);
                GameScr.vCharInMap.removeAllElements();
            }
            if (isNhapCodeTanThu && GameCanvas.gameTick % (60f * Time.timeScale) == 0f)
            {
                TField tField = new TField();
                tField.setText("tan thu nro");
                Service.gI().sendClientInput(new TField[1] { tField });
                GameScr.gI().switchToMe();
                ClientInput.instance = null;
                Char.chatPopup = null;
                isNhapCodeTanThu = false;
            }
            if (isNeedMorePean && (Char.myCharz().cgender != 1 && GameScr.hpPotion >= 10 || Char.myCharz().cgender == 1 && GameScr.hpPotion >= 20))
                isNeedMorePean = false;
            if (GameScr.hpPotion == 0 && (Char.myCharz().cMP < 15 || Char.myCharz().cHP < 15))
            {
                if (!isHarvestingPean)
                    isHarvestingPean = true;
                IsTanSat = false;
                if (!XmapController.gI.IsActing)
                    XmapController.start(Char.myCharz().cgender + 21);
            }
            if ((Char.myCharz().cMP < myMinMP || Char.myCharz().cHP < myMinHP) && !isHarvestingPean && (TileMap.mapID != Char.myCharz().cgender + 21 || Char.myCharz().taskMaint.taskId < 3) && mSystem.currentTimeMillis() - lastTimeEatPean > 2000)
            {
                lastTimeEatPean = mSystem.currentTimeMillis();
                GameScr.gI().doUseHP();
            }
            if ((Char.myCharz().isDie || Char.myCharz().cHP <= 0) && GameCanvas.gameTick % (30f * Time.timeScale) == 0f)
                Service.gI().returnTownFromDead();
            if (TileMap.mapID == Char.myCharz().cgender + 21)
            {
                if (GameScr.vItemMap.size() > 0)
                {
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(0);
                    if (mSystem.currentTimeMillis() - lastTimePickedItem >= 550)
                    {
                        lastTimePickedItem = mSystem.currentTimeMillis();
                        Service.gI().pickItem(itemMap.itemMapID);
                    }
                }
                if (GameScr.gI().magicTree.currPeas == 0 || Char.myCharz().cgender == 1 && GameScr.hpPotion >= 20 || Char.myCharz().cgender != 1 && GameScr.hpPotion >= 10)
                    isHarvestingPean = false;
                if (Char.myCharz().taskMaint.taskId >= 2 && GameScr.gI().magicTree.currPeas > 0 && (Char.myCharz().cgender == 1 && GameScr.hpPotion < 20 || Char.myCharz().cgender != 1 && GameScr.hpPotion < 10) && GameCanvas.gameTick % (30f * Time.timeScale) == 0f)
                {
                    Service.gI().openMenu(4);
                    Service.gI().confirmMenu(4, 0);
                }
                if (Char.myCharz().xu >= 5000 && GameScr.gI().magicTree.level == 1 && !GameScr.gI().magicTree.isUpdateTree && GameCanvas.gameTick % (30f * Time.timeScale) == 0f)
                {
                    Service.gI().openMenu(4);
                    Service.gI().confirmMenu(4, 1);
                    Service.gI().confirmMenu(5, 0);
                    isHarvestingPean = false;
                }
                if (GameCanvas.menu.showMenu)
                    GameCanvas.menu.doCloseMenu();
            }
            if (!isNhapCodeTanThu && !isNeedMorePean && !isHarvestingPean && !isPicking && GameCanvas.gameTick % (30 * (int)Time.timeScale) == 0 && Char.myCharz().cHP > 1 && !GameScr.gI().isBagFull() && stepXuLyDo > 1 && Char.myCharz().taskMaint.taskId <= 11)
				AutoNV();
            if (Char.myCharz().taskMaint.taskId > 3 && Char.myCharz().taskMaint.taskId <= 11)
				AutoPoint();
            if (GameScr.gI().isBagFull())
				stepXuLyDo = 0;
            if (stepXuLyDo < 2)
				XuLyDo();
            if (IsTanSat && !AutoPick())
				TanSat();
        }

        static bool isItemVip(Item item)
        {
            if (item.template.type <= 4)
            {
                for (int i = 0; i < item.itemOption.Length; i++)
                {
                    ItemOption itemOption = item.itemOption[i];
                    if (itemOption.optionTemplate.name.StartsWith("$")
						|| itemOption.optionTemplate.id == 107)
                        return true;
                }
                return false;
            }
            if (item.template.type == 23)
                return false;
            if (item.template.type != 6)
                return item.template.id != 225;
            return false;
        }

        static void XuLyDo()
        {
            IsTanSat = false;
            if (stepXuLyDo == 0 && !hasAnyVipItem())
				stepXuLyDo++;
            if (stepXuLyDo == 0)
            {
                if (TileMap.mapID != Char.myCharz().cgender + 21)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (Char.myCharz().cgender == 0 && (Math.Abs(Char.myCharz().cx - 85) > 10 || Math.Abs(Char.myCharz().cy - 336) > 10))
						Utils.teleportMyChar(85, 336);
                    if (Char.myCharz().cgender == 2 && (Math.Abs(Char.myCharz().cx - 94) > 10 || Math.Abs(Char.myCharz().cy - 336) > 10))
						Utils.teleportMyChar(94, 336);
                    if (Char.myCharz().cgender == 1 && (Math.Abs(Char.myCharz().cx - 638) > 10 || Math.Abs(Char.myCharz().cy - 336) > 10))
						Utils.teleportMyChar(638, 336);
                }
                if (mSystem.currentTimeMillis() - lastTimeXuLyDo > 750)
                {
                    lastTimeXuLyDo = mSystem.currentTimeMillis();
                    bool isCatDoVaoRuongXong = false;
                    for (int i = Char.myCharz().arrItemBag.Length - 1; i >= 0; i--)
                    {
                        Item item = Char.myCharz().arrItemBag[i];
                        if (item != null && isItemVip(item))
                        {
                            Service.gI().getItem(1, (sbyte)i);
                            isCatDoVaoRuongXong = true;
                            break;
                        }
                    }
                    if (!isCatDoVaoRuongXong)
						stepXuLyDo++;
                }
            }
            if (stepXuLyDo != 1)
                return;
            if (TileMap.mapID != Char.myCharz().cgender + 24)
            {
                if (!XmapController.gI.IsActing)
					XmapController.start(Char.myCharz().cgender + 24);
                return;
            }
            if (TileMap.mapID == 24)
            {
                if (Char.myCharz().cx != 389 || Char.myCharz().cy != 336)
					Utils.teleportMyChar(389, 336);
            }
            else if (TileMap.mapID == 25)
            {
                if (Char.myCharz().cx != 508 || Char.myCharz().cy != 336)
					Utils.teleportMyChar(508, 336);
            }
            else if (TileMap.mapID == 26 && (Char.myCharz().cx != 511 || Char.myCharz().cy != 336))
					Utils.teleportMyChar(511, 336);
            if (!GameCanvas.panel.isShow)
					Service.gI().openMenu(16);
            else
            {
                if (mSystem.currentTimeMillis() - lastTimeXuLyDo <= 750)
					return;
                lastTimeXuLyDo = mSystem.currentTimeMillis();
                bool isBanDoXong = false;
                for (int i = Char.myCharz().arrItemBag.Length - 1; i >= 0; i--)
                {
                    Item item2 = Char.myCharz().arrItemBag[i];
                    if (item2 != null && !isItemVip(item2) && item2.template.type != 23)
                    {
                        Service.gI().saleItem(0, 1, (short)i);
                        Service.gI().saleItem(1, 1, (short)i);
                        isBanDoXong = true;
                        break;
                    }
                }
                if (!isBanDoXong)
					stepXuLyDo++;
            }
        }

        static void AutoPoint()
        {
            if (mSystem.currentTimeMillis() - lastTimeAutoPoint >= 1000)
            {
                lastTimeAutoPoint = mSystem.currentTimeMillis();
                if ((Char.myCharz().cHPGoc < 400 || Char.myCharz().cDamGoc >= 40 && Char.myCharz().cHPGoc < 700) && Char.myCharz().cTiemNang > Char.myCharz().cHPGoc + 1000)
					Service.gI().upPotential(0, 1);
                else if (Char.myCharz().cMPGoc < 300 && Char.myCharz().cDamGoc >= 25 && Char.myCharz().cTiemNang > Char.myCharz().cMPGoc + 1000)
					Service.gI().upPotential(1, 1);
                else if (Char.myCharz().cDamGoc < 40 && Char.myCharz().cTiemNang > Char.myCharz().cDamGoc * 100)
					Service.gI().upPotential(2, 1);
                //if (Char.myCharz().cHPGoc >= 500)
                //{
                //  if (Char.myCharz().cMPGoc < 500 && Char.myCharz().cTiemNang > Char.myCharz().cMPGoc + 1000)
					    //Service.gI().upPotential(1, 1);
                //  if (Char.myCharz().cMPGoc >= 500 && Char.myCharz().cTiemNang > Char.myCharz().cDamGoc * 100)
					    //Service.gI().upPotential(2, 1);
                //}
            }
        }

        internal static bool AutoPick()
        {
            if (GameScr.vItemMap.size() == 0)
            {
                isPicking = false;
                return false;
            }
            bool hasPickableItem = false;
            for (int i = GameScr.vItemMap.size() - 1; i >= 0; i--)
            {
                ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                if (itemMap == null)
						continue;
                int distance = Res.distance(Char.myCharz().cx, Char.myCharz().cy, itemMap.x, itemMap.y);
                if (itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1 && distance <= 60 || itemMap.template.id == 74)
					hasPickableItem = true;
                if (itemMap.template.id >= 828 && itemMap.template.id <= 842 || itemMap.template.id == 859 || itemMap.template.id == 362 || itemMap.template.id >= 353 && itemMap.template.id <= 360)
                {
                    GameScr.vItemMap.removeElementAt(i);
                    continue;
                }
                if (mSystem.currentTimeMillis() - lastTimePickedItem > 550)
                {
                    if (itemMap.playerId == Char.myCharz().charID)
                    {
                        isPicking = true;
                        Char.myCharz().mobFocus = null;
                        if (distance > 60 && distance < 100)
						    Char.myCharz().currentMovePoint = new MovePoint(itemMap.x, itemMap.y);
                        if (distance >= 100)
						    Utils.teleportMyChar(itemMap.x, itemMap.y);
                        Service.gI().pickItem(itemMap.itemMapID);
                        lastTimePickedItem = mSystem.currentTimeMillis();
                        continue;
                    }
                    else if (itemMap.playerId == -1 && distance <= 60 || itemMap.template.id == 74)
                    {
                        isPicking = true;
                        Char.myCharz().mobFocus = null;
                        Service.gI().pickItem(itemMap.itemMapID);
                        lastTimePickedItem = mSystem.currentTimeMillis();
                        continue;
                    }
                }
            }
            if (!hasPickableItem)
				isPicking = false;
            return hasPickableItem;
            //if (mSystem.currentTimeMillis() - lastTimePickedItem <= 550)
				//isPicking = false;
        }

        static void TanSat()
        {
            Skill mySkill = Char.myCharz().myskill;
            if (isPicking || mSystem.currentTimeMillis() - mySkill.lastTimeUseThisSkill <= mySkill.coolDown + 100L)
				return;
            mySkill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
            MyVector myVector = new MyVector();
            Mob mob = ClosestMob();
            Char.myCharz().mobFocus = mob;
            myVector.addElement(mob);
            if (mSystem.currentTimeMillis() - lastTimeCheckTN > 3000)
            {
                lastTimeCheckTN = mSystem.currentTimeMillis();
                if (Char.myCharz().cTiemNang == lastTN)
                    Utils.teleportMyChar(Char.myCharz());
                lastTN = Char.myCharz().cTiemNang;
            }
            if (mob.getTemplate().type == 4)    //quái bay
            {
                if (Math.Abs(Char.myCharz().cx - mob.x) > 100)    
                {
                    Utils.teleportMyChar(mob.x);
                    return;
                }
                else
                {
                    Char.myCharz().cy = mob.y + Res.random(-3, 3);
                    Service.gI().charMove();
                }
            }
            else
                Char.myCharz().currentMovePoint = new MovePoint(mob.x, mob.y);
            if (Utils.getDistance(Char.myCharz(), mob) <= 50)
                Service.gI().sendPlayerAttack(myVector, new MyVector(), -1);
        }

        static Mob ClosestMob()
        {
            Mob result = null;
            int minDistance = int.MaxValue;
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isMobMe && mob.maxHp <= maxHPMob && mob.maxHp >= minHPMob)
                {
                    int distance = Res.distance(mob.x, mob.y, Char.myCharz().cx, Char.myCharz().cy);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        result = mob;
                    }
                }
            }
            return result;
        }

        internal static void AutoNV()
        {
            switch (Char.myCharz().taskMaint.taskId)
            {
                case 0:
                    AutoNV0();
                    break;
                case 1:
                    AutoNV1();
                    break;
                case 2:
                    AutoNV2();
                    break;
                case 3:
                    AutoNV3();
                    break;
                case 4:
                case 5:
                case 6:
                    AutoNV4to6();
                    break;
                case 7:
                    AutoNV7();
                    break;
                case 8:
                    AutoNV8();
                    break;
                case 9:
                    AutoNV9();
                    break;
                case 10:
                    AutoNV10();
                    break;
                case 11:
                    AutoNV11();
                    break;
            }
        }

        /// <summary>
        /// Làm nhiệm vụ đầu tiên
        /// </summary>
        static void AutoNV0() 
        {
            if (TileMap.mapID >= 39 && TileMap.mapID <= 41)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(0);
                Utils.teleportMyChar(waypoint.maxX - 20, waypoint.maxY);
            }
            else if (TileMap.mapID >= 21 && TileMap.mapID <= 23)
            {
                if (Char.myCharz().taskMaint.index == 2)
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
                else if (Char.myCharz().taskMaint.index == 3)
                {
                    if (Char.myCharz().cgender == 0)
                    {
                        if (Math.Abs(Char.myCharz().cx - 85) <= 10 && Math.Abs(Char.myCharz().cy - 336) <= 10)
						    Service.gI().getItem(0, 0);
                        else
                            Utils.teleportMyChar(85, 336);
                    }
                    if (Char.myCharz().cgender == 2)
                    {
                        if (Math.Abs(Char.myCharz().cx - 94) <= 10 && Math.Abs(Char.myCharz().cy - 336) <= 10)
						    Service.gI().getItem(0, 0);
                        else
                            Utils.teleportMyChar(94, 336);
                    }
                    if (Char.myCharz().cgender == 1)
                    {
                        if (Math.Abs(Char.myCharz().cx - 638) <= 10 && Math.Abs(Char.myCharz().cy - 336) <= 10)
						    Service.gI().getItem(0, 0);
                        else
                            Utils.teleportMyChar(638, 336);
                    }
                }
                else if (Char.myCharz().taskMaint.index == 4)
                {
                    Service.gI().openMenu(4);
                    Service.gI().confirmMenu(4, 0);
                }
                else if (Char.myCharz().taskMaint.index == 5)
                {
                    if (GameCanvas.menu.showMenu)
						GameCanvas.menu.doCloseMenu();
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
        }   

        /// <summary>
        /// Làm nhiệm vụ đánh mộc nhân 
        /// </summary>
        static void AutoNV1()
        {
            if (Char.myCharz().taskMaint.index == 0)
            {
                Char.myCharz().npcFocus = null;
                if (TileMap.mapID >= 21 && TileMap.mapID <= 23)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender * 7);
                }
                else if (TileMap.mapID == Char.myCharz().cgender * 7)
                    IsTanSat = true;
            }
            else if (Char.myCharz().taskMaint.index == 1)
            {
                IsTanSat = false;
                Char.myCharz().mobFocus = null;
                Char.myCharz().itemFocus = null;
                Char.myCharz().charFocus = null;
                if (!XmapController.gI.IsActing && TileMap.mapID != Char.myCharz().cgender + 21)
					XmapController.start(Char.myCharz().cgender + 21);
                else if (TileMap.mapID == Char.myCharz().cgender + 21)
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
        }

        /// <summary>
        /// Làm nhiệm vụ đùi gà
        /// </summary>
        static void AutoNV2()
        {
            if (Char.myCharz().taskMaint.index == 0)
            {
                if (TileMap.mapID != Char.myCharz().cgender * 7 + 1)
                {
                    if (!XmapController.gI.IsActing)
                    {
                        IsTanSat = false;
                        XmapController.start(Char.myCharz().cgender * 7 + 1);
                    }
                }
                else if (!IsTanSat)
                    IsTanSat = true;
            }
            else if (Char.myCharz().taskMaint.index == 1)
            {
                Char.myCharz().mobFocus = null;
                IsTanSat = false;
                if (TileMap.mapID != Char.myCharz().cgender + 21)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
        }

        /// <summary>
        /// Làm nhiệm vụ sao băng
        /// </summary>
        static void AutoNV3()
        {
            if (Char.myCharz().taskMaint.index == 0)
					Service.gI().upPotential(2, 1);
            if (Char.myCharz().taskMaint.index == 1)
            {
                if (TileMap.mapID == Char.myCharz().cgender + 42)
                {
                    if (Char.myCharz().cgender == 0)
						Utils.teleportMyChar(149, 288);
                    if (Char.myCharz().cgender == 1)
						Utils.teleportMyChar(126, 264);
                    if (Char.myCharz().cgender == 2)
						Utils.teleportMyChar(156, 288);
                    AutoPick();
                }
                else if (!XmapController.gI.IsActing)
					XmapController.start(Char.myCharz().cgender + 42);
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                if (TileMap.mapID != Char.myCharz().cgender + 21)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
        }

        /// <summary>
        /// Làm nhiệm vụ đánh quái mẹ
        /// </summary>
        static void AutoNV4to6()
        {
            if (myMinHP != 30)
				myMinHP = 30;
            if (myMinMP != 15)
				myMinHP = 15;
            if (maxHPMob != 500)
				maxHPMob = 500;
            if (minHPMob != 499)
				minHPMob = 499;
            if (Char.myCharz().taskMaint.index == 0)
            {
                if (TileMap.mapID == 2 + Char.myCharz().cgender * 7)
					IsTanSat = true;
                else
                {
                    IsTanSat = false;
                    if (!XmapController.gI.IsActing)
						XmapController.start(2 + Char.myCharz().cgender * 7);
                }
            }
            else if (Char.myCharz().taskMaint.index == 1)
            {
                int num = (Char.myCharz().cgender + 1) % 3;
                if (TileMap.mapID == 2 + num * 7)
					IsTanSat = true;
                else
                {
                    IsTanSat = false;
                    if (!XmapController.gI.IsActing)
						XmapController.start(2 + num * 7);
                }
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                int num2 = (Char.myCharz().cgender + 2) % 3;
                if (TileMap.mapID == 2 + num2 * 7)
					IsTanSat = true;
                else
                {
                    IsTanSat = false;
                    if (!XmapController.gI.IsActing)
						XmapController.start(2 + num2 * 7);
                }
            }
            else if (Char.myCharz().taskMaint.index == 3)
            {
                IsTanSat = false;
                maxHPMob = int.MaxValue;
                minHPMob = 0;
                if (TileMap.mapID != Char.myCharz().cgender + 21)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
        }

        /// <summary>
        /// Làm nhiệm vụ đánh 20 quái bay
        /// </summary>
        static void AutoNV7()
        {
            if (Char.myCharz().cPower <= 70000)
            {
                if (myMinHP != 15)
					myMinHP = 15;
                if (myMinMP != 15)
					myMinMP = 15;
                if (isNhanBua && GameCanvas.menu.showMenu && TileMap.mapID == Char.myCharz().cgender + 42)
					GameCanvas.menu.doCloseMenu();
                if (!isNhanBua)
                {
                    if (TileMap.mapID != Char.myCharz().cgender + 42)
                    {
                        if (!XmapController.gI.IsActing)
                        {
                            IsTanSat = false;
                            XmapController.start(Char.myCharz().cgender + 42);
                        }
                    }
                    else
                    {
                        Npc npc = (Npc)GameScr.vNpc.elementAt(0);
                        Utils.teleportMyChar(npc.cx);
                        if (Res.distance(Char.myCharz().cx, Char.myCharz().cy, npc.cx, npc.cy)
						<= 50)
                        {
                            if (!GameCanvas.menu.showMenu)
						        Service.gI().openMenu(21);
                            else
                            {
                                Command command = (Command)GameCanvas.menu.menuItems.elementAt(0);
                                if (command.caption.Replace('\n', ' ').ToLower() == LocalizedString.free1hCharm)
						            Service.gI().confirmMenu(21, 0);
                                isNhanBua = true;
                            }
                        }
                    }
                }
                else if (TileMap.mapID == 9 || TileMap.mapID == 3 || TileMap.mapID == 17)
                {
                    if (maxHPMob != 200)
						maxHPMob = 200;
                    if (minHPMob != 0)
						minHPMob = 0;
                    if (!IsTanSat)
						IsTanSat = true;
                }
                else if (!XmapController.gI.IsActing)
                {
                    IsTanSat = false;
                    if (Char.myCharz().cgender == 0)
						XmapController.start(3);
                    if (Char.myCharz().cgender == 1)
						XmapController.start(9);
                    if (Char.myCharz().cgender == 2)
						XmapController.start(17);
                }
            }
            else if (Char.myCharz().taskMaint.index == 1)
            {
                if (TileMap.mapID == 3 || TileMap.mapID == 11 || TileMap.mapID == 17)
                {
                    if (maxHPMob != 600)
						maxHPMob = 600;
                    if (minHPMob != 599)
						minHPMob = 599;
                    IsTanSat = true;
                }
                else if (!XmapController.gI.IsActing)
                {
                    IsTanSat = false;
                    if (Char.myCharz().cgender == 0)
						XmapController.start(3);
                    if (Char.myCharz().cgender == 1)
						XmapController.start(11);
                    if (Char.myCharz().cgender == 2)
						XmapController.start(17);
                }
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                IsTanSat = false;
                maxHPMob = int.MaxValue;
                minHPMob = 0;
                if (TileMap.mapID == Char.myCharz().cgender * 7)
					Service.gI().openMenu(Char.myCharz().cgender + 7);
                else if (!XmapController.gI.IsActing)
					XmapController.start(Char.myCharz().cgender * 7);
            }
            else if (Char.myCharz().taskMaint.index == 3)
            {
                if (TileMap.mapID != Char.myCharz().cgender + 21)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
        }

        /// <summary>
        /// Làm nhiệm vụ ngọc rồng 7 sao
        /// </summary>
        static void AutoNV8()
        {
            if (Char.myCharz().taskMaint.index == 1)
            {
                if (!XmapController.gI.IsActing)
                {
                    if (Char.myCharz().cgender == 0 && TileMap.mapID != 12)
						XmapController.start(12);
                    else if (Char.myCharz().cgender == 1 && TileMap.mapID != 18)
						XmapController.start(18);
                    else if (Char.myCharz().cgender == 2 && TileMap.mapID != 4)
						XmapController.start(4);
                }
                if (TileMap.mapID == 12 || TileMap.mapID == 18 || TileMap.mapID == 4)
                {
                    maxHPMob = 1000;
                    minHPMob = 999;
                    if (!IsTanSat)
						IsTanSat = true;
                }
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                IsTanSat = false;
                if (TileMap.mapID != Char.myCharz().cgender + 21)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
            else if (Char.myCharz().taskMaint.index == 3 && TileMap.mapID != 47 && !XmapController.gI.IsActing)
				XmapController.start(47);
        }

        /// <summary>
        /// Làm nhiệm vụ vào rừng Karin
        /// </summary>
        static void AutoNV9()
        {
            if (Char.myCharz().taskMaint.index <= 1)
            {
                if (TileMap.mapID == 47)
					Service.gI().openMenu(17);
                if (TileMap.mapID != 1 && !XmapController.gI.IsActing)
					XmapController.start(1);
                if (TileMap.mapID == 1 && !XmapController.gI.IsActing)
					XmapController.start(47);
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                if (TileMap.mapID != 47 && !XmapController.gI.IsActing)
					XmapController.start(47);
                else if (Math.Abs(Char.myCharz().cx - 600) >= 20)
					Utils.teleportMyChar(600, 336);
                else if (Char.myCharz().currentMovePoint != null && Char.myCharz().currentMovePoint.xEnd != 600 && Char.myCharz().currentMovePoint.yEnd != 10)
					Char.myCharz().currentMovePoint = new MovePoint(600, 10);
            }
            else if (Char.myCharz().taskMaint.index == 3)
            {
                if (TileMap.mapID == 46)
					Service.gI().openMenu(18);
                else if (TileMap.mapID == 47)
                {
                    if (Math.Abs(Char.myCharz().cx - 600) >= 20)
						Utils.teleportMyChar(600, 336);
                    else if (Char.myCharz().currentMovePoint != null && Char.myCharz().currentMovePoint.xEnd != 600 && Char.myCharz().currentMovePoint.yEnd != 10)
						Char.myCharz().currentMovePoint = new MovePoint(600, 10);
                }
                else if (!XmapController.gI.IsActing)
					XmapController.start(47);
            }
        }

        /// <summary>
        /// Làm nhiệm vụ thách đấu thần Mèo
        /// </summary>
        static void AutoNV10()
        {
            if (Char.myCharz().taskMaint.index == 0)
            {
                if (TileMap.mapID == 46)
                {
                    if (GameCanvas.menu.showMenu)
						GameCanvas.menu.doCloseMenu();
                    if (GameScr.findNPCInMap(18) == null || GameScr.findNPCInMap(18) != null && GameScr.findNPCInMap(18).isHide)
						PKThanMeo();
                    if (Char.myCharz().cx != 421 || Char.myCharz().cy != 408)
						Utils.teleportMyChar(421, 408);
                    if (!GameCanvas.menu.showMenu)
						Service.gI().openMenu(18);
                    if (GameCanvas.menu.menuItems.size() == 4 && ((Command)GameCanvas.menu.menuItems.elementAt(3)).caption.Replace('\n', ' ').ToLower() == LocalizedString.challengeKarin)
						Service.gI().confirmMenu(18, 3);
                    else if (GameCanvas.menu.menuItems.size() == 2 && ((Command)GameCanvas.menu.menuItems.elementAt(0)).caption.Replace('\n', ' ').ToLower() == LocalizedString.acceptChallenge)
						Service.gI().confirmMenu(18, 0);
                }
                else
                {
                    if (TileMap.mapID == 47)
                    {
                        if (Math.Abs(Char.myCharz().cx - 600) >= 20)
						Utils.teleportMyChar(600, 336);
                        else if (Char.myCharz().currentMovePoint != null && Char.myCharz().currentMovePoint.xEnd != 600 && Char.myCharz().currentMovePoint.yEnd != 10)
						Char.myCharz().currentMovePoint = new MovePoint(600, 10);
                    }
                    else if (!XmapController.gI.IsActing)
						XmapController.start(47);
                }
            }
            else if (Char.myCharz().taskMaint.index == 1)
            {
                if (TileMap.mapID == 47)
                {
                    if (isTeleT77 && (Char.myCharz().cx != 371 || Char.myCharz().cy != 336))
						Utils.teleportMyChar(371, 336);
                    else PKT77();
                }
                else if (!isTeleT77)
						isTeleT77 = true;
                if (TileMap.mapID == 46)
                {
                    if (Char.myCharz().currentMovePoint != null && Char.myCharz().currentMovePoint.xEnd != 579 && Char.myCharz().currentMovePoint.yEnd != 420)
						Char.myCharz().currentMovePoint = new MovePoint(579, 420);
                }
                else if (!XmapController.gI.IsActing)
						XmapController.start(47);
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                if (TileMap.mapID == 47)
						Service.gI().openMenu(17);
                if (!XmapController.gI.IsActing)
						XmapController.start(47);
            }
            else if (Char.myCharz().taskMaint.index == 3)
            {
                if (TileMap.mapID < 21 || TileMap.mapID > 23)
                {
                    if (!XmapController.gI.IsActing)
						XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (Char.myCharz().cgender == 0)
						Service.gI().openMenu(0);
                    if (Char.myCharz().cgender == 1)
						Service.gI().openMenu(2);
                    if (Char.myCharz().cgender == 2)
						Service.gI().openMenu(1);
                }
            }
        }

        /// <summary>
        /// Làm nhiệm vụ gặp sư phụ mới
        /// </summary>
        static void AutoNV11()
        {
            if (Char.myCharz().cgender == 0)
            {
                if (TileMap.mapID != 5 && !XmapController.gI.IsActing)
					XmapController.start(5);
            }
            else if (Char.myCharz().cgender == 1)
            {
                if (TileMap.mapID != 13 && !XmapController.gI.IsActing)
					XmapController.start(13);
            }
            else if (Char.myCharz().cgender == 2 && TileMap.mapID != 20 && !XmapController.gI.IsActing)
					XmapController.start(20);
            if (TileMap.mapID == 5 || TileMap.mapID == 13 || TileMap.mapID == 20)
					Service.gI().openMenu(13 + Char.myCharz().cgender);
        }

        static void PKThanMeo()
        {
            if (!isNeedMorePean)
				isNeedMorePean = true;
            if (myMinHP != 60)
				myMinHP = 60;
            if (myMinMP != 20)
				myMinMP = 20;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.cName == "Karin" && ch.cTypePk == 3)
                    Utils.DoDoubleClickToObj(ch);
            }
        }

        static void PKT77()
        {
            if (!isNeedMorePean)
				isNeedMorePean = true;
            if (myMinHP != 80)
				myMinHP = 80;
            if (myMinMP != 20)
				myMinMP = 20;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.cName == LocalizedString.mercenaryTao && ch.cTypePk == 3)
                    Utils.DoDoubleClickToObj(ch);
            }
        }

        static bool hasAnyVipItem()
        {
            for (int num = Char.myCharz().arrItemBag.Length - 1; num >= 0; num--)
            {
                Item item = Char.myCharz().arrItemBag[num];
                if (item != null && isItemVip(item))
					return true;
            }
            return false;
        }

        internal static void setState(bool value) => isEnabled = value;
    }
}