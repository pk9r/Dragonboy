namespace AssemblyCSharp.Mod.Xmap
{
	public struct MapNext
	{
		public int MapID;

		public TypeMapNext Type;

		public int[] Info;

		public MapNext(int mapID, TypeMapNext type, int[] info)
		{
			MapID = mapID;
			Type = type;
			Info = info;
		}
	}
}
