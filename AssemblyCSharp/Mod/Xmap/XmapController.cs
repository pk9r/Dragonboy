using Mod.ModHelper;
using Mod.ModHelper.Menu;
using System.Collections.Generic;
using System.Threading;

namespace Mod.Xmap
{
    public class XmapController : ThreadActionUpdate<XmapController>
    {
        public override int Interval => 100;

        private static int idMapEnd;
        private static Way way;
        private static int indexWay;
        private static bool isNextMapFailed;

        protected override void update()
        {
            if (way == null)
            {
                if (!isNextMapFailed)
                {
                    string mapName = TileMap.mapNames[idMapEnd];
                    MainThreadDispatcher.dispatcher(() =>
                        GameScr.info1.addInfo($"Đi đến: {mapName}", 0));
                }

                XmapAlgorithm.xmapData = new XmapData();
                XmapAlgorithm.xmapData.loadLinkMapCapsule();
                way = XmapAlgorithm.findWay(TileMap.mapID, idMapEnd);
                indexWay = 0;

                if (way == null)
                {
                    MainThreadDispatcher.dispatcher(() =>
                        GameScr.info1.addInfo("Không thể tìm thấy đường đi", 0));
                    finishXmap();
                    return;
                }
            }

            if (TileMap.mapID == way[way.Count - 1].mapId && !Utilities.isMyCharDied())
            {
                MainThreadDispatcher.dispatcher(() =>
                    GameScr.info1.addInfo("Xmap by Phucprotein", 0));
                finishXmap();
                return;
            }

            if (TileMap.mapID == way[indexWay].mapId)
            {
                if (Utilities.isMyCharDied())
                {
                    Service.gI().returnTownFromDead();
                    isNextMapFailed = true;
                    way = null;
                }
                else if (Utilities.canNextMap())
                {
                    MainThreadDispatcher.dispatcher(() =>
                        Pk9rXmap.nextMap(way[indexWay + 1]));
                }
                Thread.Sleep(500);
                return;
            }
            else if (TileMap.mapID == way[indexWay + 1].mapId)
            {
                indexWay++;
                return;
            }
            else
            {
                isNextMapFailed = true;
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
            Pk9rXmap.IsMapTransAsXmap = true;
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

        public static void startRunToMapId(int idMap)
        {
            idMapEnd = idMap;
            gI.toggle(true);
        }

        public static void finishXmap()
        {
            way = null;
            isNextMapFailed = false;
            gI.toggle(false);
        }
    }
}
