using System.Collections.Generic;

namespace Mod.Xmap
{
    public struct MapNext
    {
        public int mapId;

        public TypeMapNext type;

        public int[] info;

        public MapNext(int mapId, TypeMapNext type, int[] info)
        {
            this.mapId = mapId;
            this.type = type;
            this.info = info;
        }
    }

    public struct GroupMap
    {
        public string nameGroup;

        public List<int> maps;

        public GroupMap(string nameGroup, List<int> maps)
        {
            this.nameGroup = nameGroup;
            this.maps = maps;
        }
    }

    public class Way : List<MapNext>
    {
        public Way() : base() { }

        public Way(IEnumerable<MapNext> collection) : base(collection) { }
    }

    public enum TypeMapNext
    {
        AutoWaypoint,
        NpcMenu,
        NpcPanel,
        Position,
        Capsule
    }
}