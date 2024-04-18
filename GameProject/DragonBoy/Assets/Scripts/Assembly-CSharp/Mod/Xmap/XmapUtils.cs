using System.Text;
using Mod.R;

namespace Mod.Xmap
{
    internal class XmapUtils
    {
        internal static int mapCapsuleReturn = -1;

        internal static readonly short ID_ITEM_CAPSULE_VIP = 194;
        internal static readonly short ID_ITEM_CAPSULE_NORMAL = 193;

        internal static readonly int ID_MAP_HOME_BASE = 21;
        internal static readonly int ID_MAP_LANG_BASE = 7;
        internal static readonly int ID_MAP_TTVT_BASE = 24;

        internal static int getX(sbyte type)
        {
            for (int i = 0; i < TileMap.vGo.size(); i++)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                if (waypoint.maxX < 60 && type == 0)
                    return 15;
                if (waypoint.minX > TileMap.pxw - 60 && type == 2)
                    return TileMap.pxw - 15;
            }
            return 0;
        }

        internal static int getY(sbyte type)
        {
            for (int i = 0; i < TileMap.vGo.size(); i++)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                if (waypoint.maxX < 60 && type == 0)
                    return waypoint.maxY;
                if (waypoint.minX > TileMap.pxw - 60 && type == 2)
                    return waypoint.maxY;
            }
            return 0;
        }

        internal static Waypoint findWaypoint(int idMap)
        {
            Waypoint waypoint;
            string textPopup;
            for (int i = 0; i < TileMap.vGo.size(); i++)
            {
                waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                textPopup = Utils.getTextPopup(waypoint.popup);
                if (textPopup.Equals(TileMap.mapNames[idMap]))
                    return waypoint;
            }
            return null;
        }

        internal static int getMapIdFromName(string mapName)
        {
            int offset = Char.myCharz().cgender;
            if (mapName.Equals(LocalizedString.goHome))
                return ID_MAP_HOME_BASE + offset;
            if (mapName.Equals(LocalizedString.spaceshipStation))
                return ID_MAP_TTVT_BASE + offset;
            if (LocalizedString.backTo.ContainsReversed(mapName))
            {
                mapName = LocalizedString.backTo.Replace(mapName, ""); 
                if (TileMap.mapNames[mapCapsuleReturn].Equals(mapName))
                    return mapCapsuleReturn;
                if (mapName == LocalizedString.stoneForest)
                    return -1;
            }
            for (int i = 0; i < TileMap.mapNames.Length; i++)
                if (mapName.Equals(TileMap.mapNames[i]))
                    return i;
            return -1;
        }

        internal static int getIdMapHome(int cgender) => ID_MAP_HOME_BASE + cgender;

        internal static int getIdMapLang(int cgender) => ID_MAP_LANG_BASE * cgender;

        internal static bool hasItemCapsuleVip()
        {
            Item[] items = Char.myCharz().arrItemBag;
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null && items[i].template.id == ID_ITEM_CAPSULE_VIP)
                    return true;
            return false;
        }

        internal static bool hasItemCapsuleNormal()
        {
            Item[] items = Char.myCharz().arrItemBag;
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null && items[i].template.id == ID_ITEM_CAPSULE_NORMAL)
                    return true;
            return false;
        }
    }
}