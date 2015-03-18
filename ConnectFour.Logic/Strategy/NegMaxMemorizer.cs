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

        private StreamWriter writer1;
        private StreamWriter writer2;
        private string movePath1 = "moves1.txt";
        private string movePath2 = "moves2.txt";

        private List<string> moveStrings = new List<string>(); 

        public NegMaxMemorizer(int max_deep)
        {
            gameControl = new GameControl(this);
            MAX_DEEP = max_deep;
        }

        private StreamWriter initializeStreamWriter(string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            StreamWriter streamWriter = File.AppendText(path);

            return streamWriter;
        }

        public void ResetDatabase()
        {
            writer1 = initializeStreamWriter(movePath1);
            writer2 = initializeStreamWriter(movePath2);

            gameControl.CurrentPlayer = 1;
            gameControl.Set(3, 5);
            gameControl.CurrentPlayer = 2;
            gameControl.Set(3, 4);
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

                gameControl.CurrentPlayer = globalCurrentPlayerBuffer;
            }

            writer1.Close();
            writer2.Close();
        }

        private double scoreMove(Point pMove, int deep)
        {
            Point move = new Point(pMove.X, pMove.Y);
            CheckWinResult checkWinResult = gameControl.CheckWin();
            int winner = checkWinResult.Winner;
            bool win1 = winner == 1;
            bool win2 = winner == 2;


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
            string sGameField = winner + ";" + gameControl.GamefieldToString();

            switch (winner)
            {
                case 1: // Schreibe alle Siege von 1 in Datei move1
                    writer1.WriteLine(sGameField);
                    break;
                case 2: // Schreibe alle Siege von 2 in Datei move2
                    writer2.WriteLine(sGameField);
                    break;
                case 0: // Schreibe alle unentschiedenene Spiele in beide Dateien für eine schnellere Auswertung später
                    writer1.WriteLine(sGameField);
                    writer2.WriteLine(sGameField);
                    break;
            }
        }

        public void SetField(Point field, int currentPlayer)
        {
            // Hier darf der Code nicht vorbeikommen.
            throw new NotImplementedException();
        }

        public void Win(Point field, int currentPlayer, List<Point> points)
        {
            // Hier darf der Code nicht vorbeikommen.
            throw new NotImplementedException();
        }

        public void Draw(Point field, int currentPlayer)
        {
            // Hier darf der Code nicht vorbeikommen.
            throw new NotImplementedException();
        }
    }
}
