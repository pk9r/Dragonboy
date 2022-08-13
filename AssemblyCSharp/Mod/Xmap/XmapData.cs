using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mod.Xmap
{
    public class XmapData
    {
        private const int ID_MAP_HOME_BASE = 21;
        private const int ID_MAP_TTVT_BASE = 24;
        private const int ID_ITEM_CAPSULE_VIP = 194;
        private const int ID_ITEM_CAPSULE_NORMAL = 193;
        private const int ID_MAP_TPVGT = 19;
        private const int ID_MAP_TO_COLD = 109;

        public List<GroupMap> GroupMaps;
        public Dictionary<int, List<MapNext>> MyLinkMaps;
        public bool IsLoading;
        private bool IsLoadingCapsule;

        private XmapData()
        {
            GroupMaps = new List<GroupMap>();
            MyLinkMaps = null;
            IsLoading = false;
            IsLoadingCapsule = false;
        }

        private static XmapData _Instance;

        public static XmapData Instance()
        {
            if (_Instance == null) _Instance = new XmapData();
            return _Instance;
        }

        public void LoadLinkMaps()
        {
            IsLoading = true;
        }

        public void Update()
        {
            if (!IsLoadingCapsule)
            {
                LoadLinkMapBase();
                if (CanUseCapsuleVip())
                {
                    XmapController.UseCapsuleVip();
                    IsLoadingCapsule = true;
                    return;
                }
                if (CanUseCapsuleNormal())
                {
                    XmapController.UseCapsuleNormal();
                    IsLoadingCapsule = true;
                    return;
                }
                IsLoading = false;
                return;
            }
            if (IsWaitInfoMapTrans())
                return;
            LoadLinkMapCapsule();
            IsLoadingCapsule = false;
            IsLoading = false;
        }

        #region Thao tác với dữ liệu xmap
        #region Lấy dữ liệu các nhóm map
        public void LoadGroupMapsFromFile(string path)
        {
            GroupMaps.Clear();
            try
            {
                StreamReader sr = new StreamReader(path);
                string textLine;
                string textLine2;
                while ((textLine = sr.ReadLine()) != null)
                {
                    textLine = textLine.Trim();
                    if (textLine.StartsWith("#") || textLine.Equals(""))
                        continue;

                    textLine2 = sr.ReadLine().Trim();

                    string[] textData = textLine2.Split(' ');
                    List<int> data = Array.ConvertAll(textData, s => int.Parse(s)).ToList();

                    GroupMaps.Add(new GroupMap(textLine, data));
                }
            }
            catch (Exception e)
            {
                GameScr.info1.addInfo(e.Message, 0);
            }
            RemoveMapsHomeInGroupMaps();
        }

        private void RemoveMapsHomeInGroupMaps()
        {
            int cgender = Char.myCharz().cgender;
            foreach (var groupMap in GroupMaps)
            {
                switch (cgender)
                {
                    case 0:
                        groupMap.IdMaps.Remove(22);
                        groupMap.IdMaps.Remove(23);
                        break;
                    case 1:
                        groupMap.IdMaps.Remove(21);
                        groupMap.IdMaps.Remove(23);
                        break;
                    default:
                        groupMap.IdMaps.Remove(21);
                        groupMap.IdMaps.Remove(22);
                        break;
                }
            }
        }
        #endregion

        #region Lấy dữ liệu cho xmap
        private void LoadLinkMapCapsule()
        {
            AddKeyLinkMaps(TileMap.mapID);
            string[] mapNames = GameCanvas.panel.mapNames;
            int idMap;
            for (int select = 0; select < mapNames.Length; select++)
            {
                idMap = GetIdMapFromName(mapNames[select]);
                if (idMap != -1)
                {
                    int[] info = new int[] { select };
                    MyLinkMaps[TileMap.mapID].Add(new MapNext(idMap, TypeMapNext.Capsule, info));
                }
            }
        }

        private void LoadLinkMapBase()
        {
            MyLinkMaps = new Dictionary<int, List<MapNext>>();
            LoadLinkMapsFromFile("TextData\\LinkMapsXmap.txt");
            LoadLinkMapsAutoWaypointFromFile("TextData\\AutoLinkMapsWaypoint.txt");
            LoadLinkMapsHome();
            LoadLinkMapSieuThi();
            LoadLinkMapToCold();
        }

        private void LoadLinkMapsFromFile(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                string textLine;
                while ((textLine = sr.ReadLine()) != null)
                {
                    textLine = textLine.Trim();

                    if (textLine.StartsWith("#") || textLine.Equals(""))
                        continue;

                    string[] textData = textLine.Split(' ');
                    int[] data = Array.ConvertAll(textData, s => int.Parse(s));

                    int lenInfo = data.Length - 3;
                    int[] info = new int[lenInfo];
                    Array.Copy(data, 3, info, 0, lenInfo);

                    LoadLinkMap(data[0], data[1], (TypeMapNext)data[2], info);
                }
            }
            catch (Exception e)
            {
                GameScr.info1.addInfo(e.Message, 0);
            }
        }

        private void LoadLinkMapsAutoWaypointFromFile(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                string textLine;
                while ((textLine = sr.ReadLine()) != null)
                {
                    textLine = textLine.Trim();

                    if (textLine.StartsWith("#") || textLine.Equals(""))
                        continue;

                    string[] textData = textLine.Split(' ');
                    int[] data = Array.ConvertAll(textData, int.Parse);

                    for (int i = 0; i < data.Length; i++)
                    {
                        if (i != 0)
                            LoadLinkMap(data[i], data[i - 1], TypeMapNext.AutoWaypoint, null);

                        if (i != data.Length - 1)
                            LoadLinkMap(data[i], data[i + 1], TypeMapNext.AutoWaypoint, null);
                    }
                }
            }
            catch (Exception e)
            {
                GameScr.info1.addInfo(e.Message, 0);
            }
        }

        private void LoadLinkMapsHome()
        {
            const int ID_MAP_LANG_BASE = 7;
            int cgender = Char.myCharz().cgender;

            int idMapHome = ID_MAP_HOME_BASE + cgender;
            int idMapLang = ID_MAP_LANG_BASE * cgender;

            LoadLinkMap(idMapLang, idMapHome, TypeMapNext.AutoWaypoint, null);
            LoadLinkMap(idMapHome, idMapLang, TypeMapNext.AutoWaypoint, null);
        }

        private void LoadLinkMapSieuThi()
        {
            const int ID_MAP_TTVT_BASE = 24;
            const int ID_MAP_SIEU_THI = 84;
            const int ID_NPC = 10;
            const int INDEX = 0;

            int offset = Char.myCharz().cgender;
            int idMapNext = ID_MAP_TTVT_BASE + offset;
            int[] info = new int[]
            {
                ID_NPC, INDEX
            };
            LoadLinkMap(ID_MAP_SIEU_THI, idMapNext, TypeMapNext.NpcMenu, info);
        }

        private void LoadLinkMapToCold()
        {
            if (Char.myCharz().taskMaint.taskId <= 30)
                return;

            const int ID_NPC = 12;
            const int INDEX = 0;

            int[] info = new int[]
            {
                ID_NPC, INDEX
            };
            LoadLinkMap(ID_MAP_TPVGT, ID_MAP_TO_COLD, TypeMapNext.NpcMenu, info);
        }
        #endregion

        public List<MapNext> GetMapNexts(int idMap)
        {
            if (CanGetMapNexts(idMap)) return MyLinkMaps[idMap];
            return null;
        }

        public bool CanGetMapNexts(int idMap)
        {
            return MyLinkMaps.ContainsKey(idMap);
        }

        private void LoadLinkMap(int idMapStart, int idMapNext, TypeMapNext type, int[] info)
        {
            AddKeyLinkMaps(idMapStart);
            MapNext mapNext = new MapNext(idMapNext, type, info);
            MyLinkMaps[idMapStart].Add(mapNext);
        }

        private void AddKeyLinkMaps(int idMap)
        {
            if (!MyLinkMaps.ContainsKey(idMap)) MyLinkMaps.Add(idMap, new List<MapNext>());
        }

        private bool IsWaitInfoMapTrans()
        {
            return !Pk9rXmap.IsShowPanelMapTrans;
        }

        public static int GetIdMapFromPanelXmap(string mapName)
        {
            return int.Parse(mapName.Split(':')[0]);
        }
        #endregion

        #region Lấy dữ liệu từ game
        public static Waypoint FindWaypoint(int idMap)
        {
            Waypoint waypoint;
            string textPopup;
            for (int i = 0; i < TileMap.vGo.size(); i++)
            {
                waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                textPopup = GetTextPopup(waypoint.popup);
                if (textPopup.Equals(TileMap.mapNames[idMap]))
                {
                    return waypoint;
                }
            }
            return null;
        }

        public static int GetPosWaypointX(Waypoint waypoint)
        {
            if (waypoint.maxX < 60)
                return 15;
            if (waypoint.minX > TileMap.pxw - 60)
                return TileMap.pxw - 15;
            return waypoint.minX + 30;
        }

        public static int GetPosWaypointY(Waypoint waypoint)
        {
            return waypoint.maxY;
        }

        public static bool IsMyCharDie()
        {
            return Char.myCharz().statusMe == 14 || Char.myCharz().cHP <= 0;
        }

        public static bool CanNextMap()
        {
            return !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;
        }

        private static int GetIdMapFromName(string mapName)
        {
            int offset = Char.myCharz().cgender;
            if (mapName.Equals("Về nhà")) return ID_MAP_HOME_BASE + offset;
            if (mapName.Equals("Trạm tàu vũ trụ")) return ID_MAP_TTVT_BASE + offset;
            if (mapName.Contains("Về chỗ cũ: "))
            {
                mapName = mapName.Replace("Về chỗ cũ: ", "");
                if (TileMap.mapNames[Pk9rXmap.IdMapCapsuleReturn].Equals(mapName)) return Pk9rXmap.IdMapCapsuleReturn;
                if (mapName.Equals("Rừng đá")) return -1;
            }
            for (int i = 0; i < TileMap.mapNames.Length; i++) if (mapName.Equals(TileMap.mapNames[i])) return i;
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
            return !IsMyCharDie() && Pk9rXmap.IsUseCapsuleNormal && HasItemCapsuleNormal();
        }

        private static bool HasItemCapsuleNormal()
        {
            Item[] items = Char.myCharz().arrItemBag;
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null && items[i].template.id == ID_ITEM_CAPSULE_NORMAL)
                    return true;
            return false;
        }

        private static bool CanUseCapsuleVip()
        {
            return !IsMyCharDie() && Pk9rXmap.IsUseCapsuleVip && HasItemCapsuleVip();
        }

        private static bool HasItemCapsuleVip()
        {
            Item[] items = Char.myCharz().arrItemBag;
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null && items[i].template.id == ID_ITEM_CAPSULE_VIP)
                    return true;
            return false;
        }

        #endregion
    }
}
