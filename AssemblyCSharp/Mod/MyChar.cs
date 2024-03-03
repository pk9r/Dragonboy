namespace Mod
{
    internal class MyChar
    {
        private const sbyte HomeBaseId = 21;

        public static int homeId()
        {
            return Char.myCharz().cgender + HomeBaseId;
        }

        public static int petId()
        {
            return CharUtilities.getPetId(Char.myCharz());
        }

        public static bool isHome()
        {
            return TileMap.mapID == homeId();
        }

        public static bool isDead()
        {
            Char myChar = Char.myCharz();

            return CharUtilities.isCharDead(myChar);
        }
    }
}
