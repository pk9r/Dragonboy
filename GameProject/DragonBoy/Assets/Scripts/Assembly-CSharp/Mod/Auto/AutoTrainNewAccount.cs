using System;
using Assets.src.g;
using Mod.Constants;
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
        internal static bool isPicking;
        static long lastTimePickedItem;
        internal static bool isNhapCodeTanThu;
        internal static bool isHarvestingPean;
        static int myMinMP = 15;
        static long lastTimeEatPean;
        static int myMinHP = 15;
        static long lastTimeAutoPoint;
        static int minHPMob;
        static int maxHPMob = int.MaxValue;
        internal static bool isNhanBua;
        static bool isTeleT77 = true;
        static long lastTN;
        static long lastTimeCheckTN;
        static bool _isTanSatInternal;
        static bool isPKKarinSama;
        static bool isPKT77;
        static int minPeans;

        internal static void Update()
        {
            if (!isEnabled)
                return;
            try
            {
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
                if (GameScr.hpPotion <= 0 && (Char.myCharz().cMP < 15 || Char.myCharz().cHP < 15))
                {
                    if (!isHarvestingPean)
                        isHarvestingPean = true;
                    IsTanSat = isPKKarinSama = isPKT77 = false;
                    if (TileMap.mapID != Char.myCharz().cgender + 21 && !XmapController.gI.IsActing)
                        XmapController.start(Char.myCharz().cgender + 21);
                }
                if ((Char.myCharz().cMP < myMinMP || Char.myCharz().cHP < myMinHP) && !isHarvestingPean && (TileMap.mapID != Char.myCharz().cgender + 21 || Char.myCharz().taskMaint.taskId < 3) && mSystem.currentTimeMillis() - lastTimeEatPean > 2000 && (minPeans <= 0 || GameScr.hpPotion >= minPeans))
                {
                    lastTimeEatPean = mSystem.currentTimeMillis();
                    GameScr.gI().doUseHP();
                }
                if ((Char.myCharz().isDie || Char.myCharz().cHP <= 0) && GameCanvas.gameTick % (30f * Time.timeScale) == 0f)
                    Service.gI().returnTownFromDead();
                if (TileMap.mapID == Char.myCharz().cgender + 21)
                {
                    if (!isHarvestingPean && minPeans > 0 && GameScr.hpPotion < minPeans)
                        isHarvestingPean = true;
                    if (GameScr.vItemMap.size() > 0)
                    {
                        ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(0);
                        if (mSystem.currentTimeMillis() - lastTimePickedItem >= 550)
                        {
                            lastTimePickedItem = mSystem.currentTimeMillis();
                            Service.gI().pickItem(itemMap.itemMapID);
                        }
                    }
                    if (minPeans <= 0)
                    {
                        if (GameScr.gI().magicTree.currPeas == 0 || Char.myCharz().cgender == 1 && GameScr.hpPotion >= 30 || Char.myCharz().cgender != 1 && GameScr.hpPotion >= 20)
                            isHarvestingPean = false;
                    }
                    else if (GameScr.hpPotion >= minPeans)
                        isHarvestingPean = false;
                    if (Char.myCharz().taskMaint.taskId >= 2 && GameScr.gI().magicTree.currPeas > 0 && (Char.myCharz().cgender == 1 && GameScr.hpPotion < 30 || Char.myCharz().cgender != 1 && GameScr.hpPotion < 20) && GameCanvas.gameTick % (30f * Time.timeScale) == 0f)
                    {
                        Service.gI().openMenu(4);
                        Service.gI().confirmMenu(4, 0);
                    }
                    if (Char.myCharz().xu >= 5000 && GameScr.gI().magicTree.level == 1 && (GameScr.gI().magicTree.strInfo != LocalizedString.senzuTreeUpgrading || !GameScr.gI().magicTree.isUpdate) && GameCanvas.gameTick % (60f * Time.timeScale) == 0f)
                    {
                        Service.gI().openMenu(4);
                        Service.gI().confirmMenu(4, 1);
                        Service.gI().confirmMenu(5, 0);
                        GameScr.gI().magicTree.strInfo = LocalizedString.senzuTreeUpgrading;
                        GameScr.gI().magicTree.isUpdate = true;
                        isHarvestingPean = false;
                    }
                    if (GameCanvas.menu.showMenu)
                        GameCanvas.menu.doCloseMenu();
                }
                if (!isNhapCodeTanThu && !isHarvestingPean && !isPicking && GameCanvas.gameTick % (30 * (int)Time.timeScale) == 0 && Char.myCharz().cHP > 1 && !GameScr.gI().isBagFull() && Char.myCharz().taskMaint.taskId <= 11)
                    AutoNV();
                if (Char.myCharz().taskMaint.taskId > 3 && Char.myCharz().taskMaint.taskId <= 11)
                    AutoPoint();
                if (!XmapController.gI.IsActing && !isNhapCodeTanThu && !isHarvestingPean && Char.myCharz().cHP > 1 && !GameScr.gI().isBagFull() && Char.myCharz().taskMaint.taskId <= 11 && (minPeans <= 0 || GameScr.hpPotion >= minPeans))
                {
                    if (IsTanSat && !AutoPick())
                        TanSat();
                    if (isPKKarinSama)
                        PKThanMeo();
                    else if (isPKT77)
                        PKT77();
                }
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        static void AutoPoint()
        {
            if (mSystem.currentTimeMillis() - lastTimeAutoPoint >= 1000)
            {
                lastTimeAutoPoint = mSystem.currentTimeMillis();
                if ((Char.myCharz().cHPGoc < 400 || Char.myCharz().cDamGoc >= 40 && Char.myCharz().cHPGoc < 500) && Char.myCharz().cTiemNang > Char.myCharz().cHPGoc + 1000)
                    Service.gI().upPotential(0, 1);
                else if (Char.myCharz().cMPGoc < 300 && Char.myCharz().cDamGoc >= 25 && Char.myCharz().cTiemNang > Char.myCharz().cMPGoc + 1000)
                    Service.gI().upPotential(1, 1);
                else if (Char.myCharz().cDamGoc < 70 && Char.myCharz().cTiemNang > Char.myCharz().cDamGoc * 100)
                    Service.gI().upPotential(2, 1);
            }
        }

        static bool AutoPick()
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
                            Utils.TeleportMyChar(itemMap.x, itemMap.y);
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
            Mob mob = ClosestMob();
            if (mob == null)
                return;
            if (mSystem.currentTimeMillis() - lastTimeCheckTN > 3000)
            {
                lastTimeCheckTN = mSystem.currentTimeMillis();
                if (Char.myCharz().cTiemNang == lastTN)
                    Utils.TeleportMyChar(Char.myCharz());
                lastTN = Char.myCharz().cTiemNang;
            }
            if (mob.getTemplate().type == MonsterType.Fly)
            {
                if (Math.Abs(Char.myCharz().cx - mob.x) > 70)
                    Utils.TeleportMyChar(mob.x);
                else
                {
                    Char.myCharz().cx = mob.x + Res.random(-5, 5);
                    Char.myCharz().cy = mob.y + Res.random(-5, 5);
                    Service.gI().charMove();
                }
            }
            else
                Char.myCharz().currentMovePoint = new MovePoint(mob.x, mob.y);
            if (Utils.Distance(Char.myCharz(), mob) <= 50 || (mob.getTemplate().type == MonsterType.Fly && Math.Abs(Char.myCharz().cx - mob.x) <= 70))
            {
                mySkill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                Char.myCharz().mobFocus = mob;
                MyVector myVector = new MyVector();
                myVector.addElement(mob);
                Service.gI().sendPlayerAttack(myVector, new MyVector(), -1);
            }
        }

        static void TrainUntilMeStrongEnough(int maxHPmob)
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
                    Utils.TeleportMyChar(npc.cx);
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
                if (maxHPMob != maxHPmob)
                    maxHPMob = maxHPmob;
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

        static void AutoNV()
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
                Utils.TeleportMyChar(waypoint.maxX - 20, waypoint.maxY);
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
                            Utils.TeleportMyChar(85, 336);
                    }
                    if (Char.myCharz().cgender == 2)
                    {
                        if (Math.Abs(Char.myCharz().cx - 94) <= 10 && Math.Abs(Char.myCharz().cy - 336) <= 10)
                            Service.gI().getItem(0, 0);
                        else
                            Utils.TeleportMyChar(94, 336);
                    }
                    if (Char.myCharz().cgender == 1)
                    {
                        if (Math.Abs(Char.myCharz().cx - 638) <= 10 && Math.Abs(Char.myCharz().cy - 336) <= 10)
                            Service.gI().getItem(0, 0);
                        else
                            Utils.TeleportMyChar(638, 336);
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
                        Utils.TeleportMyChar(149, 288);
                    if (Char.myCharz().cgender == 1)
                        Utils.TeleportMyChar(126, 264);
                    if (Char.myCharz().cgender == 2)
                        Utils.TeleportMyChar(156, 288);
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
            if (Char.myCharz().taskMaint.index < 3)
            {
                //0  1  2
                //TD NM XD  0
                //NM TD XD  1
                //XD TD NM  2
                int mapID = 2 + Char.myCharz().cgender * 7;
                if (Char.myCharz().taskMaint.index == 1) 
                {
                    if (Char.myCharz().cgender == 0)
                        mapID = 9;
                    else
                        mapID = 2;
                }
                if (Char.myCharz().taskMaint.index == 2) 
                {
                    if (Char.myCharz().cgender == 2)
                        mapID = 9;
                    else
                        mapID = 16;
                } 
                if (TileMap.mapID == mapID)
                    IsTanSat = true;
                else
                {
                    IsTanSat = false;
                    if (!XmapController.gI.IsActing)
                        XmapController.start(mapID);
                }
            }
            else
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
            if (Char.myCharz().cPower <= 78000 || Char.myCharz().taskMaint.index == 0)
                TrainUntilMeStrongEnough(200);
            else if (Char.myCharz().taskMaint.index == 1)
            {
                if (TileMap.mapID == 3 || TileMap.mapID == 11 || TileMap.mapID == 17)
                {
                    if (myMinHP != 45)
                        myMinHP = 45;
                    if (myMinMP != 15)
                        myMinHP = 15;
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
            if (Char.myCharz().cPower <= 140000 || Char.myCharz().taskMaint.index == 0)
                TrainUntilMeStrongEnough(500);
            else if (Char.myCharz().taskMaint.index == 1)
            {
                if (myMinHP != 60)
                    myMinHP = 60;
                if (myMinMP != 15)
                    myMinMP = 15;
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
                else if (!XmapController.gI.IsActing)
                    XmapController.start(47);
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                if (TileMap.mapID != 47)
                {
                    if (!XmapController.gI.IsActing)
                        XmapController.start(47);
                }
                else
                {
                    if (Math.Abs(Char.myCharz().cx - 600) >= 20)
                        Utils.TeleportMyChar(600, 336);
                    else if (Char.myCharz().currentMovePoint == null || (Char.myCharz().currentMovePoint.xEnd != 600 && Char.myCharz().currentMovePoint.yEnd != 10))
                        Char.myCharz().currentMovePoint = new MovePoint(600, 10);
                }
            }
            else if (Char.myCharz().taskMaint.index == 3)
            {
                if (TileMap.mapID == 46)
                {
                    if (GameCanvas.menu.showMenu && GameCanvas.menu.menuItems.size() == 1)
                    {
                        Service.gI().confirmMenu(18, 0);
                        GameCanvas.menu.doCloseMenu();
                    }
                    else
                        Service.gI().openMenu(18);
                }
                else if (TileMap.mapID == 47)
                {
                    if (Math.Abs(Char.myCharz().cx - 600) >= 20)
                        Utils.TeleportMyChar(600, 336);
                    else if (Char.myCharz().currentMovePoint == null || (Char.myCharz().currentMovePoint.xEnd != 600 && Char.myCharz().currentMovePoint.yEnd != 10))
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
            isPKKarinSama = false;
            isPKT77 = false;
            minPeans = Char.myCharz().taskMaint.index > 1 ? 0 : 7;
            if (Char.myCharz().taskMaint.index == 0)
            {
                if (TileMap.mapID == 46)
                {
                    Npc karinSama = GameScr.findNPCInMap(18);
                    if (karinSama == null || karinSama.isHide)
                        isPKKarinSama = true;
                    else
                    {
                        if (Char.myCharz().cx != 421 || Char.myCharz().cy != 408)
                            Utils.TeleportMyChar(421, 408);
                        else
                        {
                            if (!GameCanvas.menu.showMenu)
                                Service.gI().openMenu(18);
                            else
                            {
                                if (GameCanvas.menu.menuItems.size() == 4 && ((Command)GameCanvas.menu.menuItems.elementAt(3)).caption.Replace('\n', ' ').ToLower() == LocalizedString.challengeKarin)
                                    Service.gI().confirmMenu(18, 3);
                                else if (GameCanvas.menu.menuItems.size() == 2 && ((Command)GameCanvas.menu.menuItems.elementAt(0)).caption.Replace('\n', ' ').ToLower() == LocalizedString.acceptChallenge)
                                    Service.gI().confirmMenu(18, 0);
                                GameCanvas.menu.doCloseMenu();
                                Char.chatPopup = null;
                            }
                        }
                    }
                }
                else
                {
                    if (TileMap.mapID == 47)
                    {
                        if (Math.Abs(Char.myCharz().cx - 600) >= 20)
                            Utils.TeleportMyChar(600, 336);
                        else if (Char.myCharz().currentMovePoint == null || (Char.myCharz().currentMovePoint.xEnd != 600 && Char.myCharz().currentMovePoint.yEnd != 10))
                            Char.myCharz().currentMovePoint = new MovePoint(600, 10);
                    }
                    else if (!XmapController.gI.IsActing)
                        XmapController.start(47);
                }
            }
            else if (Char.myCharz().taskMaint.index == 1)
            {
                if (TileMap.mapID == 46)
                {
                    if (Char.myCharz().currentMovePoint == null || (Char.myCharz().currentMovePoint.xEnd != 576 && Char.myCharz().currentMovePoint.yEnd != 552))
                        Char.myCharz().currentMovePoint = new MovePoint(576, 552);
                    isTeleT77 = true;
                }
                else if (TileMap.mapID == 47)
                {
                    if (isTeleT77 && (Char.myCharz().cx != 371 || Char.myCharz().cy != 336))
                    {
                        isTeleT77 = false;
                        Utils.TeleportMyChar(371, 336);
                    }
                    else
                        isPKT77 = true;
                }
                else
                {
                    isTeleT77 = true;
                    if (!XmapController.gI.IsActing)
                        XmapController.start(47);
                }
            }
            else if (Char.myCharz().taskMaint.index == 2)
            {
                if (TileMap.mapID == 47)
                    Service.gI().openMenu(17);
                else if (!XmapController.gI.IsActing)
                    XmapController.start(47);
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
        /// Làm nhiệm vụ gặp sư phụ mới
        /// </summary>
        static void AutoNV11()
        {
            if (XmapController.gI.IsActing)
                return;
            if (Char.myCharz().cgender == 0 && TileMap.mapID != 5)
                XmapController.start(5);
            else if (Char.myCharz().cgender == 1 && TileMap.mapID != 13)
                XmapController.start(13);
            else if (Char.myCharz().cgender == 2 && TileMap.mapID != 20)
                XmapController.start(20);
            else if (TileMap.mapID == 5 || TileMap.mapID == 13 || TileMap.mapID == 20)
            {
                if (!GameCanvas.menu.showMenu)
                    Service.gI().openMenu(13 + Char.myCharz().cgender);
                else
                {
                    if (GameCanvas.menu.menuItems.size() > 0)
                    {
                        Command command = (Command)GameCanvas.menu.menuItems.elementAt(0);
                        if (command.caption.Replace('\n', ' ') == LocalizedString.talk || command.caption.Replace('\n', ' ') == LocalizedString.mission)
                            command.performAction();
                    }
                    GameCanvas.menu.doCloseMenu();
                    Char.chatPopup = null;
                }
            }
        }

        static void PKThanMeo()
        {
            if (myMinHP != 60)
                myMinHP = 60;
            if (myMinMP != 20)
                myMinMP = 20;
            Skill mySkill = Char.myCharz().myskill;
            if (isPicking || mSystem.currentTimeMillis() - mySkill.lastTimeUseThisSkill <= mySkill.coolDown + 100L)
                return;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.cName != "Karin" || ch.cTypePk != 3)
                    continue;
                //Utils.DoDoubleClickToObj(ch);
                Char.myCharz().cx = ch.cx + Res.random(-5, 5);
                Char.myCharz().cy = ch.cy;
                Service.gI().charMove();
                if (Utils.Distance(Char.myCharz(), ch) <= 50)
                {
                    mySkill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                    Char.myCharz().charFocus = ch;
                    MyVector myVector = new MyVector();
                    myVector.addElement(ch);
                    Service.gI().sendPlayerAttack(new MyVector(), myVector, -1);
                }
            }
        }

        static void PKT77()
        {
            if (myMinHP != 100)
                myMinHP = 100;
            if (myMinMP != 20)
                myMinMP = 20;
            Skill mySkill = Char.myCharz().myskill;
            if (isPicking || mSystem.currentTimeMillis() - mySkill.lastTimeUseThisSkill <= mySkill.coolDown + 100L)
                return;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.cName != LocalizedString.mercenaryTao || ch.cTypePk != 3)
                    continue;
                //Utils.DoDoubleClickToObj(ch);
                Char.myCharz().cx = ch.cx + Res.random(-5, 5);
                Char.myCharz().cy = ch.cy;
                Service.gI().charMove();
                if (Utils.Distance(Char.myCharz(), ch) <= 50)
                {
                    mySkill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                    Char.myCharz().charFocus = ch;
                    MyVector myVector = new MyVector();
                    myVector.addElement(ch);
                    Service.gI().sendPlayerAttack(new MyVector(), myVector, -1);
                }
                break;
            }
        }

        internal static void SetState(bool value) => isEnabled = value;
    }
}