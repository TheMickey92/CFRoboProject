using System;
using System.Drawing;
using System.Linq;

namespace ConnectFour.Logic.Strategy
{
    public class NegMaxPlayer : IPlayer
    {
        private GameControl gameControl;
        private Random random;
        private int globalCurrentPlayerBuffer;
        private const int MAX_DEEP = 4;

        public NegMaxPlayer(GameControl gameControl)
        {
            this.gameControl = gameControl;
            random = new Random();
        }
        
        public void Turn()
        {
            if (PlayerStrategies.PlayFixedFirstOrSecondMove(gameControl)) 
                return;

            Point[] possibleMoves = gameControl.GetPossibleMoves();

            if(!PlayerStrategies.CatchMovePlayed(gameControl))
            {
                Point move = new Point();
                globalCurrentPlayerBuffer = gameControl.GetCurrentPlayer();

                //MemorizedMoveMaker memorizedMoveMaker = new MemorizedMoveMaker();
                //if (memorizedMoveMaker.MemorizedMovePlayed(gameControl)) 
                //    return;

                // wenn nur noch eine Möglichkeit übrig ist, dann diese spielen
                if (oneMoveLeft(possibleMoves))
                {
                    Point lastPossibleMove = new Point(-1, -1);
                    foreach (Point possibleMove in possibleMoves.Where(MoveCheck.PointValid))
                    {
                        lastPossibleMove = possibleMove;
                    }

                    gameControl.Move(lastPossibleMove);
                    return;
                }

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

                    gameControl.SetCurrentPlayer(globalCurrentPlayerBuffer);
                }

                gameControl.Move(move);
            }
        }

        private double scoreMove(Point pMove, int deep)
        {
            Point move = new Point(pMove.X, pMove.Y);
            CheckWinResult checkWinResult = gameControl.CheckWin();
            int winner = checkWinResult.Winner;
            bool win1 = winner == 1;
            bool win2 = winner == 2;

            
            if (gameControl.GetValidMovesCount() == 0 && !win1 && ! win2) // DRAW!
                return 0;

            if (gameControl.GetCurrentPlayer() == 1 && win1 || gameControl.GetCurrentPlayer() == 2 && win2)
                return 1;

            if (gameControl.GetCurrentPlayer() == 1 && win2 || gameControl.GetCurrentPlayer() == 2 && win1)
                return -1;

            int catched = PlayerStrategies.CatchMoves(gameControl);
            if (catched != -2)
                return catched;


            if (deep == MAX_DEEP) // Abbruch, um zeitnah zu bleiben!
            {
                //return random.Next(-99, 100)/100.0; // TODO hier wird zufällig gewählt, wenn MAX_DEEP erreicht wurde
                // Evaluate
                double eval = MoveEvaluation.evaluateContent(gameControl.GetGamefield(), gameControl.GetCurrentPlayer());
                return eval;
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

        private bool oneMoveLeft(Point[] possibleMoves)
        {
            int count = 0;
            foreach (Point possibleMove in possibleMoves)
            {
                if (!MoveCheck.PointValid(possibleMove))
                    count++;
            }

            return count == 6;
        }
    }
}
