namespace Mod
{
    internal class MyChar
    {
        private const sbyte HomeBaseId = 21;

        public static int homeId()
        {
            return Char.myCharz().cgender + HomeBaseId;
        }

        public static bool isHome()
        {
            return TileMap.mapID == homeId();
        }

        public static bool isDead()
        {
            Char myChar = Char.myCharz();
            
            return myChar.isDie || myChar.cHP <= 0 || myChar.statusMe == 14;
        }
    }
}
