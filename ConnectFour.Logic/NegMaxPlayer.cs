using System;
using System.Drawing;
using ConnectFour.Logic.CatchMoves;

namespace ConnectFour.Logic
{
    public class NegMaxPlayer : IPlayer
    {
        private GameControl gameControl;
        private Random random;
        private int globalCurrentPlayerBuffer;
        private const int MAX_DEEP = 5;

        public NegMaxPlayer(GameControl gameControl)
        {
            this.gameControl = gameControl;
            random = new Random();
        }

        public void Turn()
        {
            Point[] possibleMoves = gameControl.GetPossibleMoves();

            if(!PlayerStrategies.CatchMovePlayed(gameControl))
            {
                Point move = new Point();
                globalCurrentPlayerBuffer = gameControl.CurrentPlayer;
                double alpha = double.MinValue;
                for (int i = 0; i < 7; i++)
                {
                    if (!MoveCheck.PointValid(possibleMoves[i])) continue;

                    Point pMove = new Point(possibleMoves[i].X, possibleMoves[i].Y);
                    // Prüfen, ob nach setzen dieses Steins diagonal eine Siegmöglichkeit für den Gegner entsteht
                    // -> Bad Move
                    double eval;
                    if (PlayerStrategies.CheckBadMove(gameControl, pMove))
                        eval = -1;
                    else
                    {
                        eval = scoreMove(pMove, 0);
                    }
                    
                    if (eval > alpha)
                    {
                        alpha = eval;
                        move = pMove;
                    }

                    gameControl.CurrentPlayer = globalCurrentPlayerBuffer;
                }

                gameControl.Move(move);
            }
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

            int catched = PlayerStrategies.CatchMoves(gameControl);
            if (catched != -2)
                return catched;


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
                if (MoveCheck.PointValid(possibleMoves[i]))
                {
                    alpha = Math.Max(alpha, -1.0*scoreMove(new Point(possibleMoves[i].X, possibleMoves[i].Y), deep + 1));
                }
            }

            gameControl.UnSet(move);
            gameControl.SwapPlayer();
            return alpha;
        }

    }
}
