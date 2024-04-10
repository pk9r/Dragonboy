using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.Xmap
{
    public class XmapUtils
    {

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