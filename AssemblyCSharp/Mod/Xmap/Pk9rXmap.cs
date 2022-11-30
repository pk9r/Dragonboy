using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using System.Collections.Generic;

namespace Mod.Xmap
{
    public class Pk9rXmap
    {
        public static bool isMapTransAsXmap = false;
        public static bool isShowPanelMapTrans = true;
        public static bool isUseCapsuleNormal = false;
        public static bool isUseCapsuleVip = true;

        [ChatCommand("csdb")]
        public static void toggleUseCapsuleVip()
        {
            isUseCapsuleVip = !isUseCapsuleVip;
            GameScr.info1.addInfo("Sử dụng capsule đặc biệt Xmap: " + (isUseCapsuleVip ? "Bật" : "Tắt"), 0);
        }

        [ChatCommand("xmp")]
        public static void toggleXmap(int mapId)
        {
            if (XmapController.gI.IsActing)
            {
                XmapController.finishXmap();
                GameScr.info1.addInfo("Đã huỷ Xmap", 0);
            }
            else
            {
                XmapController.startRunToMapId(mapId);
            }
        }

        [ChatCommand("xmp"), HotkeyCommand('x')]
        public static void toggleXmap()
        {
            if (XmapController.gI.IsActing)
            {
                XmapController.finishXmap();
                GameScr.info1.addInfo("Đã huỷ Xmap", 0);
            }
            else
            {
                ShowXmapMenu();
            }
        }

        public static void ShowXmapMenu()
        {
            XmapData.loadGroupMapsFromFile("TextData\\GroupMapsXmap.txt");
            OpenMenu.start(new(menuItems =>
            {
                foreach (var groupMap in XmapData.groups)
                    menuItems.Add(new(groupMap.nameGroup, new(() =>
                    {
                        ShowXmapPanel(groupMap.maps);
                        Char.chatPopup = null;
                    })));
            }));
            ChatPopup.addChatPopup($"XmapNRO by Phucprotein\nMap hiện tại: {TileMap.mapName}, ID: {TileMap.mapID}\nVui lòng chọn nơi muốn đến", 100000, new Npc(5, 0, -100, 100, 5, Utilities.ID_NPC_MOD_FACE));
        }

        public static void ShowXmapPanel(List<int> maps)
        {
            isMapTransAsXmap = true;
            int len = maps.Count;
            GameCanvas.panel.mapNames = new string[len];
            GameCanvas.panel.planetNames = new string[len];
            for (int i = 0; i < len; i++)
            {
                var mapId = maps[i];
                var nameMap = TileMap.mapNames[maps[i]];
                GameCanvas.panel.mapNames[i] = $"{mapId}: {nameMap}";
                GameCanvas.panel.planetNames[i] = "Xmap by Phucprotein";
            }
            GameCanvas.panel.setTypeMapTrans();
            GameCanvas.panel.show();
        }

        public static void Info(string text)
        {
            if (XmapController.gI.IsActing)
            {
                var keywords = new List<string>
                {
                    "Bạn chưa thể đến khu vực này",
                    "Bang hội phải có từ 5 thành viên mới được tham gia",
                    "Chỉ tiếp các bang hội, miễn tiếp khách vãng lai",
                    "Gia nhập bang hội trên 2 ngày mới được tham gia",
                };

                if (keywords.Contains(text))
                {
                    XmapController.finishXmap();
                    GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                }
                else if (text.Equals("Có lỗi xảy ra vui lòng thử lại sau."))
                {
                    Utilities.teleportMyChar(XmapUtils.getX(2), XmapUtils.getY(2));
                }
            }
        }

        public static void SelectMapTrans(int selected)
        {
            if (isMapTransAsXmap)
            {
                InfoDlg.hide();
                string mapName = GameCanvas.panel.mapNames[selected];
                int idMap = getMapIdFromPanelXmap(mapName);
                XmapController.startRunToMapId(idMap);
                return;
            }
            Utilities.mapCapsuleReturn = TileMap.mapID;
            Service.gI().requestMapSelect(selected);
        }

        public static void ShowPanelMapTrans()
        {
            isMapTransAsXmap = false;
            if (isShowPanelMapTrans)
            {
                GameCanvas.panel.setTypeMapTrans();
                GameCanvas.panel.show();
                return;
            }
            isShowPanelMapTrans = true;
        }

        public static void FixBlackScreen()
        {
            Controller.gI().loadCurrMap(0);
            Service.gI().finishLoadMap();
            Char.isLoadingMap = false;
        }

        public static bool canUseCapsuleNormal()
        {
            return isUseCapsuleNormal && !Utilities.isMyCharDied() && Utilities.hasItemCapsuleNormal();
        }

        public static bool canUseCapsuleVip()
        {
            return isUseCapsuleVip && !Utilities.isMyCharDied() && Utilities.hasItemCapsuleVip();
        }

        public static int getMapIdFromPanelXmap(string mapName)
        {
            return int.Parse(mapName.Split(':')[0]);
        }

        public static bool isWaitInfoMapTrans()
        {
            return !isShowPanelMapTrans;
        }

        public static void nextMap(MapNext mapNext)
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

        public static void nextMapAutoWaypoint(MapNext mapNext)
        {
            var waypoint = Utilities.findWaypoint(mapNext.mapId);
            Utilities.changeMap(waypoint);
        }

        public static void nextMapNpcMenu(MapNext mapNext)
        {
            var idNpc = mapNext.info[0];
            Service.gI().openMenu(idNpc);
            for (int i = 1; i < mapNext.info.Length; i++)
            {
                int select = mapNext.info[i];
                Service.gI().confirmMenu((short)idNpc, (sbyte)select);
            }
        }

        public static void nextMapNpcPanel(MapNext mapNext)
        {
            var idNpc = mapNext.info[0];
            var selectMenu = mapNext.info[1];
            var selectPanel = mapNext.info[2];
            Service.gI().openMenu(idNpc);
            Service.gI().confirmMenu((short)idNpc, (sbyte)selectMenu);
            Service.gI().requestMapSelect(selectPanel);
        }

        public static void nextMapPosition(MapNext mapNext)
        {
            var xPos = mapNext.info[0];
            var yPos = mapNext.info[1];
            Utilities.teleportMyChar(xPos, yPos);
            Service.gI().requestChangeMap();
            Service.gI().getMapOffline();
        }

        public static void nextMapCapsule(MapNext mapNext)
        {
            Utilities.mapCapsuleReturn = TileMap.mapID;
            var select = mapNext.info[0];
            Service.gI().requestMapSelect(select);
        }
    }
}
