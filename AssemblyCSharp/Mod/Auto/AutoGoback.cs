using Mod.Xmap;
using UnityEngine;

namespace Mod.Auto
{
    public class AutoGoback
    {
        public static InfoGoback goingBackTo = new InfoGoback();
        private static bool isGoingBack = false;
        private static GobackMode mode;
        private static long lastTimeGoBack;

        public static bool isEnabled => mode != GobackMode.Disabled;
        private static bool isMyCharInHome => TileMap.mapID == Char.myCharz().cgender + 21;
        private static bool isOnIntegerGameTick => GameCanvas.gameTick % (60 * Time.timeScale) == 0;

        public static void setState(int value)
        {
            mode = (GobackMode)value;

            if (mode == GobackMode.GoBackToFixedLocation)
            {
                goingBackTo = new InfoGoback(TileMap.mapID, TileMap.zoneID, Char.myCharz().cx, Char.myCharz().cy);
                GameScr.info1.addInfo($"Goback đến map: {TileMap.mapName}, khu: {TileMap.zoneID}, tọa độ: ({goingBackTo.x}, {goingBackTo.y})!", 0);
            } else
            {
                goingBackTo = new InfoGoback();
            }
        }

        public static void update()
        {
            if (!isEnabled || !isOnIntegerGameTick) return;

            if (isGoingBack)
                handleGoingBack();
            else if (Char.myCharz().cHP <= 0 || Char.myCharz().isDie)
                handleDeath();
        }

        private static void handleGoingBack()
        {
            if (isMyCharInHome)
            {
                if (hasChicken()) Service.gI().pickItem(-1);
                else if (Char.myCharz().cHP <= 1) GameScr.gI().doUseHP();
                else if (!XmapController.gI.IsActing) XmapController.start(goingBackTo.mapID);
            }
            else if (TileMap.mapID == goingBackTo.mapID)
            {
                if (TileMap.zoneID != goingBackTo.zoneID && isOnIntegerGameTick) Service.gI().requestChangeZone(goingBackTo.zoneID, 0);
                if (TileMap.zoneID == goingBackTo.zoneID)
                {
                    if (mode == GobackMode.GoBackToFixedLocation && (Char.myCharz().cx != goingBackTo.x || Char.myCharz().cy != goingBackTo.y)) Utilities.teleportMyChar(goingBackTo.x, goingBackTo.y);
                    else isGoingBack = false;
                }
            }
        }

        private static void handleDeath()
        {
            long now = mSystem.currentTimeMillis();
            long timeSinceDeath = now - lastTimeGoBack;

            if (timeSinceDeath > 4000) lastTimeGoBack = now;

            if (timeSinceDeath > 3000)
            {
                goingBackTo = new InfoGoback(TileMap.mapID, TileMap.zoneID, Char.myCharz());
                Service.gI().returnTownFromDead();
                isGoingBack = true;
            }
        }

        private static bool hasChicken()
        {
            return GameScr.vItemMap.size() > 0;
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
            GoBackToWhereIDied,
            GoBackToFixedLocation
        }

    }
}
