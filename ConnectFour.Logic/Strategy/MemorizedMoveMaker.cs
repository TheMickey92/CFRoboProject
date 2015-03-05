using System.Collections.Generic;
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
            if (!File.Exists("moves.txt")) return false;

            readWinAndDrawLines(gameControl.CurrentPlayer);

            string currentSituation = gameControl.GamefieldToString();

            calculateDifferences(currentSituation);

            // eliminate 0 values, da diese Variante ja derzeit gespielt ist 
            // und alle Varianten, für welche schon zu viele Züge gespielt wurden
            eliminateZeroes();
            removeTooShortOnes(numberOfMoves(currentSituation) + 1);

            // TODO

            return false;
        }

        private int getSmallestDifferenceCountIndex(List<int> list)
        {
            int min = 1000;
            foreach (int diff in list)
            {
                if (diff < 1000)
                    min = diff;
            }

            if (min == 1000)
                return -1;

            return min;
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

        private void removeTooShortOnes(int minLength)
        {
            // Draws
            for (int i = 0; i < draws.Count; i++)
            {
                string s = draws[i];
                if (numberOfMoves(s) >= minLength) continue;
                draws.RemoveAt(i);
                drawDifferences.RemoveAt(i);
                i--;
            }
            
            // Wins
            for (int i = 0; i < wins.Count; i++)
            {
                string s = wins[i];
                if (numberOfMoves(s) >= minLength) continue;
                wins.RemoveAt(i);
                winDifferences.RemoveAt(i);
                i--;
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

        private void readWinAndDrawLines(int player)
        {
            StreamReader reader = new StreamReader("moves.txt");
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                if (line[0] == '0')
                {
                    line = line.Remove(0, 2);
                    draws.Add(line);
                }

                if(line[0] == '1' && player == 1 || line[0] == '2' && player == 2)
                {
                    line = line.Remove(0, 2);
                    wins.Add(line);
                }
            }
            
            reader.Close();
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
    }
}
