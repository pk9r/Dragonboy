using System.Collections.Generic;

namespace AssemblyCSharp.Mod.Xmap
{
	public class XmapAlgorithm
	{
		public static List<int> FindWay(int idMapStart, int idMapEnd)
		{
			List<int> wayPassedStart = GetWayPassedStart(idMapStart);
			return FindWay(idMapEnd, wayPassedStart);
		}

		private static List<int> FindWay(int idMapEnd, List<int> wayPassed)
		{
			int num = wayPassed[wayPassed.Count - 1];
			if (num == idMapEnd)
			{
				return wayPassed;
			}
			if (!XmapData.Instance().CanGetMapNexts(num))
			{
				return null;
			}
			List<List<int>> list = new List<List<int>>();
			foreach (MapNext mapNext in XmapData.Instance().GetMapNexts(num))
			{
				List<int> list2 = null;
				if (!wayPassed.Contains(mapNext.MapID))
				{
					List<int> wayPassedNext = GetWayPassedNext(wayPassed, mapNext.MapID);
					list2 = FindWay(idMapEnd, wayPassedNext);
				}
				if (list2 != null)
				{
					list.Add(list2);
				}
			}
			return GetBestWay(list);
		}

		private static List<int> GetBestWay(List<List<int>> ways)
		{
			if (ways.Count == 0)
			{
				return null;
			}
			List<int> list = ways[0];
			for (int i = 1; i < ways.Count; i++)
			{
				if (IsWayBetter(ways[i], list))
				{
					list = ways[i];
				}
			}
			return list;
		}

		private static List<int> GetWayPassedStart(int idMapStart)
		{
			return new List<int> { idMapStart };
		}

		private static List<int> GetWayPassedNext(List<int> wayPassed, int idMapNext)
		{
			return new List<int>(wayPassed) { idMapNext };
		}

		private static bool IsWayBetter(List<int> way1, List<int> way2)
		{
			bool flag = IsBadWay(way1);
			bool flag2 = IsBadWay(way2);
			if (!flag || flag2)
			{
				if (!(!flag && flag2))
				{
					return way1.Count < way2.Count;
				}
				return true;
			}
			return false;
		}

		private static bool IsBadWay(List<int> way)
		{
			return IsWayGoFutureAndBack(way);
		}

		private static bool IsWayGoFutureAndBack(List<int> way)
		{
			List<int> list = new List<int> { 27, 28, 29 };
			for (int i = 1; i < way.Count - 1; i++)
			{
				if (way[i] == 102 && way[i + 1] == 24 && list.Contains(way[i - 1]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
