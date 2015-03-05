using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ConnectFour.Logic.Strategy
{
    public class NegMaxMemorizer : IOutput
    {
        private readonly GameControl gameControl;
        private Random random;
        private int globalCurrentPlayerBuffer;
        private int MAX_DEEP;

        private string movePath = "moves.txt";

        private List<string> moveStrings = new List<string>(); 

        public NegMaxMemorizer(int max_deep)
        {
            gameControl = new GameControl(this);
            MAX_DEEP = max_deep;
        }

        public void ResetDatabase()
        {
            if (File.Exists(movePath))
                File.Delete(movePath);

            gameControl.CurrentPlayer = 1;
            gameControl.Set(4, 5);
            gameControl.CurrentPlayer = 2;
            gameControl.Set(4, 4);
            gameControl.CurrentPlayer = 1;

            Point[] possibleMoves = gameControl.GetPossibleMoves();
            
            globalCurrentPlayerBuffer = gameControl.CurrentPlayer;
            
            for (int i = 0; i < 7; i++)
            {
                if (!MoveCheck.PointValid(possibleMoves[i])) continue;

                Point pMove = new Point(possibleMoves[i].X, possibleMoves[i].Y);
                // Prüfen, ob nach setzen dieses Steins diagonal eine Siegmöglichkeit für den Gegner entsteht
                // -> Bad Move
                if (!PlayerStrategies.CheckBadMove(gameControl, pMove))
                    scoreMove(pMove, 0);
                else
                {
                    gameControl.Set(pMove);
                    saveGameFieldString(2);
                    gameControl.UnSet(pMove);
                }

                // Save result

                gameControl.CurrentPlayer = globalCurrentPlayerBuffer;
            }

            writeGameFieldStrings();
        }

        private double scoreMove(Point pMove, int deep)
        {
            Point move = new Point(pMove.X, pMove.Y);
            int win = gameControl.CheckWin();
            bool win1 = win == 1;
            bool win2 = win == 2;


            if (gameControl.ValidMovesCount == 0 && !win1 && !win2) // DRAW!
            {
                saveGameFieldString(0);
                return 0;
            }

            if (globalCurrentPlayerBuffer == 1 && win1 || globalCurrentPlayerBuffer == 2 && win2)
            {
                saveGameFieldString(globalCurrentPlayerBuffer);
                return 1;
            }

            if (globalCurrentPlayerBuffer == 1 && win2 || globalCurrentPlayerBuffer == 2 && win1)
            {
                saveGameFieldString(globalCurrentPlayerBuffer == 1 ? 2 : 1);
                return -1;
            }

            int catched = PlayerStrategies.CatchMoves(gameControl);
            if (catched != -2)
            {
                switch (catched)
                {
                    case 0:
                        saveGameFieldString(0);
                        break;
                    case -1:
                        saveGameFieldString(globalCurrentPlayerBuffer == 1 ? 2 : 1);
                        break;
                    case 1:
                        saveGameFieldString(globalCurrentPlayerBuffer);
                        break;
                }

                return catched;
            }


            if (deep == MAX_DEEP) // Abbruch, um zeitnah zu bleiben!
            {
                return 0.5; // Hierdurch wird das Speichern später verhindert. Später zählt hier nur -1, 1 und 0
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
                    alpha = Math.Max(alpha, -1.0 * scoreMove(new Point(possibleMoves[i].X, possibleMoves[i].Y), deep + 1));
                }
            }

            gameControl.UnSet(move);
            gameControl.SwapPlayer();
            return alpha;
        }

        private void saveGameFieldString(int winner)
        {
            string sGameField = winner + ";";

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    sGameField += gameControl.Get(x, y);
                }
            }

            moveStrings.Add(sGameField);
        }

        private void writeGameFieldStrings()
        {
            StreamWriter writer = File.AppendText(movePath);
            foreach (string s in moveStrings)
            {
                writer.WriteLine(s);
            }
            writer.Close();
        }

        public void SetField(Point field, int currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void Win(Point field, int currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void Draw(Point field, int currentPlayer)
        {
            throw new NotImplementedException();
        }
    }
}
