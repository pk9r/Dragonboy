using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.R;
using System.Collections.Generic;
using System.Linq;

namespace Mod.Xmap
{
    internal class Pk9rXmap
    {
        internal static bool isUseCapsuleNormal = false;
        internal static bool isUseCapsuleVip = true;

        [ChatCommand("xcsdb")]
        internal static void toggleUseCapsuleVip()
        {
            isUseCapsuleVip = !isUseCapsuleVip;
            GameScr.info1.addInfo(Strings.xmapUseSpecialCapsule + ": " + Strings.OnOffStatus(isUseCapsuleVip), 0);
        }

        [ChatCommand("xcsb")]
        internal static void toggleUseCapsuleNormal()
        {
            isUseCapsuleNormal = !isUseCapsuleNormal;
            GameScr.info1.addInfo(Strings.xmapUseNormalCapsule + ": " + Strings.OnOffStatus(isUseCapsuleNormal), 0);
        }

        [ChatCommand("xmp"), HotkeyCommand('x')]
        internal static void showXmapMenu()
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
                    Utils.TeleportMyChar(XmapUtils.getX(2), XmapUtils.getY(2));
            }
        }

        internal static void fixBlackScreen()
        {
            Controller.gI().loadCurrMap(0);
            Service.gI().finishLoadMap();
            Char.isLoadingMap = false;
        }

        internal static bool canUseCapsuleNormal()
        {
            return isUseCapsuleNormal && !Char.myCharz().isCharDead() && XmapUtils.hasItemCapsuleNormal();
        }

        internal static bool canUseCapsuleVip()
        {
            return isUseCapsuleVip && !Char.myCharz().isCharDead() && XmapUtils.hasItemCapsuleVip();
        }

        internal static int GetMapIdFromPanelXmap(string mapName)
        {
            return int.Parse(mapName.Split(':')[0]);
        }

        internal static void nextMap(MapNext mapNext)
        {
            switch (mapNext.type)
            {
                case TypeMapNext.AutoWaypoint:
                    nextMapAutoWaypoint(mapNext);
                    break;
                case TypeMapNext.NpcMenu:
                    nextMapNpcMenu(mapNext);
                    break;
                case TypeMapNext.NpcPanel:
                    nextMapNpcPanel(mapNext);
                    break;
                case TypeMapNext.Position:
                    nextMapPosition(mapNext);
                    break;
                case TypeMapNext.Capsule:
                    nextMapCapsule(mapNext);
                    break;
            }
        }

        internal static void nextMapAutoWaypoint(MapNext mapNext)
        {
            var waypoint = XmapUtils.findWaypoint(mapNext.to);
            Utils.ChangeMap(waypoint);
        }

        internal static void nextMapNpcMenu(MapNext mapNext)
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
                        if (Utils.random.Next(27, 29) == 27)
                            waypoint = XmapUtils.findWaypoint(27);
                        else
                            waypoint = XmapUtils.findWaypoint(29);
                    }

                    Utils.ChangeMap(waypoint);
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

        internal static void nextMapNpcPanel(MapNext mapNext)
        {
            var idNpc = mapNext.info[0];
            var selectMenu = mapNext.info[1];
            var selectPanel = mapNext.info[2];
            Service.gI().openMenu(idNpc);
            Service.gI().confirmMenu((short)idNpc, (sbyte)selectMenu);
            Service.gI().requestMapSelect(selectPanel);
        }

        internal static void nextMapPosition(MapNext mapNext)
        {
            var xPos = mapNext.info[0];
            var yPos = mapNext.info[1];
            Utils.TeleportMyChar(xPos, yPos);
            Service.gI().requestChangeMap();
            Service.gI().getMapOffline();
        }

        internal static void nextMapCapsule(MapNext mapNext)
        {
            XmapUtils.mapCapsuleReturn = TileMap.mapID;
            var select = mapNext.info[0];
            Service.gI().requestMapSelect(select);
        }
    }
}
