using Mod.Constants;
using Mod.PickMob;
using Mod.R;
using Mod.Xmap;

namespace Mod.Auto
{
    internal class AutoSellTrashItems
    {
        internal static bool isEnabled;
        internal static bool IsRunning => isEnabled && steps > 0;
        static int steps;
        static long lastTimeUpdate;
        static int lastRemoveItemIndex = -1;
        static int lastMapID;
        static int lastZoneID;
        static int lastX;
        static int lastY;
        static bool lastPickMobState;
        static int removeAttempts;

        internal static void Update()
        {
            if (!isEnabled)
                return;
            if (mSystem.currentTimeMillis() - lastTimeUpdate <= 750)
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
                    ThrowTrashItems();
                    break;
                case 6:
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
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                Item item = Char.myCharz().arrItemBag[i];
                if (item != null && ShouldMoveItemToChest(item))
                {
                    steps = 1;
                    return;
                }
            }
            steps = 3;
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
                //isEnabled = false;
                steps = 3;
            }
            else
            {
                int i = Char.myCharz().arrItemBag.Length - 1;
                for (; i >= 0; i--)
                {
                    Item item = Char.myCharz().arrItemBag[i];
                    if (item != null && ShouldMoveItemToChest(item))
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
            else if (GameCanvas.currentDialog != null && lastRemoveItemIndex > -1)
            {
                Service.gI().saleItem(1, 1, (short)lastRemoveItemIndex);
                GameCanvas.endDlg();
            }
            else
            {
                int i = Char.myCharz().arrItemBag.Length - 1;
                if (lastRemoveItemIndex != -1)
                    i = lastRemoveItemIndex;
                for (; i >= 0; i--)
                {
                    Item item = Char.myCharz().arrItemBag[i];
                    if (item != null && !ShouldKeepItem(item))
                    {
                        Service.gI().saleItem(0, 1, (short)i);
                        if (i == lastRemoveItemIndex)
                            removeAttempts++;
                        else
                            removeAttempts = 0;
                        if (removeAttempts >= 5)
                            lastRemoveItemIndex = i - 1;
                        else 
                            lastRemoveItemIndex = i;
                        break;
                    }
                }
                if (i < 0)
                {
                    GameCanvas.panel?.hide();
                    GameCanvas.panel2?.hide();
                    removeAttempts = 0;
                    lastRemoveItemIndex = -1;
                    steps = 5;
                }
            }
        }

        static void ThrowTrashItems()
        {
            if (Char.myCharz().arrItemBag.Length == 0)
            {
                steps = 6;
                return;
            }
            if (Char.myCharz().cPower < 1_500_000)
            {
                steps = 6;
                return;
            }
            int i = Char.myCharz().arrItemBag.Length - 1;
            if (lastRemoveItemIndex != -1)
                i = lastRemoveItemIndex;
            for (; i >= 0; i--)
            {
                Item item = Char.myCharz().arrItemBag[i];
                if (item != null && !ShouldKeepItem(item))
                {
                    Service.gI().useItem(1, 1, (sbyte)i, -1);
                    if (i == lastRemoveItemIndex)
                        removeAttempts++;
                    else
                        removeAttempts = 0;
                    if (removeAttempts >= 5)
                        lastRemoveItemIndex = i - 1;
                    else
                        lastRemoveItemIndex = i;
                    break;
                }
            }
            if (i < 0)
            {
                removeAttempts = 0;
                lastRemoveItemIndex = -1;
                steps = 6;
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
                Char.chatPopup = null;
                ChatPopup.currChatPopup = null;
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

        static bool ShouldMoveItemToChest(Item item) => item.IsWearableAndVip() || item.template.type == ItemTemplateType.FlyPlatform || item.template.type == ItemTemplateType.VIPFlyPlatform || item.template.type == ItemTemplateType.Backpack || item.template.type == ItemTemplateType.AvatarAndDisguise || item.template.type == ItemTemplateType.UpgradeStone || item.template.type == ItemTemplateType.DragonBall || item.template.type == ItemTemplateType.ConsumableBuffItem || (item.template.type == ItemTemplateType.Miscellaneous && item.template.id != 521);
        static bool ShouldKeepItem(Item item) => ShouldMoveItemToChest(item) || item.template.type == ItemTemplateType.SenzuBean || item.template.id == 521;

        internal static void SetState(bool value)
        {
            isEnabled = value;
            if (isEnabled)
                steps = 0;
        }
    }
}