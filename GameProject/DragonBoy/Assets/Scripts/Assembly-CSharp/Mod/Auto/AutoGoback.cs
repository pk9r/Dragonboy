using Mod.R;
using Mod.Xmap;

namespace Mod.Auto
{
    internal class AutoGoback
    {
        internal static InfoGoBack goingBackTo = new InfoGoBack();
        internal static bool IsGoingBack => isGoingBack;
        static bool isGoingBack;
        internal static GoBackMode mode { get; set; }
        static long lastTimeGoBack;
        static long lastTimeUpdate;

        internal static bool isEnabled => mode != GoBackMode.Disabled;

        internal static void setState(int value)
        {
            mode = (GoBackMode)value;
            if (isEnabled)
                Enable();
            else
                Disable();
        }

        internal static void Enable()
        {
            if (mode != GoBackMode.GoBackToFixedLocation) 
                return;
            goingBackTo = new InfoGoBack(TileMap.mapID, TileMap.zoneID, Char.myCharz().cx, Char.myCharz().cy);
            GameScr.info1.addInfo(string.Format(Strings.gobackTo, TileMap.mapName, TileMap.zoneID, goingBackTo.x, goingBackTo.y) + '!', 0);
        }

        internal static void Disable()
        {
            isGoingBack = false;
            XmapController.finishXmap();
        }

        internal static void Update()
        {
            if (!isEnabled || XmapController.gI.IsActing)
                return;
            if (mSystem.currentTimeMillis() - lastTimeUpdate < 1000)
                return;
            lastTimeUpdate = mSystem.currentTimeMillis();
            if (Char.myCharz().IsCharDead())
                HandleDeath();
            else if (isGoingBack)
                HandleGoingBack();
        }

        static void HandleGoingBack()
        {
            if (Utils.IsMyCharHome())
            {
                if (Char.myCharz().cHP <= 1)
                {
                    if (Char.myCharz().taskMaint.taskId > 2)
                        Service.gI().pickItem(-1);
                    else
                        GameScr.gI().doUseHP();
                }
                else
                    XmapController.start(goingBackTo.mapID);
            }
            else if (TileMap.mapID == goingBackTo.mapID)
            {
                if (TileMap.zoneID != goingBackTo.zoneID)
                  Service.gI().requestChangeZone(goingBackTo.zoneID, 0);
                else 
                {
                    Char.chatPopup = null;
                    if (mode != GoBackMode.GoBackToWhereIDied && Char.myCharz().cx != goingBackTo.x || Char.myCharz().cy != goingBackTo.y)
                        Utils.TeleportMyChar(goingBackTo.x, goingBackTo.y);
                    else
                        isGoingBack = false;
                }
            }
        }

        static void HandleDeath()
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

        static bool HasChicken() => GameScr.vItemMap.size() > 0;

        internal struct InfoGoBack
        {
            internal int mapID;
            internal int zoneID;
            internal int x;
            internal int y;

            internal InfoGoBack(int mapId, int zoneId, int x, int y)
            {
                mapID = mapId;
                zoneID = zoneId;
                this.x = x;
                this.y = TileMap.tileTypeAt(x, y, 2) ? y : Utils.GetYGround(x);
            }
            internal InfoGoBack(int mapId, int zoneId, IMapObject mapObject)
            {
                mapID = mapId;
                zoneID = zoneId;
                x = mapObject.getX();
                y = TileMap.tileTypeAt(x, mapObject.getY(), 2) ? mapObject.getY() : Utils.GetYGround(x);
            }
        }

        internal enum GoBackMode
        {
            Disabled,
            GoBackToWhereIDied,
            GoBackToFixedLocation,
        }
    }
}
