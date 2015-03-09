using System.Drawing;
using ConnectFour.Logic.CatchMoves;

namespace ConnectFour.Logic.Strategy
{
    public static class PlayerStrategies
    {
        public static bool CatchMovePlayed(GameControl gameControl)
        {
            Point winPointOpponent = gameControl.GetWinPoint(gameControl.CurrentPlayer == 1 ? 2 : 1);
            Point winPointCurrentPlayer = gameControl.GetWinPoint(gameControl.CurrentPlayer);
            Point catchRowTrick = gameControl.CatchRowTrick();
            Point useRowtrick = gameControl.UseRowTrick();

            if (MoveCheck.PointValid(winPointCurrentPlayer))
            {
                gameControl.Move(winPointCurrentPlayer);
            }
            else if (MoveCheck.PointValid(winPointOpponent))
            {
                gameControl.Move(winPointOpponent);
            }
            else if (MoveCheck.PointValid(catchRowTrick))
            {
                gameControl.Move(catchRowTrick);
            }
            else if (MoveCheck.PointValid(useRowtrick))
            {
                gameControl.Move(useRowtrick);
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool CheckBadMove(GameControl gameControl, Point move)
        {
            bool bad = false;
            gameControl.Set(move);
            Point opponentWinPoint = gameControl.GetWinPoint(gameControl.CurrentPlayer == 1 ? 2 : 1);
            Point opponentRowTrick = gameControl.CatchRowTrick();
            if (opponentWinPoint.X != -1 || opponentRowTrick.X != -1)
                bad = true;

            gameControl.UnSet(move);

            return bad;
        }

        public static int CatchMoves(GameControl gameControl)
        {
            // Prüfen, ob Row Trick möglich ist. Wenn ja, dann ist Sieg schon sicher
            Point rowTrick = RowTrick.UseRowTrick(gameControl.CurrentPlayer, gameControl.GetGamefield());
            if (MoveCheck.PointValid(rowTrick))
                return 1;

            Point winPoint = WinPoint.GetWinPoint(gameControl.CurrentPlayer, gameControl.GetGamefield());
            Point winPointOpponent = WinPoint.GetWinPoint(gameControl.CurrentPlayer == 1 ? 2 : 1, gameControl.GetGamefield());

            // Wenn aktueller Spieler 3 in einer Reihe hat, der Gegner aber nicht, dann ist Sieg ebenfalls bereits sicher
            if (MoveCheck.PointValid(winPoint) && !MoveCheck.PointValid(winPointOpponent))
                return 1;

            // Wenn Gegner 3 in einer Reihe, aktueller Spieler aber nicht, dann schon verloren
            if (!MoveCheck.PointValid(winPoint) && MoveCheck.PointValid(winPointOpponent))
                return -1;

            return -2;
        }

        public static bool FirstMove(Point[] possibleMoves)
        {
            bool first = true;
            foreach (Point point in possibleMoves)
            {
                if (point.Y != 5)
                    first = false;
            }

            return first;
        }

        public static bool SecondMove(Point[] possibleMoves, ref Point move)
        {
            int count = 0;
            foreach (Point point in possibleMoves)
            {
                if (point.Y != 5)
                {
                    count++;
                    move = new Point(point.X, point.Y);

                    if (point.Y < 4)
                    {
                        count++;
                    }
                }
            }

            return count == 1;
        }

        public static bool PlayFixedFirstOrSecondMove(GameControl gameControl)
        {
            Point[] possibleMoves = gameControl.GetPossibleMoves();

            // Wenn erster Zug, dann direkt in der Mitte spielen
            if (FirstMove(possibleMoves))
            {
                gameControl.Move(3, 5);
                return true;
            }

            // Wenn zweiter Zug, dann direkt auf den bereits gespielten
            Point possibleSecondMove = new Point();
            if (SecondMove(possibleMoves, ref possibleSecondMove))
            {
                gameControl.Move(possibleSecondMove);
                return true;
            }

            return false;
        }
    }
}
