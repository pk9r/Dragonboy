using System;
using System.Collections.Generic;

namespace Mod.Xmap
{
    public class XmapAlgorithm
    {
        public static XmapData xmapData;

        public static Way findWay(int idMapStart, int idMapEnd)
        {
            if (xmapData == null)
            {
                throw new Exception("xmapData is null");
            }

            var wayPassed = new Way() { new(idMapStart, TypeMapNext.AutoWaypoint, null) };
            return findWay(idMapEnd, wayPassed);
        }

        private static Way findWay(int mapIdEnd, Way wayPassed)
        {
            var cMap = wayPassed[wayPassed.Count - 1];

            if (cMap.mapId == mapIdEnd)
                return wayPassed;

            var mapNexts = xmapData.getMapNexts(cMap.mapId);
            if (mapNexts == null)
                return null;

            var ways = new List<Way>();
            foreach (var map in mapNexts)
            {
                if (!wayPassed.Exists(x => x.mapId == map.mapId))
                {
                    var wayPassedNext = new Way(wayPassed) { map };
                    var wayContinue = findWay(mapIdEnd, wayPassedNext);
                    if (wayContinue != null)
                        ways.Add(wayContinue);
                }
            }

            var bestWay = getBestWay(ways);
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
