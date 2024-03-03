using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod
{
    internal class CharUtilities
    {
        private const sbyte HomeBaseId = 21;

        public static bool isMyCharHome()
        {
            return TileMap.mapID == Char.myCharz().cgender + HomeBaseId;
        }

        public static bool isCharDead(Char @char)
        {
            return @char.isDie || @char.cHP <= 0 || @char.statusMe == 14;
        }

        public static int getPetId(Char @char)
        {
            return -@char.charID;
        }
    }
}
