namespace Mod.AStar
{
    public class Point
    {
        int x;
        int y;

        public int X => x;
        public int Y => y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}