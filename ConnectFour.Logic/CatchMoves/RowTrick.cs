using System.Collections.Generic;
using System.Drawing;

namespace ConnectFour.Logic.CatchMoves
{
    static class RowTrick
    {
        public static Point CatchRowTrick(int currentPlayer, int[,] gamefield)
        {
            int lastPlayer = currentPlayer == 1 ? 2 : 1;
            List<Point[]> pairs = new List<Point[]>();

            // search for two in a row of this player
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    int pos1 = gamefield[x,y];
                    int pos2 = gamefield[x + 1, y]; // neighbor of pos1
                    if(pos1 == lastPlayer && pos2 == lastPlayer)
                        pairs.Add(new[] { new Point(x, y), new Point(x + 1, y)});
                }
            }
            
            // now take a closer look if any of these pairs could be used for a row trick
            foreach (Point[] pair in pairs)
            {
                if (!isMoveAllowed(pair[0].X - 1, pair[0].Y, gamefield))
                    continue;

                if (!isMoveAllowed(pair[1].X + 1, pair[0].Y, gamefield))
                    continue;

                if(isMoveAllowed(pair[0].X - 2, pair[0].Y, gamefield))
                    return new Point(pair[0].X - 1, pair[0].Y);

                if(isMoveAllowed(pair[1].X + 2, pair[1].Y, gamefield))
                    return new Point(pair[1].X + 1, pair[1].Y);
            }

            return new Point(-1, -1);
        }

        private static bool isMoveAllowed(int x, int y, int[,] gamefield)
        {
            return isMoveAllowed(new Point(x, y), gamefield);
        }

        private static bool isMoveAllowed(Point p, int[,] gamefield)
        {
            return p.Y >= 0 && p.X >= 0 && p.X < 7 && gamefield[p.X, p.Y] == 0 && (p.Y == 5 || gamefield[p.X, p.Y + 1] != 0);
        }
    }
}
