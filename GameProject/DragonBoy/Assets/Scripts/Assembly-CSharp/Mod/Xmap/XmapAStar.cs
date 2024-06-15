using System;
using System.Collections.Generic;
using System.Linq;
using Mod.AStar;
using Mod.AStar.Options;

namespace Mod.Xmap
{
    internal class XmapAStar
    {
        internal static Stack<Tile> FindPath(Tile start, Tile destination)
        {
            PathFinderOptions pathfinderOptions = new PathFinderOptions
            {
                PunishChangeDirection = true,
                UseDiagonals = false,
            };
            short[,] tiles = new short[TileMap.tmh, TileMap.tmw];

            for (int y = 0; y < TileMap.tmh; y++)
            {
                for (int x = 0; x < TileMap.tmw; x++)
                {
                    tiles[y, x] = 1;
                    //if (x < 1 || x >= TileMap.tmw - 1)
                        //tiles[y, x] = 0;
                    //else if (y >= TileMap.tmh - 1)
                        //tiles[y, x] = 0;
                    //else 
                    if (TileMap.maps[y * TileMap.tmw + x] == 0)
                    { }
                    else if (!TileMap.tileTypeAt(x * TileMap.size, y * TileMap.size, 2) && !IsTileMapICantEnter(x * TileMap.size, y * TileMap.size))
                    { }
                    else
                        tiles[y, x] = 0;
                }
            }

            WorldGrid worldGrid = new WorldGrid(tiles);
            PathFinder pathfinder = new PathFinder(worldGrid, pathfinderOptions);
            List<AStar.Point> points = pathfinder.FindPath(new AStar.Point(start.x, start.y), new AStar.Point(destination.x, destination.y)).ToList();

            //string str = "path: \r\n";
            //for (int y = 0; y < TileMap.tmh; y++)
            //{
            //    for (int x = 0; x < TileMap.tmw; x++)
            //    {
            //        string v = (tiles[y, x] == 1 ? "_" : "1") + " ";
            //        if (x == start.x && y == start.y)
            //            v = "<color=yellow>" + v + "</color>";
            //        if (x == destination.x && y == destination.y)
            //            v = "<color=red>" + v + "</color>";
            //        if (points.Any(p => p.X == x && p.Y == y))
            //            v = "<color=green>" + v + "</color>";
            //        str += v;
            //    }
            //    str += Environment.NewLine;
            //}
            //Debug.Log(str);

            for (int i = points.Count - 3; i >= 0; i--)
                if (IsStraightLine(points[i], points[i + 1], points[i + 2]))
                    points.RemoveAt(i + 1);
            return new Stack<Tile>(points.Reverse<AStar.Point>().Select(p => new Tile(p.X, p.Y)));
        }

        static bool IsTileMapICantEnter(int px, int py) => TileMap.tileTypeAt(px, py, 4) || TileMap.tileTypeAt(px, py, 8) || TileMap.tileTypeAt(px, py, 8192);

        static bool IsStraightLine(AStar.Point a, AStar.Point b, AStar.Point c)
        {
            // area of triangle == 0
            return (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)) / 2 == 0;
        }
    }
}