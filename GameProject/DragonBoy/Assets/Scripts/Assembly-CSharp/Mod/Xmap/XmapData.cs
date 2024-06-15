using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Mod.Xmap
{
    internal class XmapData
    {
        const int ID_MAP_SIEU_THI = 84;
        const int ID_MAP_TPVGT = 19;
        const int ID_MAP_TO_COLD = 109;

        internal List<MapNext>[] links;
        internal bool isLoaded;

        internal XmapData()
        {
            links = new List<MapNext>[TileMap.mapNames.Length];
            for (int i = 0; i < links.Length; i++)
                links[i] = new List<MapNext>();
        }

        internal void Load()
        {
            LoadLinks();
            LoadLinksAutoWaypoint();
            AddLinksHome();
            LoadLinkSieuThi();
            LoadLinkToCold();
            isLoaded = true;
        }

        #region Groups
        internal static List<GroupMap> groups = new List<GroupMap>();

        static void RemoveMapsHomeInGroupMaps()
        {
            foreach (var groupMap in groups)
            {
                switch (Char.myCharz().cgender)
                {
                    case 0:
                        groupMap.maps.Remove(22);
                        groupMap.maps.Remove(23);
                        break;
                    case 1:
                        groupMap.maps.Remove(21);
                        groupMap.maps.Remove(23);
                        break;
                    default:
                        groupMap.maps.Remove(21);
                        groupMap.maps.Remove(22);
                        break;
                }
            }
        }

        static void LoadGroupMaps(TextReader reader)
        {
            string textLine;
            string textLine2;
            while ((textLine = reader.ReadLine()) != null)
            {
                textLine = textLine.Trim();
                if (textLine.StartsWith("#") || textLine == "")
                    continue;
                textLine2 = reader.ReadLine().Trim();
                string[] textData = textLine2.Split(' ');
                var data = Array.ConvertAll(textData, int.Parse).ToList();
                string[] groupNames = textLine.Split('|');
                if (groups.Any(gM => gM.names.SequenceEqual(groupNames)))
                {
                    GroupMap groupMap = groups.First(gM => gM.names.SequenceEqual(groupNames));
                    foreach (var item in data)
                    {
                        if (groupMap.maps.Contains(item))
                            continue;
                        groupMap.maps.Add(item);
                    }
                }
                else 
                    groups.Add(new GroupMap(groupNames, data));
            }
            reader.Dispose();
        }

        static void LoadGroupMapsFromFile(string path)
        {
            path = Path.Combine(Utils.GetRootDataPath(), path);
            try
            {
                LoadGroupMaps(new StreamReader(path));
            }
            catch { }
            RemoveMapsHomeInGroupMaps();
        }

        static void LoadGroupMapsFromResource()
        {
            try
            {
                TextAsset textAsset = Resources.Load<TextAsset>("TextData/GroupMapsXmap");
                LoadGroupMaps(new StringReader(textAsset.text));
            }
            catch (Exception e)
            {
                GameScr.info1.addInfo(e.Message, 0);
            }
            RemoveMapsHomeInGroupMaps();
        }

        internal static void LoadGroupMaps()
        {
            groups.Clear();
            LoadGroupMapsFromResource();
            LoadGroupMapsFromFile("TextData\\GroupMapsXmap.txt");
        }
        #endregion

        #region Manual Links
        void LoadLinks(TextReader reader)
        {
            string textLine;
            while ((textLine = reader.ReadLine()) != null)
            {
                textLine = textLine.Trim();
                if (textLine.StartsWith("#") || textLine.Equals(""))
                    continue;
                var data = Array.ConvertAll(textLine.Split(' '), int.Parse);
                var mapStart = data[0];
                var to = data[1];
                var typeMapNext = (TypeMapNext)data[2];
                var lenInfo = data.Length - 3;
                var info = new int[lenInfo];
                Array.Copy(data, 3, info, 0, lenInfo);
                links[mapStart].Add(new MapNext(mapStart, to, typeMapNext, info));
            }
            reader.Dispose();
        }

        void LoadLinksFromFile(string path)
        {
            path = Path.Combine(Utils.GetRootDataPath(), path);
            try
            {
                LoadLinks(new StreamReader(path));
            }
            catch (Exception e)
            {
                LogMod.writeLine($"[xmap][error] Lỗi đọc links từ tệp {path}\n{e}");
            }
        }

        void LoadLinksFromResource()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("TextData/LinkMapsXmap");
            LoadLinks(new StringReader(textAsset.text));
        }

        void LoadLinks()
        {
            LoadLinksFromResource();
            LoadLinksFromFile("TextData\\LinkMapsXmap.txt");
        }

        internal void LoadLinkMapCapsule()
        {
            if (Pk9rXmap.CanUseCapsuleVip())
            {
                LogMod.writeLine($"[xmap][dbg] Sử dụng capsule đặc biệt");
                Service.gI().useItem(0, 1, -1, XmapUtils.ID_ITEM_CAPSULE_VIP);
            }
            else if (Pk9rXmap.CanUseCapsuleNormal())
            {
                LogMod.writeLine($"[xmap][dbg] Sử dụng capsule thường");
                Service.gI().useItem(0, 1, -1, XmapUtils.ID_ITEM_CAPSULE_NORMAL);
            }
            else
                return;
            var mapStart = TileMap.mapID;
            var mapNames = GameCanvas.panel.mapNames;
            var length = mapNames.Length;
            for (int select = 0; select < length; select++)
            {
                var to = XmapUtils.getMapIdFromName(mapNames[select]);
                if (to != -1)
                    links[mapStart].Add(new MapNext(mapStart, to, TypeMapNext.Capsule, info: new int[] { select }));
            }
        }
        #endregion

        #region Auto Links
        void LoadLinksAutoWaypoint(TextReader reader)
        {
            string textLine;
            while ((textLine = reader.ReadLine()) != null)
            {
                textLine = textLine.Trim();
                if (textLine.StartsWith("#") || textLine.Equals(""))
                    continue;
                var data = Array.ConvertAll(textLine.Split(' '), int.Parse);

                int length = data.Length;
                for (int i = 0; i < length; i++)
                {
                    var mapStart = data[i];
                    if (i != 0) 
                        links[mapStart].Add(new MapNext(mapStart, to: data[i - 1], TypeMapNext.AutoWaypoint, new int[0]));
                    if (i != length - 1)
                        links[mapStart].Add(new MapNext(mapStart, to: data[i + 1], TypeMapNext.AutoWaypoint, new int[0]));
                }
            }
        }

        void LoadLinksAutoWaypointFromResource()
        {
            try
            {
                TextAsset textAsset = Resources.Load<TextAsset>("TextData/AutoLinkMapsWaypoint");
                LoadLinksAutoWaypoint(new StringReader(textAsset.text));
            }
            catch (Exception e)
            {
                LogMod.writeLine($"[xmap][error] Lỗi đọc links autowaypoint từ resource\n{e}");
            }
        }

        void LoadLinksAutoWaypointFromFile(string path)
        {
            path = Path.Combine(Utils.GetRootDataPath(), path);
            try
            {
                LoadLinksAutoWaypoint(new StreamReader(path));
            }
            catch (Exception e)
            {
                LogMod.writeLine($"[xmap][error] Lỗi đọc links autowaypoint từ tệp {path}\n{e}");
            }
        }

        void LoadLinksAutoWaypoint()
        {
            LoadLinksAutoWaypointFromResource();
            LoadLinksAutoWaypointFromFile("TextData\\AutoLinkMapsWaypoint.txt");
        }

        void AddLinksHome()
        {
            int cgender = Char.myCharz().cgender;
            int mapHome = XmapUtils.getIdMapHome(cgender);
            int mapLang = XmapUtils.getIdMapLang(cgender);
            links[mapHome].Add(new MapNext(mapHome, mapLang, TypeMapNext.AutoWaypoint, null));
            links[mapLang].Add(new MapNext(mapLang, mapHome, TypeMapNext.AutoWaypoint, null));
        }

        void LoadLinkSieuThi()
        {
            const int npcId = 10;
            const int select = 0;
            int offset = Char.myCharz().cgender;
            int mapTTVT = XmapUtils.ID_MAP_TTVT_BASE + offset;
            int[] info = new int[] { npcId, select };
            links[ID_MAP_SIEU_THI].Add(new MapNext(ID_MAP_SIEU_THI, mapTTVT, TypeMapNext.NpcMenu, info));
        }

        void LoadLinkToCold()
        {
            if (Char.myCharz().taskMaint.taskId <= 30)
                return;
            const int npcId = 12;
            const int select = 0;
            int[] info = new int[] { npcId, select };
            links[ID_MAP_TPVGT].Add(new MapNext(ID_MAP_TPVGT, ID_MAP_TO_COLD, TypeMapNext.NpcMenu, info));
        }
        #endregion

        //void addLinkMap(int idMapStart, int idMapNext, TypeMapNext type, int[] info)
        //{
        //    links[idMapStart].Add(new(idMapNext, type, info));
        //}
    }
}
