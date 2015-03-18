using System.Collections.Generic;
using System.Drawing;

namespace ConnectFour.Logic
{
    public class CheckWinResult
    {
        public CheckWinResult(int winner, List<Point> points)
        {
            Points = points;
            Winner = winner;
        }

        public CheckWinResult(int winner)
        {
            Winner = winner;
            Points = new List<Point>();
        }

        public int Winner { get; private set; }
        public List<Point> Points { get; private set; } 
    }
}
