using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ConnectFour.Logic.Strategy
{
    public class MemorizedMoveMaker
    {
        private List<string> draws = new List<string>();
        private  List<int> drawDifferences = new List<int>(); 
        private List<string> wins = new List<string>(); 
        private List<int> winDifferences = new List<int>(); 

        public bool MemorizedMovePlayed(GameControl gameControl)
        {
            int currentPlayer = gameControl.CurrentPlayer;

            if (!File.Exists("moves"+ currentPlayer + ".txt")) return false;

            string currentSituation = gameControl.GamefieldToString();
            
            readWinAndDrawLines(currentPlayer, currentSituation);
            
            calculateDifferences(currentSituation);

            // eliminate 0 values, da diese Variante ja derzeit gespielt ist
            eliminateZeroes();

            // Jetzt sollten nur noch die Spielzüge in der Liste sein, die überhaupt in Frage kommen


            while (wins.Count > 0) // probiere alle Züge durch
            {
                if (tryToPlayMove(gameControl, currentSituation, wins, winDifferences))
                {
                    return true;
                }
            }

            while (draws.Count > 0) // wenn kein Sieg möglich, ist vielleicht ein Unentschieden rauszuholen
            {
                if (tryToPlayMove(gameControl, currentSituation, draws, drawDifferences))
                {
                    return true;
                }
            }

            return false;
        }

        private bool tryToPlayMove(GameControl gameControl, string currentSituation, List<string> sList, List<int> differences)
        {
            int index = getSmallestIndex(differences);
            List<Point> neededMoves = getNeededMoves(currentSituation, sList[index], gameControl.CurrentPlayer);
            foreach (Point move in neededMoves)
            {
                if (MoveCheck.IsMoveAllowed(move, gameControl.GetGamefield()))
                {
                    gameControl.Move(move);
                    return true;
                }
            }

            sList.RemoveAt(index);
            differences.RemoveAt(index);

            return false;
        }

        private void calculateDifferences(string currentSituation)
        {
            foreach (string draw in draws)
            {
                drawDifferences.Add(difference(currentSituation, draw));
            }

            foreach (string win in wins)
            {
                winDifferences.Add(difference(currentSituation, win));
            }


        }

        private void eliminateZeroes()
        {
            // Draws
            for (int i = 0; i < drawDifferences.Count; i++)
            {
                int drawDifference = drawDifferences[i];
                if (drawDifference != 0) continue;
                drawDifferences.RemoveAt(i);
                draws.RemoveAt(i);
                i--;
            }

            // Wins
            for (int i = 0; i < winDifferences.Count; i++)
            {
                int winDifference = winDifferences[i];
                if (winDifference != 0) continue;
                winDifferences.RemoveAt(i);
                wins.RemoveAt(i);
                i--;
            }
        }

        private void readWinAndDrawLines(int player, string currentSituation)
        {
            StreamReader reader = new StreamReader("moves" + player + ".txt");
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                if (line[0] == '0')
                {
                    addLineToList(draws, currentSituation, line);
                }

                if(line[0] == '1' && player == 1 || line[0] == '2' && player == 2)
                {
                    addLineToList(wins, currentSituation, line);
                }
            }
            
            reader.Close();
        }

        private void addLineToList(List<string> list, string currentSituation, string line)
        {
            line = line.Remove(0, 2);
            bool correctNumber = numberOfMoves(line) >= numberOfMoves(currentSituation) + 1;
            bool possibleMove = possible(currentSituation, line);
            if (correctNumber && possibleMove) // Bestimmte Anzahl von Spielzügen muss bereits vorhanden sein, außerdem müssen bisherige Züge vorhanden sein
                list.Add(line);
        }

        private int difference(string s1, string s2)
        {
            if (s1.Length != s2.Length) return -1;

            return s1.Where((t, i) => t != s2[i]).Count();
        }

        private int numberOfMoves(string moves)
        {
            return moves.Count(t => t != '0');
        }

        private bool possible(string currentSituation, string s)
        {
            if (currentSituation.Length != s.Length) return false;

            for (int i = 0; i < s.Length; i++)
            {
                if (currentSituation[i] == '1' && s[i] != '1')
                    return false;
                if (currentSituation[i] == '2' && s[i] != '2')
                    return false;
            }

            return true;
        }

        private List<Point> getNeededMoves(string currentSituation, string line, int currentPlayer)
        {
            char player = currentPlayer == 1 ? '1' : '2';
            List<Point> moves = new List<Point>();
            for (int i = 0; i < currentSituation.Length; i++)
            {
                if (currentSituation[i] == '0' && line[i] == player)
                {
                    Point move = new Point(i%7, i/7); // Aus Stringkette die Position zurückrechnen
                    moves.Add(move);
                }
            }

            return moves;
        }

        private int getSmallestIndex(List<int> list)
        {
            int min = 1000;
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                    index = i;
                }
            }

            return index;
        }
    }
}
