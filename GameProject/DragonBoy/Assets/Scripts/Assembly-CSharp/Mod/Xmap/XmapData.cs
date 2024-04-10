using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Mod.Xmap
{
    public class XmapData
    {
        private const int ID_MAP_SIEU_THI = 84;
        private const int ID_MAP_TPVGT = 19;
        private const int ID_MAP_TO_COLD = 109;

        #region Groups
        public static List<GroupMap> groups = new();

        public static void loadGroupMapsFromFile(string path)
        {
            groups.Clear();
            try
            {
                using (var sr = new StreamReader(path))
                {
                    string textLine;
                    string textLine2;
                    while ((textLine = sr.ReadLine()) != null)
                    {
                        textLine = textLine.Trim();
                        if (textLine.StartsWith("#") || textLine == "")
                            continue;

                        textLine2 = sr.ReadLine().Trim();

                        string[] textData = textLine2.Split(' ');
                        var data = Array.ConvertAll(textData, int.Parse).ToList();

                        groups.Add(new GroupMap(textLine, data));
                    }
                }
            }
            catch (Exception e)
            {
                GameScr.info1.addInfo(e.Message, 0);
            }
            removeMapsHomeInGroupMaps();
        }

        private static void removeMapsHomeInGroupMaps()
        {
            int cgender = Char.myCharz().cgender;
            foreach (var groupMap in groups)
            {
                switch (cgender)
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
        #endregion

        public List<MapNext>[] links;

        public XmapData()
        {
            links = new List<MapNext>[TileMap.mapNames.Length];
            for (int i = 0; i < links.Length; i++)
                links[i] = new();

            loadLinksFromFile("TextData\\LinkMapsXmap.txt");
            loadLinksAutoWaypointFromFile("TextData\\AutoLinkMapsWaypoint.txt");
            addLinksHome();
            loadLinkSieuThi();
            loadLinkToCold();
        }

        public void loadLinkMapCapsule()
        {

            if (Pk9rXmap.canUseCapsuleVip())
            {
                LogMod.writeLine($"[xmap][dbg] Sử dụng capsule đặc biệt");
                Service.gI().useItem(0, 1, -1, Utils.ID_ITEM_CAPSULE_VIP);
                Pk9rXmap.isShowPanelMapTrans = false;
            }
            else if (Pk9rXmap.canUseCapsuleNormal())
            {
                LogMod.writeLine($"[xmap][dbg] Sử dụng capsule thường");
                Service.gI().useItem(0, 1, -1, Utils.ID_ITEM_CAPSULE_NORMAL);
                Pk9rXmap.isShowPanelMapTrans = false;
            }
            else
            {
                return;
            }
            while (Pk9rXmap.isWaitInfoMapTrans())
            {
                Thread.Sleep(100);
            }

            var mapStart = TileMap.mapID;
            var mapNames = GameCanvas.panel.mapNames;
            var length = mapNames.Length;
            for (int select = 0; select < length; select++)
            {
                var to = Utils.getMapIdFromName(mapNames[select]);
                if (to != -1) links[mapStart].Add(new(mapStart, to,
                    TypeMapNext.Capsule, info: new int[] { select }));
            }
        }

        private void loadLinksFromFile(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    string textLine;
                    while ((textLine = reader.ReadLine()) != null)
                    {
                        textLine = textLine.Trim();

                        // textLine is comment or empty
                        if (textLine.StartsWith("#") || textLine.Equals(""))
                            continue;

                        var data = Array.ConvertAll(textLine.Split(' '), int.Parse);

                        var mapStart = data[0];
                        var to = data[1];
                        var typeMapNext = (TypeMapNext)data[2];

                        var lenInfo = data.Length - 3;
                        var info = new int[lenInfo];
                        Array.Copy(data, 3, info, 0, lenInfo);

                        links[mapStart].Add(new(mapStart, to, typeMapNext, info));
                    }
                }
            }
            catch (Exception e)
            {
                LogMod.writeLine($"[xmap][error] Lỗi đọc links từ tệp {path}\n{e}");
            }
        }

        private void loadLinksAutoWaypointFromFile(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    string textLine;
                    while ((textLine = reader.ReadLine()) != null)
                    {
                        textLine = textLine.Trim();

                        // textLine is comment or empty
                        if (textLine.StartsWith("#") || textLine.Equals(""))
                            continue;

                        var data = Array.ConvertAll(textLine.Split(' '), int.Parse);

                        int length = data.Length;
                        for (int i = 0; i < length; i++)
                        {
                            var mapStart = data[i];
                            if (i != 0) links[mapStart].Add(new(mapStart, to: data[i - 1],
                                TypeMapNext.AutoWaypoint, new int[0]));
                            if (i != length - 1) links[mapStart].Add(new(mapStart, to: data[i + 1],
                                TypeMapNext.AutoWaypoint, new int[0]));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogMod.writeLine($"[xmap][error] Lỗi đọc links autowaypoint từ tệp {path}\n{e}");
            }
        }

        private void addLinksHome()
        {
            int cgender = Char.myCharz().cgender;
            int mapHome = Utils.getIdMapHome(cgender);
            int mapLang = Utils.getIdMapLang(cgender);

            links[mapHome].Add(new(mapHome, mapLang, TypeMapNext.AutoWaypoint, null));
            links[mapLang].Add(new(mapLang, mapHome, TypeMapNext.AutoWaypoint, null));
        }

        private void loadLinkSieuThi()
        {
            const int npcId = 10;
            const int select = 0;

            int offset = Char.myCharz().cgender;
            int mapTTVT = Utils.ID_MAP_TTVT_BASE + offset;
            int[] info = new int[] { npcId, select };
            links[ID_MAP_SIEU_THI].Add(new(ID_MAP_SIEU_THI, mapTTVT, TypeMapNext.NpcMenu, info));
        }

        private void loadLinkToCold()
        {
            if (Char.myCharz().taskMaint.taskId <= 30)
                return;

            const int npcId = 12;
            const int select = 0;

            int[] info = new int[] { npcId, select };
            links[ID_MAP_TPVGT].Add(new(ID_MAP_TPVGT, ID_MAP_TO_COLD, TypeMapNext.NpcMenu, info));
        }

        //private void addLinkMap(int idMapStart, int idMapNext, TypeMapNext type, int[] info)
        //{
        //    links[idMapStart].Add(new(idMapNext, type, info));
        //}
    }
}
