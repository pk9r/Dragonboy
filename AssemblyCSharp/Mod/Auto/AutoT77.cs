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
        private static bool isAK;
        private static long lastTimeCheckTN;
        private static long lastTN;

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
            if (TileMap.mapID != Char.myCharz().cgender + 21 && GameCanvas.gameTick % (20 * Time.timeScale) == 0 && Char.myCharz().cHP * 100 / Char.myCharz().cHPFull < 6) GameScr.gI().doUseHP();
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
                        AutoSS.AutoPick();
                        Service.gI().openMenu(4);
                        Service.gI().confirmMenu(4, 0);
                        GameCanvas.menu.doCloseMenu();
                        if (!AutoSS.isPicking && Char.myCharz().cHP > 1)
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

        public static void setState(bool value) => isAutoT77 = value;
    }
}
