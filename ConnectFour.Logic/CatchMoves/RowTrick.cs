using System.Collections.Generic;
using System.Drawing;

namespace ConnectFour.Logic.CatchMoves
{
    static class RowTrick
    {
        public static Point UseRowTrick(int currentPlayer, int[,] gamefield)
        {
            return calcRowTrick(gamefield, currentPlayer);
        }

        public static Point CatchRowTrick(int currentPlayer, int[,] gamefield)
        {
            int lastPlayer = currentPlayer == 1 ? 2 : 1;
            return calcRowTrick(gamefield, lastPlayer);
        }

        private static Point calcRowTrick(int[,] gamefield, int player)
        {
            List<Point[]> pairs = new List<Point[]>();

            // search for two in a row of this player
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    int pos1 = gamefield[x, y];
                    int pos2 = gamefield[x + 1, y]; // neighbor of pos1
                    if (pos1 == player && pos2 == player)
                        pairs.Add(new[] {new Point(x, y), new Point(x + 1, y)});
                }
            }

            // now take a closer look if any of these pairs could be used for a row trick
            foreach (Point[] pair in pairs)
            {
                if (!MoveCheck.IsMoveAllowed(pair[0].X - 1, pair[0].Y, gamefield))
                    continue;

                if (!MoveCheck.IsMoveAllowed(pair[1].X + 1, pair[0].Y, gamefield))
                    continue;

                if (MoveCheck.IsMoveAllowed(pair[0].X - 2, pair[0].Y, gamefield))
                    return new Point(pair[0].X - 1, pair[0].Y);

                if (MoveCheck.IsMoveAllowed(pair[1].X + 2, pair[1].Y, gamefield))
                    return new Point(pair[1].X + 1, pair[1].Y);
            }

            return new Point(-1, -1);
        }
    }
}
