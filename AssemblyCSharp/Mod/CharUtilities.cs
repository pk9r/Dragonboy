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
    }
}
