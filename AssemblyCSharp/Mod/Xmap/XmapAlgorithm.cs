using System;
using System.Collections.Generic;

namespace Mod.Xmap
{
    public class XmapAlgorithm
    {
        public static XmapData xmapData;

        public static List<MapNext> findWay(int mapStart, int mapEnd)
        {
            LogMod.writeLine($"[xmap][dbg] Bắt đầu tìm đường từ {mapStart} tới {mapEnd}");
            if (xmapData == null)
            {
                throw new Exception("xmapData is null");
            }

            int length = xmapData.links.Length;
            var prev = new MapNext[length];
            var visited = new bool[length];
            var dist = new int[length];
            for (int i = 0; i < length; i++)
                dist[i] = int.MaxValue;
            dist[mapStart] = 0;

            for (int _ = 0; _ < length; _++)
            {
                var cmap = -1;
                for (int i = 0; i < length; i++)
                    if (!visited[i] && (cmap == -1 || dist[i] < dist[cmap]))
                        cmap = i;

                if (cmap == -1)
                    break;

                var neighbors = xmapData.links[cmap];
                var count = neighbors.Count;
                for (int i = 0; i < count; i++)
                {
                    var mapNext = neighbors[i];
                    var cost = 1;
                    if (mapNext.type == TypeMapNext.NpcMenu && mapNext.info[0] == 38)
                        cost = 100;

                    int tentative = dist[cmap] + cost;
                    if (tentative < dist[mapNext.to])
                    {
                        dist[mapNext.to] = tentative;
                        prev[mapNext.to] = mapNext;
                    }
                }

                visited[cmap] = true;
            }

            var way = new List<MapNext>();
            var index = mapEnd;
            while (index != mapStart)
            {
                way.Add(prev[index]);
                index = prev[index].mapStart;
            }
            way.Reverse();

            if (way[0].mapStart == mapStart)
                return way;
            return null;
        }
    }
}
