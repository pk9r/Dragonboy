using Mod.Xmap;

namespace Mod.Auto
{
    public class AutoGoback
    {
        public static InfoGoBack goingBackTo = new InfoGoBack();
        public static bool isGoingBack = false;
        private static GoBackMode mode;
        private static long lastTimeGoBack;

        public static bool isEnabled => mode != GoBackMode.Disabled;

        public static void setState(int value)
        {
            mode = (GoBackMode)value;

            if (isEnabled) enable();
            else disable();
        }

        public static void enable()
        {
            if (mode != GoBackMode.GoBackToFixedLocation) return;

            goingBackTo = new InfoGoBack(TileMap.mapID, TileMap.zoneID, Char.myCharz().cx, Char.myCharz().cy);
            GameScr.info1.addInfo($"Goback đến map: {TileMap.mapName}, khu: {TileMap.zoneID}, tọa độ: ({goingBackTo.x}, {goingBackTo.y})!", 0);
        }

        public static void disable()
        {
            isGoingBack = false;
            XmapController.finishXmap();
        }

        public static void update()
        {
            if (!isEnabled || !Utilities.isFrameMultipleOf(60) || XmapController.gI.IsActing)
                return;

            if (isGoingBack)
                handleGoingBack();
            else if (MyChar.isDead())
                handleDeath();
        }

        private static void handleGoingBack()
        {
            if (MyChar.isHome())
            {
                if (hasChicken()) Service.gI().pickItem(-1);
                else if (Char.myCharz().cHP <= 1) GameScr.gI().doUseHP();
                else XmapController.start(goingBackTo.mapID);
            }
            else if (TileMap.mapID == goingBackTo.mapID)
            {
                if (TileMap.zoneID != goingBackTo.zoneID)
                  Service.gI().requestChangeZone(goingBackTo.zoneID, 0);
                else 
                {
                    if (mode != GoBackMode.GoBackToMap && Char.myCharz().cx != goingBackTo.x || Char.myCharz().cy != goingBackTo.y)
                        Utilities.teleportMyChar(goingBackTo.x, goingBackTo.y);
                    else
                        isGoingBack = false;
                }
            }
        }

        private static void handleDeath()
        {
            long now = mSystem.currentTimeMillis();
            long timeSinceDeath = now - lastTimeGoBack;

            if (timeSinceDeath > 4000)
            {
                lastTimeGoBack = now;
                return;
            }

            if (timeSinceDeath > 3000)
            {
                if (mode != GoBackMode.GoBackToFixedLocation)
                    goingBackTo = new InfoGoBack(TileMap.mapID, TileMap.zoneID, Char.myCharz());

                Service.gI().returnTownFromDead();
                isGoingBack = true;
            }
        }

        private static bool hasChicken()
        {
            return GameScr.vItemMap.size() > 0;
        }

        public struct InfoGoBack
        {
            public int mapID;
            public int zoneID;
            public int x;
            public int y;

            public InfoGoBack(int mapId, int zoneId, int x, int y)
            {
                mapID = mapId;
                zoneID = zoneId;
                this.x = x;
                this.y = TileMap.tileTypeAt(x, y, 2) ? y : Utilities.getYGround(x);
            }
            public InfoGoBack(int mapId, int zoneId, IMapObject mapObject)
            {
                mapID = mapId;
                zoneID = zoneId;
                x = mapObject.getX();
                y = TileMap.tileTypeAt(x, mapObject.getY(), 2) ? mapObject.getY() : Utilities.getYGround(x);
            }
        }

        public enum GoBackMode
        {
            Disabled,
            GoBackToWhereIDied,
            GoBackToFixedLocation,
            GoBackToMap,
        }
    }
}
