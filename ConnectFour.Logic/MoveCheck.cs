using System.Collections.Generic;
using System.Drawing;

namespace ConnectFour.Logic
{
    static class MoveCheck
    {
        public  static bool IsMoveAllowed(int x, int y, int[,] gamefield)
        {
            return IsMoveAllowed(new Point(x, y), gamefield);
        }

        public static bool IsMoveAllowed(Point p, int[,] gamefield)
        {
            return p.Y >= 0 && p.X >= 0 && p.X < 7 && gamefield[p.X, p.Y] == 0 && (p.Y == 5 || gamefield[p.X, p.Y + 1] != 0);
        }
        
        public static bool IsSetByPlayer(List<Point> points, int player, int[,] gamefield)
        {
            foreach (Point point in points)
            {
                if (!IsSetByPlayer(point, player, gamefield))
                    return false;
            }

            return true;
        }

        public static bool IsSetByPlayer(Point p, int player, int[,] gamefield)
        {
            return IsSetByPlayer(p.X, p.Y, player, gamefield);
        }
        public static bool IsSetByPlayer(int x, int y, int player, int[,] gamefield)
        {
            return gamefield[x, y] == player;
        }

        public static bool PointValid(Point p)
        {
            return p.X != -1 && p.Y != 1;
        }
    }
}
