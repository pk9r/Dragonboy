using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class AutoGoback
    {
        public static InfoGoback infoGoback = new InfoGoback();

        static bool isGobacking = false;

        static long lastTimeGoback;
        public static void update()
        {
            if (ModMenu.getStatusInt(2) == 0) return;
            if (GameCanvas.gameTick % (30 * Time.timeScale) == 0)
            {
                if (!isGobacking)
                {
                    if (Char.myCharz().cHP <= 0 || Char.myCharz().isDie)
                    {
                        if (mSystem.currentTimeMillis() - lastTimeGoback > 4000) lastTimeGoback = mSystem.currentTimeMillis();
                        if (mSystem.currentTimeMillis() - lastTimeGoback > 3000)
                        {
                            if (ModMenu.getStatusInt(2) == 1) infoGoback = new InfoGoback(TileMap.mapID, TileMap.zoneID, Char.myCharz());
                            Service.gI().returnTownFromDead();
                            isGobacking = true;
                        }
                    }
                }
                else
                {
                    if (TileMap.mapID == Char.myCharz().cgender + 21)
                    {
                        if (GameScr.vItemMap.size() > 0) Service.gI().pickItem(-1);
                        else if (Char.myCharz().cHP <= 1) GameScr.gI().doUseHP();
                        else if (!Pk9rXmap.IsXmapRunning) XmapController.StartRunToMapId(infoGoback.mapID);
                    }
                    else if (TileMap.mapID == infoGoback.mapID)
                    {
                        if (TileMap.zoneID != infoGoback.zoneID && GameCanvas.gameTick % (60 * Time.timeScale) == 0) Service.gI().requestChangeZone(infoGoback.zoneID, 0);
                        if (TileMap.zoneID == infoGoback.zoneID)
                        {
                            if (Char.myCharz().cx != infoGoback.x || Char.myCharz().cy != infoGoback.y) Utilities.teleportMyChar(infoGoback.x, infoGoback.y);
                            else isGobacking = false;
                        }
                    }
                }
            }
        }

        public struct InfoGoback
        {
            public int mapID;
            public int zoneID;
            public int x;
            public int y;

            public InfoGoback(int mapId, int zoneId, int x, int y)
            {
                mapID = mapId;
                zoneID = zoneId;
                this.x = x;
                this.y = y;
            }
            public InfoGoback(int mapId, int zoneId, IMapObject mapObject)
            {
                mapID = mapId;
                zoneID = zoneId;
                this.x = mapObject.getX();
                this.y = mapObject.getY();
            }
        }

    }
}