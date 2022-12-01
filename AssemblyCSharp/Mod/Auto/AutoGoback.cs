using Mod.Xmap;
using UnityEngine;

namespace Mod.Auto
{
    public class AutoGoback
    {
        public static InfoGoback infoGoback = new InfoGoback();
        private static bool isGobacking = false;
        private static GobackMode gobackMode;
        private static long lastTimeGoback;

        public static void update()
        {
            if (gobackMode == GobackMode.Disabled) return;
            if (GameCanvas.gameTick % (30 * Time.timeScale) == 0)
            {
                if (!isGobacking)
                {
                    if (Char.myCharz().cHP <= 0 || Char.myCharz().isDie)
                    {
                        if (mSystem.currentTimeMillis() - lastTimeGoback > 4000) lastTimeGoback = mSystem.currentTimeMillis();
                        if (mSystem.currentTimeMillis() - lastTimeGoback > 3000)
                        {
                            if (gobackMode == GobackMode.GoBackToFixedLocation) infoGoback = new InfoGoback(TileMap.mapID, TileMap.zoneID, Char.myCharz());
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
                        else if (!XmapController.gI.IsActing) XmapController.start(infoGoback.mapID);
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

        public static void setState(int value)
        {
            gobackMode = (GobackMode)value;
            if (gobackMode == GobackMode.GoBackToFixedLocation)
            {
                infoGoback = new InfoGoback(TileMap.mapID, TileMap.zoneID, Char.myCharz().cx, Char.myCharz().cy);
                GameScr.info1.addInfo($"Goback đến map: {TileMap.mapName}, khu: {TileMap.zoneID}, tọa độ: ({infoGoback.x}, {infoGoback.y})!", 0);
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
                this.y = TileMap.tileTypeAt(x, y, 2) ? y : Utilities.getYGround(x);
            }
            public InfoGoback(int mapId, int zoneId, IMapObject mapObject)
            {
                mapID = mapId;
                zoneID = zoneId;
                x = mapObject.getX();
                y = TileMap.tileTypeAt(x, mapObject.getY(), 2) ? mapObject.getY() : Utilities.getYGround(x);
            }
        }

        public enum GobackMode
        {
            Disabled,
            GoBackToWhereMeDied,
            GoBackToFixedLocation
        }

    }
}