using System.Collections.Generic;

namespace Mod.TeleportMenu
{
    public class TeleportChar
    {
        public int charID { get; set; }

        public string cName { get; set; }

        public long lastTimeTeleportTo { get; set; }

        public TeleportChar(int charId)
        {
            cName = "Không tên";
            charID = charId;
            lastTimeTeleportTo = mSystem.currentTimeMillis();
        }

        public TeleportChar(string cName, int charId)
        {
            charID = charId;
            this.cName = cName;
            lastTimeTeleportTo = mSystem.currentTimeMillis();
        }
        public TeleportChar(Char ch)
        {
            cName = ch.getNameWithoutClanTag();
            charID = ch.charID;
            lastTimeTeleportTo = mSystem.currentTimeMillis();
        }

        public TeleportChar(string cName, int charID, long lastTimeTeleportTo)
        {
            this.cName = cName;
            this.charID = charID;
            this.lastTimeTeleportTo = lastTimeTeleportTo;
        }

        public override string ToString()
        {
            return cName + " [" + charID + "]";
        }

        public override bool Equals(object obj)
        {
            if (obj is TeleportChar teleportChar)
            {
                return teleportChar.cName == cName && teleportChar.charID == charID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(charID, cName);
        }
    }
}