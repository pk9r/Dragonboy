using System.Collections.Generic;

namespace Mod.Xmap
{
    public struct MapNext
    {
        public int mapStart;
        public int to;
        public TypeMapNext type;
        public int[] info;

        public MapNext(int mapStart, int to, TypeMapNext type, int[] info)
        {
            this.mapStart = mapStart;
            this.to = to;
            this.type = type;
            this.info = info;
        }
    }

    public struct GroupMap
    {
        public string[] names;

        public List<int> maps;

        public GroupMap(string[] nameGroup, List<int> maps)
        {
            names = nameGroup;
            this.maps = maps;
        }
    }

    public enum TypeMapNext
    {
        None = -1,
        AutoWaypoint,
        NpcMenu,
        NpcPanel,
        Position,
        Capsule
    }
}