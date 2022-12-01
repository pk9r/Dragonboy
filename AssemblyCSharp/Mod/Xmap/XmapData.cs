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
            LoadLinksAutoWaypointFromFile("TextData\\AutoLinkMapsWaypoint.txt");
            addLinksHome();
            loadLinkSieuThi();
            loadLinkToCold();
        }

        public void loadLinkMapCapsule()
        {

            if (Pk9rXmap.canUseCapsuleVip())
            {
                LogMod.writeLine($"[xmap][dbg] Sử dụng capsule đặc biệt");
                Service.gI().useItem(0, 1, -1, Utilities.ID_ITEM_CAPSULE_VIP);
                Pk9rXmap.isShowPanelMapTrans = false;
            }
            else if (Pk9rXmap.canUseCapsuleNormal())
            {
                LogMod.writeLine($"[xmap][dbg] Sử dụng capsule thường");
                Service.gI().useItem(0, 1, -1, Utilities.ID_ITEM_CAPSULE_NORMAL);
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

            string[] mapNames = GameCanvas.panel.mapNames;
            for (int select = 0; select < mapNames.Length; select++)
            {
                int mapId = Utilities.getMapIdFromName(mapNames[select]);
                if (mapId != -1)
                {
                    int[] info = new int[] { select };
                    links[TileMap.mapID].Add(new MapNext(mapId, TypeMapNext.Capsule, info));
                }
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

                        if (textLine.StartsWith("#") || textLine.Equals(""))
                            continue;

                        string[] textData = textLine.Split(' ');
                        int[] data = Array.ConvertAll(textData, int.Parse);

                        int lenInfo = data.Length - 3;
                        int[] info = new int[lenInfo];
                        Array.Copy(data, 3, info, 0, lenInfo);

                        addLinkMap(data[0], data[1], (TypeMapNext)data[2], info);
                    }
                }
            }
            catch (Exception e)
            {
                GameScr.info1.addInfo(e.Message, 0);
            }
        }

        private void LoadLinksAutoWaypointFromFile(string path)
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
                            addLinkMap(data[i], data[i - 1], TypeMapNext.AutoWaypoint, null);

                        if (i != data.Length - 1)
                            addLinkMap(data[i], data[i + 1], TypeMapNext.AutoWaypoint, null);
                    }
                }
            }
            catch (Exception e)
            {
                GameScr.info1.addInfo(e.Message, 0);
            }
        }

        private void addLinksHome()
        {
            int cgender = Char.myCharz().cgender;
            int idMapHome = Utilities.getIdMapHome(cgender);
            int idMapLang = Utilities.getIdMapLang(cgender);

            addLinkMap(idMapLang, idMapHome, TypeMapNext.AutoWaypoint, null);
            addLinkMap(idMapHome, idMapLang, TypeMapNext.AutoWaypoint, null);
        }

        private void loadLinkSieuThi()
        {
            const int npcId = 10;
            const int select = 0;

            int offset = Char.myCharz().cgender;
            int idMapNext = Utilities.ID_MAP_TTVT_BASE + offset;
            int[] info = new int[] { npcId, select };
            addLinkMap(ID_MAP_SIEU_THI, idMapNext, TypeMapNext.NpcMenu, info);
        }

        private void loadLinkToCold()
        {
            if (Char.myCharz().taskMaint.taskId <= 30)
                return;

            const int npcId = 12;
            const int select = 0;

            int[] info = new int[] { npcId, select };
            addLinkMap(ID_MAP_TPVGT, ID_MAP_TO_COLD, TypeMapNext.NpcMenu, info);
        }

        private void addLinkMap(int idMapStart, int idMapNext, TypeMapNext type, int[] info)
        {
            links[idMapStart].Add(new(idMapNext, type, info));
        }
    }
}
