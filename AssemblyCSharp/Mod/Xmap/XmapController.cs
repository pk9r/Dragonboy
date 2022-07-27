using System.Collections.Generic;

namespace AssemblyCSharp.Mod.Xmap
{
	public class XmapController : IActionListener
	{
		private const int TIME_DELAY_NEXTMAP = 200;

		private const int TIME_DELAY_RENEXTMAP = 500;

		private const int ID_ITEM_CAPSULE_VIP = 194;

		private const int ID_ITEM_CAPSULE = 193;

		private const int ID_ICON_ITEM_TDLT = 4387;

		private static readonly XmapController _Instance = new XmapController();

		private static int IdMapEnd;

		private static List<int> WayXmap;

		private static int IndexWay;

		private static bool IsNextMapFailed;

		private static bool IsWait;

		private static long TimeStartWait;

		private static long TimeWait;

		private static bool IsWaitNextMap;

		public static void Update()
		{
			if (IsWaiting() || XmapData.Instance().IsLoading)
			{
				return;
			}
			if (IsWaitNextMap)
			{
				Wait(200);
				IsWaitNextMap = false;
				return;
			}
			if (IsNextMapFailed)
			{
				XmapData.Instance().MyLinkMaps = null;
				WayXmap = null;
				IsNextMapFailed = false;
				return;
			}
			if (WayXmap == null)
			{
				GameScr.info1.addInfo("Đi đến: " + TileMap.mapNames[IdMapEnd], 0);
				if (XmapData.Instance().MyLinkMaps == null && GameCanvas.gameTick % 50 == 0)
				{
					XmapData.Instance().LoadLinkMaps();
					return;
				}
				WayXmap = XmapAlgorithm.FindWay(TileMap.mapID, IdMapEnd);
				IndexWay = 0;
				if (WayXmap == null)
				{
					GameScr.info1.addInfo("Không thể tìm thấy đường đi", 0);
					FinishXmap();
					GameScr.info1.addInfo("Đã hủy Xmap", 0);
					return;
				}
			}
			if (TileMap.mapID == WayXmap[WayXmap.Count - 1] && !XmapData.IsMyCharDie())
			{
				GameScr.info1.addInfo("Đã đến: " + TileMap.mapNames[TileMap.mapID], 0);
				FinishXmap();
			}
			else if (TileMap.mapID == WayXmap[IndexWay])
			{
				if (XmapData.IsMyCharDie())
				{
					Service.gI().returnTownFromDead();
					IsWaitNextMap = (IsNextMapFailed = true);
				}
				else if (XmapData.CanNextMap())
				{
					NextMap(WayXmap[IndexWay + 1]);
					IsWaitNextMap = true;
				}
				Wait(500);
			}
			else if (TileMap.mapID == WayXmap[IndexWay + 1])
			{
				IndexWay++;
			}
			else
			{
				IsNextMapFailed = true;
			}
		}

		public void perform(int idAction, object p)
		{
			if (idAction == 1)
			{
				ShowPanelXmap((List<int>)p);
			}
		}

		private static void Wait(int time)
		{
			IsWait = true;
			TimeStartWait = mSystem.currentTimeMillis();
			TimeWait = time;
		}

		private static bool IsWaiting()
		{
			if (IsWait && mSystem.currentTimeMillis() - TimeStartWait >= TimeWait)
			{
				IsWait = false;
			}
			return IsWait;
		}

		public static void ShowXmapMenu()
		{
			XmapData.Instance().LoadGroupMapsFromFile("Dragonboy_vn_v200_Data\\TextData\\GroupMapsXmap.txt");
			MyVector myVector = new MyVector();
			foreach (GroupMap groupMap in XmapData.Instance().GroupMaps)
			{
				myVector.addElement(new Command(groupMap.NameGroup, _Instance, 1, groupMap.IdMaps));
			}
			GameCanvas.menu.startAt(myVector, 3);
		}

		public static void ShowPanelXmap(List<int> idMaps)
		{
			Pk9rXmap.IsMapTransAsXmap = true;
			int count = idMaps.Count;
			GameCanvas.panel.mapNames = new string[count];
			GameCanvas.panel.planetNames = new string[count];
			for (int i = 0; i < count; i++)
			{
				string text = TileMap.mapNames[idMaps[i]];
				GameCanvas.panel.mapNames[i] = idMaps[i] + ": " + text;
				GameCanvas.panel.planetNames[i] = "";
			}
			GameCanvas.panel.setTypeMapTrans();
			GameCanvas.panel.show();
		}

		public static void StartRunToMapId(int idMap)
		{
			IdMapEnd = idMap;
			Pk9rXmap.IsXmapRunning = true;
		}

		public static void FinishXmap()
		{
			Pk9rXmap.IsXmapRunning = false;
			IsNextMapFailed = false;
			XmapData.Instance().MyLinkMaps = null;
			WayXmap = null;
		}

		public static void SaveIdMapCapsuleReturn()
		{
			Pk9rXmap.IdMapCapsuleReturn = TileMap.mapID;
		}

		private static void NextMap(int idMapNext)
		{
			List<MapNext> mapNexts = XmapData.Instance().GetMapNexts(TileMap.mapID);
			if (mapNexts != null)
			{
				foreach (MapNext item in mapNexts)
				{
					if (item.MapID == idMapNext)
					{
						NextMap(item);
						return;
					}
				}
			}
			GameScr.info1.addInfo("Lỗi tại dữ liệu", 0);
		}

		private static void NextMap(MapNext mapNext)
		{
			switch (mapNext.Type)
			{
			case TypeMapNext.AutoWaypoint:
				NextMapAutoWaypoint(mapNext);
				break;
			case TypeMapNext.NpcMenu:
				NextMapNpcMenu(mapNext);
				break;
			case TypeMapNext.NpcPanel:
				NextMapNpcPanel(mapNext);
				break;
			case TypeMapNext.Position:
				NextMapPosition(mapNext);
				break;
			case TypeMapNext.Capsule:
				NextMapCapsule(mapNext);
				break;
			}
		}

		private static void NextMapAutoWaypoint(MapNext mapNext)
		{
			Waypoint waypoint = XmapData.FindWaypoint(mapNext.MapID);
			if (waypoint != null)
			{
				int posWaypointX = XmapData.GetPosWaypointX(waypoint);
				int posWaypointY = XmapData.GetPosWaypointY(waypoint);
				MoveMyChar(posWaypointX, posWaypointY);
				RequestChangeMap(waypoint);
			}
		}

		private static void NextMapNpcMenu(MapNext mapNext)
		{
			int num = mapNext.Info[0];
			Service.gI().openMenu(num);
			for (int i = 1; i < mapNext.Info.Length; i++)
			{
				int num2 = mapNext.Info[i];
				Service.gI().confirmMenu((short)num, (sbyte)num2);
			}
		}

		private static void NextMapNpcPanel(MapNext mapNext)
		{
			int num = mapNext.Info[0];
			int num2 = mapNext.Info[1];
			int selected = mapNext.Info[2];
			Service.gI().openMenu(num);
			Service.gI().confirmMenu((short)num, (sbyte)num2);
			Service.gI().requestMapSelect(selected);
		}

		private static void NextMapPosition(MapNext mapNext)
		{
			int x = mapNext.Info[0];
			int y = mapNext.Info[1];
			MoveMyChar(x, y);
			Service.gI().requestChangeMap();
			Service.gI().getMapOffline();
		}

		private static void NextMapCapsule(MapNext mapNext)
		{
			SaveIdMapCapsuleReturn();
			int selected = mapNext.Info[0];
			Service.gI().requestMapSelect(selected);
		}

		public static void UseCapsuleNormal()
		{
			Pk9rXmap.IsShowPanelMapTrans = false;
			Service.gI().useItem(0, 1, -1, 193);
		}

		public static void UseCapsuleVip()
		{
			Pk9rXmap.IsShowPanelMapTrans = false;
			Service.gI().useItem(0, 1, -1, 194);
		}

		public static void HideInfoDlg()
		{
			InfoDlg.hide();
		}

		private static void MoveMyChar(int x, int y)
		{
			Char.myCharz().cx = x;
			Char.myCharz().cy = y;
			Service.gI().charMove();
			if (!ItemTime.isExistItem(4387))
			{
				Char.myCharz().cx = x;
				Char.myCharz().cy = y + 1;
				Service.gI().charMove();
				Char.myCharz().cx = x;
				Char.myCharz().cy = y;
				Service.gI().charMove();
			}
		}

		private static void RequestChangeMap(Waypoint waypoint)
		{
			if (waypoint.isOffline)
			{
				Service.gI().getMapOffline();
			}
			else
			{
				Service.gI().requestChangeMap();
			}
		}
	}
}
