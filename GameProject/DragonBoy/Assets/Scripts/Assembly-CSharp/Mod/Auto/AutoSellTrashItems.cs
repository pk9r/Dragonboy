using Mod.PickMob;
using Mod.R;
using Mod.Xmap;

namespace Mod.Auto
{
    internal class AutoSellTrashItems
    {
        internal static bool isEnabled;
        internal static bool IsRunning => steps > 0;
        static int steps;
        static long lastTimeUpdate;
        static int lastSellItemIndex = -1;
        static int lastMapID;
        static int lastZoneID;
        static int lastX;
        static int lastY;
        static bool lastPickMobState;

        internal static void Update()
        {
            if (!isEnabled)
                return;
            if (mSystem.currentTimeMillis() - lastTimeUpdate <= 500)
                return;
            lastTimeUpdate = mSystem.currentTimeMillis();
            switch (steps)
            {
                case 0:
                default:
                    CheckShouldSellTrashItems();
                    break;
                case 1:
                    GoHome();
                    break;
                case 2:
                    MoveVipItemsToChest();
                    break;
                case 3:
                    GotoSpaceshipStation();
                    break;
                case 4:
                    SellTrashItems();
                    break;
                case 5:
                    GotoLastMapAndZone();
                    break;
            }
        }

        static void CheckShouldSellTrashItems()
        {
            if (!GameScr.gI().isBagFull())
                return;
            PausePickMob();
            lastMapID = TileMap.mapID;
            lastZoneID = TileMap.zoneID;
            lastX = Char.myCharz().cx;
            lastY = Char.myCharz().cy;
            steps = 1;
        }

        static void GoHome()
        {
            if (TileMap.mapID != Char.myCharz().cgender + 21)
            {
                if (!XmapController.gI.IsActing)
                    XmapController.start(Char.myCharz().cgender + 21);
            }
            else
                steps = 2;
        }

        static void MoveVipItemsToChest()
        {
            if (Char.myCharz().cgender == 0 && Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, 85, 336) > 15)
                Utils.TeleportMyChar(85, 336);
            else if (Char.myCharz().cgender == 2 && Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, 94, 336) > 15)
                Utils.TeleportMyChar(94, 336);
            else if (Char.myCharz().cgender == 1 && Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, 638, 336) > 15)
                Utils.TeleportMyChar(638, 336);
            else if (GameCanvas.panel.hasUse >= Char.myCharz().arrItemBox.Length)
            {
                GameCanvas.startOKDlg(Strings.autoSellTrashItemsBoxFull + '!');
                isEnabled = false;
            }
            else
            {
                int i = Char.myCharz().arrItemBag.Length - 1;
                for (; i >= 0; i--)
                {
                    Item item = Char.myCharz().arrItemBag[i];
                    if (item != null && item.IsWearableAndVip())
                    {
                        Service.gI().getItem(1, (sbyte)i);
                        break;
                    }
                }
                if (i == -1)
                    steps = 3;
            }
        }

        static void GotoSpaceshipStation()
        {
            if (TileMap.mapID != Char.myCharz().cgender + 24)
            {
                if (!XmapController.gI.IsActing)
                    XmapController.start(Char.myCharz().cgender + 24);
            }
            else
                steps = 4;
        }

        static void SellTrashItems()
        {
            if (TileMap.mapID == 24)
            {
                if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, 389, 336) > 15)
                {
                    Utils.TeleportMyChar(389, 336);
                    return;
                }
            }
            else if (TileMap.mapID == 25)
            {
                if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, 508, 336) > 15)
                {
                    Utils.TeleportMyChar(508, 336);
                    return;
                }
            }
            else if (TileMap.mapID == 26)
            {
                if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, 511, 336) > 15)
                {
                    Utils.TeleportMyChar(511, 336);
                    return;
                }
            }
            if (!GameCanvas.panel.isShow)
                Service.gI().openMenu(16);
            else if (GameCanvas.currentDialog != null && lastSellItemIndex > -1)
            {
                Service.gI().saleItem(1, 1, (short)lastSellItemIndex);
                GameCanvas.endDlg();
            }
            else
            {
                int i = Char.myCharz().arrItemBag.Length - 1;
                if (lastSellItemIndex != -1)
                    i = lastSellItemIndex;
                for (; i >= 0; i--)
                {
                    Item item2 = Char.myCharz().arrItemBag[i];
                    if (item2 != null && !item2.IsWearableAndVip() && item2.template.type != 23 && item2.template.type != 6)
                    {
                        Service.gI().saleItem(0, 1, (short)i);
                        lastSellItemIndex = i;
                        break;
                    }
                }
                if (i == -1)
                {
                    lastSellItemIndex = -1;
                    steps = 5;
                }
            }
        }

        static void GotoLastMapAndZone()
        {
            if (TileMap.mapID != lastMapID)
            {
                if (!XmapController.gI.IsActing)
                    XmapController.start(lastMapID);
            }
            else if (TileMap.zoneID != lastZoneID)
                Service.gI().requestChangeZone(lastZoneID, 0);
            else if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, lastX, lastY) > 15)
                Utils.TeleportMyChar(lastX, lastY);
            else
            {
                ResumePickMob();
                steps = 0;
            }
        }

        static void PausePickMob()
        {
            lastPickMobState = Pk9rPickMob.IsTanSat;
            Pk9rPickMob.IsTanSat = false;
        }

        static void ResumePickMob() => Pk9rPickMob.IsTanSat = lastPickMobState;

        internal static void SetState(bool value)
        {
            isEnabled = value;
            if (isEnabled)
                steps = 0;
        }
    }
}