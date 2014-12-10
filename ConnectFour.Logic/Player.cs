using System;
using System.Drawing;

namespace ConnectFour.Logic
{
    public class Player : IPlayer
    {
        private GameControl gameControl;
        private Random random;
        private int globalCurrentPlayerBuffer;
        private const int MAX_DEEP = 5;

        public Player(GameControl gameControl)
        {
            this.gameControl = gameControl;
            random = new Random();
        }

        public void Turn()
        {
            Point[] possibleMoves = gameControl.GetPossibleMoves();

            Point winPointOpponent = gameControl.GetWinPoint(gameControl.CurrentPlayer == 1 ? 2 : 1);
            Point winPointCurrentPlayer = gameControl.GetWinPoint(gameControl.CurrentPlayer);

            if (winPointCurrentPlayer.X != -1 && winPointCurrentPlayer.Y != -1)
            {
                gameControl.Move(winPointCurrentPlayer);
            }
            else if (winPointOpponent.X != -1 && winPointOpponent.Y != -1)
            {
                gameControl.Move(winPointOpponent);
            }
            else
            {
                Point move = new Point();
                globalCurrentPlayerBuffer = gameControl.CurrentPlayer;
                double alpha = double.MinValue;
                for (int i = 0; i < 7; i++)
                {
                    if (possibleMoves[i].X == -1) continue;

                    Point pMove = new Point(possibleMoves[i].X, possibleMoves[i].Y);
                    // Prüfen, ob nach setzen dieses Steins diagonal eine Siegmöglichkeit für den Gegner entsteht
                    // -> Bad Move
                    double eval;

                    if (checkBadMove(pMove))
                        eval = -1;
                    else
                        eval = scoreMove(pMove, 0);
                    

                    if (eval > alpha)
                    {
                        alpha = eval;
                        move = pMove;
                    }

                    gameControl.CurrentPlayer = globalCurrentPlayerBuffer;

                    if (Math.Abs(alpha - 1) < 0.00001)
                        break;
                }

                gameControl.Move(move);
            }
        }

        private bool checkBadMove(Point move)
        {
            bool bad = false;
            gameControl.Set(move);
            Point opponentWinPoint = gameControl.GetWinPoint(gameControl.CurrentPlayer == 1 ? 2 : 1);
            if (opponentWinPoint.X == move.X && opponentWinPoint.Y == move.Y - 1)
                bad = true;

            gameControl.UnSet(move);

            return bad;
        }

        private double scoreMove(Point pMove, int deep)
        {
            Point move = new Point(pMove.X, pMove.Y);
            int win = gameControl.CheckWin();
            bool win1 = win == 1;
            bool win2 = win == 2;


            if (gameControl.ValidMovesCount == 0 && !win1 && ! win2) // DRAW!
                return 0;

            if (globalCurrentPlayerBuffer == 1 && win1 || globalCurrentPlayerBuffer == 2 && win2)
                return 1;

            if (globalCurrentPlayerBuffer == 1 && win2 || globalCurrentPlayerBuffer == 2 && win1)
                return -1;
            

            if (deep == MAX_DEEP) // Abbruch, um zeitnah zu bleiben!
            {
                return random.Next(-99, 100)/100.0; // TODO hier wird zufällig gewählt, wenn MAX_DEEP erreicht wurde
            }

            // Führe Zug durch und teste
            gameControl.Set(move);
            gameControl.SwapPlayer();

            Point[] possibleMoves = gameControl.GetPossibleMoves();
            double alpha = double.MinValue;
            for (int i = 0; i < 7; i++)
            {
                if (possibleMoves[i].X != -1)
                    alpha = Math.Max(alpha, -1.0 * scoreMove(new Point(possibleMoves[i].X, possibleMoves[i].Y), deep + 1));
            }

            gameControl.UnSet(move);
            //gameControl.CurrentPlayer = currentPlayerBuffer;
            gameControl.SwapPlayer();
            return alpha;
        }
    }
}
