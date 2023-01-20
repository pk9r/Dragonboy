using Mod.ModMenu;
using Mod.Xmap;
using UnityEngine;

namespace Mod.Auto
{
    public class AutoT77
    {
        public static bool isAutoT77;
        public static bool isNoLongerReceivePoint;
        public static bool isT77TeleToGround;
        public static bool isTeleToT77;
        static bool isPicking;
        private static bool isAK;
        private static long lastTimeCheckTN;
        private static long lastTN;
        private static long lastTimePickedItem;

        public static void update()
        {
            if (!isAutoT77 || Char.myCharz().taskMaint.taskId < 9) return;
            if (Char.myCharz().cPower > 2000000 || Char.myCharz().cPower > 1500000 && TileMap.mapID != 111)
            {
                isAutoT77 = false;
                ModMenuMain.modMenuItemBools[6].Value = false;
                ModMenuMain.modMenuItemBools[6].isDisabled = true;
                GameScr.info1.addInfo("Đã up Tàu Pảy Pảy xong!", 0);
                return;
            }
            if (TileMap.mapID != Char.myCharz().cgender + 21 && GameCanvas.gameTick % (20 * Time.timeScale) == 0 && Char.myCharz().cHP * 100 / Char.myCharz().cHPFull < 6)
                GameScr.gI().doUseHP();
            if (TileMap.mapID != 111)
            {
                if (TileMap.mapID == 47) GameScr.gI().dHP = Char.myCharz().cHP;
                if (isAK) isAK = false;
                if (!isTeleToT77) isTeleToT77 = true;
                if (isNoLongerReceivePoint) isNoLongerReceivePoint = false;
                if (GameCanvas.gameTick % (20 * Time.timeScale) == 0)
                {
                    if (TileMap.mapID == Char.myCharz().cgender + 21)
                    {
                        AutoPick();
                        Service.gI().openMenu(4);
                        Service.gI().confirmMenu(4, 0);
                        GameCanvas.menu.doCloseMenu();
                        if (!isPicking && Char.myCharz().cHP > 1)
                        {
                            if (XmapController.gI.IsActing) XmapController.finishXmap();
                            if (!XmapController.gI.IsActing) XmapController.start(111);
                        }
                    }
                    else if (!XmapController.gI.IsActing) XmapController.start(111);
                }
            }
            else
            {
                Char t77 = (Char)GameScr.vCharInMap.elementAt(0);
                if (t77 == null)
                    return;
                if (GameCanvas.gameTick % (15 * Time.timeScale) == 0)
                {
                    if (Char.myCharz().cFlag != 8) Service.gI().getFlag(1, 8);
                    if (Char.myCharz().isDie || Char.myCharz().cHP <= 0) Service.gI().returnTownFromDead();
                    if ((Char.myCharz().cMP < 5 || Char.myCharz().cHP < 30) && GameScr.hpPotion < 1 || isNoLongerReceivePoint)
                    {
                        if (isTeleToT77)
                        {
                            Utilities.teleportMyChar(Char.myCharz().cx - 50);
                            isTeleToT77 = false;
                        }
                    }
                    else if (!isTeleToT77) isTeleToT77 = true;
                    if (isMeLostHP())
                    {
                        if (!XmapController.gI.IsActing) XmapController.start(47);
                    }
                }
                if (t77.cy == Utilities.getYGround(t77.cx) && isT77TeleToGround)
                {
                    Utilities.teleportMyChar(Char.myCharz().cx + 50);
                    isT77TeleToGround = false;
                    isAK = true;
                }
                if (isAK)
                {
                    CheckTN();
                    AK(t77);
                }
                else lastTimeCheckTN = mSystem.currentTimeMillis();
            }
        }

        private static bool isMeLostHP()
        {
            return Char.myCharz().cHP < GameScr.gI().dHP;
        }

        private static void CheckTN()
        {
            if (mSystem.currentTimeMillis() - lastTimeCheckTN >= 3000)
            {
                lastTimeCheckTN = mSystem.currentTimeMillis();
                if (lastTN == Char.myCharz().cTiemNang) isNoLongerReceivePoint = true;
                lastTN = Char.myCharz().cTiemNang;
            }
        }

        private static void AK(Char @char)
        {
            Skill skill = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[0]);
            Char.myCharz().myskill = skill;
            if (mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill <= skill.coolDown + 100L || @char.cTypePk != 5) return;
            skill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
            MyVector myVector = new MyVector();
            myVector.addElement(@char);
            Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
        }

        public static void AutoPick()
        {
            if (GameScr.vItemMap.size() == 0)
            {
                isPicking = false;
                return;
            }
            bool hasPickableItem = false;
            for (int i = GameScr.vItemMap.size() - 1; i >= 0; i--)
            {
                ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                if (itemMap == null) continue;
                int distance = Res.distance(Char.myCharz().cx, Char.myCharz().cy, itemMap.x, itemMap.y);
                if (itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1 && distance <= 60 || itemMap.template.id == 74) hasPickableItem = true;
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
                        if (distance > 60 && distance < 100) Char.myCharz().currentMovePoint = new MovePoint(itemMap.x, itemMap.y);
                        if (distance >= 100) Utilities.teleportMyChar(itemMap.x, itemMap.y);
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
            if (!hasPickableItem) isPicking = false;
            //if (mSystem.currentTimeMillis() - lastTimePickedItem <= 550) isPicking = false;
        }

        public static void setState(bool value) => isAutoT77 = value;
    }
}
