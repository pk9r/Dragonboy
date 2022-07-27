using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class TeleportChar
{
    public int charID { get; set; }

    public string cName { get; set; }

    public long lastTimeTeleportTo { get; set; }

    public TeleportChar(int charId)
    {
        cName = "Không tên";
        this.charID = charId;
        lastTimeTeleportTo = mSystem.currentTimeMillis();
    }

    public TeleportChar(string cName, int charId)
    {
        this.charID = charId;
        this.cName = cName;
        lastTimeTeleportTo = mSystem.currentTimeMillis();
    }
    public TeleportChar(Char @char) 
    {
        cName = CharExtensions.getNameWithoutClanTag(@char);
        charID = @char.charID;
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
        int hashCode = 2137564001;
        hashCode = hashCode * -1521134295 + charID.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(cName);
        return hashCode;
    }
}
