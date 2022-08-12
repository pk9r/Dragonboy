using System.Collections.Generic;

namespace Mod.Xmap
{
    public class XmapAlgorithm
    {
        public static List<int> FindWay(int idMapStart, int idMapEnd)
        {
            List<int> wayPassed = GetWayPassedStart(idMapStart);
            List<int> way = FindWay(idMapEnd, wayPassed);
            return way;
        }

        private static List<int> FindWay(int idMapEnd, List<int> wayPassed)
        {
            int idMapLast = wayPassed[wayPassed.Count - 1];

            if (idMapLast == idMapEnd)
                return wayPassed;
            if (!XmapData.Instance().CanGetMapNexts(idMapLast))
                return null;

            List<List<int>> ways = new List<List<int>>();
            List<MapNext> mapNexts = XmapData.Instance().GetMapNexts(idMapLast);
            foreach (MapNext map in mapNexts)
            {
                List<int> wayContinue = null;
                if (!wayPassed.Contains(map.MapID))
                {
                    var wayPassedNext = GetWayPassedNext(wayPassed, map.MapID);
                    wayContinue = FindWay(idMapEnd, wayPassedNext);
                }

                if (wayContinue != null)
                    ways.Add(wayContinue);
            }
            List<int> bestWay = GetBestWay(ways);
            return bestWay;
        }

        private static List<int> GetBestWay(List<List<int>> ways)
        {
            if (ways.Count == 0)
                return null;

            List<int> bestWay = ways[0];
            for (int i = 1; i < ways.Count; i++)
                if (IsWayBetter(ways[i], bestWay))
                    bestWay = ways[i];
            return bestWay;
        }

        private static List<int> GetWayPassedStart(int idMapStart)
        {
            List<int> wayPassed = new List<int>()
            {
                idMapStart
            };
            return wayPassed;
        }

        private static List<int> GetWayPassedNext(List<int> wayPassed, int idMapNext)
        {
            List<int> wayNext = new List<int>(wayPassed)
            {
                idMapNext
            };
            return wayNext;
        }

        /// <summary>
        /// Kiểm tra way1 có tốt hơn way2 không
        /// </summary>
        /// <param name="way1"></param>
        /// <param name="way2"></param>
        /// <returns></returns>
        private static bool IsWayBetter(List<int> way1, List<int> way2)
        {
            bool flag1 = IsBadWay(way1);
            bool flag2 = IsBadWay(way2);
            if (flag1 && !flag2)
                return false;
            if (!flag1 && flag2)
                return true;
            if (way1.Count >= way2.Count)
                return false;
            return true;
        }

        private static bool IsBadWay(List<int> way)
        {
            return IsWayGoFutureAndBack(way);
        }

        private static bool IsWayGoFutureAndBack(List<int> way)
        {
            List<int> mapsGoFuture = new List<int>() { 27, 28, 29 };
            for (int i = 1; i < way.Count - 1; i++)
                if (way[i] == 102 && way[i + 1] == 24 && mapsGoFuture.Contains(way[i - 1]))
                    return true;
            return false;
        }
    }
}
