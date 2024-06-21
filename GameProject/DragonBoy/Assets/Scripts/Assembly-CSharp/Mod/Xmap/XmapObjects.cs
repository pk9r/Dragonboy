using System;
using System.Collections.Generic;

namespace Mod.Xmap
{
    internal struct MapNext
    {
        internal int mapStart;
        internal int to;
        internal TypeMapNext type;
        internal int[] info;

        internal MapNext(int mapStart, int to, TypeMapNext type, int[] info)
        {
            this.mapStart = mapStart;
            this.to = to;
            this.type = type;
            this.info = info;
        }
    }

    internal struct GroupMap
    {
        internal string[] names;

        internal List<int> maps;

        internal GroupMap(string[] nameGroup, List<int> maps)
        {
            names = nameGroup;
            this.maps = maps;
        }
    }

    internal enum TypeMapNext
    {
        None = -1,
        AutoWaypoint,
        NpcMenu,
        NpcPanel,
        Position,
        Capsule
    }

    internal class Tile
    {
        internal int x;
        internal int y;

        internal Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}