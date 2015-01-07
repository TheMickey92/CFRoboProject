using System.Collections.Generic;
using System.Drawing;

namespace ConnectFour.Logic.CatchMoves
{
    static class WinPoint
    {
        public static Point GetWinPoint(int player, int[,] gamefield)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (x < 4)
                    {
                        // Check X-Axis
                        Point rpx = searchWinPoint(new List<Point>() { new Point(x, y), new Point(x + 1, y), new Point(x + 2, y), new Point(x + 3, y) }, player, gamefield);
                        if (MoveCheck.PointValid(rpx))
                            return rpx;


                        if (y > 2)
                        {
                            // Check diagonal upwards
                            Point rpdu = searchWinPoint(new List<Point>() { new Point(x, y), new Point(x + 1, y - 1), new Point(x + 2, y - 2), new Point(x + 3, y - 3) }, player, gamefield);
                            if (MoveCheck.PointValid(rpdu))
                                return rpdu;
                        }

                        if (y < 3)
                        {
                            // Check diagonal downwoards
                            Point rpdd = searchWinPoint(new List<Point>() { new Point(x, y), new Point(x + 1, y + 1), new Point(x + 2, y + 2), new Point(x + 3, y + 3) }, player, gamefield);
                            if (MoveCheck.PointValid(rpdd))
                                return rpdd;


                        }
                    }

                    if (y < 3)
                    {
                        // Check Y-Axis
                        Point rpy = searchWinPoint(new List<Point>() { new Point(x, y), new Point(x, y + 1), new Point(x, y + 2), new Point(x, y + 3) }, player, gamefield);
                        if (MoveCheck.PointValid(rpy))
                            return rpy;


                    }
                }
            }

            return new Point(-1, -1);
        }


        private static Point searchWinPoint(List<Point> points, int player, int[,] gamefield)
        {
            int count = 0;
            List<Point> setPoints = new List<Point>();
            foreach (Point point in points)
            {
                if (MoveCheck.IsSetByPlayer(point, player, gamefield))
                {
                    count++;
                    setPoints.Add(point);
                }
            }

            if (count != 3)
                return new Point(-1, -1);

            // get the points which is not set and return it if allowed
            foreach (Point point in points)
            {
                if (!setPoints.Contains(point))
                {
                    return MoveCheck.IsMoveAllowed(point, gamefield) ? point : new Point(-1, -1);
                }
            }

            return new Point(-1, -1);
        }
    }
}
