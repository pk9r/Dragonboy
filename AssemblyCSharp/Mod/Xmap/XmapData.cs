using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssemblyCSharp.Mod.Xmap
{
	public class XmapData
	{
		private const int ID_MAP_HOME_BASE = 21;

		private const int ID_MAP_TTVT_BASE = 24;

		private const int ID_ITEM_CAPSUAL_VIP = 194;

		private const int ID_ITEM_CAPSUAL_NORMAL = 193;

		private const int ID_MAP_TPVGT = 19;

		private const int ID_MAP_TO_COLD = 109;

		public List<GroupMap> GroupMaps;

		public Dictionary<int, List<MapNext>> MyLinkMaps;

		public bool IsLoading;

		private bool IsLoadingCapsule;

		private static XmapData _Instance;

		private XmapData()
		{
			GroupMaps = new List<GroupMap>();
			MyLinkMaps = null;
			IsLoading = false;
			IsLoadingCapsule = false;
		}

		public static XmapData Instance()
		{
			if (_Instance == null)
			{
				_Instance = new XmapData();
			}
			return _Instance;
		}

		public void LoadLinkMaps()
		{
			IsLoading = true;
		}

		public void Update()
		{
			if (IsLoadingCapsule)
			{
				if (!IsWaitInfoMapTrans())
				{
					LoadLinkMapCapsule();
					IsLoadingCapsule = false;
					IsLoading = false;
				}
				return;
			}
			LoadLinkMapBase();
			if (CanUseCapsuleVip())
			{
				XmapController.UseCapsuleVip();
				IsLoadingCapsule = true;
			}
			else if (CanUseCapsuleNormal())
			{
				XmapController.UseCapsuleNormal();
				IsLoadingCapsule = true;
			}
			else
			{
				IsLoading = false;
			}
		}

		public void LoadGroupMapsFromFile(string path)
		{
			GroupMaps.Clear();
			try
			{
				StreamReader streamReader = new StreamReader(path);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					text = text.Trim();
					if (!text.StartsWith("#") && !text.Equals(""))
					{
						List<int> idMaps = Array.ConvertAll(streamReader.ReadLine().Trim().Split(' '), (string s) => int.Parse(s)).ToList();
						GroupMaps.Add(new GroupMap(text, idMaps));
					}
				}
			}
			catch (Exception ex)
			{
				GameScr.info1.addInfo(ex.Message, 0);
			}
			RemoveMapsHomeInGroupMaps();
		}

		private void RemoveMapsHomeInGroupMaps()
		{
			int cgender = Char.myCharz().cgender;
			foreach (GroupMap groupMap in GroupMaps)
			{
				switch (cgender)
				{
				default:
					groupMap.IdMaps.Remove(21);
					groupMap.IdMaps.Remove(22);
					break;
				case 1:
					groupMap.IdMaps.Remove(21);
					groupMap.IdMaps.Remove(23);
					break;
				case 0:
					groupMap.IdMaps.Remove(22);
					groupMap.IdMaps.Remove(23);
					break;
				}
			}
		}

		private void LoadLinkMapCapsule()
		{
			AddKeyLinkMaps(TileMap.mapID);
			string[] mapNames = GameCanvas.panel.mapNames;
			for (int i = 0; i < mapNames.Length; i++)
			{
				int idMapFromName = GetIdMapFromName(mapNames[i]);
				if (idMapFromName != -1)
				{
					int[] info = new int[1] { i };
					MyLinkMaps[TileMap.mapID].Add(new MapNext(idMapFromName, TypeMapNext.Capsule, info));
				}
			}
		}

		private void LoadLinkMapBase()
		{
			MyLinkMaps = new Dictionary<int, List<MapNext>>();
			LoadLinkMapsFromFile("Dragonboy_vn_v200_Data\\TextData\\LinkMapsXmap.txt");
			LoadLinkMapsAutoWaypointFromFile("Dragonboy_vn_v200_Data\\TextData\\AutoLinkMapsWaypoint.txt");
			LoadLinkMapsHome();
			LoadLinkMapSieuThi();
			LoadLinkMapToCold();
		}

		private void LoadLinkMapsFromFile(string path)
		{
			try
			{
				StreamReader streamReader = new StreamReader(path);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					text = text.Trim();
					if (!text.StartsWith("#") && !text.Equals(""))
					{
						int[] array = Array.ConvertAll(text.Split(' '), (string s) => int.Parse(s));
						int num = array.Length - 3;
						int[] array2 = new int[num];
						Array.Copy(array, 3, array2, 0, num);
						LoadLinkMap(array[0], array[1], (TypeMapNext)array[2], array2);
					}
				}
			}
			catch (Exception ex)
			{
				GameScr.info1.addInfo(ex.Message, 0);
			}
		}

		private void LoadLinkMapsAutoWaypointFromFile(string path)
		{
			try
			{
				StreamReader streamReader = new StreamReader(path);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					text = text.Trim();
					if (text.StartsWith("#") || text.Equals(""))
					{
						continue;
					}
					int[] array = Array.ConvertAll(text.Split(' '), (string s) => int.Parse(s));
					for (int i = 0; i < array.Length; i++)
					{
						if (i != 0)
						{
							LoadLinkMap(array[i], array[i - 1], TypeMapNext.AutoWaypoint, null);
						}
						if (i != array.Length - 1)
						{
							LoadLinkMap(array[i], array[i + 1], TypeMapNext.AutoWaypoint, null);
						}
					}
				}
			}
			catch (Exception ex)
			{
				GameScr.info1.addInfo(ex.Message, 0);
			}
		}

		private void LoadLinkMapsHome()
		{
			int cgender = Char.myCharz().cgender;
			int num = 21 + cgender;
			int num2 = 7 * cgender;
			LoadLinkMap(num2, num, TypeMapNext.AutoWaypoint, null);
			LoadLinkMap(num, num2, TypeMapNext.AutoWaypoint, null);
		}

		private void LoadLinkMapSieuThi()
		{
			int cgender = Char.myCharz().cgender;
			int idMapNext = 24 + cgender;
			int[] info = new int[2] { 10, 0 };
			LoadLinkMap(84, idMapNext, TypeMapNext.NpcMenu, info);
		}

		private void LoadLinkMapToCold()
		{
			if (Char.myCharz().taskMaint.taskId > 30)
			{
				int[] info = new int[2] { 12, 0 };
				LoadLinkMap(19, 109, TypeMapNext.NpcMenu, info);
			}
		}

		public List<MapNext> GetMapNexts(int idMap)
		{
			if (CanGetMapNexts(idMap))
			{
				return MyLinkMaps[idMap];
			}
			return null;
		}

		public bool CanGetMapNexts(int idMap)
		{
			return MyLinkMaps.ContainsKey(idMap);
		}

		private void LoadLinkMap(int idMapStart, int idMapNext, TypeMapNext type, int[] info)
		{
			AddKeyLinkMaps(idMapStart);
			MapNext item = new MapNext(idMapNext, type, info);
			MyLinkMaps[idMapStart].Add(item);
		}

		private void AddKeyLinkMaps(int idMap)
		{
			if (!MyLinkMaps.ContainsKey(idMap))
			{
				MyLinkMaps.Add(idMap, new List<MapNext>());
			}
		}

		private bool IsWaitInfoMapTrans()
		{
			return !Pk9rXmap.IsShowPanelMapTrans;
		}

		public static int GetIdMapFromPanelXmap(string mapName)
		{
			return int.Parse(mapName.Split(':')[0]);
		}

		public static Waypoint FindWaypoint(int idMap)
		{
			for (int i = 0; i < TileMap.vGo.size(); i++)
			{
				Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
				if (GetTextPopup(waypoint.popup).Equals(TileMap.mapNames[idMap]))
				{
					return waypoint;
				}
			}
			return null;
		}

		public static int GetPosWaypointX(Waypoint waypoint)
		{
			if (waypoint.maxX < 60)
			{
				return 15;
			}
			if (waypoint.minX > TileMap.pxw - 60)
			{
				return TileMap.pxw - 15;
			}
			return waypoint.minX + 30;
		}

		public static int GetPosWaypointY(Waypoint waypoint)
		{
			return waypoint.maxY;
		}

		public static bool IsMyCharDie()
		{
			if (Char.myCharz().statusMe != 14)
			{
				return Char.myCharz().cHP <= 0;
			}
			return true;
		}

		public static bool CanNextMap()
		{
			if (!Char.isLoadingMap && !Char.ischangingMap)
			{
				return !Controller.isStopReadMessage;
			}
			return false;
		}

		private static int GetIdMapFromName(string mapName)
		{
			int cgender = Char.myCharz().cgender;
			if (mapName.Equals("Về nhà"))
			{
				return 21 + cgender;
			}
			if (mapName.Equals("Trạm tàu vũ trụ"))
			{
				return 24 + cgender;
			}
			if (mapName.Contains("Về chỗ cũ: "))
			{
				mapName = mapName.Replace("Về chỗ cũ: ", "");
				if (TileMap.mapNames[Pk9rXmap.IdMapCapsuleReturn].Equals(mapName))
				{
					return Pk9rXmap.IdMapCapsuleReturn;
				}
				if (mapName.Equals("Rừng đá"))
				{
					return -1;
				}
			}
			for (int i = 0; i < TileMap.mapNames.Length; i++)
			{
				if (mapName.Equals(TileMap.mapNames[i]))
				{
					return i;
				}
			}
			return -1;
		}

		private static string GetTextPopup(PopUp popUp)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < popUp.says.Length; i++)
			{
				stringBuilder.Append(popUp.says[i]);
				stringBuilder.Append(" ");
			}
			return stringBuilder.ToString().Trim();
		}

		private static bool CanUseCapsuleNormal()
		{
			if (!IsMyCharDie() && Pk9rXmap.IsUseCapsuleNormal)
			{
				return HasItemCapsuleNormal();
			}
			return false;
		}

		private static bool HasItemCapsuleNormal()
		{
			Item[] arrItemBag = Char.myCharz().arrItemBag;
			for (int i = 0; i < arrItemBag.Length; i++)
			{
				if (arrItemBag[i] != null && arrItemBag[i].template.id == 193)
				{
					return true;
				}
			}
			return false;
		}

		private static bool CanUseCapsuleVip()
		{
			if (!IsMyCharDie() && Pk9rXmap.IsUseCapsuleVip)
			{
				return HasItemCapsuleVip();
			}
			return false;
		}

		private static bool HasItemCapsuleVip()
		{
			Item[] arrItemBag = Char.myCharz().arrItemBag;
			for (int i = 0; i < arrItemBag.Length; i++)
			{
				if (arrItemBag[i] != null && arrItemBag[i].template.id == 194)
				{
					return true;
				}
			}
			return false;
		}

		public static int getX(sbyte type)
		{
			for (int i = 0; i < TileMap.vGo.size(); i++)
			{
				Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
				if (waypoint.maxX < 60 && type == 0)
				{
					return 15;
				}
				if (waypoint.minX > TileMap.pxw - 60 && type == 2)
				{
					return TileMap.pxw - 15;
				}
			}
			return 0;
		}

		public static int getY(sbyte type)
		{
			for (int i = 0; i < TileMap.vGo.size(); i++)
			{
				Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
				if (waypoint.maxX < 60 && type == 0)
				{
					return waypoint.maxY;
				}
				if (waypoint.minX > TileMap.pxw - 60 && type == 2)
				{
					return waypoint.maxY;
				}
			}
			return 0;
		}
	}
}
