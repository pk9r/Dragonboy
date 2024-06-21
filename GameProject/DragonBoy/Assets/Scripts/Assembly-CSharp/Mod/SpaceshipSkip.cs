namespace Mod
{
    internal static class SpaceshipSkip
    {
        internal static bool isEnabled;

        internal static void Update(Teleport teleport)
        {
            if (!isEnabled)
                return;
            if (teleport.isMe)
            {
                if (teleport.type == 0) //fly up
                {
                    Controller.isStopReadMessage = false;
                    Char.ischangingMap = true;
                    Teleport.vTeleport.removeElement(teleport);
                }
                else //fly down
                {
                    if (Char.myCharz().isTeleport)
                        Char.myCharz().cy = teleport.y = teleport.y2;
                    Char.myCharz().isTeleport = false;
                }
            }
            else
            {
                Char ch = GameScr.findCharInMap(teleport.id);
                if (ch != null)
                {
                    if (teleport.type == 0)
                    {
                        if (teleport.isDown)
                            teleport.y = teleport.y2;
                    }
                    else
                    {
                        if (ch.isTeleport)
                            ch.cy = teleport.y = teleport.y2;
                        ch.isTeleport = false;
                    }
                }
            }
        }
    }
}
