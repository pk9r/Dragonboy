using Mod.ModHelper;
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
