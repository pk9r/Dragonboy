using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mod.Xmap
{
    public class XmapAlgorithm
    {
        public static XmapData xmapData;

        private static bool[] blackMaps;
        private static List<int> blackMapsL;

        private static Way bestWay;

        public static Way findWay(int mapStart, int mapEnd)
        {
            blackMaps = new bool[TileMap.mapNames.Length];
            blackMapsL = new();
            bestWay = null;

            LogMod.writeLine($"[xmap][dbg] Bắt đầu tìm đường từ {mapStart} tới {mapEnd}");
            if (xmapData == null)
            {
                throw new Exception("xmapData is null");
            }

            var wayPassed = new Way() { new(mapStart, TypeMapNext.AutoWaypoint, null) };
            return findWay(mapEnd, wayPassed);
        }

        private static Way findWay(int mapEnd, Way wayPassed)
        {
            LogMod.writeLine($"[xmap][dbg] wayPassed: {JsonMapper.ToJson(wayPassed.Select(x => x.mapId).ToList())}");
            LogMod.writeLine($"[xmap][dbg] blackMapsL: {JsonMapper.ToJson(blackMapsL.ToList())}");

            if (!isWayBetter(wayPassed, bestWay))
                return null;

            var cMap = wayPassed[wayPassed.Count - 1];

            if (cMap.mapId == mapEnd)
                return wayPassed;

            var mapNexts = xmapData.links[cMap.mapId];
            if (mapNexts == null)
            {
                return null;
            }

            //var ways = new List<Way>();
            foreach (var map in mapNexts)
            {
                if (!wayPassed.Exists(x => x.mapId == map.mapId))
                {
                    if (!blackMaps[map.mapId])
                    {
                        var wayPassedNext = new Way(wayPassed) { map };
                        var wayContinue = findWay(mapEnd, wayPassedNext);
                        if (wayContinue != null)
                        {
                            if (isWayBetter(wayContinue, bestWay)) bestWay = wayContinue;
                        }
                        else
                        {
                            blackMaps[map.mapId] = true;
                            blackMapsL.Add(map.mapId);
                        }
                    }
                }
            }

            //var bestWay = getBestWay(ways);
            return bestWay;
        }

        private static Way getBestWay(List<Way> ways)
        {
            if (ways.Count == 0)
                return null;

            var bestWay = ways[0];
            for (int i = 1; i < ways.Count; i++)
                if (isWayBetter(ways[i], bestWay))
                    bestWay = ways[i];
            return bestWay;
        }

        /// <summary>
        /// Kiểm tra way1 có tốt hơn way2 không
        /// </summary>
        /// <param name="way1"></param>
        /// <param name="way2"></param>
        /// <returns></returns>
        private static bool isWayBetter(Way way1, Way way2)
        {
            if (way1 == null) return false;
            if (way2 == null) return true;

            bool isBadWay1 = isBadWay(way1);
            bool isBadWay2 = isBadWay(way2);

            if (isBadWay1 && !isBadWay2)
                return false;

            if (!isBadWay1 && isBadWay2)
                return true;

            if (way1.Count >= way2.Count)
                return false;

            return true;
        }

        private static bool isBadWay(Way way)
        {
            return isWayGoFutureAndBack(way);
        }

        private static bool isWayGoFutureAndBack(Way way)
        {
            List<int> mapsGoFuture = new List<int>() { 27, 28, 29 };
            for (int i = 1; i < way.Count - 1; i++)
                if (way[i].mapId == 102 && way[i + 1].mapId == 24 && mapsGoFuture.Contains(way[i - 1].mapId))
                    return true;
            return false;
        }
    }
}
