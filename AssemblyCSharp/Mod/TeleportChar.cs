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
        cName = @char.getNameWithoutClanTag();
        charID = @char.charID;
        lastTimeTeleportTo = mSystem.currentTimeMillis();
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
}
