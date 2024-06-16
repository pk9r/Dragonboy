using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.R;
using System;
using System.Linq;
using System.Threading;

namespace Mod.Xmap
{
    internal class Pk9rXmap
    {
        class XmapChatable: IChatable
        {
            public void onChatFromMe(string text, string to)
            {
                if (int.TryParse(text, out int timeout))
                {
                    if (timeout < 10 || timeout > 300)
                    {
                        GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, 10, 300) + '!');
                        return;
                    }
                    else
                    {
                        aStarTimeout = timeout;
                        GameScr.info1.addInfo(Strings.xmapTimeout + ": " + aStarTimeout, 0);
                    }
                }
                else
                {
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
                    return;
                }
                onCancelChat();
            }

            public void onCancelChat() => ChatTextField.gI().ResetTF();
        }

        internal static bool isUseCapsuleNormal = false;
        internal static bool isUseCapsuleVip = true;
        internal static bool isXmapAStar = false;
        static bool isChangingMap;
        static bool isMovingMyChar;
        internal static int aStarTimeout = 30;

        static Random random = new Random();

        [ChatCommand("xcsdb")]
        internal static void ToggleUseCapsuleVip()
        {
            isUseCapsuleVip = !isUseCapsuleVip;
            GameScr.info1.addInfo(Strings.xmapUseSpecialCapsule + ": " + Strings.OnOffStatus(isUseCapsuleVip), 0);
        }

        [ChatCommand("xcsb")]
        internal static void ToggleUseCapsuleNormal()
        {
            isUseCapsuleNormal = !isUseCapsuleNormal;
            GameScr.info1.addInfo(Strings.xmapUseNormalCapsule + ": " + Strings.OnOffStatus(isUseCapsuleNormal), 0);
        }

        [ChatCommand("xmp"), HotkeyCommand('x')]
        internal static void ShowXmapMenu()
        {
            if (XmapController.gI.IsActing)
            {
                LogMod.writeLine("[xmap][info] Người chơi yêu cầu hủy xmap");

                XmapController.finishXmap();
                GameScr.info1.addInfo(Strings.xmapCanceled, 0);
                return;
            }

            XmapData.LoadGroupMaps();

            new MenuBuilder()
                .setChatPopup(string.Format(Strings.xmapChatPopup, TileMap.mapName, TileMap.mapID))
                .map(XmapData.groups, groupMap =>
                {
                    string caption = groupMap.names[groupMap.names.Length - 1];
                    if (groupMap.names.Length > mResources.language)
                        caption = groupMap.names[mResources.language];
                    return new MenuItem(caption, new MenuAction(() =>
                    {
                        XmapPanel.Show(groupMap.maps);
                    }));
                })
                .addItem(Strings.settings, new MenuAction(ShowXmapSettings))
                .start();
        }

        static void ShowXmapSettings()
        {
            new MenuBuilder()
                .setChatPopup(string.Format(Strings.xmapChatPopup, TileMap.mapName, TileMap.mapID))
                .addItem(Strings.xmapUseNormalCapsule + ": " + Strings.OnOffStatus(isUseCapsuleNormal), new MenuAction(ToggleUseCapsuleNormal))
                .addItem(Strings.xmapUseSpecialCapsule + ": " + Strings.OnOffStatus(isUseCapsuleVip), new MenuAction(ToggleUseCapsuleVip))
                .addItem(Strings.xmapUseAStar + ": " + Strings.OnOffStatus(isXmapAStar), new MenuAction(() =>
                {
                    isXmapAStar = !isXmapAStar;
                    GameScr.info1.addInfo(Strings.xmapUseAStar + ": " + Strings.OnOffStatus(isXmapAStar), 0);
                }))
                .addItem(Strings.xmapEditTimeout, new MenuAction(() =>
                {
                    ChatTextField.gI().strChat = Strings.timeout;
                    ChatTextField.gI().tfChat.name = Strings.timeout + " (10-300s)";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(new XmapChatable(), string.Empty);
                    ChatTextField.gI().tfChat.setText(aStarTimeout.ToString());
                }))
                .start();
        }

        internal static void Info(string text)
        {
            if (XmapController.gI.IsActing)
            {
                if (LocalizedString.xmapCantGoHereKeywords.Any(lS => lS == text))
                {
                    XmapController.finishXmap();
                    GameScr.info1.addInfo(Strings.xmapCanceled, 0);
                }
                else if (text == LocalizedString.errorOccurred)
                    MoveMyChar(XmapUtils.getX(2), XmapUtils.getY(2));
            }
        }

        internal static void FixBlackScreen()
        {
            Controller.gI().loadCurrMap(0);
            Service.gI().finishLoadMap();
            Char.isLoadingMap = false;
        }

        internal static bool CanUseCapsuleNormal()
        {
            return isUseCapsuleNormal && !Char.myCharz().IsCharDead() && XmapUtils.hasItemCapsuleNormal();
        }

        internal static bool CanUseCapsuleVip()
        {
            return isUseCapsuleVip && !Char.myCharz().IsCharDead() && XmapUtils.hasItemCapsuleVip();
        }

        internal static int GetMapIdFromPanelXmap(string mapName)
        {
            return int.Parse(mapName.Split(':')[0]);
        }

        internal static void NextMap(MapNext mapNext)
        {
            switch (mapNext.type)
            {
                case TypeMapNext.AutoWaypoint:
                    NextMapAutoWaypoint(mapNext);
                    break;
                case TypeMapNext.NpcMenu:
                    NextMapNpcMenu(mapNext);
                    break;
                case TypeMapNext.NpcPanel:
                    NextMapNpcPanel(mapNext);
                    break;
                case TypeMapNext.Position:
                    NextMapPosition(mapNext);
                    break;
                case TypeMapNext.Capsule:
                    NextMapCapsule(mapNext);
                    break;
            }
        }

        internal static void NextMapAutoWaypoint(MapNext mapNext)
        {
            var waypoint = XmapUtils.findWaypoint(mapNext.to);
            ChangeMap(waypoint);
        }

        internal static void NextMapNpcMenu(MapNext mapNext)
        {
            var npcId = mapNext.info[0];
            if (npcId == 38)
            {
                var flag = false;
                int vNpcSize = GameScr.vNpc.size();
                for (int i = 0; i < vNpcSize; i++)
                {
                    var npc = (Npc)GameScr.vNpc.elementAt(i);
                    if (npc.template.npcTemplateId == npcId)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Waypoint waypoint;
                    if (TileMap.mapID == 27 || TileMap.mapID == 29)
                        waypoint = XmapUtils.findWaypoint(28);
                    else
                    {
                        if (random.Next(27, 29) == 27)
                            waypoint = XmapUtils.findWaypoint(27);
                        else
                            waypoint = XmapUtils.findWaypoint(29);
                    }

                    ChangeMap(waypoint);
                    return;
                }
            }
            Service.gI().openMenu(npcId);
            for (int i = 1; i < mapNext.info.Length; i++)
            {
                int select = mapNext.info[i];
                Service.gI().confirmMenu((short)npcId, (sbyte)select);
            }
            Char.chatPopup = null;
        }

        internal static void NextMapNpcPanel(MapNext mapNext)
        {
            var idNpc = mapNext.info[0];
            var selectMenu = mapNext.info[1];
            var selectPanel = mapNext.info[2];
            Service.gI().openMenu(idNpc);
            Service.gI().confirmMenu((short)idNpc, (sbyte)selectMenu);
            Service.gI().requestMapSelect(selectPanel);
        }

        internal static void NextMapPosition(MapNext mapNext)
        {
            var xPos = mapNext.info[0];
            var yPos = mapNext.info[1];
            MoveMyChar(xPos, yPos);
            if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, xPos, yPos) <= TileMap.size)
            {
                Service.gI().requestChangeMap();
                Service.gI().getMapOffline();
            }
        }

        internal static void NextMapCapsule(MapNext mapNext)
        {
            XmapUtils.mapCapsuleReturn = TileMap.mapID;
            var select = mapNext.info[0];
            Service.gI().requestMapSelect(select);
        }

        static void MoveMyChar(int x, int y)
        {
            if (isXmapAStar)
            {
                if (isMovingMyChar)
                    return;
                isMovingMyChar = true;
                new Thread(() =>
                {
                    try
                    {
                        long startTime = mSystem.currentTimeMillis();
                        int size = TileMap.size;
                        Tile start = new Tile(Char.myCharz().cx / size, Char.myCharz().cy / size - 1);
                        Tile destination = new Tile(x / size, y / size);
                        var path = XmapAStar.FindPath(start, destination);
                        if (path.Count == 0)
                        {
                            XmapController.finishXmap();
                            GameScr.info1.addInfo(Strings.xmapCantFindWay, 0);
                            isMovingMyChar = false;
                        }
                        while (path.Count > 0)
                        {
                            if (!XmapController.gI.IsActing)
                                break;
                            if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                            {
                                isMovingMyChar = false;
                                return;
                            }
                            var tile = path.Pop();
                            int sleep = 0;
                            int xEnd = tile.x * size;
                            int yEnd = tile.y * size;
                            Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                            while (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, xEnd, yEnd) > size * 2)
                            {
                                if (!XmapController.gI.IsActing)
                                    break;
                                if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                                {
                                    isMovingMyChar = false;
                                    return;
                                }
                                if (sleep % 500 == 0)
                                {
                                    if (sleep >= 2000)
                                    {
                                        xEnd = tile.x * size + random.Next(size / -2, size / 2);
                                        yEnd = tile.y * size + random.Next(size / -2, size / 2);
                                        sleep = 0;
                                    }
                                    if (Char.myCharz().currentMovePoint == null || Char.myCharz().currentMovePoint.xEnd != xEnd || Char.myCharz().currentMovePoint.yEnd != tile.y * size + yEnd)
                                        Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                                }
                                Thread.Sleep(100);
                                sleep += 100;
                            }
                        }
                        Thread.Sleep(500);
                    }
                    catch (Exception ex) { UnityEngine.Debug.LogException(ex); }
                    isMovingMyChar = false;
                })
                { IsBackground = true }.Start();
            }
            else
                Utils.TeleportMyChar(x, y);
        }

        static void ChangeMap(Waypoint waypoint)
        {
            if (isXmapAStar)
            {
                if (isChangingMap)
                    return;
                isChangingMap = true;
                new Thread(() =>
                {
                    try
                    {
                        long startTime = mSystem.currentTimeMillis();
                        int size = TileMap.size;
                        Tile start = new Tile(Char.myCharz().cx / size, Char.myCharz().cy / size - 1);
                        Tile destination = new Tile(waypoint.GetXInsideMap() / size, waypoint.minY / size);
                        var path = XmapAStar.FindPath(start, destination);
                        if (path.Count == 0)
                        {
                            XmapController.finishXmap();
                            GameScr.info1.addInfo(Strings.xmapCantFindWay, 0);
                            isChangingMap = false;
                        }
                        while (path.Count > 0)
                        {
                            if (!XmapController.gI.IsActing)
                                break;
                            if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                            {
                                Utils.ChangeMap(waypoint);
                                isChangingMap = false;
                                return;
                            }
                            var tile = path.Pop();
                            int sleep = 0;
                            int xEnd = tile.x * size;
                            int yEnd = tile.y * size;
                            Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                            while (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, xEnd, yEnd) > size * 2)
                            {
                                if (!XmapController.gI.IsActing)
                                    break;
                                if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                                {
                                    Utils.ChangeMap(waypoint);
                                    isChangingMap = false;
                                    return;
                                }
                                if (sleep % 500 == 0)
                                {
                                    if (sleep >= 2000)
                                    {
                                        xEnd = tile.x * size + random.Next(size / -2, size / 2);
                                        yEnd = tile.y * size + random.Next(size / -2, size / 2);
                                        sleep = 0;
                                    }
                                    if (Char.myCharz().currentMovePoint == null || Char.myCharz().currentMovePoint.xEnd != xEnd || Char.myCharz().currentMovePoint.yEnd != tile.y * size + yEnd)
                                        Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                                }
                                Thread.Sleep(100);
                                sleep += 100;
                            }
                        }
                        waypoint.popup.command.performAction();
                        Thread.Sleep(500);
                        //Utils.requestChangeMap(waypoint);
                    }
                    catch (Exception ex) { UnityEngine.Debug.LogException(ex); }
                    isChangingMap = false;
                })
                { IsBackground = true }.Start();
            }
            else
                Utils.ChangeMap(waypoint);
        }
    }
}
